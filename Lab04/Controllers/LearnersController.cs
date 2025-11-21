using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab04.Data;
using Lab04.Models;

namespace Lab04.Controllers
{
    public class LearnersController : Controller
    {
        private readonly SchoolDbContext _context;
        private int pageSize = 3;
        public LearnersController(SchoolDbContext context)
        {
            _context = context;
        }

        // GET: Learners
        public IActionResult Index(int? mid)
        {
            var learners = (IQueryable<Learner>)_context.Learner.Include(l => l.Major);
            if (mid != null)
            {
                learners = (IQueryable<Learner>)_context.Learner
                    .Where(l => l.MajorId == mid)
                    .Include(l => l.Major);
            }

            int pageNum = (int)Math.Ceiling((double)learners.Count() / (float)pageSize);

            ViewBag.PageNum = pageNum;

            var result = learners.Take(pageSize)
                .ToList();
            return View(result);


        }

        // GET: Learners/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learner = await _context.Learner
                .Include(l => l.Major)
                .FirstOrDefaultAsync(m => m.LearnerId == id);
            if (learner == null)
            {
                return NotFound();
            }

            return View(learner);
        }

        // GET: Learners/Create
        public IActionResult Create()
        {
            ViewData["MajorId"] = new SelectList(_context.Set<Major>(), "MajorId", "MajorId");
            return View();
        }

        // POST: Learners/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LearnerId,FirstMidName,LastName,EnrollmentDate,MajorId")] Learner learner)
        {
            if (ModelState.IsValid)
            {
                _context.Add(learner);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MajorId"] = new SelectList(_context.Set<Major>(), "MajorId", "MajorId", learner.MajorId);
            return View(learner);
        }

        // GET: Learners/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learner = await _context.Learner.FindAsync(id);
            if (learner == null)
            {
                return NotFound();
            }
            ViewData["MajorId"] = new SelectList(_context.Set<Major>(), "MajorId", "MajorId", learner.MajorId);
            return View(learner);
        }

        // POST: Learners/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LearnerId,FirstMidName,LastName,EnrollmentDate,MajorId")] Learner learner)
        {
            if (id != learner.LearnerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(learner);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LearnerExists(learner.LearnerId))
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
            ViewData["MajorId"] = new SelectList(_context.Set<Major>(), "MajorId", "MajorId", learner.MajorId);
            return View(learner);
        }

        // GET: Learners/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learner = await _context.Learner
                .Include(l => l.Major)
                .FirstOrDefaultAsync(m => m.LearnerId == id);
            if (learner == null)
            {
                return NotFound();
            }

            return View(learner);
        }

        // POST: Learners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var learner = await _context.Learner.FindAsync(id);
            if (learner != null)
            {
                _context.Learner.Remove(learner);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LearnerExists(int id)
        {
            return _context.Learner.Any(e => e.LearnerId == id);
        }

        public IActionResult LearnerByMajorID(int mid)
        {
            var learners = _context.Learner
                .Where(l => l.MajorId == mid)
                .Include(l => l.Major).ToList();
            return PartialView("LearnerTable", learners);
        }

        public IActionResult LearnFilter(int? mid, string? keyword, int? pageIndex)
        {
            var learners = (IQueryable<Learner>)_context.Learner;

            int page = (int)(pageIndex == null || pageIndex <= 0 ? 1 : pageIndex);

            if (mid != null)
            {
                learners = learners.Where(l => l.MajorId == mid);

                ViewBag.mid = mid;
            }

            if (keyword != null)
            {
                learners = learners.Where(l => l.FirstMidName.ToLower().Contains(keyword.ToLower()));

                ViewBag.keyword = keyword;
            }

            int pageNum = (int)Math.Ceiling((double)learners.Count() / (float)pageSize);

            ViewBag.PageNum = pageNum;

            var result = learners.Skip(pageSize * (page - 1))
                .Take(pageSize)
                .Include(l => l.Major);

            return PartialView("LearnerTable", result);
        }
    }
}
