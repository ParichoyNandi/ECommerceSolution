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
    public class AllDiscountServices: IAllDiscountService
    {
        private string baseurl;
        private string apiKey;
        String result_string = "";

        public AllDiscountServices(IConfiguration configuration)
        {
            baseurl = configuration.GetValue<string>("BaseUrl");
            apiKey = configuration.GetValue<string>("ApiKey");
        }

        public async Task<List<FeeComponent>> Feescomponentapi(int BrandID = 0)
        {

            using (var client = new HttpClient())
            {
                var fullurl = baseurl + "api/General/GetFeeComponents?BrandID="+ BrandID.ToString();
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("X-API-KEY", apiKey);



                try
                {
                    var responseTask = await client.GetAsync(fullurl);
                    //responseTask.Wait();

                    //var result = responseTask.Content.
                    if (responseTask.IsSuccessStatusCode)
                    {

                        var deserializeobj = JsonConvert.DeserializeObject<Response<List<FeeComponent>>>(responseTask.Content.ReadAsStringAsync().Result);

                        return deserializeobj.data;
                        //result_string = (await responseTask.Content.ReadAsStringAsync()).Result;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

            }

            return null;
            //Console.ReadLine();

        }




        public async Task<List<Brand>> BrandIdApi()
        {

            using (var client = new HttpClient())
            {
                var fullurl = baseurl + "api/General/GetBrands";
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("X-API-KEY", apiKey);



                try
                {
                    var responseTask = await client.GetAsync(fullurl);
                    //responseTask.Wait();

                    //var result = responseTask.Content.
                    if (responseTask.IsSuccessStatusCode)
                    {

                        var deserializeobj = JsonConvert.DeserializeObject<Response<List<Brand>>>(responseTask.Content.ReadAsStringAsync().Result);

                        return deserializeobj.data;
                        //result_string = (await responseTask.Content.ReadAsStringAsync()).Result;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

            }

            return null;
            //Console.ReadLine();

        }




        public async Task<int> Discountcreatemethod(DiscountServicesScheme dssobj)
        {
            var serialobj = JsonConvert.SerializeObject(dssobj);
            var stringContent = new StringContent(JsonConvert.SerializeObject(dssobj), UnicodeEncoding.UTF8, MediaTypeNames.Application.Json);
            var fullurl = baseurl + "api/Discounts/CreateDiscount";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("X-API-KEY", apiKey);
                var response = await client.PostAsync(fullurl, stringContent);
                string result = await response.Content.ReadAsStringAsync();
                var deserializeobj = JsonConvert.DeserializeObject<Response<int>>(result);
                return deserializeobj.data;
            }
        }
    }


    
}
