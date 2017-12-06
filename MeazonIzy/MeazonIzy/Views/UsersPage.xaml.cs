using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MeazonIzy.Localization;
using MeazonIzy.Models;
using MeazonIzy.Resources;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace MeazonIzy.Views
{
    public partial class UsersPage
    {
        private CoAdminResources res;
        public UsersPage()
        {
            InitializeComponent();
            Title = L10n.Localize("UsersTitle");
           
            

        }


        protected override async void OnAppearing()
        {
            await InitializeDataAsync();
        }

        private async Task  InitializeDataAsync()
        {
            if (Device.OS == TargetPlatform.WinPhone)
            {
                Indicator.Scale = 3;
                TheToolbarAdd.Icon = "add.png";

               

            }
            else if (Device.OS == TargetPlatform.Windows)
            {
                Indicator.Scale = 4;
                TheToolbarAdd.Icon = "Assets/add.png";

            }
            else if (Device.OS == TargetPlatform.Android)
            {

                TheToolbarAdd.Icon = "Resources/drawable/add.png";

            }
            res = new CoAdminResources();
            Indicator.IsVisible = true;
            MyLayout.IsVisible = false;

            CoAdminListView.ItemsSource = await res.GetAdminsAsync();



            MyLayout.IsVisible = true;
            Indicator.IsRunning = false;
            Indicator.IsEnabled = false;
            Indicator.IsVisible = false;

        }


        

        private async void Delete_OnClicked(object sender)
        {
            var adm = sender as CoAdmin;
            var act = await DisplayAlert(L10n.Localize("DeleteLabel"), L10n.Localize("DeleteAction"), L10n.Localize("YesAction"), L10n.Localize("NoAction"));
            if (act)
            {
                await res.DeleteAdminAsync(adm.Id);
            }
            Device.BeginInvokeOnMainThread(async () =>
            {
                
                CoAdminListView.ItemsSource = await res.GetAdminsAsync();

            });
        }

        private async void TheToolbarAdd_OnClicked(object sender, EventArgs e)
        {

            await Navigation.PushAsync(new AddCoAdminPage());
        }

       

        private async void CoAdminListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var action = await DisplayActionSheet(L10n.Localize("OptionsLabel"), L10n.Localize("CancelLabel"), "", L10n.Localize("DeleteLabel")
                );
             if (string.Equals(L10n.Localize("DeleteLabel"), action))
            {
                Delete_OnClicked(e.Item);
            }
        }
    }
}
