<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="ManageChannels.aspx.cs" Inherits="NewsApp.ManageChannels" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="overflow:hidden; overflow-x:scroll"> 
    <table class="auto-style1">
        <tr>
            <td>&nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>ا<strong>لقسم</strong></td>
            <td>
                <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource1" DataTextField="ResourcesName" DataValueField="NesourceID">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:NewsDBConnectionString %>" SelectCommand="SELECT [NesourceID], [ResourcesName] FROM [Resources]"></asp:SqlDataSource>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>
                
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="SubNesourceID" DataSourceID="SqlDataSource2" AllowPaging="True" BackColor="White" BorderColor="#CC9966" BorderStyle="None" BorderWidth="1px" CellPadding="4" PageSize="5">
                    <Columns>
                        <asp:CommandField ShowEditButton="True" />
                        <asp:TemplateField HeaderText="اسم القناة" SortExpression="SubNesourceName">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("SubNesourceName") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("SubNesourceName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="نوع المصدر" SortExpression="NesourceID">
                            <EditItemTemplate>
                                <asp:DropDownList ID="DropDownList2" runat="server" DataSourceID="SqlDataSource3" DataTextField="ResourcesName" DataValueField="NesourceID" SelectedValue='<%# Bind("NesourceID") %>'>
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:NewsDBConnectionString %>" SelectCommand="SELECT [NesourceID], [ResourcesName] FROM [Resources]"></asp:SqlDataSource>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# DropDownList1.SelectedItem.Text %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="رابط القناة"  SortExpression="SubNesourceLink">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("SubNesourceLink") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label3"  runat="server" Text='<%# Bind("SubNesourceLink") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="صورة القناة" SortExpression="ChannelImage">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox5" runat="server" Text='<%# Bind("ChannelImage") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label5" runat="server" Text='<%# Bind("ChannelImage") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="الحالة" SortExpression="IsActive">
                            <EditItemTemplate>
                                <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("IsActive") %>' />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("IsActive") %>' Enabled="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="الصورة الخلفية" SortExpression="BacugroundPicture">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("BacugroundPicture") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label4" runat="server" Text='<%# Bind("BacugroundPicture") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="تاك الخبر" SortExpression="DetailsConatinTag">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox7" runat="server" Text='<%# Bind("DetailsConatinTag") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label7" runat="server" Text='<%# Bind("DetailsConatinTag") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="تاك الصورة" SortExpression="ImageTag">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox6" runat="server" Text='<%# Bind("ImageTag") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label6" runat="server" Text='<%# Bind("ImageTag") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <FooterStyle BackColor="#FFFFCC" ForeColor="#330099" />
                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="#FFFFCC" />
                    <PagerStyle BackColor="#FFFFCC" ForeColor="#330099" HorizontalAlign="Center" />
                    <RowStyle BackColor="White" ForeColor="#330099" />
                    <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="#663399" />
                    <SortedAscendingCellStyle BackColor="#FEFCEB" />
                    <SortedAscendingHeaderStyle BackColor="#AF0101" />
                    <SortedDescendingCellStyle BackColor="#F6F0C0" />
                    <SortedDescendingHeaderStyle BackColor="#7E0000" />
                </asp:GridView>
                   
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>
                <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:NewsDBConnectionString %>" DeleteCommand="DELETE FROM [SubNesources] WHERE [SubNesourceID] = @SubNesourceID" InsertCommand="INSERT INTO [SubNesources] ([SubNesourceName], [NesourceID], [SubNesourceLink], [ChannelImage], [IsActive], [SubResourcesTypeID], [BacugroundPicture], [DetailsConatinTag], [ImageTag]) VALUES (@SubNesourceName, @NesourceID, @SubNesourceLink, @ChannelImage, @IsActive, @SubResourcesTypeID, @BacugroundPicture, @DetailsConatinTag, @ImageTag)" SelectCommand="SELECT SubNesourceID, SubNesourceName, NesourceID, SubNesourceLink, ChannelImage, IsActive, SubResourcesTypeID, BacugroundPicture, DetailsConatinTag, ImageTag FROM dbo.SubNesources WHERE (NesourceID = @NesourceID)" UpdateCommand="UPDATE [SubNesources] SET [SubNesourceName] = @SubNesourceName, [NesourceID] = @NesourceID, [SubNesourceLink] = @SubNesourceLink, [ChannelImage] = @ChannelImage, [IsActive] = @IsActive, [BacugroundPicture] = @BacugroundPicture, [DetailsConatinTag] = @DetailsConatinTag, [ImageTag] = @ImageTag WHERE [SubNesourceID] = @SubNesourceID">
                    <DeleteParameters>
                        <asp:Parameter Name="SubNesourceID" Type="Int32" />
                    </DeleteParameters>
                    <InsertParameters>
                        <asp:Parameter Name="SubNesourceName" Type="String" />
                        <asp:Parameter Name="NesourceID" Type="Int32" />
                        <asp:Parameter Name="SubNesourceLink" Type="String" />
                        <asp:Parameter Name="ChannelImage" Type="String" />
                        <asp:Parameter Name="IsActive" Type="Boolean" />
                        <asp:Parameter Name="SubResourcesTypeID" Type="Int32" />
                        <asp:Parameter Name="BacugroundPicture" Type="String" />
                        <asp:Parameter Name="DetailsConatinTag" Type="String" />
                        <asp:Parameter Name="ImageTag" Type="String" />
                    </InsertParameters>
                    <SelectParameters>
                        <asp:ControlParameter ControlID="DropDownList1" Name="NesourceID" PropertyName="SelectedValue" />
                    </SelectParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="SubNesourceName" Type="String" />
                        <asp:Parameter Name="NesourceID" Type="Int32" />
                        <asp:Parameter Name="SubNesourceLink" Type="String" />
                        <asp:Parameter Name="ChannelImage" Type="String" />
                        <asp:Parameter Name="IsActive" Type="Boolean" />
                        <asp:Parameter Name="BacugroundPicture" Type="String" />
                        <asp:Parameter Name="DetailsConatinTag" Type="String" />
                        <asp:Parameter Name="ImageTag" Type="String" />
                        <asp:Parameter Name="SubNesourceID" Type="Int32" />
                    </UpdateParameters>
                </asp:SqlDataSource>
            </td>
        </tr>
    </table>
        </div>
</asp:Content>
