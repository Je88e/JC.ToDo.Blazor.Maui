using Microsoft.AspNetCore.Components;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.Model.PageModel
{
    public class LinkItem
    {
        public string Key { get; set; }

        public OneOf<string, RenderFragment> Title { get; set; }

        public string Href { get; set; }

        public bool BlankTarget { get; set; }
    }
}
