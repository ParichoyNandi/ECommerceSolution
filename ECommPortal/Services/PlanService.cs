using ECommManagement;
using ECommPortal.Services.Interface;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace ECommPortal.Services
{
    public class PlanService: IPlanService
    {

        private string baseurl;
        private string apiKey;
        String result_string = "";
        string brandid = "109";
        private readonly string _bucketname;
        private readonly string _accesskey;
        private readonly string _secretkey;

        public PlanService(IConfiguration configuration)
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



        public async Task<string> PlanImageLink(IFormFile uploadedfile)
        {

            FileManagement awsfilemanagment = new();
            String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            string filename = uploadedfile.FileName.Substring(0, uploadedfile.FileName.IndexOf(".")) + timeStamp;
            filename += uploadedfile.FileName.Substring(uploadedfile.FileName.IndexOf("."), uploadedfile.FileName.Length - uploadedfile.FileName.IndexOf("."));
            String imageurl = await awsfilemanagment.UploadFileToS3(uploadedfile, filename, _bucketname, _accesskey, _secretkey);
            return imageurl;

        }

        public async Task<int> Plancreatemethod(PlanServiceProperty pspobj)
        {
            var serialobj = JsonConvert.SerializeObject(pspobj);
            var stringContent = new StringContent(JsonConvert.SerializeObject(pspobj), UnicodeEncoding.UTF8, MediaTypeNames.Application.Json);
            var fullurl = baseurl + "api/Plans/SavePlan";

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
