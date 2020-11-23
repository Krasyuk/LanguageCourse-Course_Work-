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
    public class TeachersController : Controller
    {
        private readonly LanguageCourseContext _context;
        private readonly CacheService _cache;
        private int pageSize = 10;
        private const string filterKey = "teachers";

        public TeachersController(LanguageCourseContext context, CacheService cache)
        {
            _context = context;
            _cache = cache;
        }

        // GET: Teachers
        public async Task<IActionResult> Index(SortState sortState = SortState.TeachersSurNameAsc, int page = 1)
        {
            TeachersFilterViewModel filter = HttpContext.Session.Get<TeachersFilterViewModel>(filterKey);
            if (filter == null)
            {
                filter = new TeachersFilterViewModel
                {
                    Name = string.Empty,
                    Position = string.Empty,
                    Education = string.Empty,
                                     
                };
                HttpContext.Session.Set(filterKey, filter);
            }

            string key = $"{typeof(Teacher).Name}-{page}-{sortState}-{filter.Name}-{filter.Position}-{filter.Education}";

            if (!_cache.TryGetValue(key, out TeacherViewModel model))
            {
                model = new TeacherViewModel();

                IQueryable<Teacher> teachers = GetSortedTeachers(sortState, filter.Name, filter.Position, filter.Education);

                int count = teachers.Count();

                model.PageViewModel = new PageViewModel(page, count, pageSize);

                model.Teachers = count == 0
                    ? new List<Teacher>()
                    : teachers.Skip((model.PageViewModel.PageIndex - 1) * pageSize).Take(pageSize).ToList();
                model.SortViewModel = new SortViewModel(sortState);
                model.TeachersFilterViewModel = filter;

                _cache.Set(key, model);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(TeachersFilterViewModel filterModel, int page)
        {
            TeachersFilterViewModel filter = HttpContext.Session.Get<TeachersFilterViewModel>(filterKey);
            if (filter != null)
            {
                filter.Name = filterModel.Name;
                filter.Position = filterModel.Position;
                filter.Education = filterModel.Education;

                HttpContext.Session.Remove(filterKey);
                HttpContext.Session.Set(filterKey, filter);
            }

            return RedirectToAction("Index", new { page });
        }

        private IQueryable<Teacher> GetSortedTeachers(SortState sortState, string name, string position, string education)
        {
            IQueryable<Teacher> teachers = _context.Teachers.AsQueryable();

            switch (sortState)
            {
                case SortState.TeachersNameAsc:
                    teachers = teachers.OrderBy(x => x.Name);
                    break;
                case SortState.TeachersNameDesc:
                    teachers = teachers.OrderByDescending(x => x.Name);
                    break;
                case SortState.TeachersSurNameAsc:
                    teachers = teachers.OrderBy(x => x.SurName);
                    break;
                case SortState.TeachersSurNameDesc:
                    teachers = teachers.OrderByDescending(x => x.SurName);
                    break;
                case SortState.TeachersMiddleNameAsc:
                    teachers = teachers.OrderBy(x => x.MiddleName);
                    break;
                case SortState.TeachersMiddleNameDesc:
                    teachers = teachers.OrderByDescending(x => x.MiddleName);
                    break;
            }
            

            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(position) && !string.IsNullOrEmpty(education))
            {
                teachers = teachers.Where(x => x.SurName.Contains(name) && x.Position.Contains(position) && x.Education.Contains(education)).AsQueryable();
                        
            }
            else if(!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(position))
            {
                teachers = teachers.Where(x => x.SurName.Contains(name) && x.Position.Contains(position)).AsQueryable();
            }
            else if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(education))
            {
                teachers = teachers.Where(x => x.SurName.Contains(name) && x.Education.Contains(education)).AsQueryable();
            }
            else if (!string.IsNullOrEmpty(position) && !string.IsNullOrEmpty(education))
            {
                teachers = teachers.Where(x => x.Position.Contains(position) && x.Education.Contains(education)).AsQueryable();
            }
            else if (!string.IsNullOrEmpty(name))
            {
                teachers = teachers.Where(x => x.SurName.Contains(name)).AsQueryable();
            }
            else if (!string.IsNullOrEmpty(position))
            {
                teachers = teachers.Where(x => x.Position.Contains(position)).AsQueryable();
            }
            else if (!string.IsNullOrEmpty(education))
            {
                teachers = teachers.Where(x => x.Education.Contains(education)).AsQueryable();
            }

            return teachers;
    }

        // GET: Teachers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers
                .FirstOrDefaultAsync(m => m.TeacherId == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // GET: Teachers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Teachers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TeacherId,Name,SurName,MiddleName,Position,Education")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                _context.Add(teacher);
                await _context.SaveChangesAsync();
                _cache.Clean();
                return RedirectToAction(nameof(Index));
            }
            return View(teacher);
        }

        // GET: Teachers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null)
            {
                return NotFound();
            }
            return View(teacher);
        }

        // POST: Teachers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TeacherId,Name,SurName,MiddleName,Position,Education")] Teacher teacher)
        {
            if (id != teacher.TeacherId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teacher);
                    await _context.SaveChangesAsync();
                    _cache.Clean();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeacherExists(teacher.TeacherId))
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
            return View(teacher);
        }

        // GET: Teachers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers
                .FirstOrDefaultAsync(m => m.TeacherId == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();
            _cache.Clean();
            return RedirectToAction(nameof(Index));
        }

        private bool TeacherExists(int id)
        {
            return _context.Teachers.Any(e => e.TeacherId == id);
        }
    }
}
