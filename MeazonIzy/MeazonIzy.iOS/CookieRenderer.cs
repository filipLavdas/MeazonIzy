using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundation;
using MeazonIzy.iOS;
using MeazonIzy.Models;

[assembly:Xamarin.Forms.Dependency(typeof(CookieRenderer))]
namespace MeazonIzy.iOS
{
  public  class CookieRenderer : ICookie
    {
      public bool SetAppCookie(string url)
      {
            var cookieStorage = NSHttpCookieStorage.SharedStorage;
          
          if (cookieStorage != null)
          {
                var cookieList = cookieStorage.CookiesForUrl(new NSUrl(url)).ToList();
                
                foreach (var cookie in cookieList)
                {
                 
                    if (cookie.Name.Contains("RawToken"))
                    {
                      
                        App.USERTOKEN = cookie.Value;
                        return true;
                    }

                }
            }
           
            return false;
        }

      public void ClearCookies(string url)
      {
            var cookieStorage = NSHttpCookieStorage.SharedStorage;
            var cookieList = cookieStorage.CookiesForUrl(new NSUrl(url)).ToList();

            foreach (var cookie in cookieList)
            {

                cookieStorage.DeleteCookie(cookie);

            }
        }
    }
}
