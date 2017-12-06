using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Util;

[assembly: Xamarin.Forms.Dependency(typeof(MeazonIzy.Droid.Localize))]
namespace MeazonIzy.Droid
{
    public class Localize : MeazonIzy.Localization.ILocalize
    {
        public CultureInfo GetCurrentCultureInfo()
        {
            var androidLocale = Java.Util.Locale.Default;
            var netlanguage = androidLocale.ToString().Replace("_", "-"); // turns pt_BR into pt-BR
            return new System.Globalization.CultureInfo(netlanguage);

        }

        public void SetLocale()
        {
            var androidLocale = Java.Util.Locale.Default;
            var netlanguage = androidLocale.ToString().Replace("_", "-"); // turns pt_BR into pt-BR
            var config = new global::Android.Content.Res.Configuration();
            config.Locale = new Locale(netlanguage); ;
            var context = global::Android.App.Application.Context;
            context.Resources.UpdateConfiguration(config, context.Resources.DisplayMetrics);

            Java.Util.Locale.Default = new Locale(netlanguage);

        }
    }
}