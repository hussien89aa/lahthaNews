package com.example.hussienalrubaye.webview;

/**
 * Created by hussienalrubaye on 11/12/15.
 */
public class OldNewsStatus {
    // this keep save the stutus of last search operation
   public static int SubNesourceID=0;
    public static  int Type=0;
    public static String q="@";
    public static  Boolean OnlyOneRequest=true; //only one call at time to the server
  public static int PrevfirstVisibleItem=0 ; //prevouse visble item
    public static boolean IsLoadMore=false;// if he look for new news
}
