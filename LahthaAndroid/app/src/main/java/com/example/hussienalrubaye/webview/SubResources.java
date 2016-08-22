package com.example.hussienalrubaye.webview;

import android.content.Context;
import android.content.Intent;

import android.os.AsyncTask;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.Menu;
import android.view.MenuItem;
import android.view.MotionEvent;
import android.view.View;
import android.view.ViewGroup;
import android.webkit.WebView;

import android.widget.BaseAdapter;
import android.widget.Button;
import android.widget.GridView;

import android.widget.TextView;



import org.json.JSONArray;
import org.json.JSONObject;

import java.io.BufferedInputStream;
import java.io.BufferedReader;
import java.io.InputStream;

import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;
import java.util.ArrayList;

public class SubResources extends AppCompatActivity {
    ArrayList<SubResourceItems > fullsongpath =new ArrayList<SubResourceItems>();
    GridView ls;
    Context context;
    int NesourceID=0;
    boolean isListOfChannel=true;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_sub_resources);
        Bundle b=getIntent().getExtras();
        NesourceID=  b.getInt("ID") ;
        ls=(GridView) findViewById(R.id.gridView);
     // ls.setAdapter(new MyCustomAdapter());
        isListOfChannel=true;
        String murl=GlobalClass.WebURL+ "MobileWebService3.asmx/GetSubNesources?UserID=" + GlobalClass.UserID + "&NesourceID=" +NesourceID;

        new MyAsyncTask().execute(murl);
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.menu_sub_resources, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        // Handle action bar item clicks here. The action bar will
        // automatically handle clicks on the Home/Up button, so long
        // as you specify a parent activity in AndroidManifest.xml.
        int id = item.getItemId();

        //noinspection SimplifiableIfStatement
        if (id == R.id.goback) {
        // Intent intent=new Intent(this,ResourcesName.class);
          //  startActivity(intent);
            this.finish();
        }

        return super.onOptionsItemSelected(item);
    }
    private class MyCustomAdapter extends BaseAdapter {


        public MyCustomAdapter() {
           // fullsongpath.add(new SubResourceItems("a",0,0,0));

        }


        @Override
        public int getCount() {
            return fullsongpath.size();
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
            View myView = mInflater.inflate(R.layout.item_subresource_list, null);
           final SubResourceItems s = fullsongpath.get(position);
            TextView textView = (TextView) myView.findViewById(R.id.txtnamefollowers);
            textView.setText(s.Name);
            WebView webview=(WebView)myView.findViewById(R.id.webView2);
            String summary = "<html><body style='background:#FF0099CC;text-align:center'><img src='"+ s.ChannelImage +"' alt='Mountain View' height='50' width='50'></body></html>\n";
            webview.loadData(summary, "text/html", null);
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

                                Intent intent = new Intent(getApplicationContext(), MainActivity.class);
                               // intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                                intent.putExtra("NewsIDAr", "-2");
                                intent.putExtra("SubResourcesID", s.ID);
                                startActivity(intent);

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
 
            final  TextView txtflollower = (TextView) myView.findViewById(R.id.txtflollower);
            txtflollower.setText(getResources().getString(R.string.numberfollowers) + " " + s.folowers);

            final   Button bufolow=(Button) myView.findViewById(R.id.bufolowSubscrible);
            if(s.isfolowed==1){
                bufolow.setCompoundDrawablesWithIntrinsicBounds(0, 0, R.drawable.check, 0);
               bufolow.setText(getResources().getString(R.string.Unfolow));}
           else
            {bufolow.setCompoundDrawablesWithIntrinsicBounds(0, 0, R.drawable.add1, 0);
             bufolow.setText(getResources().getString(R.string.folow));}
            // click event
bufolow.setOnClickListener(new View.OnClickListener() {
    @Override
    public void onClick(View v) {

        if (s.isfolowed == 1) { // try to do unfollow
            s.isfolowed = 0;
            s.folowers = s.folowers - 1;
            String murl = GlobalClass.WebURL + "MobileWebService3.asmx/Follow2Unfallow?UserID=" + GlobalClass.UserID + "&SubNesourceID=" + s.ID + "&Tag=0";

            new MyAsyncTask().execute(murl);
            bufolow.setText(getResources().getString(R.string.folow));
            txtflollower.setText(getResources().getString(R.string.numberfollowers) + " " + (s.folowers));

            bufolow.setCompoundDrawablesWithIntrinsicBounds(0, 0, R.drawable.add1, 0);
        } else// try to do follow
        {
            s.isfolowed = 1;
            s.folowers = s.folowers + 1;
            String murl = GlobalClass.WebURL + "MobileWebService3.asmx/Follow2Unfallow?UserID=" + GlobalClass.UserID + "&SubNesourceID=" + s.ID + "&Tag=1";

            new MyAsyncTask().execute(murl);
            bufolow.setText(getResources().getString(R.string.Unfolow));
            txtflollower.setText(getResources().getString(R.string.numberfollowers) + " " + (s.folowers));

            bufolow.setCompoundDrawablesWithIntrinsicBounds(0, 0, R.drawable.check, 0);
        }

        GlobalClass.UpdateHisReourcess=true;// notify new channel subscribels changes
    }
});

            return myView;
        }
    }
    String result;
    public class MyAsyncTask extends AsyncTask<String, String, String> {
        @Override
        protected void onPreExecute() {

        }
        @Override
        protected String  doInBackground(String... params) {
            // TODO Auto-generated method stub
            InputStream inputStream;

            try {
                //String query =new String( params[0].getBytes(), "UTF-8");
                URL url = new URL(params[0]);
                HttpURLConnection urlConnection = (HttpURLConnection) url.openConnection();

                try {
                    InputStream in = new BufferedInputStream(urlConnection.getInputStream());
                    result = Operations.ConvertInputToStringNoChange(in);
                }finally {
                    urlConnection.disconnect();
                }
                publishProgress(result);

            } catch (Exception e) {
                e.printStackTrace();
                // TODO Auto-generated catch block

            }
            return null;
        }

        protected void onPostExecute(String  result1){
            // pb.setVisibility(View.GONE);
            //   Toast.makeText(getApplicationContext(), result, Toast.LENGTH_LONG).show();
            if (isListOfChannel==true){
            try{
                JSONObject js=new JSONObject(result);
                JSONArray SubResourcesName=js.getJSONArray("ResourcesName");
                JSONArray SubNesourceID=js.getJSONArray("SubNesourceID");
                JSONArray IsFollow=js.getJSONArray("IsFollow");
                JSONArray NumberOfFollowers=js.getJSONArray("NumberOfFollowers");
                JSONArray ChannelImage=js.getJSONArray("ChannelImage");
                for (int i = 0; i <NumberOfFollowers.length() ; i++) {
                    fullsongpath.add(new SubResourceItems(SubResourcesName.getString(i),NumberOfFollowers.getInt(i),SubNesourceID.getInt(i),IsFollow.getInt(i),ChannelImage.getString(i)));
                }

                ls.setAdapter(new MyCustomAdapter());
            }
            catch (Exception ex){}
            isListOfChannel=false;}
        }




    }
}
