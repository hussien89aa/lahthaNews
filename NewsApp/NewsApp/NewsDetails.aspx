<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewsDetails.aspx.cs" Inherits="NewsApp.NewsDetails" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>details</title>
    <%-- add android display code --%>
     
     
    <!-- end copy body -->
   
    
    <%--  android display done --%>
    <link href="Style/StyleSheet1.css" rel="stylesheet" />
    <meta charset="utf-8">

  <meta name="viewport" content="width=device-width, initial-scale=1">
  <link rel="stylesheet" href="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/css/bootstrap.min.css">
  <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
  <script src="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/js/bootstrap.min.js"></script>
       <script type="text/javascript">

           function showAndroidDialog(message) {

               Android.showDialog(message);
           }

</script>
</head>
<body>
       <form id="form1" runat="server" style="text-align:center"/>
        
    
  <div class="container1">
         <div dir="rtl"  style="background:#fff;text-align:center" >
       <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:NewsDBConnectionString %>" SelectCommand="GetNewsDetials" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:QueryStringParameter Name="NewsID" QueryStringField="NID" Type="Int32" />
            </SelectParameters>
        </asp:SqlDataSource>
     <div class="DetailsConteiner">
        <asp:DataList ID="DataList1" runat="server" DataKeyField="NewsID" DataSourceID="SqlDataSource1" Width="100%">
        <ItemTemplate>
           
              <a  href='<%#  "NewsDetails.aspx?NID="+ Eval("NewsID") +"&id=" + Request.QueryString["id"] %>'> 
            <div class="ItemNews"  >
              <div class="ItemNewsData"  >
            <table style="text-align:right;width:100%"  >
                <tr>
                    <td style="width:50px">
                        <a href='<%#  "ChannelNews.aspx?NID="+ Eval("SubNesourceID") + "&id="+ Request.QueryString["id"] %>'> 
                        <img class="HeaderPiv" alt="Mountain View" src='<%# Eval("ChannelImage") %>'></img>
                            </a></td>
                    
                    <td >
                           <a href='<%#  "ChannelNews.aspx?NID="+ Eval("SubNesourceID") + "&id="+ Request.QueryString["id"]%>'> 
                        <asp:Label ID="NewsTitleLabel0" runat="server" Font-Bold="True" Font-Size="12pt" Text='<%# Eval("SubNesourceName") %>' />
                    </a>
                               </td>
                    <td style="text-align:left">
                        <asp:Label ID="NewsTitleLabel1" runat="server"  ForeColor="Black" Font-Size="10pt" Text='<%# Eval("NewsDateN") %>' />
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                         
                             
                         <asp:Label ID="NewsTitleLabel" runat="server" Text='<%# Eval("NewsTitle") %>' ForeColor="Black" Font-Bold="True" Font-Size="16pt" />
                                </td>
                </tr>

                <tr>
                    <td colspan="3">
                     
                        
                        
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
                            <%-- show only when we have viedo --%>
                             <div runat="server"   visible='<%# ((Eval("PicturLink").ToString().IndexOf("http://www.youtube")>=0) )?  true :false  %>'>  
                           
                            <iframe class="img-rounded" height="345"  width="90%" src='<%# Eval("PicturLink") %>'></iframe>
                        &nbsp;</div>
                            </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:Literal ID="Literal1" runat="server"     Text='<%# "<h4>"+  Eval("NewsDetails").ToString() +"</h4>"   %>'></asp:Literal>
                     </td>
                </tr>
<tr >
                    <td  colspan="3" style="text-align:center"> 
                        <br />
                        <div runat="server"  class="ShareClass" style="text-align:center" visible='<%# Eval("SubNesourceName").ToString().Length>0?  true:false   %>'> 
                                  
  <div class="dropdown"     >
               <a   dir="rtl">   مشاركة <img src="Images/StylesImage/share1.png" class="ImageSharem" onclick='<%#   String.Format("javascript:return showAndroidDialog(\"{0}\")", Eval("NewsTitle").ToString()+ "    http://newsa.azurewebsites.net/NewsDetails.aspx?NID="+ Eval("NewsID").ToString())  %>'    />
        
               </a>                    
  

         
</div>
 
     <%--<a href= '<%# Eval("SubNesourceName").ToString().Length>0?  "http://www.facebook.com/sharer.php?s=100&p[url]=http://newsa.azurewebsites.net/NewsDetails.aspx?NID="+ Eval("NewsID") +"&id=" + Request.QueryString["id"]+ "&title="+Eval("NewsTitle")   :Eval("InvestmentLink")   %>'>   <img src="Images/StylesImage/facebook.png" class="ImageShare"   /> </a> 
     <a href= '<%# Eval("SubNesourceName").ToString().Length>0?  "https://twitter.com/share?url=http://newsa.azurewebsites.net/NewsDetails.aspx?NID="+ Eval("NewsID") +"&id=" + Request.QueryString["id"]+ "&text="+Eval("NewsTitle") +  "&via=تطبيق عاجل &hashtags=اخبار" :Eval("InvestmentLink")   %>'>   <img src="Images/StylesImage/twitter.png" class="ImageShare"  /> </a> 
     <a href= '<%# Eval("SubNesourceName").ToString().Length>0?  "https://plus.google.com/share?url=http://newsa.azurewebsites.net/NewsDetails.aspx?NID="+ Eval("NewsID") +"&id=" + Request.QueryString["id"]+ "&text="+Eval("NewsTitle") +  "&via=تطبيق عاجل &hashtags=اخبار" :Eval("InvestmentLink")   %>'>   <img src="Images/StylesImage/googleplus.png" class="ImageShare"   /> </a> 
    <a href= '<%# Eval("SubNesourceName").ToString().Length>0?  "https://www.linkedin.com/shareArticle?mini=true&url=http://newsa.azurewebsites.net/NewsDetails.aspx?NID="+ Eval("NewsID") +"&id=" + Request.QueryString["id"]+ "&title="+Eval("NewsTitle") +  "&source=تطبيق عاجل  " :Eval("InvestmentLink")   %>'>   <img src="Images/StylesImage/linkedin.png" class="ImageShare"    /> </a> 
   
     <a href= '#'>   <img src="Images/StylesImage/whatsapp.png" class="ImageShare"  onclick='<%# "showAndroidDialog(" +Eval("NewsID") +  ");"  %>'  /> </a> 

 <a href= '<%# Eval("SubNesourceName").ToString().Length>0?  "mailto:?Subject=Simple Share Buttons&amp;Body= "+Eval("NewsTitle") +  "http://newsa.azurewebsites.net/NewsDetails.aspx?NID="+ Eval("NewsID") +"&id=" + Request.QueryString["id"]    :Eval("InvestmentLink")   %>'>   <img src="Images/StylesImage/email.png" class="ImageShare"  /> </a> 

   
 --%>
</div>
                        </div>
<br />
                        <br />

                         <div runat="server" class="spaceUnder" visible='<%# Eval("ReadFromWebsiteLink").ToString().Length>0?  true:false   %>'> 
                         <a href  ='<%# Eval("ReadFromWebsiteLink").ToString().IndexOf("http://")==-1? "http://" + Eval("ReadFromWebsiteLink"):Eval("ReadFromWebsiteLink") %>' >  
                        <button type="button" class="btn btn-successd">قراءة الخبر من الموقع الرسمي</button>
                        </a>
                             </div>
                         <br />
                    </td>
                </tr>
            </table>
              </div>
             </div>
       
                </div>
        </ItemTemplate>
    </asp:DataList>
         
    </div>
         </div>
        </div>
        <%-- addsense --%>
                            <div style="height:100px;max-width:100%">
            
                            </div>
    </form>
      
</body>
</html>
