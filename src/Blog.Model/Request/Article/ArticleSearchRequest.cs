﻿namespace Blog.Model.Request.Article
{
    public class ArticleSearchRequest : GridParams
    {
        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public bool? IsPublished { get; set; }
    }
}