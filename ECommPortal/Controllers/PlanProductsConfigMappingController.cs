using ECommPortal.Models;
using ECommPortal.Services;
using ECommPortal.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommPortal.Controllers
{
    public class PlanProductsConfigMappingController : Controller
    {

        private IPlanProductsConfigMappingService apiplanproductconfigmap;
        private IHttpContextAccessor _httpContext;
        private string LoginID;

        public PlanProductsConfigMappingController(IPlanProductsConfigMappingService apiplanproductconfigmapobj, IHttpContextAccessor httpContext)
        {
            apiplanproductconfigmap = apiplanproductconfigmapobj;
            _httpContext = httpContext;
            if (_httpContext.HttpContext.Session.Keys.Contains("LoginID"))
                LoginID = _httpContext.HttpContext.Session.GetString("LoginID");
        }

        // GET: PlanProductsConfigMappingController
        public async Task<ActionResult> Index()
        {

            PlanProductsConfigMappingViewModel ppcmvm = null;
            //AllDiscountServices apidiscount = new AllDiscountServices();
            if (TempData.ContainsKey("planproductconfigObj") && TempData["planproductconfigObj"] != null)
            {
                ppcmvm = JsonConvert.DeserializeObject<PlanProductsConfigMappingViewModel>(TempData["planproductconfigObj"].ToString());
            }
            //Console.WriteLine(x);
            if (object.Equals(ppcmvm, null))
            {
                ppcmvm = new();
            }


            ppcmvm.BrandLists = await apiplanproductconfigmap.BrandListsApi();

            if (ppcmvm.ChoosenBrandID > 0)
            {
                ppcmvm.PlanLists = await apiplanproductconfigmap.GetPlanListsApi(""+ppcmvm.ChoosenBrandID);
                ppcmvm.ProductLists = await apiplanproductconfigmap.GetProductsapi(ppcmvm.ChoosenBrandID,true);

                ppcmvm.SubHeaderLists = await apiplanproductconfigmap.GetPlanSubHeaderApi();
                ppcmvm.HeaderLists = await apiplanproductconfigmap.GetPlanHeaderApi();

                ppcmvm.ConfigLists = await apiplanproductconfigmap.ConfigListsApi();

            }
            if (LoginID != null)
            {
                TempData["UserName"] = LoginID;
                return View(ppcmvm);
            }
            else
            {
                return RedirectToAction("Index", "Accounts");
            }


        }

        // GET: PlanProductsConfigMappingController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PlanProductsConfigMappingController/Create
        public ActionResult Search(PlanProductsConfigMappingViewModel ppcmvm)
        {
            TempData["planproductconfigObj"] = JsonConvert.SerializeObject(ppcmvm);
            return RedirectToAction("Index");
        }


        public async Task<ActionResult> Save(PlanProductsConfigMappingViewModel ppcmvm)
        {

            List<PlanConfigMappingServiceProperty> planconfigmap = new();
            PlanProductsMappingServiceProperty planproductmap = new();

            for (int i=0;i<ppcmvm.ChoosenConfigLists.Count;i++)
            {
                if (ppcmvm.ChoosenConfigLists[i].ConfigID > 0 && ppcmvm.ChoosenConfigLists[i].ConfigDefaultValue != null && ppcmvm.ChoosenConfigLists[i].ConfigName != null)
                {

                    planconfigmap.Add(new PlanConfigMappingServiceProperty()
                    {
                        planID = ppcmvm.ChoosenPlanID,
                        configID = ppcmvm.ChoosenConfigLists[i].ConfigID,
                        configValue = ppcmvm.ChoosenConfigLists[i].ConfigDefaultValue,
                        displayName = ppcmvm.ChoosenConfigLists[i].ConfigName

                    }) ;

                 }
            }
            /*if (ppcmvm.SummeryConfigValue != null)*/
            ppcmvm.summarydetails.DownloadLink = await apiplanproductconfigmap.StudyPlanDownloadLink(ppcmvm.ProductStudyPlanIFile);
            if (ppcmvm.summarydetails.ItemList.Last().Title == null || ppcmvm.summarydetails.ItemList.Last().Title == "" || ppcmvm.summarydetails.ItemList.Last().Description == null || ppcmvm.summarydetails.ItemList.Last().Title == "")
            {
                ppcmvm.summarydetails.ItemList.RemoveAt(ppcmvm.summarydetails.ItemList.Count - 1);
            }
            string jsonString = JsonConvert.SerializeObject(ppcmvm.summarydetails);

            if (jsonString != null || jsonString != "")
            {
                ppcmvm.ConfigLists = await apiplanproductconfigmap.ConfigListsApi();
                foreach (var config in ppcmvm.ConfigLists.Where(n => n.ConfigCode == "SUM"))
                {
                    planconfigmap.Add(new PlanConfigMappingServiceProperty()
                    {

                        planID = ppcmvm.ChoosenPlanID,
                        configID = config.ConfigID,
                        configValue = jsonString,
                        displayName = config.ConfigName


                    });
                }
            }

            var dataobj = await apiplanproductconfigmap.SavePlanConfigMapping(planconfigmap);

            string status = "Created";
            string msg = "Success";

            if (String.Equals(dataobj.Status, status) /*&& String.Equals(dataobj.Message, msg)*/)
            {
                if (String.Equals(dataobj.Message, msg))
                {
                    ppcmvm.Responseflag = 0;
                    ppcmvm.message = "Plan Config Successfuly Mapped";
                }
                else
                {
                    if (dataobj.Message.Contains(msg))
                    {
                        ppcmvm.Responseflag = 0;
                        ppcmvm.message = "Plan Config Partial Successfuly Mapped";

                    }
                    else
                    {
                        ppcmvm.Responseflag = 1;
                        ppcmvm.message = dataobj.Message;

                    }
                }
            }
            else
            {
                ppcmvm.Responseflag = 1;
                ppcmvm.message = "Plan Config Not Successfuly Mapped";
            }
            
            for (int i = 0; i < ppcmvm.ChoosenProductLists.Count; i++)
            {

                if (ppcmvm.ChoosenProductLists[i].statusvalue == true)
                {
                    if (planproductmap.ProductIDList == null)
                    {
                        planproductmap.ProductIDList = "" + ppcmvm.ChoosenProductLists[i].ProductDetails.ProductID;
                    }
                    else
                    {

                        planproductmap.ProductIDList= planproductmap.ProductIDList+","+ppcmvm.ChoosenProductLists[i].ProductDetails.ProductID;
                    }

                }
            
            }
            status = "OK";
            msg = "Success";

            planproductmap.PlanID = ppcmvm.ChoosenPlanID;
            var dataplanmapobj = await apiplanproductconfigmap.SavePlanproductMapping(planproductmap);
            if (dataplanmapobj != null)
            {

                if (String.Equals(dataplanmapobj.Status, status) /*&& String.Equals(dataobj.Message, msg)*/)
                {
                    if (String.Equals(dataplanmapobj.Message, msg))
                    {
                        if (ppcmvm.message == null)
                        {
                            ppcmvm.Responseflag = 0;
                            ppcmvm.message = "Plan products Successfuly Mapped";
                        }
                        else
                        {
                            ppcmvm.Responseflag = 0;
                            ppcmvm.message = ppcmvm.message + " And Plan products Successfuly Mapped";
                        }
                    }
                    else
                    {
                        if (dataplanmapobj.Message.Contains(msg))
                        {

                            if (ppcmvm.message == null)
                            {
                                ppcmvm.Responseflag = 0;
                                ppcmvm.message = "Plan products Partial Successfuly Mapped";

                            }
                            else
                            {
                                ppcmvm.Responseflag = 0;
                                ppcmvm.message = ppcmvm.message + " And Plan products Partial Successfuly Mapped";
                            }

                        }
                        else
                        {
                            if (ppcmvm.message == null)
                            {

                                ppcmvm.Responseflag = 1;
                                ppcmvm.message = dataplanmapobj.Message;
                            }
                            else
                            {
                                ppcmvm.Responseflag = 1;
                                ppcmvm.message = ppcmvm.message + "And " + dataplanmapobj.Message;
                            }

                        }
                    }
                }
                else
                {
                    if (ppcmvm.message == null)
                    {
                        ppcmvm.Responseflag = 1;
                        ppcmvm.message = "Plan Products Not Successfuly Mapped";
                    }
                    else
                    {
                        ppcmvm.Responseflag = 1;
                        ppcmvm.message = ppcmvm.message + " And Plan Products Not Successfuly Mapped";
                    }
                }
            }
            else
            {
                if (ppcmvm.message == null)
                {
                    ppcmvm.Responseflag = 1;
                    ppcmvm.message = "Plan Products Not Successfuly Mapped";
                }
                else
                {
                    ppcmvm.Responseflag = 1;
                    ppcmvm.message = ppcmvm.message + " And Plan Products Not Successfuly Mapped";
                }

            }

            ppcmvm.ChoosenConfigLists.RemoveRange(0, ppcmvm.ChoosenConfigLists.Count);
            ppcmvm.ConfigLists.RemoveRange(0,ppcmvm.ConfigLists.Count);
            ppcmvm.ProductStudyPlanIFile = null;
            ppcmvm.summarydetails = new();
            TempData["planproductconfigObj"] = JsonConvert.SerializeObject(ppcmvm);
            return RedirectToAction("Index");
        }


        // GET: PlanProductsConfigMappingController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PlanProductsConfigMappingController/Edit/5
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

        // GET: PlanProductsConfigMappingController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PlanProductsConfigMappingController/Delete/5
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
