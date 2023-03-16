using AutoMapper;
using ECommAPI.Models;
using Entities;
using Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ECommAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlansController : ControllerBase
    {
        private IDBAccess _data;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        //private IHttpContextAccessor _context;

        public PlansController(IDBAccess data, IMapper mapper,IConfiguration config)
        {
            _data = data;
            _mapper = mapper;
            _config = config;
            // _context = context;
        }


        [HttpGet("GetPlanList/{BrandIDList}")]
        [Produces("application/json")]
        public ActionResult<Response<List<PlanDto>>> GetPlanList(string BrandIDList, int CategoryID,string ExamCategoryIDs=null, int CouponID=0, string ExamGroupIDs=null, int ProductID = 0, int LanguageID = 0,string CustomerID=null)
        {
            List<Plan> plans = new();
            Response<List<PlanDto>> res = new();


            try
            {
                if (ExamCategoryIDs == null || ExamCategoryIDs == "")
                    ExamCategoryIDs = null;

                if (ExamGroupIDs == null || ExamGroupIDs == "")
                    ExamGroupIDs = null;


                plans = _data.GetPlanList(BrandIDList, CategoryID,ExamCategoryIDs,CouponID,ExamGroupIDs,ProductID, CustomerID);

                if (plans.Count == 0 || LanguageID < 0 )
                {
                    res.Status = HttpStatusCode.NoContent.ToString();
                    res.Message = "No data found.";
                }
                else
                {
                    res.Status = HttpStatusCode.OK.ToString();
                    res.Message = "Success";

                    //filter by language : susmita //


                    if (LanguageID == 0)
                        plans = plans.FindAll(x => x.PlanLanguageID == 2); //default bengali
                    else
                        plans = plans.FindAll(x => x.PlanLanguageID == LanguageID);


                    //filter by language : susmita //

                    res.data = new List<PlanDto>();

                    foreach (var p in plans)
                    {
                        res.data.Add(_mapper.Map<PlanDto>(p));
                    }

                }
            }
            catch (Exception ex)
            {
                res.Status = HttpStatusCode.InternalServerError.ToString();
                res.Message = ex.Message;
            }


            return Ok(res);
        }

        [HttpGet("GetPlanListForCouponMapping/{BrandIDList}")]
        [Produces("application/json")]
        public ActionResult<Response<List<PlanDto>>> GetPlanListForCouponMapping(string BrandIDList, int CategoryID, string ExamCategoryIDs = null, int CouponID = 0,int LanguageID=0)
        {
            List<Plan> plans = new();
            Response<List<PlanDto>> res = new();


            try
            {
                if (ExamCategoryIDs == null || ExamCategoryIDs == "")
                    ExamCategoryIDs = null;


                plans = _data.GetPlanListForCouponMapping(BrandIDList, CategoryID, ExamCategoryIDs, CouponID);

                if (plans.Count == 0 || LanguageID <= 0)
                {
                    res.Status = HttpStatusCode.NoContent.ToString();
                    res.Message = "No data found.";
                }
                else
                {
                    res.Status = HttpStatusCode.OK.ToString();
                    res.Message = "Success";

                    //filter by language : susmita //

                    plans = plans.FindAll(x => x.PlanLanguageID == LanguageID);

                    //filter by language : susmita //

                    res.data = new List<PlanDto>();


                    foreach (var p in plans)
                    {
                        res.data.Add(_mapper.Map<PlanDto>(p));
                    }

                }
            }
            catch (Exception ex)
            {
                res.Status = HttpStatusCode.InternalServerError.ToString();
                res.Message = ex.Message;
            }


            return Ok(res);
        }

        [HttpGet("GetPlanListForPublishing/{BrandIDList}")]
        [Produces("application/json")]
        public ActionResult<Response<List<PlanDto>>> GetPlanListForPublishing(string BrandIDList, bool CanBePublished=false,int LanguageID=0)
        {
            List<Plan> plans = new();
            Response<List<PlanDto>> res = new();


            try
            {


                plans = _data.GetPlanListForPublishing(BrandIDList, CanBePublished);

                if (plans.Count == 0 || LanguageID <= 0)
                {
                    res.Status = HttpStatusCode.NoContent.ToString();
                    res.Message = "No data found.";
                }
                else
                {
                    res.Status = HttpStatusCode.OK.ToString();
                    res.Message = "Success";

                    //filter by language : susmita //

                    plans = plans.FindAll(x => x.PlanLanguageID == LanguageID);

                    //filter by language : susmita //


                    res.data = new List<PlanDto>();

                    foreach (var p in plans)
                    {
                        res.data.Add(_mapper.Map<PlanDto>(p));
                    }

                }
            }
            catch (Exception ex)
            {
                res.Status = HttpStatusCode.InternalServerError.ToString();
                res.Message = ex.Message;
            }


            return Ok(res);
        }


        [Route("GetPlanDetails")]
        [HttpGet()]
        [Produces("application/json")]
        public ActionResult<Response<PlanProductDto>> GetPlanProducts(int PlanID)
        {
            //RequestLog log = new();
            List<PlanProduct> plans = new();
            Response<PlanProductDto> res = new();


            //log.InvokedMethod = System.Reflection.MethodBase.GetCurrentMethod().Name;
            //log.UniqueAttributeName = "PlanID";
            //log.UniqueAttributeValue = PlanID.ToString();
            //log.RequestParameters = $"PlanID: {PlanID}";


            try
            {
                
                plans = _data.GetPlanProductDetails(PlanID);

                if (plans.Count == 0)
                {
                    res.Status = HttpStatusCode.NoContent.ToString();
                    res.Message = "No data found.";
                }
                else
                {
                    res.Status = HttpStatusCode.OK.ToString();
                    res.Message = "Success";

                    res.data = new PlanProductDto();

                    res.data.PlanID = plans[0].PlanID;
                    res.data.PlanName = plans[0].PlanName;

                    //added by susmita
                    res.data.PlanLanguageID = plans[0].PlanLanguageID;
                    res.data.PlanLanguageName = plans[0].PlanLanguageName;

                    //added by susmita

                    foreach (var p in plans)
                    {
                        foreach(var c in p.ProductDetails.CenterAvailability)
                        {
                            res.data.ProductCentreDetails.Add(_mapper.Map<ProductCenterMapDto>(c));
                        }

                        
                        res.data.ProductDetails.Add(_mapper.Map<ProductDto>(p.ProductDetails));
                        
                        foreach(var c in p.PlanConfigList)
                        {
                            if(res.data.PlanConfigList.Where(w=>w.PlanConfigID==c.PlanConfigID).ToList().Count==0)
                                res.data.PlanConfigList.Add(_mapper.Map<PlanConfigDto>(c));
                        }
                    }

                    

                    //foreach (var p in plans)
                    //{
                    //    res.data.Add(_mapper.Map<PlanProductDto>(p));
                    //}

                }
            }
            catch (Exception ex)
            {
                res.Status = HttpStatusCode.InternalServerError.ToString();
                res.Message = ex.Message;

                //log.ErrorMessage = res.Message;
                //log.RequestResult = res.Status;

                //_data.SaveLogs(log);

                return Ok(res);
            }

            //log.RequestResult = res.Message;

            //if (res.Message != "Success")
            //    log.ErrorMessage = res.Message;

            //_data.SaveLogs(log);

            return Ok(res);
        }

        [Route("GetPlanDetailsAPK")]
        [HttpGet()]
        [Produces("application/json")]
        public ActionResult<Response<PlanProductDto>> GetPlanProductsForAPK(int PlanID)
        {
            //RequestLog log = new();
            List<PlanProduct> plans = new();
            Response<PlanProductDto> res = new();


            //log.InvokedMethod = System.Reflection.MethodBase.GetCurrentMethod().Name;
            //log.UniqueAttributeName = "PlanID";
            //log.UniqueAttributeValue = PlanID.ToString();
            //log.RequestParameters = $"PlanID: {PlanID}";


            try
            {

                plans = _data.GetPlanProductDetails(PlanID);

                if (plans.Count == 0)
                {
                    res.Status = HttpStatusCode.NoContent.ToString();
                    res.Message = "No data found.";
                }
                else
                {
                    res.Status = HttpStatusCode.OK.ToString();
                    res.Message = "Success";

                    res.data = new PlanProductDto();

                    res.data.PlanID = plans[0].PlanID;
                    res.data.PlanName = plans[0].PlanName;

                    //added by susmita
                    res.data.PlanLanguageID = plans[0].PlanLanguageID;
                    res.data.PlanLanguageName = plans[0].PlanLanguageName;

                    //added by susmita


                    foreach (var p in plans)
                    {
                        foreach (var c in p.ProductDetails.CenterAvailability)
                        {
                            res.data.ProductCentreDetails.Add(_mapper.Map<ProductCenterMapDto>(c));
                        }


                        res.data.ProductDetails.Add(_mapper.Map<ProductDto>(p.ProductDetails));

                        foreach (var c in p.PlanConfigList)
                        {
                            if (res.data.PlanConfigList.Where(w => w.PlanConfigID == c.PlanConfigID).ToList().Count == 0)
                                res.data.PlanConfigList.Add(_mapper.Map<PlanConfigDto>(c));
                        }
                    }

                    for(int i=0;i<res.data.ProductDetails.Count;i++)
                    {
                        var s = res.data.ProductDetails[i].ProductConfigList.Where(w => w.ConfigCode == "SUM").FirstOrDefault().ConfigValue;

                        if(!String.IsNullOrEmpty(s))
                            res.data.ProductDetails[i].SummaryDetails = JsonConvert.DeserializeObject<GetSummaryFormatDto>(s);
                    }


                    var sum = res.data.PlanConfigList.Where(w => w.ConfigCode == "SUM").FirstOrDefault().ConfigValue;
                    if (!String.IsNullOrEmpty(sum))
                        res.data.SummaryDetails = JsonConvert.DeserializeObject<GetSummaryFormatDto>(sum);

                    //foreach (var p in plans)
                    //{
                    //    res.data.Add(_mapper.Map<PlanProductDto>(p));
                    //}

                }
            }
            catch (Exception ex)
            {
                res.Status = HttpStatusCode.InternalServerError.ToString();
                res.Message = ex.Message;

                //log.ErrorMessage = res.Message;
                //log.RequestResult = res.Status;

                //_data.SaveLogs(log);

                return Ok(res);
            }

            //log.RequestResult = res.Message;

            //if (res.Message != "Success")
            //    log.ErrorMessage = res.Message;

            //_data.SaveLogs(log);

            return Ok(res);
        }

        [HttpGet("GetPlanListAPK/{BrandIDList}")]
        [Produces("application/json")]
        public ActionResult<Response<List<GetApkPlanDto>>> GetPlanListForAPK(string BrandIDList, int CategoryID, 
                                                        string ExamCategoryIDs = null, int CouponID = 0, string ExamGroupIDs = null, int ProductID=0,int LanguageID=0,string CustomerID=null)
        {
            List<Plan> plans = new();
            Response<List<GetApkPlanDto>> res = new();


            try
            {
                if (ExamCategoryIDs == null || ExamCategoryIDs == "")
                    ExamCategoryIDs = null;

                if (ExamGroupIDs == null || ExamGroupIDs == "")
                    ExamGroupIDs = null;

                if(CustomerID == null || CustomerID == "")
                    CustomerID = null;

                plans = _data.GetPlanList(BrandIDList, CategoryID, ExamCategoryIDs, CouponID,ExamGroupIDs,ProductID, CustomerID);

               

                if (plans.Count == 0 || LanguageID < 0)
                {
                    res.Status = HttpStatusCode.NoContent.ToString();
                    res.Message = "No data found.";
                }
                else
                {
                    res.Status = HttpStatusCode.OK.ToString();
                    res.Message = "Success";


                    //filter by language : susmita //

                    if (LanguageID == 0)
                        plans = plans.FindAll(x => x.PlanLanguageID == 2);
                    else
                        plans = plans.FindAll(x => x.PlanLanguageID == LanguageID);


                    //filter by language : susmita //


                    res.data = new List<GetApkPlanDto>();

                    foreach (var p in plans)
                    {
                        GetApkPlanDto plan = new();

                        plan.PlanID = p.PlanID;
                        plan.CourseName = p.PlanName;
                        plan.RedirectLink = _config.GetValue<string>("EcommWebUrl")+"plan/"+p.PlanID.ToString();
                        plan.Language = p.PlanLanguageName;
                        foreach(var c in p.ConfigList)
                        {
                            if (c.ConfigCode == "LC")
                                plan.LiveClasses = c.ConfigValue;

                            if (c.ConfigCode == "TV")
                                plan.Videos = c.ConfigValue;

                            if (c.ConfigCode == "CD")
                                plan.Duration = c.ConfigValue;

                            if (c.ConfigCode == "SML")
                                plan.StudyMetarialLanguage = c.ConfigValue; //updated by susmita 

                            if (c.ConfigCode == "SM")
                                plan.StudyMaterial = c.ConfigValue;

                            if (c.ConfigCode == "LOGO")
                                plan.Banner = c.ConfigValue;

                            if (c.ConfigCode == "TE")
                                plan.TotalExams = c.ConfigValue;
                        }

                        plan.CategoryIDList = p.CategoryIDList;

                        res.data.Add(plan);
                    }

                }
            }
            catch (Exception ex)
            {
                res.Status = HttpStatusCode.InternalServerError.ToString();
                res.Message = ex.Message;
            }


            return Ok(res);
        }

        [Route("SavePlan")]
        [HttpPost()]
        [Produces("application/json")]
        [Consumes("application/json")]
        public ActionResult<Response<int>> SavePlan([FromBody] CreatePlanDto plan)
        {
            int PlanID = 0;
            Response<int> res = new();


            try
            {
                if (ModelState.IsValid)
                {
                    

                    PlanID = _data.SavePlan(_mapper.Map<Plan>(plan), plan.CreatedBy);

                    if (PlanID > 0)
                    {
                        res.Status = HttpStatusCode.OK.ToString();
                        res.Message = "Success";
                        res.data = PlanID;
                    }
                    else
                    {
                        res.Status = HttpStatusCode.BadRequest.ToString();
                        res.Message = "Failure";

                    }
                }
                else
                {
                    res.Status = HttpStatusCode.BadRequest.ToString();
                    res.Message = "Invalid Model";

                    return Ok(res);
                }
            }
            catch (Exception ex)
            {
                res.Status = HttpStatusCode.InternalServerError.ToString();
                res.Message = ex.Message;

                return Ok(res);
            }

            return Ok(res);
        }

        [HttpPut("PublishPlan/{PlanID}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public ActionResult<Response<string>> ActivateDeactivatePlan(int PlanID, bool Flag)
        {
            Response<string> res = new();
            int i = 0;

            try
            {
                i = _data.ActivateDeActivatePlan(PlanID, Flag);

                if (i > 0)
                {
                    res.Message = "Success";
                    res.Status = HttpStatusCode.OK.ToString();
                }
                else
                {
                    res.Message = "Failure";
                    res.Status = HttpStatusCode.BadRequest.ToString();
                }
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Status = HttpStatusCode.InternalServerError.ToString();

                return Ok(res);
            }

            return Ok(res);
        }

        [HttpPost("SavePlanProductMap/{PlanID}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public ActionResult<Response<string>> SavePlanProductMap(int PlanID, string ProductIDList)
        {
            Response<string> res = new();

            try
            {
                _data.SavePlanProductMap(ProductIDList,PlanID);

                res.Message = "Success";
                res.Status = HttpStatusCode.OK.ToString();
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Status = HttpStatusCode.InternalServerError.ToString();

                return Ok(res);
            }

            return Ok(res);
        }
    }
}
