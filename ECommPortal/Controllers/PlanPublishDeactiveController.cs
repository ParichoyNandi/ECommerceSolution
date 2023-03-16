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
    public class PlanPublishDeactiveController : Controller
    {


        private IPlanPublishDeactiveService apiplanpublishdeactiveservice;
        private IHttpContextAccessor _httpContext;
        private string LoginID;

        public PlanPublishDeactiveController(IPlanPublishDeactiveService planpublishdeactive, IHttpContextAccessor httpContext)
        {
            apiplanpublishdeactiveservice = planpublishdeactive;
            _httpContext = httpContext;
            if (_httpContext.HttpContext.Session.Keys.Contains("LoginID"))
                LoginID = _httpContext.HttpContext.Session.GetString("LoginID");
        }
        // GET: PlanPublishDeactiveController
        public async Task<ActionResult> Index()
        {
            PlanPublishDeactiveViewModel ppdvm = null;

            if (TempData.ContainsKey("planpublishObj") && TempData["planpublishObj"] != null)
            {
                ppdvm = JsonConvert.DeserializeObject<PlanPublishDeactiveViewModel>(TempData["planpublishObj"].ToString());
            }
            //Console.WriteLine(x);
            if (object.Equals(ppdvm, null))
            {
                ppdvm = new();

            }

            ppdvm.BrandLists = await apiplanpublishdeactiveservice.BrandListsApi();

            if (ppdvm.ChoosenBrandLists != null)
            {

                ppdvm.PlanLists = await apiplanpublishdeactiveservice.GetPublishedDeactivetedPlansapi(ppdvm.ChoosenBrandListString);
                


            }

            if (LoginID != null)
            {
                TempData["UserName"] = LoginID;
                return View(ppdvm);
            }
            else
            {
                return RedirectToAction("Index", "Accounts");
            }
        }

        // GET: PlanPublishDeactiveController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PlanPublishDeactiveController/Create
        public ActionResult Create(PlanPublishDeactiveViewModel ppdvm)
        {

            if (ppdvm.ChoosenBrandLists == null)
            {
                ppdvm.ChoosenBrandLists = new();
            }
            else
            {
                for (int i = 0; i < ppdvm.ChoosenBrandLists.Count; i++)
                {
                    if (i == 0)
                    {

                        ppdvm.ChoosenBrandListString = "" + ppdvm.ChoosenBrandLists[i];
                    }
                    else
                    {
                        ppdvm.ChoosenBrandListString += "," + ppdvm.ChoosenBrandLists[i];
                    }
                }
            }

            TempData["planpublishObj"] = JsonConvert.SerializeObject(ppdvm);
            return RedirectToAction("Index");
        }

        public ActionResult Save(PlanPublishDeactiveViewModel ppdvm)
        {

            //TempData["productlistObj"] = JsonConvert.SerializeObject(plvm);
            return RedirectToAction("Index");
        }



        // GET: PlanPublishDeactiveController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PlanPublishDeactiveController/Edit/5
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

        // GET: PlanPublishDeactiveController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PlanPublishDeactiveController/Delete/5
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
