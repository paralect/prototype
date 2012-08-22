using System.Collections.Generic;
using Prototype.Views;

namespace Prototype.Web.Models
{
    public class SubjectViewModel
    {
        public List<SiteView> Sites { get; set; }
        public SubjectView SubjectView { get; set; } 
    }

    public class SubjectPageViewModel
    {
        public List<SubjectView> Subjects { get; set; }
    }
}