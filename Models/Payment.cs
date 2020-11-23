using System;
using System.Collections.Generic;

#nullable disable

namespace LanguageCourses.Models
{
    public partial class Payment
    {
        public int PaymentId { get; set; }
        public string NameOfCourses { get; set; }
        public DateTime? Date { get; set; }
        public decimal? Sum { get; set; }
        public int? ListenerId { get; set; }
        public int? CourseId { get; set; }

        public virtual Course Course { get; set; }
        public virtual Listener Listener { get; set; }
    }
}
