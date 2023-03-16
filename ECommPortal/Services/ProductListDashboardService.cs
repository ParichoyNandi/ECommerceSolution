using ECommPortal.Services.Interface;
using Entities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ECommPortal.Services
{
    public class ProductListDashboardService : IProductListDashboardService
    {
         
        private string baseurl;
        private string apiKey;
        String result_string = "";

        public ProductListDashboardService(IConfiguration configuration)
        {
            baseurl = configuration.GetValue<string>("BaseUrl");
            apiKey = configuration.GetValue<string>("ApiKey");
        }


        public async Task<List<Product>> GetProductsapi(int brandid, Boolean Ispublished)
        {

            using (var client = new HttpClient())
            {
                var fullurl = baseurl + "api/Products/GetProducts/" + brandid + "?IsPublished=" + Ispublished;
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("X-API-KEY", apiKey);



                try
                {
                    var response = await client.GetAsync(fullurl);
                   
                    if (response.IsSuccessStatusCode)
                    {

                        var obj = JsonConvert.DeserializeObject<Response<List<Product>>>(response.Content.ReadAsStringAsync().Result);

                        return obj.data;
                       
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

            }

            return null;
        

        }

    }
}
