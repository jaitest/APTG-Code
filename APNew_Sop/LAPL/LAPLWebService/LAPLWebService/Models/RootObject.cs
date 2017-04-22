using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LAPLWebService.Models
{
    public class RootObject
    {
        public string SecurityKey { get; set; }
        public List<HSRPFields> Data { get; set; }
    }
}