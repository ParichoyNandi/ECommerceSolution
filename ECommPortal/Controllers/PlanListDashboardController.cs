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
      public ActionResult EditPlan(int planId)
        {
            //plnd.GetPlanDetails = await apiplanlistdashboard.GetPlansDetailsApi(1);
            PlanListDashboardViewModel planListDashboardView = new PlanListDashboardViewModel();

            planListDashboardView.GetPlanDetails = ecommPortalDBAccess.GetPlanDetails(planId);

            return View(planListDashboardView.GetPlanDetails);
        }

        [HttpGet]
        public  ActionResult EditConfig(int planId)
        {
            //plnd.GetPlanDetails = await apiplanlistdashboard.GetPlansDetailsApi(1);
            PlanListDashboardViewModel planListDashboardView = new PlanListDashboardViewModel();

            planListDashboardView.GetPlanDetails =  ecommPortalDBAccess.GetPlanDetails(planId);
            
                return View(planListDashboardView.GetPlanDetails);
        }
        //[HttpPost]

        //public Task<ActionResult> EditConfig(PlanListDashboardViewModel plnd)
        //{
        //    //plnd.GetPlanDetails = await apiplanlistdashboard.GetPlansDetailsApi(1);
        //    Plan plan = new Plan();

        //    plnd.GetPlanDetails = ecommPortalDBAccess.GetPlanDetails();


        //    if (LoginID != null)
        //    {
        //        TempData["UserName"] = LoginID;
        //        return View("EditStats", plnd);
        //    }
        //    else
        //    {
        //        return RedirectToAction("Index", "Accounts");
        //    }
        //}

        public async Task<ActionResult> UpdateConfig(PlanListDashboardViewModel plnd)
        {
            //plnd.GetPlanDetails = await apiplanlistdashboard.GetPlansDetailsApi(1);
            Plan plan = new Plan();

            plnd.UpdatePlanDetails = ecommPortalDBAccess.UpdatePlanDetails(plan);


            if (LoginID != null)
            {
                TempData["UserName"] = LoginID;
                return View("EditStats", plnd);
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

