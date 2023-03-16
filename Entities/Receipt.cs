using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Utilities;

namespace Entities
{
    public class Receipt
    {
        public string FeeComponentList { get; set; }
        public int ReceiptId { get; set; }
        public int CenterId { get; set; }

        public string ReceiptNo { get; set; }

        public DateTime ReceiptDate { get; set; }

        public double ReceiptAmount { get; set; }

        public double ReceiptTaxAmount { get; set; }

        public double ReceiptAmountRff { get; set; }

        public double ReceiptTaxRff { get; set; }

        public int EnquiryID { get; set; }

        public int PaymentModeID { get; set; }

        public long CreditCardNo { get; set; }
        public int Status { get; set; }
        public DateTime CreditCardExpiry { get; set; }

        public string CreditCardIssuer { get; set; }

        public string ChequeDDNo { get; set; }

        public DateTime ChequeDDDate { get; set; }

        public string BankName { get; set; }

        public string BranchName { get; set; }

        public string FundTransferStatus { get; set; }

        public string CancellationReason { get; set; }

        public List<ReceiptDetails> ReceiptDetailList { get; set; } = new();

        //public List<IReceiptPartDetails> ReceiptPartDetails { get; set; }

        public FeeSchedule Invoice { get; set; }

        public int StudentDetailID { get; set; }

        public int ReceiptType { get; set; }

        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        public bool IsModified { get; set; }

        public bool IsDeleted { get; set; }

        public string OnAccountInvoiceNo { get; set; }

        public void GenerateReceipt()
        {
            double nAmountRemaining = this.ReceiptAmount;

            bool installmentFound = false;
            int iMaxInstallmentNumber = 0;
            int iMaxSequenceNumber = 0;

            int iInstallmentNo = 1;
            int iSequenceNo = 1;
            //get the first installment to be paid
            //it also sets maximum number of installmnet and max sequence number
            GetMaxInstallmentAndSequence(ref iMaxInstallmentNumber, ref iMaxSequenceNumber);

            iInstallmentNo = FindInstallment(iMaxInstallmentNumber, iMaxSequenceNumber);

            List<ReceiptDetails> objReceiptDetails = this.ReceiptDetailList;
            //loop
            while (nAmountRemaining >= 0.01 && iMaxInstallmentNumber >= iInstallmentNo)
            {
                iSequenceNo = 1;

                while (nAmountRemaining >= 0.01 && iMaxSequenceNumber >= iSequenceNo)
                {
                    //Find fee component to be paid
                    InvoiceChildDetails invoiceChildDetails = FindFeeComponent(iInstallmentNo, iMaxSequenceNumber, iSequenceNo);
                    if (invoiceChildDetails != null)
                    {
                        invoiceChildDetails.IsModified = true;
                        ReceiptDetails clsReceiptDetails = new ReceiptDetails();
                        clsReceiptDetails.InvoiceDetailId = invoiceChildDetails.InvoiceChildDetailsId;
                        clsReceiptDetails.AmountPaid = 0;
                        clsReceiptDetails.IsModified = true;


                        ComputeReceiptDetails(clsReceiptDetails, invoiceChildDetails, ref nAmountRemaining);
                        if (nAmountRemaining >= 0.01)
                        {
                            ComputeReceiptTaxDetails(clsReceiptDetails, invoiceChildDetails, ref nAmountRemaining);
                        }
                        objReceiptDetails.Add((ReceiptDetails)clsReceiptDetails);
                    }
                    else
                    {
                        //there is no fee component tobe paid for current sequence, so increase it for next sequence
                        iSequenceNo++;
                    }
                }
                iInstallmentNo++;
            }

            //Commented on 19.11.2021 by Akash
            //if (nAmountRemaining >= 1.00)
            //{
            //    //throw exception
            //    PwCApplicationException appExp = new PwCApplicationException("ERR_RECEIPT_OVERPAYMENT", Thread.CurrentThread.CurrentCulture.ToString());
            //    appExp.ErrMessage = appExp.ErrMessage + ". Amount value " + nAmountRemaining.ToString();
            //    PwCExceptionManager.RaiseException(appExp);
            //}


            this.ReceiptDetailList = objReceiptDetails;

            foreach (ReceiptDetails objRecDet in this.ReceiptDetailList)
            {
                objRecDet.AmountPaid = Math.Round(objRecDet.AmountPaid, 2);
                if (objRecDet.ReceiptTaxDetails != null)
                {
                    foreach (ReceiptTaxDetails objRecTaxDet in objRecDet.ReceiptTaxDetails)
                    {
                        objRecTaxDet.TaxPaid = Math.Round(objRecTaxDet.TaxPaid, 2);
                    }
                }
            }

            double d = 0.0;
            foreach (ReceiptDetails objReceiptDetailsTemp in this.ReceiptDetailList)
            {
                if (objReceiptDetailsTemp.IsModified == true)
                {
                    d = d + objReceiptDetailsTemp.AmountPaid;
                    if (objReceiptDetailsTemp.ReceiptTaxDetails != null)
                    {
                        foreach (ReceiptTaxDetails objReceiptTax in objReceiptDetailsTemp.ReceiptTaxDetails)
                        {
                            d = d + objReceiptTax.TaxPaid;
                        }
                    }
                }
            }
            if ((d - this.ReceiptAmount) > 0)
            {
                double d1 = d - this.ReceiptAmount;
                foreach (ReceiptDetails objReceDetTemp in this.ReceiptDetailList)
                {
                    if (objReceDetTemp.IsModified == true)
                    {
                        if (objReceDetTemp.ReceiptTaxDetails != null)
                        {
                            foreach (ReceiptTaxDetails objReceiptTax in objReceDetTemp.ReceiptTaxDetails)
                            {
                                if (d1 <= objReceiptTax.TaxPaid)
                                {
                                    objReceiptTax.TaxPaid = objReceiptTax.TaxPaid - d1;
                                    d1 = 0.0;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            else if ((d - this.ReceiptAmount) < 0)
            {
                double d1 = this.ReceiptAmount - d;
                foreach (ReceiptDetails objReceDetTemp in this.ReceiptDetailList)
                {
                    if (objReceDetTemp.IsModified == true)
                    {
                        if (objReceDetTemp.ReceiptTaxDetails != null)
                        {
                            foreach (ReceiptTaxDetails objReceiptTax in objReceDetTemp.ReceiptTaxDetails)
                            {
                                objReceiptTax.TaxPaid = objReceiptTax.TaxPaid + d1;
                                d1 = 0.0;
                                break;
                            }
                        }
                    }
                }
            }
        }


        private InvoiceChildDetails FindFeeComponent(int iInstallmentNo, int iMaxSequenceNumber, int iSequence)
        {
            while (iMaxSequenceNumber >= iSequence)
            {
                foreach (InvoiceChild objInvoiceChild in this.Invoice.InvoiceChild)
                {
                    foreach (InvoiceChildDetails objInvoiceChildDetails in objInvoiceChild.InvoiceChildDetails)
                    {
                        if (objInvoiceChildDetails.Sequence == iSequence && objInvoiceChildDetails.InstallmentNo == iInstallmentNo)
                        {
                            //double dAmount = (double)objInvoiceChildDetails.AmountDue;//Before Discount 4.10.2021
                            double dAmount = (double)objInvoiceChildDetails.PayableAmount;//After Discount 4.10.2021
                            double dAmountPaidTillNow = GetPaidAmount(objInvoiceChildDetails.InvoiceChildDetailsId);
                            if (dAmount > dAmountPaidTillNow)
                            {
                                return objInvoiceChildDetails;
                            }
                        }
                    }
                }
                iSequence++;
            }
            return null;
        }

        /// <summary>
        /// Gets the maximum Number of installments and max sequence of fee components
        /// </summary>
        /// <param name="iMaxInstallmentNumber"></param>
        /// <param name="iMaxSequenceNumber"></param>
        private void GetMaxInstallmentAndSequence(ref int iMaxInstallmentNumber, ref int iMaxSequenceNumber)
        {
            foreach (InvoiceChild objInvoiceChild in this.Invoice.InvoiceChild)
            {
                foreach (InvoiceChildDetails objInvoiceChildDetails in objInvoiceChild.InvoiceChildDetails)
                {
                    if (objInvoiceChildDetails.InstallmentNo > iMaxInstallmentNumber)
                        iMaxInstallmentNumber = objInvoiceChildDetails.InstallmentNo;

                    if (objInvoiceChildDetails.Sequence > iMaxSequenceNumber)
                        iMaxSequenceNumber = objInvoiceChildDetails.Sequence;

                }
            }
        }

        /// <summary>
        /// Finds the first installment to be paid (if full amount is not paid)
        /// </summary>
        /// <param name="iMaxInstallmentNumber"></param>
        /// <param name="iMaxSequenceNumber"></param>
        /// <returns></returns>
        private int FindInstallment(int iMaxInstallmentNumber, int iMaxSequenceNumber)
        {
            bool installmentFound = false;
            int iInstallmentNo = 1;

            while (iInstallmentNo <= iMaxInstallmentNumber)
            {
                foreach (InvoiceChild objInvoiceChild in this.Invoice.InvoiceChild)
                {
                    foreach (InvoiceChildDetails objInvoiceChildDetails in objInvoiceChild.InvoiceChildDetails)
                    {

                        if (objInvoiceChildDetails.PayableAmount > 0 && iInstallmentNo == objInvoiceChildDetails.InstallmentNo)
                        {
                            double dAmount = (double)objInvoiceChildDetails.PayableAmount;
                            double dAmountPaidTillNow = GetPaidAmount(objInvoiceChildDetails.InvoiceChildDetailsId);
                            if (dAmount > dAmountPaidTillNow)
                            {
                                return iInstallmentNo;
                            }
                        }
                    }
                }
                iInstallmentNo++;
            }
            return iInstallmentNo;
        }

        private int FindNextInstallment(int iMaxInstallmentNumber, int iMaxSequenceNumber)
        {
            int iPaidInstallmentNo = 1;
            List<int> iInstallmentNos = new List<int>();
            bool isInstExist = false;

            if (iPaidInstallmentNo <= iMaxInstallmentNumber)
            {
                foreach (InvoiceChild objInvoiceChild in this.Invoice.InvoiceChild)
                {
                    foreach (InvoiceChildDetails objInvoiceChildDetails in objInvoiceChild.InvoiceChildDetails)
                    {

                        if (objInvoiceChildDetails.PayableAmount > 0)
                        {
                            bool isPaidInstallment = isPaidInstallmentId(objInvoiceChildDetails.InvoiceChildDetailsId);
                            if (isPaidInstallment && objInvoiceChildDetails.InstallmentNo > iPaidInstallmentNo)
                            {
                                iPaidInstallmentNo = objInvoiceChildDetails.InstallmentNo;
                            }
                            if (iPaidInstallmentNo == iMaxInstallmentNumber)
                            {
                                return iPaidInstallmentNo;
                            }
                            isInstExist = false;
                            foreach (int inst in iInstallmentNos)
                            {
                                if (objInvoiceChildDetails.InstallmentNo == inst)
                                {
                                    isInstExist = true;
                                }
                            }

                            if (!isInstExist)
                            {
                                iInstallmentNos.Add(objInvoiceChildDetails.InstallmentNo);
                            }
                        }
                    }
                }
            }

            iInstallmentNos.Sort();

            foreach (int inst in iInstallmentNos)
            {
                if (inst > iPaidInstallmentNo)
                {
                    return inst;
                }
            }

            return iPaidInstallmentNo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clsReceiptDetails"></param>
        /// <param name="objInvoiceChildDetails"></param>
        /// <param name="nAmountRemaining"></param>
        private void ComputeReceiptDetails(ReceiptDetails clsReceiptDetails, InvoiceChildDetails objInvoiceChildDetails, ref double nAmountRemaining)
        {
            //double dAmount = (double)objInvoiceChildDetails.AmountDue;//Before Discount 4.10.2021
            double dAmount = (double)objInvoiceChildDetails.PayableAmount;//After Discount 4.10.2021
            double dAmountPaidTillNow = GetPaidAmount(objInvoiceChildDetails.InvoiceChildDetailsId);
            double dFeeCompPart = 1;

            //Calculate the total amount for the current Fee Comp. with Tax
            double dFeeCompTotal = objInvoiceChildDetails.PayableAmount;
            double dFeeCompRemaining = objInvoiceChildDetails.PayableAmount - dAmountPaidTillNow;

            for (int l = 0; l < objInvoiceChildDetails.InvoiceTaxDetails.Count; l++)
            {
                dFeeCompTotal += GetPaidTaxAmount(clsReceiptDetails.InvoiceDetailId, objInvoiceChildDetails.InvoiceTaxDetails[l].TaxId) + (objInvoiceChildDetails.PayableAmount - dAmountPaidTillNow) * objInvoiceChildDetails.InvoiceTaxDetails[l].TaxValue / objInvoiceChildDetails.PayableAmount;
                dFeeCompRemaining += (objInvoiceChildDetails.PayableAmount - dAmountPaidTillNow) * objInvoiceChildDetails.InvoiceTaxDetails[l].TaxValue / objInvoiceChildDetails.PayableAmount;
            }

            if (dFeeCompRemaining > 0)
            {

                if (dAmountPaidTillNow < dAmount)
                {
                    //Calculate the percentage amount to be distributed for the Fee Component
                    List<InvoiceTaxDetails> objInvoiceTaxDetails = objInvoiceChildDetails.InvoiceTaxDetails;
                    double nTaxPaidTillNow = 0;

                    for (int l = 0; l < objInvoiceChildDetails.InvoiceTaxDetails.Count; l++)
                    {
                        dFeeCompPart += objInvoiceChildDetails.InvoiceTaxDetails[l].TaxValue / objInvoiceChildDetails.PayableAmount;
                    }
                    clsReceiptDetails.AmountPaid = Math.Min(nAmountRemaining, Math.Min(nAmountRemaining, dFeeCompRemaining) / dFeeCompPart);

                    if (nAmountRemaining - clsReceiptDetails.AmountPaid == 0.01)
                        clsReceiptDetails.AmountPaid += 0.01;

                    nAmountRemaining = nAmountRemaining - clsReceiptDetails.AmountPaid;
                    clsReceiptDetails.IsModified = true;
                }
            }
            else
            if (nAmountRemaining > dFeeCompTotal)
            {
                clsReceiptDetails.AmountPaid = dAmount; // The amount for the Fee Component
                nAmountRemaining -= clsReceiptDetails.AmountPaid;
                clsReceiptDetails.IsModified = true;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clsReceiptDetails"></param>
        /// <param name="objInvoiceChildDetails"></param>
        /// <param name="nAmountRemaining"></param>
        private void ComputeReceiptTaxDetails(ReceiptDetails clsReceiptDetails, InvoiceChildDetails objInvoiceChildDetails, ref double nAmountRemaining)
        {
            List<InvoiceTaxDetails> objInvoiceTaxDetails = objInvoiceChildDetails.InvoiceTaxDetails;
            List<ReceiptTaxDetails> objReceiptTaxDetails = new List<ReceiptTaxDetails>();

            // Total Receipt money to be distributed for Current Fee Component
            double dAmtforFeeComponent = nAmountRemaining + clsReceiptDetails.AmountPaid;
            double dTotalAmtforFeeComponent = objInvoiceChildDetails.PayableAmount - GetPaidAmount(objInvoiceChildDetails.InvoiceChildDetailsId);

            //Calculate the Total Tax for the Current Fee Component
            double dTaxTotal = 0.0;
            double dTaxTotalRemaining = 0.0;

            for (int l = 0; l < objInvoiceChildDetails.InvoiceTaxDetails.Count; l++)
            {
                dTaxTotal += GetPaidTaxAmount(clsReceiptDetails.InvoiceDetailId, objInvoiceChildDetails.InvoiceTaxDetails[l].TaxId) + (objInvoiceChildDetails.PayableAmount - GetPaidAmount(objInvoiceChildDetails.InvoiceChildDetailsId)) * objInvoiceChildDetails.InvoiceTaxDetails[l].TaxValue / objInvoiceChildDetails.PayableAmount;
                dTaxTotalRemaining += (objInvoiceChildDetails.PayableAmount - GetPaidAmount(objInvoiceChildDetails.InvoiceChildDetailsId)) * objInvoiceChildDetails.InvoiceTaxDetails[l].TaxValue / objInvoiceChildDetails.PayableAmount;
            }

            dTotalAmtforFeeComponent += dTaxTotalRemaining;

            if (dTaxTotalRemaining <= dTaxTotal)
            {
                double nTaxPaidTillNow = 0;

                //Calculate the percentage amount to be distributed for the Tax Component
                double dFeeCompPart = 1;
                double dAmountPaidTillNow = GetPaidAmount(objInvoiceChildDetails.InvoiceChildDetailsId);

                for (int l = 0; l < objInvoiceChildDetails.InvoiceTaxDetails.Count; l++)
                {
                    dFeeCompPart += objInvoiceChildDetails.InvoiceTaxDetails[l].TaxValue / objInvoiceChildDetails.PayableAmount;
                }


                nTaxPaidTillNow = 0;

                for (int l = 0; l < objInvoiceChildDetails.InvoiceTaxDetails.Count; l++)
                {
                    double dTaxAmount = objInvoiceChildDetails.InvoiceTaxDetails[l].TaxValue;
                    nTaxPaidTillNow = GetPaidTaxAmount(clsReceiptDetails.InvoiceDetailId,
                                                            objInvoiceChildDetails.InvoiceTaxDetails[l].TaxId);

                    dAmountPaidTillNow = GetPaidAmount(objInvoiceChildDetails.InvoiceChildDetailsId);
                    double dTaxPart = objInvoiceChildDetails.InvoiceTaxDetails[l].TaxValue / objInvoiceChildDetails.PayableAmount;

                    if (nAmountRemaining > 0)
                    {
                        ReceiptTaxDetails clsReceiptTaxDetails = new ReceiptTaxDetails();
                        clsReceiptTaxDetails.TaxId = objInvoiceChildDetails.InvoiceTaxDetails[l].TaxId;
                        clsReceiptTaxDetails.InvoiceDetailId = clsReceiptDetails.InvoiceDetailId;

                        clsReceiptTaxDetails.TaxPaid = Math.Min(nAmountRemaining, (Math.Min(dTotalAmtforFeeComponent, dAmtforFeeComponent) * dTaxPart) / dFeeCompPart);
                        clsReceiptTaxDetails.IsModified = true;

                        if (nAmountRemaining - clsReceiptTaxDetails.TaxPaid == 0.01)
                            clsReceiptTaxDetails.TaxPaid += 0.01;

                        nAmountRemaining = nAmountRemaining - clsReceiptTaxDetails.TaxPaid;

                        objReceiptTaxDetails.Add((ReceiptTaxDetails)clsReceiptTaxDetails);
                    }
                }
            }
            else
            {
                //Distribute the Tax in actual amounts
                for (int l = 0; l < objInvoiceChildDetails.InvoiceTaxDetails.Count; l++)
                {
                    double dTaxAmount = objInvoiceChildDetails.InvoiceTaxDetails[l].TaxValue;
                    ReceiptTaxDetails clsReceiptTaxDetails = new ReceiptTaxDetails();
                    clsReceiptTaxDetails.TaxId = objInvoiceChildDetails.InvoiceTaxDetails[l].TaxId;
                    clsReceiptTaxDetails.InvoiceDetailId = clsReceiptDetails.InvoiceDetailId;

                    clsReceiptTaxDetails.TaxPaid = dTaxAmount;

                    clsReceiptTaxDetails.IsModified = true;
                    nAmountRemaining -= clsReceiptTaxDetails.TaxPaid;

                    objReceiptTaxDetails.Add((ReceiptTaxDetails)clsReceiptTaxDetails);
                }
            }

            clsReceiptDetails.ReceiptTaxDetails = objReceiptTaxDetails;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iInvoiceDetailID"></param>
        /// <param name="iTaxID"></param>
        /// <returns></returns>
        private double GetPaidTaxAmount(int iInvoiceDetailID, int iTaxID)
        {
            double dReturn = 0.0;

            for (int m = 0; m < this.ReceiptDetailList.Count; m++)
            {
                if (this.ReceiptDetailList[m].InvoiceDetailId == iInvoiceDetailID)
                {
                    foreach (ReceiptTaxDetails objReceiptTaxDetails in this.ReceiptDetailList[m].ReceiptTaxDetails)
                    {
                        if (objReceiptTaxDetails.TaxId == iTaxID)
                        {
                            dReturn += objReceiptTaxDetails.TaxPaid;
                        }
                    }
                }
            }

            return dReturn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iInvoiceDetailID"></param>
        /// <returns></returns>
        private double GetPaidAmount(int iInvoiceDetailID)
        {
            double dReturn = 0.0;

            for (int m = 0; m < this.ReceiptDetailList.Count; m++)
            {
                if (this.ReceiptDetailList[m].InvoiceDetailId == iInvoiceDetailID)
                {
                    dReturn += this.ReceiptDetailList[m].AmountPaid;
                }
            }

            return dReturn;
        }

        private bool isPaidInstallmentId(int iInvoiceDetailID)
        {
            for (int m = 0; m < this.ReceiptDetailList.Count; m++)
            {
                if (this.ReceiptDetailList[m].InvoiceDetailId == iInvoiceDetailID)
                {
                    return true;
                }
            }

            return false;
        }


        /// <summary>
        /// Generates the XML with the Receipt Data and the corresponding
        /// Tax details
        /// </summary>
        /// <returns></returns>
        /// 
        public string GenerateReceiptDetailXML()
        {
            XmlDocument xDoc = new XmlDocument();
            XmlElement xTblRctCompDtl;
            XmlElement xTblRctTaxDtl;
            XmlElement xColRctCompDtl;
            XmlElement xColRctTaxDtl;
            XmlElement xRowRctCompDtl;


            xTblRctCompDtl = (XmlElement)xDoc.CreateElement("TblRctCompDtl");

            for (int i = 0; i < this.ReceiptDetailList.Count; i++)
            {
                if (this.ReceiptDetailList[i].IsModified)
                {
                    if (this.ReceiptDetailList[i].AmountPaid > 0)
                    {

                        xRowRctCompDtl = (XmlElement)xDoc.CreateElement("RowRctCompDtl");


                        xRowRctCompDtl.SetAttribute("I_Invoice_Detail_ID", this.ReceiptDetailList[i].InvoiceDetailId.ToString());
                        xRowRctCompDtl.SetAttribute("I_Receipt_Detail_ID", this.ReceiptId.ToString());//this.ReceiptDetailList[i].ReceiptComponentDetailId.ToString());
                        xRowRctCompDtl.SetAttribute("N_Amount_Paid", (Math.Round(this.ReceiptDetailList[i].AmountPaid, 2)).ToString());



                        xTblRctTaxDtl = (XmlElement)xDoc.CreateElement("TblRctTaxDtl");
                        XmlElement xRowRctTaxDtl;

                        if (!Helper.IsEmptyString(this.ReceiptDetailList[i].ReceiptTaxDetails))
                        {
                            for (int j = 0; j < this.ReceiptDetailList[i].ReceiptTaxDetails.Count; j++)
                            {
                                if (this.ReceiptDetailList[i].ReceiptTaxDetails[j].IsModified)
                                {
                                    xRowRctTaxDtl = (XmlElement)xDoc.CreateElement("RowRctTaxDtl");

                                    xRowRctTaxDtl.SetAttribute("I_Tax_ID", this.ReceiptDetailList[i].ReceiptTaxDetails[j].TaxId.ToString());
                                    xRowRctTaxDtl.SetAttribute("I_Receipt_Comp_Detail_ID", this.ReceiptDetailList[i].ReceiptTaxDetails[j].ReceiptcomponentDetailId.ToString());
                                    xRowRctTaxDtl.SetAttribute("I_Invoice_Detail_ID", this.ReceiptDetailList[i].ReceiptTaxDetails[j].InvoiceDetailId.ToString());
                                    xRowRctTaxDtl.SetAttribute("N_Tax_Paid", (Math.Round(this.ReceiptDetailList[i].ReceiptTaxDetails[j].TaxPaid, 2)).ToString());

                                    xTblRctTaxDtl.AppendChild(xRowRctTaxDtl);
                                }
                            }
                        }
                        xRowRctCompDtl.AppendChild(xTblRctTaxDtl);
                        xTblRctCompDtl.AppendChild(xRowRctCompDtl);
                    }

                }
            }

            xDoc.AppendChild(xTblRctCompDtl);

            return xDoc.InnerXml;
        }

        public string GetReceiptTaxXML()
        {
            XmlDocument xDoc = new XmlDocument();
            XmlElement xMainNode;
            XmlElement xChildNode;

            xMainNode = (XmlElement)xDoc.CreateElement("ReceiptTax");

            for (int i = 0; i < this.ReceiptDetailList[0].ReceiptTaxDetails.Count; i++)
            {
                xChildNode = (XmlElement)xDoc.CreateElement("TaxDetails");

                xChildNode.SetAttribute("TaxID", this.ReceiptDetailList[0].ReceiptTaxDetails[i].TaxId.ToString());
                xChildNode.SetAttribute("TaxPaid", this.ReceiptDetailList[0].ReceiptTaxDetails[i].TaxPaid.ToString());

                xMainNode.AppendChild(xChildNode);
            }

            xDoc.AppendChild(xMainNode);

            return xDoc.InnerXml;
        }

        public string BankAccName { get; set; }
        public DateTime DepositDate { get; set; }
        public string Narration { get; set; }

        //akash 6.9.2018
        public DateTime FindNextInstallmentDate(Receipt receipt)
        {
            DateTime NextInstallmentDate = DateTime.Now;
            int receiptid = receipt.ReceiptId;
            //NextInstallmentDate = CenterBuilder.FindNxtInstalmentDate(receiptid);
            return NextInstallmentDate;
        }
        //akash 6.9.2018

        public DateTime FindNextInstallmentDate()
        {
            double nAmountRemaining = this.ReceiptAmount;

            DateTime dtNextInstallment = new DateTime(2000, 1, 1);
            int iMaxInstallmentNumber = 0;
            int iMaxSequenceNumber = 0;

            int iInstallmentNo = 1;
            int iSequenceNo = 1;
            //get the first installment to be paid
            //it also sets maximum number of installmnet and max sequence number
            GetMaxInstallmentAndSequence(ref iMaxInstallmentNumber, ref iMaxSequenceNumber);

            iInstallmentNo = FindNextInstallment(iMaxInstallmentNumber, iMaxSequenceNumber);

            List<ReceiptDetails> objReceiptDetails = this.ReceiptDetailList;
            //loop
            while (iMaxInstallmentNumber >= iInstallmentNo)
            {
                iSequenceNo = 1;

                while (iMaxSequenceNumber >= iSequenceNo)
                {
                    //Find fee component to be paid
                    InvoiceChildDetails invoiceChildDetails = FindFeeComponent(iInstallmentNo, iMaxSequenceNumber, iSequenceNo);
                    if (invoiceChildDetails != null)
                    {
                        dtNextInstallment = invoiceChildDetails.DueDate;
                        return dtNextInstallment;
                    }
                    else
                    {
                        //there is no fee component tobe paid for current sequence, so increase it for next sequence
                        iSequenceNo++;
                    }
                }
                iInstallmentNo++;
            }
            return dtNextInstallment;
        }

        public void GenerateCancelReceipt(double nAmountRemaining)
        {
            // double nAmountRemaining = this.ReceiptAmount;

            bool installmentFound = false;
            int iMaxInstallmentNumber = 0;
            int iMaxSequenceNumber = 0;

            int iInstallmentNo = 1;
            int iSequenceNo = 1;
            //get the first installment to be paid
            //it also sets maximum number of installmnet and max sequence number
            GetMaxInstallmentAndSequence(ref iMaxInstallmentNumber, ref iMaxSequenceNumber);

            iInstallmentNo = FindNextInstallment(iMaxInstallmentNumber, iMaxSequenceNumber);

            List<ReceiptDetails> objReceiptDetails = this.ReceiptDetailList;
            //loop
            while (nAmountRemaining >= 0.01 && iMaxInstallmentNumber >= iInstallmentNo)
            {
                iSequenceNo = 1;

                while (nAmountRemaining >= 0.01 && iMaxSequenceNumber >= iSequenceNo)
                {
                    //Find fee component to be paid
                    InvoiceChildDetails invoiceChildDetails = FindFeeComponent(iInstallmentNo, iMaxSequenceNumber, iSequenceNo);
                    if (invoiceChildDetails != null)
                    {
                        invoiceChildDetails.IsModified = true;
                        ReceiptDetails clsReceiptDetails = new ReceiptDetails();
                        clsReceiptDetails.InvoiceDetailId = invoiceChildDetails.InvoiceChildDetailsId;
                        clsReceiptDetails.AmountPaid = 0;
                        clsReceiptDetails.IsModified = true;


                        ComputeReceiptDetails(clsReceiptDetails, invoiceChildDetails, ref nAmountRemaining);
                        if (nAmountRemaining >= 0.01)
                        {
                            ComputeReceiptTaxDetails(clsReceiptDetails, invoiceChildDetails, ref nAmountRemaining);
                        }
                        objReceiptDetails.Add((ReceiptDetails)clsReceiptDetails);
                    }
                    else
                    {
                        //there is no fee component tobe paid for current sequence, so increase it for next sequence
                        iSequenceNo++;
                    }
                }
                iInstallmentNo++;
            }

            //Commented on 9/25/2012 by Monalisa
            //if (nAmountRemaining >= 1.00)
            //{
            //    //throw exception
            //    PwCApplicationException appExp = new PwCApplicationException("ERR_RECEIPT_OVERPAYMENT", Thread.CurrentThread.CurrentCulture.ToString());
            //    appExp.ErrMessage = appExp.ErrMessage + ". Amount value " + nAmountRemaining.ToString();
            //    PwCExceptionManager.RaiseException(appExp);
            //}


            this.ReceiptDetailList = objReceiptDetails;

            foreach (ReceiptDetails objRecDet in this.ReceiptDetailList)
            {
                objRecDet.AmountPaid = Math.Round(objRecDet.AmountPaid, 2);
                if (objRecDet.ReceiptTaxDetails != null)
                {
                    foreach (ReceiptTaxDetails objRecTaxDet in objRecDet.ReceiptTaxDetails)
                    {
                        objRecTaxDet.TaxPaid = Math.Round(objRecTaxDet.TaxPaid, 2);
                    }
                }
            }

            double d = 0.0;
        }
    }
}
