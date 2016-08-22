package com.example.hussienalrubaye.webview;

import android.app.AlertDialog;
import android.app.SearchManager;
import android.content.ActivityNotFoundException;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.net.Uri;
import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.view.LayoutInflater;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.view.ViewGroup;
import android.webkit.WebView;
import android.widget.BaseExpandableListAdapter;
import android.widget.ExpandableListView;
import android.widget.SearchView;
import android.widget.TextView;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
public class myresources extends AppCompatActivity {
    ExpandableListAdapter listAdapter;
    ExpandableListView expListView;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_myresources);
        if( GlobalClass.listDataHeader.size()==0) //if there is not resourcess is loaded
        {   this.finish();
        return;}

            expListView = (ExpandableListView) findViewById(R.id.expandableListView);
            // ls.setAdapter(new MyCustomAdapter());
            listAdapter = new ExpandableListAdapter(getApplicationContext(),  GlobalClass.listDataHeader,  GlobalClass.listDataChild);
            expListView.setAdapter(listAdapter);
            ExpnadList( GlobalClass.listDataHeader.size());
            expListView.setOnChildClickListener(new ExpandableListView.OnChildClickListener() {
                @Override
                public boolean onChildClick(ExpandableListView parent, View v, int groupPosition, int childPosition, long id) {
                 //    SubResourceItems s =( SubResourceItems)parent.getExpandableListAdapter().getChild(groupPosition,childPosition);
                    SubResourceItems s;
                    if(ilistDataHeader.size()==0)
                      s =  GlobalClass.listDataChild.get( GlobalClass.listDataHeader.get(groupPosition).Name).get(childPosition);
                    else //load from search reult
                    s = ilistDataChild.get( ilistDataHeader.get(groupPosition).Name).get(childPosition);


                    Intent intent = new Intent(getApplicationContext(), MainActivity.class);
                    intent.putExtra("NewsIDAr", "-2");
                    intent.putExtra("SubResourcesID", s.ID);
                    startActivity(intent);
                    return false;
                }
            });



    }
    private  void ExpnadList(int Count){
        for (int i = 0; i <Count  ; i++)
            expListView.expandGroup(i);
    }
    SearchView searchView;
    public   List<ResourceItem> ilistDataHeader= new ArrayList<ResourceItem>();
    public      HashMap<String, List<SubResourceItems>> ilistDataChild= new HashMap<String, List<SubResourceItems>>();
    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.menu_myresources, menu);

        // Associate searchable configuration with the SearchView
        SearchManager searchManager = (SearchManager) getSystemService(Context.SEARCH_SERVICE);
        searchView = (SearchView) menu.findItem(R.id.search).getActionView();
        searchView.setSearchableInfo(searchManager.getSearchableInfo(getComponentName()));
        //final Context co=this;
        searchView.setOnQueryTextListener(new SearchView.OnQueryTextListener() {
            @Override
            public boolean onQueryTextChange(String query) {
                 //Toast.makeText(getApplicationContext(), query, Toast.LENGTH_LONG).show();
              // search in the lis
                query = query.toLowerCase();

                ilistDataHeader.clear();
                ilistDataChild .clear();

                if(query.isEmpty()){
                    listAdapter = new ExpandableListAdapter(getApplicationContext(), GlobalClass. listDataHeader,  GlobalClass.listDataChild);
                    expListView.setAdapter(listAdapter); // full with data
                    ExpnadList( GlobalClass.listDataHeader.size());
                }
                else {

                    for(ResourceItem iResourceItem : GlobalClass. listDataHeader){

                        List<SubResourceItems> countryList = GlobalClass. listDataChild.get(iResourceItem.Name);
                        ArrayList< SubResourceItems> subResourceItems = new ArrayList<SubResourceItems>();
                        for(SubResourceItems mysubResourceItems: countryList){
                            if(mysubResourceItems.Name.toLowerCase().contains(query) ){
                                subResourceItems.add(new SubResourceItems(mysubResourceItems.Name,mysubResourceItems.folowers,mysubResourceItems.ID,mysubResourceItems.isfolowed,mysubResourceItems.ChannelImage));

                            }
                        }
                        if(subResourceItems.size() > 0){
                            ilistDataChild.put(iResourceItem.Name, subResourceItems);
                            ilistDataHeader.add(new ResourceItem(iResourceItem.Name, iResourceItem.ID, iResourceItem.ImageLink));

                        }
                    }
                    listAdapter = new ExpandableListAdapter(getApplicationContext(),ilistDataHeader, ilistDataChild);
                    expListView.setAdapter(listAdapter); // full with data
                    ExpnadList(ilistDataHeader.size());
                }

                return false;
            }

            @Override
            public boolean onQueryTextSubmit(String newText) {
                return false;
            }
        });
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
            // rate app
            if(FileLoad.IsRated==0) {
                DialogInterface.OnClickListener dialogClickListener = new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialog, int which) {
                        switch (which) {
                            case DialogInterface.BUTTON_POSITIVE:
                                //Yes button clicked
                                Uri uri = Uri.parse("market://details?id=" + GlobalClass.APPURL);
                                Intent goToMarket = new Intent(Intent.ACTION_VIEW, uri);
                                // To count with Play market backstack, After pressing back button,
                                // to taken back to our application, we need to add following flags to intent.
                                goToMarket.addFlags(Intent.FLAG_ACTIVITY_NO_HISTORY |
                                        Intent.FLAG_ACTIVITY_CLEAR_WHEN_TASK_RESET |
                                        Intent.FLAG_ACTIVITY_MULTIPLE_TASK);
                                try {
                                    startActivity(goToMarket);
                                } catch (ActivityNotFoundException e) {
                                    startActivity(new Intent(Intent.ACTION_VIEW,
                                            Uri.parse("http://play.google.com/store/apps/details?id=" + GlobalClass.APPURL)));
                                }
                                FileLoad.IsRated = 1;
                                FileLoad sv = new FileLoad(getApplicationContext());
                                sv.SaveData();
                                finish();
                                break;

                            case DialogInterface.BUTTON_NEGATIVE:
                                //No button clicked
                                finish();
                                break;
                        }
                    }
                };

                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                builder.setMessage(getResources().getString(R.string.rateq)).setPositiveButton("Yes", dialogClickListener)
                        .setNegativeButton("later", dialogClickListener).show();
            }
            else
           this.finish();
        }
        else if (id == R.id.myresources) {
                Intent intent= new Intent(getApplicationContext(), ResourcesName.class);
                startActivity(intent);
            this.finish();
            }

        return super.onOptionsItemSelected(item);
    }

    public class ExpandableListAdapter extends BaseExpandableListAdapter {
        private Context _context;
        private List<ResourceItem> _listDataHeader; // header titles
        // child data in format of header title, child title
        private HashMap<String, List<SubResourceItems>> _listDataChild;

        public ExpandableListAdapter(Context context, List<ResourceItem> listDataHeader, HashMap<String, List<SubResourceItems>> listChildData) {
            this._context = context;
            this._listDataHeader = listDataHeader;
            this._listDataChild = listChildData;
        }

        @Override
        public Object getChild(int groupPosition, int childPosititon) {
            return this._listDataChild.get(this._listDataHeader.get(groupPosition).Name).get(childPosititon);
        }

        @Override
        public long getChildId(int groupPosition, int childPosition) {
            return childPosition;
        }

        @Override
        public View getChildView(int groupPosition, final int childPosition, boolean isLastChild, View convertView, ViewGroup parent) {

            SubResourceItems s= (SubResourceItems) getChild(groupPosition, childPosition);

            LayoutInflater infalInflater = (LayoutInflater) this._context.getSystemService(Context.LAYOUT_INFLATER_SERVICE);

            convertView = infalInflater.inflate(R.layout.my_resources_items_group, null);

            TextView textView = (TextView) convertView.findViewById(R.id.txtnamefollowers);
            textView.setText(s.Name);
            WebView webview=(WebView)convertView.findViewById(R.id.webView2);
            String summary = "<html><body style='background:#FF0099CC;text-align:center'><img src='"+ s.ChannelImage +"' alt='Mountain View' height='40' width='40'></body></html>\n";
            webview.loadData(summary, "text/html", null);

            final  TextView txtflollower = (TextView) convertView.findViewById(R.id.txtflollower);
            txtflollower.setText(getResources().getString(R.string.numberfollowers) +" " +s.folowers);



            return convertView;
        }

        @Override
        public int getChildrenCount(int groupPosition) {
       try{
           return this._listDataChild.get(this._listDataHeader.get(groupPosition).Name).size();

       }
       catch (Exception ex){
           return  0;
       }
        }

        @Override
        public Object getGroup(int groupPosition) {
            return this._listDataHeader.get(groupPosition);
        }

        @Override
        public int getGroupCount() {
            return this._listDataHeader.size();
        }

        @Override
        public long getGroupId(int groupPosition) {
            return groupPosition;
        }

        @Override
        public View getGroupView(int groupPosition, boolean isExpanded, View convertView, ViewGroup parent) {
            ResourceItem s = (ResourceItem) getGroup(groupPosition);

            LayoutInflater infalInflater = (LayoutInflater) this._context.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
            convertView = infalInflater.inflate(R.layout.my_resources_items, null);

              TextView textView = (TextView) convertView .findViewById(R.id.textView);
            textView.setText(s.Name);
            WebView webview=(WebView)convertView .findViewById(R.id.webView3);
            String summary = "<html><body style='background:#FF0099CC;text-align:center'><img src='"+ s.ImageLink +"' alt='Mountain View' height='40' width='40'></body></html>\n";
            webview.loadData(summary, "text/html", null);
            return convertView;
        }

        @Override
        public boolean hasStableIds() {
            return false;
        }

        @Override
        public boolean isChildSelectable(int groupPosition, int childPosition) {
            return true;
        }

    }

}
