using AutoMapper;
using ECommAPI.Models;
using Entities;
using Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ECommAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private IDBAccess _data;
        private readonly IMapper _mapper;
        private readonly IProductManagement _pm;

        public ProductsController(IDBAccess data, IMapper mapper, IProductManagement pm)
        {
            _data = data;
            _mapper = mapper;
            _pm = pm;
        }


        [HttpGet("GetProducts/{BrandID}")]
        [Produces("application/json")]
        public IActionResult GetProducts(int BrandID, int CategoryID=0,int LanguageID=0, bool IsPublished=true)
        {
            List<Product> products = new();
            Response<List<ProductDto>> res = new();


            try
            {
                products = _data.GetProducts(BrandID,0,CategoryID, IsPublished);

                if (products.Count == 0 || LanguageID < 0)
                {
                    res.Status = HttpStatusCode.NoContent.ToString();
                    res.Message = "No data found.";
                }
                else
                {
                    res.Status = HttpStatusCode.OK.ToString();
                    res.Message = "Success";

                    //susmita
                    if (LanguageID == 0)
                        products = products.FindAll(x => x.ProductLangaugeID == 2);//default bengali&english
                    else
                        products = products.FindAll(x => x.ProductLangaugeID == LanguageID);

                    //susmita


                    res.data = new List<ProductDto>();

                    foreach (var p in products)
                    {
                        res.data.Add(_mapper.Map<ProductDto>(p));
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

        [HttpGet("GetProductsForPublishing/{BrandID}")]
        [Produces("application/json")]
        public IActionResult GetProductsForPublishing(int BrandID, int CategoryID = 0,int LanguageID=0)
        {
            List<Product> products = new();
            Response<List<ProductDto>> res = new();


            try
            {
                products = _data.GetProductsForPublishing(BrandID, 0, CategoryID);

                if (products.Count == 0 || LanguageID <= 0)
                {
                    res.Status = HttpStatusCode.NoContent.ToString();
                    res.Message = "No data found.";
                }
                else
                {
                    res.Status = HttpStatusCode.OK.ToString();
                    res.Message = "Success";

                    //susmita
                  
                        products = products.FindAll(x => x.ProductLangaugeID == LanguageID);

                    //susmita

                    res.data = new List<ProductDto>();

                    foreach (var p in products)
                    {
                        res.data.Add(_mapper.Map<ProductDto>(p));
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

        [HttpGet("GetProductListAPK/{BrandID}")]
        [Produces("application/json")]
        public ActionResult<Response<List<GetApkProductDto>>> GetProductListForAPK(int BrandID, int CategoryID = 0, string ExamGroups=null, int PlanID=0,int LanguageID=0)
        {
            List<Product> products = new();
            Response<List<GetApkProductDto>> res = new();


            try
            {

               
                products = _data.GetProductList(BrandID,CategoryID,ExamGroups,PlanID);



                if (products.Count == 0 || LanguageID < 0)
                {
                    res.Status = HttpStatusCode.NoContent.ToString();
                    res.Message = "No data found.";
                }
                else
                {
                    res.Status = HttpStatusCode.OK.ToString();
                    res.Message = "Success";

                    //susmita
                    if (LanguageID == 0)
                        products = products.FindAll(x => x.ProductLangaugeID == 2);
                    else
                        products = products.FindAll(x => x.ProductLangaugeID == LanguageID);

                    //susmita

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

        [Route("SaveProduct")]
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        public ActionResult<Response<int>> SaveProductDetails([FromBody] CreateProductDto productdetails)
        {
            Response<int> res = new();
            Product product = new();

            try
            {
                if (!ModelState.IsValid)
                {
                    res.Message = "Invalid Model";
                    res.Status = HttpStatusCode.BadRequest.ToString();

                    return Ok(res);
                }

                if (productdetails.BrandID <= 0 || productdetails.CategoryID <= 0 || productdetails.CourseID <= 0)
                {
                    res.Message = "Brand, Category and Course ID values must be greater than zero";
                    res.Status = HttpStatusCode.BadRequest.ToString();

                    return Ok(res);
                }

                if (productdetails.ValidTo < productdetails.ValidFrom)
                {
                    res.Message = "Valid To cannot be less than Valid From";
                    res.Status = HttpStatusCode.BadRequest.ToString();


                    return Ok(res);
                }

                product.BrandID = productdetails.BrandID;
                product.CategoryDetails.CategoryID = productdetails.CategoryID;
                product.CourseID = productdetails.CourseID;
                product.ValidFrom = productdetails.ValidFrom;
                product.ValidTo = productdetails.ValidTo;
                product.ProductImage = productdetails.ProductImage;
                product.ProductCode = productdetails.ProductCode;
                product.ProductName = productdetails.ProductName;
                product.ShortDesc = productdetails.ProductShortDesc;
                product.LongDesc = productdetails.ProductLongDesc;
                product.IsPublished = productdetails.IsPublished;

                res.data = _data.SaveProduct(product, productdetails.CreatedBy);

                if (res.data > 0)
                {
                    res.Message = "Success";
                    res.Status = HttpStatusCode.OK.ToString();
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

        [Route("SaveProductMappings")]
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        public ActionResult<Response<string>> SaveProductMappings([FromBody] CreateProductMappingDto mappings)
        {

            Response<string> res = new();
            List<ProductCenterMap> centerMaps = new();

            try
            {
                if (!ModelState.IsValid)
                {
                    res.Message = "Invalid Model";
                    res.Status = HttpStatusCode.BadRequest.ToString();
                }
                else if (mappings.ProductID<=0)
                {
                    res.Message = "Invalid Product ID";
                    res.Status = HttpStatusCode.BadRequest.ToString();
                }
                else
                {
                    foreach(var c in mappings.CenterFeePlanDetails)
                    {
                        ProductCenterMap map = new();

                        if(c.CenterID<=0)
                        {
                            res.Message = "Invalid Center ID";
                            res.Status = HttpStatusCode.BadRequest.ToString();

                            return Ok(res);
                        }

                        if (c.FeePlanID <= 0)
                        {
                            res.Message = "Invalid FeePlan ID";
                            res.Status = HttpStatusCode.BadRequest.ToString();

                            return Ok(res);
                        }

                        map.CenterID = c.CenterID;
                        map.FeePlans.Add(new ProductFeePlan()
                        {
                            CourseFeePlanID = c.FeePlanID,
                            ValidFrom = c.ValidFrom,
                            ValidTo = c.ValidTo
                        });

                        centerMaps.Add(map);
                    }

                    _pm.SaveProductMappings(mappings.ProductID, mappings.ExamCategoryList, centerMaps, mappings.CreatedBy);


                    res.Message = "Success";
                    res.Status = HttpStatusCode.Created.ToString();
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

        [HttpPut("PublishProduct/{ProductID}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public ActionResult<Response<string>> ActivateDeactivateProduct(int ProductID, bool Flag)
        {
            Response<string> res = new();
            int i = 0;

            try
            {
                i = _data.ActivateDeActivateProduct(ProductID, Flag);

                if(i>0)
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

        [HttpGet("GetProductSyllabusSchedule")]
        [Produces("application/json")]
        public ActionResult<Response<GetProductSyllabusScheduleDto>> GetProductSyllabusSchedule(int ProductID, int PlanID=0)
        {
            List<Syllabus> syllabi = new();
            List<Schedule> schedules = new();
            Response<GetProductSyllabusScheduleDto> res = new();


            try
            {
                syllabi = _data.GetSyllabusForProduct(ProductID, PlanID);
                schedules = _data.GetScheduleForProduct(ProductID,PlanID);

                if (syllabi.Count == 0)
                {
                    res.Status = HttpStatusCode.NoContent.ToString();
                    res.Message = "No syllabus found.";
                }
                else if (schedules.Count == 0)
                {
                    res.Status = HttpStatusCode.NoContent.ToString();
                    res.Message = "No schedules found.";
                }
                else
                {
                    res.Status = HttpStatusCode.OK.ToString();
                    res.Message = "Success";

                    res.data = new GetProductSyllabusScheduleDto();
                    res.data.Syllabuses = syllabi;
                    res.data.Schedules = schedules;

                    foreach(var s in schedules)
                    {
                        res.data.AdditionalData.Add(new GetProductAdditionalDetailDto()
                        {
                            ProductID=s.ProductID,
                            CourseID=s.CourseID,
                            TotalExams=s.TotalExams,
                            ExamsCovered=s.ExamsCovered
                        });
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
