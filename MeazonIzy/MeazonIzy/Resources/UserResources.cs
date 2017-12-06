using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MeazonIzy.Models;
using Newtonsoft.Json;

namespace MeazonIzy.Resources
{
  public  class UserResources
    {
      public async Task<User> GetUserAsync()
      {
          using (var client=new HttpClient())
          {
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", App.USERTOKEN);
               
                var httpUrl = "https://izy.meazon.com/api/energylog/user";
                var jsonData = await client.GetStringAsync(httpUrl);
                
                var user = JsonConvert.DeserializeObject<User>(jsonData);
                return user;
               
            }
      }
    }
}
