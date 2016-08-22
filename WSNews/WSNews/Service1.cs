using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WSNews
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }


        protected override void OnStart(string[] args)
        {

            backgroundWorker1.RunWorkerAsync();
           
        }

        protected override void OnStop()
        {
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            DBConnection DBop = new DBConnection();

            // Console.WriteLine("Thread is working");
            string Tagname = "";
            string ImageTagname = "";
            DataTable dataTable = new DataTable();
            //password = Myenc.GetMD5Data(Encoding.Default.GetBytes(password));
            
            Twitter twitter = new Twitter();
        //twitter.GetAccessToken();
            while (true)
            {
                dataTable = DBop.cobject.SelectDataSet("SubResourcesLastNews", "*").Tables[0];
                try
                {

                    if ((dataTable != null) && (dataTable.Rows.Count > 0))
                    {
                        for (int i = 0; i < dataTable.Rows.Count; i++)
                        {

                            try
                            {
                                Tagname = Convert.ToString(dataTable.Rows[i]["DetailsConatinTag"]);
                                ImageTagname = Convert.ToString(dataTable.Rows[i]["ImageTag"]);
                            }
                            catch (Exception ex) { 
                                Tagname = "";
                            ImageTagname ="";}
                            //  twitter.StartThread();
                            try
                            {
                                // Thread thread;
                                if (Convert.ToString(dataTable.Rows[i]["SubResourcesTypeID"]).Equals(SubResourcesType.RSS.ToString()))
                                {
                                    try
                                    {


                                        twitter.Startlisten(Convert.ToString(dataTable.Rows[i]["SubNesourceLink"]), Convert.ToDateTime(dataTable.Rows[i]["NewsDate"]), Convert.ToInt32(dataTable.Rows[i]["SubNesourceID"]), 1, Tagname,ImageTagname );


                                    }
                                    catch (Exception ex)
                                    {
                                        // log errors
                                    }
                                }
                                else if (Convert.ToString(dataTable.Rows[i]["SubResourcesTypeID"]).Equals(SubResourcesType.Twitter.ToString()))
                                {
                                    try
                                    {


                                        twitter.Startlisten(Convert.ToString(dataTable.Rows[i]["SubNesourceLink"]), Convert.ToDateTime(dataTable.Rows[i]["NewsDate"]), Convert.ToInt32(dataTable.Rows[i]["SubNesourceID"]), 2, Tagname,ImageTagname);

                                    }
                                    catch (Exception ex)
                                    {
                                        // log errors
                                    }
                                }

                            }

                            catch (Exception ex) { }

                        }
                    }

                }
                catch (Exception ex)
                {
                    // log errors
                }
                Thread.Sleep(300000);
            }
            dataTable.Clone();
        }
      
    }
}
