using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.ServiceModel.Syndication;
using NewsApp.Classes;
using DBManager;
namespace NewsApp
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        DBConnection DBop = new DBConnection();
        protected void Page_Load(object sender, EventArgs e)
        {


            if (!Page.IsPostBack)
            {
                if (!(Request.QueryString["S"] == null))
                {
                    LiMessage.Text = "<strong>sucess</strong> people is added successfully.";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "ShowModalSucces();", true);
                }
            }
            // this number is temperory
          //  Session["adminID"] = 5;
        }

        protected void Buadd_Click(object sender, EventArgs e)
        {


            DateTime DateCreated = DateTime.Now;
            ColoumnParam[] Coloumns = new ColoumnParam[10];
            Coloumns[0] = new ColoumnParam("SubNesourceName", ColoumnType.NVarChar, txtSubNesourceName.Text);
            Coloumns[1] = new ColoumnParam("SubNesourceLink", ColoumnType.NVarChar, txtSubNesourceLink.Text);
            Coloumns[2] = new ColoumnParam("NesourceID", ColoumnType.Int, DDLNesourceID.SelectedValue);
            Coloumns[3] = new ColoumnParam("DateRegister", ColoumnType.DateTime, DateCreated);
            Coloumns[4] = new ColoumnParam("SubResourcesTypeID", ColoumnType.Int, SubResourcesNewType.SelectedValue);
            Coloumns[5] = new ColoumnParam("ChannelImage", ColoumnType.NVarChar, txtChannelImage.Text);
            Coloumns[6] = new ColoumnParam("BacugroundPicture", ColoumnType.NVarChar, txtBacugroundPicture.Text);
            Coloumns[7] = new ColoumnParam("ResourcesNewType", ColoumnType.Bit, DDLResourcesNewType.SelectedValue);
            Coloumns[8] = new ColoumnParam("DetailsConatinTag", ColoumnType.NVarChar, txtDetailsConatinTag.Text);
            Coloumns[9] = new ColoumnParam("ImageTag", ColoumnType.NVarChar, txtImageTag.Text);
            
            if (DBop.cobject.InsertRow("SubNesources", Coloumns))
            {


                Response.Redirect("ManageAccounts.aspx?S=T");



            }
            else
            {
                LiMessage.Text = "<strong>Warning!</strong> " + DBop.cobject.ErrorMessage;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "ShowModalError();", true);
            }


        }

        protected async void Buadd0_Click(object sender, EventArgs e)
        {
            try
            {
               CallService cl = new CallService();
               BackgrundTask twitter = new BackgrundTask();
           Literal1.Text=await    twitter.Startlisten(txtSubNesourceLink.Text,Convert.ToDateTime( DateTime.Now.AddDays(-300)), 1,Convert.ToInt32( SubResourcesNewType.SelectedValue), txtDetailsConatinTag.Text,txtImageTag.Text);
           if (Literal1.Text.Length > 0)
               Buadd.Visible = true;
            }
            catch (Exception ex) { }
        }


    }
     
}