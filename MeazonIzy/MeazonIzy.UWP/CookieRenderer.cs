using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http.Filters;
using MeazonIzy.Models;
using MeazonIzy.UWP;

[assembly:Xamarin.Forms.Dependency(typeof(CookieRenderer))]
namespace MeazonIzy.UWP
{
   public class CookieRenderer : ICookie
    {
       public bool SetAppCookie(string url)
       {
            var uri = new Uri(url);
            var httpBaseProtocolFilter = new HttpBaseProtocolFilter();
            var cookieManager = httpBaseProtocolFilter.CookieManager;
            
            var cookieCollection = cookieManager.GetCookies(uri);
          
            foreach (var cookie in cookieCollection)
            {
                if (cookie.Name.Contains("RawToken"))
                {
                    MeazonIzy.App.USERTOKEN = cookie.Value;
                   
                    return true;
                }

                
            }
       
            return false;
        }

       public void ClearCookies(string url)
       {
        
                var uri = new Uri(url);
                var httpBaseProtocolFilter = new HttpBaseProtocolFilter();
                var cookieManager = httpBaseProtocolFilter.CookieManager;

                var cookieCollection = cookieManager.GetCookies(uri);
                if (cookieCollection != null)
                {
                    foreach (var cookie in cookieCollection)
                    {
                        cookieManager.DeleteCookie(cookie);


                    }
                }
            
           
       }
    }
}
