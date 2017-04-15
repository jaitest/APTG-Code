using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IntegrationKIt4._5
{
    public partial class returnurl : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
          if(  Request.Form.Count>0)
          {
              AuthNo.Text = Request.Form["AuthNo"];
              ChassisNo.Text = Request.Form["ChassisNo"];
              DealerName.Text = Request.Form["DealerName"];
              DealerId.Text = Request.Form["DealerId"];
              DealerAddress.Text = Request.Form["DealerAddress"];
              DealerContactNo.Text = Request.Form["DealerContactNo"];
              HSRPAmnt.Text = Request.Form["HSRPAmnt"];
              PymntDt.Text = Request.Form["PymntDt"];
              PymntFlag.Text = Request.Form["PymntFlag"];
              PymntErrorMsg.Text = Request.Form["PymntErrorMsg"];
              PymntOrderNo.Text = Request.Form["PymntOrderNo"];
          }

        }
    }
}