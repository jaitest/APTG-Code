using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace IntegrationKIt4._5
{
    public partial class AxisReconcile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string strPaymentStatus = string.Empty;
            var request = (HttpWebRequest)WebRequest.Create("https://www.axisbiconnect.co.in/AXISPaymentsVerification/Web/Applications/Query.aspx");
            var postData = "payeeid=000000004658";
            postData += "&itc="+AuthNo.Text;
            postData += "&prn=" + OrderNo.Text ;
            postData += "&date=" + OrderDate.Text;
            postData += "&amt=" + OrderAmt.Text;
            var data = Encoding.ASCII.GetBytes(postData);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            var  response = (HttpWebResponse)request.GetResponse();
            ResponseText.Text = response.ToString();
            //using (StringReader stringReader = new StringReader(response))
            //using (XmlTextReader reader = new XmlTextReader(stringReader))
            //{
            //    while (reader.Read())
            //    {

            //        if (reader.Name.ToString() == "PaymentStatus")
            //        {
            //            reader.Read();
            //            strPaymentStatus = reader.Value.ToString();
            //            reader.Read();
            //        }
            //    }
            //}
        }
    }
}