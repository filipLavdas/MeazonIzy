using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using Foundation;

[assembly:Xamarin.Forms.Dependency(typeof(MeazonIzy.iOS.Localize))]
namespace MeazonIzy.iOS
{
   public class Localize : MeazonIzy.Localization.ILocalize
    {
       public CultureInfo GetCurrentCultureInfo()
       {
            var netLanguage = "en";
            var prefLanguageOnly = "en";
            if (NSLocale.PreferredLanguages.Length > 0)
            {
                var pref = NSLocale.PreferredLanguages[0];

                
                prefLanguageOnly = pref.Substring(0, 2);
               
                netLanguage = pref.Replace("_", "-");
                
            }

            // this gets called a lot - try/catch can be expensive so consider caching or something
            System.Globalization.CultureInfo ci = null;
            try
            {
                ci = new System.Globalization.CultureInfo(netLanguage);
            }
            catch
            {
                // iOS locale not valid .NET culture (eg. "en-ES" : English in Spain)
                // fallback to first characters, in this case "en"
                ci = new System.Globalization.CultureInfo(prefLanguageOnly);
            }

            return ci;
        }

       public void SetLocale()
       {
            var iosLocaleAuto = NSLocale.AutoUpdatingCurrentLocale.LocaleIdentifier;
            var netLocale = iosLocaleAuto.Replace("_", "-");
            System.Globalization.CultureInfo ci;
            try
            {
                ci = new System.Globalization.CultureInfo(netLocale);
            }
            catch
            {
                ci = GetCurrentCultureInfo();
            }
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;

           
        }
    }
}
