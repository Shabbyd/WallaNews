﻿using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WebApplication2.Models
{
    public class WallaRssFeed
    {
    }

    public partial class Welcome6
    {
        [JsonProperty("rss")]
        public Rss Rss { get; set; }
    }

    public partial class Rss
    {
        [JsonProperty("channel")]
        public Channel Channel { get; set; }

        [JsonProperty("_version")]
        public string Version { get; set; }
    }

    public partial class Channel
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("link")]
        public Uri Link { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("lastBuildDate")]
        public string LastBuildDate { get; set; }

        [JsonProperty("docs")]
        public Uri Docs { get; set; }

        [JsonProperty("generator")]
        public string Generator { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("image")]
        public Image Image { get; set; }

        [JsonProperty("copyright")]
        public string Copyright { get; set; }

        [JsonProperty("item")]
        public Item[] Item { get; set; }
    }

    public partial class Image
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("link")]
        public Uri Link { get; set; }
    }

    public partial class Item
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("link")]
        public Uri Link { get; set; }

        [JsonProperty("guid")]
        public Uri Guid { get; set; }

        [JsonProperty("pubDate")]
        public string PubDate { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }

    //public partial class Welcome6
    //{
    //    public static Welcome6 FromJson(string json) => JsonConvert.DeserializeObject<Welcome6>(json, CodeBeautify.Converter.Settings);
    //}

    //public static class Serialize
    //{
    //    public static string ToJson(this Welcome6 self) => JsonConvert.SerializeObject(self, CodeBeautify.Converter.Settings);
    //}

    //internal static class Converter
    //{
    //    public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
    //    {
    //        MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
    //        DateParseHandling = DateParseHandling.None,
    //        Converters =
    //        {
    //            new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
    //        },
    //    };
    //}

}