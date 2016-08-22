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
    public partial class ChannelNews : System.Web.UI.Page
    {
        DBConnection DBop = new DBConnection();
        public String UserImageURL = "/Image/business_user.png";
        public String UserImageBackgroundURL = "/Image/business_user.png";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ColProcedureParam[] Coloumns = new ColProcedureParam[1];
                Coloumns[0] = new ColProcedureParam("SubNesourceID", Request.QueryString["NID"].ToString());
           
                DataTable dataTable = new DataTable();
                dataTable = DBop.cobject.SelectDataSetProcedureTable("myChannel", Coloumns).Tables[0];
                if ((dataTable != null) && (dataTable.Rows.Count > 0))
                {
                    laName.Text = Convert.ToString(dataTable.Rows[0]["SubNesourceName"]);
                    lafollowers.Text =" عدد المتابعين "+ Convert.ToString(dataTable.Rows[0]["Followers"]);
                    UserImageURL = Convert.ToString(dataTable.Rows[0]["ChannelImage"]);
                    UserImageBackgroundURL = Convert.ToString(dataTable.Rows[0]["BacugroundPicture"]);
                
                }
               
                    //SqlDataSource1.SelectParameters["NewsID"].DefaultValue = "0";
                    //SqlDataSource1.SelectParameters["EndTo"].DefaultValue = "20";
                 
            
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "HideLoading();", true);
            // rest button
            //DBop.cobject.DeletedRow("UserFollowing", "UserID=" + UserID + " and SubNesourceID=" + SubNesourceID) 


        }

        protected void BuLoadMore_Click(object sender, EventArgs e)
        {

            if (HFMaxScrol.Value == "-1")
            {
                SqlDataSource1.SelectParameters["EndTo"].DefaultValue ="30";
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "MoveScroll();", true);
            

            }

            else if (HFMaxScrol.Value == "30")
            {
                SqlDataSource1.SelectParameters["EndTo"].DefaultValue = Convert.ToString(DataList1.Items.Count + 50);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "MoveScroll();", true);
            
            }
        }

        [WebMethod]
        public static void DeleteItem(string Tag, string UserID, string SubNesourceID)
        {

            

            DBConnection DBop = new DBConnection();
                if (Tag.Equals("1"))
            {  // addfollow
                ColoumnParam[] Coloumns = new ColoumnParam[3];
                Coloumns[0] = new ColoumnParam("UserID", ColoumnType.Int, UserID);
                Coloumns[1] = new ColoumnParam("SubNesourceID", ColoumnType.Int, SubNesourceID);
                Coloumns[2] = new ColoumnParam("DateRegister", ColoumnType.DateTime, DateTime.Now.ToString());
              DBop.cobject.InsertRow("UserFollowing", Coloumns) ;
                 

            }
            else  // delete follow
            {
       DBop.cobject.DeletedRow("UserFollowing", "UserID=" + UserID + " and SubNesourceID=" + SubNesourceID)  ;
               
            }
        }
    }
}