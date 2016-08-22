package com.example.hussienalrubaye.webview;

/**
 * Created by hussienalrubaye on 11/19/15.
 */
public class NewsTicketDetails {
    public   String Row;
    public  String NewsTitle;
    public  String SubNesourceID;
    public   String PicturLink;
    public   String InvestmentLink;
    public   String NewsDateN;
    public  String NewsID;
    public  String SubNesourceName;
    public  String ChannelImage;
    public  String ReadFromWebsiteLink;
    String NewsDetails;


    //for news details
    NewsTicketDetails( String Row, String NewsTitle,String SubNesourceID,
                String PicturLink, String InvestmentLink,
                String NewsDateN,  String NewsID, String SubNesourceName,
                String ChannelImage,String ReadFromWebsiteLink,  String NewsDetails){
        this. Row=Row;
        this. NewsTitle=NewsTitle;
        this. SubNesourceID=SubNesourceID;
        this. PicturLink=PicturLink;
        this. InvestmentLink= InvestmentLink;
        this. NewsDateN=NewsDateN;
        this. NewsID=NewsID;
        this. SubNesourceName=SubNesourceName;
        this. ChannelImage=ChannelImage;
        this. ReadFromWebsiteLink= ReadFromWebsiteLink;
        this.NewsDetails=NewsDetails;
    }

}
