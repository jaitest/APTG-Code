using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HSRP.Transaction
{
    public partial class Update_Zonalmanager : System.Web.UI.Page
    {
        string Mode;
        int HSRP_StateID;
        string SQLString = string.Empty;
        string UserType = string.Empty;
        string UserID = string.Empty;
        int EditStateID;
        string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {

            Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }

            HSRP_StateID = Convert.ToInt16(Session["UserHSRPStateID"]);
            UserID = Session["UID"].ToString();
            UserType = Session["UserType"].ToString();
            if (!Page.IsPostBack)
            {
                BindGrid();
            }
        }


        public void BindGrid()
        {
            DataSet ds = Utils.getDataSet("ZonalManagerUpdationAPTG '0','0','SELECT'", ConnectionString);
            if (ds.Tables[0].Rows.Count > 0)
            {
                
                gvEmpDesignation.DataSource = ds;
                gvEmpDesignation.DataBind();
            }
            else
            {
                ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                gvEmpDesignation.DataSource = ds;
                gvEmpDesignation.DataBind();
                int columncount = gvEmpDesignation.Rows[0].Cells.Count;
                gvEmpDesignation.Rows[0].Cells.Clear();
                gvEmpDesignation.Rows[0].Cells.Add(new TableCell());
                gvEmpDesignation.Rows[0].Cells[0].ColumnSpan = columncount;
                gvEmpDesignation.Rows[0].Cells[0].Text = "No Records Found";
            }
        }

        protected void gvEmpDesignation_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void gvEmpDesignation_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvEmpDesignation.EditIndex = e.NewEditIndex;
            this.BindGrid();
        }

        protected void gvEmpDesignation_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvEmpDesignation.EditIndex = -1;
            this.BindGrid();
        }

        protected void gvEmpDesignation_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
           
            Label lblEditid = (Label)gvEmpDesignation.Rows[e.RowIndex].FindControl("lblEditid");
            TextBox txtEditlocationName = (TextBox)gvEmpDesignation.Rows[e.RowIndex].FindControl("txtEditlocationName");
            TextBox txtEditZonalName = (TextBox)gvEmpDesignation.Rows[e.RowIndex].FindControl("txtEditZonalName");


            int i = Utils.ExecNonQuery("ZonalManagerUpdationAPTG " + lblEditid.Text + ",'" + txtEditZonalName.Text + "','UPDATE'", ConnectionString);

           
            if (i > 0)
            {
                lblErrMess.Text = "";
                lblSucMess.Text = "Record Update Successfully.";
                BindGrid();
               // textboxBoxHSRPState.Text = "";
            }
            else
            {
                lblSucMess.Text = "";
                lblErrMess.Text = "Record not Update";
            }
            gvEmpDesignation.EditIndex = -1;
            this.BindGrid();
        }

        
    }
}