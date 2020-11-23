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
    public class ListenersController : Controller
    {
        private readonly LanguageCourseContext _context;
        private readonly CacheService _cache;
        private int pageSize = 10;
        private const string filterKey = "listeners";

        public ListenersController(LanguageCourseContext context, CacheService cache)
        {
            _context = context;
            _cache = cache;
        }

        // GET: Listeners
        public async Task<IActionResult> Index(SortState sortState = SortState.ListinersNameAsc, int page = 1)
        {
            ListenersFilterViewModel filter = HttpContext.Session.Get<ListenersFilterViewModel>(filterKey);
            if (filter == null)
            {
                filter = new ListenersFilterViewModel
                {
                    Name = string.Empty,
                    Age = null,              
                    FromDate = null,
                    ToDate = null
                };
                HttpContext.Session.Set(filterKey, filter);
            }

            string key = $"{typeof(Listener).Name}-{page}-{sortState}-{filter.Name}-{filter.Age}-{filter.FromDate}-{filter.ToDate}";

            if (!_cache.TryGetValue(key, out ListinerViewModel model))
            {
                model = new ListinerViewModel();

                IQueryable<Listener> listeners = GetSortedListeners(sortState, filter.Name, filter.Age, filter.FromDate, filter.ToDate);

                int count = listeners.Count();

                model.PageViewModel = new PageViewModel(page, count, pageSize);

                model.Listeners = count == 0
                    ? new List<Listener>()
                    : listeners.Skip((model.PageViewModel.PageIndex - 1) * pageSize).Take(pageSize).ToList();
                model.SortViewModel = new SortViewModel(sortState);
                model.ListenersFilterViewModel = filter;

                _cache.Set(key, model);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(ListenersFilterViewModel filterModel, int page)
        {
            ListenersFilterViewModel filter = HttpContext.Session.Get<ListenersFilterViewModel>(filterKey);
            if (filter != null)
            {
                filter.Name = filterModel.Name;
                filter.Age = filterModel.Age;
                filter.FromDate = filterModel.FromDate;
                filter.ToDate = filterModel.ToDate;

                HttpContext.Session.Remove(filterKey);
                HttpContext.Session.Set(filterKey, filter);
            }

            return RedirectToAction("Index", new { page });
        }

        private IQueryable<Listener> GetSortedListeners(SortState sortState, string name, int? age, DateTime? fromDate, DateTime? toDate )
        {
            IQueryable<Listener> listeners = _context.Listeners.Include(p => p.Payments).ThenInclude(c => c.Course).AsQueryable();

            switch (sortState)
            {
                case SortState.ListinersNameAsc:
                    listeners = listeners.OrderBy(x => x.Name);
                    break;
                case SortState.ListinersNameDesc:
                    listeners = listeners.OrderByDescending(x => x.Name);
                    break;
                case SortState.ListinersSurnameAsc:
                    listeners = listeners.OrderBy(x => x.Surname);
                    break;
                case SortState.ListinersSurnameDesc:
                    listeners = listeners.OrderByDescending(x => x.Surname);
                    break;
                case SortState.ListinersMiddleNameAsc:
                    listeners = listeners.OrderBy(x => x.MiddleName);
                    break;
                case SortState.ListinersMiddleNameDesc:
                    listeners = listeners.OrderByDescending(x => x.MiddleName);
                    break;
                case SortState.ListinersDateOfBirthAsc:
                    listeners = listeners.OrderBy(x => x.DateOfBirth);
                    break;
                case SortState.ListinersDateOfBirthDesc:
                    listeners = listeners.OrderByDescending(x => x.DateOfBirth);
                    break;
                case SortState.ListinersAddressAsc:
                    listeners = listeners.OrderBy(x => x.Address);
                    break;
                case SortState.ListinersAddressDesc:
                    listeners = listeners.OrderByDescending(x => x.Address);
                    break;
                case SortState.ListinersPhoneAsc:
                    listeners = listeners.OrderBy(x => x.Phone);
                    break;
                case SortState.ListinersPhoneDesc:
                    listeners = listeners.OrderByDescending(x => x.Phone);
                    break;
                case SortState.ListinersPassportDataAsc:
                    listeners = listeners.OrderBy(x => x.PassportData);
                    break;
                case SortState.ListinersPassportDataDesc:
                    listeners = listeners.OrderByDescending(x => x.PassportData);
                    break;
            }

            if (!string.IsNullOrEmpty(name))
            {
                listeners = listeners.Where(x => x.Surname.Contains(name)).AsQueryable();
            }else if (age != null  && fromDate != null && toDate != null)
            {
                listeners = listeners.Where(x => 2020 - x.DateOfBirth.Value.Year == age  && x.DateOfBirth > fromDate && x.DateOfBirth < toDate);
            }

            return listeners;
        }

        // GET: Listeners/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listener = await _context.Listeners.Include(p => p.Payments).ThenInclude(c => c.Course)
                .FirstOrDefaultAsync(m => m.ListenerId == id);
            if (listener == null)
            {
                return NotFound();
            }

            return View(listener);
        }

        // GET: Listeners/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Listeners/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ListenerId,Name,Surname,MiddleName,DateOfBirth,Address,Phone,PassportData")] Listener listener)
        {
            if (ModelState.IsValid)
            {
                _context.Add(listener);
                await _context.SaveChangesAsync();
                _cache.Clean();
                return RedirectToAction(nameof(Index));
            }
            return View(listener);
        }

        // GET: Listeners/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listener = await _context.Listeners.FindAsync(id);
            if (listener == null)
            {
                return NotFound();
            }
            return View(listener);
        }

        // POST: Listeners/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ListenerId,Name,Surname,MiddleName,DateOfBirth,Address,Phone,PassportData")] Listener listener)
        {
            if (id != listener.ListenerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(listener);
                    await _context.SaveChangesAsync();
                    _cache.Clean();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ListenerExists(listener.ListenerId))
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
            return View(listener);
        }

        // GET: Listeners/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listener = await _context.Listeners
                .FirstOrDefaultAsync(m => m.ListenerId == id);
            if (listener == null)
            {
                return NotFound();
            }

            return View(listener);
        }

        // POST: Listeners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var listener = await _context.Listeners.FindAsync(id);
            _context.Listeners.Remove(listener);
            await _context.SaveChangesAsync();
            _cache.Clean();
            return RedirectToAction(nameof(Index));
        }

        private bool ListenerExists(int id)
        {
            return _context.Listeners.Any(e => e.ListenerId == id);
        }
    }
}
