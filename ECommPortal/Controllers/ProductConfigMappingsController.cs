using ECommPortal.Models;
using ECommPortal.Models.ValueObjects;
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
    public class ProductConfigMappingsController : Controller
    {

        private IProductConfigMappingService apiproductconfigmapservice;
        private IHttpContextAccessor _httpContext;
        private string LoginID;

        public ProductConfigMappingsController(IProductConfigMappingService productconfigmap, IHttpContextAccessor httpContext)
        {
            apiproductconfigmapservice = productconfigmap;
            _httpContext = httpContext;
            if (_httpContext.HttpContext.Session.Keys.Contains("LoginID"))
                LoginID = _httpContext.HttpContext.Session.GetString("LoginID");
        } // GET: ProductMappingsController
        public async Task<ActionResult> Index()
        {

            ProductConfigMappingsViewModel pcmvm = null;
            //AllDiscountServices apidiscount = new AllDiscountServices();
           if (TempData.ContainsKey("productconfigmapObj") && TempData["productconfigmapObj"] != null)
           {
                pcmvm = JsonConvert.DeserializeObject<ProductConfigMappingsViewModel>(TempData["productconfigmapObj"].ToString());
            }
          
            if (object.Equals(pcmvm, null))
            {
                pcmvm = new();

            }

            pcmvm.BrandList = await apiproductconfigmapservice.BrandListsApi();

            pcmvm.ProductLists = await apiproductconfigmapservice.GetProductsapi(pcmvm.ChoosenBrandId,false);

            pcmvm.SubHeaderLists = await apiproductconfigmapservice.GetProductSubHeaderApi();
            pcmvm.HeaderLists = await apiproductconfigmapservice.GetProductHeaderApi();

            pcmvm.ConfigLists = await apiproductconfigmapservice.ConfigListsApi();

            if (LoginID != null)
            {
                TempData["UserName"] = LoginID;
                return View(pcmvm);
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

        public async Task<ActionResult> Search(ProductConfigMappingsViewModel pcmvm)
        {

            TempData["productconfigmapObj"] = JsonConvert.SerializeObject(pcmvm);
            return RedirectToAction("Index");
        }


        // GET: ProductMappingsController/Create
        public async Task<ActionResult> Create(ProductConfigMappingsViewModel pcmvm)
        {

            List<ProductConfigMappingServiceProperty> choosenproductconfiglist = new();
            int headerid = 0;
            int subheaderid = 0;
            string headername = null;
            string subheadername = null;
            if (pcmvm.ChoosenHeader.HeaderID > 0)
            {
                headerid = pcmvm.ChoosenHeader.HeaderID;
                headername = pcmvm.ChoosenHeader.HeaderDisplayName;
            }

            if (pcmvm.ChoosenSubHeader.SubHeaderID > 0)
            {
                subheaderid = pcmvm.ChoosenSubHeader.SubHeaderID;
                subheadername = pcmvm.ChoosenSubHeader.SubHeaderDisplayName;
            }

            for (int i = 0; i < pcmvm.ChoosenConfigLists.Count; i++)
            {
                if (pcmvm.ChoosenConfigLists[i].ConfigID > 0 && pcmvm.ChoosenConfigLists[i].ConfigName != null && pcmvm.ChoosenConfigLists[i].ConfigDefaultValue != null)
                {
                    choosenproductconfiglist.Add(new ProductConfigMappingServiceProperty()
                    {

                        productID = pcmvm.ChoosenProductID,
                        configID = pcmvm.ChoosenConfigLists[i].ConfigID,
                        configDisplayName = pcmvm.ChoosenConfigLists[i].ConfigName,
                        configValue = pcmvm.ChoosenConfigLists[i].ConfigDefaultValue,
                        headerID = headerid,
                        headerDisplayName = headername,
                        subHeaderID = subheaderid,
                        subHeaderDisplayName= subheadername


                    });
                    
                    
                }
            }

              
            pcmvm.summarydetails.DownloadLink = await apiproductconfigmapservice.StudyPlanDownloadLink(pcmvm.ProductStudyPlanIFile);
            if (pcmvm.summarydetails.ItemList.Last().Title == null || pcmvm.summarydetails.ItemList.Last().Title == "" || pcmvm.summarydetails.ItemList.Last().Description == null || pcmvm.summarydetails.ItemList.Last().Title == "")
            {
                pcmvm.summarydetails.ItemList.RemoveAt(pcmvm.summarydetails.ItemList.Count - 1);
            }
            string jsonString = JsonConvert.SerializeObject(pcmvm.summarydetails);

            if (jsonString != null || jsonString != "")
            {
                pcmvm.ConfigLists = await apiproductconfigmapservice.ConfigListsApi();
                foreach (var config in pcmvm.ConfigLists.Where(n=>n.ConfigCode == "SUM"))
                {
                    choosenproductconfiglist.Add(new ProductConfigMappingServiceProperty()
                    {

                        productID = pcmvm.ChoosenProductID,
                        configID = config.ConfigID,
                        configDisplayName = config.ConfigName,
                        configValue = jsonString,
                        headerID = headerid,
                        headerDisplayName = headername,
                        subHeaderID = subheaderid,
                        subHeaderDisplayName = subheadername


                    });
                }
            }
            else
            {
                pcmvm.ConfigLists = await apiproductconfigmapservice.ConfigListsApi();
                foreach (var config in pcmvm.ConfigLists.Where(n => n.ConfigCode == "SUM"))
                {
                    choosenproductconfiglist.Add(new ProductConfigMappingServiceProperty()
                    {

                        productID = pcmvm.ChoosenProductID,
                        configID = config.ConfigID,
                        configDisplayName = config.ConfigName,
                        configValue = null,
                        headerID = headerid,
                        headerDisplayName = headername,
                        subHeaderID = subheaderid,
                        subHeaderDisplayName = subheadername


                    });
                }
            }

            var dataobj = await apiproductconfigmapservice.ProductconfigMap(choosenproductconfiglist);


            string sta = "Created";
            string msg = "Success";
            string fmsg = "Failure";

            if (String.Equals(dataobj.Status, sta) && String.Equals(dataobj.Message, msg))
            {
                pcmvm.Responseflag = 0;
                pcmvm.message = "Product and Config successfully Mapped";
            }
            else
            {
                if (String.Equals(dataobj.Status, sta))
                {
                    if (dataobj.Message.Contains("Success"))
                    {
                        pcmvm.Responseflag = 1;
                        pcmvm.message = "Some Product and Config Already Mapped - " + dataobj.Message;

                    }
                    else
                    {
                        pcmvm.Responseflag = 1;
                        pcmvm.message = "Product and Config Already Mapped";
                    }
                }
                else
                {
                    pcmvm.Responseflag = 1;
                    pcmvm.message = "Product and Config Not Successfuly Mapped";
                }
            }

            pcmvm.ChoosenConfigLists.RemoveRange(0, pcmvm.ChoosenConfigLists.Count);
            pcmvm.ProductStudyPlanIFile = null;
            pcmvm.summarydetails = new();
            TempData["productconfigmapObj"] = JsonConvert.SerializeObject(pcmvm);


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

        // POST: ProductsController/Delete/5
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
