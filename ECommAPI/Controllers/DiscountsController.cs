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
    public class DiscountsController : ControllerBase
    {
        private IDBAccess _data;
        private readonly IMapper _mapper;

        public DiscountsController(IDBAccess data, IMapper mapper)
        {
            _data = data;
            _mapper = mapper;
        }


        [Route("CreateDiscount")]
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        public ActionResult<Response<int>> CreateDiscountScheme([FromBody] CreateDiscountSchemeDto createDiscount)
        {
            int DiscountSchemeID = 0;
            Response<int> res = new();


            try
            {
                if (ModelState.IsValid)
                {
                    DiscountSchemeID = _data.SaveDiscountScheme(_mapper.Map<DiscountScheme>(createDiscount), createDiscount.BrandList,createDiscount.CreatedBy);

                    if (DiscountSchemeID > 0)
                    {
                        foreach (var d in createDiscount.DiscountSchemeDetails)
                        {
                            if (d.DiscountAmount == 0 && d.DiscountRate == 0)
                            {
                                continue;
                            }
                            if (d.DiscountAmount > 0 && d.DiscountRate > 0)
                            {
                                continue;
                            }

                            _data.SaveDiscountSchemeDetails(DiscountSchemeID, _mapper.Map<DiscountSchemeDetail>(d), createDiscount.CreatedBy);
                        }

                        res.Message = "Success";
                        res.Status = HttpStatusCode.Created.ToString();
                        res.data = DiscountSchemeID;
                    }
                    else
                    {
                        res.Message = "Discount Scheme with same name already exists";
                        res.Status = HttpStatusCode.BadRequest.ToString();
                    }
                }
                else
                {
                    res.Message = "Request model is invalid";
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


        [Route("CreateCenterCourseMap")]
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        public ActionResult<Response<List<GetDiscountSchemeCenterMap>>> SaveDiscountCenterCourseMap([FromBody] List<CreateDiscountSchemeCenterCourseMapDto> courseMapDtos)
        {
            List<DiscountSchemeCenterMap> centerMaps = new();
            Response<List<GetDiscountSchemeCenterMap>> res = new();

            try
            {
                if (ModelState.IsValid)
                {
                    if (courseMapDtos.Count > 0)
                    {
                        foreach (var c in courseMapDtos)
                        {
                            _data.SaveDiscountSchemeCenterCourseMap(c.DiscountSchemeID, c.CenterID, c.CourseID, c.CreatedBy);
                        }

                        res.Message = "Success";
                        res.Status = HttpStatusCode.Created.ToString();

                        res.data = new List<GetDiscountSchemeCenterMap>();

                        centerMaps = _data.GetDiscountSchemeCenterCourseMap(courseMapDtos[0].DiscountSchemeID);

                        foreach(var c in centerMaps)
                        {
                            res.data.Add(_mapper.Map<GetDiscountSchemeCenterMap>(c));
                        }
                    }
                    else
                    {
                        res.Message = "No data provided";
                        res.Status = HttpStatusCode.BadRequest.ToString();
                    }
                }
                else
                {
                    res.Message = "Failure. Please provide a valid data model";
                    res.Status = HttpStatusCode.BadRequest.ToString();
                }
            }
            catch(Exception ex)
            {
                res.Message = ex.Message;
                res.Status = HttpStatusCode.InternalServerError.ToString();
            }


            return Ok(res);
        }

        [Route("GetDiscountSchemes")]
        [HttpGet]
        [Consumes("application/json")]
        [Produces("application/json")]
        public ActionResult<Response<List<GetDiscountSchemeDto>>> GetDiscountSchemes(string BrandIDList)
        {
            List<DiscountScheme> schemes = new();
            Response<List<GetDiscountSchemeDto>> res = new();

            try
            {
                schemes = _data.GetDiscountSchemes(BrandIDList);
                if (schemes.Count > 0)
                {

                    res.Message = "Success";
                    res.Status = HttpStatusCode.OK.ToString();

                    res.data = new List<GetDiscountSchemeDto>();

                    foreach (var s in schemes)
                    {
                        res.data.Add(_mapper.Map<GetDiscountSchemeDto>(s));
                    }
                }
                else
                {
                    res.Message = "No data found";
                    res.Status = HttpStatusCode.NoContent.ToString();
                }
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Status = HttpStatusCode.InternalServerError.ToString();
            }


            return Ok(res);
        }
    }
}
