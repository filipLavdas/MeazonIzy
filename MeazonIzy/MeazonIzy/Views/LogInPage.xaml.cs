using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeazonIzy.Localization;
using MeazonIzy.Models;
using Xamarin.Forms;

namespace MeazonIzy.Views
{
    public partial class LogInPage : ContentPage
    {
        public LogInPage()
        {
            InitializeComponent();


            LogInWebView.IsVisible = false;
            Indicator.IsVisible = false;
            
            LogInWebView.Navigated+=LogInWebViewOnNavigated;
            LogInWebView.Navigating+=LogInWebViewOnNavigating;
            Title =L10n.Localize("LoginTitle");
            
            
            FbButton.Image = Device.OnPlatform(iOS: ImageSource.FromFile("Resources/facebook-logo.png"),
                Android: ImageSource.FromFile("Resources/drawable/facebook-logo.png")
                , WinPhone: ImageSource.FromFile("Assets/facebook-logo.png")) as FileImageSource;


            GoogleButton.Image = Device.OnPlatform(iOS: ImageSource.FromFile("Resources/google.png"),
                Android: ImageSource.FromFile("Resources/drawable/google.png")
                , WinPhone: ImageSource.FromFile("Assets/google.png")) as FileImageSource;
            MeazonButton.Image = Device.OnPlatform(iOS: ImageSource.FromFile("Resources/User.png"),
                Android: ImageSource.FromFile("Resources/drawable/User.png")
                , WinPhone: ImageSource.FromFile("Assets/User.png")) as FileImageSource;
        }

        private void LogInWebViewOnNavigating(object sender, WebNavigatingEventArgs e)
        {
            if (e.Url.Equals("https://izy.meazon.com/#/devices"))
            {
                LogInWebView.IsVisible = false;

                Indicator.IsVisible = true;



            }
        }

      

        private async void LogInWebViewOnNavigated(object sender, WebNavigatedEventArgs e)
        {
          
           
            //get cookies from this site
            var what = DependencyService.Get<ICookie>().SetAppCookie(e.Url);
            
            if (what)//if acces token  found
            {//go to main page
                var homepage = new HomePage
                {
                    Detail = new DevicesPage(),
                    IsPresented = Device.OnPlatform(iOS: false, Android: false,
                        WinPhone: true)

                };
                await Navigation.PopToRootAsync(true);
                await Navigation.PushAsync(homepage);

                var thispage = Navigation.NavigationStack[Navigation.NavigationStack.Count - 2];
                Navigation.RemovePage(thispage);
            }


        }

        private  void GoogleButton_OnClicked(object sender, EventArgs e)
        {
            MainGrid.IsVisible = false;
            ButtonsLayout.IsVisible = false;
           
            
            
            string url =
                "https://accounts.google.com/ServiceLogin?passive=1209600&continue=https://accounts.google.com/o/oauth2/auth?openid.realm%3Dhttps://meazonizy.accesscontrol.windows.net:443/v2/openid%26scope%3Dopenid%2Bemail%26response_type%3Dcode%26redirect_uri%3Dhttps://meazonizy.accesscontrol.windows.net:443/v2/openid%26state%3DcHI9d3NmZWRlcmF0aW9uJnJtPXVyaSUzYW1lYXpvbiUzYWl6eSUzYXByb2QmY3g9cm0lM2QwJTI2aWQlM2RwYXNzaXZlJTI2cnUlM2QlMmYlMjMlMmZkZXZpY2VzJmlwPUdvb2dsZQ2%26client_id%3D27678401929-09ksgdl75heogp847jolnpkpf9828q9a.apps.googleusercontent.com%26from_login%3D1%26as%3D19e02827ed588909&oauth=1&sarp=1&scc=1";
            LogInWebView.Source = new UrlWebViewSource
            {
                Url = url

            };
            LogInWebView.IsVisible = true;
            MainGrid.IsVisible = true;


        }

        private void FbButton_OnClicked(object sender, EventArgs e)
        {
            MainGrid.IsVisible = false;
            ButtonsLayout.IsVisible = false;

            string url =
                "https://www.facebook.com/login.php?skip_api_login=1&api_key=616961791748510&signed_next=1&next=https%3A%2F%2Fwww.facebook.com%2Fv2.1%2Fdialog%2Foauth%3Fredirect_uri%3Dhttps%253A%252F%252Fmeazonizy.accesscontrol.windows.net%252Fv2%252Ffacebook%253Fcx%253DcHI9d3NmZWRlcmF0aW9uJnJtPXVyaSUzYW1lYXpvbiUzYWl6eSUzYXByb2QmY3g9cm0lM2QwJTI2aWQlM2RwYXNzaXZlJTI2cnUlM2QlMmYlMjMlMmZkZXZpY2VzJmlwPUZhY2Vib29rLTYxNjk2MTc5MTc0ODUxMA2%26scope%3Demail%26client_id%3D616961791748510%26ret%3Dlogin%26logger_id%3D9ca5054a-eaf6-4972-a6de-7d14052f428f&cancel_url=https%3A%2F%2Fmeazonizy.accesscontrol.windows.net%2Fv2%2Ffacebook%3Fcx%3DcHI9d3NmZWRlcmF0aW9uJnJtPXVyaSUzYW1lYXpvbiUzYWl6eSUzYXByb2QmY3g9cm0lM2QwJTI2aWQlM2RwYXNzaXZlJTI2cnUlM2QlMmYlMjMlMmZkZXZpY2VzJmlwPUZhY2Vib29rLTYxNjk2MTc5MTc0ODUxMA2%26error%3Daccess_denied%26error_code%3D200%26error_description%3DPermissions%2Berror%26error_reason%3Duser_denied%23_%3D_&display=page&locale=el_GR&logger_id=9ca5054a-eaf6-4972-a6de-7d14052f428f";
            LogInWebView.Source = new UrlWebViewSource
            {
                Url = url

            };
            LogInWebView.IsVisible = true;
            MainGrid.IsVisible = true;
        }

        private void MeazonButton_OnClicked(object sender, EventArgs e)
        {
            MainGrid.IsVisible = false;
            ButtonsLayout.IsVisible = false;

            string url =
                 "https://login.microsoftonline.com/4dc61b66-cfa6-4447-9b4d-655d41364924/wsfed?wa=wsignin1.0&wtrealm=https%3a%2f%2fmeazonizy.accesscontrol.windows.net%2f&wreply=https%3a%2f%2fmeazonizy.accesscontrol.windows.net%2fv2%2fwsfederation&wctx=cHI9d3NmZWRlcmF0aW9uJnJtPXVyaSUzYW1lYXpvbiUzYWl6eSUzYXByb2QmY3g9cm0lM2QwJTI2aWQlM2RwYXNzaXZlJTI2cnUlM2QlMmYlMjMlMmZkZXZpY2Vz0";
            LogInWebView.Source = new UrlWebViewSource
            {
                Url = url

            };
            LogInWebView.IsVisible = true;
            MainGrid.IsVisible = true;

        }
    }
}
