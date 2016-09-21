using DBManager;
using NewsApp.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;

/**
 * Created by freedomseeker1981 on 8/17/16.
 */
namespace NewsApp.WebService
{
    /// <summary>
    /// Summary description for MobileWebService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class MobileWebService1 : System.Web.Services.WebService
    {

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public string GetResources()  /// get list of notes
        {

            JavaScriptSerializer ser = new JavaScriptSerializer();

            DBConnection DBop = new DBConnection();
            DataTable dataTable = new DataTable();
            string[] ResourcesName = null;
            string[] NesourceID = null;
            string[] ImageLink = null;
            // int HasNewNews = 0;
            dataTable = DBop.cobject.SelectDataSet("Resources", "ResourcesName,NesourceID,ImageLink", null, "NesourceDateAdd").Tables[0];
            if ((dataTable != null) && (dataTable.Rows.Count > 0))
            {
                ResourcesName = new string[dataTable.Rows.Count];
                NesourceID = new string[dataTable.Rows.Count];
                ImageLink = new string[dataTable.Rows.Count];
                //HasNewNews = 1;
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    ResourcesName[i] = Convert.ToString(dataTable.Rows[i]["ResourcesName"]);
                    NesourceID[i] = Convert.ToString(dataTable.Rows[i]["NesourceID"]);
                    ImageLink[i] = Convert.ToString(dataTable.Rows[i]["ImageLink"]);
                }

            }


            var jsonData = new
            {
                Tag = "Resources",
                ResourcesName = ResourcesName,
                NesourceID = NesourceID,
                ImageLink = ImageLink
            };

            return ser.Serialize(jsonData); //products.ToString();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public string GetSubNesources(int NesourceID, int UserID)  /// get list of notes
        {

            JavaScriptSerializer ser = new JavaScriptSerializer();

            DBConnection DBop = new DBConnection();
            DataTable dataTable = new DataTable();
            string[] SubResourcesName = null; // for samll size
            string[] SubNesourceID = null;
            string[] IsFollow = null;
            string[] NumberOfFollowers = null;
            string[] ChannelImage = null;
            // string[] NesourceID = null;
            // int HasNewNews = 0;
            ColProcedureParam[] Coloumns = new ColProcedureParam[2];
            Coloumns[0] = new ColProcedureParam("NesourceID", NesourceID.ToString());// Request.QueryString["id"]);// temp be 1
            Coloumns[1] = new ColProcedureParam("UserID", UserID.ToString());
            dataTable = DBop.cobject.SelectDataSetProcedureTable("GetSubNesources", Coloumns).Tables[0];
            if ((dataTable != null) && (dataTable.Rows.Count > 0))
            {
                SubResourcesName = new string[dataTable.Rows.Count];
                SubNesourceID = new string[dataTable.Rows.Count];
                IsFollow = new string[dataTable.Rows.Count];
                NumberOfFollowers = new string[dataTable.Rows.Count];
                ChannelImage = new string[dataTable.Rows.Count];
                //HasNewNews = 1;
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    //SubResourcesName[i] = "{'name': '" + Convert.ToString(dataTable.Rows[i]["SubNesourceName"]) + "'}";
                    SubResourcesName[i] = Convert.ToString(dataTable.Rows[i]["SubNesourceName"]);
                    SubNesourceID[i] = Convert.ToString(dataTable.Rows[i]["SubNesourceID"]);
                    IsFollow[i] = Convert.ToString(dataTable.Rows[i]["IsFollow"]);
                    NumberOfFollowers[i] = Convert.ToString(dataTable.Rows[i]["NumberOfFollowers"]);
                    ChannelImage[i] = Convert.ToString(dataTable.Rows[i]["ChannelImage"]);
                    
                }

            }


            var jsonData = new
            {
                Tag = "SubResources",
                ResourcesName = SubResourcesName,
                SubNesourceID = SubNesourceID,
                IsFollow = IsFollow,
                NumberOfFollowers = NumberOfFollowers,
                ChannelImage = ChannelImage
            };

            return ser.Serialize(jsonData); //products.ToString();
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public string InitializeAccount(string MAC)  /// get list of notes
        {

            JavaScriptSerializer ser = new JavaScriptSerializer();

            DBConnection DBop = new DBConnection();

            // add acount
            DataTable dataTable = new DataTable();
            Boolean AlreadyRegistered = true;
            dataTable = DBop.cobject.SelectDataSet("Users", "PhoneMac", "PhoneMac='" + MAC + "'").Tables[0];
            if ((dataTable == null) || (dataTable.Rows.Count == 0))
            {   // if the phone is not already registered
                ColoumnParam[] Coloumns = new ColoumnParam[2];
                Coloumns[0] = new ColoumnParam("DateRegister", ColoumnType.DateTime, DateTime.Now.ToString());
                Coloumns[1] = new ColoumnParam("PhoneMac", ColoumnType.varchar50, MAC);
                DBop.cobject.InsertRow("Users", Coloumns);
                AlreadyRegistered = false;
            }

            string UserID = "";



            // get user info

            dataTable = DBop.cobject.SelectDataSet("Users", "UserID", "PhoneMac='" + MAC + "'").Tables[0];
            if ((dataTable != null) && (dataTable.Rows.Count > 0))
            {
                UserID = Convert.ToString(dataTable.Rows[0]["UserID"]);
                if (AlreadyRegistered == false)
                {  // only for first time install we add him
                    ColProcedureParam[] Coloumns = new ColProcedureParam[1];
                    Coloumns[0] = new ColProcedureParam("UserID", UserID.ToString());// Request.QueryString["id"]);// temp be 1

                    try
                    {
                        DBop.cobject.SelectDataSetProcedureTable("AddDefultChannels", Coloumns);
                    }
                    catch (Exception ex) { }
                }
            }
            var jsonData = new
            {
                IsAdded = AlreadyRegistered,
                UserID = UserID
            };

            return ser.Serialize(jsonData); //products.ToString();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public string Follow2Unfallow(int SubNesourceID, int UserID, int Tag)  ///Tag =0 devele 1 is add
        {

            JavaScriptSerializer ser = new JavaScriptSerializer();

            DBConnection DBop = new DBConnection();
            int Result = 0;
            if (Tag == 1)
            {  // addfollow
                ColoumnParam[] Coloumns = new ColoumnParam[3];
                Coloumns[0] = new ColoumnParam("UserID", ColoumnType.Int, UserID);
                Coloumns[1] = new ColoumnParam("SubNesourceID", ColoumnType.Int, SubNesourceID);
                Coloumns[2] = new ColoumnParam("DateRegister", ColoumnType.DateTime, DateTime.Now.ToString());
                if (DBop.cobject.InsertRow("UserFollowing", Coloumns))
                    Result = 1;

            }
            else  // delete follow
            {
                if (DBop.cobject.DeletedRow("UserFollowing", "UserID=" + UserID + " and SubNesourceID=" + SubNesourceID))
                    Result = 1;
            }


            var jsonData = new
            {
                Result = Result,

            };

            return ser.Serialize(jsonData); //products.ToString();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public void GetOneSubNesources( int UserID, int VarSubNesourceID = 0)  /// get list of notes
        {

            JavaScriptSerializer ser = new JavaScriptSerializer();

            DBConnection DBop = new DBConnection();
            DataTable dataTable = new DataTable();
            string SubResourcesName = null; // for samll size
            string  SubNesourceID = null;
            string  IsFollow = null;
            string  NumberOfFollowers = null;
            string  ChannelImage = null;
            // string[] NesourceID = null;
            // int HasNewNews = 0;
            ColProcedureParam[] Coloumns = new ColProcedureParam[2];
             Coloumns[0] = new ColProcedureParam("UserID", UserID.ToString());
            Coloumns[1] = new ColProcedureParam("SubNesourceID", VarSubNesourceID.ToString());
            dataTable = DBop.cobject.SelectDataSetProcedureTable("GetOneSubNesources", Coloumns).Tables[0];
            if ((dataTable != null) && (dataTable.Rows.Count > 0))
            {
      
                //HasNewNews = 1;
                 
                    //SubResourcesName[i] = "{'name': '" + Convert.ToString(dataTable.Rows[i]["SubNesourceName"]) + "'}";
                    SubResourcesName  = Convert.ToString(dataTable.Rows[0]["SubNesourceName"]);
                    SubNesourceID  = Convert.ToString(dataTable.Rows[0]["SubNesourceID"]);
                    IsFollow  = Convert.ToString(dataTable.Rows[0]["IsFollow"]);
                    NumberOfFollowers  = Convert.ToString(dataTable.Rows[0]["NumberOfFollowers"]);
                    ChannelImage  = Convert.ToString(dataTable.Rows[0]["ChannelImage"]);

                

            }


            var jsonData = new
            { 
                ResourcesName = SubResourcesName,
                SubNesourceID = SubNesourceID,
                IsFollow = IsFollow,
                NumberOfFollowers = NumberOfFollowers,
                ChannelImage = ChannelImage
            };

          //  return ser.Serialize(jsonData); //products.ToString();
            HttpContext.Current.Response.Write(ser.Serialize(jsonData)); 
        }
       

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public string IsGetNewsWithHeader(string NewsID, int UserID)  /// get list of notes
        {

            JavaScriptSerializer ser = new JavaScriptSerializer();

            DBConnection DBop = new DBConnection();
            DataTable dataTable = new DataTable();
            string[] NewsTitle = null;
            string[] NewsIDAr = null;

            int HasNewNews = 0;
            //************* deprectaed it should check new update for every 45 minutes to get news to server
            //dataTable = DBop.cobject.SelectDataSet("CheckService", " DATEDIFF(minute,[CheckDate],getdate()) as minuteleft", " DATEDIFF(minute,[CheckDate],getdate())>5").Tables[0];
            //if ((dataTable != null) && (dataTable.Rows.Count > 0))
            //{ // this mean we need to call the service agin to update news
            //    // update last callservice date
            //    ColoumnParam[] Coloumns = new ColoumnParam[1];
            //    Coloumns[0] = new ColoumnParam("CheckDate", ColoumnType.DateTime, DateTime.Now.ToString());
            //    if (DBop.cobject.UpdateRow("CheckService", Coloumns, "CheckID=1"))
            //    {
            //        // call all service to bring new news
            //        //
            //        //CallService cl = new CallService();

            //        //Thread signalRConnectionRecovery1 = new Thread(cl.SignalRConnectionRecovery);
            //        //signalRConnectionRecovery1.IsBackground = true;
            //        //signalRConnectionRecovery1.Start();

            //        // MethodInvoker simpleDelegate = new MethodInvoker(Foo);

            //        // Calling Foo Async
            //        //  simpleDelegate.BeginInvoke(null, null);


            //    }
            //}


            // get last news
            ColProcedureParam[] Coloumns1 = new ColProcedureParam[4];
            Coloumns1[0] = new ColProcedureParam("UserID", Convert.ToString(UserID));// Request.QueryString["id"]);// temp be 1
            Coloumns1[1] = new ColProcedureParam("SubNesourceID", "0");
            Coloumns1[2] = new ColProcedureParam("NewsID", NewsID);
            Coloumns1[3] = new ColProcedureParam("q", "%عاجل%");
            dataTable = DBop.cobject.SelectDataSetProcedureTable("IsGetNewsWithHeader", Coloumns1).Tables[0];
            if ((dataTable != null) && (dataTable.Rows.Count > 0))
            {
                NewsTitle = new string[dataTable.Rows.Count];
                NewsIDAr = new string[dataTable.Rows.Count];

                //HasNewNews = 1;
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    NewsTitle[i] = Convert.ToString(dataTable.Rows[i]["NewsTitle"]);
                    NewsIDAr[i] = Convert.ToString(dataTable.Rows[i]["NewsID"]);
                }
                HasNewNews = 1;
            }
            var jsonData = new
            {
                HasNews = HasNewNews,
                NewsTitle = NewsTitle,
                NewsIDAr = NewsIDAr
            };

            return ser.Serialize(jsonData); //products.ToString();
        }
        public delegate void MethodInvoker();

        private void Foo()
        {
            // sleep for 10 seconds.
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

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public string GetNews(int UserID, int StratFrom, int EndTo, int SubNesourceIDvar, string NewsIDvar, string q)
        {

            JavaScriptSerializer ser = new JavaScriptSerializer();

            DBConnection DBop = new DBConnection();
            DataTable dataTable = new DataTable();
            string[] Row = null;
            string[] SubNesourceID = null;
            string[] NewsTitle = null;
            string[] PicturLink = null;
            string[] InvestmentLink = null;
            string[] NewsDateN = null;
            string[] NewsID = null;
            string[] SubNesourceName = null;
            string[] ChannelImage = null;
            int HasNewNews = 0;



            // get last news
            ColProcedureParam[] Coloumns1 = new ColProcedureParam[6];
            Coloumns1[0] = new ColProcedureParam("UserID", Convert.ToString(UserID));// Request.QueryString["id"]);// temp be 1
            Coloumns1[1] = new ColProcedureParam("SubNesourceID", Convert.ToString(SubNesourceIDvar));
            Coloumns1[2] = new ColProcedureParam("NewsID", Convert.ToString(NewsIDvar));
            Coloumns1[3] = new ColProcedureParam("q", q);
            Coloumns1[4] = new ColProcedureParam("StratFrom", Convert.ToString(StratFrom));
            Coloumns1[5] = new ColProcedureParam("EndTo", Convert.ToString(EndTo));
            dataTable = DBop.cobject.SelectDataSetProcedureTable("GetNews", Coloumns1).Tables[0];
            if ((dataTable != null) && (dataTable.Rows.Count > 0))
            {
                Row = new string[dataTable.Rows.Count];
                SubNesourceID = new string[dataTable.Rows.Count];
                NewsTitle = new string[dataTable.Rows.Count];
                PicturLink = new string[dataTable.Rows.Count];
                InvestmentLink = new string[dataTable.Rows.Count];
                NewsDateN = new string[dataTable.Rows.Count];
                NewsID = new string[dataTable.Rows.Count];
                SubNesourceName = new string[dataTable.Rows.Count];
                ChannelImage = new string[dataTable.Rows.Count];

                //HasNewNews = 1;
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {

                    Row[i] = Convert.ToString(dataTable.Rows[i]["Row"]);
                    SubNesourceID[i] = Convert.ToString(dataTable.Rows[i]["SubNesourceID"]);
                    NewsTitle[i] = Convert.ToString(dataTable.Rows[i]["NewsTitle"]);
                    PicturLink[i] = Convert.ToString(dataTable.Rows[i]["PicturLink"]);
                    InvestmentLink[i] = Convert.ToString(dataTable.Rows[i]["InvestmentLink"]); ;
                    NewsDateN[i] = Convert.ToString(dataTable.Rows[i]["NewsDateN"]);
                    NewsID[i] = Convert.ToString(dataTable.Rows[i]["NewsID"]);
                    SubNesourceName[i] = Convert.ToString(dataTable.Rows[i]["SubNesourceName"]);
                    ChannelImage[i] = Convert.ToString(dataTable.Rows[i]["ChannelImage"]);
                }
                HasNewNews = 1;
            }
            var jsonData = new
            {
                Row = Row,
                SubNesourceID = SubNesourceID,
                NewsTitle = NewsTitle,
                PicturLink = PicturLink,
                InvestmentLink = InvestmentLink,
                NewsDateN = NewsDateN,
                NewsID = NewsID,
                SubNesourceName = SubNesourceName,
                ChannelImage = ChannelImage
            };

            return ser.Serialize(jsonData); //products.ToString();
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public void GetNewsDetials(int NewsIDvar, int UserID )
        {

            JavaScriptSerializer ser = new JavaScriptSerializer();

            DBConnection DBop = new DBConnection();
            DataTable dataTable = new DataTable();
            string  Row = null;
            string  SubNesourceID = null;
            string  NewsTitle = null;
            string  PicturLink = null;
            string  InvestmentLink = null;
            string  NewsDateN = null;
            string  NewsID = null;
            string  SubNesourceName = null;
            string  ChannelImage = null;
            string  NewsDetails = null;
            int HasNewNews = 0;
            string ReadFromWebsiteLink = null;


            // get last news
            ColProcedureParam[] Coloumns1 = new ColProcedureParam[1];
            Coloumns1[0] = new ColProcedureParam("NewsID", Convert.ToString(NewsIDvar));

            dataTable = DBop.cobject.SelectDataSetProcedureTable("GetNewsDetials", Coloumns1).Tables[0];
            if ((dataTable != null) && (dataTable.Rows.Count > 0))
            { //
                 //SaveNews is readed by person
                ColoumnParam[] Coloumns = new ColoumnParam[3];
                Coloumns[0] = new ColoumnParam("NewsID", ColoumnType.Int, NewsIDvar);
                Coloumns[1] = new ColoumnParam("UserID", ColoumnType.Int, UserID);
                Coloumns[2] = new ColoumnParam("DateRead", ColoumnType.DateTime, DateTime.Now.ToString());
                DBop.cobject.InsertRow("Readers", Coloumns);

                    Row  = Convert.ToString(dataTable.Rows[0]["Row"]);
                    SubNesourceID  = Convert.ToString(dataTable.Rows[0]["SubNesourceID"]);
                    NewsTitle  = Convert.ToString(dataTable.Rows[0]["NewsTitle"]);
                    PicturLink  = Convert.ToString(dataTable.Rows[0]["PicturLink"]);
                    //InvestmentLink[i] = Convert.ToString(dataTable.Rows[i]["InvestmentLink"]); ;
                    NewsDateN = Convert.ToString(dataTable.Rows[0]["NewsDateN"]);
                    NewsID  = Convert.ToString(dataTable.Rows[0]["NewsID"]);
                    SubNesourceName  = Convert.ToString(dataTable.Rows[0]["SubNesourceName"]);
                    ChannelImage  = Convert.ToString(dataTable.Rows[0]["ChannelImage"]);
                    NewsDetails  = Convert.ToString(dataTable.Rows[0]["NewsDetails"]);
                    ReadFromWebsiteLink = Convert.ToString(dataTable.Rows[0]["ReadFromWebsiteLink"]);
                HasNewNews = 1;
            }
            var jsonData = new
            {    HasNew=HasNewNews,
                Row = Row,
                SubNesourceID = SubNesourceID,
                NewsTitle = NewsTitle,
                PicturLink = PicturLink,
                InvestmentLink = InvestmentLink,
                NewsDateN = NewsDateN,
                NewsID = NewsID,
                SubNesourceName = SubNesourceName,
                ChannelImage = ChannelImage,
                NewsDetails = NewsDetails,
                 ReadFromWebsiteLink = ReadFromWebsiteLink
            };

            //return ser.Serialize(jsonData); //products.ToString();
            HttpContext.Current.Response.Write(ser.Serialize(jsonData));
        }



        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public string GetMyResources(int UserID)  /// get list of notes
        {

            JavaScriptSerializer ser = new JavaScriptSerializer();

            DBConnection DBop = new DBConnection();
            DataTable dataTable = new DataTable();
            DataTable dataTableSubResources = new DataTable();
            string[] ResourcesName = null;
            string[] NesourceID = null;
            string[] ImageLink = null;
            // int HasNewNews = 0;
            myresources[] myresourcesn=null;
            dataTable = DBop.cobject.SelectDataSet("ResourcesForUser", "ResourcesName,NesourceID,ImageLink", "UserID=" + UserID, "ResourcesName").Tables[0];
            if ((dataTable != null) && (dataTable.Rows.Count > 0))
            {
                ResourcesName = new string[dataTable.Rows.Count];
                NesourceID = new string[dataTable.Rows.Count];
                ImageLink = new string[dataTable.Rows.Count];
                myresourcesn = new myresources[dataTable.Rows.Count];
                //HasNewNews = 1;
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                   // sub rsourcess class
                     string[] SubResourcesName = null; // for samll size
                     string[] SubNesourceID = null;
                     string[] IsFollow = null;
                     string[] NumberOfFollowers = null;
                     string[] ChannelImage = null;
                   
                 ColProcedureParam[] Coloumns = new ColProcedureParam[2];
                 Coloumns[0] = new ColProcedureParam("NesourceID", Convert.ToString(dataTable.Rows[i]["NesourceID"]));// Request.QueryString["id"]);// temp be 1
                 Coloumns[1] = new ColProcedureParam("UserID", UserID.ToString());
                 dataTableSubResources = DBop.cobject.SelectDataSetProcedureTable("GetMySubNesources", Coloumns).Tables[0];
                 if ((dataTableSubResources != null) && (dataTableSubResources.Rows.Count > 0))
                 {  // get all sub rsourcs for every resources
                     SubResourcesName = new string[dataTableSubResources.Rows.Count];
                     SubNesourceID = new string[dataTableSubResources.Rows.Count];
                     IsFollow = new string[dataTableSubResources.Rows.Count];
                     NumberOfFollowers = new string[dataTableSubResources.Rows.Count];
                     ChannelImage = new string[dataTableSubResources.Rows.Count];
                     //HasNewNews = 1;
                     for (int j = 0; j < dataTableSubResources.Rows.Count; j++)
                     {
                         //SubResourcesName[i] = "{'name': '" + Convert.ToString(dataTable.Rows[i]["SubNesourceName"]) + "'}";
                         SubResourcesName[j] = Convert.ToString(dataTableSubResources.Rows[j]["SubNesourceName"]);
                         SubNesourceID[j] = Convert.ToString(dataTableSubResources.Rows[j]["SubNesourceID"]);
                         IsFollow[j] = Convert.ToString(dataTableSubResources.Rows[j]["IsFollow"]);
                         NumberOfFollowers[j] = Convert.ToString(dataTableSubResources.Rows[j]["NumberOfFollowers"]);
                         ChannelImage[j] = Convert.ToString(dataTableSubResources.Rows[j]["ChannelImage"]);

                     }

                 }
             myresourcesn[i]=new myresources();
              myresourcesn[i].ResourcesName = Convert.ToString(dataTable.Rows[i]["ResourcesName"]);
                myresourcesn[i]. NesourceID = Convert.ToString(dataTable.Rows[i]["NesourceID"]);
                myresourcesn[i]. ImageLink = Convert.ToString(dataTable.Rows[i]["ImageLink"]);
                 myresourcesn[i].SubNesourceID = SubNesourceID;
                myresourcesn[i]. IsFollow = IsFollow;
                myresourcesn[i]. NumberOfFollowers = NumberOfFollowers;
                myresourcesn[i]. ChannelImage =   ChannelImage  ;
                myresourcesn[i].SubResourcesName = SubResourcesName;
              
                }

            }


            var r = new
            {
                imageLink = myresourcesn
            };

            return ser.Serialize( r); //products.ToString();
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public   void IsGetNews( string UserID, string NewsID, string q, string SubNesourceID)
        {
            if (q.Equals("@"))
                q = "%";
            else
                q = "%" + q.Trim() + "%";
            JavaScriptSerializer ser = new JavaScriptSerializer();
            DBConnection DBop = new DBConnection();
            // check if there is new news 
            DataTable dataTable = new DataTable();
            //password = Myenc.GetMD5Data(Encoding.Default.GetBytes(password));
            ColProcedureParam[] Coloumns = new ColProcedureParam[4];
            Coloumns[0] = new ColProcedureParam("UserID", UserID);// temp be 1
            Coloumns[1] = new ColProcedureParam("SubNesourceID", SubNesourceID);
            Coloumns[2] = new ColProcedureParam("NewsID", NewsID);
            Coloumns[3] = new ColProcedureParam("q", q);
            dataTable = DBop.cobject.SelectDataSetProcedureTable("IsGetNews", Coloumns).Tables[0];
            if ((!Convert.ToString(dataTable.Rows[0]["CountItem"]).Equals("0")))
            {
  
        var r = new
        {
            Tag = 1,
            newData = "newNews"
        };
        HttpContext.Current.Response.Write(ser.Serialize(r));
            }
            else
            {
                
                var r = new
               {
                   Tag = 0,
                 newData = "NonewNews"
               };
                HttpContext.Current.Response.Write(ser.Serialize(r));
            }


        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public void GetNewsNow(string UserID, string StratFrom, string EndTo, string SubNesourceID, string NewsID, string q, string Type)
        {
            if (q.Equals("@"))
                q = "%";
            else
                q = "%" + q.Trim() + "%";

            //definaltions 
            MyNewDetails[] myresourcesn = null;
            JavaScriptSerializer ser = new JavaScriptSerializer();
 DBConnection DBop = new DBConnection();
            // check if there is new news 
            DataTable dataTable = new DataTable();
            //password = Myenc.GetMD5Data(Encoding.Default.GetBytes(password));
            ColProcedureParam[] Coloumns = new ColProcedureParam[7];
            Coloumns[0] = new ColProcedureParam("UserID", UserID);// temp be 1
            Coloumns[1] = new ColProcedureParam("StratFrom", StratFrom);
            Coloumns[2] = new ColProcedureParam("EndTo", EndTo);
            Coloumns[3] = new ColProcedureParam("SubNesourceID", SubNesourceID);
            Coloumns[4] = new ColProcedureParam("NewsID", NewsID);
            Coloumns[5] = new ColProcedureParam("q", q);
            Coloumns[6] = new ColProcedureParam("Type", Type);
            try
            {


                dataTable = DBop.cobject.SelectDataSetProcedureTable("GetNews", Coloumns).Tables[0];
                if ((dataTable != null) && (dataTable.Rows.Count > 0))
                {
                    myresourcesn = new MyNewDetails[dataTable.Rows.Count];
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        myresourcesn[i] = new MyNewDetails();

                        myresourcesn[i].Row = Convert.ToString(dataTable.Rows[i]["Row"]);
                        myresourcesn[i].NewsTitle = Convert.ToString(dataTable.Rows[i]["NewsTitle"]);
                        myresourcesn[i].SubNesourceID = Convert.ToString(dataTable.Rows[i]["SubNesourceID"]);
                        myresourcesn[i].PicturLink = Convert.ToString(dataTable.Rows[i]["PicturLink"]);
                        myresourcesn[i].InvestmentLink = Convert.ToString(dataTable.Rows[i]["InvestmentLink"]);
                        myresourcesn[i].NewsDateN = Convert.ToString(dataTable.Rows[i]["NewsDateN"]);
                        myresourcesn[i].NewsID = Convert.ToString(dataTable.Rows[i]["NewsID"]);
                        myresourcesn[i].SubNesourceName = Convert.ToString(dataTable.Rows[i]["SubNesourceName"]);
                        myresourcesn[i].ChannelImage = Convert.ToString(dataTable.Rows[i]["ChannelImage"]);
                        myresourcesn[i].readers = Convert.ToString(dataTable.Rows[i]["readers"]);

                    }
                    var r = new
                    {
                        Tag = 1,
                        newData = myresourcesn
                    };
                    HttpContext.Current.Response.Write(ser.Serialize(r));
                }
                else
                {
                    var r = new
                    {
                        Tag = 0,  // no news
                        newData = ""
                    };
                    HttpContext.Current.Response.Write(ser.Serialize(r));
                }
            }
            catch (Exception ex)//incuase error return
            {
                var r = new
                {
                    Tag = 0,  // no news
                    newData = ""
                };
                HttpContext.Current.Response.Write(ser.Serialize(r));

            }
     
        }


        //[WebMethod]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        //public void ChannelInfo(string UserID, string NesourceID)
        //{
        //    JavaScriptSerializer ser = new JavaScriptSerializer();
        //     DBConnection DBop = new DBConnection();
        //    DataTable dataTable = new DataTable();
        //    ColProcedureParam[] Coloumns = new ColProcedureParam[2];
        //    Coloumns[0] = new ColProcedureParam("NesourceID", NesourceID);// Request.QueryString["id"]);// temp be 1
        //    Coloumns[1] = new ColProcedureParam("UserID", UserID);
        //    dataTable = DBop.cobject.SelectDataSetProcedureTable("GetSubNesources", Coloumns).Tables[0];
        //    if ((dataTable != null) && (dataTable.Rows.Count > 0))
        //    {
        //         var r = new
        //        {
        //            Tag = 1,  // no news
        //            channelname= Convert.ToString(dataTable.Rows[0]["SubNesourceName"]),
        //            followers= " عدد المتابعين " + Convert.ToString(dataTable.Rows[0]["Followers"]),
        //            channelinamge= Convert.ToString(dataTable.Rows[0]["ChannelImage"]),
        //            channelcover = Convert.ToString(dataTable.Rows[0]["BacugroundPicture"]),
        //               IsFollow  = Convert.ToString(dataTable.Rows[0]["IsFollow"])  
        //        };
        //        HttpContext.Current.Response.Write(ser.Serialize(r));  
        //    }
        //}
       
    }
}
