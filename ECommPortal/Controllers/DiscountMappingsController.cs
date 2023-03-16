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
    public class DiscountMappingsController : Controller
    {
        private IDiscountCourseCenterMappingService apisccm;
        private IHttpContextAccessor _httpContext;
        private string LoginID;

        public DiscountMappingsController(IDiscountCourseCenterMappingService allDiscount, IHttpContextAccessor httpContext)
        {
            apisccm = allDiscount;
            _httpContext = httpContext;
            if (_httpContext.HttpContext.Session.Keys.Contains("LoginID"))
                LoginID = _httpContext.HttpContext.Session.GetString("LoginID");
        }
        // GET: DiscountMappingsController
        public async Task<ActionResult> Index(CreateDiscountMappingsViewModel dm)
        {

           // CreateDiscountMappingsViewModel dm = new();

            if (object.Equals(dm, null))
            {
                dm = new();
            }

            //DiscountCourseCenterMappingService apisccm = new();

            //DiscountScheme
            dm.DiscountList = await apisccm.DiscountSchemeapi();
            if (dm.DiscountList == null)
            { dm.DiscountList = new(); }
            //course
            dm.CourseList = await apisccm.Courseapi();
            //centre 
            dm.CenterList = await apisccm.Centerapi();


            if (LoginID != null)
            {
                TempData["UserName"] = LoginID;
                return View(dm);
            }
            {
                return RedirectToAction("Index", "Accounts");
            }
        }

        // GET: DiscountMappingsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DiscountMappingsController/Create
        public async Task<ActionResult> Create(CreateDiscountMappingsViewModel dm)
        {
            List<DiscountMappingProperty> dml = new();


            for (int i = 0; i < dm.CoursecenterList.Count; i++)
            {
                if (dm.CoursecenterList[i].SelectedCourseList != null)
                {
                    for (int j = 0; j < dm.CoursecenterList[i].SelectedCourseList.Count; j++)
                    {
                        dml.Add(new DiscountMappingProperty()
                        {

                            DiscountSchemeID = dm.MappedDiscountId,
                            CenterID = dm.CoursecenterList[i].Center,
                            courseID = dm.CoursecenterList[i].SelectedCourseList[j],
                            createdBy= LoginID

                        });

                    }
                }
                
            }

            //DiscountCourseCenterMappingService apidccms = new();
           
           
            var data = await apisccm.DiscountMappingmethod(dml);

            string status = "Created";
            string msg = "Success";

            if (String.Equals(data.Status, status) && String.Equals(data.Message, msg))
            {
                dm.Responseflag = 0;
                dm.message = "Discount Course Center Successfuly Mapped";
            }
            else
            {
                dm.Responseflag = 1;
                dm.message = "Discount Course Center Not Successfuly Mapped";
            }

            return RedirectToAction("Index", dm);


            
        }

        // POST: DiscountMappingsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DiscountMappingsController/Edit/5
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

        // GET: DiscountMappingsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DiscountMappingsController/Delete/5
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
