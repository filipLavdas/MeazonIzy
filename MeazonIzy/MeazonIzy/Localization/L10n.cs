using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MeazonIzy.Localization
{
  public class L10n
    {
        public static CultureInfo GetCurrentCultureInfo()
        {
            return DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
        }

        public void SetLocale()
        {
            DependencyService.Get<ILocalize>().SetLocale();
        }

        public static string Localize(string key)
        {
            CultureInfo ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();

            // Platform-specific
            ResourceManager temp = new ResourceManager("MeazonIzy.Strings.AppResources", typeof(L10n).GetTypeInfo().Assembly);
            string result = temp.GetString(key, ci);

            return result;
        }

    }
}
