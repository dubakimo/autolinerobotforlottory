using HtmlAgilityPack;
using System;
using System.Web ;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ThreadingTimer = System.Threading.Timer;
using TimersTimer = System.Timers.Timer;
using System.Collections.Specialized;
using WebKit;

namespace AutoBetRobot
{
    public partial class Form1 : Form
    {
        //程式開始
        ThreadingTimer _ThreadTimer = null;
        TimersTimer _TimersTimer = null;
        //WebKitBrowser web = new WebKitBrowser();
        public Form1()
        {
            InitializeComponent();
        }

        //internal static List<HtmlElement> getElementsByTagAndClassName(this HtmlDocument doc, string tag = "", string className = "")
        //{
        //    List<HtmlElement> lst = new List<HtmlElement>();
        //    bool empty_tag = String.IsNullOrEmpty(tag);
        //    bool empty_cn = String.IsNullOrEmpty(className);
        //    if (empty_tag && empty_cn) return lst;
        //    HtmlElementCollection elmts = empty_tag ? doc.All : doc.GetElementsByTagName(tag);
        //    if (empty_cn)
        //    {
        //        lst.AddRange(elmts.Cast<HtmlElement>());
        //        return lst;
        //    }
        //    for (int i = 0; i < elmts.Count; i++)
        //    {
        //        if (elmts[i].GetAttribute("className") == className)
        //        {
        //            lst.Add(elmts[i]);
        //        }
        //    }
        //    return lst;
        //}

        private void Form1_Load(object sender, EventArgs e)
        {
            //var loginAddress = "www.mywebsite.com/login";
            //var loginData = new NameValueCollection
            //{
            //  { "username", "shimmy" },
            //  { "password", "mypassword" }
            //};

            //var client = new CookieAwareWebClient();
            //client.Login(loginAddress, loginData);

            web.Navigate("http://www.b1122.com/main");
            //webBrowser1.Navigate("http://www.b1122.com/main");
            

            //縮小為80%
            //webBrowser1.Document.Body.Style = "zoom:30%;";
            //自動登入
            //System.Windows.Forms.HtmlDocument HtmlDocument = this.webBrowser1.Document;
            //HtmlElement HE_username = HtmlDocument.GetElementById("username");
            //HE_username.InnerText = "arthurtu";
            //HtmlElement HE_password = HtmlDocument.GetElementById("password");
            //HE_password.InnerText = "773824";
            //foreach (HtmlElement form in webBrowser1.Document.Forms)
            //    form.InvokeMember("submit");
            //Thread t = Thread.CurrentThread;
            //bool IsThreadPool = t.IsThreadPoolThread;
            //bool IsBackground = t.IsBackground;
            //t.Name = "Main Thread";

            //this._TimersTimer = new TimersTimer();
            //this._TimersTimer.Interval = 9000;
            //this._TimersTimer.Elapsed += new System.Timers.ElapsedEventHandler(_TimersTimer_Elapsed);
            //this._TimersTimer.Start();
           
        }

        void _TimersTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //Thread t = Thread.CurrentThread;
            //bool IsThreadPool = t.IsThreadPoolThread;
            //bool IsBackground = t.IsBackground;
            //web.Navigate("http://www.b1122.com/member/index");
            //WebKit.DOM.Document HtmlDocument = web.Document;
            //MessageBox.Show("go to timer!");
            //try
            //{
            //    List<HtmlElement> drawnumber = Utils.getElementsByTagAndClassName(HtmlDocument,"div", "draw_number"); // all div's with "error" class
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message.ToString());
            //}
         
            
        }

      

        //文件載入完成
        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            ////縮小為80%
            //webBrowser1.Document.Body.Style = "zoom:30%;";
            ////自動登入
            //System.Windows.Forms.HtmlDocument HtmlDocument = this.webBrowser1.Document;
            //HtmlElement HE_username = HtmlDocument.GetElementById("username");
            //HE_username.InnerText = "arthurtu";
            //HtmlElement HE_password = HtmlDocument.GetElementById("password");
            //HE_password.InnerText = "773824";
            //foreach (HtmlElement form in webBrowser1.Document.Forms)
            // form.InvokeMember("submit");

            //FrmPriceGrid fpg = new FrmPriceGrid();
            //fpg.Show();

          
            //HtmlElementCollection links = webBrowser1.Document.GetElementsByTagName("A");

            //foreach (HtmlElement link in links)
            //{
            //    if (link.InnerText.Equals("同意"))
            //        link.InvokeMember("Click");
            //}
            //foreach (HtmlElement link2 in webBrowser1.Document.Links)
            //    link2.InvokeMember("Click");     
        }

        private void button1_Click(object sender, EventArgs e)
        {
            web = new WebKitBrowser();
            this.web.Navigate("http://www.b1122.com/member/index");
            //while (!web.IsBusy)
            //{
            //    Application.DoEvents();
            //}

            WebKit.DOM.Document htmDoc = web.Document;

            WebKit.DOM.Element rsinfo = htmDoc.GetElementById("result_info");

            try
            {
                //List<HtmlElement> drawnumber = Utils.getElementsByTagAndClassName(HtmlDocument, "div", "draw_number"); // all div's with "error" class
                lblMessage.Text = rsinfo.FirstChild + "||"+ rsinfo.TextContent  ;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        
    }

    public class CookieAwareWebClient : WebClient
    {
        public void Login(string loginPageAddress, NameValueCollection loginData)
        {
            CookieContainer container;

            var request = (HttpWebRequest)WebRequest.Create(loginPageAddress);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            var buffer = Encoding.ASCII.GetBytes(loginData.ToString());
            request.ContentLength = buffer.Length;
            var requestStream = request.GetRequestStream();
            requestStream.Write(buffer, 0, buffer.Length);
            requestStream.Close();

            container = request.CookieContainer = new CookieContainer();

            var response = request.GetResponse();
            response.Close();
            CookieContainer = container;
        }

        public CookieAwareWebClient(CookieContainer container)
        {
            CookieContainer = container;
        }

        public CookieAwareWebClient()
            : this(new CookieContainer())
        { }

        public CookieContainer CookieContainer { get; private set; }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = (HttpWebRequest)base.GetWebRequest(address);
            request.CookieContainer = CookieContainer;
            return request;
        }
    }

    internal static class Utils
    {
        internal static List<HtmlElement> getElementsByTagAndClassName(this System.Windows.Forms.HtmlDocument doc, string tag = "", string className = "")
        {
            List<HtmlElement> lst = new List<HtmlElement>();
            bool empty_tag = String.IsNullOrEmpty(tag);
            bool empty_cn = String.IsNullOrEmpty(className);
            if (empty_tag && empty_cn) return lst;
            HtmlElementCollection elmts = empty_tag ? doc.All : doc.GetElementsByTagName(tag);
            if (empty_cn)
            {
                lst.AddRange(elmts.Cast<HtmlElement>());
                return lst;
            }
            for (int i = 0; i < elmts.Count; i++)
            {
                if (elmts[i].GetAttribute("className") == className)
                {
                    lst.Add(elmts[i]);
                }
            }
            return lst;
        }
    }

    public class BrowserSession
    {
        private bool _isPost;
        private HtmlAgilityPack.HtmlDocument _htmlDoc;

        /// <summary>
        /// System.Net.CookieCollection. Provides a collection container for instances of Cookie class 
        /// </summary>
        public CookieCollection Cookies { get; set; }

        /// <summary>
        /// Provide a key-value-pair collection of form elements 
        /// </summary>
        public FormElementCollection FormElements { get; set; }

        /// <summary>
        /// Makes a HTTP GET request to the given URL
        /// </summary>
        public string Get(string url)
        {
            _isPost = false;
            CreateWebRequestObject().Load(url);
            return _htmlDoc.DocumentNode.InnerHtml;
        }

        /// <summary>
        /// Makes a HTTP POST request to the given URL
        /// </summary>
        public string Post(string url)
        {
            _isPost = true;
            CreateWebRequestObject().Load(url, "POST");
            return _htmlDoc.DocumentNode.InnerHtml;
        }

        /// <summary>
        /// Creates the HtmlWeb object and initializes all event handlers. 
        /// </summary>
        private HtmlWeb CreateWebRequestObject()
        {
            HtmlWeb web = new HtmlWeb();
            web.UseCookies = true;
            web.PreRequest = new HtmlWeb.PreRequestHandler(OnPreRequest);
            web.PostResponse = new HtmlWeb.PostResponseHandler(OnAfterResponse);
            web.PreHandleDocument = new HtmlWeb.PreHandleDocumentHandler(OnPreHandleDocument);
            return web;
        }

        /// <summary>
        /// Event handler for HtmlWeb.PreRequestHandler. Occurs before an HTTP request is executed.
        /// </summary>
        protected bool OnPreRequest(HttpWebRequest request)
        {
            AddCookiesTo(request);               // Add cookies that were saved from previous requests
            if (_isPost) AddPostDataTo(request); // We only need to add post data on a POST request
            return true;
        }

        /// <summary>
        /// Event handler for HtmlWeb.PostResponseHandler. Occurs after a HTTP response is received
        /// </summary>
        protected void OnAfterResponse(HttpWebRequest request, HttpWebResponse response)
        {
            SaveCookiesFrom(response); // Save cookies for subsequent requests
        }

        /// <summary>
        /// Event handler for HtmlWeb.PreHandleDocumentHandler. Occurs before a HTML document is handled
        /// </summary>
        protected void OnPreHandleDocument(HtmlAgilityPack.HtmlDocument document)
        {
            SaveHtmlDocument(document);
        }

        /// <summary>
        /// Assembles the Post data and attaches to the request object
        /// </summary>
        private void AddPostDataTo(HttpWebRequest request)
        {
            string payload = FormElements.AssemblePostPayload();
            byte[] buff = Encoding.UTF8.GetBytes(payload.ToCharArray());
            request.ContentLength = buff.Length;
            request.ContentType = "application/x-www-form-urlencoded";
            System.IO.Stream reqStream = request.GetRequestStream();
            reqStream.Write(buff, 0, buff.Length);
        }

        /// <summary>
        /// Add cookies to the request object
        /// </summary>
        private void AddCookiesTo(HttpWebRequest request)
        {
            if (Cookies != null && Cookies.Count > 0)
            {
                request.CookieContainer.Add(Cookies);
            }
        }

        /// <summary>
        /// Saves cookies from the response object to the local CookieCollection object
        /// </summary>
        private void SaveCookiesFrom(HttpWebResponse response)
        {
            if (response.Cookies.Count > 0)
            {
                if (Cookies == null) Cookies = new CookieCollection();
                Cookies.Add(response.Cookies);
            }
        }

        /// <summary>
        /// Saves the form elements collection by parsing the HTML document
        /// </summary>
        private void SaveHtmlDocument(HtmlAgilityPack.HtmlDocument document)
        {
            _htmlDoc = document;
            FormElements = new FormElementCollection(_htmlDoc);
        }
    }
    /// <summary>
    /// Represents a combined list and collection of Form Elements.
    /// </summary>
    public class FormElementCollection : Dictionary<string, string>
    {
        /// <summary>
        /// Constructor. Parses the HtmlDocument to get all form input elements. 
        /// </summary>
        public FormElementCollection(HtmlAgilityPack.HtmlDocument htmlDoc)
        {
            var inputs = htmlDoc.DocumentNode.Descendants("input");
            foreach (var element in inputs)
            {
                string name = element.GetAttributeValue("name", "undefined");
                string value = element.GetAttributeValue("value", "");
                if (!name.Equals("undefined")) Add(name, value);
            }
        }

        /// <summary>
        /// Assembles all form elements and values to POST. Also html encodes the values.  
        /// </summary>
        public string AssemblePostPayload()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var element in this)
            {
                string value = Uri.EscapeDataString(element.Value);
                sb.Append("&" + element.Key + "=" + value);
            }
            return sb.ToString().Substring(1);
        }
    }
}
