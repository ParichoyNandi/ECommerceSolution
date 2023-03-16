using ECommPortal.Models;
using ECommPortal.Models.ValueObjects;
using ECommPortal.Services;
using ECommPortal.Services.Interface;
using ECommPortal.Services.ValueObjectsService;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ECommPortal.Controllers
{
    public class AjaxController : Controller
    {
        private IPlanCouponMapService apiplancouponmap;
        private IProductsService apiproductcreation;
        private IProductMapService apiproductmap;
        private IProductConfigMappingService apiproductconfigmap;
        private IProductPublishDeactiveService apiproductpublishdeactive;
        private IPlanProductsConfigMappingService apiplanproductconfigmap;
        private IPlanPublishDeactiveService apiplanpublish;

        public AjaxController(IPlanCouponMapService plan, IProductsService product,IProductMapService productmap,IProductConfigMappingService productconfigmap, IProductPublishDeactiveService productpublishedstatus, IPlanProductsConfigMappingService apiplanproductconfigmapobj, IPlanPublishDeactiveService apiplanpublishobj)
        {
            apiplancouponmap = plan;
            apiproductcreation = product;
            apiproductmap = productmap;
            apiproductconfigmap = productconfigmap;
            apiproductpublishdeactive=productpublishedstatus;
            apiplanproductconfigmap=apiplanproductconfigmapobj;
            apiplanpublish = apiplanpublishobj;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult> AjaxMethod(int catid,int brandid)
        {
            List<Coupon> couponList = new();
            //PlanCouponMapService apiplancouponmap = new PlanCouponMapService();
            couponList=await apiplancouponmap.Couponapi(catid, brandid);
            //{
            //    new Coupon{ CouponID = 1, CouponName = "CouponName1" },
            //    new Coupon{ CouponID = 2, CouponName = "CouponName2" },
            //    new Coupon{ CouponID = 3, CouponName = "CouponName" },

            //};



            return Ok(couponList);
        }



        public async Task<ActionResult> GetCourseMethod(int brandid)
        {
            List<Course> CourseLists = new();

            //PlanCouponMapService apiplancouponmap = new PlanCouponMapService();

            CourseLists = await apiproductcreation.Courseapi(brandid);
          
            return Ok(CourseLists);
        }

        public async Task<ActionResult> GetCategoryMethod(int brandid)
        {
            List<Category> CategoryLists = new();

            //PlanCouponMapService apiplancouponmap = new PlanCouponMapService();

            CategoryLists = await apiproductcreation.Categoryapi(brandid);

            return Ok(CategoryLists);
        }

        public async Task<ActionResult> Getconfigdetails(int configid)
        {
            List<Config> configLists = new();
            Config searchedconfig = new();

            configLists = await apiproductconfigmap.ConfigListsApi();

            foreach (var configvalue in configLists)
            {
                if (configvalue.ConfigID == configid)
                {
                    searchedconfig.ConfigID = configvalue.ConfigID;
                    searchedconfig.ConfigDefaultValue = configvalue.ConfigDefaultValue;
                    searchedconfig.ConfigName = configvalue.ConfigName;

                    return Ok(searchedconfig);
                }
            }

            return Ok(searchedconfig);

        }



        public async Task<ActionResult> GetProductHeaderdetails(int headerid)
        {
            List<ProductHeaderProperty> ProductHeaderLists = new();
            ProductHeaderProperty searchedconfig = new();

            ProductHeaderLists = await apiproductconfigmap.GetProductHeaderApi();

            foreach (var Headervalue in ProductHeaderLists)
            {
                if (Headervalue.HeaderID == headerid)
                {
                    searchedconfig.HeaderID = Headervalue.HeaderID;
                    searchedconfig.HeaderDisplayName = Headervalue.HeaderDisplayName;
                    searchedconfig.HeaderCode = Headervalue.HeaderCode;

                    return Ok(searchedconfig);
                }
            }

            return Ok(searchedconfig);

        }


        public async Task<ActionResult> GetProductSubHeaderdetails(int subheaderid)
        {
            List<ProductSubHeaderProperty> ProductSubHeaderLists = new();
            ProductSubHeaderProperty searchedconfig = new();

            ProductSubHeaderLists = await apiproductconfigmap.GetProductSubHeaderApi();

            foreach (var SubHeadervalue in ProductSubHeaderLists)
            {
                if (SubHeadervalue.SubHeaderID == subheaderid)
                {
                    searchedconfig.SubHeaderID = SubHeadervalue.SubHeaderID;
                    searchedconfig.SubHeaderDisplayName = SubHeadervalue.SubHeaderDisplayName;
                    searchedconfig.SubHeaderCode = SubHeadervalue.SubHeaderCode;

                    return Ok(searchedconfig);
                }
            }

            return Ok(searchedconfig);

        }

        public async Task<ActionResult> GetFeePlanDetails(int ProductId,int CentreId)
        {


            List<ProductFeePlanProperty> ProductFeePlanLists = await apiproductmap.GetFeePlanapi(CentreId, ProductId);

            return Ok(ProductFeePlanLists);
        }

        public async Task<ActionResult> ModifyProductIsPublish(int productid, Boolean ispublished)
        {

            Responsestatus response = new();
            response = await apiproductpublishdeactive.ModifyProductIsPublishapi(productid, ispublished);
            return Ok(response);
        
        }



        public async Task<ActionResult> GetPlanHeaderdetails(int headerid)
        {
            List<PlanHeaderProperty> PlanHeaderLists = new();
            ProductHeaderProperty searchedconfig = new();

            PlanHeaderLists = await apiplanproductconfigmap.GetPlanHeaderApi();

            foreach (var Headervalue in PlanHeaderLists)
            {
                if (Headervalue.HeaderID == headerid)
                {
                    searchedconfig.HeaderID = Headervalue.HeaderID;
                    searchedconfig.HeaderDisplayName = Headervalue.HeaderDisplayName;
                    searchedconfig.HeaderCode = Headervalue.HeaderCode;

                    return Ok(searchedconfig);
                }
            }

            return Ok(searchedconfig);

        }


        public async Task<ActionResult> GetPlanSubHeaderdetails(int subheaderid)
        {
            List<PlanSubHeaderProperty> PlanSubHeaderLists = new();
            PlanSubHeaderProperty searchedconfig = new();

            PlanSubHeaderLists = await apiplanproductconfigmap.GetPlanSubHeaderApi();

            foreach (var SubHeadervalue in PlanSubHeaderLists)
            {
                if (SubHeadervalue.SubHeaderID == subheaderid)
                {
                    searchedconfig.SubHeaderID = SubHeadervalue.SubHeaderID;
                    searchedconfig.SubHeaderDisplayName = SubHeadervalue.SubHeaderDisplayName;
                    searchedconfig.SubHeaderCode = SubHeadervalue.SubHeaderCode;

                    return Ok(searchedconfig);
                }
            }

            return Ok(searchedconfig);

        }



        public async Task<ActionResult> ModifyPlanIsPublish(int planid, Boolean ispublished)
        {

            Responsestatus response = new();
            response = await apiplanpublish.ModifyPlanIsPublishapi(planid, ispublished);
            return Ok(response);

        }



    }
}
