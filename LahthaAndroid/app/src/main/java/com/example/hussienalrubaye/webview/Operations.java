package com.example.hussienalrubaye.webview;

import android.content.Context;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.provider.Settings;
import android.telephony.TelephonyManager;

import java.io.BufferedReader;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.util.Date;
import java.util.Random;

/**
 * Created by hussienalrubaye on 9/17/15.
 */
public class Operations {


    public static String ConvertInputToStringNoChange(InputStream inputStream) {

        BufferedReader bureader=new BufferedReader( new InputStreamReader(inputStream));
        String line ;
        String linereultcal="";

        try{
            while((line=bureader.readLine())!=null) {

                    linereultcal+=line;

            }
            inputStream.close();


        }catch (Exception ex){}

        return linereultcal;
    }

    public static boolean isConnectingToInternet(Context context){
        ConnectivityManager connectivity = (ConnectivityManager) context.getSystemService(Context.CONNECTIVITY_SERVICE);
        if (connectivity != null)
        {
            NetworkInfo activeNetwork = connectivity.getActiveNetworkInfo();
            boolean isConnected = activeNetwork != null && activeNetwork.isConnectedOrConnecting();

                        return isConnected;
                 

        }
        return false;
    }
    public static String getUniqueID(Context context){
        String myAndroidDeviceId = "";
       try{
           TelephonyManager mTelephony = (TelephonyManager) context.getSystemService(Context.TELEPHONY_SERVICE);
           if (mTelephony.getDeviceId() != null){
               myAndroidDeviceId = mTelephony.getDeviceId();
           }else{
               myAndroidDeviceId = Settings.Secure.getString(context.getApplicationContext().getContentResolver(), Settings.Secure.ANDROID_ID);
           }

       }
       catch (Exception ex){
          long DateNumber = (long) new Date().getTime();
           Random r = new Random();
           myAndroidDeviceId=String.valueOf(DateNumber)+"F"+String.valueOf(  r.nextInt(90000 - 65) + 65);

       }
        return myAndroidDeviceId;
    }
}
