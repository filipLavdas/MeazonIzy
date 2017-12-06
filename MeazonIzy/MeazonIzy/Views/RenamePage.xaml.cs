using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using MeazonIzy.Localization;
using MeazonIzy.Models;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace MeazonIzy.Views
{
    public partial class RenamePage : ContentPage
    {

        public Plug SelectedDevice { get; set; }
        public RenamePage(Plug device)
        {
            InitializeComponent();
            Title = L10n.Localize("RenameTitle");
            SelectedDevice = device;
            MyEntry.Text = SelectedDevice.Name;
            CancelButton.Image = Device.OnPlatform(iOS: ImageSource.FromFile("Resources/Cancel.png"),
                Android: ImageSource.FromFile("Resources/drawable/Cancel.png")
                , WinPhone: ImageSource.FromFile("Assets/Cancel.png")) as FileImageSource;


            SaveButton.Image = Device.OnPlatform(iOS: ImageSource.FromFile("Resources/Check.png"),
                Android: ImageSource.FromFile("Resources/drawable/Check.png")
                , WinPhone: ImageSource.FromFile("Assets/Check.png")) as FileImageSource;

        }


        private async void ButtonSave_OnClicked(object sender, EventArgs e)
        {

            SelectedDevice.Name = MyEntry.Text;
            try
            {
                using (var client = new HttpClient())
                {
                    string weburl = "https://izy.meazon.com/api/energylog/devices/";
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.USERTOKEN);
                    var json = JsonConvert.SerializeObject(SelectedDevice);
                    HttpContent httpContent = new StringContent(json);
                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var result = await client.PutAsync(weburl + SelectedDevice.Id, httpContent);

                }
                await Navigation.PopAsync(true);

            }
            catch (Exception)
            {

                await DisplayAlert("error ", "an error occured in communication", "Ok");
            }

        }


        private async void ButtonCancel_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync(true);

        }

       
    }
}
