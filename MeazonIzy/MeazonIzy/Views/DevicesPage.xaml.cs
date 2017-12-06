using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MeazonIzy.Localization;
using MeazonIzy.Models;
using MeazonIzy.Resources;
using Newtonsoft.Json;
using Xamarin.Forms;
using Websockets;

namespace MeazonIzy.Views
{
    public partial class DevicesPage : ContentPage
    {
        
        private string Mtoken;
        private IWebSocketConnection connection;
        private ObservableCollection<Plug> devicesCollection;
        private StateActionResources str;
       
        public DevicesPage()
        {
            InitializeComponent();
            Title = L10n.Localize("DevicesTitle");
            str=new StateActionResources();
            devicesCollection=new ObservableCollection<Plug>();
            connection = WebSocketFactory.Create();
            



        }




        private void ConnectionOnOnError(string s)
        {

        }

        private void ConnectionOnOnClosed()
        {
            connection.Close();
        }

        private void ConnectionOnOnOpened()
        {
            //var token = App.MTOKEN.Replace("\"", "");
            string authmsg = "Identity {\"MToken\":\"" + Mtoken + "\"" + "}";
            connection.Send(authmsg);
            string msg = "SubscribeToMyMacs";
            connection.Send(msg);
        }

        private async Task GetMToken()
        {
            try
            {
                var mtokenurl = "https://izy.meazon.com/api/energylog/mToken";
                var client = new HttpClient();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", App.USERTOKEN);
                var json = await client.GetStringAsync(mtokenurl);
                var token = JsonConvert.DeserializeObject<string>(json);
                Mtoken = token.Replace("\"", "");
            }
            catch (Exception e)
            {
                var msg = e.Message;
                string action = await DisplayActionSheet(msg, "Cancel", "Ok");
            }
            


        }

        
        private  void ConnectionOnOnMessage(string msg)
        {
            
                if (msg.StartsWith("PlugState"))
                {


                    msg = msg.Replace("PlugState", "");
                    MyPlugState state = JsonConvert.DeserializeObject<MyPlugState>(msg);



                    for (int i = 0; i < devicesCollection.Count; i++)
                    {
                        if (devicesCollection[i].Mac == state.Mac)
                        {
                            state.IsOn= state.IsOn ?? false;
                            devicesCollection[i].MyPlugState = state;
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

        private async Task GetDeviceList()
        {
            try
            {


                using (var client = new HttpClient())
                {
                    string weburl = "https://izy.meazon.com/api/energylog/devices";
                    
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", App.USERTOKEN);
                   

                    var json = await client.GetStringAsync(weburl);
                    var devList = JsonConvert.DeserializeObject<ObservableCollection<Plug>>(json);
                    devicesCollection = devList;

                 

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

        protected override async void OnAppearing()
        {
            DevListView.ItemsSource = null;
            Indicator.IsRunning = true;
            Indicator.IsVisible = true;
           
            await GetMToken();
            await GetDeviceList();
            connection.Open("wss://webcontrol.meazon.com");
            connection.OnOpened += ConnectionOnOnOpened;
            connection.OnClosed += ConnectionOnOnClosed;
            connection.OnError += ConnectionOnOnError;
            connection.OnMessage += ConnectionOnOnMessage;







            

            

        }

        

       

        protected override void OnDisappearing()
        {
           connection.Close();
        }

       

        


        //private async Task<string> returnTokenAsync()
        //{
        //    //string MtokenUrl = "https://izy.meazon.com/api/energylog/mToken";

        //    //using (var client = new HttpClient())
        //    //{
        //    //    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.USERTOKEN);
        //    //    //client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", App.USERTOKEN);
        //    //    //var token = await client.GetStringAsync(MtokenUrl);
        //    //    //token = token.Replace("\"", "");
        //    //    //return token;




        //    //}



        //}


        private async Task SelectActionState(string mac)
        {
        var act=    await
                DisplayAlert(L10n.Localize("AlertLabel"), L10n.Localize("DisableScheduleQuestion"),
                    L10n.Localize("YesAction"), L10n.Localize("CancelLabel"));
            if (act)
            {
                string schOffMsg = "DisableSchedule {\"MAC\":" + mac + ",\"Number\":0}";
                connection.Send(schOffMsg);
            }
        }
      

        private async void Switch_OnToggled(object sender, ToggledEventArgs e)
        {
            
           
                var s = ((BindableObject)sender).BindingContext;
                if (s != null)
                {

                    var p = (Plug)s;

                    var stateaction = await str.GetStateAction();
                    if (!string.Equals(stateaction, L10n.Localize("State3")))
                    {
                        if (string.Equals(stateaction, L10n.Localize("State2")))
                        {
                            if (p.MyPlugState.IsScheduleEnabled != null && (bool)p.MyPlugState.IsScheduleEnabled)
                            {
                                await SelectActionState(p.Mac);
                            }

                        }
                        else if (string.Equals(stateaction, L10n.Localize("State1")))
                        {
                            string schOffMsg = "DisableSchedule {\"MAC\":" + p.Mac + ",\"Number\":0}";
                            connection.Send(schOffMsg);
                        }
                    }


                    if (e.Value == true)
                    {

                        string turnOnMsg = "TurnOn {\"MAC\":" + p.Mac + ",\"Number\":0}";
                        connection.Send(turnOnMsg);

                    }
                    else if (e.Value == false)
                    {
                        string turnOffMsg = "TurnOff {\"MAC\":" + p.Mac + ",\"Number\":0}";
                        connection.Send(turnOffMsg);

                    }

                }
               
            
           
        }

        private async void DevListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = e.Item as Plug;
            await Navigation.PushAsync(new RenamePage(item));
        }


       
    }
}
