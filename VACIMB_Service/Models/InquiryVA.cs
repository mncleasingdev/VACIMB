using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Xml.Xsl;
using VACIMB_Service.Models;

namespace VACIMB_Service.Models
{
    public class InquiryVA
    {

        public List<dataTask> data { get; set; }

        //public List<CIMBWebReference.InquiryRq> data { get; set; }

        public static InquiryVA GetResult(InquiryRequest x)
        {
            //string strResult = "";
            CIMBWebReference.InquiryRq req = new CIMBWebReference.InquiryRq();
            CIMBWebReference.InquiryRs res = new CIMBWebReference.InquiryRs();

            InquiryVA reszzz = new InquiryVA();

            bool isErrorParam = false;
            DateTime outDate = DateTime.Now;

            try
            {
                if (x.CompanyCode == null || x.CompanyCode == "")
                {
                    res.ResponseCode = "10";
                    res.ResponseDescription = "CompanyCode Empty";
                    isErrorParam = true;
                }

                if (x.CustomerKey1 == null || x.CustomerKey1 == "")
                {
                    res.ResponseCode = "11";
                    res.ResponseDescription = "CustomerNumber Empty";
                    isErrorParam = true;
                }

                if (x.TransactionID == null || x.TransactionID == "")
                {
                    res.ResponseCode = "12";
                    res.ResponseDescription = "RequestID Empty";
                    isErrorParam = true;
                }

                if (x.ChannelID == null || x.ChannelID == "")
                {
                    res.ResponseCode = "13";
                    res.ResponseDescription = "ChannelType Empty";
                    isErrorParam = true;
                }

                if (x.TransactionDate == null || x.TransactionDate == "")
                {
                    res.ResponseCode = "14";
                    res.ResponseDescription = "TransactionDate Empty";
                    isErrorParam = true;
                }

                if (DateTime.TryParseExact(
                    x.TransactionDate,
                    "yyyyMMddHHmmss", //format
                    CultureInfo.CurrentCulture,
                    DateTimeStyles.None, out outDate))
                {
                    res.ResponseCode = "15";
                    res.ResponseDescription = "TransactionDate format not match";
                    isErrorParam = true;
                }

                //16,17,18,19 later

                if (isErrorParam == false)
                {
                    string ssql = "exec [dbo].[sp_MNCL_CIMB_VA_GetBill]";
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
                            //PARAM SP
                            cmd.Parameters.Add("ACCOUNT", SqlDbType.VarChar, 100);
                            //PARAM SP VALUE
                            cmd.Parameters["@ACCOUNT"].Value = req.CustomerKey1;

                            cmd.ExecuteNonQuery();
                            SqlDataReader reader = sqlCommand.ExecuteReader();

                            resDT.Load(reader);
                            conn.Close();


                            CIMBWebReference.BillDetail mdlBill = new CIMBWebReference.BillDetail();

                            var listBilldt = new List<CIMBWebReference.BillDetail>();
                            var listDtTask = new List<CIMBWebReference.InquiryRs>();

                            foreach (DataRow dr in resDT.Rows)
                            {
                                CIMBWebReference.InquiryRs resp = new CIMBWebReference.InquiryRs();
                                resp.TransactionID = x.TransactionID;
                                resp.ChannelID = x.ChannelID;
                                resp.TerminalID = x.TerminalID;
                                resp.TransactionDate = x.TransactionDate;
                                resp.CompanyCode = x.CompanyCode;
                                resp.CustomerKey1 = x.CustomerKey1;
                                resp.CustomerKey2 = x.CustomerKey2;
                                resp.CustomerKey3 = x.CustomerKey3;
                                //LIST BILL DETAIL
                                mdlBill.BillCurrency = "IDR";
                                mdlBill.BillCode = "";
                                //mdlBill.BillAmount = Convert.ToInt32(dr["installment_amt"]);
                                mdlBill.BillCode = "";

                                resp.Currency = "IDR";
                                //SUM ALL AMOUNT FROM BILL DETAIL
                                resp.Amount = Convert.ToInt32(dr["installment_amt"]);
                                resp.Fee = 10000;
                                resp.PaidAmount = resp.Amount + resp.Fee;
                                resp.CustomerName = dr["Customer_Name"].ToString();
                                resp.AdditionalData1 = x.AdditionalData1;
                                resp.AdditionalData2 = x.AdditionalData2;
                                resp.AdditionalData3 = x.AdditionalData3;
                                resp.AdditionalData4 = x.AdditionalData4;
                                resp.FlagPayment = "1";
                                resp.ResponseCode = "00";
                                resp.ResponseDescription = "Transaction Success";

                                listDtTask.Add(resp);
                            }

                            //final add to header
                            //res.data = listDtTask;

                            //string jsonRes = JsonConvert.SerializeObject(res);
                            InsertLogData("https://external.mncleasing.com/VACIMBWebService/api/Inquiry", "", 1, "", "POST", "sysadmin");
                        }

                        catch (SqlException ex)
                        {
                            //string jsonRes_Er = JsonConvert.SerializeObject(res);
                            InsertLogData("https://external.mncleasing.com/VACIMBWebService/api/Inquiry", "", 0, "ERROR 500 : Internal Server Error", "POST", "sysadmin");
                        }
                }
            }
            catch (Exception ex)
            {

                InsertLogData("https://external.mncleasing.com/VACIMBWebService/api/Inquiry", ex.Message.ToString(), 0, "ERROR 500 : Internal Server Error", "POST", "sysadmin");
            }

            return reszzz;
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

        public class dataTask
        {
            public string transactionIDField { get; set; }
            public string channelIDField { get; set; }
            public string terminalIDField { get; set; }
            public string transactionDateField { get; set; }
            public string companyCodeField { get; set; }
            public string customerKey1Field { get; set; }
            public string customerKey2Field { get; set; }
            public string customerKey3Field { get; set; }
            public string additionalData1Field { get; set; }
            public string additionalData2Field { get; set; }
            public string additionalData3Field { get; set; }
            public string additionalData4Field { get; set; }
        }
    }
}