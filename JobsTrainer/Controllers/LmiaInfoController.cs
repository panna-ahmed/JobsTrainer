using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JobsTrainer.Models;

namespace JobsTrainer.Controllers
{
    public class LmiaInfoController : Controller
    {
        private readonly TrainingContext _context;

        public LmiaInfoController(TrainingContext context)
        {
            _context = context;
        }

        // GET: LmiaInfo
        public async Task<IActionResult> Index()
        {
            return View(await _context.LmiaInfos.ToListAsync());
        }

        // GET: LmiaInfo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.LmiaInfos == null)
            {
                return NotFound();
            }

            var lmiaInfo = await _context.LmiaInfos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lmiaInfo == null)
            {
                return NotFound();
            }

            return View(lmiaInfo);
        }

        // GET: LmiaInfo/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LmiaInfo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProvinceTerritory,ProgramStream,Employer,Address,Occupation,Approved")] LmiaInfo lmiaInfo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lmiaInfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(lmiaInfo);
        }

        // GET: LmiaInfo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.LmiaInfos == null)
            {
                return NotFound();
            }

            var lmiaInfo = await _context.LmiaInfos.FindAsync(id);
            if (lmiaInfo == null)
            {
                return NotFound();
            }
            return View(lmiaInfo);
        }

        // POST: LmiaInfo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProvinceTerritory,ProgramStream,Employer,Address,Occupation,Approved")] LmiaInfo lmiaInfo)
        {
            if (id != lmiaInfo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lmiaInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LmiaInfoExists(lmiaInfo.Id))
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
            return View(lmiaInfo);
        }

        // GET: LmiaInfo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.LmiaInfos == null)
            {
                return NotFound();
            }

            var lmiaInfo = await _context.LmiaInfos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lmiaInfo == null)
            {
                return NotFound();
            }

            return View(lmiaInfo);
        }

        // POST: LmiaInfo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.LmiaInfos == null)
            {
                return Problem("Entity set 'TrainingContext.LmiaInfos'  is null.");
            }
            var lmiaInfo = await _context.LmiaInfos.FindAsync(id);
            if (lmiaInfo != null)
            {
                _context.LmiaInfos.Remove(lmiaInfo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LmiaInfoExists(int id)
        {
            return _context.LmiaInfos.Any(e => e.Id == id);
        }
    }
}
