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

    public class ProductsController : Controller
    {


        private IProductsService apiproductservice;
        private IHttpContextAccessor _httpContext;
        private string LoginID;

        public ProductsController(IProductsService product, IHttpContextAccessor httpContext)
        {
            apiproductservice = product;
            _httpContext = httpContext;
            if (_httpContext.HttpContext.Session.Keys.Contains("LoginID"))
                LoginID = _httpContext.HttpContext.Session.GetString("LoginID");
        }
        // GET: ProductsController
        public async Task<ActionResult> Index(CreateProductsViewModel cpvm)
        {

           // CreateProductsViewModel cpvm = null;
            
           //if (TempData.ContainsKey("productObj") && TempData["productObj"] != null)
           // {
           //     cpvm = JsonConvert.DeserializeObject<CreateProductsViewModel>(TempData["productObj"].ToString());
           // }


            //CouponServices apicouponservice = new CouponServices();

            if (object.Equals(cpvm, null))
            {
                cpvm = new();
            }
           

            cpvm.BrandList = await apiproductservice.BrandListsApi();
            if (LoginID != null)
            {
                TempData["UserName"] = LoginID;
                return View(cpvm);
            }
            else
            {
                return RedirectToAction("Index", "Accounts");
            }
        }

        // GET: ProductsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProductsController/Create
        public async Task<ActionResult> Create(CreateProductsViewModel cpvm)
        {

            cpvm.ProductDetails.ProductImage = await apiproductservice.ProductImageLink(cpvm.ProductImageIFile);

            ProductsServiceProperties pspobj = new();
            pspobj.brandID = cpvm.ProductDetails.BrandID;
            pspobj.productShortDesc = cpvm.ProductDetails.ShortDesc;
            pspobj.productCode = cpvm.ProductDetails.ProductCode;
            pspobj.productName = cpvm.ProductDetails.ProductName;
            pspobj.categoryID = cpvm.ProductDetails.CategoryDetails.CategoryID;
            pspobj.courseID = cpvm.ProductDetails.CourseID;
            pspobj.validFrom = cpvm.ProductDetails.ValidFrom;
            pspobj.validTo = cpvm.ProductDetails.ValidTo;
            pspobj.productImage = cpvm.ProductDetails.ProductImage;
            pspobj.createdBy = LoginID;
            var data = await apiproductservice.Productcreatemethod(pspobj);

            if (data > 0)
            {
                cpvm.Responseflag = 0;
                cpvm.message = "Product Successfuly Created";
            }
            else
            {
                cpvm.Responseflag = 1;
                cpvm.message = "Product Not Successfuly Created";
            }



            return RedirectToAction("Index",cpvm);
        }

        // POST: ProductsController/Create
      

        // GET: ProductsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProductsController/Edit/5
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

        // GET: ProductsController/Delete/5
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
