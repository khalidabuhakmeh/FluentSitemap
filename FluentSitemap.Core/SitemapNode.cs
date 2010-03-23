// Copyright 2010 Aqua Bird Consulting (http://www.aquabirdconsulting.com)
// 
// Licensed under the Apache License, Version 2.0 (the "License"); 
// you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at 
// 
// http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software 
// distributed under the License is distributed on an "AS IS" BASIS, 
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
// See the License for the specific language governing permissions and 
// limitations under the License.
// 
// The latest version of this file can be found at http://github.com/khalidabuhakmeh/FluentSitemap
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