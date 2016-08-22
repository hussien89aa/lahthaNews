package com.example.hussienalrubaye.webview;

import android.app.ProgressDialog;
import android.app.SearchManager;

import android.content.Context;
import android.content.Intent;

import android.net.Uri;
import android.os.AsyncTask;
import android.os.Build;
import android.support.v4.content.ContextCompat;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.DisplayMetrics;
import android.util.Log;
import android.util.TypedValue;
import android.view.KeyEvent;
import android.view.LayoutInflater;
import android.view.Menu;
import android.view.MenuItem;
import android.view.MotionEvent;
import android.view.View;
import android.view.ViewGroup;
import android.webkit.WebView;

import android.widget.AbsListView;

import android.widget.BaseAdapter;
import android.widget.Button;

import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.ListView;

import android.widget.SearchView;
import android.widget.TextView;

import com.google.android.gms.ads.AdListener;
import com.google.android.gms.ads.AdRequest;
import com.google.android.gms.ads.InterstitialAd;


import org.apache.http.HttpResponse;
import org.apache.http.client.HttpClient;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.impl.client.DefaultHttpClient;

import org.apache.http.params.HttpConnectionParams;
import org.apache.http.params.HttpParams;
import org.json.JSONArray;
import org.json.JSONObject;

import java.io.BufferedInputStream;

import java.io.BufferedReader;
import java.io.InputStream;

import java.io.InputStreamReader;

import java.io.UnsupportedEncodingException;
import java.net.HttpURLConnection;
import java.net.URL;
import java.net.URLEncoder;
import java.util.ArrayList;

 
import com.google.android.gms.ads.AdView;
//import com.facebook.ads.*;
public class MainActivity extends AppCompatActivity {

    Button buhome;
    Button buadd;
    Button bunewsa;
    Button bulogotype;
   Button bunewNewsComming;
    InterstitialAd mInterstitialAd;
ListView lsNews;
    public static   ArrayList<NewsTicket>    listnewsData = new ArrayList<NewsTicket>();
    ArrayList<NewsTicket>    listnewsDataTemp = new ArrayList<NewsTicket>();
    MyCustomAdapter myadapter;
   int totalItemCountVisible=0; //totalItems visible
    Thread BackThread; //only one thread;
    LinearLayout ChannelInfo; //for channel info
  //  String Query="@"; //loading query
//AQuery aq;
    int IsFollow=0; //for channel if it foloww or not
    int NumberOfFollowers=0; //number of folower for channel
Context context;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        context=this;
        // laod user info
        FileLoad fileinfo=new FileLoad(this);
        fileinfo.LoadData();
        // welcome into system
//aq=new AQuery(this);



        lsNews=(ListView)findViewById(R.id.LVNews);

        myadapter=new MyCustomAdapter(listnewsData);
        lsNews.setAdapter(myadapter);//intisal with data
        buhome=(Button)findViewById(R.id.buhome);
        buadd=(Button)findViewById(R.id. buadd);
        bunewsa=(Button)findViewById(R.id.bunewsa);
        bulogotype=(Button)findViewById(R.id.bulogotype);
        bunewNewsComming=(Button)findViewById(R.id.bunewNewsComming);
        bunewNewsComming.setVisibility(View.GONE);
        ChannelInfo=(LinearLayout)findViewById(R.id.ChannelInfo);
        ChannelInfo.setVisibility(View.GONE);
      //  progressBar = (ProgressBar) findViewById(R.id.progressBar1);

        // start notify
        if(! ServiceNotification.ServiceIsRunInbackground ) {
            ServiceNotification.ServiceIsRunInbackground  = true;
            ServiceNotification.EnableNootification=0;
            Intent intent = new Intent(this, ServiceNotification.class);
             startService(intent);
            //bindService(intent, mConnection, Context.BIND_AUTO_CREATE);
//
        }

    //    bunews(null); // call menu niews

        //display  admob /============admob
       // String android_id = Settings.Secure.getString(this.getContentResolver(), Settings.Secure.ANDROID_ID);


        listnewsData.clear();
// list view scrool
        lsNews.setOnScrollListener(new AbsListView.OnScrollListener() {
            @Override
            public void onScrollStateChanged(AbsListView view, int scrollState) {
// scroll up loading


            }

            @Override
            public void onScroll(AbsListView view, int firstVisibleItem, int visibleItemCount, int totalItemCount) {
               //firstVisibleItem it have been seen
                //visibleItemCount visible now
                totalItemCountVisible=firstVisibleItem + visibleItemCount;
                /*loading items before  he be in the end it 15 news
                we call server in backing and asking for netx 20 news and save it in temp list
                and make sure one (listnewsDataTemp) loaded at time
                and no more than one request at time
                 */
                if((totalItemCountVisible<=  totalItemCount-10)&&((firstVisibleItem>2)))
                {// if we donaot aleady loaded data
                    if((listnewsDataTemp.size() == 0) && (totalItemCount>1)&&(OldNewsStatus.OnlyOneRequest==true)) // to agnore if the result is one item for lading and load more only for
                    {//one time  for request this code load items in hidded way
                    OldNewsStatus.IsLoadMore=true; // make this tag as llod more
                         loadUrl(OldNewsStatus.q, OldNewsStatus.SubNesourceID, "0", OldNewsStatus.Type, totalItemCount, totalItemCount + 20);
                     }
                }
                //loading  (listnewsDataTemp)  list  when he reach end of the lsit
               else if(totalItemCountVisible==  totalItemCount)
                {
                    RefreshListView();// load news when he read the end
                }
                /* when he want new news up of the lsit
                and make sure one request at time
                 */
              if(((firstVisibleItem==0)) &&(OldNewsStatus.PrevfirstVisibleItem!=firstVisibleItem))
                {
                        //if he be in first news
                            if (listnewsData.size() > 1)//if we have loaded data but isnot appeaed in list
                            {  //if isnot already loading know
                                if (  !listnewsData.get(1).NewsDateN.equals("loading_ticket"))
                                {   OldNewsStatus.OnlyOneRequest = true;
                                    loadUrl(OldNewsStatus.q, OldNewsStatus.SubNesourceID, "0", OldNewsStatus.Type, 1, 20);
                                }
                            }
                }
    OldNewsStatus.PrevfirstVisibleItem=firstVisibleItem;

            }
        });


        try
        { // try if there is any notification
            Bundle b = getIntent().getExtras(); // load the notifications
            String NewsID = b.getString("NewsIDAr");
        }
        catch (Exception ex)
        {
// this mean he first time open project or he comm from general form
            bunews(null);

        }

        //*************** alway get new news


        //Display ads

        DisplayAds myDisplayAds=new DisplayAds();
        myDisplayAds.start();

    }
    Boolean IsRunThread=false;
    class mybackthread extends  Thread{
        int LastIndex = 0; //index of lastnews

        @Override
        public void run() {
            while (IsRunThread) {
                try { //delay
                    Thread.sleep(300000);
                } catch (InterruptedException e) {
                    e.printStackTrace();
                }
                if((OldNewsStatus.OnlyOneRequest==true)&&  (bunewNewsComming.getVisibility() == View.GONE) )
                {



                    if (listnewsData.size() > 2)//if we have loaded data but isnot appeaed in list
                    {
                        if (listnewsData.get(0).NewsDateN.equals("ticket_first_item"))
                            if (listnewsData.get(1).NewsDateN.equals("loading_ticket"))
                                LastIndex = -1; //it is already loading


                        // remove any loading ticket
                        if((LastIndex != -1)&& (OldNewsStatus.q.equals("@")) &&(OldNewsStatus.Type==0)) //only in main form display new news
                            for (int i = 0; i < listnewsData.size(); i++)
                            {
                                if (!listnewsData.get(i).NewsDateN.equals("loading_ticket")&& !listnewsData.get(i).NewsDateN.equals("ticket_first_item"))
                                {
                                    String url = GlobalClass.WebURL + "MobileWebService3.asmx/IsGetNews?UserID=" + GlobalClass.UserID + "&NewsID=" + listnewsData.get(i).NewsID + "&SubNesourceID=" + OldNewsStatus.SubNesourceID + "&q=" + OldNewsStatus.q;
                                    //new MyAsyncGetNewNews().cancel(true);
                                    new MyAsyncGetNewNews().execute(url);

                                    try {//delay for stability
                                        Thread.sleep(2000);
                                    } catch (InterruptedException e) {
                                        e.printStackTrace();
                                    }
                                    break;
                                }
                            }

                    }


                }
            }
        }
    }

    // @Override
    public boolean onKeyDown(int keyCode, KeyEvent event) {
        if (keyCode == KeyEvent.KEYCODE_BACK) {
            ServiceNotification.EnableNootification=1;//start notifactions
            Intent intent = new Intent(Intent.ACTION_MAIN);
            intent.addCategory(Intent.CATEGORY_HOME);
            intent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
            startActivity(intent);
            // onQuitPressed();


        }

        return super.onKeyDown(keyCode, event);
    }
    @Override
    protected void onRestart() {
        super.onRestart();  // Always call the superclass method first
        //  LoadAdmob();//8 will run wehn upgrad
        // Activity being restarted from stopped state
    }
    private  void    LoadAdmob(){
        try{
            mInterstitialAd = new InterstitialAd(this);
            mInterstitialAd.setAdUnitId(getResources().getString(R.string.Pop_ad_unit_id));
            AdRequest adRequest = new AdRequest.Builder()
                    //  .addTestDevice(AdRequest.DEVICE_ID_EMULATOR)
                    .build();
            mInterstitialAd.loadAd(adRequest);

            mInterstitialAd.setAdListener(new AdListener() {
                @Override
                public void onAdClosed() {

                }

                @Override
                public void onAdLoaded() {
                    DisplayAdmob();
                }
            });
        } catch (Exception ex) {
        }
    }
    private void DisplayAdmob() {
        if (mInterstitialAd.isLoaded()) {
            mInterstitialAd.show();
        }
        if (!mInterstitialAd.isLoading() && !mInterstitialAd.isLoaded()) {
            AdRequest adRequest = new AdRequest.Builder().build();
            mInterstitialAd.loadAd(adRequest);
        }
    }

//this method update the list view with news when we load it
  private void RefreshListView()
  {

          if (listnewsDataTemp.size() > 0)//if we have loaded data but isnot appeaed in list
          {  //remove loading from first if he loading first items
            if (  listnewsData.get(1).NewsDateN.equals("loading_ticket"))
            {
                listnewsData.clear();//clear for new items
                listnewsData.add(new NewsTicket(null, null, null, null, null, "ticket_first_item", null, null, null, null));
            }
            // remove any loading ticket
              for(int i=0;i<listnewsData.size();i++)
              {
                  if (listnewsData.get(i).NewsDateN.equals("loading_ticket"))
                      listnewsData.remove(i);
              }
//refresh the new data
              listnewsData.addAll(listnewsDataTemp);//get items from temp
              listnewsDataTemp.clear();
              myadapter.notifyDataSetChanged();

              if((listnewsData.size()<=22)&& (!  listnewsData.get( listnewsData.size()-1).NewsTitle.equals(R.string.no_search_result)) && (!  listnewsData.get( listnewsData.size()-1).NewsDateN.equals("loading_ticket"))) // this mean he load first 20 news wih one divestment
              {
                  lsNews.setSelection(1);
              }
          }

      else { //display loading if the server still loading
              if((listnewsData.size()>0)&&(OldNewsStatus.OnlyOneRequest==false))
              {
                  if (!  listnewsData.get( listnewsData.size()-1).NewsDateN.equals("loading_ticket"))
              {
                  listnewsData.add(new NewsTicket(null, null, null, null, null, "loading_ticket", null, null, null, null));
              myadapter.notifyDataSetChanged();
              }
              }

         }


    }
///============admob
/*
    @TargetApi(Build.VERSION_CODES.JELLY_BEAN_MR1)
    private void forceRTLIfSupported()
    {
        if(Build.VERSION.SDK_INT >= Build.VERSION_CODES.JELLY_BEAN_MR1){
            getWindow().getDecorView().setLayoutDirection(View.LAYOUT_DIRECTION_RTL);
        }
    }
*/
    SearchView searchView;
    Menu myMenu;
    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.options_menu, menu);
        myMenu=menu;
        // Associate searchable configuration with the SearchView
        SearchManager searchManager = (SearchManager) getSystemService(Context.SEARCH_SERVICE);
        searchView = (SearchView) menu.findItem(R.id.search).getActionView();
        searchView.setSearchableInfo(searchManager.getSearchableInfo(getComponentName()));
      //final Context co=this;
        searchView.setOnQueryTextListener(new SearchView.OnQueryTextListener() {
            @Override
            public boolean onQueryTextSubmit(String query) {
                // Toast.makeText(co, query, Toast.LENGTH_LONG).show();

                OldNewsStatus.OnlyOneRequest = true;// all another request
                loadUrl(query, 0, "0", 0, 1, 20);
                return false;
            }

            @Override
            public boolean onQueryTextChange(String newText) {
                return false;
            }
        });
     //   searchView.setOnCloseListener(this);
        return true;
    }


    @Override
    protected void onPause() {
        super.onPause();
      //ServiceNotification.SendtNotification=true;
        IsRunThread=false; // stop the thread that lsiten to news
        ServiceNotification.EnableNootification=1;//start notifactions

    }


    @Override
    protected void onResume() {


        super.onResume();
        ServiceNotification.EnableNootification=0; //stop notifaction
        // Logs 'install' and 'app activate' App Events.
      //  AppEventsLogger.activateApp(this);

        //if interent connection is feild
        if (!Operations.isConnectingToInternet(this)) {

            listnewsData.add(0,new NewsTicket(null, getResources().getString(R.string.errorairplane), null, null, null, "No_new_data", null, null, null, null));


            myadapter.notifyDataSetChanged();
            lsNews.setSelection(1);
            return; // do not counine if there is internet service

        }
        // for activiyt resume we clear mhistory

      //ServiceNotification.SendtNotification=false;


           // bunews(null); // call menu niews
        // open news
        try { // try if there is any notification
            Bundle b = getIntent().getExtras(); // load the notifications
            String NewsID = b.getString("NewsIDAr");
            OldNewsStatus.OnlyOneRequest=true;
            //progressBar.setVisibility(View.VISIBLE);
           if(NewsID.equals("0")) // load the news history
                loadUrl("@", 0, "0", 1, 1, 20);
            else if(NewsID.equals("-1")) // load the news most readed
               loadUrl("@", 0, "0", 2, 1, 20);

           else if(NewsID.equals("-2")){// load cchannel news
                int NID = b.getInt("SubResourcesID");
               loadUrl("@", NID, "0", 0, 1, 30);

           }
            else  // this mean notification
           {
               Intent myintents = new Intent(getApplicationContext(), NewsDetails.class);
               myintents.putExtra("NewsID", NewsID);
               startActivity(myintents);

           }
            getIntent().removeExtra("NewsIDAr");

        }
        catch (Exception ex){
// this mean he first time open project or he comm from general form


        }



        if( IsRunThread==false)
        {
            IsRunThread = true;
            BackThread=   new mybackthread();
            BackThread.start(); //start list new news
        }

    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        // Handle action bar item clicks here. The action bar will
        // automatically handle clicks on the Home/Up button, so long
        // as you specify a parent activity in AndroidManifest.xml.
        int id = item.getItemId();

        //noinspection SimplifiableIfStatement
        if (id == R.id.menu) {
            Intent br= new Intent(this,SettingsApp.class);
            startActivity(br);
      }
        if(id==R.id.myresources) {
            Intent br= new Intent(this,ResourcesName.class);
            startActivity(br);

        }

        return super.onOptionsItemSelected(item);
    }

    // load the url of the page
    ProgressDialog dialog;

    private MyAsyncTaskgetNews mTask=null;
    private  void  loadUrl(String q,int SubNesourceID,String NewsID,int Type,int StratFrom,int EndTo) {

        if(OldNewsStatus.OnlyOneRequest==false)return  ;// no more than one request

        OldNewsStatus.OnlyOneRequest=false; // stop accept another request until this one done

        //if interent connection is feild
        if (!Operations.isConnectingToInternet(this)) {

            listnewsData.add(0, new NewsTicket(null, getResources().getString(R.string.nonetworkConnection), null, null, null, "No_new_data", null, null, null, null));


            myadapter.notifyDataSetChanged();
            lsNews.setSelection(1);
            return; // do not counine if there is internet service

        }
        //Toast.makeText(getApplicationContext(),String.valueOf(StratFrom),Toast.LENGTH_LONG).show();

//initailze the  data of var for load more purpose
        OldNewsStatus.SubNesourceID=SubNesourceID;
        OldNewsStatus.Type=Type ;
        OldNewsStatus.q=q;

        if((StratFrom==1) ) //intail loading

        {
            //this first template ticket i dded in the biggening
            if(listnewsData.size()==0)//load first tiket
                listnewsData.add(0,new NewsTicket(null, null, null, null, null, "ticket_first_item", null, null, null, null));
            // add loading ticket to show it loading when we load first 20 news
            if(listnewsData.size()>2) //if he already display loaded
            {
                if (!listnewsData.get(1).NewsDateN.equals("loading_ticket"))
                    listnewsData.add(1, new NewsTicket(null, null, null, null, null, "loading_ticket", null, null, null, null));
            }
            else // loading for first time
                listnewsData.add(1, new NewsTicket(null, null, null, null, null, "loading_ticket", null, null, null, null));


            myadapter.notifyDataSetChanged();
            lsNews.setSelection(1);

        }
        //else  // that mean load more



        //loadind ticket for any thing add item at end of list

        try {
            String query = URLEncoder.encode(q.trim(), "utf-8");

        String url =GlobalClass.WebURL+ "MobileWebService3.asmx/GetNewsNow?UserID=" + GlobalClass.UserID + "&StratFrom="+StratFrom+"&EndTo="+EndTo+"&SubNesourceID="+SubNesourceID+"&NewsID="+NewsID+"&Type="+Type+"&q="+  query;
         // canncel the task
            try{
                mTask.cancel(true);
            }catch (Exception ex){}
            // read news
            mTask=new MyAsyncTaskgetNews();
            mTask.execute(url, "news");
        } catch (UnsupportedEncodingException e) {
            e.printStackTrace();
        }
        //this for view and hide channel info when we want see channel news
        if( SubNesourceID==0)
            ChannelInfo.setVisibility(View.GONE);
        else if((StratFrom==1)&& ( SubNesourceID!=0))// only one time dispaly res info
            {
                //ChannelInfo.setVisibility(View.VISIBLE);
                //get channel info
                String   url = GlobalClass.WebURL + "MobileWebService3.asmx/GetOneSubNesources?UserID=" + GlobalClass.UserID + "&VarSubNesourceID=" + SubNesourceID;
                // read news

                new MyAsyncTaskgetNews().execute(url, "channelInfo");

        }



    }


    private  void  SetDuttons(){
        buhome.setSelected(false);
      buhome.setTextColor( ContextCompat.getColor(context, R.color.lowDarkfont));
        buadd.setSelected(false);
        buadd.setTextColor( ContextCompat.getColor(context, R.color.lowDarkfont));
        bunewsa.setSelected(false);
      bunewsa.setTextColor( ContextCompat.getColor(context, R.color.lowDarkfont));
        bulogotype.setSelected(false);
       bulogotype.setTextColor( ContextCompat.getColor(context, R.color.lowDarkfont));
        bunewNewsComming.setVisibility(View.GONE);//hide new news if it availbe
    }
    // load last news
static  int DisplayAds=0; // display ads after 3 click
    public void bunews(View view) {
        SetDuttons();
        OldNewsStatus. OnlyOneRequest=true;// all another request
        loadUrl("@", 0, "0", 0, 1, 20);
        buhome.setSelected(true);
        buhome.setTextColor( ContextCompat.getColor(context, R.color.darkblue));

        if(DisplayAds==4){
            DisplayAds myDisplayAds=new DisplayAds();
                 myDisplayAds.start();
        }
        DisplayAds++;
        if(   DisplayAds>=8)
            DisplayAds=0;
    }
    //display ads after 4 sec from click home 3 times
    class DisplayAds extends  Thread{

        public void run() {
         try{
Thread.sleep(4000);
            runOnUiThread(new Runnable() {

                @Override
                public void run() {
                    LoadAdmob();//8 will run wehn upgrad
                }
            });

         }catch (Exception ex){}

        }
    }


    public void buyoutube(View view) {
        SetDuttons();
        OldNewsStatus. OnlyOneRequest=true;// all another request
        loadUrl("فيديو", 0, "0", 0, 1, 20);
        bulogotype.setSelected(true);
        bulogotype.setTextColor( ContextCompat.getColor(context, R.color.darkblue));
    }

    public void buajala(View view) {
        SetDuttons();
        OldNewsStatus. OnlyOneRequest=true;// all another request
        loadUrl("عاجل", 0, "0", 0, 1, 20);
        bunewsa.setSelected(true);
        bunewsa.setTextColor( ContextCompat.getColor(context, R.color.darkblue));
    }

    public void onadd(View view) {

        Intent br= new Intent(this,myresources.class);
        startActivity(br);
        // load resource and subresources only when open app


    }



// get list f my resoures
String  result;

    public void bunewnews(View view) {
        OldNewsStatus.OnlyOneRequest=false;
        bunewNewsComming.setVisibility(View.GONE);
lsNews.smoothScrollToPosition(0);

    }
//load my resources
    public class AsyTaskMyResources extends AsyncTask<String, String, String> {
        @Override
        protected void onPreExecute() {

        }
        @Override
        protected String  doInBackground(String... params) {
            // TODO Auto-generated method stub
            InputStream inputStream;
            String murl=GlobalClass.WebURL+ "MobileWebService3.asmx/GetMyResources?UserID=" +GlobalClass.UserID;

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
            GlobalClass.listDataHeader.clear();
            GlobalClass. listDataChild.clear();
            try{
                JSONObject ImageLinklist=new JSONObject(result);
                JSONArray jsa=ImageLinklist.getJSONArray("imageLink");

                for (int i = 0; i <jsa.length() ; i++) {
                    // try to add the resourcess
                    JSONObject js=jsa.getJSONObject(i);

                     JSONArray SubResourcesName=js.getJSONArray("SubResourcesName");
                    JSONArray SubNesourceID=js.getJSONArray("SubNesourceID");
                    JSONArray IsFollow=js.getJSONArray("IsFollow");
                    JSONArray NumberOfFollowers=js.getJSONArray("NumberOfFollowers");
                    JSONArray ChannelImage=js.getJSONArray("ChannelImage");
                    ArrayList< SubResourceItems> subResourceItems = new ArrayList<SubResourceItems>();
                    for (int j = 0; j <NumberOfFollowers.length() ; j++) {
                        subResourceItems.add(new SubResourceItems(SubResourcesName.getString(j),NumberOfFollowers.getInt(j),SubNesourceID.getInt(j),IsFollow.getInt(j),ChannelImage.getString(j)));
                    }
                    if(subResourceItems.size()>0) {
                        GlobalClass. listDataHeader.add(new ResourceItem(js.getString("ResourcesName"), js.getInt("NesourceID"), js.getString("ImageLink")));
                        GlobalClass. listDataChild.put(js.getString("ResourcesName"), subResourceItems);
                    }
                }


            }
            catch (Exception ex){

                Log.d("Err",ex.getMessage());
            }
        }




    }
// -------get all resources
String result1;

    public class AsyTaskAllResourcess extends AsyncTask<String, String, String>{
        @Override
        protected void onPreExecute() {

        }
        @Override
        protected String  doInBackground(String... params) {
            // TODO Auto-generated method stub
            InputStream inputStream;
            String murl=GlobalClass.WebURL+ "MobileWebService3.asmx/GetResources";

            try {
                //String query =new String( params[0].getBytes(), "UTF-8");
                URL url = new URL(murl);
                HttpURLConnection urlConnection = (HttpURLConnection) url.openConnection();

                try {
                    InputStream in = new BufferedInputStream(urlConnection.getInputStream());
                    result1 = Operations.ConvertInputToStringNoChange(in);
                }finally {
                    urlConnection.disconnect();
                }

                publishProgress(result1);

            } catch (Exception e) {
                // TODO Auto-generated catch block

            }
            return null;
        }

        protected void onPostExecute(String  result2){
            // pb.setVisibility(View.GONE);
            //   Toast.makeText(getApplicationContext(), result, Toast.LENGTH_LONG).show();
            try{
                JSONObject js=new JSONObject(result1);
                JSONArray ResourcesName=js.getJSONArray("ResourcesName");
                JSONArray NesourceID=js.getJSONArray("NesourceID");
                JSONArray ImageLink=js.getJSONArray("ImageLink");
                GlobalClass.fullsongpath.clear();
                for (int i = 0; i <NesourceID.length() ; i++) {
                    GlobalClass.fullsongpath.add(new ResourceItem(ResourcesName.getString(i), NesourceID.getInt(i), ImageLink.getString(i)));
                }

            }
            catch (Exception ex){}
        }




    }



// loading news list
    public class MyAsyncTaskgetNews extends AsyncTask<String, String, String>{
        @Override
        protected void onPreExecute() {
          //before works
        }
        @Override
        protected String  doInBackground(String... params) {
            // TODO Auto-generated method stub
            String NewsData="";

            try {
if((int) Build.VERSION.SDK_INT>=23) {
    URL url = new URL(params[0]);
    HttpURLConnection urlConnection = (HttpURLConnection) url.openConnection();
    urlConnection.setConnectTimeout(7000);//set timeout to 5 seconds
    // urlConnection.setReadTimeout(5000);
    try {
        InputStream in = new BufferedInputStream(urlConnection.getInputStream());
        NewsData = Operations.ConvertInputToStringNoChange(in);
    } finally {
        urlConnection.disconnect();
    }

   }else {
    InputStream inputStream;
    HttpClient httpClient = new DefaultHttpClient();
    HttpParams httpParameters = httpClient.getParams();
    HttpConnectionParams.setConnectionTimeout(httpParameters, 7000);
    HttpConnectionParams.setSoTimeout(httpParameters,7000);
    HttpConnectionParams.setTcpNoDelay(httpParameters, true);
    HttpResponse httpResponse = httpClient.execute(new HttpGet(params[0]));
    inputStream = httpResponse.getEntity().getContent();

    NewsData = Operations.ConvertInputToStringNoChange(inputStream);
  }
                publishProgress(NewsData,params[1]) ;

            } catch (Exception e) {
                // TODO Auto-generated catch block

            }

            return null;
        }
        protected void onProgressUpdate(String... progress) {
// if he call it for news puprpose//***************************************************
            if(progress[1].equals("news")) {
                try {
                    JSONObject json = new JSONObject(progress[0]);

                    //  listnewsData.remove(NewsTicket.)
                    listnewsDataTemp.clear();

                    int Tag = json.getInt("Tag") ;
                    if (Tag == 1)
                    {// there is news loading
                        JSONArray newData = json.getJSONArray("newData");


                        for (int i = 0; i < newData.length(); i++) {
                            JSONObject newDataItem = newData.getJSONObject(i);

                            // laod news to list
                            listnewsDataTemp.add(new NewsTicket(newDataItem.getString("Row"), newDataItem.getString("NewsTitle")
                                            , newDataItem.getString("SubNesourceID"), newDataItem.getString("PicturLink"),
                                            newDataItem.getString("InvestmentLink"), newDataItem.getString("NewsDateN"), newDataItem.getString("NewsID"),
                                            newDataItem.getString("SubNesourceName"), newDataItem.getString("ChannelImage"),
                                            newDataItem.getString("readers"))
                            );
                        }
                    }
                    else { // no more new data
                        listnewsDataTemp.add(new NewsTicket(null, getResources().getString(R.string.no_search_result), null, null, null, "No_new_data", null, null, null, null));
                    }

                    // for lading more news in bachground
                   if( OldNewsStatus.IsLoadMore ==false){
                       //no news comming only inverstment
                       if((listnewsDataTemp.get(0).InvestmentLink.length()>0)){
                           listnewsDataTemp.clear();
                           listnewsDataTemp.add(new NewsTicket(null,  getResources().getString(R.string.no_search_result), null, null, null, "No_new_data", null, null, null, null));
                       }
                       RefreshListView();

                   }



                } catch (Exception ex) {
                }
            }//***************************************************
            //in cuase we get news channel info

         else   if(progress[1].equals("channelInfo")) {
              try{

                  JSONObject newDataItem = new JSONObject(progress[0]);

                String ResourcesName = newDataItem.getString("ResourcesName") ;
                 IsFollow = newDataItem.getInt("IsFollow") ;
                 NumberOfFollowers = newDataItem.getInt("NumberOfFollowers") ;
                  String ChannelImage = newDataItem.getString("ChannelImage") ;
                final   String SubNesourceID = newDataItem.getString("SubNesourceID") ;
                  ChannelInfo.setVisibility(View.VISIBLE);


                  final  TextView textView = (TextView) findViewById(R.id.txtnamefollowers);
                  textView.setText(ResourcesName);
                  WebView webview=(WebView) findViewById(R.id.webView2);
                  //String summary = "<html><body style='background:#3b5998;text-align:center'><img src='"+ ChannelImage +"' alt='Mountain View' height='50' width='50'></body></html>\n";
                 // webview.loadData(summary, "text/html", null);
                  webview.getSettings().setLoadWithOverviewMode(true);
                  webview.getSettings().setUseWideViewPort(true);
                  String html = "<html><body><img src=\"" + ChannelImage+ "\" width=\"100%\" style=' height:\"50px;'\"\"/></body></html>";
                  webview.loadData(html, "text/html", null);

                  final  TextView txtflollower = (TextView) findViewById(R.id.txtflollower);
                  txtflollower.setText(getResources().getString(R.string.numberfollowers) + " " + NumberOfFollowers);

                  final   Button bufolow=(Button) findViewById(R.id.bufolowSubscrible);
                  if(IsFollow==1){
                      bufolow.setCompoundDrawablesWithIntrinsicBounds(0, 0, R.drawable.check, 0);
                      bufolow.setText(getResources().getString(R.string.Unfolow));}
                  else
                  {bufolow.setCompoundDrawablesWithIntrinsicBounds(0, 0, R.drawable.add1, 0);
                      bufolow.setText(getResources().getString(R.string.folow));}
                  // click event
                  bufolow.setOnClickListener(new View.OnClickListener() {
                      @Override
                      public void onClick(View v) {

                          if (IsFollow == 1) { // try to do unfollow
                              IsFollow= 0;
                              NumberOfFollowers =NumberOfFollowers - 1;
                              String murl = GlobalClass.WebURL + "MobileWebService3.asmx/Follow2Unfallow?UserID=" + GlobalClass.UserID + "&SubNesourceID=" + SubNesourceID + "&Tag=0";
                              new MyAsyncTaskgetNews().execute(murl, "followandunfollow");
                              bufolow.setText(getResources().getString(R.string.folow));
                              txtflollower.setText(getResources().getString(R.string.numberfollowers) + " " + (NumberOfFollowers));

                              bufolow.setCompoundDrawablesWithIntrinsicBounds(0, 0, R.drawable.add1, 0);
                          } else// try to do follow
                          {
                              IsFollow = 1;
                              NumberOfFollowers = NumberOfFollowers + 1;
                              String murl = GlobalClass.WebURL + "MobileWebService3.asmx/Follow2Unfallow?UserID=" + GlobalClass.UserID + "&SubNesourceID=" + SubNesourceID + "&Tag=1";
                              new MyAsyncTaskgetNews().execute(murl, "followandunfollow");
                              bufolow.setText(getResources().getString(R.string.Unfolow));
                              txtflollower.setText(getResources().getString(R.string.numberfollowers) + " " + (NumberOfFollowers));

                              bufolow.setCompoundDrawablesWithIntrinsicBounds(0, 0, R.drawable.check, 0);
                          }
                          GlobalClass.UpdateHisReourcess=true;
                      }

                  });

              }
catch (Exception ex){}

            }
        }
        protected void onPostExecute(String  result2){
            //this mean he reach the end (3) for loading and first packet
            if((OldNewsStatus.IsLoadMore==true)&&(totalItemCountVisible>=listnewsData.size()-3))
            RefreshListView();

            OldNewsStatus.IsLoadMore=false; //distable load more
            OldNewsStatus. OnlyOneRequest=true; // enable another request

            //load list of resourcess and my resourcess
            if(( GlobalClass.fullsongpath.size()==0)||(GlobalClass.UpdateHisReourcess==true) )
            {
                GlobalClass.UpdateHisReourcess=false;
                // load resource and subresources only when open app
                new AsyTaskAllResourcess().execute();
                new AsyTaskMyResources().execute();}
        }




    }


//check the new news notifaction and update
public class MyAsyncGetNewNews extends AsyncTask<String, String, String> {
    @Override
    protected void onPreExecute() {

    }
    @Override
    protected String  doInBackground(String... params) {
        // TODO Auto-generated method stub
        String NewsData="";
        InputStream inputStream;
        try {

            URL oracle = new URL(params[0]);
            BufferedReader in = new BufferedReader(new InputStreamReader(oracle.openStream()));
            String inputLine="";
            while ((inputLine = in.readLine()) != null)
                NewsData=   NewsData+inputLine;
            in.close();


            publishProgress(NewsData );

        } catch (Exception e) {
            // TODO Auto-generated catch block

        }
        return null;
    }
    protected void onProgressUpdate(String... progress) {
// if he call it for news puprpose//***************************************************

        try {

            JSONObject newDataItem = new JSONObject(progress[0]);

            int Tag = newDataItem.getInt("Tag") ;
            if (Tag == 1) {// we have new news
// if he is in first location we load it direcory
if(   OldNewsStatus.PrevfirstVisibleItem<=1){
    loadUrl(OldNewsStatus.q, OldNewsStatus.SubNesourceID, "0", OldNewsStatus.Type, 1, 20);


}
                else
{
    bunewNewsComming.setVisibility(View.VISIBLE);
}




            }

        } catch (Exception ex) {
            ex.printStackTrace();
        }

    }
    protected void onPostExecute(String  result2){


    }




}

    //display news list
    private class MyCustomAdapter extends BaseAdapter {
        public  ArrayList<NewsTicket>  listnewsDataAdpater ;

        public MyCustomAdapter(ArrayList<NewsTicket>  listnewsDataAdpater) {
            this.listnewsDataAdpater=listnewsDataAdpater;
        }


        @Override
        public int getCount() {
            return listnewsDataAdpater.size();
        }

        @Override
        public String getItem(int position) {
            return null;
        }

        @Override
        public long getItemId(int position) {
            return position;
        }

        @Override
        public View getView(int position, View convertView, ViewGroup parent) {
            LayoutInflater mInflater = getLayoutInflater();

            final   NewsTicket s = listnewsDataAdpater.get(position);
            if(s.NewsDateN.equals("loading_ticket")) { //it is loading ticket

                View myView = mInflater.inflate(R.layout.news_ticket_loading, null);


                return myView;
            }
            else if( s.NewsDateN.equals("No_new_data")){ //no more news
                View myView = mInflater.inflate(R.layout.news_ticket_no_news, null);
                TextView txtMessage=( TextView)myView.findViewById(R.id.txtMessage);
                txtMessage.setText(s.NewsTitle);
                return myView;
            }
            else if( s.NewsDateN.equals("ticket_first_item")){
                View myView = mInflater.inflate(R.layout.news_ticket_first_item, null);
                return myView;
            }
            else
            {
                //***********************************for news ticket***********************************
                if(s.InvestmentLink.length()==0){
                    View myView = mInflater.inflate(R.layout.news_ticket, null);
                    final   TextView txt_viewer = (TextView) myView.findViewById(R.id.txt_viewer);
                    txt_viewer.setText(s.readers.equals("0")?" ":s.readers);
                    TextView txt_channel_name = (TextView) myView.findViewById(R.id.txt_channel_name);
                    txt_channel_name.setText(s.SubNesourceName);
                    txt_channel_name.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            loadUrl("@", Integer.parseInt(s.SubNesourceID), "0", 0, 1, 20);
                        }
                    });
                    //txt_channel_name.setPaintFlags(Paint.UNDERLINE_TEXT_FLAG);
                    TextView txt_news_date = (TextView) myView.findViewById(R.id.txt_news_date);
                    txt_news_date.setText(s.NewsDateN);
                    TextView txt_news_title = (TextView) myView.findViewById(R.id.txt_news_title);
                    txt_news_title.setText(s.NewsTitle);
                    txt_news_title.setTextSize(TypedValue.COMPLEX_UNIT_SP,GlobalClass.FontSize);
                    txt_news_title.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {

                            callDetailsView(s.NewsID , txt_viewer,s.readers);
                        }
                    });




                    //alal icon
                    ImageView IVAjal=(ImageView)myView.findViewById(R.id.IVAjal);
                    if(!s.NewsTitle.contains(getResources().getString(R.string.ajala)))// if isnot ajail
                        IVAjal.setVisibility(View.GONE);
                    WebView webview=(WebView)myView.findViewById(R.id.wv_channel_mage);
                    // String html = "<html><body style='background:#ffffff;text-align:left'><img src='"+ s.ChannelImage +"' alt='Mountain View' height='35px' width='35px'></body></html>\n";
                    //  webview.loadData(html, "text/html", null);
                    webview.getSettings().setLoadWithOverviewMode(true);
                    webview.getSettings().setUseWideViewPort(true);
                    // define size
                    int density= getResources().getDisplayMetrics().densityDpi;
                    int Width=40;
                    int Height=40;
                    switch(density)
                    {
                        case DisplayMetrics.DENSITY_LOW:
                            //Toast.makeText(context, "LDPI", Toast.LENGTH_SHORT).show();
                            Width=40;
                            Height=40;
                            break;

                        case DisplayMetrics.DENSITY_MEDIUM:
                            //Toast.makeText(context, "MDPI", Toast.LENGTH_SHORT).show();
                            Width=100;
                            Height=100;
                            break;
                        case DisplayMetrics.DENSITY_HIGH:
                            // Toast.makeText(context, "HDPI", Toast.LENGTH_SHORT).show();
                            Width=60;
                            Height=60;
                            break;
                        case DisplayMetrics.DENSITY_XHIGH:
                            // Toast.makeText(context, "XHDPI", Toast.LENGTH_SHORT).show();
                            Width=70;
                            Height=70;
                            break;
                        case DisplayMetrics.DENSITY_XXHIGH:
                            // Toast.makeText(context, "XHDPI", Toast.LENGTH_SHORT).show();
                            Width=80;
                            Height=80;
                            break;
                        case DisplayMetrics.DENSITY_XXXHIGH :
                            // Toast.makeText(context, "XHDPI", Toast.LENGTH_SHORT).show();
                            Width=90;
                            Height=90;
                            break;
                    }
                    String html = "<html><body><img src=" + s.ChannelImage+ " width="+ Width+"px style=' height:"+ Height+"px'/></body></html>";
                    webview.loadData(html, "text/html", null);
                    webview.setOnTouchListener(new View.OnTouchListener() {

                        public final static int FINGER_RELEASED = 0;
                        public final static int FINGER_TOUCHED = 1;
                        public final static int FINGER_DRAGGING = 2;
                        public final static int FINGER_UNDEFINED = 3;

                        private int fingerState = FINGER_RELEASED;


                        @Override
                        public boolean onTouch(View view, MotionEvent motionEvent) {

                            switch (motionEvent.getAction()) {

                                case MotionEvent.ACTION_DOWN:
                                    if (fingerState == FINGER_RELEASED)
                                        fingerState = FINGER_TOUCHED;
                                    else fingerState = FINGER_UNDEFINED;
                                    break;

                                case MotionEvent.ACTION_UP:
                                    if (fingerState != FINGER_DRAGGING) {
                                        fingerState = FINGER_RELEASED;

                                        loadUrl("@", Integer.parseInt(s.SubNesourceID), "0", 0, 1, 20);

                                    } else if (fingerState == FINGER_DRAGGING)
                                        fingerState = FINGER_RELEASED;
                                    else fingerState = FINGER_UNDEFINED;
                                    break;

                                case MotionEvent.ACTION_MOVE:
                                    if (fingerState == FINGER_TOUCHED || fingerState == FINGER_DRAGGING)
                                        fingerState = FINGER_DRAGGING;
                                    else fingerState = FINGER_UNDEFINED;
                                    break;

                                default:
                                    fingerState = FINGER_UNDEFINED;

                            }

                            return false;
                        }
                    });
                     WebView wv_news_image=(WebView)myView.findViewById(R.id.wv_news_image);
                    if(s.PicturLink.length()==0){
                        wv_news_image.setVisibility(View.GONE);

                    }else
                    {

//1

                        wv_news_image.getSettings().setLoadWithOverviewMode(true);
                        wv_news_image.getSettings().setUseWideViewPort(true);
                         html = "<html><body><img src=\"" + s.PicturLink + "\" width=\"100%\" style=' height:\"100%;'\"\"/></body></html>";
                        wv_news_image.loadData(html, "text/html", null);
                        wv_news_image.setOnTouchListener(new View.OnTouchListener() {

                            public final static int FINGER_RELEASED = 0;
                            public final static int FINGER_TOUCHED = 1;
                            public final static int FINGER_DRAGGING = 2;
                            public final static int FINGER_UNDEFINED = 3;

                            private int fingerState = FINGER_RELEASED;


                            @Override
                            public boolean onTouch(View view, MotionEvent motionEvent) {

                                switch (motionEvent.getAction()) {

                                    case MotionEvent.ACTION_DOWN:
                                        if (fingerState == FINGER_RELEASED)
                                            fingerState = FINGER_TOUCHED;
                                        else fingerState = FINGER_UNDEFINED;
                                        break;

                                    case MotionEvent.ACTION_UP:
                                        if (fingerState != FINGER_DRAGGING) {
                                            fingerState = FINGER_RELEASED;

                                            callDetailsView(s.NewsID , txt_viewer,s.readers);

                                        } else if (fingerState == FINGER_DRAGGING)
                                            fingerState = FINGER_RELEASED;
                                        else fingerState = FINGER_UNDEFINED;
                                        break;

                                    case MotionEvent.ACTION_MOVE:
                                        if (fingerState == FINGER_TOUCHED || fingerState == FINGER_DRAGGING)
                                            fingerState = FINGER_DRAGGING;
                                        else fingerState = FINGER_UNDEFINED;
                                        break;

                                    default:
                                        fingerState = FINGER_UNDEFINED;

                                }

                                return false;
                            }
                        });

                    }
                    // share button
                    ImageView share=(ImageView)myView.findViewById(R.id.iv_share);
                    share.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            try{
                                String message="";
                                message=s.NewsTitle + " "    +  System.getProperty("line.separator")    +     GlobalClass.WebURL+"NewsDetails.aspx?NID=" +
                                        s.NewsID+"&id=" +  String.valueOf(GlobalClass.UserID)  + "    "+  System.getProperty("line.separator")    + getResources().getString(R.string.PowerdBy);

                                Intent sharingIntent = new Intent(Intent.ACTION_SEND);
                                sharingIntent.setType("text/plain");
                                sharingIntent.putExtra(android.content.Intent.EXTRA_TEXT, message);
                                //sharingIntent.setPackage("com.whatsapp");
                                startActivity(Intent.createChooser(sharingIntent, "Share using"));
                            } catch (Exception e) {
                                //  Toast.makeText(mContext, "Cannot share", Toast.LENGTH_SHORT).show();
                            }
                        }
                    });

                    return myView;}

                else  //*********************************** for inverstment link***********************************
                {
                    if(s.NewsTitle.equals("googleads"))
                    { // for google ads

                        View myView = mInflater.inflate(R.layout.news_ticket_ads, null);
                        AdView mAdView = (AdView) myView.findViewById(R.id.adView);
                        AdRequest adRequest = new AdRequest.Builder().build();
                        mAdView.loadAd(adRequest);
                        return myView;
                    }
                      /*
                     else if(s.NewsTitle.equals("facebookads")) // if it facebook ads
                    {
                        // Instantiate an AdView view
                        View myView1 = mInflater.inflate(R.layout.news_ticket_ads_facebook, null);

                        // Find the main layout of your activity
                        RelativeLayout adViewContainer = (RelativeLayout) myView1.findViewById(R.id.adViewContainer);

                        com.facebook.ads.AdView adViewfacebook  = new com.facebook.ads.AdView(context,  "794447820664446_799303996845495", AdSize.BANNER_HEIGHT_50);

                        // Add the ad view to your activity layout
                        adViewContainer.addView(adViewfacebook);

                        // Request to load an ad
                        adViewfacebook.loadAd();



                        return myView1;

                    }*/
                    // for user ads
                    else {
                        View myView = mInflater.inflate(R.layout.news_ticket_investment, null);

                        TextView txt_news_date = (TextView) myView.findViewById(R.id.txt_news_date);
                        txt_news_date.setText(s.NewsDateN);
                        TextView txt_news_title = (TextView) myView.findViewById(R.id.txt_news_title);
                        txt_news_title.setText(s.NewsTitle);

                        WebView wv_news_image = (WebView) myView.findViewById(R.id.wv_news_image);
                        if (s.PicturLink.length() == 0) {
                            wv_news_image.setVisibility(View.GONE);
                        } else {
                            //  String summary = "<html><body style='background:#FF0099CC;text-align:center'><img src='"+ s.PicturLink +"' alt='Mountain View' height='100' width='200'></body></html>\n";
                            // wv_news_image.loadData(summary, "text/html", null);
                            wv_news_image.getSettings().setLoadWithOverviewMode(true);
                            wv_news_image.getSettings().setUseWideViewPort(true);
                            String html = "<html><body><img src=\"" + s.PicturLink + "\" width=\"100%\" style=' height:\"247.50px;'\"\"/></body></html>";
                            wv_news_image.loadData(html, "text/html", null);

                        }
                        //  wv_news_image.setOnTouchListener(convertView);
                        wv_news_image.setLongClickable(true);
                        wv_news_image.setOnTouchListener(new View.OnTouchListener() {

                            public final static int FINGER_RELEASED = 0;
                            public final static int FINGER_TOUCHED = 1;
                            public final static int FINGER_DRAGGING = 2;
                            public final static int FINGER_UNDEFINED = 3;

                            private int fingerState = FINGER_RELEASED;


                            @Override
                            public boolean onTouch(View view, MotionEvent motionEvent) {

                                switch (motionEvent.getAction()) {

                                    case MotionEvent.ACTION_DOWN:
                                        if (fingerState == FINGER_RELEASED)
                                            fingerState = FINGER_TOUCHED;
                                        else fingerState = FINGER_UNDEFINED;
                                        break;

                                    case MotionEvent.ACTION_UP:
                                        if (fingerState != FINGER_DRAGGING) {
                                            fingerState = FINGER_RELEASED;

                                            Intent browserIntent = new Intent(Intent.ACTION_VIEW, Uri.parse(s.InvestmentLink));
                                            startActivity(browserIntent);

                                        } else if (fingerState == FINGER_DRAGGING)
                                            fingerState = FINGER_RELEASED;
                                        else fingerState = FINGER_UNDEFINED;
                                        break;

                                    case MotionEvent.ACTION_MOVE:
                                        if (fingerState == FINGER_TOUCHED || fingerState == FINGER_DRAGGING)
                                            fingerState = FINGER_DRAGGING;
                                        else fingerState = FINGER_UNDEFINED;
                                        break;

                                    default:
                                        fingerState = FINGER_UNDEFINED;

                                }

                                return false;
                            }
                        });


                        return myView;
                    }
                }


            }
        }
    }
    private  void callDetailsView(String NewsID ,TextView txt_viewer ,String readers){
        Intent myintents = new Intent(getApplicationContext(), NewsDetails.class);

        myintents.putExtra("NewsID", NewsID);
        startActivity(myintents);
        txt_viewer.setText(String.valueOf(Integer.parseInt(readers) + 1));
    }

}
