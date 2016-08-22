using DBManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.ServiceModel.Syndication;
using System.Net;
using System.IO;
using HtmlAgilityPack;
using System.Web.Script.Serialization;
using System.Xml.Linq;
using System.Drawing;

namespace WSNews
{
    public class Twitter
    {
        public string OAuthConsumerSecret = "Your_OAuthConsumerSecret";
        public string OAuthConsumerKey = "Your_OAuthConsumerKey";
        DBConnection DBop = new DBConnection();
        string accessToken = null;
        public Twitter() // inixilize witter api
        {
        GetAccessToken();
        }
        public async Task<string> GetTwitts(string userName, int count, DateTime LastGet, int SubNesourceID, string TagContent, string ImageTag)
        {
            //if (accessToken == null)
            //{
            //    accessToken = await GetAccessToken();
            //}
            try
            {

         
            var requestUserTimeline = new HttpRequestMessage(HttpMethod.Get, string.Format("https://api.twitter.com/1.1/statuses/user_timeline.json?count={0}&screen_name={1}", count, userName));//"https://api.twitter.com/1.1/search/tweets.json?q=%&count=10&geocode=37.781157,-122.398720,1mi"); //
            requestUserTimeline.Headers.Add("Authorization", "Bearer " + accessToken);
            var httpClient = new HttpClient();
            HttpResponseMessage responseUserTimeLine = await httpClient.SendAsync(requestUserTimeline);
            var serializer = new JavaScriptSerializer();
            dynamic json = serializer.Deserialize<object>(await responseUserTimeLine.Content.ReadAsStringAsync());
            var enumerableTwitts = (json as IEnumerable<dynamic>);

            if (enumerableTwitts == null)
            {
                return null;
            }
            // List<PostInfo> ls = new List<PostInfo>();
            List<string> listoflinks = new List<string>();
            string twitterlink = "";
            string Articlelink = "";
            string ArticleData = "";
            string SummaryContent = "";
            string summary = "";

            
            foreach (dynamic t in enumerableTwitts.Reverse())
            {
                try
                {
                    string Post_url = "";
                    string host = "";
                    string mytempImageURL = "";
                    try
                    {
                        dynamic media = t["entities"]["urls"];

                        Post_url = media[0]["url"].ToString();
                    
                    }
                    catch (Exception ex) { }
                    /// 


                    string dateStr = t["created_at"].ToString();
                    // DateTime PostDate = DateTime.ParseExact(dateStr, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                     DateTime PostDate = DateTime.ParseExact(dateStr, "ddd MMM dd HH:mm:ss zzzz yyyy", CultureInfo.InvariantCulture);
                  //  DateTime PostDate = Convert.ToDateTime(dateStr);
                    if ((PostDate > LastGet) &&(PostDate<=DateTime.Now))
                    {
                        try  // get news details
                        {
                            if (Post_url.Length > 0 && TagContent.Length > 0)
                            {
                               // SummaryContent = await ReadTextFromUrl(Post_url, TagContent,2);
                                //if (SummaryContent.Length > 0)
                                //    summary = SummaryContent;
                                String LoadURL = Post_url;
                                if (Post_url.IndexOf("http") < 0)
                                    LoadURL = "http://" + Post_url;
                                string PageContain = "";

                                string PageFull;
                                try
                                {

                                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(LoadURL);
                                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                                    if (response.StatusCode == HttpStatusCode.OK)
                                    {
                                        LoadURL = response.ResponseUri.ToString();
                                        host = response.ResponseUri.Host.ToString();

                                        // get page content====================
                                        HtmlWeb web = new HtmlWeb();
                                        HtmlAgilityPack.HtmlDocument doc = web.Load(LoadURL);
                                        //doc.LoadHtml(PageContainhtml); 
                                        HtmlNode rateNode = doc.DocumentNode.SelectSingleNode(TagContent);
                                        PageContain = rateNode.InnerHtml;
                                        PageFull = "";
                                        foreach (HtmlAgilityPack.HtmlNode node2 in doc.DocumentNode.SelectNodes(TagContent))
                                            PageFull = PageFull + "<p>" + node2.InnerHtml + "</p>";
                                        if (PageFull.Length > 0)
                                            PageContain = PageFull;
                                        if (PageContain.Length > 0)
                                            summary = PageContain;

                                        // get image==============================
                                        // get image
                                        if (ImageTag.Length > 0)
                                        {

                                            if (host.IndexOf("http") < 0)
                                                host = "http://" + host;
                                            HtmlNode rateNodeimage = doc.DocumentNode.SelectSingleNode(ImageTag);
                                            string tempimageURL;
                                            foreach (Match m in Regex.Matches(rateNodeimage.InnerHtml, "<img.+?src=[\"'](.+?)[\"'].+?>", RegexOptions.IgnoreCase | RegexOptions.Multiline))
                                            {
                                                tempimageURL = m.Groups[1].Value;
                                                if (tempimageURL.IndexOf("http") < 0)
                                                    tempimageURL = host + m.Groups[1].Value;
                                                // لget only the picture with  big size
                                                try
                                                {
                                                    System.Net.WebRequest request1 = System.Net.WebRequest.Create(tempimageURL);
                                                    System.Net.WebResponse response1 = request1.GetResponse();
                                                    System.IO.Stream responseStream = response1.GetResponseStream();
                                                    Bitmap bitmap2 = new Bitmap(responseStream);
                                                    var imageHeight = bitmap2.Height;
                                                    var imageWidth = bitmap2.Width;
                                                    // string value = attribute.Value.ToLower();
                                                    if ((bitmap2.Width > 100) && (bitmap2.Height > 100) && (!tempimageURL.EndsWith(".gif")))
                                                        if ((mytempImageURL.Length == 0))
                                                        {

                                                            mytempImageURL = tempimageURL;
                                                            break;
                                                        }
                                                }
                                                catch (Exception ex) { }
                                            }
                                            //et viedos
                                            // get image in cause they cannot get in direct
                                            if (mytempImageURL.Length == 0)
                                            {
                                                foreach (HtmlNode img in doc.DocumentNode.SelectNodes(ImageTag))
                                                {
                                                    mytempImageURL = img.GetAttributeValue("src", null);
                                                    if (mytempImageURL.IndexOf("http") < 0)
                                                        mytempImageURL = host + mytempImageURL;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                { }
                            }
                        }
                        catch (Exception ex) { }

                        // extract the links for text
                        ArticleData = t["text"].ToString();
                        try
                        {
                            Regex urlRx = new Regex(@"((https?|ftp|file)\://|www.)[A-Za-z0-9\.\-]+(/[A-Za-z0-9\?\&\=;\+!'\(\)\*\-\._~%]*)*", RegexOptions.IgnoreCase);

                            MatchCollection matches = urlRx.Matches(ArticleData);
                            foreach (Match match in matches)
                            {
                                listoflinks.Add(match.Value);
                                twitterlink = match.Value; // link in twitter will update
                                if (listoflinks.Count > 1)
                                    Articlelink = listoflinks.ElementAt(0);// het article link
                                    ArticleData = ArticleData.Remove(ArticleData.IndexOf(twitterlink), twitterlink.Length);
                                 
                            }
                        }
                        catch (Exception ex) { }
                        ColoumnParam[] Coloumns = new ColoumnParam[6];
                        Coloumns[0] = new ColoumnParam("NewsTitle", ColoumnType.NVarChar, ArticleData);
                        Coloumns[1] = new ColoumnParam("NewsDate", ColoumnType.DateTime, PostDate);
                        Coloumns[2] = new ColoumnParam("ReadFromWebsiteLink", ColoumnType.NVarChar, Post_url);
                        Coloumns[3] = new ColoumnParam("SubNesourceID", ColoumnType.Int, SubNesourceID);
                        Coloumns[4] = new ColoumnParam("NewsDetails", ColoumnType.NVarChar, summary);
                        Coloumns[5] = new ColoumnParam("DateIN", ColoumnType.DateTime, DateTime.Now.ToString());
                        if (DBop.cobject.InsertRow("News", Coloumns))
                        {
                            DataTable dataTable = new DataTable();
                            dataTable = DBop.cobject.SelectDataSet("News", "NewsID", "SubNesourceID=" + SubNesourceID + "  and NewsDate ='" + PostDate + "'").Tables[0];
                            if ((dataTable != null) && (dataTable.Rows.Count > 0))
                            {
                                string media_url = "";
                                try
                                {
                                    dynamic media = t["entities"]["media"];

                                    media_url = media[0]["media_url_https"].ToString();
 
                                }
                                catch (Exception ex) {
                                    media_url = "";
                                    if (mytempImageURL.Length > 0)// we looked for image
                                    {
                                        media_url = mytempImageURL;
                                     
                                    }
                                }
                                if (media_url.Length > 0) { 
                                ColoumnParam[] Coloumns1 = new ColoumnParam[2];
                                Coloumns1[0] = new ColoumnParam("NewsID", ColoumnType.Int, Convert.ToString(dataTable.Rows[0]["NewsID"]));
                                Coloumns1[1] = new ColoumnParam("NewsLink", ColoumnType.NVarChar, media_url);
                                DBop.cobject.InsertRow("NewsImages", Coloumns1);}
                                //
                            }
                        }
                       // Console.Clear();
                        twitterlink = "";
                        Articlelink = "";
                        ArticleData = "";
                        listoflinks.Clear();
                       // Console.WriteLine("Last New Readed was at :" + PostDate.ToString());
                    }
                }
                catch (Exception ex) { }
                //ls.Add(new PostInfo(t["text"].ToString(), PostDate));
            }
   }catch(Exception ex8){}
            return null;

        }

        public async void GetAccessToken()
        {
            var httpClient = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.twitter.com/oauth2/token ");
            var customerInfo = Convert.ToBase64String(new UTF8Encoding().GetBytes(OAuthConsumerKey + ":" + OAuthConsumerSecret));
            request.Headers.Add("Authorization", "Basic " + customerInfo);
            request.Content = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");

            HttpResponseMessage response = await httpClient.SendAsync(request);

            string json = await response.Content.ReadAsStringAsync();
            var serializer = new JavaScriptSerializer();
            dynamic item = serializer.Deserialize<object>(json);
            accessToken= item["access_token"];
        }

        async Task<string> ReadRSS(string url, DateTime LastGet, int SubNesourceID, string TagContent, string ImageTag)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.DtdProcessing = DtdProcessing.Ignore;
            try
            {

            using (XmlReader reader = XmlReader.Create(url, settings))
            {
                SyndicationFeed feed = SyndicationFeed.Load(reader);
                string link;
                string summary = "";
                string SummaryContent = "";
                foreach (SyndicationItem item in feed.Items.Reverse())
                {
                    try
                    {
                       string dateStr = item.PublishDate.ToString("ddd MMM dd HH:mm:ss zzzz yyyy");
                       // // DateTime PostDate = DateTime.ParseExact(dateStr, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                       DateTime PostDate = DateTime.ParseExact(dateStr, "ddd MMM dd HH:mm:ss zzzz yyyy", CultureInfo.InvariantCulture);
                      // DateTime PostDate = Convert.ToDateTime(item.PublishDate.ToString());
                      if ((PostDate > LastGet) )
                        {
                            link = item.Links[0].Uri.ToString();
                            string ImageURL = "";
                            try
                            { summary = item.Summary.Text;
                            if (item.Links.Count > 1)
                            {
                                string value = item.Links[item.Links.Count - 1].Uri.ToString().ToLower();
                                if (value.StartsWith("http://") && (value.EndsWith(".jpg") || value.EndsWith(".jpeg") || value.EndsWith(".png") || value.EndsWith(".gif")))
                                    ImageURL = item.Links[item.Links.Count - 1].Uri.ToString();
                            }
                                // read the attach images
                                foreach (SyndicationElementExtension extension in item.ElementExtensions)
                                {
                                    XElement element = extension.GetObject<XElement>();
                                    if (element.HasAttributes)
                                        foreach (var attribute in element.Attributes())
                                        {
                                            string value = attribute.Value.ToLower();
                                            if (value.StartsWith("http://") && (value.EndsWith(".jpg") || value.EndsWith(".png") || value.EndsWith(".gif")))
                                            {
                                                ImageURL = value; // Add here the image link to some array
                                                break; //get ne filesonly
                                            }
                                        }
                                }
                            }
                            catch (Exception ex) { summary = ""; }

                            try  // get news details
                            {
                                if (link.Length > 0)
                                {
                                    //  SummaryContent = await ReadTextFromUrl(link, TagContent);
                                    // if (SummaryContent.Length > 0)
                                    //   summary = SummaryContent;
                                    try
                                    {
                                        string PageContain = "";
                                        String PageFull = "";
                                        HtmlWeb web = new HtmlWeb();
                                        HtmlAgilityPack.HtmlDocument doc = web.Load(link);
                                        //doc.LoadHtml(PageContainhtml);
                                        string host = web.ResponseUri.Host.ToString();
                                        if (host.IndexOf("http") < 0)
                                            host = "http://" + host;
                                        if (TagContent.Length > 0)
                                        {  /// get one node data
                                            try
                                            {
                                                HtmlNode rateNode = doc.DocumentNode.SelectSingleNode(TagContent);
                                                PageContain = rateNode.InnerHtml;

                                                //get all div content
                                                //   var  query = doc.DocumentNode.SelectNodes(TagContent);
                                                //   PageContain = query.ToString();
                                                // get all nodes tags data
                                                PageFull = "";
                                                foreach (HtmlAgilityPack.HtmlNode node2 in doc.DocumentNode.SelectNodes(TagContent))
                                                    PageFull = PageFull + "<p>" + node2.InnerHtml + "</p>";
                                                if (PageFull.Length > 0)
                                                    PageContain = PageFull;

                                                if (PageContain.Length > 0)
                                                    summary = PageContain;
                                            }
                                            catch (Exception ex) { }

                                        }

                                        if (ImageTag.Length > 0)
                                        {
                                            HtmlNode rateNodeimage = doc.DocumentNode.SelectSingleNode(ImageTag);
                                            string tempimageURL;
                                            foreach (Match m in Regex.Matches(rateNodeimage.InnerHtml, "<img.+?src=[\"'](.+?)[\"'].+?>", RegexOptions.IgnoreCase | RegexOptions.Multiline))
                                            {
                                                tempimageURL = m.Groups[1].Value;
                                                if (tempimageURL.IndexOf("http") < 0)
                                                    tempimageURL = host + m.Groups[1].Value;
                                                // لget only the picture with  big size
                                                System.Net.WebRequest request = System.Net.WebRequest.Create(tempimageURL);
                                                System.Net.WebResponse response = request.GetResponse();
                                                System.IO.Stream responseStream = response.GetResponseStream();
                                                Bitmap bitmap2 = new Bitmap(responseStream);
                                                var imageHeight = bitmap2.Height;
                                                var imageWidth = bitmap2.Width;
                                                // string value = attribute.Value.ToLower();
                                                if ((bitmap2.Width > 100) && (bitmap2.Height > 100)&& (!tempimageURL.EndsWith(".gif")))
                                                    if ((ImageURL.Length == 0))
                                                    {

                                                        ImageURL = tempimageURL;
                                                        break;
                                                    }
                                            }

                                            // get image in cause they cannot get in direct
                                            if (ImageURL.Length == 0)
                                            {
                                                foreach (HtmlNode img in doc.DocumentNode.SelectNodes(ImageTag))
                                                {
                                                    ImageURL = img.GetAttributeValue("src", null);
                                                    if (ImageURL.IndexOf("http") < 0)
                                                        ImageURL = host + ImageURL;
                                                    break;
                                                }
                                            }
                                            if (ImageURL.Length == 0) { 
                                            // get the viedos url
                                            List<string> list = new List<string>();
                                            Regex urlRx = new Regex(@"(http://youtu|https://youtu|https://www.youtu|http://www.youtu)[A-Za-z0-9\.\-]+(/[A-Za-z0-9\?\&\=;\+!'\(\)\*\-\._~%]*)*", RegexOptions.IgnoreCase);
                                            string ViedoURL = "";
                                            MatchCollection matches = urlRx.Matches(rateNodeimage.InnerHtml);
                                            foreach (Match match in matches)
                                            {
                                                ViedoURL = match.Value;
                                                try
                                                {
                                                    string[] sp = ViedoURL.Split('&');
                                                    if (sp.Length > 1)
                                                        ViedoURL = sp[0];
                                                    string[] spid = ViedoURL.Split('/');
                                                    if (spid.Length > 1)
                                                        ViedoURL ="http://www.youtube.com/embed/"+ spid[spid.Length - 1];
                                                    break;
                                                }
                                                catch (Exception ex) { }
                                                list.Add(ViedoURL);
                                            }
                                            if (list.Count > 0)
                                                ImageURL = list[0];

                                             
                                                if (list.Count == 0)
                                                {// get mp4
                                                    
                                                    Regex urlRx1 = new Regex(@"(http://|https://)[A-Za-z0-9\.\-]+(/[A-Za-z0-9\?\&\=;\+!'\(\)\*\-\._~%]*)*(.mp4|.MP4)", RegexOptions.IgnoreCase);
                                                    ViedoURL = "";
                                                     
                                                    MatchCollection matches1 = urlRx1.Matches(rateNodeimage.InnerHtml);
                                                    if (matches1.Count > 0)
                                                    {
                                                        list.Clear();
                                                        foreach (Match match in matches1)
                                                        {
                                                            ViedoURL = match.Value;

                                                            list.Add(ViedoURL);
                                                            break;
                                                        }
                                                        if (list.Count > 0)
                                                            ImageURL = list[0];
                                                    }
                                                }

                                                /////in cuase use video tag
                                                tempimageURL = "";
                                                if (list.Count == 0)
                                                {
                                                    foreach (Match m in Regex.Matches(rateNodeimage.InnerHtml, "<iframe.+?src=[\"'](.+?)[\"'].+?>", RegexOptions.IgnoreCase | RegexOptions.Multiline))
                                                    {
                                                        tempimageURL = m.Groups[1].Value;
                                                        break;
                                                    }
                                                    if (tempimageURL.Length > 0)
                                                        ImageURL = tempimageURL;
                                                }
                                        }
                                        }
                                    }
                                    catch (Exception ex7) { }
                                }
                            }
                            catch (Exception ex) { }

                            ColoumnParam[] Coloumns = new ColoumnParam[6];
                            Coloumns[0] = new ColoumnParam("NewsTitle", ColoumnType.NVarChar, item.Title.Text);
                            Coloumns[1] = new ColoumnParam("NewsDate", ColoumnType.DateTime, PostDate);
                            Coloumns[2] = new ColoumnParam("ReadFromWebsiteLink", ColoumnType.NVarChar, link);
                            Coloumns[3] = new ColoumnParam("SubNesourceID", ColoumnType.Int, SubNesourceID);
                            Coloumns[4] = new ColoumnParam("NewsDetails", ColoumnType.NVarChar, summary);
                            Coloumns[5] = new ColoumnParam("DateIN", ColoumnType.DateTime, DateTime.Now.ToString());
                            if (DBop.cobject.InsertRow("News", Coloumns))
                            {
                                DataTable dataTable = new DataTable();
                                dataTable = DBop.cobject.SelectDataSet("News", "NewsID", "SubNesourceID=" + SubNesourceID + "  and NewsDate ='" + PostDate + "'").Tables[0];
                                if ((dataTable != null) && (dataTable.Rows.Count > 0))
                                {
                                   

                                    if (ImageURL.Length > 0)// get image from content
                                    {
                                        ColoumnParam[] Coloumns1 = new ColoumnParam[2];
                                        Coloumns1[0] = new ColoumnParam("NewsID", ColoumnType.Int, Convert.ToString(dataTable.Rows[0]["NewsID"]));
                                        Coloumns1[1] = new ColoumnParam("NewsLink", ColoumnType.NVarChar, ImageURL);

                                        DBop.cobject.InsertRow("NewsImages", Coloumns1);
                                    }
                                }
                            }
                            //Console.Clear();
                           // Console.WriteLine("Last New Readed was at :" + PostDate.ToString());
                        }
                    }
                    catch (Exception xexp)
                    {
                        // fix those datetime nodes with exceptions and read again.
                        try
                        {
                            /// old version of RSS
                            XmlDocument rssXmlDoc = new XmlDocument();

                            // Load the RSS file from the RSS URL
                            rssXmlDoc.Load(url);

                            // Parse the Items in the RSS file
                            XmlNodeList rssNodes = rssXmlDoc.SelectNodes("rss/channel/item");

                            StringBuilder rssContent = new StringBuilder();
                             
                            string title;
                            string description;
                     
                            string pubdate;
                            string ImageURL = "";
                            // Iterate through the items in the RSS file
                            foreach (XmlNode rssNode in rssNodes)
                            {
                               
                                XmlNode rssSubNode = rssNode.SelectSingleNode("title");
                                title = rssSubNode != null ? rssSubNode.InnerText : "";

                                rssSubNode = rssNode.SelectSingleNode("link");
                                link = rssSubNode != null ? rssSubNode.InnerText : "";

                                rssSubNode = rssNode.SelectSingleNode("description");
                                description = rssSubNode != null ? rssSubNode.InnerText : "";
                                rssSubNode = rssNode.SelectSingleNode("pubDate");
                                pubdate = rssSubNode != null ? rssSubNode.InnerText.ToString() : "";

                                try
                                {
                                    // string dateStr = rssSubNode.InnerText.ToString("ddd MMM dd HH:mm:ss zzzz yyyy");
                                     //DateTime PostDate = Convert.ToDateTime(pubdate);// DateTime.ParseExact(pubdate, "ddd MMM dd HH:mm:ss zzzz yyyy", null);
                                    // DateTime date = DateTime.ParseExact(pubdate, "dd/MM/yyyy", null);
                                    // string dateTime = parsedDateTime.ToString("ddd MMM dd HH:mm:ss zzzz yyyy");
                                    // DateTime dateStr = DateTime.ParseExact(pubdate, "ddd MMM dd HH:mm:ss zzzz yyyy", null );
                                    // DateTime PostDate = DateTime.ParseExact(pubdate, "ddd MMM dd HH:mm:ss zzzz yyyy", CultureInfo.InvariantCulture);
                                   // string dateStr = item.PublishDate.ToString("ddd MMM dd HH:mm:ss zzzz yyyy");
                                    // DateTime PostDate = DateTime.ParseExact(dateStr, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                                    DateTime PostDate = DateTime.ParseExact(pubdate, "ddd MMM dd HH:mm:ss zzzz yyyy", CultureInfo.InvariantCulture);
                    
                                    if ((PostDate > LastGet)  )
                                    {

                                        ImageURL = "";
                                        summary = description;
                                        if (link.Length > 0)
                                        {
                                            //  SummaryContent = await ReadTextFromUrl(link, TagContent);
                                            // if (SummaryContent.Length > 0)
                                            //   summary = SummaryContent;
                                            try
                                            {
                                                string PageContain = "";
                                                String PageFull = "";
                                                HtmlWeb web = new HtmlWeb();
                                                HtmlAgilityPack.HtmlDocument doc = web.Load(link);
                                                //doc.LoadHtml(PageContainhtml);
                                                string host = web.ResponseUri.Host.ToString();
                                                if (host.IndexOf("http") < 0)
                                                    host = "http://" + host;
                                                if (TagContent.Length > 0)
                                                {  /// get one node data
                                                    try
                                                    {
                                                        HtmlNode rateNode = doc.DocumentNode.SelectSingleNode(TagContent);
                                                        PageContain = rateNode.InnerHtml;

                                                        //get all div content
                                                        //   var  query = doc.DocumentNode.SelectNodes(TagContent);
                                                        //   PageContain = query.ToString();
                                                        // get all nodes tags data
                                                        PageFull = "";
                                                        foreach (HtmlAgilityPack.HtmlNode node2 in doc.DocumentNode.SelectNodes(TagContent))
                                                            PageFull = PageFull + "<p>" + node2.InnerHtml + "</p>"; ;
                                                        if (PageFull.Length > 0)
                                                            PageContain = PageFull;

                                                        if (PageContain.Length > 0)
                                                            summary = PageContain;
                                                    }
                                                    catch (Exception e34x) { }

                                                }

                                                if (ImageTag.Length > 0)
                                                {
                                                    HtmlNode rateNodeimage = doc.DocumentNode.SelectSingleNode(ImageTag);
                                                    string tempimageURL;
                                                    foreach (Match m in Regex.Matches(rateNodeimage.InnerHtml, "<img.+?src=[\"'](.+?)[\"'].+?>", RegexOptions.IgnoreCase | RegexOptions.Multiline))
                                                    {
                                                        tempimageURL = m.Groups[1].Value;
                                                        if (tempimageURL.IndexOf("http") < 0)
                                                            tempimageURL = host + m.Groups[1].Value;
                                                        // لget only the picture with  big size
                                                        System.Net.WebRequest request = System.Net.WebRequest.Create(tempimageURL);
                                                        System.Net.WebResponse response = request.GetResponse();
                                                        System.IO.Stream responseStream = response.GetResponseStream();
                                                        Bitmap bitmap2 = new Bitmap(responseStream);
                                                        var imageHeight = bitmap2.Height;
                                                        var imageWidth = bitmap2.Width;
                                                        // string value = attribute.Value.ToLower();
                                                        if ((bitmap2.Width > 100) && (bitmap2.Height > 100) && (!tempimageURL.EndsWith(".gif")))
                                                            if ((ImageURL.Length == 0))
                                                            {

                                                                ImageURL = tempimageURL;
                                                                break;
                                                            }
                                                    }

                                                    // get image in cause they cannot get in direct
                                                    if (ImageURL.Length == 0)
                                                    {
                                                        foreach (HtmlNode img in doc.DocumentNode.SelectNodes(ImageTag))
                                                        {
                                                            ImageURL = img.GetAttributeValue("src", null);
                                                            if (ImageURL.IndexOf("http") < 0)
                                                                ImageURL = host + ImageURL;
                                                            break;
                                                        }
                                                    }
                                                    //et viedos
                                                }

                                                ColoumnParam[] Coloumns = new ColoumnParam[6];
                                                Coloumns[0] = new ColoumnParam("NewsTitle", ColoumnType.NVarChar, title);
                                                Coloumns[1] = new ColoumnParam("NewsDate", ColoumnType.DateTime, PostDate);
                                                Coloumns[2] = new ColoumnParam("ReadFromWebsiteLink", ColoumnType.NVarChar, link);
                                                Coloumns[3] = new ColoumnParam("SubNesourceID", ColoumnType.Int, SubNesourceID);
                                                Coloumns[4] = new ColoumnParam("NewsDetails", ColoumnType.NVarChar, summary);
                                                Coloumns[5] = new ColoumnParam("DateIN", ColoumnType.DateTime, DateTime.Now.ToString());
                                                if (DBop.cobject.InsertRow("News", Coloumns))
                                                {
                                                    DataTable dataTable = new DataTable();
                                                    dataTable = DBop.cobject.SelectDataSet("News", "NewsID", "SubNesourceID=" + SubNesourceID + "  and NewsDate ='" + PostDate + "'").Tables[0];
                                                    if ((dataTable != null) && (dataTable.Rows.Count > 0))
                                                    {


                                                        if (ImageURL.Length > 0)// get image from content
                                                        {
                                                            ColoumnParam[] Coloumns1 = new ColoumnParam[2];
                                                            Coloumns1[0] = new ColoumnParam("NewsID", ColoumnType.Int, Convert.ToString(dataTable.Rows[0]["NewsID"]));
                                                            Coloumns1[1] = new ColoumnParam("NewsLink", ColoumnType.NVarChar, ImageURL);

                                                            DBop.cobject.InsertRow("NewsImages", Coloumns1);
                                                        }
                                                    }
                                                }
                                                //Console.Clear();
                                                // Console.WriteLine("Last New Readed was at :" + PostDate.ToString());

                                            }
                                            catch (Exception xexssp)
                                            {
                                                // fix those datetime nodes with exceptions and read again.
                                            }
                                        }
                                    }
                                }
                                catch (Exception exerror) { }

                            }




                        }

                        catch (Exception exerror) { }
                    }
                }
            }

            }
            catch (Exception ex) { }
            return null;
        }
        public async void Startlisten(string url, DateTime LastGet, int SubNesourceID, int MethodID, string TagContent, string ImageTag="")
        {
            if (MethodID== (int) SubResourcesType.RSS )

                await ReadRSS(url, LastGet, SubNesourceID, TagContent, ImageTag);
            else if (MethodID == (int)SubResourcesType.Twitter)
                await GetTwitts(url, 10, LastGet, SubNesourceID, TagContent, ImageTag);

        }

       

        public  async Task<string> ReadTextFromUrl(string url,string TagName, int type=1)
        {
            String LoadURL = url;
            if (url.IndexOf("http") < 0)
                LoadURL = "http://" + url;
            string PageContain = "";
         
            string PageFull;
            try
            {
                if (type == (int)SubResourcesType.Twitter) // chnge twitter url tonormal url
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(LoadURL);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        LoadURL = response.ResponseUri.ToString();
                    }
                }
                
                    HtmlWeb web = new HtmlWeb();
                    HtmlAgilityPack.HtmlDocument doc = web.Load(LoadURL);
                    //doc.LoadHtml(PageContainhtml);
                    HtmlNode rateNode = doc.DocumentNode.SelectSingleNode(TagName);
                    PageContain = rateNode.InnerHtml;
                    PageFull = "";
                    foreach (HtmlAgilityPack.HtmlNode node2 in doc.DocumentNode.SelectNodes(TagName))
                        PageFull = PageFull + node2.InnerHtml;
                    if (PageFull.Length > 0)
                        PageContain = PageFull;
              
            
                  }
                catch (Exception ex)
                { }
            return PageContain;
        }
    }
}