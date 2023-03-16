using ECommManagement;
using ECommPortal.Models.ValueObjects;
using ECommPortal.Services.Interface;
using Entities;
using Microsoft.AspNetCore.Http;
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
    public class PlanProductsConfigMappingService : IPlanProductsConfigMappingService
    {
        private string baseurl;
        private string apiKey;
        String result_string = "";
        Boolean CanBePublished = false;

        private readonly string _bucketname;
        private readonly string _accesskey;
        private readonly string _secretkey;

        public PlanProductsConfigMappingService(IConfiguration configuration)
        {
            baseurl = configuration.GetValue<string>("BaseUrl");
            apiKey = configuration.GetValue<string>("ApiKey");

            _bucketname = configuration.GetValue<string>("S3BucketName");
            _accesskey = configuration.GetValue<string>("AWSAccessKey");
            _secretkey = configuration.GetValue<string>("AWSSecretKey");
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


        public async Task<List<Plan>> GetPlanListsApi(string BrandLists)
        {

            using (var client = new HttpClient())
            {
                //var fullurl = baseurl + "ECommAPI/api/Products/GetProducts/" + brandid;
                // var fullurl = baseurl + "ECommAPI/api/Plans/GetPlanList/" + BrandLists;
                var fullurl = baseurl + "api/Plans/GetPlanListForPublishing/" + BrandLists+ "?CanBePublished="+ CanBePublished+ "&LanguageID=2";
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

        public async Task<List<Product>> GetProductsapi(int brandid, Boolean Ispublished)
        {

            using (var client = new HttpClient())
            {
                //var fullurl = baseurl + "ECommAPI/api/Products/GetProducts/"+ brandid;
                var fullurl = baseurl + "api/Products/GetProducts/" + brandid + "?IsPublished=" + Ispublished+ "&LanguageID=2";
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("X-API-KEY", apiKey);



                try
                {
                    var responseTask = await client.GetAsync(fullurl);
                    //responseTask.Wait();

                    //var result = responseTask.Content.
                    if (responseTask.IsSuccessStatusCode)
                    {

                        var deserializeobj = JsonConvert.DeserializeObject<Response<List<Product>>>(responseTask.Content.ReadAsStringAsync().Result);

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


        public async Task<List<Config>> ConfigListsApi()
        {

            using (var client = new HttpClient())
            {
                var fullurl = baseurl + "api/Configs/GetConfigs";
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("X-API-KEY", apiKey);



                try
                {
                    var responseTask = await client.GetAsync(fullurl);
                    //responseTask.Wait();

                    //var result = responseTask.Content.
                    if (responseTask.IsSuccessStatusCode)
                    {

                        var deserializeobj = JsonConvert.DeserializeObject<Response<List<Config>>>(responseTask.Content.ReadAsStringAsync().Result);

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


        public async Task<List<PlanHeaderProperty>> GetPlanHeaderApi()
        {

            List<PlanHeaderProperty> PlanHeaderLists = new();

            for (int i = 1; i < 10; i++)
            {
                PlanHeaderLists.Add(new PlanHeaderProperty()
                {
                    HeaderID = i,
                    HeaderCode = "Header " + i,
                    HeaderDisplayName = "Plan Header " + i,
                });

            }
            return PlanHeaderLists;
        }


        public async Task<List<PlanSubHeaderProperty>> GetPlanSubHeaderApi()
        {

            List<PlanSubHeaderProperty> PlanSubHeaderLists = new();

            for (int i = 1; i < 10; i++)
            {
                PlanSubHeaderLists.Add(new PlanSubHeaderProperty()
                {
                    SubHeaderID = i,
                    SubHeaderCode = "SubHeader " + i,
                    SubHeaderDisplayName = "Plan Sub Header " + i,
                });

            }
            return PlanSubHeaderLists;
        }



        public async Task<Responsestatus> SavePlanConfigMapping(List<PlanConfigMappingServiceProperty> ppcsp)
        {
            var serialobj = JsonConvert.SerializeObject(ppcsp);
            var stringContent = new StringContent(JsonConvert.SerializeObject(ppcsp), UnicodeEncoding.UTF8, MediaTypeNames.Application.Json);
            var fullurl = baseurl + "api/Configs/PlanConfigMap";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("X-API-KEY", apiKey);
                var response = await client.PostAsync(fullurl, stringContent);
                string result = await response.Content.ReadAsStringAsync();
                var deserializeobj = JsonConvert.DeserializeObject<Responsestatus>(result);
                return deserializeobj;
            }
        }


        public async Task<Responsestatus> SavePlanproductMapping(PlanProductsMappingServiceProperty ppmsp)
        {

            var serialobj = JsonConvert.SerializeObject(ppmsp);
            var stringContent = new StringContent(JsonConvert.SerializeObject(ppmsp), UnicodeEncoding.UTF8, MediaTypeNames.Application.Json);
            var fullurl = baseurl + "api/Plans/SavePlanProductMap/" + ppmsp.PlanID+ "?ProductIDList="+ppmsp.ProductIDList;

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("X-API-KEY", apiKey);
                var response = await client.PostAsync(fullurl, stringContent);
                string result = await response.Content.ReadAsStringAsync();
                var deserializeobj = JsonConvert.DeserializeObject<Responsestatus>(result);
                return deserializeobj;
            }
        }

        public async Task<string> StudyPlanDownloadLink(IFormFile uploadedfile)
        {

            FileManagement awsfilemanagment = new();
            String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            string filename = uploadedfile.FileName.Substring(0, uploadedfile.FileName.IndexOf(".")) + timeStamp;
            filename += uploadedfile.FileName.Substring(uploadedfile.FileName.IndexOf("."), uploadedfile.FileName.Length - uploadedfile.FileName.IndexOf("."));
            String studyplanurl = await awsfilemanagment.UploadFileToS3(uploadedfile, filename, _bucketname, _accesskey, _secretkey);
            return studyplanurl;
        }

    }
}
