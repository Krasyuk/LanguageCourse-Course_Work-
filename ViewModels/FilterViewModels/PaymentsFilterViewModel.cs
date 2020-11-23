using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LanguageCourses.ViewModels.FilterViewModels
{
    public class PaymentsFilterViewModel
    {
        [Display(Name = "Name")]
        public string Name { get; set; }
        public DateTime? Date { get; set; }
        public decimal? Sum { get; set; }
    }
}
