package com.example.hussienalrubaye.webview;


/**
 * Created by hussienalrubaye on 11/12/15.
 */
//http://ennews.azurewebsites.net/MobileWebService1.asmx/GetNewsNow?UserID=5&StratFrom=1&EndTo=2&SubNesourceID=0&NewsID=0&q=%&Type=0
public class NewsTicket {
  public   String Row;
    public  String NewsTitle;
    public  String SubNesourceID;
    public   String PicturLink;
    public   String InvestmentLink;
    public   String NewsDateN;
    public  String NewsID;
    public  String SubNesourceName;
    public  String ChannelImage;
    public  String readers;
    String NewsDetails;
    // for load news data
    NewsTicket( String Row, String NewsTitle,String SubNesourceID,
                String PicturLink, String InvestmentLink,
                String NewsDateN,  String NewsID, String SubNesourceName,
                String ChannelImage,String readers){
       this. Row=Row;
        this. NewsTitle=NewsTitle;
        this. SubNesourceID=SubNesourceID;
        this. PicturLink=PicturLink;
        this. InvestmentLink= InvestmentLink;
        this. NewsDateN=NewsDateN;
        this. NewsID=NewsID;
        this. SubNesourceName=SubNesourceName;
        this. ChannelImage=ChannelImage;
        this. readers= readers;

    }

//for news details
    NewsTicket( String Row, String NewsTitle,String SubNesourceID,
                       String PicturLink, String InvestmentLink,
                       String NewsDateN,  String NewsID, String SubNesourceName,
                       String ChannelImage,String readers,  String NewsDetails){
        this. Row=Row;
        this. NewsTitle=NewsTitle;
        this. SubNesourceID=SubNesourceID;
        this. PicturLink=PicturLink;
        this. InvestmentLink= InvestmentLink;
        this. NewsDateN=NewsDateN;
        this. NewsID=NewsID;
        this. SubNesourceName=SubNesourceName;
        this. ChannelImage=ChannelImage;
        this. readers= readers;
        this.NewsDetails=NewsDetails;
    }


}
