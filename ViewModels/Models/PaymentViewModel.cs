using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LanguageCourses.Models;
using LanguageCourses.ViewModels.FilterViewModels;

namespace LanguageCourses.ViewModels.Models
{
    public class PaymentViewModel
    {
        public IEnumerable<Payment> Payments { get; set; }
        public Payment Payment { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public SortViewModel SortViewModel { get; set; }
        public PaymentsFilterViewModel PaymentsFilterViewModel { get; set; }
    }
}
