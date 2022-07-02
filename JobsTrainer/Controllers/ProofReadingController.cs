using AutoMapper;
using JobsTrainer.Core.DTOs;
using JobsTrainer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

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
        public async Task<IActionResult> Index(int? page, [FromQuery] string search, bool? isPositive, string sortOrder)
        {
            if (_context.TrainJobs == null)
                return Problem("Entity set 'TrainingContext.TrainJobs'  is null.");

            ViewBag.Page = page ?? 0;

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            var tj = _context.TrainJobs.AsQueryable();

            switch (sortOrder)
            {
                case "name_desc":
                    tj = tj.OrderByDescending(s => s.Title);
                    break;
                case "Date":
                    tj = tj.OrderBy(s => s.CreatedAt);
                    break;
                case "date_desc":
                    tj = tj.OrderByDescending(s => s.CreatedAt);
                    break;
                default:
                    tj = tj.OrderBy(s => s.Title);
                    break;
            }
                
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
        public async Task<IActionResult> Edit(uint? id, [FromQuery] int? page, string search, bool? isPositive, string sortOrder, string navigator, string direction)
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

            ViewBag.PrevPage = ViewBag.NextPage = page ?? 0;

            ViewBag.CurrentSort = sortOrder;

            var tj = _context.TrainJobs.AsQueryable();

            switch (sortOrder)
            {
                case "name_desc":
                    tj = tj.OrderByDescending(s => s.Title);
                    break;
                case "Date":
                    tj = tj.OrderBy(s => s.CreatedAt);
                    break;
                case "date_desc":
                    tj = tj.OrderByDescending(s => s.CreatedAt);
                    break;
                default:
                    tj = tj.OrderBy(s => s.Title);
                    break;
            }

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

            uint [] jobList = await tj.Skip((page ?? 0) * 10).Take(10).Select(j => j.JobId).ToArrayAsync();

            int index = Array.IndexOf(jobList, id);

            direction = direction != "next" && direction != "prev" ? "next" : direction;

            uint? prev = null;
            uint? next = null;

            if (index >= 0)
            {
                if (index > 0) prev = jobList[index - 1];
                if (index < jobList.Count()-1) next = jobList[index + 1];

                if(next == null && jobList.Count() == 10)
                {
                    var n = await tj.Skip(((page ?? 0) + 1) * 10).Take(10).FirstOrDefaultAsync();
                    next = n?.JobId;
                    if (n != null) ViewBag.NextPage = page + 1;
                }
                else if(prev == null && page!= 0)
                {
                    var p = await tj.Skip(((page ?? 0) - 1) * 10).Take(10).LastOrDefaultAsync();
                    prev = p?.JobId;

                    if (p != null) ViewBag.PrevPage = page - 1;
                }
            }
            else
            {
                if (direction == "prev")
                {
                    var result = await tj.Skip(((page ?? 0) - 1) * 10).Take(10).AnyAsync();
                    if (result)
                    {
                        ViewBag.PrevPage = page - 1;
                        ViewBag.NextPage = page;
                    }
                }
                else if (direction == "next")
                {
                    var result = await tj.Skip(((page ?? 0) + 1) * 10).Take(10).AnyAsync();
                    if (result)
                    {
                        ViewBag.PrevPage = page;
                        ViewBag.NextPage = page + 1;
                    }
                }
            }

            //ViewBag.Navigator = string.Join(',', jobList.Select(x => x.JobId));
            ViewBag.Direction = direction;

            mappedJob.PrevJobId = prev;
            mappedJob.NextJobId = next;

            foreach (var k in _context.Keywords)
            {
                mappedJob.Sample = mappedJob.Sample.Replace(k.Name, $"<mark>{k.Name}</mark>");
            }

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
