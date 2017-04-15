using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LAPLWebService.Models
{
    public class TransactionFields
    {
        public string transactionNo { get; set; }
        public string transactionDate { get; set; }
        public string amount { get; set; }
        public string authorizationRefNo { get; set; }
    }
}