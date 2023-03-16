using ECommPortal.Models;
using ECommPortal.Services;
using ECommPortal.Services.Interface;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommPortal.Controllers
{
    public class ProductPublishDeactiveController : Controller
    {
        private IProductPublishDeactiveService apiproductlistingservice;
        private IHttpContextAccessor _httpContext;
        private string LoginID;

        public ProductPublishDeactiveController(IProductPublishDeactiveService productListing, IHttpContextAccessor httpContext)
        {
            apiproductlistingservice = productListing;
            _httpContext = httpContext;
            if (_httpContext.HttpContext.Session.Keys.Contains("LoginID"))
                LoginID = _httpContext.HttpContext.Session.GetString("LoginID");
        } // GET: ProductMappingsController
          // GET: ProductListingController
        public async Task<ActionResult> Index()
        {

            ProductPublishDeactiveViewModel plvm = null;
            
            if (TempData.ContainsKey("productlistObj") && TempData["productlistObj"] != null)
            {
                plvm = JsonConvert.DeserializeObject<ProductPublishDeactiveViewModel>(TempData["productlistObj"].ToString());
            }
            //Console.WriteLine(x);
            if (object.Equals(plvm, null))
            {
                plvm = new();

            }

            plvm.BrandList = await apiproductlistingservice.BrandListsApi();

            if (plvm.ChoosenBrandId > 0)
            {
                //List<Product> FalsePublishedProductlists = new();
                plvm.ProductLists = await apiproductlistingservice.GetPublishedDeactivetedProductsapi(plvm.ChoosenBrandId);
                //FalsePublishedProductlists = await apiproductlistingservice.GetProductsapi(plvm.ChoosenBrandId, false);
                //if (FalsePublishedProductlists != null)
                //{
                //    plvm.ProductLists = plvm.ProductLists.Concat(FalsePublishedProductlists).ToList();
                //}


            }

            if (LoginID != null)
            {
                TempData["UserName"] = LoginID;
                return View(plvm);
            }
            else
            {
                return RedirectToAction("Index", "Accounts");
            }
        }

        // GET: ProductListingController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProductListingController/Create
        public ActionResult Create(ProductPublishDeactiveViewModel plvm)
        {

            TempData["productlistObj"] = JsonConvert.SerializeObject(plvm);
            return RedirectToAction("Index");
        }


        public ActionResult Save(ProductPublishDeactiveViewModel plvm)
        {

            //TempData["productlistObj"] = JsonConvert.SerializeObject(plvm);
            return RedirectToAction("Index");
        }



        // GET: ProductListingController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProductListingController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductListingController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProductListingController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
