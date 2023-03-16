using ECommPortal.Services.Interface;
using Entities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace ECommPortal.Services
{
    public class AccountService: IAccountService
    {

        private string baseurl;
        private string apiKey;
        String result_string = "";

        public AccountService(IConfiguration configuration)
        {
            baseurl = configuration.GetValue<string>("BaseUrl");
            apiKey = configuration.GetValue<string>("ApiKey");
        }

        public async Task<int> Validateuserapi(string Loginid,string password)
        {

            using (var client = new HttpClient())
            {
                var fullurl = baseurl + "api/General/ValidateUser?LoginID=" + Loginid+ "&Password="+ password;
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("X-API-KEY", apiKey);



                try
                {
                    var responseTask = await client.GetAsync(fullurl);
                    //responseTask.Wait();

                    //var result = responseTask.Content.
                    if (responseTask.IsSuccessStatusCode)
                    {

                        var deserializeobj = JsonConvert.DeserializeObject<int>(responseTask.Content.ReadAsStringAsync().Result);

                        return deserializeobj;
                        //result_string = (await responseTask.Content.ReadAsStringAsync()).Result;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

            }

            return 0;
            //Console.ReadLine();

        }
    }
}
