//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using ECommPortal.Services.Interface;
//using Microsoft.Extensions.Logging;
//using Newtonsoft.Json;

//using System.Net;
//using System.Net.Http;
//using System.Net.Mime;
//using System.Text;
//using System.Linq;
//using System.Threading;
//using ECommPortal.Controllers;
//using Interfaces;
//using ECommPortal.Models;
//using ECommPortal.Services;
//using Entities;




////using ECommPortal.Data;
////using ECommPortal.Data;
////using ECommPortal.Entities.Push;
////using ECommPortal.Model;


//namespace ECommPortal.Services
//{
//    public class DiscountServices: IDiscountServices
//    {
//        private readonly ILogger<HomeController> _logger;
//        private IDBAccess _data;

//        public DiscountServices(ILogger<HomeController> logger)
//        {
//            _logger = logger;
//        }
//        public async Task Discount( )
//        {
//            _logger.LogInformation($"{DateTime.Now:hh:mm:ss} Discount Services is working.");

//            List<DiscountSchemeCenterMap> Disc = new List<DiscountSchemeCenterMap>();
           
//            int DiscountSchemeID = 0;

//            Disc = _data.GetDiscountSchemeCenterCourseMap(DiscountSchemeID);

//            foreach (var d in Disc)
//            {


//                DiscountSchemeCenterCourseMap Dis = new DiscountSchemeCenterCourseMap();
//                Response<DiscountSchemeCenterCourseMap> response = new Response<DiscountSchemeCenterCourseMap>();

//                Dis.DiscountCenterCourseDetailID = d.DiscountCenterDetailID;
                

//                        response.data = Dis;
                        

                   

//                        HttpClient client = new HttpClient();

//                string url = "http://111.93.179.194:85/ECommPortal/api/Discounts/CreateDiscount";
//               // string url = "http://localhost:44316/EComPortal/api/Discounts/CreateDiscount";
//                        client.DefaultRequestHeaders.Accept.Clear();
//                        client.DefaultRequestHeaders.Add("X-API-KEY", "983rr8hf82nfwi393e42");

//                        var chk = JsonConvert.SerializeObject(response);
//                        var stringContent = new StringContent(JsonConvert.SerializeObject(response), UnicodeEncoding.UTF8, MediaTypeNames.Application.Json);

//                        _logger.LogInformation($"{DateTime.Now:hh:mm:ss} API is called.");

                        
//                        var result = await client.PutAsync(url, stringContent);

//                        if (result.StatusCode.Equals(HttpStatusCode.OK))
//                        {
//                            _data.DiscountInterface(d.DiscountCenterDetailID);
//                        }
//                        else
//                        {
//                            var r = JsonConvert.DeserializeObject<ErrorViewModel>(result.Content.ReadAsStringAsync().Result);
//                            _data.DiscountInterface(d.DiscountCenterDetailID);
//                        }
                    
                   

                
//            }

            
//        }
//    }


//}
