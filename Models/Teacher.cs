using System;
using System.Collections.Generic;

#nullable disable

namespace LanguageCourses.Models
{
    public partial class Teacher
    {
        public Teacher()
        {
            Courses = new HashSet<Course>();
        }

        public int TeacherId { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public string MiddleName { get; set; }
        public string Position { get; set; }
        public string Education { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
    }
}
