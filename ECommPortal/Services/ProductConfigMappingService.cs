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
    public class ProductConfigMappingService: IProductConfigMappingService
    {
        private string baseurl;
        private string apiKey;
        String result_string = "";
        string brandid = "109";
        private readonly string _bucketname;
        private readonly string _accesskey;
        private readonly string _secretkey;

        public ProductConfigMappingService(IConfiguration configuration)
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

        public async Task<List<Product>> GetProductsapi(int brandid,Boolean Ispublished)
        {

            using (var client = new HttpClient())
            {
                var fullurl = baseurl + "api/Products/GetProducts/" + brandid+ "?IsPublished="+ Ispublished+ "&LanguageID=2";
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

        public async Task<Responsestatus> ProductconfigMap(List<ProductConfigMappingServiceProperty> pcmsp)
        {


            var serialobj = JsonConvert.SerializeObject(pcmsp);
            var stringContent = new StringContent(JsonConvert.SerializeObject(pcmsp), UnicodeEncoding.UTF8, MediaTypeNames.Application.Json);
            var fullurl = baseurl + "api/Configs/ProductConfigMap";

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


        /* custom created Header and SubHeader */

        public async Task<List<ProductHeaderProperty>> GetProductHeaderApi()
        {

            List<ProductHeaderProperty> ProductHeaderLists = new();

            for (int i = 1; i < 10; i++)
            {
                ProductHeaderLists.Add(new ProductHeaderProperty()
                {
                    HeaderID = i,
                    HeaderCode = "Header "+ i,
                    HeaderDisplayName = "Product Header "+ i,
                });

            }
            return ProductHeaderLists;
        }


        public async Task<List<ProductSubHeaderProperty>> GetProductSubHeaderApi()
        {

            List<ProductSubHeaderProperty> ProductSubHeaderLists = new();

            for (int i = 1; i < 10; i++)
            {
                ProductSubHeaderLists.Add(new ProductSubHeaderProperty()
                {
                     SubHeaderID =i,
                     SubHeaderCode = "SubHeader " + i,
                     SubHeaderDisplayName = "Product Sub Header " + i,
                });

            }
            return ProductSubHeaderLists;
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
