using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LanguageCourses.ViewModels.FilterViewModels
{
    public class TeachersFilterViewModel
    {
        [Display(Name = "Name")]
        public string Name { get; set; }
        public string Position { get; set; }
        public string Education { get; set; }

    }
}
