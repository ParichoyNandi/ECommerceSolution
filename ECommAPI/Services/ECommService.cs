using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ECommAPI.Services
{
    public class ECommService
    {
        public async Task SendTransactionNotification(Transaction transaction)
        {
            string name = "";
            string mobile = "";
            string email = "";
            string receiptlist = "";
            string feeschedulelist = "";
            string transactionno = "";
            string status = "";

            try
            {
                HttpClient client = new HttpClient();

                name = transaction.CustomerName;
                email = transaction.EmailID;
                mobile = transaction.MobileNo;
                transactionno = transaction.TransactionNo;
                status = transaction.TransactionStatus.ToLower();

                foreach(var t in transaction.PlanDetails)
                {
                    foreach(var p in t.ProductDetails)
                    {
                        if (p.ReceiptHeaderID > 0)
                            receiptlist = receiptlist + p.ReceiptHeaderID.ToString() + ",";

                        if (p.FeeScheduleID > 0)
                            feeschedulelist = feeschedulelist + p.FeeScheduleID.ToString() + ",";
                    }
                }

                string param= $"ReceiptList={receiptlist}&feeSchedule={feeschedulelist}&name={name}&email={email}&mobile_number={mobile}&trn_number={transactionno}&status={status}";
                string uri = $"https://subscribe.ricesmart.in/api/invoiceNotificationForSMS?{param}";

                //client.DefaultRequestHeaders.Accept.Clear();
                //client.DefaultRequestHeaders.Add("X-API-KEY", _authkey);


                await client.PostAsync(uri,null);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
