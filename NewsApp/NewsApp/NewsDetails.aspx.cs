using DBManager;
using NewsApp.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NewsApp
{
    public partial class NewsDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //if (!Request.QueryString["NID"]==null )
                    SqlDataSource1.SelectParameters["NewsID"].DefaultValue = Request.QueryString["NID"];

                    if ( !( Request.QueryString["id"] == null))
                    {
                        DBConnection DBop = new DBConnection();
                        ColoumnParam[] Coloumns = new ColoumnParam[3];
                        Coloumns[0] = new ColoumnParam("UserID", ColoumnType.Int, Request.QueryString["id"]);
                        Coloumns[1] = new ColoumnParam("NewsID", ColoumnType.Int, Request.QueryString["NID"]);
                        Coloumns[2] = new ColoumnParam("DateRead", ColoumnType.DateTime, DateTime.Now.ToString());
                        DBop.cobject.InsertRow("Readers", Coloumns);
                    }
            }
        }
    }
}