using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace IntegrationKIt4._5
{
    public class HSRPPayment
    {
        [Required]
        public string AuthNo { get; set; }
        [Required]
        public string ChassisNo { get; set; }
        [Required]
        public string DealerName { get; set; }
        [Required]
      
        public string DealerAddress { get; set; }
        [Required]
        public string DealerContactNo { get; set; }
        [Required]
        [Range(0, float.MaxValue, ErrorMessage = "Please enter valid float Number")]
        public float HSRPAmnt { get; set; }
        [Required]
        public string PymntDt { get; set; }
        [Required]
        public string PymntFlag { get; set; }
        [Required]
        public string SecurityKey { get; set; }


    }
}