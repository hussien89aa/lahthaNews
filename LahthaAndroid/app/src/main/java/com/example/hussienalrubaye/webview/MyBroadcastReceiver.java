package com.example.hussienalrubaye.webview;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.os.Bundle;


/**
 * Created by hussienalrubaye on 9/17/15.
 */
public class MyBroadcastReceiver extends BroadcastReceiver {
    @Override
    public void onReceive(Context context, Intent intent) {
        Bundle bundle = intent.getExtras();
        if (bundle != null) {
            String mstring = bundle.getString("NewsTitle");
           // NewMessageNotification notfiy= new NewMessageNotification();
           // notfiy.notify();
            //Toast.makeText(context,mstring,Toast.LENGTH_LONG).show();
             NewMessageNotification.notify(context, mstring, 123);
            // update the password
            FileLoad fileinfo=new FileLoad(context);
            fileinfo.SaveData();
        }
    }



}
