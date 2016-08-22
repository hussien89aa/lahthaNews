# Lahtha News
This app helps you to know all the news about the Arabic region from you lovely channels that you would like to follow.
 

Folder content
1-LathaAndroid: it is android App for the news system. in GlobalClass.java update the server url to your server url
2-NewApp :it is web portal for the news system. TO make it work change the connection string in web.config to your own connection string and login  to the system with (username=admin, password=admin) 
3- WSNews: it is windows services that bring news from different server. To make it work go to DBConnection.cs and change the connection string to your own connection string and login with. Also in twitter.cs add your twitter Secret and key value. make sure to put this service on VM running all time in cloud when your app be life

4- script_news.sql : it the database script run it on Sql server to build your database
5- script_data.sql:contain app data . Run it on your database.
Next:
add news channel to web portal, and use HtmlAgilityPack library tags to get news details from the news channel web page  or rss


To understand  this code you need to know
1- C# basic to OOP
2- Windows services in C#
3- C# web services in C#
4- ASP.NET in C#
5- SQL server
6- Java
7- Android

I would suggest to study this tutorials  here
https://www.youtube.com/user/alxs1aa

