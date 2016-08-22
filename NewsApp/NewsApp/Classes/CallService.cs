using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace NewsApp.Classes
{
    public class CallService
    {
       // public static WindowsIdentity ident;
     
        public void SignalRConnectionRecovery()
        {
            string Tagname = "";
            DBConnection DBop = new DBConnection();
            



                try
                {

                    DataTable dataTable = new DataTable();
                    //password = Myenc.GetMD5Data(Encoding.Default.GetBytes(password));
                    dataTable = DBop.cobject.SelectDataSet("SubResourcesLastNews", "*").Tables[0];
                    if ((dataTable != null) && (dataTable.Rows.Count > 0))
                    {
                        for (int i = 0; i < dataTable.Rows.Count; i++)
                        {


                            try
                            {
                                Tagname = Convert.ToString(dataTable.Rows[i]["DetailsConatinTag"]);
                            }
                            catch (Exception ex) { Tagname = ""; }
                            //  twitter.StartThread();
                            try
                            {
                                // Thread thread;
                                if (Convert.ToString(dataTable.Rows[i]["SubResourcesTypeID"]).Equals("1"))
                                {
                                    try
                                    {
                                        // Task t1 = Task.Run(() =>
                                        //{
                                        //     
                                        BackgrundTask twitter = new BackgrundTask();
                                        twitter.Startlisten(Convert.ToString(dataTable.Rows[i]["SubNesourceLink"]), Convert.ToDateTime(dataTable.Rows[i]["NewsDate"]), Convert.ToInt32(dataTable.Rows[i]["SubNesourceID"]), 1, Tagname);


                                        // });
                                        //t1.Wait();

                                    }
                                    catch (Exception ex)
                                    {
                                        // log errors
                                    }
                                }
                                else if (Convert.ToString(dataTable.Rows[i]["SubResourcesTypeID"]).Equals("2"))
                                {
                                    try
                                    {
                                        //Task t2 = Task.Run(() =>
                                        // {
                                        BackgrundTask twitterq = new BackgrundTask();
                                        // twitterq.Startlisten(Convert.ToString(dataTable.Rows[i]["SubNesourceLink"]), Convert.ToDateTime(dataTable.Rows[i]["NewsDate"]), Convert.ToInt32(dataTable.Rows[i]["SubNesourceID"]), 2);
                                        twitterq.Startlisten(Convert.ToString(dataTable.Rows[i]["SubNesourceLink"]), Convert.ToDateTime(dataTable.Rows[i]["NewsDate"]), Convert.ToInt32(dataTable.Rows[i]["SubNesourceID"]), 2, Tagname);

                                        //});
                                        //  t2.Wait();
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
                    dataTable.Clone();
                }
                catch (Exception ex)
                {
                    // log errors
                }
                 
             
        }
    }
}