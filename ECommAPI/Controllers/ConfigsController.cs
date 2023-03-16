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
    public class ConfigsController : ControllerBase
    {
        private IDBAccess _data;
        private readonly IMapper _mapper;

        public ConfigsController(IDBAccess data, IMapper mapper)
        {
            _data = data;
            _mapper = mapper;
        }


        [Route("GetConfigs")]
        [HttpGet]
        [Produces("application/json")]
        public ActionResult<Response<List<GetConfigDto>>> GetConfigs()
        {
            List<Config> configs = new();
            Response<List<GetConfigDto>> res = new();

            configs = _data.GetConfigs();

            if(configs.Count==0)
            {
                res.Message = "No configurations found";
                res.Status = HttpStatusCode.NoContent.ToString();
            }
            else
            {
                res.Message = "Success";
                res.Status = HttpStatusCode.OK.ToString();

                res.data = new List<GetConfigDto>();

                foreach (var c in configs)
                {
                    res.data.Add(_mapper.Map<GetConfigDto>(c));
                }
            }


            return Ok(res);
        }


        [Route("ProductConfigMap")]
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        public ActionResult<Response<List<GetProductConfigMapDto>>> SaveProductConfigMap([FromBody] List<CreateProductConfigMapDto> configMaps)
        {
            if (configMaps == null || configMaps.Count==0)
                return BadRequest();

            Response<List<GetProductConfigMapDto>> res = new();
            res.data = new List<GetProductConfigMapDto>();

            try
            {
                foreach (var c in configMaps)
                {
                    if (c.ProductID > 0 && c.ConfigID > 0)
                    {
                        ProductConfig config = new();

                        config = _data.SaveProductConfig(_mapper.Map<ProductConfig>(c));

                        if (config.ProductConfigID > 0)
                            res.data.Add(_mapper.Map<GetProductConfigMapDto>(config));
                    }
                }

                if (res.data.Count > 0 && res.data.Count == configMaps.Count)
                {
                    res.Message = "Success";
                    res.Status = HttpStatusCode.Created.ToString();
                }
                else if (res.data.Count > 0 && res.data.Count < configMaps.Count)
                {
                    res.Message = "Partial Success. Some mappings could not be saved.";
                    res.Status = HttpStatusCode.Created.ToString();
                }
                else
                {
                    res.Message = "Mappings could not be saved.";
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

        [Route("PlanConfigMap")]
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        public ActionResult<Response<List<GetPlanConfigMapDto>>> SavePlanConfigMap([FromBody] List<CreatePlanConfigMapDto> configMaps)
        {
            if (configMaps == null || configMaps.Count == 0)
                return BadRequest();

            Response<List<GetPlanConfigMapDto>> res = new();
            res.data = new List<GetPlanConfigMapDto>();

            try
            {
                foreach (var c in configMaps)
                {
                    if (c.PlanID > 0 && c.ConfigID > 0)
                    {
                        PlanConfig config = new();

                        config = _data.SavePlanConfig(_mapper.Map<PlanConfig>(c));

                        if (config.PlanConfigID > 0)
                            res.data.Add(_mapper.Map<GetPlanConfigMapDto>(config));
                    }
                }

                if (res.data.Count > 0 && res.data.Count == configMaps.Count)
                {
                    res.Message = "Success";
                    res.Status = HttpStatusCode.Created.ToString();
                }
                else if (res.data.Count > 0 && res.data.Count < configMaps.Count)
                {
                    res.Message = "Partial Success. Some mappings could not be saved.";
                    res.Status = HttpStatusCode.Created.ToString();
                }
                else
                {
                    res.Message = "Mappings could not be saved.";
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

        [Route("SaveConfig")]
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        public IActionResult SaveConfig([FromBody] CreateConfigDto createConfig)
        {
            int c = 0;
            Response<int> res = new Response<int>();

            if (createConfig == null || !ModelState.IsValid)
            {
                res.Message = "Model is incorrect";
                res.Status = HttpStatusCode.BadRequest.ToString();

                return Ok(res);
            }


            try
            {
                if (createConfig.ConfigCode != null && createConfig.ConfigCode != ""
                    && createConfig.ConfigDefaultValue != null)
                {
                    c = _data.SaveConfigDetails(_mapper.Map<Config>(createConfig));
                }
                else
                {
                    res.Message = "ConfigCode cannot be null or blank. Default value cannot be null";
                    res.Status = HttpStatusCode.OK.ToString();
                    res.data = c;
                }

                if (c > 0)
                {
                    res.Message = "Success";
                    res.Status = HttpStatusCode.Created.ToString();
                    res.data = c;
                }
                else
                {
                    res.Message = "Conflict between existing ConfigCode";
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
    }
}
