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
    public class PlanListDashboardService : IPlanListDashboardService
    {

        private string baseurl;
        private string apiKey;
        String result_string = "";

        public PlanListDashboardService(IConfiguration configuration)
        {
            baseurl = configuration.GetValue<string>("BaseUrl");
            apiKey = configuration.GetValue<string>("ApiKey");
        }

        public async Task<List<Brand>> BrandListsApi()
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


        public async Task<List<Plan>> GetPlansapi(String brandid, int language)
        {

            using (var client = new HttpClient())
            {
                var fullurl = baseurl + "api/Plans/GetPlanList/" + brandid + "?LanguageID=" + language;
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("X-API-KEY", apiKey);



                try
                {
                    var response = await client.GetAsync(fullurl);

                    if (response.IsSuccessStatusCode)
                    {

                        var obj = JsonConvert.DeserializeObject<Response<List<Plan>>>(response.Content.ReadAsStringAsync().Result);

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
        public async Task<Plan> GetPlansDetailsApi(int PlanID)
        {

            using (var client = new HttpClient())
            {
                var fullurl = baseurl + "api/Plans/GetPlanDetails?PlanID=" + PlanID;
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("X-API-KEY", apiKey);

                try
                {
                    var response = await client.GetAsync(fullurl);
                    if (response.IsSuccessStatusCode)
                    {

                        var obj = JsonConvert.DeserializeObject<Response<Plan>>(response.Content.ReadAsStringAsync().Result);
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
