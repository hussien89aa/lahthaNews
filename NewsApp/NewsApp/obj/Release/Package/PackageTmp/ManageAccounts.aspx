<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" Async="true" AutoEventWireup="true" CodeBehind="ManageAccounts.aspx.cs" Inherits="NewsApp.WebForm1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  
   
  
    <style type="text/css">
        .auto-style4 {
            width: 234px;
        }
        .auto-style5 {
            width: 234px;
            font-weight: bold;
        }
    </style>
  
   
  
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h3> أضافة قناة جديدة 
        </h3><h3>
        <table class="nav-justified">
        <tr>
            <td class="auto-style5">اسم القناة</td>
            <td>
                <asp:TextBox ID="txtSubNesourceName"    runat="server"></asp:TextBox>
                </td>
        </tr>
        <tr>
            <td class="auto-style5">&nbsp;</td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtSubNesourceName" ErrorMessage="مطلوب" ForeColor="Red"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="auto-style5">رابط القناة|معرفها</td>
            <td>
                <asp:TextBox ID="txtSubNesourceLink" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="auto-style5">&nbsp;</td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtSubNesourceLink" ErrorMessage="مطلوب" ForeColor="Red"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="auto-style5">lمسار صورة القناة</td>
            <td>
                <asp:TextBox ID="txtChannelImage" runat="server"></asp:TextBox>
                </td>
        </tr>
        <tr>
            <td class="auto-style5">&nbsp;</td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtChannelImage" ErrorMessage="مطلوب" ForeColor="Red"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="auto-style4">
                <strong>مسار الصورة الخلفية</strong></td>
            <td>
                <asp:TextBox ID="txtBacugroundPicture" runat="server"></asp:TextBox>
                </td>
        </tr>
        <tr>
            <td class="auto-style4">
                <strong>تاك الخبر</strong></td>
            <td>
                <asp:TextBox ID="txtDetailsConatinTag" placeholder="//div[@id='innerbody']" style="direction:ltr;" runat="server"></asp:TextBox>
                </td>
        </tr>
        <tr>
            <td class="auto-style4">
                <strong>تاك الصورة</strong></td>
            <td>
                <asp:TextBox ID="txtImageTag" runat="server" style="direction:ltr;" placeholder="//div[@class='innerbody']"></asp:TextBox>
                </td>
        </tr>
        <tr>
            <td class="auto-style5">&nbsp;</td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtBacugroundPicture" ErrorMessage="مطلوب" ForeColor="Red"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="auto-style5">نوع القناة</td>
            <td>
                <asp:DropDownList ID="DDLNesourceID" runat="server" DataSourceID="SDSNesourceID" DataTextField="ResourcesName" DataValueField="NesourceID" Width="200px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="auto-style5">&nbsp;</td>
            <td>
                <asp:SqlDataSource ID="SDSNesourceID" runat="server"   SelectCommand="SELECT [NesourceID], [ResourcesName] FROM [Resources]" ConnectionString="<%$ ConnectionStrings:NewsDBConnectionString %>">
                </asp:SqlDataSource>
            </td>
        </tr>
        <tr>
            <td class="auto-style5">مصدر البيانات</td>
            <td>
                <asp:DropDownList ID="SubResourcesNewType" runat="server" DataSourceID="SDSSubResourcesTypeID" DataTextField="SubResourcesTypeName" DataValueField="SubResourcesTypeID" Width="200px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="auto-style5">&nbsp;</td>
            <td>
                <asp:SqlDataSource ID="SDSSubResourcesTypeID" runat="server"   SelectCommand="SELECT * FROM [SubResourcesType]" ConnectionString="<%$ ConnectionStrings:NewsDBConnectionString %>"></asp:SqlDataSource>
            </td>
        </tr>
        <tr>
            <td class="auto-style5">نوع الأخبار</td>
            <td>
                <asp:DropDownList ID="DDLResourcesNewType" runat="server" Width="200px"  >
                    <asp:ListItem Value="0">اعتيادي</asp:ListItem>
                    <asp:ListItem Value="1">عاجل</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="auto-style4">&nbsp;</td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="auto-style4">&nbsp;</td>
            <td>
                <div class="btn-group">
                         <asp:Button ID="Buadd0" runat="server" class="btn btn-success" Text="فحص المعلومات" OnClick="Buadd0_Click"   />
           
                <asp:Button ID="Buadd"  runat="server" class="btn btn-success" Text="اضافة" OnClick="Buadd_Click" Visible="False" />
            </div>
                    </td>
        </tr>
        <tr>
            <td class="auto-style4">&nbsp;</td>
            <td>
                <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                    </td>
        </tr>
        </table>
    </h3>
     <h3>
         &nbsp;</h3>
     <meta charset="utf-8">
  <meta name="viewport" content="width=device-width, initial-scale=1">
  <link rel="stylesheet" href="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/css/bootstrap.min.css">
  <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
  <script src="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/js/bootstrap.min.js"></script>
       <!-- Message Lables-->

          <script type="text/javascript">
              function ShowModalSucces() {
                  $('#ModalMessage').modal('show');
                  window.setTimeout(HideModal, 1000);

              }

              function ShowModalError() {

                  $("#DivMessagePanel").addClass('alert alert-danger');
                  $('#ModalMessage').modal('show');
              }

              function HideModal() {
                  $('#ModalMessage').modal('hide');
              }
</script>                                   
<div id="ModalMessage" class="modal fade" role="dialog">
  <div class="modal-dialog">

     
          <div class="alert alert-success"   id="DivMessagePanel">

           <asp:Literal ID="LiMessage" Text="<strong>Success!</strong> Data is added successfully." runat="server"></asp:Literal>
              <a href="#" class="close" data-dismiss="modal" aria-label="close">&times;</a>
</div>
     

  </div>
</div>

</asp:Content>
