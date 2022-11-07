using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using System.Xml.Serialization;

namespace VACIMB_Service
{
    /// <summary>
    /// Summary description for Payment
    /// </summary>
    [WebService(Namespace = "http://CIMB3rdParty/BillPaymentWS")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [SoapDocumentService(RoutingStyle = SoapServiceRoutingStyle.RequestElement)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Payment : System.Web.Services.WebService
    {
        [WebMethod]
        [SoapDocumentMethod(ResponseElementName = "CIMB3rdParty_PaymentRs")]
        [return: XmlElement("PaymentRs")]
        //[SoapDocumentMethod(ParameterStyle = SoapParameterStyle.Bare)]
        public CIMBWebReference.PaymentRs CIMB3rdParty_PaymentRq(CIMBWebReference.PaymentRq PaymentRq)
        {
            string time16 = DateTime.Now.ToString("yyyyMMddHHmmssff",
                                       CultureInfo.InvariantCulture);

            CIMBWebReference.paymentResponse RESPON = new CIMBWebReference.paymentResponse();
            CIMBWebReference.PaymentRs res = new CIMBWebReference.PaymentRs();

            var postdt = DateTime.Now.AddDays(-3).ToString("yyyy/MM/dd");

            //ini format date normal
            string format = "yyyyMMddHHmmssff";
            string dateTime = PaymentRq.TransactionDate;
            DateTime dtformat = DateTime.ParseExact(dateTime, format, CultureInfo.InvariantCulture);
            var postdate = dtformat.ToString("yyyy/MM/dd");
            //DateTime dtformat = DateTime.ParseExact(dateTime, "YYYYMMDDhhmmss", null);
            //ini format date -3 day
            DateTime dtFormatadd3 = DateTime.Now.AddDays(-3);

            bool isErrorParam = false;
            DateTime outDate = DateTime.Now;

            string xmlreq = "";
            
            XmlDocument xdoc = new XmlDocument();
            using (Stream receiveStream = HttpContext.Current.Request.InputStream)
            {
                // Move to begining of input stream and read
                receiveStream.Position = 0;
                using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8))
                {
                    // Load into XML document
                    xdoc.Load(readStream);
                    xmlreq = xdoc.InnerXml.ToString();

                }
            }

            if (PaymentRq.CompanyCode == null || PaymentRq.CompanyCode == "" || PaymentRq.CompanyCode != "4739")
            {
                //res.ResponseCode = "21";
                //res.ResponseDescription = "CompanyCode Empty";
                isErrorParam = true;
            }

            if (PaymentRq.CustomerKey1 == null || PaymentRq.CustomerKey1 == "")
            {
                //res.ResponseCode = "22";
                //res.ResponseDescription = "CustomerNumber Empty";
                isErrorParam = true;
            }

            if (PaymentRq.TransactionID == null || PaymentRq.TransactionID == "")
            {
                //res.ResponseCode = "23";
                //res.ResponseDescription = "RequestID Empty";
                isErrorParam = true;
            }

            if (PaymentRq.ChannelID == null || PaymentRq.ChannelID == "")
            {
                //res.ResponseCode = "24";
                //res.ResponseDescription = "ChannelID Empty";
                isErrorParam = true;
            }

            if (PaymentRq.TransactionDate == null || PaymentRq.TransactionDate == "")
            {
                //res.ResponseCode = "25";
                //res.ResponseDescription = "TransactionDate Empty";
                isErrorParam = true;
            }

            if (DateTime.TryParseExact(
                PaymentRq.TransactionDate,
                "yyyyMMddHHmmss", //format
                CultureInfo.CurrentCulture,
                DateTimeStyles.None, out outDate))
            {
                //res.ResponseCode = "26";
                //res.ResponseDescription = "TransactionDate format not match";
                isErrorParam = true;
            }

            if (isErrorParam == true)
            {
                res.TransactionID = PaymentRq.TransactionID;
                res.ChannelID = PaymentRq.ChannelID;
                res.TerminalID = PaymentRq.TerminalID;
                res.TransactionDate = time16;
                res.CompanyCode = PaymentRq.CompanyCode;
                res.CustomerKey1 = PaymentRq.CustomerKey1;
                res.CustomerKey2 = PaymentRq.CustomerKey2;
                res.CustomerKey3 = PaymentRq.CustomerKey3;
                res.PaymentFlag = "100000";
                res.CustomerName = PaymentRq.CustomerName;
                res.Currency = "IDR";
                res.Amount = PaymentRq.Amount;
                res.AmountSpecified = true;
                res.Fee = PaymentRq.Fee;
                res.FeeSpecified = true;
                res.PaidAmount = PaymentRq.PaidAmount;
                res.PaidAmountSpecified = true;
                res.ReferenceNumberTransaction = PaymentRq.ReferenceNumberTransaction;
                res.AdditionalData1 = PaymentRq.AdditionalData1;
                res.AdditionalData2 = PaymentRq.AdditionalData2;
                res.AdditionalData3 = PaymentRq.AdditionalData3;
                res.AdditionalData4 = PaymentRq.AdditionalData4;

                if (PaymentRq.CompanyCode == null || PaymentRq.CompanyCode == "" || PaymentRq.CompanyCode != "4739")
                {
                    res.ResponseCode = "21";
                    res.ResponseDescription = "CompanyCode Empty";
                    //isErrorParam = true;
                }

                if (PaymentRq.CustomerKey1 == null || PaymentRq.CustomerKey1 == "")
                {
                    res.ResponseCode = "22";
                    res.ResponseDescription = "CustomerNumber Empty";
                    //isErrorParam = true;
                }

                if (PaymentRq.TransactionID == null || PaymentRq.TransactionID == "")
                {
                    res.ResponseCode = "23";
                    res.ResponseDescription = "RequestID Empty";
                    //isErrorParam = true;
                }

                if (PaymentRq.ChannelID == null || PaymentRq.ChannelID == "")
                {
                    res.ResponseCode = "24";
                    res.ResponseDescription = "ChannelID Empty";
                    //isErrorParam = true;
                }

                if (PaymentRq.TransactionDate == null || PaymentRq.TransactionDate == "")
                {
                    res.ResponseCode = "25";
                    res.ResponseDescription = "TransactionDate Empty";
                    //isErrorParam = true;
                }

                if (DateTime.TryParseExact(
                    PaymentRq.TransactionDate,
                    "yyyyMMddHHmmss", //format
                    CultureInfo.CurrentCulture,
                    DateTimeStyles.None, out outDate))
                {
                    res.ResponseCode = "26";
                    res.ResponseDescription = "TransactionDate format not match";
                    //isErrorParam = true;
                }

                return res;
            }

            if (isErrorParam == false)
            {
                try
                {
                    string ssql = "exec [dbo].[sp_PostingPayment_VA_CIMB] '"+ PaymentRq.TransactionID +"','"+ PaymentRq.CompanyCode+"' ,'" + PaymentRq.CustomerKey1 + "', '" + PaymentRq.CustomerName + "'," + PaymentRq.Amount + ",'" + postdate + "'";
                    DataTable resDT = new DataTable();
                    string connString = ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString;
                    using (SqlConnection conn = new SqlConnection(connString))
                    using (SqlCommand cmd = new SqlCommand(ssql, conn))
                        try
                        {
                            SqlCommand sqlCommand = new SqlCommand(ssql);
                            sqlCommand.CommandType = CommandType.Text;
                            sqlCommand.Connection = conn;
                            conn.Open();

                            cmd.ExecuteNonQuery();
                            SqlDataReader reader = sqlCommand.ExecuteReader();

                            resDT.Load(reader);
                            conn.Close();
                        }

                        catch (SqlException ex)
                        {
                            res.TransactionID = PaymentRq.TransactionID;
                            res.ChannelID = PaymentRq.ChannelID;
                            res.TerminalID = PaymentRq.TerminalID;
                            res.TransactionDate = time16;
                            res.CompanyCode = PaymentRq.CompanyCode;
                            res.CustomerKey1 = PaymentRq.CustomerKey1;
                            res.CustomerKey2 = PaymentRq.CustomerKey2;
                            res.CustomerKey3 = PaymentRq.CustomerKey3;
                            res.PaymentFlag = "100000";
                            res.CustomerName = PaymentRq.CustomerName;
                            res.Currency = "IDR";
                            res.Amount = PaymentRq.Amount;
                            res.AmountSpecified = true;
                            res.Fee = PaymentRq.Fee;
                            res.FeeSpecified = true;
                            res.PaidAmount = PaymentRq.PaidAmount;
                            res.PaidAmountSpecified = true;
                            res.ReferenceNumberTransaction = PaymentRq.ReferenceNumberTransaction;
                            res.AdditionalData1 = PaymentRq.AdditionalData1;
                            res.AdditionalData2 = PaymentRq.AdditionalData2;
                            res.AdditionalData3 = PaymentRq.AdditionalData3;
                            res.AdditionalData4 = PaymentRq.AdditionalData4;

                            if (ex.Message.ToString().Substring(ex.Message.ToString().Length - 5) == "bayar")
                            {
                                res.ResponseCode = "33";
                                res.ResponseDescription = "Bill Already Paid";
                                isErrorParam = true;
                            }

                            else
                            {
                                res.ResponseCode = "33";
                                res.ResponseDescription = "Bill Already Paid.";
                            }
                            string jsonString = JsonConvert.SerializeObject(PaymentRq);

                            InsertLogData("https://external.mncleasing.com/VACIMBWebService/api/Payment", ex.Message.ToString(), 1, xmlreq, jsonString, "sysadmin");

                            return res;
                        }

                    res.TransactionID = PaymentRq.TransactionID;
                    res.ChannelID = PaymentRq.ChannelID;
                    res.TerminalID = PaymentRq.TerminalID;
                    res.TransactionDate = time16;
                    res.CompanyCode = PaymentRq.CompanyCode;
                    res.CustomerKey1 = PaymentRq.CustomerKey1;
                    res.CustomerKey2 = PaymentRq.CustomerKey2;
                    res.CustomerKey3 = PaymentRq.CustomerKey3;
                    res.PaymentFlag = "100000";
                    res.CustomerName = PaymentRq.CustomerName;
                    res.Currency = "IDR";
                    res.Amount = PaymentRq.Amount;
                    res.AmountSpecified = true;
                    res.Fee = PaymentRq.Fee;
                    res.FeeSpecified = true;
                    res.PaidAmount = PaymentRq.PaidAmount;
                    res.PaidAmountSpecified = true;
                    res.ReferenceNumberTransaction = PaymentRq.ReferenceNumberTransaction;
                    res.AdditionalData1 = PaymentRq.AdditionalData1;
                    res.AdditionalData2 = PaymentRq.AdditionalData2;
                    res.AdditionalData3 = PaymentRq.AdditionalData3;
                    res.AdditionalData4 = PaymentRq.AdditionalData4;
                    res.ResponseCode = "00";
                    res.ResponseDescription = "Transaction Success";

                    string jsonString1 = JsonConvert.SerializeObject(PaymentRq);

                    InsertLogData("https://external.mncleasing.com/VACIMBWebService/api/Payment", res.ResponseDescription.ToString(), 0, xmlreq,jsonString1, "sysadmin");
                }
                catch (Exception ex)
                {
                    InsertLogData("https://external.mncleasing.com/VACIMBWebService/api/Payment", ex.Message.ToString(), 0, xmlreq, "ERROR 500 : Internal Server Error", "sysadmin");
                }
            }

            //RESPON.PaymentRs = res;
            return res;
        }

        public static void InsertLogData(string api_url, string result, int is_error, string postval, string retval, string user)
        {
            result = result.Replace("'", "");
            postval = postval.Replace("'", "");

            string connString = ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString;
            string ssql = "INSERT INTO [dbo].[API_Log] SELECT '" + api_url + "', '" + result + "', '" + retval + "'," + is_error + ",'" +
                "sysadmin" + "',GETDATE(),'" + postval + "'";
            using (SqlConnection conn = new SqlConnection(connString))
            using (SqlCommand cmd = new SqlCommand(ssql, conn))
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
    }
}
