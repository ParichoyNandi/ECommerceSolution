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
    public class PlanListingController : Controller
    {

        private IPlanListingService apiplanlist;

        public PlanListingController(IPlanListingService apiplanlistobj)
        {
            apiplanlist = apiplanlistobj;
        }
        // GET: PlanListingController
        public async Task<ActionResult> Index()
        {
            PlanListingViewModel plvm = null;
            //AllDiscountServices apidiscount = new AllDiscountServices();
            if (TempData.ContainsKey("planlistObj") && TempData["planlistObj"] != null)
            {
                plvm = JsonConvert.DeserializeObject<PlanListingViewModel>(TempData["planlistObj"].ToString());
            }
            //Console.WriteLine(x);
            if (object.Equals(plvm, null))
            {
                plvm = new();
            }


            plvm.BrandLists = await apiplanlist.BrandListsApi();

            if (plvm.ChoosenBrandLists.Count > 0 && plvm.choosenBrandListString != null)
            {
                plvm.PlanLists = await apiplanlist.GetPlanListsApi(plvm.choosenBrandListString);
            }

            return View(plvm);
        }

        // GET: PlanListingController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PlanListingController/Create
        public ActionResult Create(PlanListingViewModel plvm)
        {
            if (plvm.ChoosenBrandLists == null)
            {
                plvm.ChoosenBrandLists = new();
            }
            else
            {
                for (int i = 0; i < plvm.ChoosenBrandLists.Count; i++)
                {
                    if (i == 0)
                    {

                        plvm.choosenBrandListString = "" + plvm.ChoosenBrandLists[i];
                    }
                    else
                    {
                        plvm.choosenBrandListString+= ","+plvm.ChoosenBrandLists[i];
                    }
                }
            }

            TempData["planlistObj"] = JsonConvert.SerializeObject(plvm);
            return RedirectToAction("Index");
        }


        public ActionResult Save(PlanListingViewModel plvm)
        {
            return RedirectToAction("Index");
        }


        // GET: PlanListingController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PlanListingController/Edit/5
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

        // GET: PlanListingController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PlanListingController/Delete/5
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
