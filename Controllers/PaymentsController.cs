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
    public class PaymentsController : Controller
    {
        private readonly LanguageCourseContext _context;
        private readonly CacheService _cache;
        private int pageSize = 10;
        private const string filterKey = "payments";

        public PaymentsController(LanguageCourseContext context, CacheService cache)
        {
            _context = context;
            _cache = cache;
        }

        // GET: Payments
        public async Task<IActionResult> Index(SortState sortState = SortState.PaymentsNameOfCoursesAsc, int page = 1)
        {
            PaymentsFilterViewModel filter = HttpContext.Session.Get<PaymentsFilterViewModel>(filterKey);
            if (filter == null)
            {
                filter = new PaymentsFilterViewModel
                {
                    Name = string.Empty,
                    Date = null,
                    Sum = null                  
                };
                HttpContext.Session.Set(filterKey, filter);
            }

            string key = $"{typeof(Payment).Name}-{page}-{sortState}-{filter.Name}-{filter.Date}-{filter.Sum}";

            if (!_cache.TryGetValue(key, out PaymentViewModel model))
            {
                model = new PaymentViewModel();

                IQueryable<Payment> payments = GetSortedPayments(sortState, filter.Name, filter.Date, filter.Sum);

                int count = payments.Count();

                model.PageViewModel = new PageViewModel(page, count, pageSize);

                model.Payments = count == 0
                    ? new List<Payment>()
                    : payments.Skip((model.PageViewModel.PageIndex - 1) * pageSize).Take(pageSize).ToList();
                model.SortViewModel = new SortViewModel(sortState);
                model.PaymentsFilterViewModel = filter;

                _cache.Set(key, model);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(PaymentsFilterViewModel filterModel, int page)
        {
            PaymentsFilterViewModel filter = HttpContext.Session.Get<PaymentsFilterViewModel>(filterKey);
            if (filter != null)
            {
                filter.Name = filterModel.Name;
                filter.Date = filterModel.Date;
                filter.Sum = filterModel.Sum;

                HttpContext.Session.Remove(filterKey);
                HttpContext.Session.Set(filterKey, filter);
            }

            return RedirectToAction("Index", new { page });
        }

        private IQueryable<Payment> GetSortedPayments(SortState sortState, string name, DateTime? date, decimal? sum)
        {
            IQueryable<Payment> payments = _context.Payments.Include(x => x.Course).Include(y => y.Listener).AsQueryable();

            switch (sortState)
            {
                case SortState.PaymentsNameOfCoursesAsc:
                    payments = payments.OrderBy(x => x.NameOfCourses);
                    break;
                case SortState.PaymentsNameOfCoursesDesc:
                    payments = payments.OrderByDescending(x => x.NameOfCourses);
                    break;
                case SortState.PaymentsDateAsc:
                    payments = payments.OrderBy(x => x.Date);
                    break;
                case SortState.PaymentsDateDesc:
                    payments = payments.OrderByDescending(x => x.Date);
                    break;
                case SortState.PaymentsSumAsc:
                    payments = payments.OrderBy(x => x.Sum);
                    break;
                case SortState.PaymentsSumDesc:
                    payments = payments.OrderByDescending(x => x.Sum);
                    break;
                case SortState.PaymentsListenerIdAsc:
                    payments = payments.OrderBy(x => x.Listener.Surname);
                    break;
                case SortState.PaymentsListenerIdDesc:
                    payments = payments.OrderByDescending(x => x.Listener.Surname);
                    break;
                case SortState.PaymentsCourseIdAsc:
                    payments = payments.OrderBy(x => x.CourseId);
                    break;
                case SortState.PaymentsCourseIdDesc:
                    payments = payments.OrderByDescending(x => x.CourseId);
                    break;
            }

            if (!string.IsNullOrEmpty(name) && date != null && sum != null)
            {
                payments = payments.Where(x => x.NameOfCourses.Contains(name) && x.Date == date && x.Sum == sum).AsQueryable();
            }
            else if (!string.IsNullOrEmpty(name) && date != null)
            {
                payments = payments.Where(x => x.NameOfCourses.Contains(name) && x.Date == date).AsQueryable();
            }
            else if (!string.IsNullOrEmpty(name) && sum != null)
            {
                payments = payments.Where(x => x.NameOfCourses.Contains(name) && x.Sum == sum).AsQueryable();
            }
            else if (date != null && sum != null)
            {
                payments = payments.Where(x=> x.Date == date && x.Sum == sum).AsQueryable();
            }
            else if (date != null)
            {
                payments = payments.Where(x => x.Date == date).AsQueryable();
            }
            else if (sum != null)
            {
                payments = payments.Where(x => x.Sum == sum).AsQueryable();
            }
            else if (!string.IsNullOrEmpty(name))
            {
                payments = payments.Where(x => x.NameOfCourses.Contains(name)).AsQueryable();
            }

            return payments;
        }

        // GET: Payments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payments
                .Include(p => p.Course)
                .Include(p => p.Listener)
                .FirstOrDefaultAsync(m => m.PaymentId == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // GET: Payments/Create
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "CourseId");
            ViewData["ListenerId"] = new SelectList(_context.Listeners, "ListenerId", "ListenerId");
            return View();
        }

        // POST: Payments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PaymentId,NameOfCourses,Date,Sum,ListenerId,CourseId")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(payment);
                await _context.SaveChangesAsync();
                _cache.Clean();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "CourseId", payment.CourseId);
            ViewData["ListenerId"] = new SelectList(_context.Listeners, "ListenerId", "ListenerId", payment.ListenerId);
            return View(payment);
        }

        // GET: Payments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "CourseId", payment.CourseId);
            ViewData["ListenerId"] = new SelectList(_context.Listeners, "ListenerId", "ListenerId", payment.ListenerId);
            return View(payment);
        }

        // POST: Payments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PaymentId,NameOfCourses,Date,Sum,ListenerId,CourseId")] Payment payment)
        {
            if (id != payment.PaymentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(payment);
                    await _context.SaveChangesAsync();
                    _cache.Clean();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaymentExists(payment.PaymentId))
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
            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "CourseId", payment.CourseId);
            ViewData["ListenerId"] = new SelectList(_context.Listeners, "ListenerId", "ListenerId", payment.ListenerId);
            return View(payment);
        }

        // GET: Payments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payments
                .Include(p => p.Course)
                .Include(p => p.Listener)
                .FirstOrDefaultAsync(m => m.PaymentId == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // POST: Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();
            _cache.Clean();
            return RedirectToAction(nameof(Index));
        }

        private bool PaymentExists(int id)
        {
            return _context.Payments.Any(e => e.PaymentId == id);
        }
    }
}
