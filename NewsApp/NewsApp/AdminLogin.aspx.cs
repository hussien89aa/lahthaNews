using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NewsApp
{
    public partial class AdminLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        private bool IsAdminExists(string userName, string password)
        {
            if ((userName.Equals("admin")) && (password.Equals("admin")))
            {
                Session["adminID"] = "1";
               


                return true;
            }


            return false;
        }
        protected void BuAddData_Click1(object sender, EventArgs e)
        {
            if (IsAdminExists(txtUserName.Text, txtPassword.Text))
            {


                Response.Redirect("AddResources.aspx");
            }
            else
            {
                theDiv.Visible = true;

                LiMessage.Text = "<strong>Warning!</strong> User name or password is not correct";
            }
        }
    }
}