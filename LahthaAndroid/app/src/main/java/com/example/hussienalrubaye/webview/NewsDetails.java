package com.example.hussienalrubaye.webview;

import android.content.Intent;

import android.os.AsyncTask;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.text.Html;
import android.util.DisplayMetrics;
import android.util.TypedValue;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.webkit.WebView;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.ProgressBar;
import android.widget.TextView;

import com.google.android.gms.ads.AdListener;
import com.google.android.gms.ads.AdRequest;
import com.google.android.gms.ads.InterstitialAd;

import org.json.JSONObject;

import java.io.BufferedReader;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.net.URL;

public class NewsDetails extends AppCompatActivity {
    ProgressBar progressBar1;
   public NewsTicketDetails s=null;
    LinearLayout layoutapp;
    InterstitialAd mInterstitialAd;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_news_details);
        layoutapp=(LinearLayout)findViewById(R.id.layoutapp);
        layoutapp.setVisibility(View.GONE);
       progressBar1=(ProgressBar)findViewById(R.id.progressBar1);
        Bundle b = getIntent().getExtras(); // load the notifications
       String NewsID = b.getString("NewsID");
        String url =GlobalClass.WebURL+ "MobileWebService3.asmx/GetNewsDetials?NewsIDvar="+NewsID +"&UserID="+ GlobalClass.UserID  ;
       new  MyAsyncTaskgetNews().execute(url);

        if(MainActivity.DisplayAds==4){
            LoadAdmob();
        }
        MainActivity. DisplayAds++;
        if(    MainActivity. DisplayAds>=8)
            MainActivity. DisplayAds=0;
    }

    //google ads
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
    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.news_detials, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {

        int id = item.getItemId();
        if (id == R.id.gbackmenu) {
if(MainActivity.listnewsData.size()<=0){
    Intent intent=new Intent(this,MainActivity.class);
    //intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
startActivity(intent);
//}
  //  else
}
            this.finish();
        //
        }
        if(id==R.id.sharedetails){
            try{
                String message="";
                message=s.NewsTitle + ""+  System.getProperty("line.separator")    +    GlobalClass.WebURL+"NewsDetails.aspx?NID=" +
                        s.NewsID+"&id=" +  String.valueOf(GlobalClass.UserID) + System.getProperty("line.separator")+ "    " +getResources().getString(R.string.PowerdBy);

                Intent sharingIntent = new Intent(Intent.ACTION_SEND);
                sharingIntent.setType("text/plain");
                sharingIntent.putExtra(android.content.Intent.EXTRA_TEXT, message);
                startActivity(Intent.createChooser(sharingIntent, "Share using"));
            } catch (Exception e) {
                //  Toast.makeText(mContext, "Cannot share", Toast.LENGTH_SHORT).show();
            }
        }
        return super.onOptionsItemSelected(item);
    }

    public class MyAsyncTaskgetNews extends AsyncTask<String, String, String> {
        @Override
        protected void onPreExecute() {
            progressBar1.setVisibility(View.VISIBLE);
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

                    int Tag = newDataItem.getInt("HasNew") ;
                    if (Tag == 1) {
  s=new  NewsTicketDetails(newDataItem.getString("Row"), newDataItem.getString("NewsTitle")
        , newDataItem.getString("SubNesourceID"), newDataItem.getString("PicturLink"),
        newDataItem.getString("InvestmentLink"), newDataItem.getString("NewsDateN"), newDataItem.getString("NewsID"),
        newDataItem.getString("SubNesourceName"), newDataItem.getString("ChannelImage"),
        newDataItem.getString("ReadFromWebsiteLink"),newDataItem.getString("NewsDetails") );
                        TextView txt_channel_name = (TextView)  findViewById(R.id.txt_channel_name);
                        txt_channel_name.setText(s.SubNesourceName);
                        TextView txt_news_date = (TextView)  findViewById(R.id.txt_news_date);
                        txt_news_date.setText(s.NewsDateN);
                        TextView txt_news_title = (TextView)  findViewById(R.id.txt_news_title);
                        txt_news_title.setText(s.NewsTitle);
                        txt_news_title.setTextSize(TypedValue.COMPLEX_UNIT_SP,GlobalClass.FontSize);
                        TextView  txtweb = (TextView) findViewById(R.id.txtDetails);
                        txtweb.setText(Html.fromHtml(s.NewsDetails));
                        txtweb.setTextSize(TypedValue.COMPLEX_UNIT_SP,GlobalClass.FontSize);
                        //alal icon
                        ImageView IVAjal=(ImageView) findViewById(R.id.IVAjal);
                        if(!s.NewsTitle.contains(getResources().getString(R.string.ajala)))// if isnot ajail
                            IVAjal.setVisibility(View.GONE);
                        WebView webview=(WebView) findViewById(R.id.wv_channel_mage);
                        // String summary = "<html><body style='background:#FF0099CC;text-align:center'><img src='"+ s.ChannelImage +"' alt='Mountain View' height='30' width='30'></body></html>\n";
                        // webview.loadData(summary, "text/html", null);
                        webview.getSettings().setLoadWithOverviewMode(true);
                       webview.getSettings().setUseWideViewPort(true);
                        //String html = "<html><body style='background:#ffffff;text-align:left'><img src='"+ s.ChannelImage +"' alt='Mountain View' height='35px' width='35px'></body></html>\n";
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
                        // webview.getSettings().setLoadWithOverviewMode(true);
                        // webview.getSettings().setUseWideViewPort(true);
                        // String html = "<html><body><img src=\"" + s.ChannelImage + "\" width=\"100%\" height=\"100%\"\"/></body></html>";
                        // webview.loadData(html, "text/html", null);
                        WebView wv_news_image=(WebView) findViewById(R.id.wv_news_image);
                        if(s.PicturLink.length()==0){
                            wv_news_image.setVisibility(View.GONE);

                        }else
                        {

//1
                            //summary = "<html><body style='background:#FF0099CC;text-align:center'><img src='"+ s.PicturLink +"' alt='Mountain View' style='width:304px;height:228px;'></body></html>\n";
                            //  wv_news_image.loadData(summary, "text/html", null);
                            //2
                            wv_news_image.getSettings().setLoadWithOverviewMode(true);
                            wv_news_image.getSettings().setUseWideViewPort(true);
                            // wv_news_image.getSettings().setLayoutAlgorithm(WebSettings.LayoutAlgorithm.SINGLE_COLUMN);
                            html = "<html><body><img src=\"" + s.PicturLink + "\" width=\"100%\" style=' height:\"290.50px;'\"\"/></body></html>";
                            wv_news_image.loadData(html, "text/html", null);
                            //  wv_news_image.setOnTouchListener(convertView);
                            wv_news_image.setLongClickable(true);
                            wv_news_image.setOnLongClickListener(new View.OnLongClickListener() {
                                @Override
                                public boolean onLongClick(View v) {
                                    Intent myintents = new Intent(getApplicationContext(), NewsDetails.class);
                                    myintents.putExtra("NewsID",s.NewsID);
                                    startActivity(myintents);
                                    return false;
                                }
                            });


                        }
                        // share button
                        Button bugotowebsite=(Button) findViewById(R.id.bugotowebsite);
                        bugotowebsite.setOnClickListener(new View.OnClickListener() {
                            @Override
                            public void onClick(View v) {
                                try{

                                    Intent myintents = new Intent(getApplicationContext(), new_view_url.class);
                                    myintents.putExtra("newurl", s.ReadFromWebsiteLink);
                                    startActivity(myintents);
                                    } catch (Exception e) {
                                    //  Toast.makeText(mContext, "Cannot share", Toast.LENGTH_SHORT).show();
                                }
                            }
                        });




                        layoutapp.setVisibility(View.VISIBLE);

                }

                } catch (Exception ex) {
                    ex.printStackTrace();
                }

        }
        protected void onPostExecute(String  result2){
            progressBar1.setVisibility(View.GONE);

        }




    }
}
