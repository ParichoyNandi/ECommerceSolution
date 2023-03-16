using ECommManagement.Services;
using Entities;
using Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Utilities;

namespace ECommManagement
{
    public class PaymentManagement: IPaymentManagement
    {
        private string _conn;
        private IDBAccess _data;

        public PaymentManagement(IConfiguration config, IDBAccess data)
        {
            _conn = config.GetConnectionString("DevConn");
            _data = data;
        }
        public void ProcessPayment(Payment pay)
        {
            
            int RegID = 0;
            Registration reg = new();
            Transaction transaction = new();

            try
            {
                //reg.Address = pay.Address;
                //reg.CityID = pay.CityID;
                //reg.CountryID = pay.CountryID;
                //reg.Pincode = pay.Pincode;
                reg.CustomerID = pay.CustomerID;
                //reg.StateID = pay.StateID;

                RegID = _data.UpdateRegistration(reg);

                

                if (RegID > 0)
                {
                    transaction = _data.GetTransactionDetails(pay.TransactionNo);

                    if (transaction.CanBeProcessed && transaction.IsCompleted == false && transaction.TransactionStatus== "Success")
                    {
                        foreach (var pl in transaction.PlanDetails)
                        {
                            if (pl.CanBeProcessed && pl.IsCompleted == false)
                            {
                                foreach (var pr in pl.ProductDetails)
                                {
                                    if (pr.IsCompleted == false && pr.CanBeProcessed)
                                    {
                                        int EnquiryID = 0;
                                        int StudentDetailID = 0;

                                        EnquiryID = _data.GetEnquiryDetails(RegID, pr.CenterID, pr.BatchID, 0);

                                        if (EnquiryID > 0)
                                        {
                                            StudentDetailID = _data.VerifyAdmissionType(EnquiryID);

                                            if(StudentDetailID>0)
                                                AdmissionFromStudentID(StudentDetailID, pr, pay.CustomerID);
                                            else
                                                AdmissionFromEnquiry(EnquiryID, pr, pay.CustomerID);


                                        }

                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    throw new Exception($"{pay.CustomerID} does not exist");
                }


                
            }
            catch (Exception ex)
            { 
                throw ex;
            }

        }

        public void ProcessExistingTransaction(Transaction pay, string TransactionStatus)
        {

            int RegID = 0;
            Registration reg = new();
            Transaction transaction = new();

            try
            {
                //reg.Address = pay.Address;
                //reg.CityID = pay.CityID;
                //reg.CountryID = pay.CountryID;
                //reg.Pincode = pay.Pincode;
                reg.CustomerID = pay.CustomerID;
                //reg.StateID = pay.StateID;

                RegID = _data.UpdateRegistration(reg);



                if (RegID > 0)
                {
                    transaction = _data.GetTransactionDetails(pay.TransactionNo);

                    if (transaction.CanBeProcessed && transaction.IsCompleted == false && TransactionStatus == "Success")
                    {
                        foreach (var pl in transaction.PlanDetails)
                        {
                            if (pl.CanBeProcessed && pl.IsCompleted == false)
                            {
                                foreach (var pr in pl.ProductDetails)
                                {
                                    if (pr.IsCompleted == false && pr.CanBeProcessed)
                                    {
                                        int EnquiryID = 0;
                                        int StudentDetailID = 0;

                                        EnquiryID = _data.GetEnquiryDetails(RegID, pr.CenterID, pr.BatchID, 0);

                                        if (EnquiryID > 0)
                                        {
                                            StudentDetailID = _data.VerifyAdmissionType(EnquiryID);

                                            if (StudentDetailID > 0)
                                                AdmissionFromStudentID(StudentDetailID, pr,pay.CustomerID);
                                            else
                                                AdmissionFromEnquiry(EnquiryID, pr, pay.CustomerID);


                                        }

                                    }
                                }
                            }
                        }

                        
                    }

                    
                }



            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public int ProcessSubscriptionPayment(StudentSubscriptionTransaction transaction,int FeeScheduleID)
        {
            FeeSchedule newFeeSchedule = new();
            Receipt receipt = new();

            DataHelper dh = new(_conn);
            SqlTransaction trans = dh.DataConn.BeginTransaction(IsolationLevel.ReadCommitted);

            try
            {
                if (FeeScheduleID > 0)
                {
                    newFeeSchedule.InvoiceId = FeeScheduleID;
                    _data.GetInvoiceDetail(newFeeSchedule, dh, trans);

                    PrepareSubscriptionReceiptData(receipt, newFeeSchedule, transaction, transaction.StudentDetailID);
                    _data.GetReceipt(receipt, dh, trans);
                    receipt.GenerateReceipt();

                    _data.InsertReceiptObject(receipt, transaction.BrandID, dh, trans);

                    if (receipt.ReceiptId > 0 && receipt.Invoice.InvoiceId > 0)
                    {
                        _data.UpdateReceiptDetail(receipt, dh, trans);
                    }

                    if (receipt.ReceiptId > 0)
                        _data.UpdateSubscriptionTransactionDetails(transaction.SubscriptionTransactionID, receipt.ReceiptId,transaction.TransactionStatus, dh, trans);

                    trans.Commit();
                }
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
            finally
            {
                trans.Dispose();
                dh.DataConn.Close();
            }

            return receipt.ReceiptId;
        }

        public int ProcessPayoutPayment(StudentPayOutTransaction transaction, int FeeScheduleID)
        {
            FeeSchedule newFeeSchedule = new();
            Receipt receipt = new();

            DataHelper dh = new(_conn);
            SqlTransaction trans = dh.DataConn.BeginTransaction(IsolationLevel.ReadCommitted);

            try
            {
                if (FeeScheduleID > 0 && transaction.TransactionStatus=="Success")
                {
                    newFeeSchedule.InvoiceId = FeeScheduleID;
                    _data.GetInvoiceDetail(newFeeSchedule, dh, trans);

                    PreparePayoutReceiptData(receipt, newFeeSchedule, transaction, transaction.StudentDetailID);
                    _data.GetReceipt(receipt, dh, trans);
                    receipt.GenerateReceipt();

                    _data.InsertReceiptObject(receipt, transaction.BrandID, dh, trans);

                    if (receipt.ReceiptId > 0 && receipt.Invoice.InvoiceId > 0)
                    {
                        _data.UpdateReceiptDetail(receipt, dh, trans);
                    }

                    if (receipt.ReceiptId > 0)
                        _data.UpdatePayoutTransactionDetails(transaction.PayoutTransactionID, receipt.ReceiptId,transaction.TransactionStatus, dh, trans);

                    trans.Commit();
                }
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
            finally
            {
                trans.Dispose();
                dh.DataConn.Close();
            }

            return receipt.ReceiptId;
        }

        public int ProcessExistingPayoutPayment(StudentPayOutTransaction transaction, string TransactionStatus)
        {
            FeeSchedule newFeeSchedule = new();
            Receipt receipt = new();
            int FeeScheduleID = transaction.FeeScheduleID;

            DataHelper dh = new(_conn);
            SqlTransaction trans = dh.DataConn.BeginTransaction(IsolationLevel.ReadCommitted);

            try
            {
                if(TransactionStatus=="Failure" && transaction.TransactionStatus == "Pending")
                {
                    _data.UpdatePayoutTransactionDetails(transaction.PayoutTransactionID, 0, TransactionStatus, dh, trans);
                }
                else if (FeeScheduleID > 0 && transaction.TransactionStatus=="Pending" && TransactionStatus=="Success")
                {
                    newFeeSchedule.InvoiceId = FeeScheduleID;
                    _data.GetInvoiceDetail(newFeeSchedule, dh, trans);

                    PreparePayoutReceiptData(receipt, newFeeSchedule, transaction, transaction.StudentDetailID);
                    _data.GetReceipt(receipt, dh, trans);
                    receipt.GenerateReceipt();

                    _data.InsertReceiptObject(receipt, transaction.BrandID, dh, trans);

                    if (receipt.ReceiptId > 0 && receipt.Invoice.InvoiceId > 0)
                    {
                        _data.UpdateReceiptDetail(receipt, dh, trans);
                    }

                    if (receipt.ReceiptId > 0)
                        _data.UpdatePayoutTransactionDetails(transaction.PayoutTransactionID, receipt.ReceiptId, TransactionStatus, dh, trans);

                    trans.Commit();
                }
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
            finally
            {
                trans.Dispose();
                dh.DataConn.Close();
            }

            return receipt.ReceiptId;
        }

        public void AdmissionFromEnquiry(int EnquiryID, ProductTransaction pr, string CustomerID)
        {
            string StudentID = "";
            int StudentDetailID = 0;
            string CourseXML = "";
            string InvoiceXML = "";
            int FeeScheduleID = 0;
            FeeSchedule newFeeSchedule = new();
            Receipt receipt = new();

            List<FeeStructure> feeplan = new();
            DataHelper dh = new DataHelper(_conn);
            SqlTransaction trans = dh.DataConn.BeginTransaction(IsolationLevel.ReadCommitted);

            try
            {
                _data.CloseExistingSameCourseAccounts(CustomerID, pr.ProductID, trans, dh);

                StudentID = GetStudentID(pr.BrandID, pr.BatchStartDate);

                StudentDetailID = _data.uspInsertStudentDetailsFromEnquiry(EnquiryID, StudentID, "rice-group-admin", dh, trans);

                if (StudentDetailID > 0)
                {
                    
                    CourseXML = GetCourseXML(StudentDetailID, pr);
                    _data.uspInsertStudentBatchDetails(CourseXML, "rice-group-admin", dh, trans);

                    if (pr.PaymentModeID == 1)
                        feeplan = _data.GetFeePlanDetails(pr.FeePlanID, pr.PaymentModeID, pr.DiscountSchemeID, pr.BatchStartDate).Where(w => w.IsLumpsum == "Y").ToList();
                    else if (pr.PaymentModeID == 2)
                        feeplan = _data.GetFeePlanDetails(pr.FeePlanID, pr.PaymentModeID, pr.DiscountSchemeID, pr.BatchStartDate).Where(w => w.IsLumpsum == "N").ToList();

                    if (feeplan.Count > 0)
                    {
                        FeeSchedule feeSchedule = new();
                        feeSchedule = GenerateFeeScheduleFromFeePlan(feeSchedule, feeplan, StudentDetailID, pr);
                        InvoiceXML = GetInvoiceXML(feeSchedule);
                        FeeScheduleID = _data.InsertInvoice(InvoiceXML, pr.BrandID, dh, trans);

                        if (FeeScheduleID > 0)
                        {
                            newFeeSchedule.InvoiceId = FeeScheduleID;
                            _data.SetAdmissionFeeSchedule(FeeScheduleID, trans, dh);
                            _data.GetInvoiceDetail(newFeeSchedule, dh, trans);

                            PrepareReceiptData(receipt, newFeeSchedule, pr, StudentDetailID);
                            _data.GetReceipt(receipt, dh, trans);
                            receipt.GenerateReceipt();

                            _data.InsertReceiptObject(receipt, pr.BrandID, dh, trans);

                            if (receipt.ReceiptId > 0 && receipt.Invoice.InvoiceId > 0)
                            {
                                _data.UpdateReceiptDetail(receipt, dh, trans);
                            }
                        }
                    }

                    if (FeeScheduleID > 0 && StudentDetailID > 0 && receipt.ReceiptId > 0)
                    {
                        _data.UpdateEnquiryOnAdmission(EnquiryID, trans, dh);
                        _data.InsertStudentLoginDetails(StudentID, 0, trans, dh);

                        int q = 0;
                        _data.UpdateTransactionDetails(pr.TransactionProductDetailID, FeeScheduleID, StudentDetailID, receipt.ReceiptId, dh, trans);
                        //q=_data.InsertStudentBatchInQueue(StudentDetailID, pr.BatchID, "NEW", trans, dh);

                        //if (q == 0)
                        //    throw new Exception($"Unable to insert student in queue. TransactionProductDetailID: {pr.TransactionProductDetailID}");
                    }

                    trans.Commit();

                    //if(receipt.ReceiptId>0)
                    //{
                    //    LMSSyncService lms = new();
                    //    lms.AddNewStudent(StudentDetailID).Wait(30000);
                        
                    //}
                }
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
            finally
            {
                trans.Dispose();
                dh.DataConn.Close();
            }

        }

        public void AdmissionFromStudentID(int StudentDetailID, ProductTransaction pr, string CustomerID)
        {
            //string StudentID = "";
            string CourseXML = "";
            string InvoiceXML = "";
            int FeeScheduleID = 0;
            FeeSchedule newFeeSchedule = new();
            Receipt receipt = new();

            List<FeeStructure> feeplan = new();
            DataHelper dh = new DataHelper(_conn);
            SqlTransaction trans = dh.DataConn.BeginTransaction(IsolationLevel.ReadCommitted);

            try
            {
                //StudentID = GetStudentID(pr.BrandID, pr.BatchStartDate);

                //StudentDetailID = _data.uspInsertStudentDetailsFromEnquiry(EnquiryID, StudentID, "rice-group-admin", dh, trans);

                if (StudentDetailID > 0)
                {
                    _data.CloseExistingSameCourseAccounts(CustomerID, pr.ProductID, trans, dh);

                    CourseXML = GetCourseXML(StudentDetailID, pr);
                    _data.uspInsertStudentBatchDetails(CourseXML, "rice-group-admin", dh, trans);

                    if (pr.PaymentModeID == 1)
                        feeplan = _data.GetFeePlanDetails(pr.FeePlanID, pr.PaymentModeID, pr.DiscountSchemeID, pr.BatchStartDate).Where(w => w.IsLumpsum == "Y").ToList();
                    else if (pr.PaymentModeID == 2)
                        feeplan = _data.GetFeePlanDetails(pr.FeePlanID, pr.PaymentModeID, pr.DiscountSchemeID, pr.BatchStartDate).Where(w => w.IsLumpsum == "N").ToList();

                    if (feeplan.Count > 0)
                    {
                        FeeSchedule feeSchedule = new();
                        feeSchedule = GenerateFeeScheduleFromFeePlan(feeSchedule, feeplan, StudentDetailID, pr);
                        InvoiceXML = GetInvoiceXML(feeSchedule);
                        FeeScheduleID = _data.InsertInvoice(InvoiceXML, pr.BrandID, dh, trans);

                        if (FeeScheduleID > 0)
                        {
                            newFeeSchedule.InvoiceId = FeeScheduleID;
                            _data.SetAdmissionFeeSchedule(FeeScheduleID, trans, dh);
                            _data.GetInvoiceDetail(newFeeSchedule, dh, trans);

                            PrepareReceiptData(receipt, newFeeSchedule, pr, StudentDetailID);
                            _data.GetReceipt(receipt, dh, trans);
                            receipt.GenerateReceipt();

                            _data.InsertReceiptObject(receipt, pr.BrandID, dh, trans);

                            if (receipt.ReceiptId > 0 && receipt.Invoice.InvoiceId > 0)
                            {
                                _data.UpdateReceiptDetail(receipt, dh, trans);
                            }
                        }

                    }

                    if (FeeScheduleID > 0 && StudentDetailID > 0 && receipt.ReceiptId > 0)
                    {
                        int q = 0;
                        _data.UpdateTransactionDetails(pr.TransactionProductDetailID, FeeScheduleID, StudentDetailID, receipt.ReceiptId, dh, trans);
                        //q = _data.InsertStudentBatchInQueue(StudentDetailID, pr.BatchID, "ADD", trans, dh);

                        //if (q == 0)
                        //    throw new Exception($"Unable to insert student in queue. TransactionProductDetailID: {pr.TransactionProductDetailID}");
                    }

                    trans.Commit();

                    //if (receipt.ReceiptId > 0)
                    //{
                    //    LMSSyncService lms = new();
                    //    lms.UpdateStudentBatch(StudentDetailID).Wait(30000);

                    //}
                }
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
            finally
            {
                trans.Dispose();
                dh.DataConn.Close();
            }

        }


        #region PrivateMethods
        private string GetStudentID(int BrandID, DateTime dtBatchStartDate)
        {
            string StudentID = string.Empty;
            string FY = Helper.GetCurrentFinancialYear(BrandID, dtBatchStartDate);
            string[] Year = FY.ToString().Split('-');
            int rollno;
            switch (BrandID)
            {
                case 109: //for RICE
                    StudentID = FY + "/" + "RICE" + "/";
                    rollno = _data.GetRollNo(StudentID);
                    StudentID = StudentID + rollno.ToString().PadLeft(3, '0');
                    break;
                    //case 111://for adamas career
                    //    StudentID = FY + "/" + Enumerators.Brand.AC.ToString() + "/";
                    //    rollno = CenterManager.GetRollNo(StudentID);
                    //    StudentID = StudentID + rollno.ToString().PadLeft(3, '0');
                    //    break;
                    //case 107: //for AIS
                    //    StudentID = iBatchyear.ToString().Substring(2, 2) + "-";
                    //    rollno = CenterManager.GetRollNo(StudentID);
                    //    StudentID = StudentID + rollno.ToString().PadLeft(4, '0');
                    //    break;
                    //case (int)Enumerators.Brand.AWS: //for AWS
                    //    //StudentID = rollno.ToString().PadLeft(3, '0') + "/" + Enumerators.Brand.AWS.ToString() + "/" + FY;
                    //    StudentID = FY + "/" + Enumerators.Brand.AWS.ToString() + "/";
                    //    rollno = CenterManager.GetRollNo(StudentID);
                    //    StudentID = StudentID + rollno.ToString().PadLeft(3, '0');
                    //    break;
                    //case (int)Enumerators.Brand.AHSMS: //for AHSMS
                    //    //StudentID = rollno.ToString().PadLeft(3, '0') + "/" + Enumerators.Brand.AWS.ToString() + "/" + FY;
                    //    StudentID = FY + "/" + Enumerators.Brand.AHSMS.ToString() + "/";
                    //    rollno = CenterManager.GetRollNo(StudentID);
                    //    StudentID = StudentID + rollno.ToString().PadLeft(3, '0');
                    //    break;

            }


            return StudentID;
        }

        private string GetCourseXML(int StudentDetailID, ProductTransaction pr)
        {
            XmlDocument xDoc = new XmlDocument();
            XmlElement xStudent;
            XmlElement xCourse;

            xStudent = (XmlElement)xDoc.CreateElement("Student");
            xCourse= (XmlElement)xDoc.CreateElement("Course");
            xStudent.SetAttribute("I_Student_Detail_ID", StudentDetailID.ToString());
            xCourse.SetAttribute("I_Centre_ID", pr.CenterID.ToString());
            xCourse.SetAttribute("I_Batch_ID", pr.BatchID.ToString());
            xCourse.SetAttribute("I_Course_ID", pr.CourseID.ToString());
            xCourse.SetAttribute("I_Delivery_Pattern_ID", pr.DeliveryPatternID.ToString());
            xCourse.SetAttribute("I_Fee_Plan_ID", pr.FeePlanID.ToString());

            if(pr.PaymentModeID==1)
                xCourse.SetAttribute("C_Is_LumpSum", "Y");
            else
                xCourse.SetAttribute("C_Is_LumpSum", "N");

            xCourse.SetAttribute("Dt_Course_Start_Date", pr.BatchStartDate.ToString("yyyy-MM-dd"));

            xStudent.AppendChild(xCourse);
            xDoc.AppendChild(xStudent);


            return xDoc.InnerXml;
        }

        private FeeSchedule GenerateFeeScheduleFromFeePlan(FeeSchedule feeSchedule, List<FeeStructure> feeStructure,int StudentDetailID, ProductTransaction pr)
        {
            decimal totaldiscountamt = 0;
            decimal totalinvoiceamt = 0;
            decimal totalinvoicetaxamt = 0;
            int childno = 1;

            feeSchedule.InvoiceId = -1;
            feeSchedule.StudentDetailID = StudentDetailID;
            feeSchedule.CenterID = pr.CenterID;
            feeSchedule.CouponDiscount = 0;
            feeSchedule.InvoiceDate = DateTime.Now;
            feeSchedule.InvoiceStatus = 1;
            feeSchedule.DiscountSchemeId = pr.DiscountSchemeID;
            feeSchedule.DiscountAppliedAt = 0;
            feeSchedule.CreatedBy = "rice-group-admin";
            feeSchedule.CreatedOn = DateTime.Now;

            InvoiceChild invoiceChild = new();

            invoiceChild.InvoiceChildID = 1;
            invoiceChild.InvoiceHeaderID = feeSchedule.InvoiceId;
            invoiceChild.CourseID = pr.CourseID;
            invoiceChild.BatchID = pr.BatchID;
            invoiceChild.DiscountSchemeId = pr.DiscountSchemeID;
            invoiceChild.DiscountAppliedAt = 0;
            invoiceChild.CourseFeePlanID = pr.FeePlanID;

            if (pr.PaymentModeID == 1)
                invoiceChild.IsLumpSum = "Y";
            else
                invoiceChild.IsLumpSum = "N";

            foreach(var f in feeStructure)
            {
                InvoiceChildDetails details = null;

                if(f.Amount>0)
                {
                    details = new();

                    details.InvoiceChildDetailsId = childno;
                    details.InvoiceChildHeaderId = invoiceChild.InvoiceChildID;
                    details.FeeComponentId = f.FeeComponentID;
                    details.InstallmentNo = f.InstalmentNo;
                    details.DueDate = f.InstalmentDate;
                    details.AmountDue = (double)f.Amount;
                    details.DiscountAmount = (double)f.DiscountAmount;
                    details.PayableAmount = (double)f.FinalAmount;
                    details.DisplayFeeComponentId = f.DisplayFeeComponentID;
                    details.Sequence = f.Sequence;

                    totalinvoiceamt += f.Amount;
                    totaldiscountamt += f.DiscountAmount;

                    foreach (var t in f.TaxDetails)
                    {
                        InvoiceTaxDetails invoiceTax = new();

                        invoiceTax.InvoiceChildDetailId = details.InvoiceChildDetailsId;
                        invoiceTax.TaxId = t.TaxID;
                        invoiceTax.TaxValue = (double)t.TaxAmount;

                        totalinvoicetaxamt += t.TaxAmount;

                        details.InvoiceTaxDetails.Add(invoiceTax);
                    }

                    invoiceChild.InvoiceChildDetails.Add(details);
                    childno += 1;
                }
            }

            invoiceChild.CourseAmount = (double)totalinvoiceamt;
            invoiceChild.CourseDiscount = (double)totaldiscountamt;

            feeSchedule.InvoiceChild.Add(invoiceChild);

            feeSchedule.InvoiceAmount = (double)totalinvoiceamt;
            feeSchedule.DiscountAmount = (double)totaldiscountamt;
            feeSchedule.TotalTaxAmount = (double)totalinvoicetaxamt;

            return feeSchedule;
        }

        public string GetInvoiceXML(FeeSchedule schedule)
        {
            XmlDocument xDoc = new XmlDocument();
            XmlElement xInvoice;
            XmlElement xInvoiceChild;
            XmlElement xInvoiceChildDetails;
            XmlElement xInvoiceTaxDetails;
            XmlElement xTaxDetails;
            XmlElement xInstallment;


            xInvoice = (XmlElement)xDoc.CreateElement("Invoice");
            xInvoice.SetAttribute("I_Invoice_Header_ID", schedule.InvoiceId.ToString());
            xInvoice.SetAttribute("I_Student_Detail_ID", schedule.StudentDetailID.ToString());
            xInvoice.SetAttribute("I_Centre_Id", schedule.CenterID.ToString());
            xInvoice.SetAttribute("N_Invoice_Amount", schedule.InvoiceAmount.ToString());
            xInvoice.SetAttribute("N_Discount_Amount", schedule.DiscountAmount.ToString());
            xInvoice.SetAttribute("N_Tax_Amount", schedule.TotalTaxAmount.ToString());
            xInvoice.SetAttribute("I_Coupon_Discount", schedule.CouponDiscount.ToString());
            xInvoice.SetAttribute("Dt_Invoice_Date", schedule.InvoiceDate.ToString("yyyy-MM-dd HH:mm:ss"));
            xInvoice.SetAttribute("I_Status", Convert.ToString(1));
            xInvoice.SetAttribute("I_Discount_Scheme_ID", schedule.DiscountSchemeId.ToString());
            xInvoice.SetAttribute("I_Discount_Applied_At", schedule.DiscountAppliedAt.ToString());
            xInvoice.SetAttribute("S_Crtd_By", schedule.CreatedBy.ToString());
            xInvoice.SetAttribute("Dt_Crtd_On", schedule.CreatedOn.ToString("yyyy-MM-dd HH:mm:ss"));

            for (int i = 0; i < schedule.InvoiceChild.Count; i++)
            {
                xInvoiceChild = (XmlElement)xDoc.CreateElement("InvoiceChild");
                xInvoiceChild.SetAttribute("I_Invoice_Child_Header_ID", schedule.InvoiceChild[i].InvoiceChildID.ToString());
                xInvoiceChild.SetAttribute("I_Invoice_Header_ID", schedule.InvoiceChild[i].InvoiceHeaderID.ToString());
                xInvoiceChild.SetAttribute("I_Course_ID", schedule.InvoiceChild[i].CourseID.ToString());
                if (schedule.InvoiceChild[i].BatchID>0)
                {
                    xInvoiceChild.SetAttribute("I_Batch_ID", schedule.InvoiceChild[i].BatchID.ToString());
                }
                //Add By UC
                if (!string.IsNullOrEmpty(schedule.InvoiceChild[i].CourseDiscount.ToString()))
                {
                    xInvoiceChild.SetAttribute("N_Discount_Amount", schedule.InvoiceChild[i].CourseDiscount.ToString());
                }
                if (!string.IsNullOrEmpty(schedule.InvoiceChild[i].DiscountSchemeId.ToString()))
                {
                    xInvoiceChild.SetAttribute("I_Discount_Scheme_ID", schedule.InvoiceChild[i].DiscountSchemeId.ToString());
                }
                if (!string.IsNullOrEmpty(schedule.InvoiceChild[i].DiscountAppliedAt.ToString()))
                {
                    xInvoiceChild.SetAttribute("I_Discount_Applied_At", schedule.InvoiceChild[i].DiscountAppliedAt.ToString());
                }

                if (schedule.InvoiceChild[i].CourseFeePlanID>0)
                {
                    xInvoiceChild.SetAttribute("I_Course_FeePlan_ID", schedule.InvoiceChild[i].CourseFeePlanID.ToString());
                }
                else
                {
                    xInvoiceChild.SetAttribute("I_Course_FeePlan_ID", schedule.InvoiceChild[i].CourseFeePlanID.ToString());
                }
                if ((schedule.InvoiceChild[i].CourseFeePlanID>0) && (!string.IsNullOrEmpty(schedule.InvoiceChild[i].IsLumpSum)))
                {
                    xInvoiceChild.SetAttribute("C_Is_LumpSum", schedule.InvoiceChild[i].IsLumpSum.ToString());
                }
                else
                {
                    xInvoiceChild.SetAttribute("C_Is_LumpSum", schedule.InvoiceChild[i].IsLumpSum.ToString());
                }

                xInvoiceChildDetails = (XmlElement)xDoc.CreateElement("InvoiceChildDetails");
                for (int j = 0; j < schedule.InvoiceChild[i].InvoiceChildDetails.Count; j++)
                {
                    xInstallment = (XmlElement)xDoc.CreateElement("Installment");

                    xInstallment.SetAttribute("I_Invoice_Detail_ID", schedule.InvoiceChild[i].InvoiceChildDetails[j].InvoiceChildDetailsId.ToString());
                    xInstallment.SetAttribute("I_Fee_Component_ID", schedule.InvoiceChild[i].InvoiceChildDetails[j].FeeComponentId.ToString());
                    xInstallment.SetAttribute("I_Invoice_Child_Header_ID", schedule.InvoiceChild[i].InvoiceChildDetails[j].InvoiceChildHeaderId.ToString());
                    xInstallment.SetAttribute("I_Installment_No", schedule.InvoiceChild[i].InvoiceChildDetails[j].InstallmentNo.ToString());
                    xInstallment.SetAttribute("Dt_Installment_Date", schedule.InvoiceChild[i].InvoiceChildDetails[j].DueDate.ToString("yyyy-MM-dd HH:mm:ss"));
                    xInstallment.SetAttribute("N_Amount_Due", Math.Round(schedule.InvoiceChild[i].InvoiceChildDetails[j].AmountDue, 5).ToString());
                    //After Discount Akash 1.10.2021

                    xInstallment.SetAttribute("N_Amount_Discount", Math.Round(schedule.InvoiceChild[i].InvoiceChildDetails[j].DiscountAmount, 5).ToString());
                    xInstallment.SetAttribute("N_Amount_Payable", Math.Round(schedule.InvoiceChild[i].InvoiceChildDetails[j].PayableAmount, 5).ToString());

                    //After Discount Akash 1.10.2021
                    xInstallment.SetAttribute("I_Display_Fee_Component_ID", schedule.InvoiceChild[i].InvoiceChildDetails[j].DisplayFeeComponentId.ToString());
                    xInstallment.SetAttribute("I_Sequence", schedule.InvoiceChild[i].InvoiceChildDetails[j].Sequence.ToString());
                    //

                    xInvoiceTaxDetails = (XmlElement)xDoc.CreateElement("InvoiceChildTaxDetails");
                    for (int k = 0; k < schedule.InvoiceChild[i].InvoiceChildDetails[j].InvoiceTaxDetails.Count; k++)
                    {
                        xTaxDetails = (XmlElement)xDoc.CreateElement("TaxDetails");
                        xTaxDetails.SetAttribute("I_Tax_ID", schedule.InvoiceChild[i].InvoiceChildDetails[j].InvoiceTaxDetails[k].TaxId.ToString());
                        xTaxDetails.SetAttribute("I_Invoice_Detail_ID", schedule.InvoiceChild[i].InvoiceChildDetails[j].InvoiceTaxDetails[k].InvoiceChildDetailId.ToString());
                        xTaxDetails.SetAttribute("N_Tax_Value", Math.Round(schedule.InvoiceChild[i].InvoiceChildDetails[j].InvoiceTaxDetails[k].TaxValue, 5).ToString());

                        xInvoiceTaxDetails.AppendChild(xTaxDetails);
                    }
                    xInstallment.AppendChild(xInvoiceTaxDetails);
                    //
                    xInvoiceChildDetails.AppendChild(xInstallment);

                }
                xInvoiceChild.AppendChild(xInvoiceChildDetails);
                xInvoice.AppendChild(xInvoiceChild);
            }


            xDoc.AppendChild(xInvoice);

            return xDoc.InnerXml;
        }

        private void PrepareReceiptData(Receipt receipt,FeeSchedule feeSchedule,ProductTransaction pr,int StudentDetailID)
        {
            receipt.StudentDetailID = StudentDetailID;
            receipt.Invoice = feeSchedule;
            receipt.ReceiptAmount = (double)pr.AmountPaid+ (double)pr.TaxPaid;
            receipt.ReceiptTaxAmount = (double)pr.TaxPaid;
            receipt.FundTransferStatus = "N";
            receipt.Narration = "";
            receipt.PaymentModeID = 28;
            receipt.ReceiptType = 2;
            receipt.ReceiptDate = DateTime.Now;
            receipt.CreatedBy = "rice-group-admin";
            receipt.CreatedOn = DateTime.Now;
            receipt.CenterId = pr.CenterID;
        }

        private void PrepareSubscriptionReceiptData(Receipt receipt, FeeSchedule feeSchedule, StudentSubscriptionTransaction pr, int StudentDetailID)
        {
            receipt.StudentDetailID = StudentDetailID;
            receipt.Invoice = feeSchedule;
            receipt.ReceiptAmount = (double)pr.Amount + (double)pr.Tax;
            receipt.ReceiptTaxAmount = (double)pr.Tax;
            receipt.FundTransferStatus = "N";
            receipt.Narration = "";
            receipt.PaymentModeID = 28;
            receipt.ReceiptType = 2;
            receipt.ReceiptDate = DateTime.Now;
            receipt.CreatedBy = "rice-group-admin";
            receipt.CreatedOn = DateTime.Now;
            receipt.CenterId = pr.CenterID;
        }

        private void PreparePayoutReceiptData(Receipt receipt, FeeSchedule feeSchedule, StudentPayOutTransaction pr, int StudentDetailID)
        {
            receipt.StudentDetailID = StudentDetailID;
            receipt.Invoice = feeSchedule;
            receipt.ReceiptAmount = (double)pr.Amount + (double)pr.Tax;
            receipt.ReceiptTaxAmount = (double)pr.Tax;
            receipt.FundTransferStatus = "N";
            receipt.Narration = "";
            receipt.PaymentModeID = 28;
            receipt.ReceiptType = 2;
            receipt.ReceiptDate = DateTime.Now;
            receipt.CreatedBy = "rice-group-admin";
            receipt.CreatedOn = DateTime.Now;
            receipt.CenterId = pr.CenterID;
        }

        #endregion PrivateMethods
    }

}
