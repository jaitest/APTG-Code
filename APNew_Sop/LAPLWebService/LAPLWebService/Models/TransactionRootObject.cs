using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LAPLWebService.Models
{
    public class TransactionRootObject
    {
        public string SecurityKey { get; set; }
        public List<TransactionFields> Data { get; set; }
    }
}