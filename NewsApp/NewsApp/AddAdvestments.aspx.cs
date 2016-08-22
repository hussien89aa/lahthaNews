using DBManager;
using NewsApp.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NewsApp
{
    public partial class AddAdvestments : System.Web.UI.Page
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
            //Session["adminID"] = 5;
        }

        protected void Buadd_Click(object sender, EventArgs e)
        {
            String password = "DF34345";
             string imgPath="";
             try
             {
                 if (FileUpload1.HasFile)
                 {
                     password = StringGeneration.getString(30);
                     imgPath = ("/attachments/"
                                 + (password + Path.GetExtension(FileUpload1.PostedFile.FileName)));
                     //  selct path before post back check nesa becose name file will dispose after poastback
                     //   MsgBox(Server.MapPath(imgPath))
                     FileUpload1.SaveAs(Server.MapPath(imgPath));
                     imgPath = "http://www.news.alruabye.net/" + imgPath;//my host name
                 }
             }
             catch (Exception ex) { imgPath = "~/Images/StylesImage/advestments.png"; }
            DateTime DateCreated = DateTime.Now;
            ColoumnParam[] Coloumns = new ColoumnParam[5];
            Coloumns[0] = new ColoumnParam("InvestmentTitle", ColoumnType.NVarChar, txtInvestmentTitle.Text);
            Coloumns[1] = new ColoumnParam("InvestmentImage", ColoumnType.NVarChar, imgPath);
            Coloumns[2] = new ColoumnParam("InvestmentLink", ColoumnType.NVarChar, txtInvestmentLink.Text);
            Coloumns[3] = new ColoumnParam("InvestmentDate", ColoumnType.DateTime, DateCreated);
            Coloumns[4] = new ColoumnParam("InverstmentPostion", ColoumnType.Int, txtInverstmentPostion.Text );
             
            
            if (DBop.cobject.InsertRow("Investments", Coloumns))
            {


                Response.Redirect("AddAdvestments.aspx?S=T");



            }
            else
            {
                LiMessage.Text = "<strong>Warning!</strong> " + DBop.cobject.ErrorMessage;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "ShowModalError();", true);
            }


        }
    }
}