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

    public class PlanController : Controller
    {


        private IPlanService apiplan;
        private IHttpContextAccessor _httpContext;
        private string LoginID;

        public PlanController(IPlanService apiplanobj, IHttpContextAccessor httpContext)
        {
            apiplan = apiplanobj;
            _httpContext = httpContext;
            if (_httpContext.HttpContext.Session.Keys.Contains("LoginID"))
                LoginID = _httpContext.HttpContext.Session.GetString("LoginID");
        }
        // GET: PlanController
        public async Task<ActionResult> Index()
        {

            CreatePlanViewModel cpvm = null;
            //AllDiscountServices apidiscount = new AllDiscountServices();
            if (TempData.ContainsKey("planObj") && TempData["planObj"] != null)
            {
                cpvm = JsonConvert.DeserializeObject<CreatePlanViewModel>(TempData["planObj"].ToString());
            }
            //Console.WriteLine(x);
            if (object.Equals(cpvm, null))
            {
                cpvm = new();
            }


            cpvm.BrandLists = await apiplan.BrandListsApi();

            if (LoginID != null)
            {
                TempData["UserName"] = LoginID;
                return View(cpvm); }
            else
            {
                return RedirectToAction("Index", "Accounts");
            }
            

          
        }

        // GET: PlanController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PlanController/Create
        public async Task<ActionResult> Create(CreatePlanViewModel cpvm)
        {
            TempData["planObj"] = JsonConvert.SerializeObject(cpvm);

            cpvm.PlanImageString = await apiplan.PlanImageLink(cpvm.PlanImageIFile);

            PlanServiceProperty pspobj = new();

            pspobj.planCode= cpvm.choosenPlan.PlanCode;
            pspobj.planName = cpvm.choosenPlan.PlanName;
            pspobj.planDesc = cpvm.PlanDesc;
            pspobj.planImage = cpvm.PlanImageString;
            pspobj.brandIDList = null;

            for (int j = 0; j < cpvm.ChoosenBrandsLists.Count; j++)
            {
                if (pspobj.brandIDList == null)
                {
                    pspobj.brandIDList = "" + cpvm.ChoosenBrandsLists[j];
                }
                else
                {
                    pspobj.brandIDList = pspobj.brandIDList + "," + cpvm.ChoosenBrandsLists[j];
                }

            }

            pspobj.validFrom = cpvm.ValidFrom;
            pspobj.validTo = cpvm.ValidTo;
            pspobj.createdBy = LoginID;
            var data = await apiplan.Plancreatemethod(pspobj);

            if (data > 0)
            {
                cpvm.Responseflag = 0;
                cpvm.message = "Plan Successfuly Created";
            }
            else
            {
                cpvm.Responseflag = 1;
                cpvm.message = "Plan Not Successfuly Created";
            }
            cpvm.PlanImageIFile = null;
            TempData["planObj"] = JsonConvert.SerializeObject(cpvm);

            return RedirectToAction("Index");
        }

     

        // GET: PlanController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PlanController/Edit/5
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

        // GET: PlanController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PlanController/Delete/5
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
