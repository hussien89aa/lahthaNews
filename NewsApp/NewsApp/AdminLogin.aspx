<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminLogin.aspx.cs" Inherits="NewsApp.AdminLogin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login page</title>
            <style type="text/css">
        .auto-style7 {
        }

        </style>
<meta charset="utf-8">
  <meta name="viewport" content="width=device-width, initial-scale=1">
  <link rel="stylesheet" href="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/css/bootstrap.min.css">
  <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
  <script src="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/js/bootstrap.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">

  
   <div class="container"    >
 
       <br />
  <div class="row">
       <div class="col-md-3">
           </div>
    <div class="col-md-3">
  
                 
                     <div class="panel panel-default" style="Width:499px">
      <div class="panel-heading">تسجيل دخول الموظفين</div>
      <div class="panel-body">
                    <div id="LoginAdmin">
                        <table class="auto-style6">
                            <tr>
                                <td class="auto-style7">
                               
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="auto-style7" colspan="2">
                                       <div class="alert alert-danger" id="theDiv" runat="server">
                                       
   
                                              <asp:Literal ID="LiMessage" Text="<strong>Warning!</strong> Indicates a warning that might need attention." runat="server"></asp:Literal>
  </div>
                                  
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style7">
                                    <asp:Label ID="LaStudentName0" runat="server" CssClass="bold" Font-Size="18pt" meta:resourcekey="LaStudentNameResource1" Text="البريد الألكتروني"></asp:Label>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="auto-style7">
                                    <asp:TextBox ID="txtUserName" runat="server" placeholder="Enter Your Email"  class="form-control input-lg" MaxLength="50" meta:resourcekey="txtStudentNameResource1" Width="286px"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtUserName" ErrorMessage="User name is required field" ForeColor="#FF0066" meta:resourcekey="RequiredFieldValidator1Resource1"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style7">
                                    <asp:Label ID="LaStudentName1" runat="server" CssClass="bold" Font-Size="18pt" meta:resourcekey="LaStudentNameResource1" Text="كلمة المرور"></asp:Label>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="auto-style7">
                                    <asp:TextBox ID="txtPassword" runat="server" class="form-control input-lg" placeholder="Enter your password"   MaxLength="50" meta:resourcekey="txtStudentNameResource1" TextMode="Password" Width="286px"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"   ControlToValidate="txtPassword" ErrorMessage="password is required field" ForeColor="#FF0066" meta:resourcekey="RequiredFieldValidator1Resource1"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align:right ">
                                    &nbsp;</td>
                                <td></td>
                            </tr>
                            <tr>
                                <td class="auto-style7">
                                    <asp:Button ID="BuAddData" runat="server" class="btn btn-success"     Height="39px" ToolTip="Click to login"     Text="دخول" Width="111px" OnClick="BuAddData_Click1" />
                                
                                </td>
                                <td>
 
                                 

                                </td>
                            </tr>
                        </table>
                    </div>
               </div>
    </div>
        <asp:HiddenField ID="HFMAC" runat="server" />
        </div>
           </div>
             </div>
    </form>
</body>
</html>
