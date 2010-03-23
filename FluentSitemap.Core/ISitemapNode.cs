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
}
