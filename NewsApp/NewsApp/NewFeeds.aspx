<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewFeeds.aspx.cs" Inherits="NewsApp.NewFeeds" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="Style/StyleSheet1.css" rel="stylesheet" />
     <meta charset="utf-8">
  <meta name="viewport" content="width=device-width, initial-scale=1">
  <link rel="stylesheet" href="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/css/bootstrap.min.css">
  <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
  <script src="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/js/bootstrap.min.js"></script>
    <script src="Jquery/jquery-1.11.3.min.js"></script>
    
</head>
<body>
    <form id="form1" runat="server" style="text-align:center">
       
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
         <ContentTemplate>

              <script>

                  var myVar = setInterval(function () { myTimer() }, 60000);

                  function myTimer() {
                      if ($(window).scrollTop() < 7) // if zero is in the top
                      {

                          document.getElementById("HFMaxScrol").value = "-1";

                          document.getElementById('BuLoadMore').click(); return false;

                      }
                      else if ($(window).scrollTop() > 40) { // in cuase of new news is comming

                          //call the c# function to update first post
                          // 
                          try {
                              $.ajax({
                                  type: "POST",
                                  url: 'NewFeeds.aspx/NewNewsComming',
                                  data: "{'Tag':'1','UserID':'" + '<%= Request.QueryString["id"] %>' + "','NewsID':'" + '<%=DataList1.Items.Count>0? DataList1.DataKeys[0].ToString(): "0" %>' + "','q':'" + '<%= Request.QueryString["q"] %>' + "'}",
                                  contentType: "application/json; charset=utf-8",
                                  dataType: "json",
                                  success: function (msg) {
                                      // submit.value = msg.data;
                                      if (msg.d == "newNews")
                                          ShowNewNewsDiv();
                                  },
                                  error: function (e) {
                                      //$("#divResult").html("Something Wrong.");
                                  }
                              });
                          } catch (err) { }


             }
     }
     $(document).ready(function () {
         document.getElementById("HeaderMessageLoading").style.display = 'none';
     });


     $(window).scroll(function () {
         //  document.getElementById("HFMaxScrol").value = "0";
         if ($(window).scrollTop() == 0) // if zero is in the top
         { //HeaderMessage
             HideNewNewsDiv(); // hide new news div
             document.getElementById("HeaderMessageLoading").style.display = 'block';

             document.getElementById("HFMaxScrol").value = "2";
             document.getElementById('BuLoadMore').click(); return false;

             document.getElementById("newfeedDiv").Visible = false;

         }
         /// in cause he be in the lat item in the document we load more
         if ($(document).height() <= $(window).scrollTop() + $(window).height()) {
             //  alert("end doc");
             document.getElementById("HFMaxScrol").value = "30";
             document.getElementById('BuLoadMore').click(); return false;
             // alert(document.getElementById("HFMaxScrol").value);
         }

     });
     function ShowNewNewsDiv() {
         $("#newfeedDiv").fadeIn(100);
     }
     function HideNewNewsDiv() {
         $("#newfeedDiv").fadeOut();
     }
     function HideLoading() {
         document.getElementById("HeaderMessageLoading").style.display = 'none';

     }
     function NewNewsempty() {

         $("#HeaderMessage").fadeIn();
         $("#HeaderMessage").fadeOut(6000);
         //document.getElementById("HeaderMessage").style.display = 'block';
     }
</script>
    <script type="text/javascript">

        function showAndroidDialog(message) {
          //  alert(message);
            Android.showDialog(message);
        }

</script>
              <div id="newfeedDiv"  style="width:100%;display:none"     > 
           <div class="container">
        <div  class="navbar navbar-fixed-top "  style=" width:100%;margin-top:2px;"> 
            <div   class="btn btn-default img-circle"  >           
    <a href="#top"  >
        <asp:Label ID="Label1" runat="server" ForeColor="Black"   Font-Bold="true" Text="أخبار جديدة"></asp:Label>  <img src="Images/StylesImage/upirrow.png" width="20" height="12"   />  
    </a>              
                </div>
</div> 
               </div>
           </div>
            <div dir="rtl"  style="background:#d4d3d3;text-align:center" >
     <div style="width:96%; background:#d4d3d3;margin-right:2%;padding-top:5px">
         <div     id="HeaderMessageLoading" style="padding-bottom:3px;text-align:center;  ">
  
                 <img src="Images/loading_spinner.gif" width="50" height="50" />
                 
          </div>
          <div     id="HeaderMessage" style="padding-bottom:3px;text-align:center; height: 40px;display:none">
    <div   class="btn btn-default img-circle"  dir="ltr" >           
    <a href="#top"  >
         <asp:Label ID="Label4" runat="server"   Text="لا يوجد مزيد من الأخبار"></asp:Label> <img src="Images/StylesImage/empty.png" width="20" height="12"   />  
    </a>              
                </div>
                  
             
         </div>
         <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:NewsDBConnectionString %>" SelectCommand="GetNews" SelectCommandType="StoredProcedure">
             <SelectParameters>
                 <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32" />
                 <asp:QueryStringParameter DefaultValue="1" Name="UserID" QueryStringField="id" Type="Int32" />
                 <asp:Parameter DefaultValue="1" Name="StratFrom" Type="Int32" />
                 <asp:Parameter DefaultValue="" Name="EndTo" Type="Int32" />
                 <asp:Parameter DefaultValue="0" Name="SubNesourceID" Type="Int32" />
                 <asp:Parameter DefaultValue="0" Name="NewsID" Type="Int32" />
                 <asp:QueryStringParameter DefaultValue="%" Name="q" QueryStringField="q" Type="String" />
                 <asp:Parameter DefaultValue="0" Name="Type" Type="Int32" />
             </SelectParameters>
         </asp:SqlDataSource>
        <%--  if   datalist is empty --%>
           <asp:Label class="btn btn-default img-circle"     runat="server" ID="lblNoRecordFount" Text="لاتوجد نتائج بحث " Visible="False"></asp:Label>

        <asp:DataList ID="DataList1"  runat="server" DataKeyField="NewsID" DataSourceID="SqlDataSource1" Width="100%">
         <ItemTemplate>
           
            <div class="ItemNews"  >
              <div class="ItemNewsData"  >
            <table style="text-align:right;width:100%"  >
                <tr>
                    <td style="width:30px">  <div runat="server"  class="BodyPiv" visible='<%# Eval("SubNesourceName").ToString().Length>0?  true:false   %>'> 
                          
                        <a href='<%#  "ChannelNews.aspx?NID="+ Eval("SubNesourceID") + "&id="+ Request.QueryString["id"] %>'> 

                        <img class="HeaderPiv" alt="Mountain View" src='<%# Eval("ChannelImage") %>'></img>
                            </a></div>
                        </td>
                    
                    <td >
                           <a href='<%#  "ChannelNews.aspx?NID="+ Eval("SubNesourceID") + "&id="+ Request.QueryString["id"] %>'> 
                        <asp:Label ID="NewsTitleLabel0" runat="server" Font-Bold="True" Font-Size="12pt" Text='<%# Eval("SubNesourceName") %>' />
                    </a>
                               </td>
                    <td style="text-align:left">
                        <asp:Label ID="NewsTitleLabel1" CssClass='<%# Eval("NewsTitle").ToString().IndexOf("عاجل")>=0?  "btn-danger img-rounded DateNewPading":"" %>' runat="server"  ForeColor="Black" Font-Size="10pt" Text='<%# Eval("NewsTitle").ToString().IndexOf("عاجل")>=0? "عاجل":Eval("NewsDateN") %>'     />
                
                            </td>
                </tr>
                <tr>
                    <td colspan="3">
                         
                                <a  href='<%# Eval("SubNesourceName").ToString().Length>0?  "NewsDetails.aspx?NID="+ Eval("NewsID") +"&id=" + Request.QueryString["id"]:Eval("InvestmentLink")   %>'> 
           
                         <asp:Label ID="NewsTitleLabel" runat="server" Text='<%# Eval("NewsTitle") %>' ForeColor="Black" Font-Bold="True" Font-Size="16pt" />
                              </a>
                                      </td>
                </tr>
                <tr>
                    <td colspan="3">
                           <a  href='<%# Eval("SubNesourceName").ToString().Length>0?  "NewsDetails.aspx?NID="+ Eval("NewsID") +"&id=" + Request.QueryString["id"]:Eval("InvestmentLink")   %>'> 
           
                        <div runat="server"  class="BodyPiv" visible='<%# Eval("PicturLink").ToString().Length>0?  true:false   %>'> 
                          <%-- show only when we have image --%>
                            <div runat="server"   visible='<%# ( ((!Eval("PicturLink").ToString().ToLower().EndsWith(".mp4")) &&(Eval("PicturLink").ToString().IndexOf("http://www.youtube")<0)) || (Eval("PicturLink").ToString().ToLower().EndsWith(".jpg") || Eval("PicturLink").ToString().ToLower().EndsWith(".png") || Eval("PicturLink").ToString().ToLower().EndsWith(".gif")))? true:false   %>'>  
                            <img      src ='<%# Eval("PicturLink") %>'   class="img-rounded" alt="Cinque Terre" width="100%"  > 
                            </div>
                            <%-- show only when we have viedo --%>
                             <div runat="server"   visible='<%# Eval("PicturLink").ToString().ToLower().EndsWith(".mp4") ?  true :false  %>'>  
                          <%-- 
                            <iframe class="img-rounded" height="345"  width="100%" src='<%# Eval("PicturLink") %>'></iframe>
                        &nbsp;</div>--%>
                            <video height="300"  width="100%" controls>
  <source  src='<%# Eval("PicturLink") %>' type="video/mp4">
</video>
                            </div>
                            </div>
                               </a>
                    </td>
                </tr>
                <tr>
                    <td colspan="3"> 
                 
                             <div runat="server"    class="ShareClass" visible='<%# Eval("SubNesourceName").ToString().Length>0?  true:false   %>'> 
                
                                 <table class ="ShareClassTable">
                                     <tr>
                                         

  <td class ="ShareClassTableCell">
 
               <img src="Images/StylesImage/share1.png" class="ImageSharem" onclick='<%#   String.Format("javascript:return showAndroidDialog(\"{0}\")", Eval("NewsTitle").ToString()+ "    http://newsa.azurewebsites.net/NewsDetails.aspx?NID="+ Eval("NewsID").ToString())  %>'    />
                         
  

  
                                         </td>
                                         <td class ="ShareClassTableCell">
                      
         <div runat="server"        visible='<%# Eval("readers").ToString().Equals("0")?  false:true   %>'>  
                             
       <a href= '#' title='<%# Eval("readers").ToString()    %>'> 
                                               
                   <img src="Images/StylesImage/eye.png" class="ImageSharem"  />                                       
                    <asp:Label ID="Label3" runat="server" Text='<%# Eval("readers").ToString()%>' ></asp:Label>
                 
            </a>                                              
   </div>    
                                         </td>
                                       
                                     </tr>
                                 </table>          
 
                           
                                                   
                                
                   
                        </div>

                    </td>
                </tr>
            </table>
              </div>
             </div>
       
             
        </ItemTemplate>
          <FooterTemplate>
<asp:Label class="btn btn-default img-circle"  Visible='<%#bool.Parse((DataList1.Items.Count==0).ToString())%>' runat="server" ID="lblNoRecord" Text="لاتوجد نتائج بحث "></asp:Label>
</FooterTemplate>
    </asp:DataList>
         
    </div>
         </div>
             <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                 <ProgressTemplate>
             <div style="text-align:center; height: 74px;">
                 <img src="Images/loading_spinner.gif" width="80" height="80" />
                 
             </div>
                     </ProgressTemplate>
                 </asp:UpdateProgress>
           <%--  <asp:Timer ID="Timer1"  Interval="3000"  runat="server" OnTick="Timer1_Tick" Enabled="False"></asp:Timer>
            --%> 
             <asp:HiddenField ID="HFMaxScrol" runat="server" />
              <asp:HiddenField ID="HFNewsID1" runat="server" />
           
           
             </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="BuLoadMore" EventName="Click" />
                
            </Triggers>
            
        </asp:UpdatePanel>
         
          <div style="text-align:center">
                <br />
                   <br /> 
               <br />
                 
       <%-- <div runat="server" visible="false">--%>
        <asp:Button ID="BuLoadMore"  runat="server" Text="" OnClick="BuLoadMore_Click" Width="1px" Height="1px" />
     </div>
      
                                    
 
                 
 
          
    </form>
</body>
</html>
