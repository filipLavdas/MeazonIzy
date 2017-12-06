using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MeazonIzy.Localization;
using MeazonIzy.Models;
using MeazonIzy.Resources;
using MeazonIzy.Strings;
using Xamarin.Forms;

namespace MeazonIzy.Views
{
    public partial class AddCoAdminPage : ContentPage
    {
        private Regex regex;

        public AddCoAdminPage()
        {
            InitializeComponent();
            Title = L10n.Localize("AddAdminTitle");

        }

    

        protected override void OnAppearing()
        {
            string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
         + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
         + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            regex = new Regex(pattern, RegexOptions.IgnoreCase);
            CancelButton.Image = Device.OnPlatform(iOS: ImageSource.FromFile("Resources/Cancel.png"),
                Android: ImageSource.FromFile("Resources/drawable/Cancel.png")
                , WinPhone: ImageSource.FromFile("Assets/Cancel.png")) as FileImageSource;


            SaveButton.Image= Device.OnPlatform(iOS: ImageSource.FromFile("Resources/Check.png"),
                Android: ImageSource.FromFile("Resources/drawable/Check.png")
                , WinPhone: ImageSource.FromFile("Assets/Check.png")) as FileImageSource;

        }

        private async void Button_CancelOnClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync(true);
        }

        private async void Button_SaveOnClicked(object sender, EventArgs e)
        {



            string tmp = MyEntry.Text;
            
            if (tmp != null)
            {

                Match match = regex.Match(tmp);
                if (match.Success)
                {

                    var r = new CoAdminResources();
                  


                        var admin = new CoAdmin
                        {
                            Email = tmp
                        };
                        await r.PostAdminAsync(admin);

                   
                   
                    await Navigation.PopAsync();

                }
                else
                {
                    await DisplayAlert(L10n.Localize("AlertLabel"), L10n.Localize("ValidationAlert"), L10n.Localize("OkAction"));
                }
            }
            else
            {
                await DisplayAlert(L10n.Localize("AlertLabel"), L10n.Localize("NullAlert"), L10n.Localize("OkAction"));
            }


        }
    }
}
