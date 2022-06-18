using AutoMapper;
using JobsTrainer.DTOs;
using JobsTrainer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobsTrainer.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ProofReadingController : Controller
    {
        private readonly TrainingContext _context;
        private readonly IMapper _mapper;

        public ProofReadingController(TrainingContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [Route("ProofReading")]
        [Route("ProofReading/Index")]
        [Route("ProofReading/Index/{page?}")]
        public async Task<IActionResult> Index(int? page, [FromQuery] string search, bool? isPositive)
        {
            if (_context.TrainJobs != null)
            {
                ViewBag.Page = page ?? 0;

                var tj = _context.TrainJobs.OrderBy(tj => tj.JobId).AsQueryable();
                
                if(!string.IsNullOrEmpty(search))
                {
                    tj = tj.Where(t => t.Title.Contains(search));
                    ViewBag.SearchTerm = search;
                }

                if (isPositive != null)
                {
                    tj = tj.Where(t => t.Sentiment == isPositive);
                    ViewBag.IsPositive = isPositive;
                }

                ViewBag.Count = await tj.CountAsync();
                return View(await tj.Skip((page ?? 0) * 10).Take(10).ToListAsync());
            } 
            else 
                return Problem("Entity set 'TrainingContext.TrainJobs'  is null.");
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
        public async Task<IActionResult> Edit(uint? id, [FromQuery] string search, bool? isPositive)
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

            var mappedJob = _mapper.Map<TrainJobDto>(trainJob);

            var tj = _context.TrainJobs.OrderBy(tj => tj.JobId).AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                tj = tj.Where(t => t.Title.Contains(search));
                ViewBag.SearchTerm = search;
            }

            if (isPositive != null)
            {
                tj = tj.Where(t => t.Sentiment == isPositive);
                ViewBag.IsPositive = isPositive;
            }

            var prev = tj.OrderByDescending(t => t.JobId).Where(x => x.JobId < id).FirstOrDefault();
            var next = tj.OrderBy(t => t.JobId).Where(x => x.JobId > id).FirstOrDefault();

            mappedJob.PrevJobId = prev?.JobId;
            mappedJob.NextJobId = next?.JobId;

            return View(mappedJob);
        }

        // POST: ProofReading/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(uint id, [Bind("JobId, Sentiment, Country, CompanyLink")] TrainJob trainJob)
        {
            if (id != trainJob.JobId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.TrainJobs.Attach(trainJob);
                    _context.Entry(trainJob).Property(x => x.Sentiment).IsModified = true;
                    _context.Entry(trainJob).Property(x => x.Country).IsModified = true;
                    _context.Entry(trainJob).Property(x => x.CompanyLink).IsModified = true;

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

                return RedirectToAction(nameof(Edit), id);
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
