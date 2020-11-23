using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LanguageCourses.ViewModels.FilterViewModels
{
    public class CoursesFilterViewModel
    {
        [Display(Name = "Name")]
        public string Name { get; set; }
        public decimal? Cost { get; set; }
        public int? NumberOfhouse { get; set; }
        public string TrainingProgramm { get; set; }
        public decimal? FromCost { get; set; }
        public decimal? ToCost { get; set; }

    }
}
