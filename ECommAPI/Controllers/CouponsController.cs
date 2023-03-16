using AutoMapper;
using ECommAPI.Models;
using Entities;
using Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public class CouponsController : ControllerBase
    {
        private IDBAccess _data;
        private readonly IMapper _mapper;

        public CouponsController(IDBAccess data, IMapper mapper)
        {
            _data = data;
            _mapper = mapper;
        }

        //[HttpGet()]
        //[Produces("application/json")]
        //public ActionResult<Response<List<GetPlanCouponDto>>> GetCouponList(string PlanIDs, decimal PurchaseAmount=0, string StudentID="")
        //{
        //    List<PlanCoupon> planCoupons = new();
        //    Response<List<GetPlanCouponDto>> res = new();


        //    try
        //    {
        //        List<string> planids = new();
        //        planids = PlanIDs.Split(",").ToList();


        //        foreach (var p in planids)
        //        {
        //            List<Coupon> coupons = new();
        //            coupons = _data.GetCouponsForPlan(Convert.ToInt32(p));

        //            coupons = coupons.Distinct().ToList();

        //            if (coupons.Count > 0)
        //            {
        //                planCoupons.Add(new PlanCoupon()
        //                {
        //                    PlanID = Convert.ToInt32(p),
        //                    Coupons = coupons
        //                });
        //            }

        //        }

        //        if (planCoupons.Count == 0)
        //        {
        //            res.Status = HttpStatusCode.NoContent.ToString();
        //            res.Message = "No data found.";
        //        }
        //        else
        //        {
        //            res.Status = HttpStatusCode.OK.ToString();
        //            res.Message = "Success";

        //            res.data = new List<GetPlanCouponDto>();

        //            foreach (var p in planCoupons)
        //            {
        //                res.data.Add(_mapper.Map<GetPlanCouponDto>(p));
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        res.Status = HttpStatusCode.InternalServerError.ToString();
        //        res.Message = ex.Message;
        //    }


        //    return Ok(res);
        //}



        [HttpGet("couponList /{PlanID}")]
        [Produces("application/json")]
        public ActionResult<Response<GetCouponPerCustomerPlanDto>> GetCouponListPerCustomer(int PlanID, String CustomerID,int ProductID,int centerID,int PaymentMode, double PurchaseAmount)
        {
            List<Coupon> Coupons = new();
            Response<GetCouponPerCustomerPlanDto> res = new();

            try
            {
                Coupons = _data.GetCouponsPerCustomerPlan(PlanID, CustomerID, ProductID, centerID,PaymentMode,PurchaseAmount);


                if (Coupons.Count == 0)
                {
                    res.Status = HttpStatusCode.NoContent.ToString();
                    res.Message = "No data found.";
                }
                else
                {
                    res.Status = HttpStatusCode.OK.ToString();
                    res.Message = "Success";

                    res.data = new GetCouponPerCustomerPlanDto();
                    res.data.PlanID = PlanID;
                    res.data.CustomerID = CustomerID;
                    res.data.Coupons = new List<GetCouponDetailsWithCustomerIdDto>();

                    foreach (var p in Coupons)
                    {
                        res.data.Coupons.Add(_mapper.Map<GetCouponDetailsWithCustomerIdDto>(p));
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



        [Route("ValidateCoupon")]
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        public ActionResult<Response<List<ValidateCouponResponseDto>>> ValidateCouponCode([FromBody] ValidateCouponRequestDto validateCoupon)
        {
            //bool flag = false;
            Response<List<ValidateCouponResponseDto>> res = new();

            RequestLog log = new();

            log.InvokedMethod = System.Reflection.MethodBase.GetCurrentMethod().Name;
            log.InvokedRoute = "ValidateCoupon";
            log.UniqueAttributeName = "CouponCode";
            log.UniqueAttributeValue = validateCoupon.CouponCode;
            log.RequestParameters = JsonConvert.SerializeObject(validateCoupon);

            try
            {
                if (ModelState.IsValid)
                {
                    res.data = new List<ValidateCouponResponseDto>();


                    if (validateCoupon.PlanDetails.Count > 0)
                    {
                        foreach (var p in validateCoupon.PlanDetails)
                        {


                            foreach (var pr in p.ProductDetails)
                            {
                                int PaymentModeID = 0;

                                if (pr.PaymentMode == "Lumpsum")
                                    PaymentModeID = 1;
                                else if (pr.PaymentMode == "Instalment")
                                    PaymentModeID = 2;

                                ValidateCoupon validate = new() {
                                    PlanID = p.PlanID,
                                    ProductID = pr.ProductID,
                                    PaymentMode = pr.PaymentMode,
                                    CenterID = pr.CenterID
                                };

                                validate = _data.ValidatePlanCoupon(pr.BrandID, validateCoupon.CouponCode, 
                                                                    p.PlanID, pr.ProductID, PaymentModeID, pr.CenterID, validate, 
                                                                    validateCoupon.PurchaseAmount, validateCoupon.CustomerID);

                                res.data.Add(_mapper.Map<ValidateCouponResponseDto>(validate));
                            }
                        }
                    }

                    if (res.data!=null && res.data.Count>0)
                    {
                        res.Status = HttpStatusCode.OK.ToString();
                        res.Message = "Success";
                    }
                    else
                    {
                        res.Status = HttpStatusCode.NotFound.ToString();
                        res.Message = "Failure";
                    }
                }
                else
                {
                    res.Status = HttpStatusCode.BadRequest.ToString();
                    res.Message = "Request object is not proper";
                }


                log.RequestResult = res.Message;

                if (res.Message != "Success")
                    log.ErrorMessage = res.Message;

                _data.SaveLogs(log);

            }
            catch(Exception ex)
            {
                res.Status = HttpStatusCode.InternalServerError.ToString();
                res.Message = ex.Message;

                log.ErrorMessage = res.Message;
                log.RequestResult = res.Status;

                _data.SaveLogs(log);

                return Ok(res);
            }

            

            return Ok(res);
        }

        [Route("SaveCoupon")]
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        public ActionResult<Response<int>> SaveCouponDetails([FromBody]CreateCouponDto coupon)
        {
            Response<int> res = new();
            int couponid = 0;

            try
            {
                if (ModelState.IsValid)
                {
                    Coupon coup = new();

                    //coup.BrandID = coupon.BrandID;
                    coup.CouponCode = coupon.CouponCode;
                    coup.CouponName = coupon.CouponName;
                    coup.CustomerCode = coupon.CustomerCode;
                    coup.CouponDesc = coupon.CouponDesc;
                    coup.CouponTypeDetails.CouponTypeID = coupon.CouponTypeID;
                    coup.CategoryDetails.CouponCategoryID = coupon.CouponCategoryID;
                    coup.DiscountDetails.DiscountSchemeID = coupon.DiscountID;
                    coup.ValidFrom = coupon.ValidFrom;
                    coup.ValidTo = coupon.ValidTo;
                    coup.CouponCount = coupon.CouponCount;
                    coup.GreaterThanAmount = coupon.GreaterThanAmount;
                    coup.PerStudentCount = coupon.PerStudentCount;
                    coup.IsPrivate = coupon.IsPrivate; // By Susmita: 12-09-2022 :For Coupon Private/Public

                    couponid = _data.SaveCoupon(coup, coupon.BrandList);

                    if (couponid > 0)
                    {
                        res.Message = "Success";
                        res.Status = HttpStatusCode.Created.ToString();
                        res.data = couponid;
                    }
                    else
                    {
                        res.Message = "Failure";
                        res.Status = HttpStatusCode.BadRequest.ToString();
                        res.data = couponid;
                    }
                }
                else
                {
                    res.Message = "Request model is invalid";
                    res.Status = HttpStatusCode.BadRequest.ToString();
                    res.data = couponid;
                }
            }
            catch(Exception ex)
            {
                res.Message = ex.Message;
                res.Status = HttpStatusCode.InternalServerError.ToString();
                res.data = couponid;
            }


            return Ok(res);
        }


        [Route("SaveCouponPlanMap")]
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        public ActionResult<Response<int>> SaveCouponPlanMap([FromBody] List<CreateCouponPlanMapDto> planmap)
        {
            Response<int> res = new();
            int c = 0;


            try
            {
                if (planmap.Count == 0)
                {
                    res.Message = "No data provided";
                    res.Status = HttpStatusCode.BadRequest.ToString();
                }
                else if (!ModelState.IsValid)
                {
                    res.Message = "Model not valid";
                    res.Status = HttpStatusCode.BadRequest.ToString();
                }
                else
                {
                    foreach (var p in planmap)
                    {
                        c += _data.SaveCouponPlanMap(p.CouponID, p.PlanID, p.ValidFrom, p.ValidTo);
                    }
                }

                if (c > 0)
                {
                    res.Message = "Success";
                    res.Status = HttpStatusCode.OK.ToString();
                    res.data = c;
                }
                else
                {
                    res.Message = "Failure";
                    res.Status = HttpStatusCode.OK.ToString();
                }
            }
            catch(Exception ex)
            {
                res.Message = ex.Message;
                res.Status = HttpStatusCode.InternalServerError.ToString();
                return Ok(res);
            }


            return Ok(res);
        }


        [Route("GenerateCampaignCouponCode")]
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        public ActionResult<Response<CampaignCoupon>> GenerateCouponCodeForCampaign(string CampaignName, decimal MarksObtained, string CustomerID,
                                                            DateTime? ValidFrom=null, DateTime? ValidTo=null, int LanguageID = 0)
        {
            Response<CampaignCoupon> res = new();
            CampaignCoupon coupon = new();

            try
            {
                if (String.IsNullOrEmpty(CampaignName))
                {
                    res.Message = "Campaign Name cannot be empty";
                    res.Status = HttpStatusCode.BadRequest.ToString();

                    return Ok(res);
                }

                if (String.IsNullOrEmpty(CustomerID))
                {
                    res.Message = "CustomerID cannot be empty";
                    res.Status = HttpStatusCode.BadRequest.ToString();

                    return Ok(res);
                }

                coupon = _data.GenerateCouponCodeForCampaign(CampaignName, MarksObtained, CustomerID, ValidFrom, ValidTo, LanguageID);

                if (coupon.CouponCode == null)
                {
                    res.Message = "Coupon could not be generated";
                    res.Status = HttpStatusCode.BadRequest.ToString();
                }
                else
                {
                    coupon.MessageDesc=coupon.MessageDesc.Replace("[mark%]", MarksObtained.ToString() + "%");

                    res.Message = "Success";
                    res.Status = HttpStatusCode.Created.ToString();
                    res.data = coupon;
                }
            }
            catch(Exception ex)
            {
                res.Message = ex.Message;
                res.Status = HttpStatusCode.InternalServerError.ToString();

                return Ok(res);
            }

            return Ok(res);
        }

        [Route("GetCouponCards")]
        [HttpGet]
        [Consumes("application/json")]
        [Produces("application/json")]
        public ActionResult<Response<List<CouponCard>>> GetCouponCardsForCustomer(string CustomerID,string CouponType="CUSTOMER", string CampaignName=null)
        {
            Response<List<CouponCard>> res = new();
            List<CouponCard> coupons = new();

            try
            {
                if (String.IsNullOrEmpty(CustomerID))
                {
                    res.Message = "CustomerID cannot be empty";
                    res.Status = HttpStatusCode.BadRequest.ToString();

                    return Ok(res);
                }

                if (String.IsNullOrEmpty(CouponType))
                {
                    res.Message = "CouponType cannot be empty";
                    res.Status = HttpStatusCode.BadRequest.ToString();

                    return Ok(res);
                }

                coupons = _data.GetCouponCards(CustomerID, CouponType,CampaignName);

                if (coupons.Count==0)
                {
                    res.Message = "No coupon found";
                    res.Status = HttpStatusCode.NotFound.ToString();
                }
                else
                {
                    res.Message = "Success";
                    res.Status = HttpStatusCode.Created.ToString();
                    res.data = new List<CouponCard>();
                    res.data = coupons;
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
    }
}
