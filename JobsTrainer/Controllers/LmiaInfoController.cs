using AutoMapper;
using CsvHelper;
using CsvHelper.Excel;
using DocumentFormat.OpenXml.Office2010.Excel;
using JobsTrainer.Core.DTOs;
using JobsTrainer.Infrastructure;
using JobsTrainer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobsTrainer.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class LmiaInfoController : Controller
    {       

        private readonly TrainingContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public LmiaInfoController(TrainingContext context, IWebHostEnvironment environment, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _environment = environment;
            _configuration = configuration;
            _mapper = mapper;
        }

        // GET: LmiaInfo
        public async Task<IActionResult> Index()
        {
            return View(await _context.LmiaInfos.OrderByDescending(l => l.Approved).ToListAsync());
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

            var mappedLmia = _mapper.Map<LmiaInfoDto>(lmiaInfo);

            var li = _context.LmiaInfos.OrderByDescending(l => l.Approved).AsQueryable();

            int[] lmialist = await li.Select(j => j.Id).ToArrayAsync();

            int index = Array.IndexOf(lmialist, id);

            int? prev = null;
            int? next = null;

            if (index >= 0)
            {
                if (index > 0) prev = lmialist[index - 1];
                if (index < lmialist.Count() - 1) next = lmialist[index + 1];
            }

            mappedLmia.PrevLmiaId = prev;
            mappedLmia.NextLmiaId = next;

            return View(mappedLmia);
        }

        // POST: LmiaInfo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProvinceTerritory,ProgramStream,Employer,Address,Occupation,CompanyLink,Approved")] LmiaInfo lmiaInfo)
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
                return RedirectToAction(nameof(Edit), id);
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

        [HttpPost]
        public async Task<IActionResult> Index(IFormFile postedFile)
        {
            var nocs = new List<string>() { "2173", "2174", "2175", "2147" };
            if (postedFile != null)
            {
                //Create a Folder.
                string path = Path.Combine(this._environment.WebRootPath, "uploads");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                //Save the uploaded Excel file.
                string fileName = Path.GetFileName(postedFile.FileName);
                string filePath = Path.Combine(path, fileName);

                var fileWithoutExt = Path.GetFileNameWithoutExtension(fileName);
                var ext = Path.GetExtension(fileName);

                if (ext == ".xlsx")
                {
                    int i = 1;
                    while (System.IO.File.Exists(filePath))
                    {
                        filePath = Path.Combine(path, fileWithoutExt + '-' + i + ext);
                        i++;
                    }

                    using (FileStream stream = new FileStream(filePath, FileMode.Create))
                    {
                        postedFile.CopyTo(stream);
                    }

                    var lmias = new List<ExcelLmia>();
                    using var reader = new CsvReader(new ExcelParser(filePath));
                    reader.Read();
                    while (reader.Read())
                    {
                        try
                        {
                            var record = reader.GetRecord<ExcelLmia>();
                            var lmiaInfo = await _context.LmiaInfos.FirstOrDefaultAsync(m => m.Employer == record.Employer);

                            if (lmiaInfo == null)
                            {
                                foreach (var n in nocs)
                                {
                                    if (record.Occupation.Contains(n))
                                    {
                                        lmias.Add(record);
                                        break;
                                    }
                                }
                            }
                            else if(lmiaInfo.Approved != record.Approved)
                            {
                                lmiaInfo.Approved += record.Approved;

                                try
                                {
                                    _context.Update(lmiaInfo);
                                    await _context.SaveChangesAsync();
                                }
                                catch
                                {                                    
                                }
                            }
                        }
                        catch
                        {

                        }
                    }

                    var mappedLmias = _mapper.Map<List<LmiaInfo>>(lmias);
                    await _context.AddRangeAsync(mappedLmias);
                    await _context.SaveChangesAsync();
                }
            }

            return View(await _context.LmiaInfos.OrderByDescending(l => l.Approved).ToListAsync());
        }
    }
}
