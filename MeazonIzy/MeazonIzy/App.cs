using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using MeazonIzy.Localization;
using MeazonIzy.Models;
using MeazonIzy.Strings;
using MeazonIzy.Views;
using Websockets;
using Xamarin.Forms;

namespace MeazonIzy
{
    public class App : Application
    {
        public static string USERTOKEN;
        
        public App()
        {

            USERTOKEN = "";

            if (Device.OS == TargetPlatform.Android || Device.OS == TargetPlatform.iOS)
            {
                DependencyService.Get<ILocalize>().SetLocale();
                AppResources.Culture = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();

            }

            
          
            
            MainPage = new NavigationPage(new LogInPage()); ;


        }

        


        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
