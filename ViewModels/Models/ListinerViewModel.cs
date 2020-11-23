using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LanguageCourses.Models;
using LanguageCourses.ViewModels.FilterViewModels;

namespace LanguageCourses.ViewModels.Models
{
    public class ListinerViewModel
    {
        public IEnumerable<Listener> Listeners { get; set; }
        public Listener Listener { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public SortViewModel SortViewModel { get; set; }
        public ListenersFilterViewModel ListenersFilterViewModel { get; set; }

    }
}
