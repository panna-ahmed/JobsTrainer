using JobsTrainer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobsTrainer.Controllers.Api
{
    [ApiController]
    [Route("api/keywords")]
    public class KeywordsController : Controller
    {
        private readonly TrainingContext _context;

        public KeywordsController(TrainingContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var keywords = await _context.Keywords.Select(c => c.Name).ToArrayAsync();

            return Ok(keywords);
        }
    }
}
