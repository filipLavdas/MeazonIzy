﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: Xamarin.Forms.Dependency(typeof(MeazonIzy.WinPhone.Localize))]
namespace MeazonIzy.WinPhone
{
    public class Localize : MeazonIzy.Localization.ILocalize
    {
        public CultureInfo GetCurrentCultureInfo()
        {
            return CultureInfo.CurrentUICulture;
        }

        public void SetLocale()
        {
            //not in use
        }
    }
}
