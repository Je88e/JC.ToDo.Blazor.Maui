using Microsoft.AspNetCore.Components;
using OneOf;

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
