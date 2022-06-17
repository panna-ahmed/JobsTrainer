using JobsTrainer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobsTrainer.Controllers.Api
{
    [ApiController]
    [Route("api/companies")]
    public class CompaniesController : Controller
    {
        private readonly TrainingContext _context;

        public CompaniesController(TrainingContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var companies = await _context.Companies.Select(c => c.Name).ToArrayAsync();

            return Ok(companies);
        }
    }
}
