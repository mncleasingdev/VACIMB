using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Metadata;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

namespace VACIMB_Service
{
    /// <summary>
    /// Summary description for Inquiry
    /// </summary>
    [WebService(Namespace = "http://CIMB3rdParty/BillPaymentWS")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [SoapDocumentService(RoutingStyle = SoapServiceRoutingStyle.RequestElement)]
    //Public Function Method2() As String
    //   Return "method 2"
    //End Function

    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Inquiry : System.Web.Services.WebService
    {
        CIMBWebReference.inquiryRequest a = new CIMBWebReference.inquiryRequest();

        [WebMethod]
        [SoapDocumentMethod(ResponseElementName = "CIMB3rdParty_InquiryRs")]
        [return: XmlElement("InquiryRs")]
        public CIMBWebReference.InquiryRs CIMB3rdParty_InquiryRq(CIMBWebReference.InquiryRq InquiryRq)
        {
            string time16 = DateTime.Now.ToString("yyyyMMddHHmmssff",
                                       CultureInfo.InvariantCulture);

            CIMBWebReference.inquiryResponse RESPON = new CIMBWebReference.inquiryResponse();
            CIMBWebReference.InquiryRs res = new CIMBWebReference.InquiryRs();

            string custName = "";
            string refno = "";
            int amt = 0;
            bool isErrorParam = false;
            DateTime outDate = DateTime.Now;

            if (InquiryRq.CompanyCode == null || InquiryRq.CompanyCode == "" || InquiryRq.CompanyCode != "4739")
            {
                //res.ResponseCode = "10";
                //res.ResponseDescription = "CompanyCode Empty";
                isErrorParam = true;
            }

            if (InquiryRq.CustomerKey1 == null || InquiryRq.CustomerKey1 == "")
            {
                //res.ResponseCode = "11";
                //res.ResponseDescription = "CustomerNumber Empty";
                isErrorParam = true;
            }

            if (InquiryRq.TransactionID == null || InquiryRq.TransactionID == "")
            {
                //res.ResponseCode = "12";
                //res.ResponseDescription = "RequestID Empty";
                isErrorParam = true;
            }

            if (InquiryRq.ChannelID == null || InquiryRq.ChannelID == "")
            {
                //res.ResponseCode = "13";
                //res.ResponseDescription = "ChannelType Empty";
                isErrorParam = true;
            }

            if (InquiryRq.TransactionDate == null || InquiryRq.TransactionDate == "")
            {
                //res.ResponseCode = "14";
                //res.ResponseDescription = "TransactionDate Empty";
                isErrorParam = true;
            }

            if (DateTime.TryParseExact(
                InquiryRq.TransactionDate,
                "yyyyMMddHHmmss", //format
                CultureInfo.CurrentCulture,
                DateTimeStyles.None, out outDate))
            {
                //res.ResponseCode = "15";
                //res.ResponseDescription = "TransactionDate format not match";
                isErrorParam = true;
            }

            if (isErrorParam == true)
            {
                res.TransactionID = "";
                res.ChannelID = "";
                res.TerminalID = "";
                res.TransactionDate = "";
                res.CompanyCode = "";
                res.CustomerKey1 = "";
                res.CustomerKey2 = "";
                res.CustomerKey3 = "";

                CIMBWebReference.BillDetail bd = new CIMBWebReference.BillDetail();
                List<CIMBWebReference.BillDetail> lb = new List<CIMBWebReference.BillDetail>();

                bd.BillCurrency = "";
                bd.BillCode = "";
                bd.BillAmount = 0;
                bd.BillReference = "";
                bd.BillAmountSpecified = true;

                lb.Add(bd);
                res.BillDetailList = lb.ToArray();

                res.Currency = "";
                res.Amount = 0;
                res.AmountSpecified = true;
                res.Fee = 0;
                res.FeeSpecified = true;
                res.PaidAmount = 0;
                res.PaidAmountSpecified = true;
                res.CustomerName = "";
                res.AdditionalData1 = "";
                res.AdditionalData2 = "";
                res.AdditionalData3 = "";
                res.AdditionalData4 = "";
                res.FlagPayment = "";
                if (InquiryRq.CompanyCode == null || InquiryRq.CompanyCode == "" || InquiryRq.CompanyCode != "4739")
                {
                    res.ResponseCode = "10";
                    res.ResponseDescription = "CompanyCode Empty";
                    //isErrorParam = true;
                }

                if (InquiryRq.CustomerKey1 == null || InquiryRq.CustomerKey1 == "")
                {
                    res.ResponseCode = "11";
                    res.ResponseDescription = "CustomerNumber Empty";
                    //isErrorParam = true;
                }

                if (InquiryRq.TransactionID == null || InquiryRq.TransactionID == "")
                {
                    res.ResponseCode = "12";
                    res.ResponseDescription = "RequestID Empty";
                    //isErrorParam = true;
                }

                if (InquiryRq.ChannelID == null || InquiryRq.ChannelID == "")
                {
                    res.ResponseCode = "13";
                    res.ResponseDescription = "ChannelType Empty";
                    //isErrorParam = true;
                }

                if (InquiryRq.TransactionDate == null || InquiryRq.TransactionDate == "")
                {
                    res.ResponseCode = "14";
                    res.ResponseDescription = "TransactionDate Empty";
                    //isErrorParam = true;
                }

                if (DateTime.TryParseExact(
                    InquiryRq.TransactionDate,
                    "yyyyMMddHHmmss", //format
                    CultureInfo.CurrentCulture,
                    DateTimeStyles.None, out outDate))
                {
                    res.ResponseCode = "15";
                    res.ResponseDescription = "TransactionDate format not match";
                    //isErrorParam = true;
                }

                //return res;
            }

            if (isErrorParam == false)
            {
                try
                {
                    string ssql = "exec [dbo].[sp_MNCL_CIMB_VA_GetBill] '" + InquiryRq.CustomerKey1 + "'";
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

                            foreach (DataRow dr in resDT.Rows)
                            {
                                CIMBWebReference.InquiryRs resp = new CIMBWebReference.InquiryRs();
                                amt = Convert.ToInt32(dr["installment_amt"]);
                                //resp.PaidAmount = resp.Amount + resp.Fee;
                                custName = dr["billingName"].ToString();
                                refno = dr["referenceNo"].ToString();
                            }
                        }

                        catch (SqlException ex)
                        {
                            //var result = ex.Message.ToString().Substring(ex.Message.ToString().Length - 5);
                            res.TransactionID = "";
                            res.ChannelID = "";
                            res.TerminalID = "";
                            res.TransactionDate = "";
                            res.CompanyCode = "";
                            res.CustomerKey1 = "";
                            res.CustomerKey2 = "";
                            res.CustomerKey3 = "";

                            CIMBWebReference.BillDetail bd2 = new CIMBWebReference.BillDetail();
                            List<CIMBWebReference.BillDetail> lb2 = new List<CIMBWebReference.BillDetail>();

                            bd2.BillCurrency = "";
                            bd2.BillCode = "";
                            bd2.BillAmount = 0;
                            bd2.BillReference = "";
                            bd2.BillAmountSpecified = true;

                            lb2.Add(bd2);
                            res.BillDetailList = lb2.ToArray();

                            res.Currency = "";
                            res.Amount = 0;
                            res.AmountSpecified = true;
                            res.Fee = 0;
                            res.FeeSpecified = true;
                            res.PaidAmount = 0;
                            res.PaidAmountSpecified = true;
                            res.CustomerName = "";
                            res.AdditionalData1 = "";
                            res.AdditionalData2 = "";
                            res.AdditionalData3 = "";
                            res.AdditionalData4 = "";
                            res.FlagPayment = "";

                            if (ex.Message.ToString().Substring(ex.Message.ToString().Length - 5) == "exist")
                            {
                                res.ResponseCode = "16";
                                res.ResponseDescription = "Customer Not Found";
                                isErrorParam = true;
                            }

                            else if (ex.Message.ToString().Substring(ex.Message.ToString().Length - 4) == "Paid" || (ex.Message.ToString().Substring(ex.Message.ToString().Length - 7) == "Expired"))
                            {
                                res.ResponseCode = "41";
                                res.ResponseDescription = "Bill Already Paid";
                                isErrorParam = true;
                            }

                            else
                            {
                                res.ResponseCode = "99";
                                res.ResponseDescription = "General Failure";
                                isErrorParam = true;
                            }

                            InsertLogData("https://external.mncleasing.com/VACIMBWebService/api/Inquiry", "", 0, ex.Message.ToString(), InquiryRq.ToString(), "sysadmin");
                            return res;
                        }

                    res.TransactionID = InquiryRq.TransactionID;
                    res.ChannelID = InquiryRq.ChannelID;
                    res.TerminalID = InquiryRq.TerminalID;
                    res.TransactionDate = time16;
                    res.CompanyCode = InquiryRq.CompanyCode;
                    res.CustomerKey1 = InquiryRq.CustomerKey1;
                    res.CustomerKey2 = InquiryRq.CustomerKey2;
                    res.CustomerKey3 = InquiryRq.CustomerKey3;

                    CIMBWebReference.BillDetail bd = new CIMBWebReference.BillDetail();
                    List<CIMBWebReference.BillDetail> lb = new List<CIMBWebReference.BillDetail>();

                    bd.BillCurrency = "IDR";
                    bd.BillCode = "ANGSURAN";
                    bd.BillAmount = amt;
                    bd.BillReference = refno;
                    bd.BillAmountSpecified = true;

                    lb.Add(bd);
                    res.BillDetailList = lb.ToArray();

                    res.Currency = "IDR";
                    res.Amount = amt;
                    res.AmountSpecified = true;
                    res.Fee = 0;
                    res.FeeSpecified = true;
                    res.PaidAmount = amt + res.Fee;
                    res.PaidAmountSpecified = true;
                    res.CustomerName = custName;
                    res.AdditionalData1 = InquiryRq.AdditionalData1;
                    res.AdditionalData2 = InquiryRq.AdditionalData2;
                    res.AdditionalData3 = InquiryRq.AdditionalData3;
                    res.AdditionalData4 = InquiryRq.AdditionalData4;
                    res.FlagPayment = "1";
                    res.ResponseCode = "00";
                    res.ResponseDescription = "Transaction Success";
                    
                    InsertLogData("https://external.mncleasing.com/VACIMBWebService/api/Inquiry", res.ToString(), 0, InquiryRq.ToString(), res.ResponseDescription.ToString(), "sysadmin");
                }

                catch (Exception ex)
                {
                    InsertLogData("https://external.mncleasing.com/VACIMBWebService/api/Inquiry", ex.Message.ToString(), 0, InquiryRq.ToString(), "ERROR 500 : Internal Server Error", "sysadmin");
                }
                
            }

            //RESPON.InquiryRs = res;
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
