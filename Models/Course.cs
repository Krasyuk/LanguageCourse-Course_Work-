using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace LanguageCourses.Models
{
    public partial class Course
    {
        public Course()
        {
            Payments = new HashSet<Payment>();
        }

        public int CourseId { get; set; }
        public string Name { get; set; }
        [Display(Name = "Training program")]
        public string TrainingProgram { get; set; }
        public string Description { get; set; }
        public string IntensityOfClasses { get; set; }
        public int? GroupSize { get; set; }
        public int? FreePlaces { get; set; }
        public int? NumberOfHours { get; set; }
        public decimal? Cost { get; set; }
        public int? TeacherId { get; set; }

        public virtual Teacher Teacher { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
