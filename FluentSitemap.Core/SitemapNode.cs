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
        private readonly ISitemapConfigurator _sitemapConfigurator;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <remarks>Will be disconnected from a SitemapConfigurator</remarks>
        public SitemapNode()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sitemapConfigurator">the SitemapConfigurator the node is connected to</param>
        public SitemapNode(ISitemapConfigurator sitemapConfigurator)
        {
            _sitemapConfigurator = sitemapConfigurator;
        }

        #region ISitemapNode Members

        /// <summary>
        /// The location of a node
        /// </summary>
        /// <remarks>After a <loc> tag, an URL is expected which should start with "http://".The length of the URL can be 2048 characters at most.</remarks>
        public string Location { get; set; }

        /// <summary>
        /// The date when this location was last modified 
        /// Be advised that you do not have to modify this tag each time you modify the document. The search engines will get the dates of the documents once they crawl them.
        /// </summary>
        public DateTime? LastModified { get; set; }

        /// <summary>
        /// used as a hint for the crawlers to indicate how ofter the page is modified and how often it should be indexed. 
        /// </summary>
        public ChangeFrequencyType? ChangeFrequency { get; set; }

        /// <summary>
        /// The Priority value can vary from 0.0 to 1.0. 
        /// Be advised that this indicates only your personal preferences for the way you would like to have your website indexed. 
        /// The default value of a page that is not prioritized is 0.5. Any page with higher value will be crawled before the page with priority 0.5, and all pages with lower priority will be indexed after the page with 0.5 value. 
        /// Since the priority is relative it is used only for your website and even if you set a high priority to all of your pages this does not mean that they will be indexed more often, because this value is not used to make comparison between different websites.  
        /// </summary>
        public double? Priority { get; set; }

        /// <summary>
        /// The location of a node. Must start with http:// or https://
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public ISitemapNode WithLocation(string location)
        {
            Location = location;
            return this;
        }

        /// <summary>
        /// The date when this location was last modified 
        /// Be advised that you do not have to modify this tag each time you modify the document. The search engines will get the dates of the documents once they crawl them.
        /// </summary>        
        /// <param name="lastModified"></param>
        /// <returns></returns>
        public ISitemapNode WithLastModified(DateTime? lastModified)
        {
            LastModified = lastModified;
            return this;
        }

        /// <summary>
        /// used as a hint for the crawlers to indicate how ofter the page is modified and how often it should be indexed. 
        /// </summary>
        /// <param name="changeFrequency"></param>
        /// <returns></returns>
        public ISitemapNode WithChangeFrequency(ChangeFrequencyType? changeFrequency)
        {
            ChangeFrequency = changeFrequency;
            return this;
        }

        /// <summary>
        /// The Priority value can vary from 0.0 to 1.0. 
        /// Be advised that this indicates only your personal preferences for the way you would like to have your website indexed. 
        /// The default value of a page that is not prioritized is 0.5. Any page with higher value will be crawled before the page with priority 0.5, and all pages with lower priority will be indexed after the page with 0.5 value. 
        /// Since the priority is relative it is used only for your website and even if you set a high priority to all of your pages this does not mean that they will be indexed more often, because this value is not used to make comparison between different websites.  
        /// </summary>
        /// <param name="priority"></param>
        /// <returns></returns>
        public ISitemapNode WithPriority(double? priority)
        {
            Priority = priority;
            return this;
        }

        /// <summary>
        /// Allows you to set changes all at once by using a SiteMapConfigurator node
        /// </summary>
        /// <param name="changes">the changes to be mapped</param>
        /// <returns></returns>
        public ISitemapConfigurator Set(ISitemapNode changes)
        {
            WithChangeFrequency(changes.ChangeFrequency)
                .WithLastModified(changes.LastModified)
                .WithLastModified(changes.LastModified)
                .WithPriority(changes.Priority);

            return Set();
        }

        /// <summary>
        /// Returns you back the SiteMapConfigurator so you can continue adding nodes
        /// </summary>
        /// <returns></returns>
        public ISitemapConfigurator Set()
        {
            if (_sitemapConfigurator == null)
                throw new ArgumentNullException("you attempted to set a node outside of a SitemapConfigurator");

            return _sitemapConfigurator;
        }

        #endregion
    }
}