using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace FluentSitemap.Core
{
    public interface ISitemapNode
    {
        string Location { get; set; }
        DateTime? LastModified { get; set; }
        ChangeFrequencyType? ChangeFrequency { get; set; }
        double? Priority { get; set; }

        ISitemapNode WithLocation(string Location);
        ISitemapNode WithLastModified(DateTime? lastModified);
        ISitemapNode WithChangeFrequency(ChangeFrequencyType? changeFrequency);
        ISitemapNode WithPriority(double? priority);
        ISitemap Set(ISitemapNode changes);
        ISitemap Set();
    }

    public enum ChangeFrequencyType
    {
        Always, Hourly, Daily, Weekly, Monthly, Yearly, Never
    }
}
