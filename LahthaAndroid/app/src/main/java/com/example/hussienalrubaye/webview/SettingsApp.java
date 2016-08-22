package com.example.hussienalrubaye.webview;

import android.content.ActivityNotFoundException;
import android.content.Intent;
import android.net.Uri;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.text.Html;
import android.view.LayoutInflater;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.view.ViewGroup;

import android.widget.AdapterView;
import android.widget.BaseAdapter;

import android.widget.CompoundButton;

import android.widget.ImageView;
import android.widget.ListView;
import android.widget.SeekBar;
import android.widget.Switch;
import android.widget.TextView;

import java.util.ArrayList;

public class SettingsApp extends AppCompatActivity {
    ArrayList<SettingItem> fullsongpath =new ArrayList<SettingItem>();
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_settings_app);
        //intiixae items
        fullsongpath.add(new SettingItem(getResources().getString(R.string.generldepartment), R.drawable.settings));
        fullsongpath.add(new SettingItem(getResources().getString(R.string.readit), R.drawable.living1));
        fullsongpath.add(new SettingItem(getResources().getString(R.string.readitmore), R.drawable.eye));
        fullsongpath.add(new SettingItem(getResources().getString(R.string.resource), R.drawable.add1));
        fullsongpath.add(new SettingItem(getResources().getString(R.string.action_settings), R.drawable.settings));
        fullsongpath.add(new SettingItem(getResources().getString(R.string.alerts), R.drawable.notifications));
        fullsongpath.add(new SettingItem(getResources().getString(R.string.FontSize), R.drawable.fontsize));
        fullsongpath.add(new SettingItem(getResources().getString(R.string.callus),R.drawable.envelope91));
        fullsongpath.add(new SettingItem(getResources().getString(R.string.separeteapp), R.drawable.network));

        fullsongpath.add(new SettingItem(getResources().getString(R.string.rateapp), R.drawable.social));

     ListView ls=( ListView) findViewById(R.id.listView);
        ls.setAdapter(new MyCustomAdapter());
        ls.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
                try {


                    switch (position) {
                        case 7:
                        {
                            Intent intent = new Intent(Intent.ACTION_VIEW, Uri.parse(GlobalClass.WebURL));
                            startActivity(intent);
                        } break;

                        case 8:
                        {
                            Intent sharingIntent = new Intent(Intent.ACTION_SEND);
                            sharingIntent.setType("text/plain");
                            sharingIntent.putExtra(android.content.Intent.EXTRA_TEXT, Html.fromHtml(getResources().getString(R.string.sharemessage)+   "  https://play.google.com/store/apps/details?id=" + GlobalClass.APPURL + ""));
                                    startActivity(Intent.createChooser(sharingIntent, "Share using"));
                        } break;
                        case 3:
                        {
                            Intent intent = new Intent(getApplicationContext(), ResourcesName.class);
                             startActivity(intent);

                        }break;
                        case 9:
                        {
                            Uri uri = Uri.parse("market://details?id=" + GlobalClass.APPURL);
                            Intent goToMarket = new Intent(Intent.ACTION_VIEW, uri);
                            // To count with Play market backstack, After pressing back button,
                            // to taken back to our application, we need to add following flags to intent.
                            goToMarket.addFlags(
                                    Intent.FLAG_ACTIVITY_MULTIPLE_TASK);
                            try {
                                startActivity(goToMarket);
                            } catch (ActivityNotFoundException e) {
                                startActivity(new Intent(Intent.ACTION_VIEW,
                                        Uri.parse("http://play.google.com/store/apps/details?id=" + GlobalClass.APPURL)));
                            }
                            //save rating
                            FileLoad.IsRated = 1;
                            FileLoad sv = new FileLoad(getApplicationContext());
                            sv.SaveData();

                        }

                            break;
                        case 1: // for my read history
                            Intent myintent = new Intent(getApplicationContext(), MainActivity.class);
                            myintent.putExtra("NewsIDAr", "0");
                            startActivity(myintent);
                            break;
                        case 2: // for most reaed
                            Intent intent = new Intent(getApplicationContext(), MainActivity.class);
                            intent.putExtra("NewsIDAr", "-1");
                            startActivity(intent);
                            break;
                    }
                } catch (Exception e) {
                    e.printStackTrace();
                }
            }
        });
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.menu_settings_app, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        // Handle action bar item clicks here. The action bar will
        // automatically handle clicks on the Home/Up button, so long
        // as you specify a parent activity in AndroidManifest.xml.
        int id = item.getItemId();

        //noinspection SimplifiableIfStatement
        if (id == R.id.goback) { // stoped
           // Intent intent=new Intent(this,MainActivity.class);
            //startActivity(intent);
            this.finish();
        }

        return super.onOptionsItemSelected(item);
    }

    // adapter for song list
    private class MyCustomAdapter extends BaseAdapter {


        public MyCustomAdapter() {

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

            final   SettingItem s = fullsongpath.get(position);

            if((position==0) || (position==4)){

                View myView = mInflater.inflate(R.layout.setting_item_header, null);
                TextView textView = (TextView) myView.findViewById(R.id.textView);
                textView.setText(s.Name);
                return myView;
            }
            else if(position==5){
                View myView = mInflater.inflate(R.layout.setting_item_alert, null);
                TextView textView = (TextView) myView.findViewById(R.id.textView);
                textView.setText(s.Name);
                ImageView img=(ImageView)myView.findViewById(R.id.imgchannel);
                img.setImageResource(s.ImageURL);
                final Switch swNotify=(Switch)myView.findViewById(R.id.switch1);
                swNotify.setChecked( ServiceNotification.EnableNootification==1?true:false);
                swNotify.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
                    @Override
                    public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
                        ServiceNotification.EnableNootification = isChecked == true ? 1 : 0;
                        FileLoad fl = new FileLoad(getApplicationContext());
                        fl.SaveData();
                    }
                });
                return myView;
            }
            else if(position==6){
                View myView = mInflater.inflate(R.layout.setting_item_font_size, null);
                TextView tvTextDisplay = (TextView) myView.findViewById(R.id.tvTextDisplay);
                tvTextDisplay.setText(s.Name);
                ImageView img=(ImageView)myView.findViewById(R.id.imgchannel);
                img.setImageResource(s.ImageURL);
                final   TextView sbDisplayValue = (TextView) myView.findViewById(R.id.sbDisplayValue);
                sbDisplayValue.setText( GlobalClass.FontSize +" px");
                final SeekBar sbFontSize=(SeekBar) myView.findViewById(R.id.sbFontSize);
                sbFontSize.setProgress(GlobalClass.FontSize-12);
                sbFontSize.setOnSeekBarChangeListener(new SeekBar.OnSeekBarChangeListener() {
                    @Override
                    public void onProgressChanged(SeekBar seekBar, int progress, boolean fromUser) {
                        GlobalClass.FontSize=progress+12;
                        sbDisplayValue.setText( GlobalClass.FontSize +"px");
                    }

                    @Override
                    public void onStartTrackingTouch(SeekBar seekBar) {

                    }

                    @Override
                    public void onStopTrackingTouch(SeekBar seekBar) {
                        FileLoad fileSave=new FileLoad(getApplicationContext());
                        fileSave.SaveData();
                    }
                });
                return myView;
            }
            else
            {
                View myView = mInflater.inflate(R.layout.settingitem, null);
            TextView textView = (TextView) myView.findViewById(R.id.textView);
            textView.setText(s.Name);
            ImageView img=(ImageView)myView.findViewById(R.id.imgchannel);
           img.setImageResource(s.ImageURL);
            return myView;}
        }
    }
}
