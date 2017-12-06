using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeazonIzy.Localization;
using MeazonIzy.Models;
using MeazonIzy.Resources;
using Xamarin.Forms;

namespace MeazonIzy.Views
{
    public partial class HomePage : MasterDetailPage
    {
        public HomePage()
        {
            InitializeComponent();
            if (Device.OS == TargetPlatform.Android)
            {
                Master.Title = L10n.Localize("MasterTitleAndroid");
                //Master.Icon = "select.png";
                Master.Padding = 2;

            }
            else if (Device.OS == TargetPlatform.WinPhone)
            {
                Master.Title = L10n.Localize("MasterTitleAndroid"); 
                //Master.Icon = "swap.png";
            }
            else if (Device.OS == TargetPlatform.iOS)
            {
                Master.Title = L10n.Localize("MasterTitleAndroid"); 
                //Master.Title = "Resources/select.png";

            }
            else if (Device.OS == TargetPlatform.Windows)
            {
                Master.Title = L10n.Localize("MasterTitleAndroid");
                //Master.Icon = "Assets/select.png";

            }


            


            var r = new MenuResources();
            MenuList.ItemsSource = r.GetNames();

        }

        

        private async Task SetPage(string option)
        {
            if (string.Equals(option, L10n.Localize("DevicesTitle")))
            {
                Detail = new DevicesPage();
                Device.OnPlatform(iOS: IsPresented = false, Android: IsPresented = false,
                    WinPhone: IsPresented = true);
            }
            else if (string.Equals(option, L10n.Localize("SettingsTitle")))
            {
                Detail = new SettingsPage();
                Device.OnPlatform(iOS: IsPresented = false, Android: IsPresented = false,
                    WinPhone: IsPresented = true);

            }
            else if (string.Equals(option, L10n.Localize("ScheduleTitle")))
            {
                Detail = new SchedulesPage();
                Device.OnPlatform(iOS: IsPresented = false, Android: IsPresented = false,
                    WinPhone: IsPresented = true);
            }
            else if (string.Equals(option, L10n.Localize("AboutTitle")))
            {
                Detail = new AboutPage();
                Device.OnPlatform(iOS:IsPresented=false,Android:IsPresented=false,
                    WinPhone:IsPresented=true);

            }
            else if (string.Equals(option, L10n.Localize("UsersTitle")))
            {

                Detail = new UsersPage();
                Device.OnPlatform(iOS: IsPresented = false, Android: IsPresented = false,
                    WinPhone: IsPresented = true);
            }


        }

        protected override async void OnAppearing()
        {
            var ur = new UserResources();
            var user = await ur.GetUserAsync();
            
            UserNameLabel.Text = user.Name;

        }

        private async void MenuList_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var MenuBind = e.Item;
            var MenuChoice = (MainMenuItems)MenuBind;

            var option = MenuChoice.Name;
            await SetPage(option);
        }
    }
}
