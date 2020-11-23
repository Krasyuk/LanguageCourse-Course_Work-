using System;
using System.Collections.Generic;

#nullable disable

namespace LanguageCourses.Models
{
    public partial class Listener
    {
        public Listener()
        {
            Payments = new HashSet<Payment>();
        }

        public int ListenerId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string MiddleName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string PassportData { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }
    }
}
