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
using Utilities;

namespace ECommAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IssuesController : ControllerBase
    {
        private IDBAccess _data;
        private readonly IMapper _mapper;

        public IssuesController(IDBAccess data, IMapper mapper)
        {
            _data = data;
            _mapper = mapper;
        }


        [Route("IssueCategory")]
        [HttpGet]
        [Produces("application/json")]
        public ActionResult<Response<List<GetIssueCategoryDto>>> GetIssueCategoryList()
        {
            Response<List<GetIssueCategoryDto>> res = new();
            List<IssueCategory> categories = new();

            try
            {
                categories = _data.GetIssueCategories();

                if (categories.Count == 0)
                {
                    res.Message = "No categories found";
                    res.Status = HttpStatusCode.NoContent.ToString();

                    return Ok(res);
                }
                else
                {
                    res.data = new List<GetIssueCategoryDto>();

                    foreach (var c in categories)
                    {
                        res.data.Add(_mapper.Map<GetIssueCategoryDto>(c));
                    }

                    res.Message = "Success";
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

        [Route("SaveIssue")]
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        public ActionResult SaveCustomerIssue([FromBody] CreateCustomerIssueDto issue)
        {
            Response<int> res = new();
            int c = 0;

            try
            {
                if(!ModelState.IsValid)
                {
                    res.Message = "Invalid Model";
                    res.Status = HttpStatusCode.BadRequest.ToString();

                    return Ok(res);
                }


                c = _data.SaveCustomerIssue(issue.Issue, issue.IssueCategoryID,issue.Name, issue.StudentID, 
                                                issue.CustomerID, issue.ContactNo, issue.EmailID);

                if (c == 0)
                {
                    res.Message = "Issue could not be saved. Please try again.";
                    res.Status = HttpStatusCode.BadRequest.ToString();

                    return Ok(res);
                }
                else
                {
                    //string fromEmail = "rice.team@riceindia.org";
                    //string password = "Password@999";
                    string fromEmail = "help@ricesmart.in";
                    string password = "Nun73280";
                    string subject = "";
                    string message = "";
                    string toEmail = "";

                    List<IssueCategory> categories = new();

                    categories = _data.GetIssueCategories();

                    subject = categories.Where(w => w.ID == issue.IssueCategoryID)
                                        .Select(s => s.CategoryName).FirstOrDefault();
                    toEmail = categories.Where(w => w.ID == issue.IssueCategoryID)
                                        .Select(s => s.DesignatedEmailID).FirstOrDefault();
                    message = $"{issue.Issue}\nName: {issue.Name} \nStudentID: {issue.StudentID} \nCustomerID: {issue.CustomerID} \nContactNo: {issue.ContactNo} \nEmailID: {issue.EmailID}";

                    Helper.SendEmail365(fromEmail, password, toEmail, message, subject);

                    res.Message = "Success";
                    res.Status = HttpStatusCode.OK.ToString();
                    res.data = c;

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
