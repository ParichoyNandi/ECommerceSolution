using ECommPortal.Models;
using ECommPortal.Models.ValueObjects;
using ECommPortal.Services;
using ECommPortal.Services.Interface;
using ECommPortal.Services.ValueObjectsService;
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
    public class ProductMappingsController : Controller
    {
        private IProductMapService apiproductmapservice;
        private IHttpContextAccessor _httpContext;
        private string LoginID;

        public ProductMappingsController(IProductMapService productmap, IHttpContextAccessor httpContext)
        {
            apiproductmapservice = productmap;
            _httpContext = httpContext;
            if (_httpContext.HttpContext.Session.Keys.Contains("LoginID"))
                LoginID = _httpContext.HttpContext.Session.GetString("LoginID");
        } // GET: ProductMappingsController
        public async Task<ActionResult> Index()
        {

            ProductMappingsViewModel pmvm = null;
            //AllDiscountServices apidiscount = new AllDiscountServices();
            if (TempData.ContainsKey("productmapObj") && TempData["productmapObj"] != null)
            {
                pmvm = JsonConvert.DeserializeObject<ProductMappingsViewModel>(TempData["productmapObj"].ToString());
            }
            //Console.WriteLine(x);
            if (object.Equals(pmvm, null))
            {
                pmvm = new();
                
            }

            pmvm.BrandList = await apiproductmapservice.BrandListsApi();       

            if (pmvm.ChoosenBrandId > 0)
            {
                pmvm.ProductLists = await apiproductmapservice.GetProductsapi(pmvm.ChoosenBrandId,false);
                pmvm.CenterLists = await apiproductmapservice.ProductCenterapi(pmvm.ChoosenBrandId);
                pmvm.ExamCategoryLists = await apiproductmapservice.GetExamCategoryapi(pmvm.ChoosenBrandId);
            }
            if (LoginID != null)
            {
                TempData["UserName"] = LoginID;
                return View(pmvm);
            }
            else
            {
                return RedirectToAction("Index", "Accounts");
            }
        }

        // GET: ProductMappingsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        public async Task<ActionResult> Search(ProductMappingsViewModel pmvm)
        {
           
            TempData["productmapObj"] = JsonConvert.SerializeObject(pmvm);
            return RedirectToAction("Index");
        }


        // GET: ProductMappingsController/Create
        public async Task<ActionResult> Create(ProductMappingsViewModel pmvm)
        {
            ProductMapServiceProperty choosenproductmap = new();
            choosenproductmap.productID = pmvm.ProductID;
            choosenproductmap.createdBy = LoginID;
            if (pmvm.ChoosenExamCategoryLists != null)
            {
                for (int i = 0; i < pmvm.ChoosenExamCategoryLists.Count; i++)
                {
                    if (i == 0)
                    {
                        choosenproductmap.examCategoryList = "" + pmvm.ChoosenExamCategoryLists[i];
                    }
                    else
                    {
                        choosenproductmap.examCategoryList = choosenproductmap.examCategoryList + "," + pmvm.ChoosenExamCategoryLists[i];
                    }
                }
            }
            else
            {
                choosenproductmap.examCategoryList = "";
            }

            for (int i = 0; i < pmvm.ChoosenCentreFeePlanLists.Count; i++)
            {
                if (pmvm.ChoosenCentreFeePlanLists[i].CentreID > 0 && pmvm.ChoosenCentreFeePlanLists[i].FeePlanID > 0)
                {
                    choosenproductmap.centerFeePlanDetails.Add(new CentreFeePlanMapServiceProperty()
                    {
                        feePlanID = pmvm.ChoosenCentreFeePlanLists[i].FeePlanID,
                        centerID=pmvm.ChoosenCentreFeePlanLists[i].CentreID,
                        validFrom=pmvm.ChoosenCentreFeePlanLists[i].ValidFrom,
                        validTo=pmvm.ChoosenCentreFeePlanLists[i].ValidTo
                    }) ;
      
                }
            }

            
           
            if (pmvm.ChoosenExamCategoryLists != null && choosenproductmap.centerFeePlanDetails.Count > 0)
            {
               
                
                    var dataobj = await apiproductmapservice.SaveProductMapping(choosenproductmap);

                    string status = "Created";
                    string msg = "Success";

                    if (String.Equals(dataobj.Status, status) && String.Equals(dataobj.Message, msg))
                    {
                        pmvm.Responseflag = 0;
                        pmvm.message = "Product Successfuly Mapped";
                    }
                    else
                    {
                        pmvm.Responseflag = 1;
                        pmvm.message = "Product Not Successfuly Mapped";
                    }
                

            }
            else
            {
                pmvm.message = "";

                if (pmvm.ChoosenExamCategoryLists == null)
                {
                    pmvm.Responseflag = 1;
                    pmvm.message = "Please Select Exam Category ";
                }
                if(choosenproductmap.centerFeePlanDetails.Count <= 0)
                {
                    pmvm.Responseflag = 1;
                    pmvm.message = pmvm.message+"Please Fill Up Center FeePlans";
                }
            }

            pmvm.ChoosenCentreFeePlanLists.RemoveRange(0, pmvm.ChoosenCentreFeePlanLists.Count);

            TempData["productmapObj"] = JsonConvert.SerializeObject(pmvm);

            return RedirectToAction("Index");
        }

       

        // GET: ProductMappingsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProductMappingsController/Edit/5
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

        // GET: ProductMappingsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProductMappingsController/Delete/5
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
