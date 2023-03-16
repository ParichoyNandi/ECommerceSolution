using AutoMapper;
using ECommAPI.Models;
using ECommManagement;
using Entities;
using Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Threading.Tasks;

namespace ECommAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneralController : ControllerBase
    {
        private IDBAccess _data;
        private readonly IMapper _mapper;
        private readonly string _bucketname;
        private readonly string _accesskey;
        private readonly string _secretkey;

        public GeneralController(IDBAccess data, IMapper mapper,IConfiguration configuration)
        {
            _data = data;
            _mapper = mapper;
            _bucketname = configuration.GetValue<string>("S3BucketName");
            _accesskey = configuration.GetValue<string>("AWSAccessKey");
            _secretkey = configuration.GetValue<string>("AWSSecretKey");
        }


        [HttpGet("HighestQualification/{BrandID}")]
        [Produces("application/json")]
        public ActionResult<Response<List<HighestEducationQualificationDto>>> GetHighestEduList(int BrandID)
        {
            List<HighestEducationQualification> qualifications = new();
            Response<List<HighestEducationQualificationDto>> res = new();


            try
            {
                qualifications = _data.GetHighestEducationQualificationList(BrandID);

                if (qualifications.Count == 0)
                {
                    res.Status = HttpStatusCode.NoContent.ToString();
                    res.Message = "No data found.";
                }
                else
                {
                    res.Status = HttpStatusCode.OK.ToString();
                    res.Message = "Success";

                    res.data = new List<HighestEducationQualificationDto>();

                    foreach (var p in qualifications)
                    {
                        res.data.Add(_mapper.Map<HighestEducationQualificationDto>(p));
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

        [HttpGet("GetGenders")]
        [Produces("application/json")]
        public ActionResult<Response<List<GetGenderDto>>> GetGenderList()
        {
            List<Gender> genders = new();
            Response<List<GetGenderDto>> res = new();


            try
            {
                genders = _data.GetGenders();

                if (genders.Count == 0)
                {
                    res.Status = HttpStatusCode.NoContent.ToString();
                    res.Message = "No data found.";
                }
                else
                {
                    res.Status = HttpStatusCode.OK.ToString();
                    res.Message = "Success";

                    res.data = new List<GetGenderDto>();

                    foreach (var g in genders)
                    {
                        res.data.Add(_mapper.Map<GetGenderDto>(g));
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

        [HttpGet("GetFeeComponents")]
        [Produces("application/json")]
        public ActionResult<Response<List<GetFeeComponentDto>>> GetFeeComponentList(int BrandID)
        {
            List<FeeComponent> fees = new();
            Response<List<GetFeeComponentDto>> res = new();


            try
            {
                fees = _data.GetFeeComponents(BrandID);

                if (fees.Count == 0)
                {
                    res.Status = HttpStatusCode.NoContent.ToString();
                    res.Message = "No data found.";
                }
                else
                {
                    res.Status = HttpStatusCode.OK.ToString();
                    res.Message = "Success";

                    res.data = new List<GetFeeComponentDto>();

                    foreach (var f in fees)
                    {
                        res.data.Add(_mapper.Map<GetFeeComponentDto>(f));
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

        [HttpGet("GetBrands")]
        [Produces("application/json")]
        public ActionResult<Response<List<GetBrandDto>>> GetBrandList()
        {
            List<Brand> brands = new();
            Response<List<GetBrandDto>> res = new();


            try
            {
                brands = _data.GetBrands();

                if (brands.Count == 0)
                {
                    res.Status = HttpStatusCode.NoContent.ToString();
                    res.Message = "No data found.";
                }
                else
                {
                    res.Status = HttpStatusCode.OK.ToString();
                    res.Message = "Success";

                    res.data = new List<GetBrandDto>();

                    foreach (var b in brands)
                    {
                        res.data.Add(_mapper.Map<GetBrandDto>(b));
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

        [HttpGet("GetCentres")]
        [Produces("application/json")]
        public ActionResult<Response<List<GetCentreDto>>> GetCentreList(int BrandID=109)
        {
            List<Centre> centres = new();
            Response<List<GetCentreDto>> res = new();


            try
            {
                centres = _data.GetCentres(BrandID);

                if (centres.Count == 0)
                {
                    res.Status = HttpStatusCode.NoContent.ToString();
                    res.Message = "No data found.";
                }
                else
                {
                    res.Status = HttpStatusCode.OK.ToString();
                    res.Message = "Success";

                    res.data = new List<GetCentreDto>();

                    foreach (var b in centres)
                    {
                        res.data.Add(_mapper.Map<GetCentreDto>(b));
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

        [HttpGet("GetCourses")]
        [Produces("application/json")]
        public ActionResult<Response<List<GetCourseDto>>> GetCourseList(int BrandID = 109,int LanguageID=0)
        {
            List<Course> courses = new();
            Response<List<GetCourseDto>> res = new();


            try
            {
                courses = _data.GetCourses(BrandID);

                if (courses.Count == 0 || LanguageID < 0)
                {
                    res.Status = HttpStatusCode.NoContent.ToString();
                    res.Message = "No data found.";
                }
                else
                {
                    res.Status = HttpStatusCode.OK.ToString();
                    res.Message = "Success";

                    courses = courses.FindAll(x => x.CourseLanguageID == LanguageID);

                    res.data = new List<GetCourseDto>();

                    foreach (var b in courses)
                    {
                        res.data.Add(_mapper.Map<GetCourseDto>(b));
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

        [HttpGet("GetCouponCategory")]
        [Produces("application/json")]
        public ActionResult<Response<List<GetCouponCategoryDto>>> GetCouponCategoryList()
        {
            List<CouponCategory> categories = new();
            Response<List<GetCouponCategoryDto>> res = new();


            try
            {
                categories = _data.GetCouponCategories();

                if (categories.Count == 0)
                {
                    res.Status = HttpStatusCode.NoContent.ToString();
                    res.Message = "No data found.";
                }
                else
                {
                    res.Status = HttpStatusCode.OK.ToString();
                    res.Message = "Success";

                    res.data = new List<GetCouponCategoryDto>();

                    foreach (var b in categories)
                    {
                        res.data.Add(_mapper.Map<GetCouponCategoryDto>(b));
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

        [HttpGet("GetCouponList")]
        [Produces("application/json")]
        public ActionResult<Response<List<GetCouponDto>>> GetCouponList(int CouponCategoryID, int BrandID=109)
        {
            List<Coupon> coupons = new();
            Response<List<GetCouponDto>> res = new();


            try
            {
                coupons = _data.GetCoupons(CouponCategoryID,BrandID);

                if (coupons.Count == 0)
                {
                    res.Status = HttpStatusCode.NoContent.ToString();
                    res.Message = "No data found.";
                }
                else
                {
                    res.Status = HttpStatusCode.OK.ToString();
                    res.Message = "Success";

                    res.data = new List<GetCouponDto>();

                    foreach (var b in coupons)
                    {
                        res.data.Add(_mapper.Map<GetCouponDto>(b));
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

        [HttpPost("GetBatches")]
        [Produces("application/json")]
        public ActionResult<Response<List<GetBatchListResponseDto>>> GetBatchList([FromBody] List<GetBatchListRequestDto> getBatches)
        {
            
            Response<List<GetBatchListResponseDto>> res = new();
            res.data = new List<GetBatchListResponseDto>();

            try
            {
                foreach(var g in getBatches)
                {
                    if (g.AfterDate.Date < DateTime.Now.Date)
                    {
                        res.Message = "AfterDate cannot be less than current date";
                        res.Status = HttpStatusCode.BadRequest.ToString();

                        return Ok(res);
                    }

                    List<Batch> batches = new();
                    GetBatchListResponseDto getBatch = new();

                    getBatch=_mapper.Map<GetBatchListResponseDto>(g);

                    batches = _data.GetBatches(g.ProductID, g.CenterID, g.AfterDate);

                    foreach (var b in batches)
                    {
                        getBatch.BatchList.Add(_mapper.Map<GetBatchDto>(b));
                    }

                    res.data.Add(getBatch);
                }

                res.Message = "Success";
                res.Status = HttpStatusCode.OK.ToString();
                
            }
            catch (Exception ex)
            {
                res.Status = HttpStatusCode.InternalServerError.ToString();
                res.Message = ex.Message;
            }


            return Ok(res);
        }

        [Route("GetSecondLanguage")]
        [HttpGet]
        [Produces("application/json")]
        public ActionResult GetSecondLanguageList()
        {
            Response<List<GetSecondLanguageDto>> res = new();

            res.data = new List<GetSecondLanguageDto>();


            res.data.Add(new GetSecondLanguageDto()
            {
                SecondLanguageName="Bengali"
            });
            res.data.Add(new GetSecondLanguageDto()
            {
                SecondLanguageName = "Hindi"
            });

            res.Message = "Success";
            res.Status = HttpStatusCode.OK.ToString();

            return Ok(res);
        }

        [Route("GetSocialCategory")]
        [HttpGet]
        [Produces("application/json")]
        public ActionResult GetSocialCategoryList()
        {
            Response<List<GetSocialCategoryDto>> res = new();
            List<SocialCategory> categories = new();

            res.data = new List<GetSocialCategoryDto>();

            try
            {
                categories = _data.GetSocialCategoryList();

                foreach (var s in categories)
                {
                    res.data.Add(_mapper.Map<GetSocialCategoryDto>(s));
                }

                if(res.data.Count>0)
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
            catch(Exception ex)
            {
                res.Message = ex.Message;
                res.Status = HttpStatusCode.InternalServerError.ToString();

                return Ok(res);
            }

            return Ok(res);
        }

        [Route("GetFeePlans")]
        [HttpGet]
        [Produces("application/json")]
        public ActionResult<Response<List<GetFeePlanDto>>> GetFeePlanList(int CenterID, int ProductID)
        {
            Response<List<GetFeePlanDto>> res = new();
            List<FeePlan> feePlans = new();

            res.data = new List<GetFeePlanDto>();

            try
            {
                feePlans = _data.GetFeePlanList(CenterID,ProductID);

                foreach (var s in feePlans)
                {
                    res.data.Add(_mapper.Map<GetFeePlanDto>(s));
                }

                if (res.data.Count > 0)
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


        [Route("UploadFile")]
        [HttpPost]
        public async Task<ActionResult> FileUpload(IFormFile uploadedfile)
        {
            Response<string> res = new();


            FileManagement f = new();

            try
            {
                String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                //var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
                string filename = uploadedfile.FileName.Substring(0,uploadedfile.FileName.IndexOf("."))+ timeStamp;
                filename += uploadedfile.FileName.Substring(uploadedfile.FileName.IndexOf("."), uploadedfile.FileName.Length-uploadedfile.FileName.IndexOf("."));


                res.data=await f.UploadFileToS3(uploadedfile,filename,_bucketname,_accesskey,_secretkey);

                if(res.data!=null && res.data!="")
                {
                    res.Message = "Success";
                    res.Status = HttpStatusCode.OK.ToString();
                }
                else
                {
                    res.Message = "Failure";
                    res.Status = HttpStatusCode.NoContent.ToString();
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

        [Route("ValidateUser")]
        [HttpGet]
        [Produces("application/json")]
        public ActionResult ValidateUser(string LoginID, string Password)
        {
            RequestLog log = new();

            log.InvokedMethod = System.Reflection.MethodBase.GetCurrentMethod().Name;
            log.InvokedRoute = "ValidateUser";
            log.UniqueAttributeName = "LoginID";
            log.UniqueAttributeValue = LoginID;
            log.RequestParameters = $"LoginID:{LoginID}, Password:{Password}";

            int UserID = 0;

            try
            {
                UserID = _data.ValidateUser(LoginID, Password);
            }
            catch(Exception ex)
            {
                log.ErrorMessage = ex.Message;
                _data.SaveLogs(log);
                return BadRequest();
            }

            _data.SaveLogs(log);

            return Ok(UserID);
        }

        [Route("ResetCustomerData")]
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        public ActionResult ResetCustomerData(string CustomerID)
        {

            try
            {
                _data.ResetDataForTesting(CustomerID);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [Route("AboutBrand")]
        [HttpGet]
        [Produces("application/json")]
        public ActionResult<Response<GetAboutBrandDto>> GetBrandDetails(string ToBeDisplayedIn="LMS", int BrandID=109)
        {
            Response<GetAboutBrandDto> res = new();
            AboutBrand aboutBrand = new();

            try
            {
                aboutBrand = _data.GetAboutBrandDetails(ToBeDisplayedIn, BrandID);

                if (aboutBrand.ID > 0)
                {
                    res.data = _mapper.Map<GetAboutBrandDto>(aboutBrand);
                    res.Message = "Success";
                    res.Status = HttpStatusCode.OK.ToString();
                }
                else
                {
                    res.Message = "Not Found";
                    res.Status = HttpStatusCode.NotFound.ToString();

                    return Ok(res);
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



        [Route("GetActiveCampaignDetails")]
        [HttpGet]
        [Produces("application/json")]
        public ActionResult<Response<List<GetCampaignDetailsDto>>> GetActiveCampaignDetails()
        {
            Response< List < GetCampaignDetailsDto >> res = new();
            List<Campaign> Campaigns = new();

           

            try
            {
                Campaigns = _data.GetActiveCampaigns();

                res.data = new List<GetCampaignDetailsDto>();

                foreach (var s in Campaigns)
                {
                    res.data.Add(_mapper.Map<GetCampaignDetailsDto>(s));
                }

                if (res.data.Count > 0)
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



        [HttpGet("GetRecommendedCourseLists/{CourseID}")]
        [Produces("application/json")]
        public ActionResult<Response<List<GetRecommendedCourseProductPlanDto>>> GetRecommendedCourseLists(int CourseID)
        {
            Response<List<GetRecommendedCourseProductPlanDto>> res = new();
            List<RecommendedCourseProductPlan> RecommendedCourseProductPlans = new();



            try
            {
                RecommendedCourseProductPlans = _data.GetRecommendedCourseLists(CourseID);

                res.data = new List<GetRecommendedCourseProductPlanDto>();

                foreach (var s in RecommendedCourseProductPlans)
                {
                    res.data.Add(_mapper.Map<GetRecommendedCourseProductPlanDto>(s));
                }

                if (res.data.Count > 0)
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



        [HttpGet("GetRecommendedProductLists/{CourseID}")]
        [Produces("application/json")]
        public ActionResult<Response<List<GetApkProductDto>>> GetRecommendedProductLists(int CourseID)
        {
            List<Product> products = new();
            Response<List<GetApkProductDto>> res = new();


            try
            {


                products = _data.GetRecommendedProductList(CourseID);



                if (products.Count == 0)
                {
                    res.Status = HttpStatusCode.NoContent.ToString();
                    res.Message = "No data found.";
                }
                else
                {
                    res.Status = HttpStatusCode.OK.ToString();
                    res.Message = "Success";


                    res.data = new List<GetApkProductDto>();

                    foreach (var p in products)
                    {
                        GetApkProductDto productDto = new();

                        productDto.ProductID = p.ProductID;
                        productDto.CourseName = p.ProductName;
                        productDto.Banner = p.ProductImage;
                        productDto.CategoryID = p.CategoryDetails.CategoryID;
                        productDto.CategoryName = p.CategoryDetails.CategoryName;
                        productDto.ProductLangaugeID = p.ProductLangaugeID;
                        productDto.ProductLangaugeName = p.ProductLanguageName;

                        foreach (var c in p.ProductConfigList)
                        {
                            if (c.ConfigCode == "LC")
                                productDto.LiveClasses = c.ConfigValue;

                            if (c.ConfigCode == "TV")
                                productDto.Videos = c.ConfigValue;

                            if (c.ConfigCode == "CD")
                                productDto.Duration = c.ConfigValue;

                            if (c.ConfigCode == "SML")
                                productDto.StudyMetarialLanguage = c.ConfigValue;

                            if (c.ConfigCode == "SM")
                                productDto.StudyMaterial = c.ConfigValue;

                            if (c.ConfigCode == "TE")
                                productDto.TotalExams = c.ConfigValue;
                        }

                        res.data.Add(productDto);
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






    }
}
