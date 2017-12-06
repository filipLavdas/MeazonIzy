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
    public partial class SchedulesPage : ContentPage
    {
        private IWebSocketConnection connection;
        private ObservableCollection<Plug> devicesCollection;
        private ScheduleHelperClass helper;
        private string Mtoken;
        

        public SchedulesPage()
        {
            InitializeComponent();
       
            
            Title = L10n.Localize("ScheduleTitle");
            devicesCollection=new ObservableCollection<Plug>();
           
            connection = Websockets.WebSocketFactory.Create();
            helper = new ScheduleHelperClass();
            
           

            
        }

        protected override async void OnAppearing()
        {
            DevListView.ItemsSource = null;
            Indicator.IsRunning = true;
            Indicator.IsVisible = true;
            
            await GetMToken();

            connection.Open("wss://webcontrol.meazon.com");
            connection.OnOpened += ConnectionOnOnOpened;
            connection.OnError += ConnectionOnOnError;
            connection.OnMessage += ConnectionOnOnMessage;

            try
            {


                using (var client = new HttpClient())
                {
                    string weburl = "https://izy.meazon.com/api/energylog/devices";
                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.USERTOKEN);
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", App.USERTOKEN);
                    //client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", App.USERTOKEN));

                    var json = await client.GetStringAsync(weburl);
                    var devList = JsonConvert.DeserializeObject<ObservableCollection<Plug>>(json);
                    devicesCollection = devList;

                    //if (devList.Count == 0)
                    //{
                    //    await DisplayAlert("Alert", "No devices Were Found", "Ok");
                    //}
                    //else
                    //{
                    //    DevListView.ItemsSource = devList;

                    //}

                }
            }
            catch (Exception e)
            {
                var msg = e.Message;
                string action = await DisplayActionSheet(msg, "Cancel", "Ok");
                //if (action == "Ok")
                //{
                //    
                //    await Navigation.PushAsync(new HomePage());

                //}

            }


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

        private void ConnectionOnOnMessage(string msg)
        {
            if (msg.StartsWith("PlugState"))
            {

                msg = msg.Replace("PlugState", "");
                MyPlugState state = JsonConvert.DeserializeObject<MyPlugState>(msg);



                for (int i = 0; i < devicesCollection.Count; i++)
                {
                    if (string.Equals(devicesCollection[i].Mac , state.Mac))
                    {
                        state.IsOn = state.IsOn ?? false;
                        state.IsScheduleEnabled = state.IsScheduleEnabled ?? false;
                        
                        devicesCollection[i].MyPlugState = state;
                        if (state.Schedule!=null)
                        {
                            devicesCollection[i].MyPlugState.MyScheduleList =
                            helper.DisirializeSchedule(devicesCollection[i].MyPlugState.Schedule);
                        }
                        
                    }
                }

              Device.BeginInvokeOnMainThread(() =>
              {
                  Indicator.IsRunning = false;
                  Indicator.IsVisible = false;
                  DevListView.ItemsSource = devicesCollection;
              });
                

            }
        }

        private void ConnectionOnOnError(string s)
        {
           connection.Close();
        }

        private void ConnectionOnOnOpened()
        {
            var token = Mtoken;
            string authmsg = "Identity {\"MToken\":\"" + token + "\"" + "}";
            connection.Send(authmsg);
            string msg = "SubscribeToMyMacs";
            connection.Send(msg);
        }


        protected override void OnDisappearing()
        {
            connection.Close();
        }

        

        

        private void Switch_OnToggled(object sender, ToggledEventArgs e)
        {
           
                var s = ((BindableObject)sender).BindingContext;
            if (s != null)
            {

                var p = (Plug)s;
                if (e.Value == true)
                {

                    string schOnMsg = "EnableSchedule {\"MAC\":" + p.Mac + ",\"Number\":0}";
                    connection.Send(schOnMsg);

                }
                else if (e.Value == false)
                {
                    string schOffMsg = "DisableSchedule {\"MAC\":" + p.Mac + ",\"Number\":0}";
                    connection.Send(schOffMsg);

                }

            }
                
            
            
        }

        private async void DevListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = e.Item as Plug;
            var page = new SchedulePage(item);
            page.BindingContext = item;

          
            await Navigation.PushAsync(page);

        }
    }
}
