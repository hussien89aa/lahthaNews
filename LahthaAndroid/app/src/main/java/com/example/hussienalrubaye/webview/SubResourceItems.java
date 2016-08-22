package com.example.hussienalrubaye.webview;

/**
 * Created by hussienalrubaye on 9/17/15.
 */
public class SubResourceItems {
    public  String Name;
    public  int folowers;
    public  int isfolowed;
    public  int ID;
    public  String ChannelImage;

    public  SubResourceItems (String Name,int folowers, int ID,int isfolowed ,String ChannelImage){
        this.Name=Name;
        this.ID=ID;
        this.isfolowed=isfolowed;
        this.folowers=folowers;
        this.ChannelImage=ChannelImage;

    }
}
