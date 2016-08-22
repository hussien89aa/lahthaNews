using DBManager;
using NewsApp.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NewsApp
{
    public partial class NewFeeds : System.Web.UI.Page
    {    DBConnection DBop = new DBConnection();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                SqlDataSource1.SelectParameters["NewsID"].DefaultValue = "0";
                SqlDataSource1.SelectParameters["EndTo"].DefaultValue = "30";
              //  SqlDataSource1.SelectParameters["q"].DefaultValue =Convert.ToString( Request.QueryString["q"]);
           if(!( Request.QueryString["Type"]==null))
           {
               SqlDataSource1.SelectParameters["Type"].DefaultValue = Request.QueryString["Type"];
           }
            }
          ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "HideLoading();", true);

          if (DataList1.Items.Count == 0)
              lblNoRecordFount.Visible = true;
          else
              lblNoRecordFount.Visible =false;
        }

        protected void BuLoadMore_Click(object sender, EventArgs e)
        {
            
         // SqlDataSource1.SelectParameters["EndTo"].DefaultValue = Convert.ToString(DataList1.Items.Count  + 20);
         //ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "MoveScroll();", true);
         //HFMaxScrol.Value = "0";
          //  HeaderMessage.Visible = false;
            if (HFMaxScrol.Value == "-1")
            {
                SqlDataSource1.SelectParameters["NewsID"].DefaultValue = "0";
                SqlDataSource1.SelectParameters["EndTo"].DefaultValue = "30";
              //  newfeedDiv.Visible = false;
             //  ScriptManager.RegisterStartupScript(this, this.GetType(), "Popa", "HideNewNewsDiv();", true);
               // ScriptManager.RegisterStartupScript(this, this.GetType(), "Popb", "ShowNoMoreNews();", true);
               DataList1.DataBind();
               // HeaderMessage.Visible = true;
              
            }

            else if (HFMaxScrol.Value == "30")
            {
                SqlDataSource1.SelectParameters["NewsID"].DefaultValue = DataList1.DataKeys[0].ToString();
                SqlDataSource1.SelectParameters["EndTo"].DefaultValue = Convert.ToString(DataList1.Items.Count + 50);
                //  ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "MoveScroll();", true);
                DataList1.DataBind();
            }
            else
            {
                // check if there is new news 
                DataTable dataTable = new DataTable();
                //password = Myenc.GetMD5Data(Encoding.Default.GetBytes(password));
                ColProcedureParam[] Coloumns = new ColProcedureParam[3];
                Coloumns[0] = new ColProcedureParam("UserID",Convert.ToString(  Request.QueryString["id"]));// temp be 1
                Coloumns[1] = new ColProcedureParam("SubNesourceID", "0");
                Coloumns[2] = new ColProcedureParam("NewsID", DataList1.DataKeys[0].ToString());
               // Coloumns[3] = new ColProcedureParam("NewsID", DataList1.DataKeys[0].ToString());
                dataTable = DBop.cobject.SelectDataSetProcedureTable("IsGetNews", Coloumns).Tables[0];
                if (  (!Convert.ToString(dataTable.Rows[0]["CountItem"]).Equals("0")))
                {
                     
                    //   LiMessage.Text = "<strong>Warning!</strong> " + DBop.cobject.ErrorMessage;
                   // ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "ShowNewNewsDiv();", true);
                    SqlDataSource1.SelectParameters["NewsID"].DefaultValue = "0";
                    SqlDataSource1.SelectParameters["EndTo"].DefaultValue = "30";
                    DataList1.DataBind();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop1", "NewNewsempty();", true);
            
                }
            }
           // HFMaxScrol.Value = "0";
        //   HeaderMessageLoading.Visible = false;

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "HideLoading();", true);
              
        }

        //protected void Timer1_Tick(object sender, EventArgs e)
        //{
            
        //        //SqlDataSource1.SelectParameters["NewsID"].DefaultValue = "0";
        //        //if (HFMaxScrol.Value == "-1")
        //        //    SqlDataSource1.SelectParameters["EndTo"].DefaultValue = "20";

        //        //else if (HFMaxScrol.Value == "20")
        //        //{
        //        //    SqlDataSource1.SelectParameters["EndTo"].DefaultValue = Convert.ToString(DataList1.Items.Count + Convert.ToInt32(HFMaxScrol.Value));
        //        //    //  ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "MoveScroll();", true);
        //        //}
        //        //else
        //        //{
        //        //    // check if there is new news 
        //        //    DataTable dataTable = new DataTable();
        //        //    //password = Myenc.GetMD5Data(Encoding.Default.GetBytes(password));
        //        //    ColProcedureParam[] Coloumns = new ColProcedureParam[3];
        //        //    Coloumns[0] = new ColProcedureParam("UserID","1");// Request.QueryString["id"]);// temp be 1
        //        //    Coloumns[1] = new ColProcedureParam("SubNesourceID", "0");
        //        //    Coloumns[2] = new ColProcedureParam("NewsID", DataList1.DataKeys[0].ToString());
        //        //    dataTable = DBop.cobject.SelectDataSetProcedureTable("IsGetNews", Coloumns).Tables[0];
        //        //    if ((dataTable != null) && (dataTable.Rows.Count > 0))
        //        //    {



        //        //        SqlDataSource1.SelectParameters["NewsID"].DefaultValue = DataList1.DataKeys[0].ToString();
        //        //        SqlDataSource1.SelectParameters["EndTo"].DefaultValue = Convert.ToString(DataList1.Items.Count);

        //        //    }
        //        //}
        //        //HFMaxScrol.Value = "0";
          

           
        //}

        //protected void BuFirstAll_Click(object sender, EventArgs e)
        //{
        //  // Timer1.Enabled = false;
        // //    SqlDataSource1.SelectParameters["EndTo"].DefaultValue = "20";
        //    //Timer1.Enabled = true;
          
        //}

        [WebMethod]  // display new   news is comming
        public static string NewNewsComming(string Tag, string UserID, string NewsID, string q)
        {
            DBConnection DBop = new DBConnection();
            // check if there is new news 
            DataTable dataTable = new DataTable();
            //password = Myenc.GetMD5Data(Encoding.Default.GetBytes(password));
            ColProcedureParam[] Coloumns = new ColProcedureParam[4];
            Coloumns[0] = new ColProcedureParam("UserID", UserID);// temp be 1
            Coloumns[1] = new ColProcedureParam("SubNesourceID", "0");
            Coloumns[2] = new ColProcedureParam("NewsID", NewsID);
           Coloumns[3] = new ColProcedureParam("q", q);
            dataTable = DBop.cobject.SelectDataSetProcedureTable("IsGetNews", Coloumns).Tables[0];
            if ((!Convert.ToString(dataTable.Rows[0]["CountItem"]).Equals("0")))
            {
                return "newNews";
            }
            else
            {
                return "NonewNews" ;

            }

  
        } 
    }
}