using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using MeazonIzy.Localization;
using MeazonIzy.Models;
using Newtonsoft.Json;

namespace MeazonIzy.Resources
{
   public class StateActionResources
    {
       

       public async Task<string> GetStateAction()
       {
          
           var client=new HttpClient();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", App.USERTOKEN);
         
            var httpUrl = "https://izy.meazon.com/api/energylog/UserSettings?key=Override.Scheduler";
            var jsonData =await client.GetStringAsync(httpUrl);
            var act = JsonConvert.DeserializeObject<string>(jsonData);
           

           string action;

            if (act == null)
            {
                return "";
            }

            if (string.Equals(act, "ask"))
            {
                action = L10n.Localize("State2");
            }
            else if (string.Equals(act, "nothing"))
            {
                action = L10n.Localize("State3");
            }
            else
            {
                action = L10n.Localize("State1");
            }
           
           return action;
       }

        public async  Task PostStateAction(string key,string newkey)
       {
            var client = new HttpClient();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", App.USERTOKEN);
            var httpUrl = "https://izy.meazon.com/api/energylog/UserSettings?key=Override.Scheduler";
          
            var jsonData = JsonConvert.SerializeObject(newkey);
            HttpContent content = new StringContent(jsonData);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
           
            var ans2 = await client.PostAsync(httpUrl, content);
           
        }

       public string TranslateStateAction(string obj)
       {
           string action="";
           if (string.Equals(L10n.Localize("State1"), obj))
           {
               action= "override";
           }
           if (string.Equals(L10n.Localize("State2"), obj))
           {
               action= "ask";
           }
           if (string.Equals(L10n.Localize("State3"), obj))
           {
               action= "nothing";
           }
           return action;
       }
    }
}
