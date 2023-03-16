using ECommAPI.Models;
using Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ECommAPI.Services
{
    public class LanguageService
    {
        //private readonly String baseurl = "https://lms.ricesmart.in/";
        //private readonly String authkey = "3c643f2d8c624292ae190df0beb644d7";

        private readonly String baseurl = " https://lms-pp.ricesmart.in/";
        private readonly String authkey = "3c643f2d8c624292ae190df0beb644d7";

        //private readonly String baseurl = " https://lms-dev.ricesmart.in";
        //private readonly String authkey = "d4ff1944491b48f984d2d5fe7209decb";

        //private readonly String baseurl = "https://lms-uat.ricesmart.in/";
        //private readonly String authkey = "d4ff1944491b48f984d2d5fe7209decb";

        public async Task<List<Langauge>> GetLanguageAPI()
        {
            try
            {
                HttpClient client = new HttpClient();



                string uri = baseurl + "api/v1/get_languages";

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("AUTHORIZATION", authkey);


                var responseTask = await client.GetAsync(uri);

                if (responseTask.IsSuccessStatusCode)
                {

                    var deserializeobj = JsonConvert.DeserializeObject<Response<List<Langauge>>>(responseTask.Content.ReadAsStringAsync().Result);

                    return deserializeobj.data;
                    //result_string = (await responseTask.Content.ReadAsStringAsync()).Result;
                }



            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return null;
        }


       

    }
}
