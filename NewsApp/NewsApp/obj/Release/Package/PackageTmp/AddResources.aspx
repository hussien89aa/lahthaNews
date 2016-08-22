<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="AddResources.aspx.cs" Inherits="NewsApp.AddResources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:NewsDBConnectionString %>" DeleteCommand="DELETE FROM [Resources] WHERE [NesourceID] = @NesourceID" InsertCommand="INSERT INTO [Resources] ([ResourcesName], [NesourceDateAdd], [ImageLink]) VALUES (@ResourcesName, @NesourceDateAdd, @ImageLink)" SelectCommand="SELECT * FROM [Resources]" UpdateCommand="UPDATE [Resources] SET [ResourcesName] = @ResourcesName, [NesourceDateAdd] = @NesourceDateAdd, [ImageLink] = @ImageLink WHERE [NesourceID] = @NesourceID">
        <DeleteParameters>
            <asp:Parameter Name="NesourceID" Type="Int32" />
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="ResourcesName" Type="String" />
            <asp:Parameter Name="NesourceDateAdd" Type="DateTime" />
            <asp:Parameter Name="ImageLink" Type="String" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="ResourcesName" Type="String" />
            <asp:Parameter Name="NesourceDateAdd" Type="DateTime" />
            <asp:Parameter Name="ImageLink" Type="String" />
            <asp:Parameter Name="NesourceID" Type="Int32" />
        </UpdateParameters>
    </asp:SqlDataSource>
    <asp:FormView ID="FormView1" runat="server" AllowPaging="True" DataKeyNames="NesourceID" DataSourceID="SqlDataSource1">
        <EditItemTemplate>
            NesourceID:
            <asp:Label ID="NesourceIDLabel1" runat="server" Text='<%# Eval("NesourceID") %>' />
            <br />
            ResourcesName:
            <asp:TextBox ID="ResourcesNameTextBox" runat="server" Text='<%# Bind("ResourcesName") %>' />
            <br />
            NesourceDateAdd:
            <asp:TextBox ID="NesourceDateAddTextBox" runat="server" Text='<%# Bind("NesourceDateAdd") %>' />
            <br />
            ImageLink:
            <asp:TextBox ID="ImageLinkTextBox" runat="server" Text='<%# Bind("ImageLink") %>' />
            <br />
            <asp:LinkButton ID="UpdateButton" runat="server" CausesValidation="True" CommandName="Update" Text="Update" />
            &nbsp;<asp:LinkButton ID="UpdateCancelButton" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel" />
        </EditItemTemplate>
        <InsertItemTemplate>
            ResourcesName:
            <asp:TextBox ID="ResourcesNameTextBox" runat="server" Text='<%# Bind("ResourcesName") %>' />
            <br />
            NesourceDateAdd:
            <asp:TextBox ID="NesourceDateAddTextBox" runat="server" Text='<%# Bind("NesourceDateAdd") %>' />
            <br />
            ImageLink:
            <asp:TextBox ID="ImageLinkTextBox" runat="server" Text='<%# Bind("ImageLink") %>' />
            <br />
            <asp:LinkButton ID="InsertButton" runat="server" CausesValidation="True" CommandName="Insert" Text="Insert" />
            &nbsp;<asp:LinkButton ID="InsertCancelButton" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel" />
        </InsertItemTemplate>
        <ItemTemplate>
            NesourceID:
            <asp:Label ID="NesourceIDLabel" runat="server" Text='<%# Eval("NesourceID") %>' />
            <br />
            ResourcesName:
            <asp:Label ID="ResourcesNameLabel" runat="server" Text='<%# Bind("ResourcesName") %>' />
            <br />
            NesourceDateAdd:
            <asp:Label ID="NesourceDateAddLabel" runat="server" Text='<%# Bind("NesourceDateAdd") %>' />
            <br />
            ImageLink:
            <asp:Label ID="ImageLinkLabel" runat="server" Text='<%# Bind("ImageLink") %>' />
            <br />
            <asp:LinkButton ID="EditButton" runat="server" CausesValidation="False" CommandName="Edit" Text="Edit" />
            &nbsp;<asp:LinkButton ID="DeleteButton" runat="server" CausesValidation="False" CommandName="Delete" Text="Delete" />
            &nbsp;<asp:LinkButton ID="NewButton" runat="server" CausesValidation="False" CommandName="New" Text="New" />
        </ItemTemplate>
    </asp:FormView>
</asp:Content>
