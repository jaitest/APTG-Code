using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;

namespace HSRP.Master
{
    public partial class UpdateEmbosingCenter : System.Web.UI.Page
    {
        int UserID;
        int intStateID;
        int HSRPStateID;
        int intUserID;
        int UserType;
        string CnnString = string.Empty;
        String SQLString = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Utils.GZipEncodePage();
                lblErrMess.Text = string.Empty;
                lblSucMess.Text = string.Empty;
                if (Session["UID"] == null)
                {
                    Response.Redirect("~/Login.aspx", true);
                }
                else
                {
                    int.TryParse(Session["UserType"].ToString(), out UserType);
                    int.TryParse(Session["UserHSRPStateID"].ToString(), out HSRPStateID);
                    int.TryParse(Session["UID"].ToString(), out UserID);
                    lblErrMess.Text = string.Empty;
                    lblSucMess.Text = string.Empty;
                    CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                   

                }
                if (!IsPostBack)
                {

                    FilldropDownListdealer();
                    FilldropDownListCenter();
                }
                  
                
            }
            catch (Exception ee)
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Contact to admin.";
                lblErrMess.Text = lblErrMess.Text + " " + ee.Message;
            }
        }

        

        private void FilldropDownListdealer()
        {
            SQLString = "select sno,[NAME OF THE DEALER] +'//Dealer Id -'+ convert(varchar(max), sno) as dealername    from delhi_dealermaster where activestatus = 'Y'";
            Utils.PopulateDropDownList(dropDownDealer, SQLString,CnnString ,  "--Select Dealer--" );
          
            
        }


        private void FilldropDownListCenter()
        {
            SQLString = "SELECT Emb_Center_Id, EmbCenterName fROM EmbossingCenters where State_Id = 2 and  ActiveStatus = 'Y'";
            Utils.PopulateDropDownList(dropDownListEmbCenter, SQLString, CnnString, "--Select Emb Center Name--");
                    

        }

      
         protected void buttonSave_Click(object sender, EventArgs e)
        {
              lblErrMess.Text = String.Empty;
              lblErrMess.Text = string.Empty;
            if ( string.IsNullOrEmpty( dropDownDealer.SelectedValue )|| dropDownDealer.SelectedValue.Equals("--Select Dealer--"))
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Select Dealer.";
                return;
            }
            if (string.IsNullOrEmpty(dropDownListEmbCenter.SelectedValue) || dropDownListEmbCenter.SelectedValue.Equals("--Select Emb Center Name--"))
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Select Embossing Center.";
                return;
            }
            string sqlquery = "update delhi_dealermaster set  ERPembCode ='" + dropDownListEmbCenter.SelectedValue.ToString() + "' where  sno= '" + dropDownDealer.SelectedValue.ToString() + "'";
            int i = Utils.ExecNonQuery(sqlquery, CnnString);
               if(i>0)
               {
                 lblSucMess.Text = String.Empty;
                 lblSucMess.Text ="Updatd Successful.";
                 FilldropDownListdealer();
                 FilldropDownListCenter();
                 return;
               }
               else
               {
                   lblErrMess.Text = String.Empty;
                   lblErrMess.Text = "Not Updated .";
                   return;

               }
        }

       
       
    }
}