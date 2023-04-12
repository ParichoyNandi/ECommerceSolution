using ECommPortal.Models;
using ECommPortal.Services.Interface;
using Entities;
using Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Utilities;

namespace ECommPortal.Controllers
{
    public class PlanListDashboardController : Controller
    {
        private IPlanListDashboardService apiplanlistdashboard;
        private IHttpContextAccessor _httpContext;
        private string LoginID;
        private IEcommPortalDBAccess ecommPortalDBAccess;

        public PlanListDashboardController(IPlanListDashboardService planlist, IHttpContextAccessor httpContext, IEcommPortalDBAccess _ecommPortalDBAccess)
        {
            this.apiplanlistdashboard = planlist;
            ecommPortalDBAccess = _ecommPortalDBAccess;
            _httpContext = httpContext;
            if (_httpContext.HttpContext.Session.Keys.Contains("LoginID"))
                LoginID = _httpContext.HttpContext.Session.GetString("LoginID");
        }

        public async Task<ActionResult> Index()
        {

            PlanListDashboardViewModel plnd = null;

            if (TempData.ContainsKey("PlanListObj") && TempData["PlanListObj"] != null)
            {
                plnd = JsonConvert.DeserializeObject<PlanListDashboardViewModel>(TempData["PlanListObj"].ToString());
            }
            //Console.WriteLine(x);
       

            if (object.Equals(plnd, null))
            {
                plnd = new();
            }


            plnd.BrandList = await apiplanlistdashboard.BrandListsApi();

            if (plnd.ChoosenBrandID > 0)
            {
                plnd.PlanLists.RemoveRange(0, plnd.PlanLists.Count);
                string BrandList = "" + plnd.ChoosenBrandID;
                plnd.PlanLists = await apiplanlistdashboard.GetPlansapi(BrandList, plnd.ChoosenLanguageID);
            }
            else
            {
                plnd.PlanLists = await apiplanlistdashboard.GetPlansapi("109", 2);
            }

            if (LoginID != null)
            {
                TempData["UserName"] = LoginID;
                return View(plnd);
            }
            else
            {
                return RedirectToAction("Index", "Accounts");
            }
        }
       

        public async Task<ActionResult> SearchPlan(PlanListDashboardViewModel plnd) 
        {

            TempData["PlanListObj"] = JsonConvert.SerializeObject(plnd);
            return RedirectToAction("Index");

        }

        [HttpGet]
      public ActionResult EditPlan(int planid)
        {
            //plnd.GetPlanDetails = await apiplanlistdashboard.GetPlansDetailsApi(1);
            PlanListDashboardViewModel planListDashboardView = new PlanListDashboardViewModel();

            planListDashboardView.GetPlanDetails = ecommPortalDBAccess.GetPlanDetails(planid);

            return View(planListDashboardView.GetPlanDetails);
        }


        [HttpPost]
        public ActionResult EditPlan(Plan plan)
        {
            PlanListDashboardViewModel planListDashboardView = new PlanListDashboardViewModel();
            planListDashboardView.UpdatePlanDetails = ecommPortalDBAccess.UpdatePlanDetails(plan);
            planListDashboardView.GetPlanDetails = ecommPortalDBAccess.GetPlanDetails(plan.PlanID);

            if (LoginID != null)
            {
                TempData["UserName"] = LoginID;
                return View("EditConfig", planListDashboardView.GetPlanDetails);
            }
            else
            {
                return RedirectToAction("Index", "Accounts");
            }
         
        }

        [HttpGet]
        //public  ActionResult EditConfig(int planId,Plan plan)
        //{
        //    //plnd.GetPlanDetails = await apiplanlistdashboard.GetPlansDetailsApi(1);
        //    PlanListDashboardViewModel planListDashboardView = new PlanListDashboardViewModel();
        //    planListDashboardView.UpdatePlanDetails = ecommPortalDBAccess.UpdatePlanDetails(plan);
        //    planListDashboardView.GetPlanDetails =  ecommPortalDBAccess.GetPlanDetails(planId);
            
        //        return View(planListDashboardView.GetPlanDetails);
        //}
        [HttpPost]
        public ActionResult EditConfig(Plan plan)
        {

            PlanListDashboardViewModel planListDashboardView = new PlanListDashboardViewModel();
            planListDashboardView.UpdatePlanConfig = ecommPortalDBAccess.UpdatePlanConfig(plan);
            //if (LoginID != null)
            //{
            //    TempData["UserName"] = LoginID;
            //    return View("EditStats", plnd);
            //}
            //else
            //{
            //    return RedirectToAction("Index", "Accounts");
            //}
            return View(plan);
        }

       



       

    }



}

