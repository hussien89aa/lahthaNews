<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="DeleteData.aspx.cs" Inherits="NewsApp.DeleteData" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <asp:Button ID="Button1" runat="server" Text="مسح الاخبار قبل اخر ثلاث ايام" OnClick="Button1_Click" />
</asp:Content>
