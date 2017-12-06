using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using MeazonIzy.Models;
using Newtonsoft.Json;

namespace MeazonIzy.Resources
{
   public class CoAdminResources
    {
        public async Task<ObservableCollection<CoAdmin>> GetAdminsAsync()
        {

           
            var httpUrl = "https://izy.meazon.com/api/energylog/userCoAdmins/";
            
            var client = new HttpClient();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", App.USERTOKEN);
            var jsonData = await client.GetStringAsync(httpUrl);
            var coadminlist = JsonConvert.DeserializeObject<ObservableCollection<CoAdmin>>(jsonData);

            return coadminlist;
           
        }

        public async Task PostAdminAsync(CoAdmin admin)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", App.USERTOKEN);
            var httpUrl = "https://izy.meazon.com/api/energylog/userCoAdmins";
            var jsonData = JsonConvert.SerializeObject(admin);
            HttpContent content = new StringContent(jsonData);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
          var smth=  await client.PostAsync(httpUrl, content);
            var sthnew = smth.IsSuccessStatusCode;
        }

   

        public async Task DeleteAdminAsync(int id)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", App.USERTOKEN);
            
            var httpUrl = "https://izy.meazon.com/api/energylog/userCoAdmins/" + id;
            await client.DeleteAsync(httpUrl);
        }
    }
}
