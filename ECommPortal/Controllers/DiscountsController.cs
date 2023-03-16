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
    public class DiscountsController : Controller
    {
        private IAllDiscountService apidiscount;
        private IHttpContextAccessor _httpContext;
        private string LoginID;

        public DiscountsController(IAllDiscountService allDiscount, IHttpContextAccessor httpContext)
        {
            apidiscount = allDiscount;
            _httpContext = httpContext;
            if (_httpContext.HttpContext.Session.Keys.Contains("LoginID"))
                LoginID = _httpContext.HttpContext.Session.GetString("LoginID");
        }
        // GET: DiscountsController
        public async Task<ActionResult> Index()
        {
            CreateDiscountsViewModel cdvm = null;
            //AllDiscountServices apidiscount = new AllDiscountServices();
            if (TempData.ContainsKey("disObj") && TempData["disObj"] != null)
            {
                cdvm = JsonConvert.DeserializeObject<CreateDiscountsViewModel>(TempData["disObj"].ToString());
            }
            //Console.WriteLine(x);
            if(object.Equals(cdvm, null))
                    {
                cdvm = new();
            }
            cdvm.FeescomponentList = await apidiscount.Feescomponentapi();

            cdvm.BrandList = await apidiscount.BrandIdApi();

            if (cdvm.ChoosenDiscountBrandList.Count > 0)
            {
                for (int j = 0; j < cdvm.ChoosenDiscountBrandList.Count; j++)
                {
                    cdvm.DiscountBrandLists.Add(new CreateDiscountBrandDetails()
                    {
                        BrandID = cdvm.ChoosenDiscountBrandList[j]
                    });

                    var feecomponents = await apidiscount.Feescomponentapi(cdvm.ChoosenDiscountBrandList[j]);
                    for (int i = 0; i < feecomponents.Count; i++)
                    {
                        cdvm.DiscountBrandLists[j].DiscountSchemeLumpsumList.Add(new DiscountSchemeDetail());
                        cdvm.DiscountBrandLists[j].DiscountSchemeInstallmentList.Add(new DiscountSchemeDetail());
                    }
                }


                cdvm.BrandList = await apidiscount.BrandIdApi();

            }

            if (LoginID != null)
            {
                TempData["UserName"] = LoginID;
                return View(cdvm);
            }
            else
            {
                return RedirectToAction("Index", "Accounts");
            }
        }

        // GET: DiscountsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DiscountsController/Create
        public async Task<ActionResult> Create(CreateDiscountsViewModel cdvm)
        {


            TempData["disObj"] = JsonConvert.SerializeObject(cdvm);

            return RedirectToAction("Index");

        }




        public async Task<ActionResult> Save(CreateDiscountsViewModel cdvm)
        {
           

            DiscountServicesScheme discountobj = new();


            discountobj.DiscountSchemeName = cdvm.DiscountschemeList.DiscountSchemeName;
            discountobj.validFrom = cdvm.DiscountschemeList.ValidFrom;
            discountobj.validTo = cdvm.DiscountschemeList.ValidTo;

            discountobj.BrandList = null;

            for (int j = 0; j < cdvm.ChoosenDiscountBrandList.Count; j++)
            {
                if (discountobj.BrandList == null)
                {
                    discountobj.BrandList = "" + cdvm.ChoosenDiscountBrandList[j];
                }
                else
                {
                    discountobj.BrandList = discountobj.BrandList+"," + cdvm.ChoosenDiscountBrandList[j];
                }

                for (int i = 0; i < cdvm.DiscountBrandLists[j].DiscountSchemeLumpsumList.Count; i++)
                {

                    if ((int)cdvm.DiscountBrandLists[j].DiscountSchemeLumpsumList[i].DiscountRate != 0 || (int)cdvm.DiscountBrandLists[j].DiscountSchemeLumpsumList[i].DiscountAmount != 0)
                         {
                            discountobj.DiscountSchemeList.Add(new DiscountServicesSchemeDetails()
                              {

                                DiscountRate = (int)cdvm.DiscountBrandLists[j].DiscountSchemeLumpsumList[i].DiscountRate,
                                  DiscountAmount = (int)cdvm.DiscountBrandLists[j].DiscountSchemeLumpsumList[i].DiscountAmount,
                                 isApplicableOn = (int)cdvm.DiscountBrandLists[j].DiscountSchemeLumpsumList[i].IsApplicableOn,
                                  fromInstallment = null,
                                 feeComponentID = cdvm.DiscountBrandLists[j].DiscountSchemeLumpsumList[i].FeeComponentID

                                 });

                         }
                }

                for (int i = 0; i < cdvm.DiscountBrandLists[j].DiscountSchemeInstallmentList.Count; i++)
                {

                    if ((int)cdvm.DiscountBrandLists[j].DiscountSchemeInstallmentList[i].DiscountRate != 0 || (int)cdvm.DiscountBrandLists[j].DiscountSchemeInstallmentList[i].DiscountAmount != 0)
                    {
                        discountobj.DiscountSchemeList.Add(new DiscountServicesSchemeDetails()
                        {

                            DiscountRate = (int)cdvm.DiscountBrandLists[j].DiscountSchemeInstallmentList[i].DiscountRate,
                            DiscountAmount = (int)cdvm.DiscountBrandLists[j].DiscountSchemeInstallmentList[i].DiscountAmount,
                            isApplicableOn = (int)cdvm.DiscountBrandLists[j].DiscountSchemeInstallmentList[i].IsApplicableOn,
                            fromInstallment = cdvm.DiscountBrandLists[j].DiscountSchemeInstallmentList[i].FromInstalment,
                            feeComponentID = cdvm.DiscountBrandLists[j].DiscountSchemeLumpsumList[i].FeeComponentID

                        });

                    }
                }
            }
            discountobj.CreatedBy = LoginID;
            var data= await apidiscount.Discountcreatemethod(discountobj);

            if (data > 0)
            {
                cdvm.Responseflag = 0;
                cdvm.message = "Discount Scheme Successfuly Created";
            }
            else
            {
                cdvm.Responseflag = 1;
                cdvm.message = "Discount Scheme Not Successfuly Created";
            }

            cdvm.DiscountBrandLists.RemoveRange(0, cdvm.ChoosenDiscountBrandList.Count);
            cdvm.ChoosenDiscountBrandList.RemoveRange(0, cdvm.ChoosenDiscountBrandList.Count);

            TempData["disObj"] = JsonConvert.SerializeObject(cdvm);




            return RedirectToAction("Index");
            //return View();
        }

        // GET: DiscountsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DiscountsController/Edit/5
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

        // GET: DiscountsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DiscountsController/Delete/5
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
