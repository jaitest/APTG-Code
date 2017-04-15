using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace LAPLWebService.Models
{
    public class PRRootObject
    {
        public string SecurityKey { get; set; }
        public List<PRFields> Data { get; set; }
    }
}