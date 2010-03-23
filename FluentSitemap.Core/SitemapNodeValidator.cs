﻿// Copyright 2010 Aqua Bird Consulting (http://www.aquabirdconsulting.com)
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