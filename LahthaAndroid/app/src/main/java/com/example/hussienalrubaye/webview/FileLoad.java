package com.example.hussienalrubaye.webview;

import android.content.Context;
import android.content.SharedPreferences;
import android.os.AsyncTask;


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

public class FileLoad {

    Context context;
    SharedPreferences sharedpreferences;
    public static final String MyPREFERENCES = "MyPrefs3" ;
    public static final String UserID = "UserID";
    public static final String LastNewsID = "LastNewsID";
    public static final String EnableNootification = "EnableNootification";
    public static int IsRated=0;//app rate 0 not rate 1 is rate
    public  FileLoad(Context context) {
        this.context=context;
        sharedpreferences = context.getSharedPreferences(MyPREFERENCES, Context.MODE_PRIVATE);

    }
    public void SaveData()  {

        try

        {

            SharedPreferences.Editor editor = sharedpreferences.edit();

            editor.putString(UserID,String.valueOf(  GlobalClass.UserID));
            editor.putString(LastNewsID,String.valueOf( GlobalClass.LastNewsID));
            editor.putInt("IsRated", IsRated);
            editor.putInt("FontSize", GlobalClass.FontSize);
            editor.putString(EnableNootification,String.valueOf( ServiceNotification.EnableNootification));
            editor.commit();
            LoadData( );
        }

        catch( Exception e)

        {

           // Toast.makeText(context, "Unable to write to the SettingFile file.", Toast.LENGTH_LONG).show();
        }
    }
    public   void LoadData( ) {

        String UserID=sharedpreferences.getString("UserID","empty");
        if(!UserID.equals("empty"))
       {
                GlobalClass.UserID=Integer.parseInt(UserID);// load user name
                GlobalClass.LastNewsID=Integer.parseInt(sharedpreferences.getString("LastNewsID","0") );// load last news
           ServiceNotification.EnableNootification=Integer.parseInt(sharedpreferences.getString("EnableNootification","0") );// load last news
           IsRated=sharedpreferences.getInt("IsRated", 0);
           GlobalClass.FontSize=sharedpreferences.getInt("FontSize", 16);
       }
        else { //iniatil account for firt load time
             GlobalClass.Deviceuid= Operations.getUniqueID(context) ;//  tManager.getDeviceId();

            new MyAsyncTask().execute( );
        }
    }
public  String  result;
    public class MyAsyncTask extends AsyncTask<String, String, String> {
        @Override
        protected void onPreExecute() {

        }
        @Override
        protected String  doInBackground(String... params) {

            // TODO Auto-generated method stub
            InputStream inputStream;

            String murl=GlobalClass.WebURL+ "MobileWebService3.asmx/InitializeAccount?MAC=" + GlobalClass.Deviceuid;

            try {
                //String query =new String( params[0].getBytes(), "UTF-8");
                URL url = new URL(murl);
                HttpURLConnection urlConnection = (HttpURLConnection) url.openConnection();

                try {
                    InputStream in = new BufferedInputStream(urlConnection.getInputStream());
                    result = Operations.ConvertInputToStringNoChange(in);
                }finally {
                    urlConnection.disconnect();
                }

                publishProgress(result);

            } catch (Exception e) {
                // TODO Auto-generated catch block

            }
            return null;
        }

        protected void onPostExecute(String  result1){
            // pb.setVisibility(View.GONE);
            //   Toast.makeText(getApplicationContext(), result, Toast.LENGTH_LONG).show();
            try{
                JSONObject js=new JSONObject(result);
                GlobalClass.UserID=js.getInt("UserID");
                //JSONArray NesourceID=js.getJSONArray("NesourceID");
                 SaveData();
            }
            catch (Exception ex){}
        }




    }

}
