using JobsTrainer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobsTrainer.Controllers.Api
{
    [ApiController]
    [Route("api/skills")]
    public class SkillsController : Controller
    {
        private readonly TrainingContext _context;

        public SkillsController(TrainingContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var skills = await _context.Skills.Select(c => c.Name).ToArrayAsync();

            return Ok(skills);
        }
    }
}
