using ECommPortal.Models;
using ECommPortal.Services.Interface;
using Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities;

namespace ECommPortal.Controllers
{
    public class AccountsController : Controller
    {
        private ISession session;
        private IDBAccess _data;
        private IAccountService apiservice;

        public AccountsController(IHttpContextAccessor httpContextAccessor, IDBAccess data, IAccountService apiserviceobj)
        {
            this.session = httpContextAccessor.HttpContext.Session;
            //_account = Account
            _data = data;
            apiservice = apiserviceobj;
        }
        // GET: /<controller>/
        [HttpGet]
        public IActionResult Index(AccountsViewModel avm)
        {
            //id = "1";
            //if (id!=null)
            //{
            //    this.session.SetString("SessionID",id);
            //}
            //string a = this.session.GetString("SessionID");
            //return RedirectToAction("Index", "Home", new { area = "" });
            return View(avm);
        }

        [HttpPost]
        public async Task<ActionResult> ValidateUser(AccountsViewModel avm)
        {
            int UserID = 0;
            //avm.LoginID = LoginID;
            avm.Password = Helper.Encrypt(avm.Password);
            avm.Password = avm.Password.Replace("+", "%2B");
            //UserID = _data.ValidateUser(LoginID, avm.Password);
            var dataobj = await apiservice.Validateuserapi(avm.LoginID, avm.Password);
            UserID = dataobj;
            if (UserID > 0)
            {
                
                HttpContext.Session.SetString("LoginID", avm.LoginID);
                TempData["UserName"] = avm.LoginID;
                return RedirectToAction("Index", "Dashboard", new { area = "" });
            }

            else
            {
                avm.IsValid = false;
                return RedirectToAction("Index", avm);

            }
            
            
        }
    }
}
