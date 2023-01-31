using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using VACIMB_Service.Models;
using VACIMB_Service.Controllers;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Data;
using System.Web.Http;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using System.Text;

namespace VACIMB_Service
{
    /// <summary>
    /// Summary description for VACIMB_Inquiry
    /// </summary>
    [WebService(Namespace = "http://CIMB3rdParty/BillPaymentWS")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(true)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class VACIMB_Inquiry : System.Web.Services.WebService
    {
        [XmlRoot(ElementName = "InquiryParamList")]
        public class InquiryParamList
        {
            [XmlElement("InquiryParam")]
            public List<InquiryParam> InquiryParam { get; set; }
        }

        public class InquiryParam
        {
            [XmlElement("TransactionID")]
            public string TransactionID { get; set; }
            [XmlElement("ChannelID")]
            public string ChannelID { get; set; }
            [XmlElement("TerminalID")]
            public string TerminalID { get; set; }
            [XmlElement("TransactionDate")]
            public string TransactionDate { get; set; }
            [XmlElement("CompanyCode")]
            public string CompanyCode { get; set; }
            [XmlElement("CustomerKey1")]
            public string CustomerKey1 { get; set; }
            [XmlElement("CustomerKey2")]
            public string CustomerKey2 { get; set; }
            [XmlElement("CustomerKey3")]
            public string CustomerKey3 { get; set; }
            [XmlElement("AdditionalData1")]
            public string AdditionalData1 { get; set; }
            [XmlElement("AdditionalData2")]
            public string AdditionalData2 { get; set; }
            [XmlElement("AdditionalData3")]
            public string AdditionalData3 { get; set; }
            [XmlElement("AdditionalData4")]
            public string AdditionalData4 { get; set; }
        }

        public List<dataTask> data { get; set; }

        [WebMethod]
        public CIMBWebReference.inquiryResponse CIMB3rdParty_InquiryRq(CIMBWebReference.inquiryRequest InquiryRq)
        {
            XmlDocument xdoc = new XmlDocument();
            using (Stream receiveStream = HttpContext.Current.Request.InputStream)
            {
                // Move to begining of input stream and read
                receiveStream.Position = 0;
                using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8))
                {
                    // Load into XML document
                    xdoc.Load(readStream);
                    var xmlstr = xdoc.InnerXml.ToString();

                    //XmlSerializer serializer = new XmlSerializer(typeof(CIMBWebReference.InquiryRq));
                    //using (TextReader reader = new StringReader(xmlstr))
                    //{
                    //    CIMBWebReference.InquiryRq result = (CIMBWebReference.InquiryRq)serializer.Deserialize(reader);
                    //}

                    var settings = new XmlReaderSettings();
                    var obj = new CIMBWebReference.InquiryRq();
                    //var reader = XmlReader.Create(xdoc.InnerXml, settings);
                    XmlTextReader reader = new XmlTextReader(new StringReader(xmlstr));
                    var serializer = new System.Xml.Serialization.XmlSerializer(typeof(CIMBWebReference.InquiryRq));
                    obj = (CIMBWebReference.InquiryRq)serializer.Deserialize(reader);
                    reader.Close();
                }
            }

            //res.AdditionalData1 = "a";
            //res.AdditionalData2 = "b";
            //res.AdditionalData3 = "c";
            //res.AdditionalData4 = "d";
            //res.Amount = 100000;
            //res.AmountSpecified = false;
            //res.Fee = 5000;

            CIMBWebReference.InquiryRq reqz = new CIMBWebReference.InquiryRq();

            //InquiryParam rez = x;

            CIMBWebReference.inquiryRequest inqr = new CIMBWebReference.inquiryRequest();

            CIMBWebReference.InquiryRq inqer = new CIMBWebReference.InquiryRq();

            InquiryParam req = new InquiryParam();
            CIMBWebReference.inquiryResponse res = new CIMBWebReference.inquiryResponse();

            //InquiryVA reszzz = new InquiryVA();

            bool isErrorParam = false;
            DateTime outDate = DateTime.Now;

            try
            {
                //if (x.CompanyCode == null || x.CompanyCode == "")
                //{
                //    res.ResponseCode = "10";
                //    res.ResponseDescription = "CompanyCode Empty";
                //    isErrorParam = true;
                //}

                //if (x.CustomerKey1 == null || x.CustomerKey1 == "")
                //{
                //    res.ResponseCode = "11";
                //    res.ResponseDescription = "CustomerNumber Empty";
                //    isErrorParam = true;
                //}

                //if (x.TransactionID == null || x.TransactionID == "")
                //{
                //    res.ResponseCode = "12";
                //    res.ResponseDescription = "RequestID Empty";
                //    isErrorParam = true;
                //}

                //if (x.ChannelID == null || x.ChannelID == "")
                //{
                //    res.ResponseCode = "13";
                //    res.ResponseDescription = "ChannelType Empty";
                //    isErrorParam = true;
                //}

                //if (x.TransactionDate == null || x.TransactionDate == "")
                //{
                //    res.ResponseCode = "14";
                //    res.ResponseDescription = "TransactionDate Empty";
                //    isErrorParam = true;
                //}

                //if (DateTime.TryParseExact(
                //    x.TransactionDate,
                //    "yyyyMMddHHmmss", //format
                //    CultureInfo.CurrentCulture,
                //    DateTimeStyles.None, out outDate))
                //{
                //    res.ResponseCode = "15";
                //    res.ResponseDescription = "TransactionDate format not match";
                //    isErrorParam = true;
                //}

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
                            ////PARAM SP
                            //cmd.Parameters.Add("customerId", SqlDbType.VarChar, 100);
                            ////PARAM SP VALUE
                            //cmd.Parameters["@customerId"].Value = req.CustomerKey1;

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

                                resp.TransactionID = InquiryRq.InquiryRq.TransactionID;
                                resp.ChannelID = InquiryRq.InquiryRq.ChannelID;
                                resp.TerminalID = InquiryRq.InquiryRq.TerminalID;
                                resp.TransactionDate = InquiryRq.InquiryRq.TransactionDate;
                                resp.CompanyCode = InquiryRq.InquiryRq.CompanyCode;
                                resp.CustomerKey1 = InquiryRq.InquiryRq.CustomerKey1;
                                resp.CustomerKey2 = InquiryRq.InquiryRq.CustomerKey2;
                                resp.CustomerKey3 = InquiryRq.InquiryRq.CustomerKey3;
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
                                resp.CustomerName = dr["billingName"].ToString();
                                resp.AdditionalData1 = InquiryRq.InquiryRq.AdditionalData1;
                                resp.AdditionalData2 = InquiryRq.InquiryRq.AdditionalData2;
                                resp.AdditionalData3 = InquiryRq.InquiryRq.AdditionalData3;
                                resp.AdditionalData4 = InquiryRq.InquiryRq.AdditionalData4;
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

            return res;
        }

        //public InquiryRequest Post(InquiryRequest inqReq)
        //{
        //    InquiryRequest res = inqReq;

        //    try
        //    {
        //        VACIMB_Service.Models.InquiryVA r = InquiryVA.GetResult(res);
        //    }
        //    catch (Exception ex)
        //    {
        //        InsertLogData("https://external.mncleasing.com/VACIMBWebService/api/Inquiry", "gagal cok ancok", 1, "", "POST", "sysadmin");
        //    }


        //    return res;
        //}

        //public string Get()
        //{
        //    return "Welcome To Web API MNC GUI";
        //}

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
            public string transactionID { get; set; }
            public string channelID { get; set; }
            public string terminalID { get; set; }
            public string transactionDate { get; set; }
            public string companyCode { get; set; }
            public string customerKey1 { get; set; }
            public string customerKey2 { get; set; }
            public string customerKey3 { get; set; }
            //public BillDetail[] billDetailList
            public string currency { get; set; }
            public int amount { get; set; }
            public bool amountSpecified { get; set; }
            public int fee { get; set; }
            public bool feeSpecified { get; set; }
            public int paidAmount { get; set; }
            public bool paidAmountSpecified { get; set; }
            public string customerName { get; set; }
            public string additionalData1 { get; set; }
            public string additionalData2 { get; set; }
            public string additionalData3 { get; set; }
            public string additionalData4 { get; set; }
            public string flagPayment { get; set; }
            public string responseCode { get; set; }
            public string responseDescription { get; set; }
        }

        //public class InquiryParam
        //{
        //    public string TransactionID { get; set; }
        //    public string ChannelID { get; set; }
        //    public string TerminalID { get; set; }
        //    public string TransactionDate { get; set; }
        //    public string CompanyCode { get; set; }
        //    public string CustomerKey1 { get; set; }
        //    public string CustomerKey2 { get; set; }
        //    public string CustomerKey3 { get; set; }
        //    public string AdditionalData1 { get; set; }
        //    public string AdditionalData2 { get; set; }
        //    public string AdditionalData3 { get; set; }
        //    public string AdditionalData4 { get; set; }
        //}
    }
}
