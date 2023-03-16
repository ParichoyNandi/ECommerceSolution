using AutoMapper;
using ECommAPI.Models;
using ECommAPI.Services;
using Entities;
using Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
    public class PaymentsController : ControllerBase
    {
        private IDBAccess _data;
        private IPaymentManagement _pm;
        private readonly IMapper _mapper;
        private IConfiguration _config;

        public PaymentsController(IDBAccess data, IMapper mapper, IPaymentManagement pm, IConfiguration config)
        {
            _data = data;
            _mapper = mapper;
            _pm = pm;
            _config = config;
        }

        [Route("ValidatePurchase")]
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        public ActionResult<Response<bool>> ValidatePaymentDetailsFromExternalSource([FromBody] CreatePaymentDto createPayment)
        {
            Response<bool> res = new();
            bool IsValid = false;

            RequestLog log = new();

            log.InvokedMethod = System.Reflection.MethodBase.GetCurrentMethod().Name;
            log.InvokedRoute = "ValidatePurchase";
            log.UniqueAttributeName = "TransactionNo";
            log.UniqueAttributeValue = createPayment.TransactionNo;
            log.RequestParameters = JsonConvert.SerializeObject(createPayment);


            Transaction transaction = new();

            try
            {
                if (createPayment == null)
                {
                    res.Status = HttpStatusCode.BadRequest.ToString();
                    res.Message = "Empty model";
                    res.data = IsValid;

                    return Ok(res);
                }

                if (!ModelState.IsValid)
                {
                    res.Status = HttpStatusCode.BadRequest.ToString();
                    res.Message = "Model not valid";
                    res.data = IsValid;

                    return Ok(res);
                }

                if (createPayment.TransactionSource != "Online-RICESMARTWEB" && createPayment.TransactionSource != "Online-LMS")
                {
                    res.Status = HttpStatusCode.BadRequest.ToString();
                    res.Message = "Invalid Transaction Source";
                    res.data = IsValid;

                    return Ok(res);
                }

                foreach (var pl in createPayment.PlanPaymentDetails)
                {
                    foreach (var pr in pl.ProductPaymentDetails)
                    {
                        if (pr.BatchID <= 0)
                        {
                            res.Status = HttpStatusCode.BadRequest.ToString();
                            res.Message = "BatchID is mandatory";
                            res.data = IsValid;

                            return Ok(res);
                        }
                    }
                }

                IsValid = _data.ValidatePurchase(_mapper.Map<Payment>(createPayment));

                if (IsValid == false)
                {
                    res.Status = HttpStatusCode.BadRequest.ToString();
                    res.Message = "Invalid Purchase request";
                    res.data = IsValid;
                }
                else
                {
                    res.Status = HttpStatusCode.OK.ToString();
                    res.Message = "Success";
                    res.data = IsValid;
                }

                log.RequestResult = res.Message;

                if (res.Message != "Success")
                    log.ErrorMessage = res.Message;

                _data.SaveLogs(log);
            }
            catch (Exception ex)
            {
                res.Status = HttpStatusCode.BadRequest.ToString();
                res.Message = ex.Message;
                res.data = IsValid;

                log.ErrorMessage = res.Message;
                log.RequestResult = res.Status;

                _data.SaveLogs(log);

                return Ok(res);
            }


            return Ok(res);
        }

        [Route("MakePayment")]
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        public ActionResult<Response<Transaction>> GetPaymentDetailsFromExternalSource([FromBody]CreatePaymentDto createPayment)
        {
            Response<Transaction> res = new();
            bool IsValid = false;

            Transaction transaction = new();

            RequestLog log = new();

            log.InvokedMethod = System.Reflection.MethodBase.GetCurrentMethod().Name;
            log.InvokedRoute = "MakePayment";
            log.UniqueAttributeName = "TransactionNo";
            log.UniqueAttributeValue = createPayment.TransactionNo;
            log.RequestParameters = JsonConvert.SerializeObject(createPayment);


            try
            {
                if (createPayment == null)
                {
                    res.Status = HttpStatusCode.BadRequest.ToString();
                    res.Message = "Empty model";

                    return Ok(res);
                }

                if (!ModelState.IsValid)
                {
                    res.Status = HttpStatusCode.BadRequest.ToString();
                    res.Message = "Model not valid";

                    return Ok(res);
                }

                if(createPayment.TransactionSource!="Online-RICESMARTWEB" && createPayment.TransactionSource!="Online-LMS")
                {
                    res.Status = HttpStatusCode.BadRequest.ToString();
                    res.Message = "Invalid Transaction Source";

                    return Ok(res);
                }

                foreach(var pl in createPayment.PlanPaymentDetails)
                {
                    foreach(var pr in pl.ProductPaymentDetails)
                    {
                        if(pr.BatchID<=0)
                        {
                            res.Status = HttpStatusCode.BadRequest.ToString();
                            res.Message = "BatchID is mandatory";

                            return Ok(res);
                        }
                    }
                }

                IsValid = _data.ValidatePurchase(_mapper.Map<Payment>(createPayment));

                if (!IsValid)
                {
                    res.Status = HttpStatusCode.BadRequest.ToString();
                    res.Message = "Invalid Purchase request";

                    return Ok(res);
                }
                else
                {
                    _data.SaveTransactionDetails(_mapper.Map<Payment>(createPayment));
                    _pm.ProcessPayment(_mapper.Map<Payment>(createPayment));
                    transaction = _data.GetTransactionDetails(createPayment.TransactionNo);
                    res.data = transaction;

                    if(transaction!=null)
                    {
                        ECommService service = new();

                        service.SendTransactionNotification(transaction).Wait();
                    }


                    if(transaction.IsCompleted)
                    {
                        res.Status = HttpStatusCode.OK.ToString();
                        res.Message = "Success";
                    }
                    else
                    {
                        res.Status = HttpStatusCode.OK.ToString();
                        res.Message = "Failure";
                    }

                    
                }
            }
            catch(Exception ex)
            {
                res.Status = HttpStatusCode.BadRequest.ToString();
                res.Message = ex.Message;

                log.ErrorMessage = res.Message;
                log.RequestResult = res.Status;

                _data.SaveLogs(log);

                return Ok(res);
            }

            log.RequestResult = res.Message;

            if (res.Message != "Success")
                log.ErrorMessage = res.Message;

            _data.SaveLogs(log);


            return Ok(res);
        }

        [Route("RegisterSubscription")]
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        public ActionResult<Response<List<CreateNewSubscriptionDto>>> SaveNewSubscription([FromBody] List<CreateNewSubscriptionDto> createSub)
        {
            Response<List<CreateNewSubscriptionDto>> res = new();
            res.data = new List<CreateNewSubscriptionDto>();

            RequestLog log = new();

            log.InvokedMethod = System.Reflection.MethodBase.GetCurrentMethod().Name;
            log.InvokedRoute = "RegisterSubscription";
            log.RequestParameters = JsonConvert.SerializeObject(createSub);

            try
            {
                if (ModelState.IsValid)
                {
                    for (int i = 0; i < createSub.Count; i++)
                    {
                        var s = createSub[i];

                        createSub[i].SubscriptionID = _data.SaveNewSubscription(s.TransactionNo,s.PlanID,s.ProductID, s.SubscriptionPlanID, s.Authkey,
                                                                s.BillingPeriod, s.BillingStartDate, s.BillingEndDate, s.TotalBillingAmount,s.SubscriptionStatus,s.MandateLink);

                        if(createSub[i].SubscriptionID<=0)
                        {
                            res.Message = "Failure";
                            res.Status = HttpStatusCode.OK.ToString();

                            return Ok(res);
                        }
                        else
                        {
                            res.data.Add(createSub[i]);
                        }
                    }
                }
                else
                {
                    res.Message = "Invalid Model";
                    res.Status = HttpStatusCode.BadRequest.ToString();

                    return Ok(res);
                }

                if (res.data.Count > 0)
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

                log.ErrorMessage = res.Message;
                log.RequestResult = res.Status;

                _data.SaveLogs(log);

                return Ok(res);
            }

            log.RequestResult = res.Message;

            if (res.Message != "Success")
                log.ErrorMessage = res.Message;

            _data.SaveLogs(log);

            return Ok(res);
        }

        [Route("GetSubscriptionDues")]
        [HttpGet]
        [Produces("application/json")]
        public ActionResult<Response<List<GetStudentSubscriptionDto>>> GetStudentSubscriptionDues(DateTime DueDate)
        {
            Response<List<GetStudentSubscriptionDto>> res = new();

            List<StudentSubscription> subscriptions = new();

            try
            {
                subscriptions = _data.GetStudentSubscriptionDues(DueDate);

                if (subscriptions.Count > 0)
                {
                    res.data = new List<GetStudentSubscriptionDto>();

                    foreach (var s in subscriptions)
                    {
                        res.data.Add(_mapper.Map<GetStudentSubscriptionDto>(s));
                    }

                    res.Message = "Success";
                    res.Status = HttpStatusCode.OK.ToString();
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

                return Ok(res);
            }


            return Ok(res);
        }


        [Route("GetPayoutDues")]
        [HttpGet]
        [Produces("application/json")]
        public ActionResult<Response<GetStudentPayOutDto>> GetStudentPayoutDues(string CustomerID)
        {
            Response<GetStudentPayOutDto> res = new();

            StudentPayOut payout = new();

            try
            {
                payout = _data.GetStudentPayoutDues(CustomerID);

                if (payout!=null && payout.CustomerID!=null && payout.CustomerID.Trim()!="")
                {
                    res.data = _mapper.Map<GetStudentPayOutDto>(payout);

                    res.Message = "Success";
                    res.Status = HttpStatusCode.OK.ToString();
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

                return Ok(res);
            }


            return Ok(res);
        }

        [Route("SaveSubscription")]
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        public ActionResult<Response<List<StudentSubscriptionPaymentResponseDto>>> SaveSubscriptionTransaction([FromBody] List<CreateStudentSubscriptionPaymentDto> paymentlist)
        {
            Response<List<StudentSubscriptionPaymentResponseDto>> res = new();
            res.data = new();

            bool saveflag = true;

            RequestLog log = new();

            log.InvokedMethod = System.Reflection.MethodBase.GetCurrentMethod().Name;
            log.InvokedRoute = "SaveSubscription";
            log.RequestParameters = JsonConvert.SerializeObject(paymentlist);

            try
            {
                if (paymentlist == null || paymentlist.Count <= 0)
                {
                    res.Message = "Empty request object";
                    res.Status = HttpStatusCode.BadRequest.ToString();

                    return Ok(res);
                }
                else
                {
                    foreach (var p in paymentlist)
                    {
                        if (p.TransactionSource != "Online-RICESMARTWEB" && p.TransactionSource != "Online-LMS")
                        {
                            res.Status = HttpStatusCode.BadRequest.ToString();
                            res.Message = "Invalid Transaction Source";

                            return Ok(res);
                        }
                        res.data.Add(_mapper.Map<StudentSubscriptionPaymentResponseDto>(p));
                    }

                    for (int i = 0; i < res.data.Count; i++)
                    {
                        var p = res.data[i];

                        for (int j = 0; j < p.FeeSchedulePaymentDetails.Count; j++)
                        {
                            var f = p.FeeSchedulePaymentDetails[j];
                            int FeeScheduleID = 0;
                            int SubscriptionTransactionID = 0;
                            int ReceiptHeaderID = 0;

                            StudentSubscriptionTransaction sub = new();

                            FeeScheduleID = _data.ValidateSubscriptionPayment(p.SubscriptionID, p.Amount, p.Tax, p.DueDate, f.FeeScheduleID);

                            if (FeeScheduleID > 0)
                            {
                                sub = _mapper.Map<StudentSubscriptionTransaction>(p);
                                sub.FeeScheduleID = FeeScheduleID;
                                sub.Amount = f.Amount;
                                sub.Tax = f.Tax;

                                SubscriptionTransactionID = _data.SaveSubscriptionTransactionDetails(sub);
                            }

                            if (SubscriptionTransactionID <= 0)
                                saveflag = false;

                            res.data[i].FeeSchedulePaymentDetails[j].SubscriptionTransactionID = SubscriptionTransactionID;

                            if (FeeScheduleID > 0 && SubscriptionTransactionID > 0)
                            {
                                StudentSubscriptionTransaction transaction = new();

                                transaction = _data.GetSubscriptionTransactionDetails(p.TransactionNo, FeeScheduleID);

                                if (transaction != null && transaction.CenterID > 0 && transaction.StudentDetailID > 0
                                    && transaction.IsCompleted == false && transaction.TransactionStatus == "Success")
                                {
                                    ReceiptHeaderID = _pm.ProcessSubscriptionPayment(transaction, FeeScheduleID);

                                    if (ReceiptHeaderID > 0)
                                    {
                                        res.data[i].FeeSchedulePaymentDetails[j].ReceiptHeaderID = ReceiptHeaderID;
                                        res.data[i].FeeSchedulePaymentDetails[j].IsCompleted = true;
                                    }
                                }
                            }
                        }
                    }
                }

                if (saveflag)
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

                log.ErrorMessage = res.Message;
                log.RequestResult = res.Status;

                _data.SaveLogs(log);

                return Ok(res);
            }

            log.RequestResult = res.Message;

            if (res.Message != "Success")
                log.ErrorMessage = res.Message;

            _data.SaveLogs(log);

            return Ok(res);
        }

        [Route("PaySubscription")]
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        public ActionResult<Response<List<StudentSubscriptionPaymentResponseDto>>> ProcessSubscriptionTransaction([FromBody] List<CreateStudentSubscriptionPaymentDto> paymentlist)
        {
            Response<List<StudentSubscriptionPaymentResponseDto>> res = new();
            res.data = new();

            bool saveflag = true;

            RequestLog log = new();

            log.InvokedMethod = System.Reflection.MethodBase.GetCurrentMethod().Name;
            log.InvokedRoute = "PaySubscription";
            log.RequestParameters = JsonConvert.SerializeObject(paymentlist);

            try
            {
                if (paymentlist == null || paymentlist.Count <= 0)
                {
                    res.Message = "Empty request object";
                    res.Status = HttpStatusCode.BadRequest.ToString();

                    return Ok(res);
                }
                else
                {
                    foreach (var p in paymentlist)
                    {
                        if (p.TransactionSource != "Online-RICESMARTWEB" && p.TransactionSource != "Online-LMS")
                        {
                            res.Status = HttpStatusCode.BadRequest.ToString();
                            res.Message = "Invalid Transaction Source";

                            return Ok(res);
                        }
                        res.data.Add(_mapper.Map<StudentSubscriptionPaymentResponseDto>(p));
                    }

                    for (int i = 0; i < res.data.Count; i++)
                    {
                        var p = res.data[i];

                        for (int j = 0; j < p.FeeSchedulePaymentDetails.Count; j++)
                        {
                            var f = p.FeeSchedulePaymentDetails[j];
                            int FeeScheduleID = 0;
                            int ReceiptHeaderID = 0;
                            int SubscriptionTransactionID = 0;
                            DateTime DueDate = DateTime.MinValue;

                            StudentSubscriptionTransaction sub = new();

                            if (p.DueDate != DateTime.MinValue)
                                DueDate = p.DueDate;

                            FeeScheduleID = _data.ValidateSubscriptionPayment(p.SubscriptionID, p.Amount, p.Tax, DueDate,f.FeeScheduleID);

                            if (FeeScheduleID > 0)
                            {
                                sub = _mapper.Map<StudentSubscriptionTransaction>(p);
                                sub.FeeScheduleID = FeeScheduleID;
                                sub.Amount = f.Amount;
                                sub.Tax = f.Tax;

                                SubscriptionTransactionID = _data.SaveSubscriptionTransactionDetails(sub);
                            }

                            if (FeeScheduleID > 0 && SubscriptionTransactionID > 0)
                            {
                                StudentSubscriptionTransaction transaction = new();

                                transaction = _data.GetSubscriptionTransactionDetails(p.TransactionNo, FeeScheduleID);

                                if (transaction != null && transaction.CenterID > 0 && transaction.StudentDetailID > 0
                                    && transaction.IsCompleted == false && transaction.TransactionStatus == "Success")
                                {
                                    res.data[i].FeeSchedulePaymentDetails[j].SubscriptionTransactionID = transaction.SubscriptionTransactionID;

                                    ReceiptHeaderID = _pm.ProcessSubscriptionPayment(transaction, FeeScheduleID);

                                    if (ReceiptHeaderID > 0)
                                    {
                                        res.data[i].FeeSchedulePaymentDetails[j].ReceiptHeaderID = ReceiptHeaderID;
                                        res.data[i].FeeSchedulePaymentDetails[j].IsCompleted = true;

                                        _data.CalculateStudentStatus("FINANCIAL", transaction.StudentDetailID);
                                        _data.InsertStudentStatusInQueue();
                                    }
                                    else
                                    {
                                        saveflag = false;
                                    }
                                }
                                else
                                {
                                    saveflag = false;
                                }
                            }
                            else
                            {
                                saveflag = false;
                            }
                        }
                    }
                }

                if (saveflag)
                {
                    res.Message = "Success";
                    res.Status = HttpStatusCode.Created.ToString();
                }
                else
                {
                    res.Message = "Failure";
                    res.Status = HttpStatusCode.OK.ToString();
                }
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Status = HttpStatusCode.InternalServerError.ToString();

                log.ErrorMessage = res.Message;
                log.RequestResult = res.Status;

                _data.SaveLogs(log);

                return Ok(res);
            }

            log.RequestResult = res.Message;

            if (res.Message != "Success")
                log.ErrorMessage = res.Message;

            _data.SaveLogs(log);

            return Ok(res);
        }

        [Route("PayDuesViaPayOutLink")]
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        public ActionResult<Response<StudentPayOutPaymentResponseDto>> ProcessPayoutTransaction([FromBody] CreateStudentPayOutPaymentDto payment)
        {
            Response<StudentPayOutPaymentResponseDto> res = new();
            res.data = new();
            List<StudentPayOutTransaction> studentPayList = new();

            bool saveflag = true;

            RequestLog log = new();

            log.InvokedMethod = System.Reflection.MethodBase.GetCurrentMethod().Name;
            log.InvokedRoute = "PayDuesViaPayOutLink";
            log.RequestParameters = JsonConvert.SerializeObject(payment);

            try
            {
                if (payment == null)
                {
                    res.Message = "Empty request object";
                    res.Status = HttpStatusCode.BadRequest.ToString();

                    return Ok(res);
                }
                else
                {
                    res.data=_mapper.Map<StudentPayOutPaymentResponseDto>(payment);
                    


                    if (res.data.TransactionSource != "Online-RICESMARTWEB" && res.data.TransactionSource != "Online-LMS")
                    {
                        res.Status = HttpStatusCode.BadRequest.ToString();
                        res.Message = "Invalid Transaction Source";

                        return Ok(res);
                    }

                    //studentPay = _mapper.Map<StudentPayOutTransaction>(res.data);


                    for (int i = 0; i < res.data.FeeScheduleDetails.Count; i++)
                    {
                        var p = res.data.FeeScheduleDetails[i];
                        int FeeScheduleID = 0;
                        int ReceiptHeaderID = 0;
                        int PayoutTransactionID = 0;

                        StudentPayOutTransaction studentPay = new()
                        {
                            CustomerID = res.data.CustomerID,
                            TransactionNo = res.data.TransactionNo,
                            TransactionMode = res.data.TransactionMode,
                            TransactionDate = res.data.TransactionDate,
                            TransactionSource = res.data.TransactionSource,
                            TransactionStatus = res.data.TransactionStatus,
                            Amount=p.Amount,
                            Tax=p.Tax
                            
                        };


                        FeeScheduleID = _data.ValidatePayOutPayment(p.FeeScheduleID, p.Amount, p.Tax);

                        if (FeeScheduleID > 0)
                        {
                            PayoutTransactionID = _data.SavePayoutTransactionDetails(studentPay, FeeScheduleID);
                            studentPay.PayoutTransactionID = PayoutTransactionID;
                            studentPay.FeeScheduleID = FeeScheduleID;
                        }

                        if (studentPay.PayoutTransactionID > 0)
                        {

                            studentPayList = _data.GetPayoutTransactionDetails(studentPay.TransactionNo,studentPay.FeeScheduleID);

                            studentPay = studentPayList.Where(w => w.FeeScheduleID == FeeScheduleID && w.PayoutTransactionID==PayoutTransactionID).FirstOrDefault();

                            if (studentPay != null && studentPay.CenterID > 0 && studentPay.StudentDetailID > 0
                                && studentPay.IsCompleted == false && studentPay.TransactionStatus == "Success")
                            {
                                //res.data.FeeScheduleDetails[i].SubscriptionTransactionID = transaction.SubscriptionTransactionID;

                                ReceiptHeaderID = _pm.ProcessPayoutPayment(studentPay, FeeScheduleID);

                                if (ReceiptHeaderID > 0)
                                {
                                    res.data.FeeScheduleDetails[i].ReceiptHeaderID = ReceiptHeaderID;
                                    res.data.FeeScheduleDetails[i].IsCompleted = true;

                                    _data.CalculateStudentStatus("FINANCIAL", studentPay.StudentDetailID);
                                    _data.InsertStudentStatusInQueue();
                                }
                                else
                                {
                                    saveflag = false;
                                }
                            }
                            else
                            {
                                saveflag = false;
                            }
                        }
                        else
                        {
                            saveflag = false;
                        }
                    }
                }

                if (saveflag)
                {
                    res.Message = "Success";
                    res.Status = HttpStatusCode.Created.ToString();
                }
                else
                {
                    res.Message = "Failure";
                    res.Status = HttpStatusCode.OK.ToString();
                }
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Status = HttpStatusCode.InternalServerError.ToString();

                log.ErrorMessage = res.Message;
                log.RequestResult = res.Status;

                _data.SaveLogs(log);

                return Ok(res);
            }

            log.RequestResult = res.Message;

            if (res.Message != "Success")
                log.ErrorMessage = res.Message;

            _data.SaveLogs(log);

            return Ok(res);
        }

        [Route("PrintFeeSchedule")]
        [HttpGet]
        [Produces("application/json")]
        public ActionResult<Response<List<PrintFeeSchedule>>> GetPrintFeeSchedule(string FeeScheduleIDList)
        {
            Response<List<PrintFeeSchedule>> res = new();
            res.data = new List<PrintFeeSchedule>();

            try
            {
                foreach (var f in FeeScheduleIDList.Split(","))
                {
                    int FeeScheduleID = Convert.ToInt32(f);
                    res.data.Add(_data.GetPrintFeeScheduleDetails(FeeScheduleID));
                }

                if (res.data != null && res.data.Count>0)
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

        [Route("PrintTaxInvoice")]
        [HttpGet]
        [Produces("application/json")]
        public ActionResult<Response<List<PrintTaxInvoice>>> GetPrintTaxInvoice(string ReceiptList)
        {
            Response<List<PrintTaxInvoice>> res = new();
            res.data = new List<PrintTaxInvoice>();

            

            try
            {
                foreach (var f in ReceiptList.Split(","))
                {
                    List<string> Invoices = new();
                    Invoices = _data.GetReceiptInvoices(Convert.ToInt32(f));

                    foreach (var i in Invoices)
                        res.data.Add(_data.GetPrintTaxInvoiceDetails(i, Convert.ToInt32(f)));

                    //res.data.Add(_data.GetPrintFeeScheduleDetails(FeeScheduleID));
                }

                if (res.data != null && res.data.Count > 0)
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
            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Status = HttpStatusCode.InternalServerError.ToString();

                return Ok(res);
            }


            return Ok(res);
        }

        [Route("UpdatePaymentStatus")]
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        public ActionResult<Response<Transaction>> UpdatePaymentStatus(string TransactionNo, string TransactionStatus)
        {
            Response<Transaction> res = new();
            //bool IsValid = false;

            Transaction transaction = new();

            RequestLog log = new();

            log.InvokedMethod = System.Reflection.MethodBase.GetCurrentMethod().Name;
            log.InvokedRoute = "UpdatePaymentStatus";
            log.UniqueAttributeName = "TransactionNo";
            log.UniqueAttributeValue = TransactionNo;
            log.RequestParameters = $"TransactionNo:{TransactionNo}, TransactionStatus:{TransactionStatus}";

            try
            {
                if(TransactionNo==null || TransactionNo.Trim()=="")
                {
                    res.Status = HttpStatusCode.BadRequest.ToString();
                    res.Message = "TransactionNo cannot be blank";

                    return Ok(res);
                }

                if (TransactionStatus == null || TransactionStatus.Trim() == "")
                {
                    res.Status = HttpStatusCode.BadRequest.ToString();
                    res.Message = "Transaction Status cannot be blank";

                    return Ok(res);
                }
                else if(TransactionStatus!="Success" && TransactionStatus!="Failure")
                {
                    res.Status = HttpStatusCode.BadRequest.ToString();
                    res.Message = "Invalid Transaction Status";

                    return Ok(res);
                }
                else
                {
                    _data.UpdateTransactionStatus(transaction.TransactionNo, TransactionStatus);

                    transaction = _data.GetTransactionDetails(TransactionNo);
                    //_data.SaveTransactionDetails(_mapper.Map<Payment>(createPayment));
                    //_pm.ProcessPayment(_mapper.Map<Payment>(createPayment));

                    _pm.ProcessExistingTransaction(transaction, TransactionStatus);

                    transaction = _data.GetTransactionDetails(TransactionNo);
                    res.data = transaction;

                    if (transaction != null)
                    {
                        ECommService service = new();

                        service.SendTransactionNotification(transaction).Wait();
                    }

                    if (transaction.IsCompleted)
                    {
                        res.Status = HttpStatusCode.OK.ToString();
                        res.Message = "Success";
                    }
                    else
                    {
                        res.Status = HttpStatusCode.OK.ToString();
                        res.Message = "Failure";
                    }


                }
            }
            catch (Exception ex)
            {
                res.Status = HttpStatusCode.BadRequest.ToString();
                res.Message = ex.Message;

                log.ErrorMessage = res.Message;
                log.RequestResult = res.Status;

                _data.SaveLogs(log);

                return Ok(res);
            }

            log.RequestResult = res.Message;

            if (res.Message != "Success")
                log.ErrorMessage = res.Message;

            _data.SaveLogs(log);

            return Ok(res);
        }

        [Route("UpdatePayoutPaymentStatus")]
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        public ActionResult<Response<Transaction>> UpdatePayOutPaymentStatus(string TransactionNo, string TransactionStatus)
        {
            Response<List<StudentPayOutTransaction>> res = new();
            res.data = new List<StudentPayOutTransaction>();
            //bool IsValid = false;

            List<StudentPayOutTransaction> transactions = new();

            RequestLog log = new();

            log.InvokedMethod = System.Reflection.MethodBase.GetCurrentMethod().Name;
            log.InvokedRoute = "UpdatePayoutPaymentStatus";
            log.UniqueAttributeName = "TransactionNo";
            log.UniqueAttributeValue = TransactionNo;
            log.RequestParameters = $"TransactionNo:{TransactionNo}, TransactionStatus:{TransactionStatus}";

            try
            {

                if (TransactionStatus == null || TransactionStatus.Trim() == "")
                {
                    res.Status = HttpStatusCode.BadRequest.ToString();
                    res.Message = "Transaction Status cannot be blank";

                    return Ok(res);
                }
                else if (TransactionStatus != "Success" && TransactionStatus != "Failure")
                {
                    res.Status = HttpStatusCode.BadRequest.ToString();
                    res.Message = "Invalid Transaction Status";

                    return Ok(res);
                }
                else
                {
                    transactions = _data.GetPayoutTransactionDetails(TransactionNo);

                    for(int i=0;i<transactions.Count;i++)
                    {
                        StudentPayOutTransaction transaction = new();
                        transaction = transactions[i];

                        if(transaction.IsCompleted==false)
                            transactions[i].ReceiptHeaderID=_pm.ProcessExistingPayoutPayment(transaction, TransactionStatus);

                        if (transactions[i].ReceiptHeaderID > 0)
                        {
                            _data.CalculateStudentStatus("FINANCIAL", transactions[i].StudentDetailID);
                            _data.InsertStudentStatusInQueue();
                        }
                    }

                    transactions = _data.GetPayoutTransactionDetails(TransactionNo);
                    res.data = transactions;

                    foreach (var transaction in transactions)
                    {
                        if(transaction.IsCompleted==false)
                        {
                            res.Message = "Failure";
                            res.Status = HttpStatusCode.OK.ToString();

                            break;
                        }
                        else
                        {
                            res.Message = "Success";
                            res.Status = HttpStatusCode.OK.ToString();
                        }
                    }

                    //if (transaction.IsCompleted)
                    //{
                    //    res.Status = HttpStatusCode.OK.ToString();
                    //    res.Message = "Success";
                    //}
                    //else
                    //{
                    //    res.Status = HttpStatusCode.OK.ToString();
                    //    res.Message = "Failure";
                    //}


                }
            }
            catch (Exception ex)
            {
                res.Status = HttpStatusCode.BadRequest.ToString();
                res.Message = ex.Message;

                log.ErrorMessage = res.Message;
                log.RequestResult = res.Status;

                _data.SaveLogs(log);

                return Ok(res);
            }

            log.RequestResult = res.Message;

            if (res.Message != "Success")
                log.ErrorMessage = res.Message;

            _data.SaveLogs(log);

            return Ok(res);
        }

        [Route("GetCustomerMandateLinks")]
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        public ActionResult<Response<List<GetCustomerMandateLinkDto>>> GetCustomerMandateLinksForLMS(string CustomerID)
        {
            Response<List<GetCustomerMandateLinkDto>> res = new();
            res.data = new List<GetCustomerMandateLinkDto>();
            List<ProductSubscription> subscriptions = new();

            string mandatelinkpart = "user/mandate-register";

            try
            {
                subscriptions = _data.GetCustomerMandateLinks(CustomerID);

                foreach (var s in subscriptions)
                {
                    if (s.MandateLink == null)
                        s.MandateLink = _config.GetValue<string>("EcommWebUrl") + mandatelinkpart;

                    res.data.Add(_mapper.Map<GetCustomerMandateLinkDto>(s));
                }

                if (res.data.Count > 0)
                {
                    res.Message = "Success";
                    res.Status = HttpStatusCode.OK.ToString();
                }
                else
                {
                    res.Message = "No data found";
                    res.Status = HttpStatusCode.NotFound.ToString();
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

        [Route("GetCustomerOrders")]
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        public ActionResult<Response<List<GetOrderDto>>> GetCustomerOrders(string CustomerID)
        {
            Response<List<GetOrderDto>> res = new();
            res.data = new List<GetOrderDto>();
            List<Order> orders = new();

            try
            {
                orders = _data.GetOrders(CustomerID);

                foreach (var s in orders)
                {
                    res.data.Add(_mapper.Map<GetOrderDto>(s));
                }

                if (res.data.Count > 0)
                {
                    res.Message = "Success";
                    res.Status = HttpStatusCode.OK.ToString();
                }
                else
                {
                    res.Message = "No data found";
                    res.Status = HttpStatusCode.NotFound.ToString();
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
