using ECommPortal.Models;
using ECommPortal.Services;
using ECommPortal.Services.Interface;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommPortal.Controllers
{
    public class PlanCouponMappingsController : Controller
    {
        private IPlanCouponMapService apiplancouponmap;
        private IHttpContextAccessor _httpContext;
        private string LoginID;

        public PlanCouponMappingsController(IPlanCouponMapService plan, IHttpContextAccessor httpContext)
        {
            apiplancouponmap = plan;
            _httpContext = httpContext;
            if (_httpContext.HttpContext.Session.Keys.Contains("LoginID"))
                LoginID = _httpContext.HttpContext.Session.GetString("LoginID");
        }
        // GET: PlanCouponMappingsController
        public async Task<ActionResult> Index(CreateCouponPlanMapModel ccpm)
        {

            if (object.Equals(ccpm, null))
            {
                ccpm = new();
            }
            //PlanCouponMapService apiplancouponmap = new PlanCouponMapService();

            //CreateCouponPlanMapModel ccpm = new();
            //plan category
            ccpm.CouponCategoryList = await apiplancouponmap.CouponCategoryapi();
            ccpm.BrandList = await apiplancouponmap.BrandListsApi();
            //int s = 0;
            // // plan api
            //ccpm.PlanList = await apiplancouponmap.Planapi(s);
            if (LoginID != null)
            {
                TempData["UserName"] = LoginID;
                return View(ccpm);
            }
            else
            {
                return RedirectToAction("Index", "Accounts");
            }
        }

        // GET: PlanCouponMappingsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PlanCouponMappingsController/Create
        public async Task<ActionResult> Create(CreateCouponPlanMapModel ccpm)
        {

            // List<CouponPlanMapServiceProperty> cpsp = new();

            //int s = 1;
            // plan api
            ccpm.PlanList = await apiplancouponmap.Planapi(ccpm.couponId, ccpm.ChoosenBranID);
            if (ccpm.couponId > 0)
            {
                ccpm.PlanList = await apiplancouponmap.Planapi(ccpm.couponId,ccpm.ChoosenBranID);
            }
            //string status = "true";

            //for (int i = 0; i < ccpm.ChoosedPlanList.Count; i++)
            //{

            //    if (ccpm.ChoosedPlanList[i].statusvalue)
            //    {
            //        cpsp.Add(new CouponPlanMapServiceProperty()
            //        {
            //            validFrom = ccpm.ValidFrom,
            //            validTo = ccpm.ValidTo,
            //            couponID = (int)ccpm.couponId,
            //            planID = (int)ccpm.ChoosedPlanList[i].ChoosedPlanID

            //        });

            //    }

            //}

            //PlanCouponMapService apiplancoupon = new();


            // var dataobj = await apiplancouponmap.CouponPlanapi(cpsp);


            //string sta = "OK";
            //string msg = "Success";
            //string fmsg = "Failure";

            //if (String.Equals(dataobj.Status, sta) && String.Equals(dataobj.Message, msg))
            //{
            //    ccpm.Responseflag = 0;
            //    ccpm.message = "Plans & Coupon Successfuly Mapped";
            //}
            //else
            //{
            //    if (String.Equals(dataobj.Status, sta) && String.Equals(dataobj.Message, fmsg))
            //    {
            //        ccpm.Responseflag = 1;
            //        ccpm.message = "Plans & Coupon Already Mapped";
            //    }
            //    else
            //    {
            //        ccpm.Responseflag = 1;
            //        ccpm.message = "Discount Course Center Not Successfuly Mapped";
            //    }
            //}

            return View(ccpm);
            //return RedirectToAction("Index", ccpm);

        }

        public async Task<ActionResult> Save(CreateCouponPlanMapModel ccpm)
        {

            //string status = "true";
            List<CouponPlanMapServiceProperty> cpsp = new();
            for (int i = 0; i < ccpm.ChoosedPlanList.Count; i++)
            {

                if (ccpm.ChoosedPlanList[i].statusvalue)
                {
                    cpsp.Add(new CouponPlanMapServiceProperty()
                    {
                        validFrom = ccpm.ValidFrom,
                        validTo = ccpm.ValidTo,
                        couponID = (int)ccpm.couponId,
                        planID = (int)ccpm.ChoosedPlanList[i].ChoosedPlanID

                    });

                }

            }

           // PlanCouponMapService apiplancoupon = new();


            var dataobj = await apiplancouponmap.CouponPlanapi(cpsp);


            string sta = "OK";
            string msg = "Success";
            string fmsg = "Failure";

            if (String.Equals(dataobj.Status, sta) && String.Equals(dataobj.Message, msg))
            {
                ccpm.Responseflag = 0;
                ccpm.message = "Plans & Coupon Successfuly Mapped";
            }
            else
            {
                if (String.Equals(dataobj.Status, sta) && String.Equals(dataobj.Message, fmsg))
                {
                    ccpm.Responseflag = 1;
                    ccpm.message = "Plans & Coupon Already Mapped";
                }
                else
                {
                    ccpm.Responseflag = 1;
                    ccpm.message = "Discount Course Center Not Successfuly Mapped";
                }
            }

            return RedirectToAction("Index", ccpm);
            //return View();
        }

        // POST: PlanCouponMappingsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
       
        // GET: PlanCouponMappingsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PlanCouponMappingsController/Edit/5
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

        // GET: PlanCouponMappingsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PlanCouponMappingsController/Delete/5
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
