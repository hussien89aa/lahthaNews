package com.example.hussienalrubaye.webview;


import android.content.Intent;

import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.view.LayoutInflater;
import android.view.Menu;
import android.view.MenuItem;
import android.view.MotionEvent;
import android.view.View;
import android.view.ViewGroup;
import android.webkit.WebView;
import android.widget.AdapterView;
import android.widget.BaseAdapter;
import android.widget.Button;
import android.widget.GridView;

import android.widget.ProgressBar;
import android.widget.TextView;

import java.util.ArrayList;


public class ResourcesName extends AppCompatActivity {

    GridView ls;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_resources_name);
 
         ls=(GridView ) findViewById(R.id.gridView);

        ls.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
                try {

                    // videoView.setVideoPath(fullsongpath.get(position).Path);
                    // Toast.makeText(getApplicationContext(),String.valueOf( fullsongpath.get(position).ID), Toast.LENGTH_LONG).show();
                    // videoView.start();
                    Intent intent = new Intent(getApplicationContext(), SubResources.class);
                    Bundle b = new Bundle();
                    b.putInt("ID",  GlobalClass.fullsongpath.get(position).ID);
                    intent.putExtras(b);
                    startActivity(intent);

                } catch (Exception e) {
                    e.printStackTrace();
                }
            }
        });

        ls.setAdapter(new MyCustomAdapter( GlobalClass.fullsongpath));
       // Toast.makeText(getApplicationContext(), x, Toast.LENGTH_LONG).show();

    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.menu_resources_name, menu);
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
            //Intent intent= new Intent(getApplicationContext(), MainActivity.class);
          //  startActivity(intent);
            this.finish();

        }

        return super.onOptionsItemSelected(item);
    }

    // adapter for song list
    private class MyCustomAdapter extends BaseAdapter {
        public  ArrayList<ResourceItem> fullsongpath ;

        public MyCustomAdapter(ArrayList<ResourceItem> fullsongpath) {
this.fullsongpath=fullsongpath;
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
            View myView = mInflater.inflate(R.layout.item_resources_ame, null);
          final   ResourceItem s = fullsongpath.get(position);
            TextView textView = (TextView) myView.findViewById(R.id.textView);
            textView.setText(s.Name);

            WebView webview=(WebView)myView.findViewById(R.id.webView3);
            String summary = "<html><body style='background:#FF0099CC;text-align:center'><img src='"+ s.ImageLink +"' alt='Mountain View' height='50' width='50'></body></html>\n";
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

                                Intent intent = new Intent(getApplicationContext(), SubResources.class);
                                Bundle b = new Bundle();
                                b.putInt("ID", s.ID);
                                intent.putExtras(b);
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
Button openDep=(Button)myView.findViewById(R.id.openDep);
            openDep.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View v) {
                    Intent intent = new Intent(getApplicationContext(), SubResources.class);
                    Bundle b = new Bundle();
                    b.putInt("ID", s.ID);
                    intent.putExtras(b);
                    startActivity(intent);
                }
            });
            return myView;
        }
    }
    private ProgressBar pb;



















}













    // Class with extends AsyncTask class

