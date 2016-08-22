package com.example.hussienalrubaye.webview;

import android.app.IntentService;

import android.content.Intent;


import org.json.JSONArray;
import org.json.JSONObject;

import java.io.BufferedInputStream;
import java.io.BufferedReader;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;

/**
 * Created by hussienalrubaye on 9/17/15.
 */
public class ServiceNotification  extends IntentService {
boolean ServiceIsRun=true;
    public  static  boolean ServiceIsRunInbackground=false;
    public  static  int EnableNootification=1;
   //public  static  boolean SendtNotification=true;// deprectated
    public ServiceNotification() {
        super("MyWebRequestService");
    }
    protected void onHandleIntent(Intent workIntent) {
        // Gets data from the incoming Intent
        String result;

      while ( ServiceIsRun) {
          try{
              Thread.sleep(600000);
          }catch (Exception ex){}
          if (( EnableNootification==1) ){
          InputStream inputStream;
          String murl=GlobalClass.WebURL+ "MobileWebService3.asmx/IsGetNewsWithHeader?NewsID=" +GlobalClass.LastNewsID +"&UserID="+ GlobalClass.UserID;

          try {

              URL url = new URL(murl);
              HttpURLConnection urlConnection = (HttpURLConnection) url.openConnection();

              try {
                  InputStream in = new BufferedInputStream(urlConnection.getInputStream());
                  result = Operations.ConvertInputToStringNoChange(in);
              }finally {
                  urlConnection.disconnect();
              }

              JSONObject js=new JSONObject(result);
              //JSONObject HasNews=js.getJSONObject("HasNews");

              if (js.getInt("HasNews")==1){
              JSONArray NewsTitle=js.getJSONArray("NewsTitle");
                  JSONArray NewsIDAr=js.getJSONArray("NewsIDAr");
                  GlobalClass.LastNewsID=NewsIDAr.getInt(0);

                  // send notification only on the close mode

              Intent myintent = new Intent(this, MyBroadcastReceiver.class);
              myintent.putExtra("NewsTitle", NewsTitle.getString(0));
                //  myintent.putExtra("NewsIDAr", NewsIDAr.getString(0));
             // myintent.putExtra("message", "hi");
              sendBroadcast(myintent);
              // update the news id


              }
          } catch (Exception e) {
              // TODO Auto-generated catch block

          }}




        }
    }


}
