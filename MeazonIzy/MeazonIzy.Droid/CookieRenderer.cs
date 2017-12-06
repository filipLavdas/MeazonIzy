using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using MeazonIzy.Droid;
using MeazonIzy.Models;

[assembly:Xamarin.Forms.Dependency(typeof(CookieRenderer))]
namespace MeazonIzy.Droid
{
  public class CookieRenderer : ICookie
    {
      public bool SetAppCookie(string url)
      {
           
          
            var cookieHeader = CookieManager.Instance.GetCookie(url);
          if (cookieHeader != null)
          {
                
                if (cookieHeader.Contains("RawToken"))
                {
                    var spliting = cookieHeader.Split(';').ToList();
                    foreach (var spl in spliting)
                    {
                        if (spl.Contains("RawToken"))
                        {
                            var str = spl.Replace("RawToken=", "");
                            str = str.Trim();
                            App.USERTOKEN = str;

                            return true;

                        }

                    }


                }
            }
            
            return false;
        }

      public void ClearCookies(string url)
      {

            CookieManager.Instance.RemoveAllCookie();
           
        }
    }
}