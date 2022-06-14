using AutoMapper;
using AutoMapper.QueryableExtensions;
using JobsTrainer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobsTrainer.Controllers
{
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ILogger<CompanyController> _logger;
        private readonly IMapper _mapper;
        private readonly TrainingContext _ctx;

        public CompanyController(ILogger<CompanyController> logger, IMapper mapper, TrainingContext ctx)
        {
            _ctx = ctx;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("companies")]
        public async Task<IActionResult> Index()
        {
            var companies = await _ctx.Companies.Select(c => c.Name).ToArrayAsync();

            return Ok(companies);
        }
    }
}
