using CamenoDePetraWeb.Models;
using CamenoDePetraWeb.Models.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace CamenoDePetraWeb.Controllers
{
    public class ReviewController : Controller
    {
        private readonly ERPDbContext _context;

        public ReviewController(ERPDbContext context)
        {
            _context = context;
        }

        // GET: Show reviews on index page
        public IActionResult Index()
        {
            var reviews = _context.Reviews
                .OrderByDescending(r => r.CreatedAt)
                .ToList();
            return View(reviews);
        }

        // GET: Add new review form
        public IActionResult Create()
        {
            return View();
        }

        // POST: Save review
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Review review)
        {
            if (ModelState.IsValid)
            {
                _context.Reviews.Add(review);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        // Admin: Delete inappropriate review
        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
