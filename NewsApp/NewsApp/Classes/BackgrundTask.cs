using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DBManager;
using System.Data;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Xml;
using System.ServiceModel.Syndication;
using HtmlAgilityPack;
using System.Net;
using System.Xml.Linq;
using System.Drawing;
using System.IO;
namespace NewsApp.Classes
{ 
  
    public class BackgrundTask
    {
        public string OAuthConsumerSecret = "8kpH1Tv7QfuJc8JuHoPikyDULKlPor5Lw797oG5FsAgiCjxMc9";
        public string OAuthConsumerKey = "O35T58PxEzh16vrHJXZgIL7wl";
        DBConnection DBop = new DBConnection();
        string accessToken = null;

        public async Task<string> GetTwitts(string userName, int count, DateTime LastGet, int SubNesourceID, string TagContent, string ImageTag)
        {
            string MessageData = "";
            if (accessToken == null)
            {
                accessToken = await GetAccessToken();
            }
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
                foreach (dynamic t in enumerableTwitts)
                {
                    HtmlAgilityPack.HtmlDocument doc;
                    string tempImageURL = "";
                    try
                    {
                        string Post_url = "";
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

                        if ((PostDate > LastGet)  )
                        {
                            try  // get news details
                            {
                                if (Post_url.Length > 0 && TagContent.Length > 0)
                                {
                                 //   SummaryContent = await ReadTextFromUrl(Post_url, TagContent, 2);
                                    ////if (SummaryContent.Length > 0)
                                       // summary = SummaryContent;
                                    String LoadURL = Post_url;
                                    if (Post_url.IndexOf("http") < 0)
                                        LoadURL = "http://" + Post_url;
                                    string PageContain = "";

                                    string PageFull;
                                    try
                                    {

                                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(LoadURL);
                                        //request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2";
                                        //request.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)";
                                        //request.Headers.Add("Cookie: YPF8827340282Jdskjhfiw_928937459182JAX666=xxx.xxx.xxx.xxx;");
                                             HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                                             string host = "";
                                             if (response.StatusCode == HttpStatusCode.OK)
                                             {
                                                 LoadURL = response.ResponseUri.ToString();
                                                 host = response.ResponseUri.Host.ToString();
                                             }
                                         //String responseString;
                                         //using (Stream stream = response.GetResponseStream())
                                         //{
                                         //    StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                                         //    responseString= reader.ReadToEnd();
                                         //}

                                        HtmlWeb web = new HtmlWeb();
                                         doc = web.Load(LoadURL);
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
                                                        if ((tempImageURL.Length == 0))
                                                        {

                                                            tempImageURL = tempimageURL;
                                                            break;
                                                        }
                                                }
                                                catch (Exception ex) { }
                                            }
                                            //et viedos
                                            // in cause he select specfic tag for img
                                            if (tempImageURL.Length == 0)
                                            {
                                                foreach (HtmlNode img in doc.DocumentNode.SelectNodes(ImageTag))
                                                {
                                                    tempImageURL = img.GetAttributeValue("src", null);
                                                    if (tempImageURL.IndexOf("http") < 0)
                                                        tempImageURL = host + tempImageURL;
                                                    break;
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
                            string ImageURL = "";
                            try
                            {
                                dynamic media = t["entities"]["media"];

                              ImageURL = media[0]["media_url_https"].ToString();
 
                            }
                            catch (Exception ex) {

                                ImageURL = tempImageURL;
                               
                            
                            
                            
                            
                            
                            
                            }
                           // ArticleData = t["text"].ToString();
                            MessageData = MessageData + "</br></hr> <h2>" + ArticleData + "</h2></br><img src='" + ImageURL + "'  style='width:304px;height:228px;'></br>" + summary +  " </br>===================</br>";
                               
                          //  return ArticleData;
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
            }
            catch (Exception ex8) { }
            return MessageData;

        }

        public async Task<string> GetAccessToken()
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
            return item["access_token"];
        }

        async Task<string> ReadRSS(string url, DateTime LastGet, int SubNesourceID, string TagContent, string ImageTag)
        {
            string MessageData = "ST";
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

                    foreach (SyndicationItem item in feed.Items)
                    {
                        try
                        {
                            string dateStr = item.PublishDate.ToString("ddd MMM dd HH:mm:ss zzzz yyyy");
                            // DateTime PostDate = DateTime.ParseExact(dateStr, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                            DateTime PostDate = DateTime.ParseExact(dateStr, "ddd MMM dd HH:mm:ss zzzz yyyy", CultureInfo.InvariantCulture);

                            if ((PostDate > LastGet)  )
                            {
                                link = item.Links[0].Uri.ToString();
                                string ImageURL = "";
                                try
                                {
                                    summary = item.Summary.Text;
                                    if (item.Links.Count > 1)
                                    {
                                        string value = item.Links[item.Links.Count - 1].Uri.ToString().ToLower();
                                        if (value.StartsWith("http://") && (value.EndsWith(".jpg") || value.EndsWith(".jpeg") || value.EndsWith(".png") || value.EndsWith(".gif")))
                                            ImageURL = item.Links[item.Links.Count - 1].Uri.ToString();
                                    }
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
                                                        PageFull = PageFull + "<p>" + node2.InnerHtml + "</p>"; ;
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
                                                    if ((bitmap2.Width > 100) && (bitmap2.Height > 100) && (!tempimageURL.EndsWith(".gif")))
                                                        if ((ImageURL.Length == 0))
                                                        {

                                                            ImageURL = tempimageURL;
                                                            break;
                                                        }
                                                }
                                                // in cause he select specfic tag for img
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
                                                //foreach (Match m in Regex.Matches(rateNodeimage.InnerHtml, "<video.+?src=[\"'](.+?)[\"'].+?>", RegexOptions.IgnoreCase | RegexOptions.Multiline))
                                                //    tempimageURL = m.Groups[1].Value;
                                                //foreach (Match m in Regex.Matches(rateNodeimage.InnerHtml, "<iframe.+?src=[\"'](.+?)[\"'].+?>", RegexOptions.IgnoreCase | RegexOptions.Multiline))
                                                //    tempimageURL = m.Groups[1].Value;
                                                //  var links = rateNodeimage.InnerHtml.Split("\t\n ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Where(s => s.StartsWith("http://www.yout") || s.StartsWith("https://youtu") || s.StartsWith("http://youtu") || s.StartsWith("https://www.yout"));
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
                                                            ViedoURL = "http://www.youtube.com/embed/" + spid[spid.Length - 1];
                                                    }
                                                    catch (Exception ex) { }
                                                    list.Add(ViedoURL);
                                                }
                                                if (list.Count > 0)
                                                    ImageURL = list[0];

                                                // mp4 viedo
                                                if (list.Count == 0)
                                                {
                                                    list.Clear();
                                                    Regex urlRx1 = new Regex(@"(http://|https://)[A-Za-z0-9\.\-]+(/[A-Za-z0-9\?\&\=;\+!'\(\)\*\-\._~%]*)*(.mp4|.MP4)", RegexOptions.IgnoreCase);
                                                    ViedoURL = "";
                                                    MatchCollection matches1 = urlRx1.Matches(rateNodeimage.InnerHtml);
                                                    foreach (Match match in matches1)
                                                    {
                                                        ViedoURL = match.Value;

                                                        list.Add(ViedoURL);
                                                    }
                                                    if (list.Count > 0)
                                                        ImageURL = list[0];
                                                }

                                                /////in cuase use video tag
                                                tempimageURL = "";
                                                if (list.Count == 0)
                                                {
                                                    foreach (Match m in Regex.Matches(rateNodeimage.InnerHtml, "<iframe.+?src=[\"'](.+?)[\"'].+?>", RegexOptions.IgnoreCase | RegexOptions.Multiline))
                                                    {
                                                        tempimageURL = m.Groups[1].Value;
                                                    }
                                                    if (tempimageURL.Length > 0)
                                                        ImageURL = tempimageURL;
                                                }
                                            }
                                        }
                                        catch (Exception ex7) { }
                                    }
                                }
                                catch (Exception ex) { }
                                string viedourl;
                                ImageURL = ImageURL.ToLower();
                                if ((ImageURL.EndsWith(".jpg") || ImageURL.EndsWith(".png") || ImageURL.EndsWith(".gif") || ImageURL.EndsWith(".jpeg")))
                                    viedourl = "<img src='" + ImageURL + "'  style='width:304px;height:228px;'>";
                                else
                                    viedourl = "<iframe width='420' height='345' src='" + ImageURL + "'></iframe>";



                                MessageData = MessageData + "</br></hr> <h2>" + item.Title.Text + "</h2></br>" + viedourl + "  </br>" + summary + " </br>===================</br>";

                                //Console.Clear();
                                // Console.WriteLine("Last New Readed was at :" + PostDate.ToString());
                            }
                        }
                        catch (Exception xexp)
                        {
                            // fix those datetime nodes with exceptions and read again.
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                try
                {
                    /// old version of RSS
                    XmlDocument rssXmlDoc = new XmlDocument();

                    // Load the RSS file from the RSS URL
                    rssXmlDoc.Load(url);

                    // Parse the Items in the RSS file
                    XmlNodeList rssNodes = rssXmlDoc.SelectNodes("rss/channel/item");

                    StringBuilder rssContent = new StringBuilder();
                    string link;
                    string summary = "";
                    string title;
                    string description;
                    string SummaryContent = "";
                    string pubdate;
                    string ImageURL = "";
                    // Iterate through the items in the RSS file
                    foreach (XmlNode rssNode in rssNodes)
                    {
                        ImageURL = "";
                        XmlNode rssSubNode = rssNode.SelectSingleNode("title");
                        title = rssSubNode != null ? rssSubNode.InnerText : "";

                        rssSubNode = rssNode.SelectSingleNode("link");
                        link = rssSubNode != null ? rssSubNode.InnerText : "";

                        rssSubNode = rssNode.SelectSingleNode("description");
                        description = rssSubNode != null ? rssSubNode.InnerText : "";
                        rssSubNode = rssNode.SelectSingleNode("pubDate");
                        pubdate = rssSubNode != null ? rssSubNode.InnerText.ToString( ) : "";

                        try
                        {
                           // string dateStr = rssSubNode.InnerText.ToString("ddd MMM dd HH:mm:ss zzzz yyyy");
                            DateTime PostDate = Convert.ToDateTime(pubdate);// DateTime.ParseExact(pubdate, "ddd MMM dd HH:mm:ss zzzz yyyy", null);
                           // DateTime date = DateTime.ParseExact(pubdate, "dd/MM/yyyy", null);
                           // string dateTime = parsedDateTime.ToString("ddd MMM dd HH:mm:ss zzzz yyyy");
                           // DateTime dateStr = DateTime.ParseExact(pubdate, "ddd MMM dd HH:mm:ss zzzz yyyy", null );
                           // DateTime PostDate = DateTime.ParseExact(pubdate, "ddd MMM dd HH:mm:ss zzzz yyyy", CultureInfo.InvariantCulture);

                            if ((PostDate > LastGet)  )
                            {

                               // MessageData = title;
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

                                        MessageData = MessageData + "</br></hr> <h2>" + title + "</h2></hr> <h2>" + "<img src='" + ImageURL + "'  style='width:304px;height:228px;'>" + "</h2></br> " + summary + " </br>===================</br>";

                                        //Console.Clear();
                                        // Console.WriteLine("Last New Readed was at :" + PostDate.ToString());

                                    }
                                    catch (Exception xexp)
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
            return MessageData;
        }
        public async Task<string> Startlisten(string url, DateTime LastGet, int SubNesourceID, int MethodID, string TagContent, string ImageTag = "")
        {
            if (MethodID == 1)

                return (await ReadRSS(url, LastGet, SubNesourceID, TagContent, ImageTag));
            else if (MethodID == 2)
                return (await GetTwitts(url, 10, LastGet, SubNesourceID, TagContent, ImageTag));
            else return null;
        }

        


        public async Task<string> ReadTextFromUrl(string url, string TagName, int type = 1)
        {
            String LoadURL = url;
            if (url.IndexOf("http") < 0)
                LoadURL = "http://" + url;
            string PageContain = "";
            // WebClient is still convenient
            // Assume UTF8, but detect BOM - could also honor response charset I suppose
            //try
            //{
            //    HtmlWeb web = new HtmlWeb();
            //    HtmlAgilityPack.HtmlDocument doc = web.Load(url);
            //    HtmlNode rateNode = doc.DocumentNode.SelectSingleNode(TagName);
            //    PageContain = rateNode.InnerHtml;//&&&&&&&&&&&&&&&&&& chnage to inner html

            //}
            //catch (Exception ex)
            //{

            //}
            try
            {
                if (type == 2) // chnge twitter url tonormal url
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



            }
            catch (Exception ex)
            { }
            return PageContain;
        }

       
    
    }
}