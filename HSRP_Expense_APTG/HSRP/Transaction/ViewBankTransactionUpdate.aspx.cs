using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace HSRP.Transaction
{


    public partial class ViewBankTransactionUpdate : System.Web.UI.Page
    {
          String ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        Utils bl = new Utils();

        string HSRPStateID = string.Empty;
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        string UserType1 = string.Empty;
        string ProductivityID = string.Empty;
        int UserType;
        //string HSRPStateID = string.Empty, RTOLocationID = string.Empty, ProductivityID = string.Empty, UserType = string.Empty, UserName = string.Empty;
     
        String StringMode = string.Empty;
        string CurrentDate = DateTime.Now.ToString();
        string query1 = string.Empty;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            Utils.GZipEncodePage();

            //if (StringMode.Equals("Voids"))
            //{
            //    ProductivityID = Request.QueryString["TransactionID"].ToString();                

            //}
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                HSRPStateID = Session["UserHSRPStateID"].ToString();
                UserType = Convert.ToInt32(Session["UserType"].ToString());
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                strUserID = Session["UID"].ToString();
                ComputerIP = Request.UserHostAddress;

                if (!IsPostBack)
                {
                    try
                    {
                        buildGrid();
                       // Filldropdowndealer();


                       // Utils.user_log(strUserID, "View Organization", ComputerIP, "Page load", CnnString);
                    }
                    catch (Exception err)
                    {
                        lblErrMsg.Text = "Error on Page Load" + err.Message.ToString();
                    }
                }
            }
        }

        private void Filldropdowndealer()
        {
            if (UserType1 == "0")
            {
                // SQLString = "select [NAME OF THE DEALER] from delhi_dealermaster  where ActiveStatus='Y'";
                SQLString = "select distinct bt.DealerID,dm.dealername from dealermaster dm join BankTransaction bt on bt.dealerid=dm.dealerid where bt.stateid='" + HSRPStateID + "'";
               // SQLString = "select distinct dealerid,(select [dealername] from dealerMaster a where a.DealerId=hsrprecords.dealerid) as DealerName from hsrprecords where HSRP_StateID='" + DropDownListStateName.SelectedValue + "' and Addrecordby='Dealer'";
                Utils.PopulateDropDownList(ddlBothDealerHHT, SQLString.ToString(), CnnString, "All");
            }
            else
            {
                // SQLString = "select [NAME OF THE DEALER]  from delhi_dealermaster  where  ActiveStatus='Y'";
                SQLString = "select distinct bt.DealerID,dm.dealername from dealermaster dm join BankTransaction bt on bt.dealerid=dm.dealerid where bt.stateid='" + HSRPStateID + "'";
               // SQLString = "select distinct dealerid,(select [dealername] from dealermaster a where a.DealerId=hsrprecords.dealerid) as DealerName from hsrprecords where HSRP_StateID='" + DropDownListStateName.SelectedValue + "' and Addrecordby='Dealer'";
                Utils.PopulateDropDownList(ddlBothDealerHHT, SQLString.ToString(), CnnString, "All");
                //DataTable dts = Utils.GetDataTable(SQLString, CnnString);
                //ddlBothDealerHHT.DataSource = dts;
                //ddlBothDealerHHT.DataBind();
            }
        }

        public void buildGrid()
        {
            try
            {
               // string SQLString = "SELECT [TransactionID] ,convert(varchar(10), DepositDate, 103) AS 'Deposit Date',bm.[BankName],[BranchName],[DepositAmount],[DepositBy],r.RtoLocationName as DepositLocation,[StateID],[RTOLocation],[UserID],convert(varchar(10), CurrentDate, 103) as CurrentDate,[BankSlipNo],[Remarks],bm.[AccountNo],[ETAProcess],[depositelocationid],[EntryType],[oldTransactionID],[EntryDate],[DealerID],[chq_no],[chq_date],[ApprovedStatus],[VoidStatus] FROM [dbo].[BankTransaction] bt inner join [dbo].[BankMaster] bm on bt.bankname=convert(varchar,bm.id) inner join Rtolocation r on r.rtolocationId=bt.rtolocation where StateID='" + HSRPStateID + "' and isnull(BankSlipNo,'')!='' order by DepositDate desc";
                string SQLString = "SELECT  [TransactionID] ,convert(varchar(10), DepositDate, 103) AS 'Deposit Date',bm.[BankName],[BranchName],[DepositAmount],[DepositBy],r.RtoLocationName as DepositLocation,[StateID],[RTOLocation],[UserID],convert(varchar(10), CurrentDate, 103) as CurrentDate,[BankSlipNo],[Remarks],bm.[AccountNo],[voidstatus] FROM [dbo].[BankTransaction] bt inner join [dbo].[BankMaster] bm on bt.bankname=convert(varchar,bm.id) inner join Rtolocation r on r.rtolocationId=bt.rtolocation where StateID='" + HSRPStateID + "' order by DepositDate desc";

                DataTable dt = Utils.GetDataTable(SQLString.ToString(), CnnString.ToString());

                Grid1.DataSource = dt;
               // Grid1.RunningMode = (ComponentArt.Web.UI.GridRunningMode)Enum.Parse(typeof(ComponentArt.Web.UI.GridRunningMode), "Client");
              //  Grid1.SearchOnKeyPress = true;
                Grid1.DataBind();
              //  Grid1.RecordCount.ToString();
            }
            catch (Exception ex)
            {
                lblErrMsg.Text = "Error in Populating Grid :" + ex.Message.ToString();
            }
        }


        protected void btnGO_Click1(object sender, EventArgs e)
        {
            buildGrid();
        }

       


        //private void UpdateVoid(string TransactionID)
        //{
        //    SQLString = "Update BankTransaction set=voidstatus='Void' WHERE TransactionID='"+TransactionID+"'";
           
        //    int i = Utils.ExecNonQuery(SQLString, ConnectionString); //GetDataTable(SQLString, ConnectionString);          

            
        //}

        //protected void btnvoid_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        UpdateVoid(Request.QueryString["TransactionID"].ToString());
        //    }
        //    catch (Exception ex)
        //    {
                
        //        throw ex;
        //    }
        //}
    }
}