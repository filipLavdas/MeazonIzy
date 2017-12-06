using System;
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
    public partial class SchedulePage : ContentPage
    {
        private ScheduleHelperClass _helper;
        private Plug  p;
        private IWebSocketConnection connection;
        private string Mtoken;

        protected override async void OnAppearing()
        {
          await  GetMToken();
            

            connection.Open("wss://webcontrol.meazon.com");
            connection.OnOpened += ConnectionOnOnOpened;
            connection.OnMessage += ConnectionOnOnMessage;
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

        public SchedulePage(Plug plug)
        {
            InitializeComponent();
            Title = L10n.Localize("SchedulesTitle");
            InitializePlatformSpecific();

            Indicator.IsRunning = true;
            Indicator.IsVisible = true;
            MainStack.IsVisible = false;
            connection = Websockets.WebSocketFactory.Create();
            p =new Plug();
            _helper=new ScheduleHelperClass();
           
            p.Mac = plug.Mac;
            p.Name = plug.Name;
            _helper = new ScheduleHelperClass();
            

            


           

        }

        private void InitializePlatformSpecific()
        {
            if (Device.OS == TargetPlatform.WinPhone)
            {

                TheToolbarAdd.Icon = "add.png";
                Indicator.Scale = 2;


            }
            else if (Device.OS == TargetPlatform.Windows)
            {
                Indicator.Scale = 3;
                TheToolbarAdd.Icon = "Assets/add.png";

            }
            else if (Device.OS == TargetPlatform.Android)
            {
               
                TheToolbarAdd.Icon = "Resources/drawable/add.png";

            }
        }

        private void ConnectionOnOnMessage(string msg)
        {
            if (msg.StartsWith("PlugState"))
            {
                msg = msg.Replace("PlugState", "");
                MyPlugState state = JsonConvert.DeserializeObject<MyPlugState>(msg);
                if (string.Equals(p.Mac, state.Mac))
                {
                    p.MyPlugState = state;
                    p.MyPlugState.MyScheduleList = _helper.DisirializeSchedule(p.MyPlugState.Schedule);
                    Device.BeginInvokeOnMainThread(() =>
                    {

                        SchedulesListView.ItemsSource = p.MyPlugState.MyScheduleList;
                        Indicator.IsRunning = false;
                        Indicator.IsVisible = false;
                        MainStack.IsVisible = true;
                    });

                    
                }
            }
        }


        private void ConnectionOnOnOpened()
        {
            //var token = App.MTOKEN.Replace("\"", "");
            string authmsg = "Identity {\"MToken\":\"" + Mtoken + "\"" + "}";
            connection.Send(authmsg);
            string msg = "SubscribeToMyMacs";
            connection.Send(msg);
        }

        private async void TheToolbarAdd_OnClicked(object sender, EventArgs e)
        {
            //Navigation.InsertPageBefore(new SetSchedulePage(p), this);
            //await Navigation.PopAsync().ConfigureAwait(false);

            await Navigation.PushAsync(new SetSchedulePage(p));
            var thispage = Navigation.NavigationStack[Navigation.NavigationStack.Count - 2];
            Navigation.RemovePage(thispage);
        }

        private void Delete_OnClicked(object sender)
        {
            MainStack.IsVisible = false;
            Indicator.IsRunning = true;
            Indicator.IsVisible = true;

            var mysched = sender as MySchedule;
            var colection = SchedulesListView.ItemsSource as ObservableCollection<MySchedule>;
            colection.Remove(mysched);
            var b64string = _helper.SerializeSchedule(colection);
            string setSchedule = "SetSchedule {\"MAC\":" +
                                     p.Mac + ",\"Number\":0" +
                                     ",\"Schedule\":{\"Events\":\"" + b64string + "\"" +
                                     ",\"Scope\":0,\"Resolution\":2}}";
            connection.Send(setSchedule);

        }

        private async void Edit_OnClicked(object sender)
        {
            var obj = sender as MySchedule;

            await Navigation.PushAsync(new SetSchedulePage(p, obj));

            var thispage = Navigation.NavigationStack[Navigation.NavigationStack.Count - 2];
            Navigation.RemovePage(thispage);
        }

        protected override bool OnBackButtonPressed()
        {
            
            //var homepage = new HomePage
            //{
            //    Detail = new SchedulesPage(),
            //    IsPresented = true
            //};
           
            ////Navigation.InsertPageBefore(homepage, Navigation.NavigationStack[0]);
            ////Navigation.RemovePage(Navigation.NavigationStack[1]);
            //Navigation.PushAsync(homepage);
            //Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count-2]);
            ////Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 3]);
            Navigation.PopAsync();
            return true;
        }

    

        private async void SchedulesListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var action = await DisplayActionSheet(L10n.Localize("OptionsLabel"), L10n.Localize("CancelLabel"), "", L10n.Localize("DeleteLabel"),
                L10n.Localize("EditLabel"));
            if (string.Equals(L10n.Localize("EditLabel"), action))
            {
                Edit_OnClicked(e.Item);
            }
            else if (string.Equals(L10n.Localize("DeleteLabel"), action))
            {
                Delete_OnClicked(e.Item);
            }
        }
    }
}
