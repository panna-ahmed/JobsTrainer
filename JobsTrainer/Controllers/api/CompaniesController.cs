using JobsTrainer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobsTrainer.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompaniesController : Controller
    {
        private readonly TrainingContext _context;

        public CompaniesController(TrainingContext context)
        {
            _context = context;
        }

        [HttpGet("api/companies")]
        public async Task<IActionResult> All()
        {
            var companies = await _context.Companies.Select(c => c.Name).ToArrayAsync();

            return Ok(companies);
        }
    }
}
