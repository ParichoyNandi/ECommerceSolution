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
    public class ExamCategoriesController : ControllerBase
    {
        private IDBAccess _data;
        private readonly IMapper _mapper;

        public ExamCategoriesController(IDBAccess data, IMapper mapper)
        {
            _data = data;
            _mapper = mapper;
        }


        [HttpGet("{BrandID}")]
        [Produces("application/json")]
        public ActionResult<Response<List<GetExamCategoryDto>>> GetExamCategories(int BrandID)
        {
            List<ExamCategory> categories = new List<ExamCategory>();
            Response<List<GetExamCategoryDto>> res = new Response<List<GetExamCategoryDto>>();


            try
            {
                categories = _data.GetExamCategories(BrandID);

                if (categories.Count == 0)
                {
                    res.Status = HttpStatusCode.NoContent.ToString();
                    res.Message = "No data found.";
                }
                else
                {
                    res.Status = HttpStatusCode.OK.ToString();
                    res.Message = "Success";

                    res.data = new List<GetExamCategoryDto>();

                    foreach (var c in categories)
                    {
                        res.data.Add(_mapper.Map<GetExamCategoryDto>(c));
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
