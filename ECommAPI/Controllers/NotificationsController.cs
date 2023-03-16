using AutoMapper;
using ECommAPI.Models;
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
    public class NotificationsController : ControllerBase
    {
        private IDBAccess _data;
        private readonly IMapper _mapper;

        public NotificationsController(IDBAccess data, IMapper mapper)
        {
            _data = data;
            _mapper = mapper;
        }

        [Route("SaveBatchNotification")]
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        public ActionResult<Response<string>> SaveBatchAvailabilityNotification([FromBody] List<CreateBatchNotificationDto> notifications)
        {
            Response<string> res = new();

            try
            {
                if (notifications == null || notifications.Count == 0)
                {
                    res.Message = "Empty list is not allowed";
                    res.Status = HttpStatusCode.BadRequest.ToString();

                    return Ok(res);
                }

                if (!ModelState.IsValid)
                {
                    res.Message = "Invalid model";
                    res.Status = HttpStatusCode.BadRequest.ToString();

                    return Ok(res);
                }

                foreach (var n in notifications)
                {
                    if (n.EmailID == null)
                        n.EmailID = "";

                    _data.SaveBatchNotification(n.CustomerID, n.PlanID, n.ProductID, n.CenterID,n.MobileNo, n.EmailID);

                }

                res.Message = "Success";
                res.Status = HttpStatusCode.Created.ToString();
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
