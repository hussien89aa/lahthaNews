<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm2.aspx.cs" Inherits="NewsApp.WebForm2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div runat="server"  class="ShareClass" style="text-align:center" visible='<%# Eval("SubNesourceName").ToString().Length>0?  true:false   %>'> 
                                  
  
 
     <a href= '<%# Eval("SubNesourceName").ToString().Length>0?  "http://www.facebook.com/sharer.php?s=100&p[url]=http://newsa.azurewebsites.net/NewsDetails.aspx?NID="+ Eval("NewsID") +"&id=" + Request.QueryString["id"]+ "&title="+Eval("NewsTitle")   :Eval("InvestmentLink")   %>'>   <img src="Images/StylesImage/facebook.png" class="ImageSharem"   /> </a> 
     <a href= '<%# Eval("SubNesourceName").ToString().Length>0?  "https://twitter.com/share?url=http://newsa.azurewebsites.net/NewsDetails.aspx?NID="+ Eval("NewsID") +"&id=" + Request.QueryString["id"]+ "&text="+Eval("NewsTitle") +  "&via=تطبيق عاجل &hashtags=اخبار" :Eval("InvestmentLink")   %>'>   <img src="Images/StylesImage/twitter.png" class="ImageSharem"  /> </a> 
     <a href= '<%# Eval("SubNesourceName").ToString().Length>0?  "https://plus.google.com/share?url=http://newsa.azurewebsites.net/NewsDetails.aspx?NID="+ Eval("NewsID") +"&id=" + Request.QueryString["id"]+ "&text="+Eval("NewsTitle") +  "&via=تطبيق عاجل &hashtags=اخبار" :Eval("InvestmentLink")   %>'>   <img src="Images/StylesImage/googleplus.png" class="ImageSharem"  /> </a> 
    <a href= '<%# Eval("SubNesourceName").ToString().Length>0?  "https://www.linkedin.com/shareArticle?mini=true&url=http://newsa.azurewebsites.net/NewsDetails.aspx?NID="+ Eval("NewsID") +"&id=" + Request.QueryString["id"]+ "&title="+Eval("NewsTitle") +  "&source=تطبيق عاجل  " :Eval("InvestmentLink")   %>'>   <img src="Images/StylesImage/linkedin.png" class="ImageSharem"    /> </a> 
     <a href="#" >   <img src="Images/StylesImage/whatsapp.png" class="ImageSharem" onclick='<%# "showAndroidDialog(" +Eval("NewsID") +  ");"  %>'      /> </a> 
   
 
</div>
    </form>
</body>
</html>
