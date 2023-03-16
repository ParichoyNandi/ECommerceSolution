using ECommPortal.Models;
using ECommPortal.Services;
using ECommPortal.Services.Interface;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommPortal.Controllers
{
    public class ProductListDashboardController : Controller
    {
        private IProductListDashboardService apiproductlistdashboard;
        private IHttpContextAccessor _httpContext;
        private string LoginID;

        public ProductListDashboardController  (IProductListDashboardService productlist ,IHttpContextAccessor httpContext)
        {
            this.apiproductlistdashboard = productlist;
            _httpContext = httpContext;
            if (_httpContext.HttpContext.Session.Keys.Contains("LoginID"))
                LoginID = _httpContext.HttpContext.Session.GetString("LoginID");
        }

        public async Task<ActionResult> Index()
        {

            ProductListDashboardViewModel pld = null;
            if (TempData.ContainsKey("productlist") && TempData["productlistObj"] != null)
            {
                pld = JsonConvert.DeserializeObject<ProductListDashboardViewModel>(TempData["productlistObj"].ToString());
            }

            if (object.Equals(pld, null))
            {
                pld = new();

            }

            pld.ProductLists = await apiproductlistdashboard.GetProductsapi(109,true);

          

            if (LoginID != null)
            {
                TempData["UserName"] = LoginID;
                return View(pld);
            }
            else
            {
                return RedirectToAction("Index", "Accounts");
            }
        }

      
















        //public IActionResult Index()
        //{
          //  ProductListDashboardViewModel pld = null;
            //pld = new(); 
          
            //pld.





            //for (int i = 1; i <= 15; i++)
            //{
                
                //pld.ProductLists.Add(new Product()
                //{
                  //  ProductID = i,
                //    ProductCode = "productcode" + i
              //  }); 
            //}
              //  if (LoginID != null)
           // {
             //   TempData["UserName"] = LoginID;
               // return View(pld);
            //}
            //else
            //{
              //  return RedirectToAction("Index", "Accounts");
            //}
           

        }


    
    }

