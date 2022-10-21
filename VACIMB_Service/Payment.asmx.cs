using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
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
            CIMBWebReference.paymentResponse RESPON = new CIMBWebReference.paymentResponse();
            CIMBWebReference.PaymentRs res = new CIMBWebReference.PaymentRs();

            var postdt = DateTime.Now.AddDays(-3).ToString("yyyyMMdd");

            //ini format date normal
            string format = "yyyyMMddHHmmssff";
            string dateTime = PaymentRq.TransactionDate;
            DateTime dtformat = DateTime.ParseExact(dateTime, format, CultureInfo.InvariantCulture);
            //DateTime dtformat = DateTime.ParseExact(dateTime, "YYYYMMDDhhmmss", null);
            //ini format date -3 day
            DateTime dtFormatadd3 = DateTime.Now.AddDays(-3);

            bool isErrorParam = false;
            DateTime outDate = DateTime.Now;

            if (PaymentRq.CompanyCode == null || PaymentRq.CompanyCode == "" || PaymentRq.CompanyCode != "2739")
            {
                res.ResponseCode = "21";
                res.ResponseDescription = "CompanyCode Empty";
                isErrorParam = true;
            }

            if (PaymentRq.CustomerKey1 == null || PaymentRq.CustomerKey1 == "")
            {
                res.ResponseCode = "22";
                res.ResponseDescription = "CustomerNumber Empty";
                isErrorParam = true;
            }

            if (PaymentRq.TransactionID == null || PaymentRq.TransactionID == "")
            {
                res.ResponseCode = "23";
                res.ResponseDescription = "RequestID Empty";
                isErrorParam = true;
            }

            if (PaymentRq.ChannelID == null || PaymentRq.ChannelID == "")
            {
                res.ResponseCode = "24";
                res.ResponseDescription = "ChannelID Empty";
                isErrorParam = true;
            }

            if (PaymentRq.TransactionDate == null || PaymentRq.TransactionDate == "")
            {
                res.ResponseCode = "25";
                res.ResponseDescription = "TransactionDate Empty";
                isErrorParam = true;
            }

            if (DateTime.TryParseExact(
                PaymentRq.TransactionDate,
                "yyyyMMddHHmmss", //format
                CultureInfo.CurrentCulture,
                DateTimeStyles.None, out outDate))
            {
                res.ResponseCode = "26";
                res.ResponseDescription = "TransactionDate format not match";
                isErrorParam = true;
            }


            if (isErrorParam == false)
            {
                try
                {
                    string ssql = "exec [dbo].[sp_PostingPayment_VA_CIMB] "+ PaymentRq.TransactionID +","+ PaymentRq.CompanyCode+" ," + PaymentRq.CustomerKey1 + ", " + PaymentRq.CustomerName + "," + PaymentRq.Amount + "," + dtformat + "";
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
                            //string jsonRes_Er = JsonConvert.SerializeObject(res);
                            InsertLogData("https://external.mncleasing.com/VACIMBWebService/api/Inquiry", "", 0, PaymentRq.ToString(), "ERROR Interact with DB", "sysadmin");
                        }

                    res.TransactionID = PaymentRq.TransactionID;
                    res.ChannelID = PaymentRq.ChannelID;
                    res.TerminalID = PaymentRq.TerminalID;
                    res.TransactionDate = PaymentRq.TransactionDate;
                    res.CompanyCode = PaymentRq.CompanyCode;
                    res.CustomerKey1 = PaymentRq.CustomerKey1;
                    res.CustomerKey2 = PaymentRq.CustomerKey2;
                    res.CustomerKey3 = PaymentRq.CustomerKey3;
                    res.PaymentFlag = "1";
                    res.CustomerName = PaymentRq.CustomerName;
                    res.Currency = "IDR";
                    res.Amount = PaymentRq.Amount;
                    res.Fee = PaymentRq.Fee;
                    res.PaidAmount = PaymentRq.PaidAmount;
                    res.ReferenceNumberTransaction = PaymentRq.ReferenceNumberTransaction;
                    res.AdditionalData1 = PaymentRq.AdditionalData1;
                    res.AdditionalData2 = PaymentRq.AdditionalData2;
                    res.AdditionalData3 = PaymentRq.AdditionalData3;
                    res.AdditionalData4 = PaymentRq.AdditionalData4;
                    res.ResponseCode = "00";
                    res.ResponseDescription = "Transaction Success";

                    InsertLogData("https://external.mncleasing.com/VACIMBWebService/api/Inquiry", res.ToString(), 0, PaymentRq.ToString(), res.ResponseDescription.ToString(), "sysadmin");
                }
                catch (Exception ex)
                {
                    InsertLogData("https://external.mncleasing.com/VACIMBWebService/api/Inquiry", ex.Message.ToString(), 0, PaymentRq.ToString() , "ERROR 500 : Internal Server Error", "sysadmin");
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
