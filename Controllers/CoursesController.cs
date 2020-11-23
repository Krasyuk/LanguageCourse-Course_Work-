using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LanguageCourses.Data;
using LanguageCourses.Models;
using LanguageCourses.Services;
using LanguageCourses.ViewModels;
using LanguageCourses.ViewModels.FilterViewModels;
using Microsoft.AspNetCore.Authorization;
using HotelWebApp.Infrastructure;
using LanguageCourses.ViewModels.Models;

namespace LanguageCourses.Controllers
{
    [Authorize]
    public class CoursesController : Controller
    {
        private readonly LanguageCourseContext _context;
        private readonly CacheService _cache;
        private int pageSize = 10;
        private const string filterKey = "courses";

        public CoursesController(LanguageCourseContext context, CacheService cache)
        {
            _context = context;
            _cache = cache;
        }

        // GET: Courses
        public async Task<IActionResult> Index(SortState sortState = SortState.CoursesNameAsc, int page = 1)
        {
            CoursesFilterViewModel filter = HttpContext.Session.Get<CoursesFilterViewModel>(filterKey);
            if (filter == null)
            {
                filter = new CoursesFilterViewModel
                {
                    Name = string.Empty,
                    Cost = null,
                    NumberOfhouse = null,
                    TrainingProgramm = string.Empty,
                    FromCost = null,
                    ToCost = null
                };
                HttpContext.Session.Set(filterKey, filter);
            }

            string key = $"{typeof(Course).Name}-{page}-{sortState}-{filter.Name}-{filter.Cost}-{filter.NumberOfhouse}-{filter.TrainingProgramm}-{filter.FromCost}-{filter.ToCost}";

            if (!_cache.TryGetValue(key, out CourseViewModel model))
            {
                model = new CourseViewModel();

                IQueryable<Course> clients = GetSortedCourses(sortState, filter.Name, filter.Cost, filter.NumberOfhouse, filter.TrainingProgramm, filter.FromCost, filter.ToCost);

                int count = clients.Count();

                model.PageViewModel = new PageViewModel(page, count, pageSize);

                model.Courses = count == 0
                    ? new List<Course>()
                    : clients.Skip((model.PageViewModel.PageIndex - 1) * pageSize).Take(pageSize).ToList();
                model.SortViewModel = new SortViewModel(sortState);
                model.CoursesFilterViewModel = filter;

                _cache.Set(key, model);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(CoursesFilterViewModel filterModel, int page)
        {
            CoursesFilterViewModel filter = HttpContext.Session.Get<CoursesFilterViewModel>(filterKey);
            if (filter != null)
            {
                filter.Name = filterModel.Name;
                filter.Cost = filterModel.Cost;
                filter.NumberOfhouse = filterModel.NumberOfhouse;
                filter.TrainingProgramm = filterModel.TrainingProgramm;
                filter.FromCost = filterModel.FromCost;
                filter.ToCost = filterModel.ToCost;

                HttpContext.Session.Remove(filterKey);
                HttpContext.Session.Set(filterKey, filter);
            }

            return RedirectToAction("Index", new { page });
        }

        private IQueryable<Course> GetSortedCourses(SortState sortState, string name, decimal? Cost, int? NumberOfHouse, string TrainingProgramm, decimal? FromCost, decimal? ToCost)
        {
            IQueryable<Course> courses = _context.Courses.Include(x => x.Teacher).AsQueryable();

            switch (sortState)
            {
                case SortState.CoursesNameAsc:
                    courses = courses.OrderBy(x => x.Name);
                    break;
                case SortState.CoursesNameDesc:
                    courses = courses.OrderByDescending(x => x.Name);
                    break;
                case SortState.CoursesTrainingProgramAsc:
                    courses = courses.OrderBy(x => x.TrainingProgram);
                    break;
                case SortState.CoursesTrainingProgramDesc:
                    courses = courses.OrderByDescending(x => x.TrainingProgram);
                    break;
                case SortState.CoursesDescriptionAsc:
                    courses = courses.OrderBy(x => x.Description);
                    break;
                case SortState.CoursesDescriptionDesc:
                    courses = courses.OrderByDescending(x => x.Description);
                    break;
                case SortState.CoursesIntensityOfClassesAsc:
                    courses = courses.OrderBy(x => x.IntensityOfClasses);
                    break;
                case SortState.CoursesIntensityOfClassesDesc:
                    courses = courses.OrderByDescending(x => x.IntensityOfClasses);
                    break;
                case SortState.CoursesGroupSizeAsc:
                    courses = courses.OrderBy(x => x.GroupSize);
                    break;
                case SortState.CoursesGroupSizeDesc:
                    courses = courses.OrderByDescending(x => x.GroupSize);
                    break;
                case SortState.CoursesFreePlacesAsc:
                    courses = courses.OrderBy(x => x.FreePlaces);
                    break;
                case SortState.CoursesFreePlacesDesc:
                    courses = courses.OrderByDescending(x => x.FreePlaces);
                    break;
                case SortState.CoursesNumberOfHoursAsc:
                    courses = courses.OrderBy(x => x.NumberOfHours);
                    break;
                case SortState.CoursesNumberOfHoursDesc:
                    courses = courses.OrderByDescending(x => x.NumberOfHours);
                    break;
                case SortState.CoursesCostAsc:
                    courses = courses.OrderBy(x => x.Cost);
                    break;
                case SortState.CoursesCostDesc:
                    courses = courses.OrderByDescending(x => x.Cost);
                    break;
                case SortState.CoursesTeacherIdAsc:
                    courses = courses.OrderBy(x => x.Teacher.SurName);
                    break;
                case SortState.CoursesTeacherIdDesc:
                    courses = courses.OrderByDescending(x => x.Teacher.SurName);
                    break;
            }

            if (!string.IsNullOrEmpty(name))
            {
                courses = courses.Where(x => x.Name.Contains(name)).AsQueryable();
            }
            else if (Cost != null && NumberOfHouse != null && !string.IsNullOrEmpty(TrainingProgramm))
            {
                courses = courses.Where(x => x.Cost == Cost && x.NumberOfHours == NumberOfHouse && x.TrainingProgram.Contains(TrainingProgramm)).AsQueryable();
            }
            else if (FromCost != null && ToCost !=null)
            {
                courses = courses.Where(x => x.Cost > FromCost && x.Cost < ToCost).AsQueryable();
            }

            return courses;
        }

            // GET: Courses/Details/5
            public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.Teacher)
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "TeacherId", "SurName");
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseId,Name,TrainingProgram,Description,IntensityOfClasses,GroupSize,FreePlaces,NumberOfHours,Cost,TeacherId")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                _cache.Clean();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "TeacherId", "TeacherId", course.TeacherId);
            return View(course);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "TeacherId", "SurName", course.TeacherId);
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CourseId,Name,TrainingProgram,Description,IntensityOfClasses,GroupSize,FreePlaces,NumberOfHours,Cost,TeacherId")] Course course)
        {
            if (id != course.CourseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                    _cache.Clean();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.CourseId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "TeacherId", "TeacherId", course.TeacherId);
            return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.Teacher)
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            _cache.Clean();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.CourseId == id);
        }
    }
}
