<%@ Page Title="" Language="C#"   EnableEventValidation="false"  MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="AddAdvestments.aspx.cs" Inherits="NewsApp.AddAdvestments" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .auto-style1 {
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
     <h3> اضافة اعلانات
         </h3>
         <h3>
          <table class="nav-justified">
             <tr>
                 <td class="auto-style1">
                <b>عنوان&nbsp; الخبر</b></td>
                 <td>
                <asp:TextBox ID="txtInvestmentTitle"    runat="server"></asp:TextBox>
                 </td>
             </tr>
             <tr>
                 <td class="auto-style1">&nbsp;</td>
                 <td>
                     &nbsp;</td>
             </tr>
             <tr>
                 <td class="auto-style1">
                <b>تحميل صورة</b></td>
                 <td>
                <asp:FileUpload ID="FileUpload1" runat="server" />
                 </td>
             </tr>
             <tr>
                 <td class="auto-style1">&nbsp;</td>
                 <td>
                     &nbsp;</td>
             </tr>
             <tr>
                 <td class="auto-style1">
                <b>رابط االأعلان</b></td>
                 <td>
                <asp:TextBox ID="txtInvestmentLink" runat="server"></asp:TextBox>
                 </td>
             </tr>
             <tr>
                 <td class="auto-style1">
                     &nbsp;</td>
                 <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup="Add" runat="server" ControlToValidate="txtInvestmentLink" ErrorMessage="مطلوب" ForeColor="Red"></asp:RequiredFieldValidator>
                 </td>
             </tr>
             <tr>
                 <td class="auto-style1"><strong>موقع الأعلان</strong></td>
                 <td>
                <asp:TextBox ID="txtInverstmentPostion" runat="server" Width="50px"></asp:TextBox>
                 </td>
             </tr>
             <tr>
                 <td class="auto-style1">&nbsp;</td>
                 <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ValidationGroup="Add" runat="server" ControlToValidate="txtInverstmentPostion" ErrorMessage="مطلوب" ForeColor="Red"></asp:RequiredFieldValidator>
                     <br />
                     <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ValidationGroup="Add" runat="server" ErrorMessage="فقط ارقام" ForeColor="#FF0066" ControlToValidate="txtInverstmentPostion" ValidationExpression="^[0-9]*$" ></asp:RegularExpressionValidator>
                 </td>
             </tr>
             <tr>
                 <td class="auto-style1">&nbsp;</td>
                 <td>
                <asp:Button ID="Buadd" runat="server" class="btn btn-success" ValidationGroup="Add" Text="اضافة" OnClick="Buadd_Click" />
                 </td>
             </tr>
             <tr>
                 <td class="auto-style1">&nbsp;</td>
                 <td>&nbsp;</td>
             </tr>
             <tr>
                 <td class="auto-style1" colspan="2">
                     <div style="overflow:hidden;overflow-x:scroll "> 
                     <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="InvestmentID" DataSourceID="SqlDataSource1" AllowPaging="True">
                         <Columns>
                              <asp:TemplateField ShowHeader="False">
                                  <EditItemTemplate>
                                      <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Update" Text="تحديث"></asp:LinkButton>
                                      &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel" Text="الغاء"></asp:LinkButton>
                                  </EditItemTemplate>
                                  <ItemTemplate>
                                      <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Edit" Text="تعديل"></asp:LinkButton>
                                      &nbsp;<asp:LinkButton ID="LinkButton3" runat="server" CausesValidation="False" CommandName="Delete" Text="حذف"></asp:LinkButton>
                                  </ItemTemplate>
                              </asp:TemplateField>
                              <asp:BoundField HeaderText="العنوان" DataField="InvestmentTitle" SortExpression="InvestmentTitle" />
                              <asp:TemplateField HeaderText="رابط الأعلان" SortExpression="InvestmentLink">
                                  <EditItemTemplate>
                                      <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("InvestmentLink") %>'></asp:TextBox>
                                  </EditItemTemplate>
                                  <ItemTemplate>
                                      <asp:LinkButton ID="LinkButton1" runat="server" PostBackUrl='<%# Eval("InvestmentLink") %>'>الرابط</asp:LinkButton>
                                  </ItemTemplate>
                              </asp:TemplateField>
                              <asp:TemplateField HeaderText="تاريخ الأضافة" SortExpression="InvestmentDate">
                                  
                                  <ItemTemplate>
                                      <asp:Label ID="Label1" runat="server" Text='<%# Bind("InvestmentDate") %>'></asp:Label>
                                  </ItemTemplate>
                              </asp:TemplateField>
                             
                                 <asp:TemplateField HeaderText="موقع الأعلان" SortExpression="InverstmentPostion">
                                     <EditItemTemplate>
                                          <asp:TextBox ID="CheckBox1" Width="50px" runat="server" text='<%# Bind("InverstmentPostion") %>'  />
                                     </EditItemTemplate>
                                     <ItemTemplate>
                                              <asp:Label ID="Label2" runat="server" text='<%# Bind("InverstmentPostion") %>' />
                                    
                                     </ItemTemplate>
                              </asp:TemplateField>
                        
                             <asp:CheckBoxField HeaderText="الأعلان فعال" DataField="IsActive" SortExpression="IsActive" />
                         </Columns>
                     </asp:GridView>
                         </div>
                 </td>
             </tr>
             <tr>
                 <td class="auto-style1">&nbsp;</td>
                 <td>
                     <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:NewsDBConnectionString %>" DeleteCommand="DELETE FROM [Investments] WHERE [InvestmentID] = @InvestmentID" InsertCommand="INSERT INTO [Investments] ([InvestmentTitle], [InvestmentImage], [InvestmentLink], [InvestmentDate], [IsActive]) VALUES (@InvestmentTitle, @InvestmentImage, @InvestmentLink, @InvestmentDate, @IsActive)" SelectCommand="SELECT * FROM [Investments] ORDER BY [IsActive] DESC" UpdateCommand="UPDATE dbo.Investments SET InvestmentTitle = @InvestmentTitle, InvestmentLink = @InvestmentLink, IsActive = @IsActive, InverstmentPostion=@InverstmentPostion WHERE (InvestmentID = @InvestmentID)">
                         <DeleteParameters>
                             <asp:Parameter Name="InvestmentID" Type="Int32" />
                         </DeleteParameters>
                         <InsertParameters>
                             <asp:Parameter Name="InvestmentTitle" Type="String" />
                             <asp:Parameter Name="InvestmentImage" Type="String" />
                             <asp:Parameter Name="InvestmentLink" Type="String" />
                             <asp:Parameter Name="InvestmentDate" Type="DateTime" />
                             <asp:Parameter Name="IsActive" Type="Boolean" />
                         </InsertParameters>
                         <UpdateParameters>
                             <asp:Parameter Name="InvestmentTitle" Type="String" />
                             <asp:Parameter Name="InvestmentLink" Type="String" />
                             <asp:Parameter Name="IsActive" Type="Boolean" />
                             <asp:Parameter Name="InvestmentID" Type="Int32" />
                             <asp:Parameter Name="InverstmentPostion" />
                         </UpdateParameters>
                     </asp:SqlDataSource>
                 </td>
             </tr>
         </table>
        </h3>
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
        </div>
</asp:Content>
