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
    public class ExamGroupsController : ControllerBase
    {
        private IDBAccess _data;
        private readonly IMapper _mapper;

        public ExamGroupsController(IDBAccess data, IMapper mapper)
        {
            _data = data;
            _mapper = mapper;
        }


        [HttpGet("{BrandID}")]
        [Produces("application/json")]
        public ActionResult<Response<List<GetExamGroupDto>>> GetExamGroups(int BrandID=109)
        {
            List<ExamGroup> groups = new List<ExamGroup>();
            Response<List<GetExamGroupDto>> res = new Response<List<GetExamGroupDto>>();


            try
            {
                groups = _data.GetExamGroups(BrandID);

                if (groups.Count == 0)
                {
                    res.Status = HttpStatusCode.NoContent.ToString();
                    res.Message = "No data found.";
                }
                else
                {
                    res.Status = HttpStatusCode.OK.ToString();
                    res.Message = "Success";

                    res.data = new List<GetExamGroupDto>();

                    foreach (var c in groups)
                    {
                        res.data.Add(_mapper.Map<GetExamGroupDto>(c));
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
