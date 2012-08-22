using System.Collections.Generic;
using Prototype.Views;

namespace Prototype.Web.Models
{
    public class SiteViewModel
    {
        public SiteView SiteView { get; set; } 
    }

    public class SitePageViewModel
    {
        public List<SiteView> Sites { get; set; }
    }
}