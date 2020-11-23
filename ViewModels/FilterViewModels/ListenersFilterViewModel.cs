using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LanguageCourses.ViewModels.FilterViewModels
{
    public class ListenersFilterViewModel
    {
        [Display(Name = "Name")]
        public string Name { get; set; }
        public int? Age { get; set; }
        public string NameOfCourses { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
