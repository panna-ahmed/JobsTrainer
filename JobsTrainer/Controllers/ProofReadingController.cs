using JobsTrainer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobsTrainer.Controllers
{
    public class ProofReadingController : Controller
    {
        private readonly TrainingContext _context;

        public ProofReadingController(TrainingContext context)
        {
            _context = context;
        }

        // GET: ProofReading
        public async Task<IActionResult> Index()
        {
            return _context.TrainJobs != null ?
                        View(await _context.TrainJobs.ToListAsync()) :
                        Problem("Entity set 'TrainingContext.TrainJobs'  is null.");
        }

        // GET: ProofReading/Details/5
        public async Task<IActionResult> Details(uint? id)
        {
            if (id == null || _context.TrainJobs == null)
            {
                return NotFound();
            }

            var trainJob = await _context.TrainJobs
                .FirstOrDefaultAsync(m => m.JobId == id);
            if (trainJob == null)
            {
                return NotFound();
            }

            return View(trainJob);
        }

        // GET: ProofReading/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProofReading/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("JobId,Title,Sample,Sentiment,Country,Company,CompanyLink,CreatedAt,IsEasy")] TrainJob trainJob)
        {
            if (ModelState.IsValid)
            {
                _context.Add(trainJob);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(trainJob);
        }

        // GET: ProofReading/Edit/5
        public async Task<IActionResult> Edit(uint? id)
        {
            if (id == null || _context.TrainJobs == null)
            {
                return NotFound();
            }

            var trainJob = await _context.TrainJobs.FindAsync(id);
            if (trainJob == null)
            {
                return NotFound();
            }
            return View(trainJob);
        }

        // POST: ProofReading/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(uint id, [Bind("JobId,Title,Sample,Sentiment,Country,Company,CompanyLink,CreatedAt,IsEasy")] TrainJob trainJob)
        {
            if (id != trainJob.JobId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trainJob);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainJobExists(trainJob.JobId))
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
            return View(trainJob);
        }

        // GET: ProofReading/Delete/5
        public async Task<IActionResult> Delete(uint? id)
        {
            if (id == null || _context.TrainJobs == null)
            {
                return NotFound();
            }

            var trainJob = await _context.TrainJobs
                .FirstOrDefaultAsync(m => m.JobId == id);
            if (trainJob == null)
            {
                return NotFound();
            }

            return View(trainJob);
        }

        // POST: ProofReading/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(uint id)
        {
            if (_context.TrainJobs == null)
            {
                return Problem("Entity set 'TrainingContext.TrainJobs'  is null.");
            }
            var trainJob = await _context.TrainJobs.FindAsync(id);
            if (trainJob != null)
            {
                _context.TrainJobs.Remove(trainJob);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrainJobExists(uint id)
        {
            return (_context.TrainJobs?.Any(e => e.JobId == id)).GetValueOrDefault();
        }
    }
}
