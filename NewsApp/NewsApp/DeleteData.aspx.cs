using NewsApp.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NewsApp
{
    public partial class DeleteData : System.Web.UI.Page
    {
        DBConnection DBop = new DBConnection();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (DBop.cobject.DeletedRow("news","datein< '"+ DateTime.Now.AddDays(-2).ToString()  +"'")) 
            {

                Button1.Text = "تم الحذف";
              

            }
            else
            {
                Button1.Text = "<strong>Warning!</strong> " + DBop.cobject.ErrorMessage;
              }

        }
    }
}