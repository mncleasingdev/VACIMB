using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VACIMB_Service.Models
{
    public class InquiryRequest
    {
        public string TransactionID { get; set; }
        public string ChannelID { get; set; }
        public string TerminalID { get; set; }
        public string TransactionDate { get; set; }
        public string CompanyCode { get; set; }
        public string CustomerKey1 { get; set; }
        public string CustomerKey2 { get; set; }
        public string CustomerKey3 { get; set; }
        public string AdditionalData1 { get; set; }
        public string AdditionalData2 { get; set; }
        public string AdditionalData3 { get; set; }
        public string AdditionalData4 { get; set; }
    }
}