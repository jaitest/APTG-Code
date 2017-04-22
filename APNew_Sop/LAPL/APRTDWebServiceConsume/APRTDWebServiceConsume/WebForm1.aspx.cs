//using System;
//using System.Collections.Generic;
using System.Linq;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using System.Net.Http;
//using RestSharp;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.Configuration;
using System.IO;
using System.Drawing;
using System;
using RestSharp;
using System.Collections.Generic;
using Newtonsoft.Json;
using RestSharp.Authenticators;

namespace APRTDWebServiceConsume
{
    // var client = new RestClient("http://productionapp.trafficmanager.net:8080/registration/login");
    //string locationJSON = "{\"username\":\"LAPLFORHSRP\",\"password\":\"wE4FtNBoXwrmH1kr8wW/Vg==\"}";
    //var client = new RestClient("http://productionapp.trafficmanager.net:8080/registration/hsrp/notify/payment/");
    
    public class Test
    {
        public string userName { get; set; }
        public string password { get; set; }
    }
  
    public partial class WebForm1 : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
           

            //string b = "{\"username\":\"LAPLFORHSRP\",\"password\":\"wE4FtNBoXwrmH1kr8wW/Vg==\"}";
           // Test aa = JsonConvert.DeserializeObject<Test>(b);

        }
         //string CnnString = string.Empty;
        string SQLString = string.Empty;
        string CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string b = "{\"username\":\"LAPLFORHSRP\",\"password\":\"wE4FtNBoXwrmH1kr8wW/Vg==\"}";
            //string b="{\"username\":\"laplforhsrp\",\"password\":\"wE4FtNBoXwrmH1kr8wW/Vg==\"}";  --- Local
            Test aa = JsonConvert.DeserializeObject<Test>(b);
            token = Login(aa.ToString());
            
        }
        public static string token = string.Empty;

        public string Login(string locationJSON)
        {
            // var client = new RestClient("http://59.162.46.199:8181/registration/login");    --- Local
            var client = new RestClient("http://productionapp.trafficmanager.net:8080/registration/login");
            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("cache-control", "no-cache");
            // request.AddHeader("Authorization", _authorizationResponse.Token);
            request.AddParameter("Application/Json", locationJSON, ParameterType.RequestBody);
           var  response2 = client.Execute<Login>(request);
            var name = response2.Data.token;
            //var response = client.Execute(request);
            //return response.Content.ToString();
            return name;
        }

        protected void btnConfirmPayment_Click(object sender, EventArgs e)
        {
            string json = "{\"transactionNo\":\"TS01010101\",\"transactionStatus\":\"PR\",\"amount\":\"245\",\"paymentReceivedDate\":\"07/09/2016\",\"orderNo\":\"4\",\"orderDate\":\"07/09/2016\",\"authRefNo\":\"HSRP101\"}";
            string s=ConfirmPayment(json, token);
        }

        public string ConfirmPayment(string locationJSON, string token)
        {
            var client = new RestClient("http://productionapp.trafficmanager.net:8080/registration/hsrp/notify/payment/");
           // var client = new RestClient("http://59.162.46.199:8181/registration/hsrp/notify/payment/");    --- Local
            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Authorization", token);
            request.AddParameter("Application/Json", locationJSON, ParameterType.RequestBody);
            var response = client.Execute(request);
            return response.StatusCode.ToString();
        }



        protected void btnUpdateLaserCodes_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    string SQLString = "select hsrprecordid,vehicleregno,hsrp_rear_lasercode,hsrp_Front_lasercode,HSRPRecord_AuthorizationNo,OrderEmbossingDate from hsrprecords where hsrp_stateid ='9' and paymentgateway='NewSop-SBI' and orderstatus ='Closed' and APWebserviceRespMsg='Save successfully' and APNewSOPWebserviceResp='0' order by VehicleClass,VehicleType,VehicleRegNo";
            //     DataTable dtRecords = Utils.GetDataTable(SQLString, CnnString);

            //     if (dtRecords.Rows.Count > 0)
            //     {
            //         for (int k = 0; k < dtRecords.Rows.Count; k++)
            //         {
            //             //string json = "{\"frontLaserCode\":\"" + dtRecords.Rows[k]["OrderEmbossingDate"].ToString() + "\",\"authRefNo\":\"" + dtRecords.Rows[k]["HSRPRecord_AuthorizationNo"].ToString() + "\"}";
            //             string json = "{\"frontLaserCode\":\"" + dtRecords.Rows[k]["hsrp_Front_lasercode"].ToString() + "\",\"rearLaserCode\":\"" + dtRecords.Rows[k]["hsrp_rear_lasercode"].ToString() + "\",\"authRefNo\":\"" + dtRecords.Rows[k]["HSRPRecord_AuthorizationNo"].ToString() + "\"}";
            //             string s = UpdateLaserCodes(json, token);
                      
            //         }
            //     }
                
            //}
            //catch (Exception ex)
            //{
                
            //    throw ex;
            //}
            string json = "{\"frontLaserCode\":\"AA211026072\",\"rearLaserCode\":\"AA211026073\",\"embossingDate\":\"08/09/2016\",\"authRefNo\":\"HSRP101\"}";
            string s = UpdateLaserCodes(json, token);
        }
        public string UpdateLaserCodes(string locationJSON, string token)
        {
            var client = new RestClient("http://productionapp.trafficmanager.net:8080/registration/hsrp/provide/laser/");
            // var client = new RestClient("http://59.162.46.199:8181/registration/hsrp/provide/laser/");  --- Local
            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Authorization", token);
            request.AddParameter("Application/Json", locationJSON, ParameterType.RequestBody);
            var response = client.Execute(request);
            return response.StatusCode.ToString();
        }

        protected void btnHSRPAvailability_Click(object sender, EventArgs e)
        {
            string json = "{\"affixationCenterCode\":\"AP031\",\"hsrpAvailabilityDate\":\"09/09/2016\",\"authRefNo\":\"HSRP101\"}";
            string s = HSRPAvailability(json, token);
        }

        public string HSRPAvailability(string locationJSON, string token)
        {
            var client = new RestClient("http://productionapp.trafficmanager.net:8080/registration/hsrp/notify/affixation");
            //var client = new RestClient("http://59.162.46.199:8181/registration/hsrp/notify/affixation");  --- Local
            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Authorization", token);
            request.AddParameter("Application/Json", locationJSON, ParameterType.RequestBody);
            var response = client.Execute(request);
            return response.StatusCode.ToString();
        }

        protected void btnConfirmAffixation_Click(object sender, EventArgs e)
        {
            string json = "{\"affixationCenterCode\":\"AP031\",\"affixationDate\":\"10/09/2016\",\"authRefNo\":\"HSRP101\"}";
            string s = ConfirmAffixation(json, token);
        }
        public string ConfirmAffixation(string locationJSON, string token)
        {
            var client = new RestClient("http://productionapp.trafficmanager.net:8080/registration/hsrp/confirm/affixation");
            //var client = new RestClient("http://59.162.46.199:8181/registration/hsrp/confirm/affixation"); --- Local
            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Authorization", token);
            request.AddParameter("Application/Json", locationJSON, ParameterType.RequestBody);
            var response = client.Execute(request);
            return response.StatusCode.ToString();
        }

    }
}