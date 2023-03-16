using AutoMapper;
using ECommAPI.Models;
using ECommAPI.Services;
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
    public class RegistrationsController : ControllerBase
    {
        private IDBAccess _data;
        private readonly IMapper _mapper;

        public RegistrationsController(IDBAccess data, IMapper mapper)
        {
            _data = data;
            _mapper = mapper;
        }


      

        [HttpPost]
        [Consumes("application/json")]
        public async Task<ActionResult<Response<int>>> SaveRegistration([FromBody] CreateRegistrationDto reg)
        {
            Registration registration = new();
            Response<int> res = new();
            res.data = 0;
            LanguageService langaugeserviceapi = new LanguageService();

            //susmita

            List<Langauge> LMSLanguageDetails = await langaugeserviceapi.GetLanguageAPI();

            //susmita

            try
            {
                if(ModelState.IsValid)
                {
                    if (reg.RegistrationSource != "RICESMARTWEB" && reg.RegistrationSource != "LMS")
                    {
                        res.Message = "Invalid Registration Source. Must be either RICESMARTWEB or LMS";
                        res.Status = HttpStatusCode.BadRequest.ToString();
                    }
                    else
                    {
                        if (reg.HighestEducationQualification==null || reg.HighestEducationQualification=="")
                        {
                            res.Status = HttpStatusCode.BadRequest.ToString();
                            res.Message = "Please provide valid HighestEducationQualification";

                            return Ok(res);
                        }

                        if (reg.Gender == null || reg.Gender == "")
                        {
                            res.Status = HttpStatusCode.BadRequest.ToString();
                            res.Message = "Please provide valid Gender";

                            return Ok(res);
                        }

                        //susmita

                        if (reg.LanguageID <= 0)
                        {
                            res.Status = HttpStatusCode.BadRequest.ToString();
                            res.Message = "Please provide valid Language ID";

                            return Ok(res);
                        }
                        else
                        {
                            if (reg.LanguageID > 0)
                            { 
                                Langauge SearchedLanguage= LMSLanguageDetails.Find(x => x.id == reg.LanguageID);
                                reg.LanguageName = SearchedLanguage.name;
                            }
                        }

                        //susmita

                        res.data = _data.SaveRegistration(_mapper.Map<Registration>(reg));

                        if (res.data > 0)
                        {
                            res.Status = HttpStatusCode.Created.ToString();
                            res.Message = "Success";
                        }
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

        [HttpGet]
        [Produces("application/json")]
        public ActionResult<Response<GetRegistrationCourseEnrolmentDto>> GetRegistrationCourseEnrolment(string CustomerID)
        {
            RegistrationCourseEnrolment registration = new();
            Response<GetRegistrationCourseEnrolmentDto> res = new();


            try
            {
                if(CustomerID==null || CustomerID=="")
                {
                    res.Message = "CustomerID cannot be empty";
                    res.Status = HttpStatusCode.BadRequest.ToString();
                }
                else
                {
                    registration = _data.GetRegistrationCourseEnrolment(CustomerID);

                    if (registration.CustomerID != null)
                    {
                        res.data = _mapper.Map<GetRegistrationCourseEnrolmentDto>(registration);
                        res.Message = "Success";
                        res.Status = HttpStatusCode.OK.ToString();
                    }
                    else
                    {
                        res.Message = "Failure";
                        res.Status = HttpStatusCode.BadRequest.ToString();
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


        [HttpGet("{CustomerID}")]
        [Produces("application/json")]
        public ActionResult<Response<GetRegistrationMissingDetailsDto>> GetMissingRegistrationDetails(string CustomerID)
        {
            Response<GetRegistrationMissingDetailsDto> res = new();
            res.data = new GetRegistrationMissingDetailsDto();

            Registration reg = new();

            try
            {
                if(CustomerID==null || CustomerID.Trim()=="")
                {
                    res.Message = "CustomerID cannot be blank";
                    res.Status = HttpStatusCode.BadRequest.ToString();

                    return Ok(res);
                }


                reg = _data.GetRegistrationDetails(CustomerID);

                if (reg.CustomerID == null)
                {
                    res.Message = "Invalid CustomerID";
                    res.Status = HttpStatusCode.BadRequest.ToString();

                    return Ok(res);
                }

                //if (reg.GuardianName == null || reg.GuardianMobileNo == null || reg.SecondLanguage == null || reg.SocialCategory == null)
                res.data = _mapper.Map<GetRegistrationMissingDetailsDto>(reg);

                if (res.data.CustomerID == null)
                {
                    res.data.CustomerID = CustomerID;
                    res.Message = "No data found";
                    res.Status = HttpStatusCode.OK.ToString();
                }
                else
                {
                    res.Message = "Success";
                    res.Status = HttpStatusCode.OK.ToString();
                }
            }
            catch(Exception ex)
            {
                res.Message = ex.Message;
                res.Status = HttpStatusCode.InternalServerError.ToString();
            }


            return Ok(res);
        }


        [HttpPut]
        [Consumes("application/json")]
        [Produces("application/json")]
        public ActionResult<Response<GetRegistrationMissingDetailsDto>> UpdateRegistrationMissingDetails([FromBody] UpdateRegistrationDto reg)
        {
            Response<GetRegistrationMissingDetailsDto> res = new();

            int RegID = 0;

            try
            {
                //if (ModelState.IsValid)
                //{
                    RegID = _data.UpdateRegistration(_mapper.Map<Registration>(reg));

                    if (RegID == 0)
                    {
                        res.Message = "Invalid CustomerID";
                        res.Status = HttpStatusCode.BadRequest.ToString();
                    }
                    else
                    {
                        res.data = _mapper.Map<GetRegistrationMissingDetailsDto>(_data.GetRegistrationDetails(null, RegID));
                        res.Message = "Success";
                        res.Status = HttpStatusCode.OK.ToString();
                    }
                //}
                //else
                //{
                //    res.Message = "Invalid Request Model";
                //    res.Status = HttpStatusCode.BadRequest.ToString();
                //}
            }
            catch(Exception ex)
            {
                res.Message = ex.Message;
                res.Status = HttpStatusCode.InternalServerError.ToString();

                return Ok(res);
            }


            return Ok(res);
        }


        [Route("UpdateStudentProfile")]
        [HttpPut]
        [Consumes("application/json")]
        [Produces("application/json")]
        public ActionResult<Response<GetUpdatedStudentRegDetailsDto>> UpdateStudentProfile([FromBody] UpdateStudentProfileDto stureg)
        {
            Response<GetUpdatedStudentRegDetailsDto> res = new();

            StudentRegDetails StudentRegDetail = new StudentRegDetails();

            RequestLog log = new();

            log.InvokedMethod = System.Reflection.MethodBase.GetCurrentMethod().Name;
            log.InvokedRoute = "StudentProfileUpdate";
            log.RequestParameters = JsonConvert.SerializeObject(stureg);



            try
            {

                if ((DateTime.Now.Date - stureg.DoB.Date).Days > 4000)
                {



                    StudentRegDetail = _data.UpdateStudentProfile(_mapper.Map<Registration>(stureg));


                    if (StudentRegDetail.RegID == 0)
                    {
                        res.Message = "Invalid CustomerID";
                        res.Status = HttpStatusCode.BadRequest.ToString();
                        log.UniqueAttributeName = "Customer ID";
                        log.UniqueAttributeValue = stureg.CustomerID;

                        log.RequestResult = "Failure";
                        log.ErrorMessage = "Invalid CustomerID";
                    }
                    else
                    {
                        res.data = _mapper.Map<GetUpdatedStudentRegDetailsDto>(StudentRegDetail);
                        res.Message = "Success";
                        res.Status = HttpStatusCode.OK.ToString();
                        log.RequestResult = "Success";

                        log.UniqueAttributeName = "Reg";
                        log.UniqueAttributeValue = "" + res.data.RegID;

                        if (res.data.StudentIDs.Count > 0)
                        {
                            log.UniqueAttributeName = "Student ID";
                            log.UniqueAttributeValue = "";
                            for (int j = 0; j < res.data.StudentIDs.Count; j++)
                            {
                                log.UniqueAttributeValue = log.UniqueAttributeValue + res.data.StudentIDs[j] + ",";
                            }
                            log.UniqueAttributeValue = log.UniqueAttributeValue.Substring(0, log.UniqueAttributeValue.Length - 1);

                        }
                        else
                        {

                            if (res.data.EnquiryNos.Count > 0)
                            {
                                log.UniqueAttributeName = "EnquiryNos";
                                log.UniqueAttributeValue = "";
                                for (int j = 0; j < res.data.EnquiryNos.Count; j++)
                                {
                                    log.UniqueAttributeValue = log.UniqueAttributeValue + res.data.EnquiryNos[j] + ",";
                                }
                                log.UniqueAttributeValue = log.UniqueAttributeValue.Substring(0, log.UniqueAttributeValue.Length - 1);

                            }
                            else
                            {
                                log.UniqueAttributeName = "Registration No";
                                log.UniqueAttributeValue = "" + res.data.RegID;


                            }
                        }


                    }
                }
                else
                { 
                    log.UniqueAttributeName = "Customer ID";
                    log.UniqueAttributeValue = stureg.CustomerID;

                    log.RequestResult = "Failure";
                    log.ErrorMessage = "Invalid DOB " + stureg.DoB.Date.ToString();

                    res.Message = "Invalid DOB,Age of Student Have to be greater than 10 years";
                    res.Status = HttpStatusCode.BadRequest.ToString();
                }

                _data.SaveLogs(log);

            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Status = HttpStatusCode.InternalServerError.ToString();


                log.UniqueAttributeName = "Customer ID";
                log.UniqueAttributeValue = stureg.CustomerID;

                log.RequestResult = "Failure";
                log.ErrorMessage = res.Message;

                _data.SaveLogs(log);

                return Ok(res);
            }


            return Ok(res);
        }





    }
}
