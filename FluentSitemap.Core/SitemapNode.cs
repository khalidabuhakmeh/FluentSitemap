using System;

namespace FluentSitemap.Core
{
    public class SitemapNode : ISitemapNode
    {
        private readonly ISitemap _sitemap;

        public SitemapNode()
        {}

        public SitemapNode(ISitemap sitemap)
        {
            _sitemap = sitemap;
        }

        public string Location { get; set; }
        public DateTime? LastModified { get; set;}
        public ChangeFrequencyType? ChangeFrequency { get; set;}
        public double? Priority { get; set; }

        public ISitemapNode WithLocation(string location)
        {
            Location = location;
            return this;
        }

        public ISitemapNode WithLastModified(DateTime? lastModified)
        {
            LastModified = lastModified;
            return this;
        }

        public ISitemapNode WithChangeFrequency(ChangeFrequencyType? changeFrequency)
        {
            ChangeFrequency = changeFrequency;
            return this;
        }

        public ISitemapNode WithPriority(double? priority)
        {
            Priority = priority;
            return this;
        }

        public ISitemap Set(ISitemapNode changes)
        {
            this.WithChangeFrequency(changes.ChangeFrequency)
                .WithLastModified(changes.LastModified)
                .WithLastModified(changes.LastModified)
                .WithPriority(changes.Priority);

            return Set();
        }

        public ISitemap Set()
        {
            if (_sitemap == null)
                throw new ArgumentNullException("you attempted to set a node outside of a sitemap");

            return _sitemap;
        }
    }
}