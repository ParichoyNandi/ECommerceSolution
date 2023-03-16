using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public static class Helper
    {
        

        public static string GetCurrentFinancialYear(int BrandID, DateTime dtBatchStartDate)
        {
            int CurrentYear = DateTime.Today.Year;
            int PreviousYear = DateTime.Today.Year - 1;
            int NextYear = DateTime.Today.Year + 1;
            string PreYear = null;
            string NexYear = null;
            string CurYear = null;
            string FinYear = null;

            switch (BrandID)
            {
                case 109: //for RICE
                    PreYear = PreviousYear.ToString().Substring(2, 2);
                    NexYear = NextYear.ToString().Substring(2, 2);
                    CurYear = CurrentYear.ToString().Substring(2, 2);
                    if (DateTime.Today.Month > 3)
                        FinYear = CurYear + NexYear;
                    else
                        FinYear = PreYear + CurYear;
                    break;
                case 111: //for adamas career
                    PreYear = PreviousYear.ToString().Substring(2, 2);
                    NexYear = NextYear.ToString().Substring(2, 2);
                    CurYear = CurrentYear.ToString().Substring(2, 2);
                    if (DateTime.Today.Month > 3)
                        FinYear = CurYear + NexYear;
                    else
                        FinYear = PreYear + CurYear;
                    break;
                case 107:  //for AIS                    
                    //FinYear = CurYear.ToString().Substring(2,2);
                    //break;
                    NexYear = NextYear.ToString().Substring(2, 2);
                    CurYear = CurrentYear.ToString().Substring(2, 2);
                    if (DateTime.Today.Month > 3)
                        FinYear = CurYear + '-' + NexYear;
                    else
                        FinYear = PreYear + '-' + CurYear;

                    break;
                case 110: //for AWS
                    PreYear = PreviousYear.ToString().Substring(2, 2);
                    NexYear = NextYear.ToString().Substring(2, 2);
                    CurYear = CurrentYear.ToString().Substring(2, 2);
                    if (dtBatchStartDate != DateTime.MinValue && dtBatchStartDate.Month > 3 && dtBatchStartDate.Year >= DateTime.Now.Year)
                    {
                        FinYear = dtBatchStartDate.Year.ToString().Substring(2, 2) + '-' + (dtBatchStartDate.Year + 1).ToString().Substring(2, 2);
                    }
                    else
                    {
                        if (DateTime.Today.Month > 3)
                            FinYear = CurYear + '-' + NexYear;
                        else
                            FinYear = PreYear + '-' + CurYear;
                    }
                    break;
                case 112: //for AHSMS
                    PreYear = PreviousYear.ToString().Substring(2, 2);
                    NexYear = NextYear.ToString().Substring(2, 2);
                    CurYear = CurrentYear.ToString().Substring(2, 2);
                    if (dtBatchStartDate != DateTime.MinValue && dtBatchStartDate.Month > 3 && dtBatchStartDate.Year >= DateTime.Now.Year)
                    {
                        FinYear = dtBatchStartDate.Year.ToString().Substring(2, 2) + '-' + (dtBatchStartDate.Year + 1).ToString().Substring(2, 2);
                    }
                    else
                    {
                        if (DateTime.Today.Month > 3)
                            FinYear = CurYear + '-' + NexYear;
                        else
                            FinYear = PreYear + '-' + CurYear;
                    }
                    break;

                default:
                    FinYear = "";
                    break;
            }

            return FinYear.Trim();
        }

        public static bool IsEmptyString(object o)
        {
            bool bReturn = false;

            if (o == null || o.ToString() == string.Empty || o.ToString().Trim() == string.Empty)
                bReturn = true;

            return bReturn;
        }

        public static string Encrypt(string sStringToEncrypt)
        {
            //get the system key and init vector
            string key = "ZBknC5U2e5RT6u4kiwUi3bLAdN2hRrEP";
            string initVector = "Saaa+FEOnWw=";
            string sEncryptedString = string.Empty;
            System.IO.MemoryStream stmEncrptedText;
            CryptoStream cryptStrm;
            SymmetricAlgorithm alg;

            try
            {
                alg = SymmetricAlgorithm.Create("TripleDES");
                byte[] myKey = Convert.FromBase64String(key);
                byte[] initVec = Convert.FromBase64String(initVector);
                byte[] plainText = System.Text.Encoding.ASCII.GetBytes(sStringToEncrypt);

                stmEncrptedText = new System.IO.MemoryStream();
                ICryptoTransform encryptor = alg.CreateEncryptor(myKey, initVec);
                cryptStrm = new CryptoStream(stmEncrptedText, encryptor, CryptoStreamMode.Write);
                cryptStrm.Write(plainText, 0, plainText.Length);
                cryptStrm.FlushFinalBlock();

                sEncryptedString = Convert.ToBase64String(stmEncrptedText.ToArray());

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //cryptStrm.Close();
                //stmEncrptedText.Close();
                //alg.Clear();
            }
            return sEncryptedString;
        }

        public static void SendEmail365(string fromEmail, string pass, string toEmail, string message, string subject)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var client = new SmtpClient("smtp.office365.com")
            {
                Port = 587,
                UseDefaultCredentials = false,
                EnableSsl = true,
                Credentials = new NetworkCredential(fromEmail, pass)
            };

            var mailMessage = new MailMessage { From = new MailAddress(fromEmail) };
            mailMessage.To.Add(toEmail);
            mailMessage.Body = message;
            mailMessage.Subject = subject;
            client.Send(mailMessage);
        }

    }
}
