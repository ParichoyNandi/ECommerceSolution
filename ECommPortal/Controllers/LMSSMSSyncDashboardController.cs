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
    public class LMSSMSSyncDashboardController : Controller
    {
        //private IPlanPublishDeactiveService apiplanpublishdeactiveservice;
        private IHttpContextAccessor _httpContext;
        private string LoginID;

        public LMSSMSSyncDashboardController(IHttpContextAccessor httpContext)
        {

            _httpContext = httpContext;
            if (_httpContext.HttpContext.Session.Keys.Contains("LoginID"))
                LoginID = _httpContext.HttpContext.Session.GetString("LoginID");
        }


        public IActionResult Index()
        {
            LMSSMSSyncDashboardViewModel lssd = new();
            lssd = null;
            if (LoginID != null)
            {
                TempData["UserName"] = LoginID;
                return View(lssd);
            }
            else
            {
                return RedirectToAction("Index", "Accounts");
            }

        }

        public ActionResult Create(LMSSMSSyncDashboardViewModel lssd)
        {

            TempData[""] = JsonConvert.SerializeObject(lssd);
            return RedirectToAction("Index");
        }


    }
}
