package com.example.hussienalrubaye.webview;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

/**
 * Created by hussienalrubaye on 9/14/15.
 */
public class GlobalClass {
    public  static  String WebURL="Your_Server_URL";
public static  String APPURL="com.ruabye.hussienalrubaye.webview";
    public  static  int UserID=1;
    public  static  int LastNewsID=1;
    public  static  int FontSize=16;
    public  static  Boolean UpdateHisReourcess=false; // call when he update his resouress
    public  static  String Deviceuid;
    public  static List<ResourceItem> listDataHeader=new ArrayList<ResourceItem>() ;
    public  static HashMap<String, List<SubResourceItems>> listDataChild= new HashMap<String, List<SubResourceItems>>();
    public static ArrayList<ResourceItem> fullsongpath=new ArrayList<ResourceItem>() ;
}
