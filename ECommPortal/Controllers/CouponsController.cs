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
    public class CouponsController : Controller
    {

        private ICouponService apicouponservice;
        private IHttpContextAccessor _httpContext;
        private string LoginID;

        public CouponsController(ICouponService coupon, IHttpContextAccessor httpContext)
        {
            apicouponservice = coupon;
            _httpContext = httpContext;
            if (_httpContext.HttpContext.Session.Keys.Contains("LoginID"))
                LoginID = _httpContext.HttpContext.Session.GetString("LoginID");
        }
        // GET: CouponsController
        public async Task<ActionResult> Index()
        {

            CreateCouponsViewModel ccvm = null;
            //AllDiscountServices apidiscount = new AllDiscountServices();
            if (TempData.ContainsKey("couponObj") && TempData["couponObj"] != null)
            {
                ccvm = JsonConvert.DeserializeObject<CreateCouponsViewModel>(TempData["couponObj"].ToString());
            }


            //CouponServices apicouponservice = new CouponServices();

            if (object.Equals(ccvm, null))
            {
                ccvm = new();
            }


            ccvm.BrandLists = await apicouponservice.BrandListsApi();

            ccvm.CouponCategoryList = await apicouponservice.CouponCategoryapi();
           

            ccvm.CouponTypeList.Add(new CouponType()
            {
                CouponTypeID = 1,
                CouponTypeDesc = "Bulk"
            });

            ccvm.CouponTypeList.Add(new CouponType()
            {
                CouponTypeID = 2,
                CouponTypeDesc = "Single"
            });

            if (LoginID != null)
            {
                TempData["UserName"] = LoginID;
                return View(ccvm); }
            else
            {
                return RedirectToAction("Index", "Accounts");
            }

           
        }

        // GET: CouponsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CouponsController/Create
        public async Task<ActionResult> Create(CreateCouponsViewModel ccvm)
        {
            String BrandListstring = null;
            for (int j = 0; j < ccvm.ChoosenBrandLists.Count; j++)
            {
                if (BrandListstring == null)
                {
                    BrandListstring = "" + ccvm.ChoosenBrandLists[j];
                }
                else
                {
                    BrandListstring = BrandListstring +","+ ccvm.ChoosenBrandLists[j];
                }
            }
                
            ccvm.DiscountSchemeForcoupon = await apicouponservice.DiscountSchemeapi(BrandListstring);

            if (ccvm.DiscountSchemeForcoupon == null)
                ccvm.DiscountSchemeForcoupon = new();

            TempData["couponObj"] = JsonConvert.SerializeObject(ccvm);

            return RedirectToAction("Index");


           
        }



        public async Task<ActionResult> Save(CreateCouponsViewModel ccvm)
        {
            String BrandListstring = null;
            for (int j = 0; j < ccvm.ChoosenBrandLists.Count; j++)
            {
                if (BrandListstring == null)
                {
                    BrandListstring = "" + ccvm.ChoosenBrandLists[j];
                }
                else
                {
                    BrandListstring = BrandListstring + "," + ccvm.ChoosenBrandLists[j];
                }
            }

            ccvm.DiscountSchemeForcoupon = await apicouponservice.DiscountSchemeapi(BrandListstring);
           
            if (ccvm.DiscountSchemeForcoupon == null)
                ccvm.DiscountSchemeForcoupon = new();


            if (ccvm.DiscountSchemeForcoupon.Count > 0)
            {
                CouponServiceProperties csp = new CouponServiceProperties();
                csp.couponCode = ccvm.CouponBasicDetailsList.CouponCode;
                csp.couponName = ccvm.CouponBasicDetailsList.CouponName;
                csp.couponDesc = ccvm.CouponBasicDetailsList.CouponDesc;
                csp.discountID = (int)ccvm.CouponBasicDetailsList.DiscountDetails.DiscountSchemeID;
                csp.couponCategoryID = (int)ccvm.CouponBasicDetailsList.CategoryDetails.CouponCategoryID;
                csp.couponTypeID = (int)ccvm.CouponBasicDetailsList.CouponTypeDetails.CouponTypeID;
                csp.greaterThanAmount = (int)ccvm.CouponBasicDetailsList.GreaterThanAmount;
                csp.customerCode = ccvm.CouponBasicDetailsList.CustomerCode;
                csp.validFrom = ccvm.CouponBasicDetailsList.ValidFrom;
                csp.validTo = ccvm.CouponBasicDetailsList.ValidTo;
                csp.couponCount = ccvm.CouponBasicDetailsList.CouponCount;

                csp.isPrivate = ccvm.CouponBasicDetailsList.IsPrivate;

                csp.brandList = BrandListstring;


                var data = await apicouponservice.Couponcreatemethod(csp);


                if (data > 0)
                {
                    ccvm.Responseflag = 0;
                    ccvm.message = "Coupon Successfuly Created";
                }
                else
                {
                    ccvm.Responseflag = 1;
                    ccvm.message = "Coupon Not Successfuly Created";
                }
            }
            else
            {
               
                ccvm.Responseflag = 1;
                ccvm.message = "Please select Correct Brands";

            }

            int count = ccvm.ChoosenBrandLists.Count;
            ccvm.ChoosenBrandLists.RemoveRange(0, count);

            TempData["couponObj"] = JsonConvert.SerializeObject(ccvm);
            return RedirectToAction("Index");

        }

            // POST: CouponsController/Create
            [HttpPost]
        [ValidateAntiForgeryToken]
        // GET: CouponsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CouponsController/Edit/5
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

        // GET: CouponsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CouponsController/Delete/5
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
