using JobsTrainer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobsTrainer.Controllers.Api
{
    [ApiController]
    [Route("api/lmiainfo")]
    public class LmiaInfoController : Controller
    {
        private readonly TrainingContext _context;

        public LmiaInfoController(TrainingContext context)
        {
            _context = context;
        }

        // PUT: LmiaInfo/UpdateLink?id=5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPut("UpdateLink")]
        public async Task<IActionResult> UpdateLink(int? id, [FromBody] string link)
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

            try
            {
                lmiaInfo.CompanyLink = link;
                _context.Update(lmiaInfo);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return Ok(lmiaInfo);
        }
    }
}
