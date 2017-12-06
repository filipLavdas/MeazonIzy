using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MeazonIzy.Localization;
using MeazonIzy.Models;
using MeazonIzy.Resources;
using Newtonsoft.Json;
using Websockets;
using Xamarin.Forms;

namespace MeazonIzy.Views
{
    public partial class SetSchedulePage : TabbedPage
    {
        private double StepValue;
        private bool IsOnPreviousState;
        private ScheduleHelperClass Helper;
        private MySchedule myschedule;
        private int _index;
        private Plug plug;
        private IWebSocketConnection connection;
        private string Mtoken;


        protected async override void OnAppearing()
        {
            CancelButton.Image = Device.OnPlatform(iOS: ImageSource.FromFile("Resources/Cancel.png"),
                Android: ImageSource.FromFile("Resources/drawable/Cancel.png")
                , WinPhone: ImageSource.FromFile("Assets/Cancel.png")) as FileImageSource;


            SaveButton.Image = Device.OnPlatform(iOS: ImageSource.FromFile("Resources/Check.png"),
                Android: ImageSource.FromFile("Resources/drawable/Check.png")
                , WinPhone: ImageSource.FromFile("Assets/Check.png")) as FileImageSource;
            await GetMToken();
           
            connection.Open("wss://webcontrol.meazon.com");
            connection.OnOpened += ConnectionOnOnOpened;
           

        }

        private async Task GetMToken()
        {
            var mtokenurl = "https://izy.meazon.com/api/energylog/mToken";
            var client = new HttpClient();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", App.USERTOKEN);
            var json = await client.GetStringAsync(mtokenurl);
            var token = JsonConvert.DeserializeObject<string>(json);
            Mtoken = token.Replace("\"", "");


        }





        private void ConnectionOnOnOpened()
        {
           
            string authmsg = "Identity {\"MToken\":\"" + Mtoken + "\"" + "}";
            connection.Send(authmsg);
            string msg = "SubscribeToMyMacs";
            connection.Send(msg);
        }



        public SetSchedulePage(Plug p)
        {
            InitializeComponent();
            Helper = new ScheduleHelperClass();
            StepValue = 1.0;
            IsOnPreviousState = false;
            InitializeValues(0.0, 1.0);
            plug=new Plug();
            connection = Websockets.WebSocketFactory.Create();
            plug = p;
            NavigationPage.SetHasBackButton(CurrentPage, false);
        }


       

        public SetSchedulePage(Plug p, MySchedule s)
        {
            InitializeComponent();
            myschedule = new MySchedule();
            myschedule = s;
            _index = p.MyPlugState.MyScheduleList.IndexOf(s);
            connection = Websockets.WebSocketFactory.Create();
            //Exclude schedule from List 
            p.MyPlugState.MyScheduleList.Remove(s);
            plug=new Plug();
            
            plug = p;
            Helper = new ScheduleHelperClass();
            StepValue = 1.0;
            IsOnPreviousState = true;

            
            SetValuesFromPreviousState(s);
        }


        private void SetValuesFromPreviousState(MySchedule s)
        {
            List<string> days = s.Days.Split(' ').ToList();

            foreach (var v in days)
            {
                if (string.Equals(v,L10n.Localize("MonLabel")))
                {
                    MonSwitch.IsToggled = true;
                }
                else if (string.Equals(v, L10n.Localize("TueLabel")))
                {
                    TueSwitch.IsToggled = true;
                }
                else if (string.Equals(v, L10n.Localize("WedLabel")))
                {
                    WedSwitch.IsToggled = true;
                }
                else if (string.Equals(v, L10n.Localize("ThuLabel")))
                {
                    ThuSwitch.IsToggled = true;
                }
                else if (string.Equals(v, L10n.Localize("FriLabel")))
                {
                    FriSwitch.IsToggled = true;
                }
                else if (string.Equals(v, L10n.Localize("SatLabel")))
                {
                    SatSwitch.IsToggled = true;
                }
                else if (string.Equals(v, L10n.Localize("SunLabel")))
                {
                    SunSwitch.IsToggled = true;
                }
            }



            var fromt = Helper.StringToQt(s.FromTime);
            var tot = Helper.StringToQt(s.ToTime);

            InitializeValues(fromt, tot);

        }


        private void InitializeValues(double v1, double v2)
        {
            FromSlider.Value = v1;
            FromTimeLabel.Text = Helper.QtToTime(FromSlider.Value);
            ToSlider.Value = v2;
            ToTimeLabel.Text = Helper.QtToTime(v2);
        }

        private void AllDaySwitch_OnToggled(object sender, ToggledEventArgs e)
        {
           
            if (AllDaySwitch.IsToggled)
            {
                AllDayLabel.FontSize = 30;
                AllDayLabel.FontAttributes = FontAttributes.Bold;
                InitializeValues(0.0, 96.0);
                TimeStack.IsVisible = false;
            }
            else
            {
                AllDayLabel.FontSize = 22;
                AllDayLabel.FontAttributes = FontAttributes.None;
                InitializeValues(0.0, 1.0);
                TimeStack.IsVisible = true;
            }
        }

        private async void FromSlider_OnValueChanged(object sender, ValueChangedEventArgs e)
        {
            var newStep = Math.Round(e.NewValue / StepValue);
            FromSlider.Value = newStep * StepValue;
            FromTimeLabel.Text = Helper.QtToTime(FromSlider.Value);
            await Task.Delay(200);
            if (!IsOnPreviousState)
            {
                ToSlider.Value = (FromSlider.Value + StepValue);
            }
        }

        private async void ToSlider_OnValueChanged(object sender, ValueChangedEventArgs e)
        {
            var newStep = Math.Round(e.NewValue / StepValue);
            ToSlider.Value = newStep * StepValue;
            ToTimeLabel.Text = Helper.QtToTime(ToSlider.Value);
            await CheckTimeValidation();

            if (ToSlider.Value == 96.0)
            {

                if (FromSlider.Value == 0.0)
                {
                    AllDaySwitch.IsToggled = true;
                }
            }
        }

        private async Task<bool> CheckTimeValidation()
        {
            if (ToSlider.Value <= FromSlider.Value)
            {
                TimeValidationLabel.IsVisible = true;
                return false;
            }

            TimeValidationLabel.IsVisible = false;
            return true;

        }

        private async Task<bool> CheckValidation()
        {
            var time = await CheckTimeValidation();
            if (!time)
            {
                await DisplayAlert("Wrong Time Set", "From time Value must be lower than To Time Value", L10n.Localize("OkAction"));

            }
            var day = MonSwitch.IsToggled || TueSwitch.IsToggled || WedSwitch.IsToggled || ThuSwitch.IsToggled ||
                      FriSwitch.IsToggled || SatSwitch.IsToggled || SunSwitch.IsToggled;
            if (!day)
            {
                await DisplayAlert("Wrong Day Schedule", "You must set at leasy one Day Schedule", L10n.Localize("OkAction"));
            }
            return time & day;


        }

        private string SerializeScheduleLocally()
        {
            bool[] quarters = new bool[96 * 7];
            var fromQt = (int)FromSlider.Value;
            var toQt = (int)ToSlider.Value;
            var daysChecked = new bool[7];

            //Delete previous scheule from server
            if (IsOnPreviousState)
            {
                var schlist = plug.MyPlugState.MyScheduleList;
                schlist.RemoveAt(_index);
                var b64string = Helper.SerializeSchedule(schlist);
                string setSchedule = "SetSchedule {\"MAC\":" +
                                         plug.Mac + ",\"Number\":0" +
                                         ",\"Schedule\":{\"Events\":\"" + b64string + "\"" +
                                         ",\"Scope\":0,\"Resolution\":2}}";
                connection.Send(setSchedule);
            }
            

            //set new Schedule
            var events = Helper.SerializeSchedule(plug.MyPlugState.MyScheduleList);
            quarters = Helper.StringToQuartersDesirialize(events);



            daysChecked[0] = SatSwitch.IsToggled;
            daysChecked[1] = SunSwitch.IsToggled;
            daysChecked[2] = MonSwitch.IsToggled;
            daysChecked[3] = TueSwitch.IsToggled;
            daysChecked[4] = WedSwitch.IsToggled;
            daysChecked[5] = ThuSwitch.IsToggled;
            daysChecked[6] = FriSwitch.IsToggled;



            if (toQt <= 96)
            {
                for (var d = 0; d < daysChecked.Length; d++)
                {
                    if (daysChecked[d])
                    {

                        for (var k = fromQt; k < toQt; k++)
                        {
                            quarters[k + (d * 96)] = true;

                        }


                    }

                }
            }




            var bits = new BitArray(quarters);

            var bytes = new List<byte>();

            var currentbyte = 0x00;
            for (int i = 0; i < bits.Length;)
            {
                currentbyte = 0x00;
                for (int j = 0; j < 8 && i < bits.Length; j++, i++)
                {
                    if (bits[i])
                    {
                        currentbyte |= (0x80 >> (i % 8));
                    }


                }
                bytes.Add((byte)currentbyte);
            }









            var b64String = Convert.ToBase64String(bytes.ToArray());
            return b64String;
        }

        private async void Save_OnClicked(object sender, EventArgs e)
        {
            var valid = await CheckValidation();

            if (valid)
            {

                string setSchedule = "SetSchedule {\"MAC\":" +
                                     plug.Mac + ",\"Number\":0" +
                                     ",\"Schedule\":{\"Events\":\"" + SerializeScheduleLocally()+"\"" +
                                     ",\"Scope\":0,\"Resolution\":2}}";
                connection.Send(setSchedule);

                var page = new SchedulePage(plug);
                page.BindingContext = page;
                var homepage = new HomePage();
                homepage.Detail = page;
                homepage.IsPresented = false;
                Navigation.InsertPageBefore(homepage, this);
                await Navigation.PopAsync().ConfigureAwait(false);
            }



        }

        protected override void OnDisappearing()
        {
            connection.Close();
        }

        private async void Cancel_OnClicked(object sender, EventArgs e)
        {

            //var homepage = new HomePage();
            //homepage.Detail = page;
            //homepage.IsPresented = false;
            var page = new SchedulePage(plug);
            page.BindingContext = plug;
            await Navigation.PushAsync(page);
            Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count-2]);
            //Navigation.InsertPageBefore(page, this);
            //await Navigation.PopAsync().ConfigureAwait(false);


        }

        protected  override bool OnBackButtonPressed()
        {

            
           
            Task.Factory.StartNew(async () =>
            {
               
                var page = new SchedulePage(plug);
                page.BindingContext = plug;
                await Navigation.PushAsync(page);
                Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
            });
            
            

            return true;
        }

        private void AllWeekSwitch_OnToggled(object sender, ToggledEventArgs e)
        {
            if (AllWeekSwitch.IsToggled)
            {
                AllWeekLabel.FontSize = 30;
                AllWeekLabel.FontAttributes = FontAttributes.Bold;
                SetAllWeek(true);
                //WeekLayout.IsVisible = false;
            }
            else
            {
                AllWeekLabel.FontSize = 25;
                AllWeekLabel.FontAttributes = FontAttributes.None;
                SetAllWeek(false);
                //WeekLayout.IsVisible = true;
            }
        }

        private void SetAllWeek(bool week)
        {
            MonSwitch.IsToggled = week;
            TueSwitch.IsToggled = week;
            WedSwitch.IsToggled = week;
            ThuSwitch.IsToggled = week;
            FriSwitch.IsToggled = week;
            SatSwitch.IsToggled = week;
            SunSwitch.IsToggled = week;
        }
    }
}
