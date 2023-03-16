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
    public class CategoriesController : ControllerBase
    {
        private IDBAccess _data;
        private readonly IMapper _mapper;

        public CategoriesController(IDBAccess data, IMapper mapper)
        {
            _data = data;
            _mapper = mapper;
        }


        [HttpGet("{BrandID}")]
        [Produces("application/json")]
        public ActionResult<Response<List<CategoryDto>>> GetCategories(int BrandID)
        {
            List<Category> categories = new List<Category>();
            Response<List<CategoryDto>> res = new Response<List<CategoryDto>>();


            try
            {
                categories = _data.GetCategories().Where(w => w.BrandID == BrandID).ToList();

                if (categories.Count == 0)
                {
                    res.Status = HttpStatusCode.NoContent.ToString();
                    res.Message = "No data found.";
                }
                else
                {
                    res.Status = HttpStatusCode.OK.ToString();
                    res.Message = "Success";

                    res.data = new List<CategoryDto>();

                    foreach (var c in categories)
                    {
                        res.data.Add(_mapper.Map<CategoryDto>(c));
                    }

                }
            }
            catch(Exception ex)
            {
                res.Status = HttpStatusCode.InternalServerError.ToString();
                res.Message = ex.Message;
            }

            return Ok(res);
        }
    }
}
