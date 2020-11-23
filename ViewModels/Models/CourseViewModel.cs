using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LanguageCourses.Models;
using LanguageCourses.ViewModels.FilterViewModels;

namespace LanguageCourses.ViewModels.Models
{
    public class CourseViewModel
    {
        public IEnumerable<Course> Courses { get; set; }
        public Course Course { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public SortViewModel SortViewModel { get; set; }
        public CoursesFilterViewModel CoursesFilterViewModel { get; set; }
    }
}
