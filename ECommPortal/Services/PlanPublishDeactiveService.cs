using ECommPortal.Services.Interface;
using Entities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Timers;
using System.Threading.Tasks;

namespace ECommPortal.Services
{
    public class PlanPublishDeactiveService : IPlanPublishDeactiveService
    {

        private string baseurl;
        private string apiKey;
        String result_string = "";
        Boolean CanBePublished = true;

        public PlanPublishDeactiveService(IConfiguration configuration)
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


        public async Task<List<Plan>> GetPublishedDeactivetedPlansapi(string BrandList)
        {


            using (var client = new HttpClient())
            {
                //var fullurl = baseurl + "ECommAPI/api/Plans/GetPlanList/" + BrandList;
                var fullurl = baseurl + "api/Plans/GetPlanListForPublishing/" + BrandList + "?CanBePublished=" + CanBePublished+ "&LanguageID=2";

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("X-API-KEY", apiKey);



                try
                {
                    var responseTask = await client.GetAsync(fullurl);
                    //responseTask.Wait();

                    //var result = responseTask.Content.
                    if (responseTask.IsSuccessStatusCode)
                    {

                        var deserializeobj = JsonConvert.DeserializeObject<Response<List<Plan>>>(responseTask.Content.ReadAsStringAsync().Result);

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



        public async Task<Responsestatus> ModifyPlanIsPublishapi(int planid, Boolean IsPublished)
        {

            PlanPublishDeactiveServiceProperty planpublish = new();

            planpublish.PlanID = planid;
            planpublish.Flag = IsPublished;
            var serialobj = JsonConvert.SerializeObject(planpublish);
            var stringContent = new StringContent(JsonConvert.SerializeObject(planpublish), UnicodeEncoding.UTF8, MediaTypeNames.Application.Json);
            var fullurl = baseurl + "api/Plans/PublishPlan/" + planid + "?Flag=" + IsPublished;

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("X-API-KEY", apiKey);
                var response = await client.PutAsync(fullurl, stringContent);
                string result = await response.Content.ReadAsStringAsync();
                var deserializeobj = JsonConvert.DeserializeObject<Responsestatus>(result);
                return deserializeobj;
            }
        }



    }
}
