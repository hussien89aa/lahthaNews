using NewsApp.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NewsApp
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["adminID"] == null) // tem stoped
                Response.Redirect("AdminLogin.aspx");



             if (!Page.IsPostBack)
             {

                 ///Images/StylesImage/business_user.png
                 ///Images/AdminImage/20%&6292015124826.jpg
                 DataTable TBUse = new DataTable("tblUserSystemViewGrid");
                     //  add tabel coloumns
                     DataColumn keyField1 = new DataColumn("ViewUserID", typeof(int));
                     TBUse.Columns.Add(keyField1);
                     DataColumn keyField2 = new DataColumn("NameView", typeof(string));
                     TBUse.Columns.Add(keyField2);
                     DataColumn keyField3 = new DataColumn("ImageView", typeof(string));
                     TBUse.Columns.Add(keyField3);
                     DataColumn keyField4 = new DataColumn("LinkPageView", typeof(string));
                     TBUse.Columns.Add(keyField4);
                     DataColumn keyField5 = new DataColumn("ViewUserLevel", typeof(string));
                     TBUse.Columns.Add(keyField5);


                     //  add tabel contain
                     DataRow oneRow;
                     // 1
                     oneRow = TBUse.NewRow();

                     oneRow["ViewUserID"] = 1;
                     oneRow["NameView"] = "اضافة قنوات";
                     oneRow["ImageView"] = "~/Images/StylesImage/application_edit.png";
                     oneRow["LinkPageView"] = "ManageAccounts.aspx";
                     oneRow["ViewUserLevel"] = "1,2,3 ";
                     TBUse.Rows.Add(oneRow);

                     oneRow = TBUse.NewRow();
                     oneRow["ViewUserID"] = 2;
                     oneRow["NameView"] = "اضافة اعلانات";
                     oneRow["ImageView"] = "~/Images/StylesImage/business_user_edit.png";
                     oneRow["LinkPageView"] = "AddAdvestments.aspx";
                     oneRow["ViewUserLevel"] = "1,2,3"; ;
                     TBUse.Rows.Add(oneRow);



                     // 2
                     oneRow = TBUse.NewRow();
                     oneRow["ViewUserID"] = 4;
                     oneRow["NameView"] = "اقسام";
                     oneRow["ImageView"] = "~/Images/StylesImage/business_user_edit.png";
                     oneRow["LinkPageView"] = "AddResources.aspx"; // "NewActivity.aspx?ID=C5AC851D-91C1-4A48-858A-2FAE20738FDC"; //
                     oneRow["ViewUserLevel"] = "1,2,3";
                     TBUse.Rows.Add(oneRow);
                     //// 2
                     //// 2
                     oneRow = TBUse.NewRow();
                     oneRow["ViewUserID"] = 5;
                     oneRow["NameView"] = "ادارة القنوات";
                     oneRow["ImageView"] = "~/Images/StylesImage/business_user_edit.png";
                     oneRow["LinkPageView"] = "ManageChannels.aspx";  //"~/NewActivity.aspx?ID=C5AC851D-91C1-4A48-858A-2FAE20738FDC"
                     oneRow["ViewUserLevel"] = "1,2,3";
                     TBUse.Rows.Add(oneRow);
                     //DeleteData.aspx
                     oneRow = TBUse.NewRow();
                     oneRow["ViewUserID"] = 5;
                     oneRow["NameView"] = "ادارة الاخبار  ";
                     oneRow["ImageView"] = "~/Images/StylesImage/business_user_edit.png";
                     oneRow["LinkPageView"] = "DeleteData.aspx";  //"~/NewActivity.aspx?ID=C5AC851D-91C1-4A48-858A-2FAE20738FDC"
                     oneRow["ViewUserLevel"] = "1,2,3";
                     TBUse.Rows.Add(oneRow);
                     try
                     {
                         DataRow[] matchingRows = TBUse.Select(" ViewUserLevel LIKE '%'", " ViewUserID");

                         DataTable FoundRow = TBUse.Clone();

                         foreach (DataRow dtRow in matchingRows)
                         {
                             FoundRow.ImportRow(dtRow);
                             // DataGridView1.DataSource = FoundRows
                         }
                         DataList1.DataSource = FoundRow;
                         DataList1.DataBind();
                     }
                     catch (Exception ex)
                     {
                         //  MsgBox(ex.Message);
                     }
                 }
             }
         
    }
}