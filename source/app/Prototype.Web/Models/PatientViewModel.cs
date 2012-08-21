using System.Collections.Generic;
using Prototype.Views;

namespace Prototype.Web.Models
{
    public class PatientViewModel
    {
        public PatientView PatientView { get; set; } 
    }

    public class PatientPageViewModel
    {
        public List<PatientView> PatientView { get; set; }
    }
}