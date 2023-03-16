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
    public class FAQController : ControllerBase
    {
        private IDBAccess _data;
        private readonly IMapper _mapper;

        public FAQController(IDBAccess data, IMapper mapper)
        {
            _data = data;
            _mapper = mapper;
        }

        [Route("GetFAQForPlan")]
        [HttpGet]
        [Produces("application/json")]
        public ActionResult GetFAQForPlan(int PlanID)
        {
            Response<GetFAQDto> res = new();
            FAQ fq = new();

            try
            {
                fq = _data.GetFAQForPlan(PlanID);

                if (fq.FAQID == 0 || fq.FAQDetails == null || fq.FAQDetails.Count == 0)
                {
                    res.Message = "No data found";
                    res.Status = HttpStatusCode.NoContent.ToString();

                    return Ok(res);
                }

                res.data = _mapper.Map<GetFAQDto>(fq);
                res.Message = "Success";
                res.Status = HttpStatusCode.OK.ToString();
            }
            catch(Exception ex)
            {
                res.Message = ex.Message;
                res.Status = HttpStatusCode.InternalServerError.ToString();

                return Ok(res);
            }


            return Ok(res);
        }
    }
}
