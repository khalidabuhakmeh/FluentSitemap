using System;

namespace FluentSitemap.Core
{
    public static class SitemapNodeValidator
    {
        public static void Validate(ISitemapNode node)
        {
            if (node == null) 
                throw new ArgumentNullException("node");

            if (string.IsNullOrWhiteSpace(node.Location))
                throw new ArgumentNullException("node.Location", "Location is required.");

            if (node.Priority.HasValue && (node.Priority < 0 || node.Priority > 1))
                throw new ArgumentNullException("node.Priority", "Priority must be between 0.0 and 1.0.");
        }
    }
}