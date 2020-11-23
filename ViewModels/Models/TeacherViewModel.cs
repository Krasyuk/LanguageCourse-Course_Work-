using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LanguageCourses.Models;
using LanguageCourses.ViewModels.FilterViewModels;

namespace LanguageCourses.ViewModels.Models
{
    public class TeacherViewModel
    {
        public IEnumerable<Teacher> Teachers { get; set; }
        public Teacher Teacher { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public SortViewModel SortViewModel { get; set; }
        public TeachersFilterViewModel TeachersFilterViewModel { get; set; }
    }
}
