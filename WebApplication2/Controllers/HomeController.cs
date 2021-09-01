using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        private readonly Cache _cache;
        public HomeController()
        {
            _cache = new Cache();
        }
        //public const string RSSURL = "https://rss.walla.co.il/feed/3";
        //public const string RSSURL_channel = "https://rss.walla.co.il/feed/4?type=main";
        protected void Set(string key, object value, DateTime? expiration, TimeSpan? sliding)
        {
            if (sliding.HasValue)
                _cache.Insert(key, value, null, Cache.NoAbsoluteExpiration, sliding.Value);
            else if (expiration.HasValue)
                _cache.Insert(key, value, null, expiration.Value, Cache.NoSlidingExpiration);
            else
                _cache.Insert(key, value);
        }
        public object Get(string key)
        {
            return _cache.Get(key);
        }

        public JsonResult GetItems(string categoryTitle, int? categoryId)
        {
            List<Channel> channelList = Get("AllChannelList") as List<Channel>;

            if (channelList == null)
            {
                channelList = ReadRssData();

            }

            var channel = channelList.Where(x => x.Title.Equals(categoryTitle)).FirstOrDefault();
            List<Item> items = channel.Item.ToList();

            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCategoriesList(string rssURL = "")
        {
            List<Channel> channelList = Get("AllChannelList") as List<Channel>;

            if (channelList == null)
            {
                channelList = ReadRssData();
            }

            var categoryList = channelList.Select(x => new { Name = x.Title }).ToList();

            return Json(categoryList, JsonRequestBehavior.AllowGet);

        }

        //private List<Channel> GetAllChannelData(string rssUrl)
        //{
        //    WebClient wclient = new WebClient();
        //    wclient.Encoding = Encoding.UTF8;
        //    ServicePointManager.Expect100Continue = true;
        //    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        //    string RSSData = wclient.DownloadString(rssUrl);

        //    XDocument xml = XDocument.Parse(RSSData);
        //}

        private List<Channel> ReadRssData()
        {
            WebClient wclient = new WebClient();
            wclient.Encoding = Encoding.UTF8;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            List<Channel> channelList = new List<Channel>();
            for (int i = 1; i < 4; i++)
            {
                string RSSData = wclient.DownloadString(string.Format("https://rss.walla.co.il/feed/{0}?type=main", i));
                XDocument xml = XDocument.Parse(RSSData);
                var channelData = (from x in xml.Descendants("rss").Descendants("channel")
                                   select new Channel
                                   {
                                       Title = ((string)x.Element("title")),
                                       //Link = x.Element("link")==null ? null : new Uri(((string)x.Element("link"))),
                                       Description = ((string)x.Element("description")),
                                       Item = (from y in x.Descendants("item")
                                               select new Item
                                               {
                                                   Title = ((string)y.Element("title")),
                                                   Link = new Uri(((string)y.Element("link"))),
                                                   Description = ((string)y.Element("description")),
                                                   PubDate = ((string)y.Element("pubDate"))
                                               }
                                               ).ToArray()
                                   }).FirstOrDefault();

                channelList.Add(channelData);

                Set("Channel" + i.ToString(), channelData, null, null);
            }

            Set("AllChannelList", channelList, null, null);

            return channelList;
        }
        public ActionResult Index()
        {

            ReadRssData();


            //XDocument xml = XDocument.Parse(RSSData);
            ////var RSSFeedData = (from x in xml.Descendants("item")
            ////                   select new Item
            ////                   {
            ////                       Title = ((string)x.Element("title")),
            ////                       Link = new Uri(((string)x.Element("link"))),
            ////                       Description = ((string)x.Element("description")),
            ////                       PubDate = ((string)x.Element("pubDate"))
            ////                   });

            ////var first = xml.Descendants("Cannel").First();

            ////XNode node = xml.Root.FirstNode;
            ////XElement RSSNodeList = xml.Descendants("channel").FirstOrDefault();

            ////var RSSFeedData = (from x in xml.Descendants("channel").Descendants("item")
            ////                   select new Item
            ////                   {
            ////                       Title = ((string)x.Element("title")),
            ////                       Link = new Uri(((string)x.Element("link"))),
            ////                       Description = ((string)x.Element("description")),
            ////                       PubDate = ((string)x.Element("pubDate"))
            ////                   });

            //var RSSFeedData = (from x in xml.Descendants("rss").Descendants("channel")
            //                   select new Channel
            //                   {
            //                       Title = ((string)x.Element("title")),
            //                       //Link = x.Element("link")==null ? null : new Uri(((string)x.Element("link"))),
            //                       Description = ((string)x.Element("description")),
            //                       Item = (from y in x.Descendants("item")
            //                               select new Item
            //                               {
            //                                   Title = ((string)y.Element("title")),
            //                                   Link = new Uri(((string)y.Element("link"))),
            //                                   Description = ((string)y.Element("description")),
            //                                   PubDate = ((string)y.Element("pubDate"))
            //                               }
            //                               ).ToArray()
            //                   }).ToList();



            //ViewBag.RSSFeed = RSSFeedData;
            ////ViewBag.URL = RSSURL;
            return View("~/Views/Home/ViewRss.cshtml");
            ////return PartialView("~/Views/Home/WallaRssView.cshtml", RSSFeedData);
            //return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}