using AutoMapper;
using JobsTrainer.Core.DTOs;
using JobsTrainer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobsTrainer.Controllers.api
{
    [ApiController]
    [Route("api/train")]
    public class TrainingController : ControllerBase
    {
        private readonly ILogger<TrainingController> _logger;
        private readonly IMapper _mapper;
        private readonly TrainingContext _ctx;

        public TrainingController(ILogger<TrainingController> logger, IMapper mapper, TrainingContext ctx)
        {
            _ctx = ctx;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(IEnumerable<TrainJobDto> data)
        {
            _logger.LogInformation($"Posted with {data.Count()} items");
            var mappedJobs = _mapper.Map<IEnumerable<TrainJob>>(data);

            int count = 0;
            foreach (var j in mappedJobs)
            {
                if (!_ctx.TrainJobs.Any(t => t.JobId == j.JobId))
                {
                    j.CreatedAt = DateTime.Now;

                    _ctx.TrainJobs.Add(j);
                    count += await _ctx.SaveChangesAsync();
                }
            }

            _logger.LogInformation($"Inserted {count} items");

            return Accepted();
        }

        [HttpPost("exists")]
        public IActionResult Exists([FromBody] CheckJobDto[] jobIds)
        {
            var existingJobs = jobIds.Where(t => !_ctx.TrainJobs.Any(j => j.JobId == t.JobId)).ToList();

            _logger.LogInformation($"Existing {existingJobs.Count} items");

            return Ok(existingJobs);
        }

        [HttpPost("optimize")]
        public IActionResult Optimize()
        {
            var jobCounts = _ctx.TrainJobs.GroupBy(t => t.Company)
                .Select(j => new
                {
                    CompanyName = j.Key,
                    JobCount = j.Count()
                }).Where(j => j.JobCount > 10).ToList();

            foreach (var jc in jobCounts)
            {
                var tj = _ctx.TrainJobs.Where(t => t.Company == jc.CompanyName);

                var s = tj.First().Sentiment;
                if (tj.Any(t => t.Sentiment != s))
                    continue;

                _ctx.Companies.Add(new Company { Name = jc.CompanyName, Link = tj.FirstOrDefault(t => t.CompanyLink != null)?.CompanyLink, IsFriendly = s, CreatedAt = DateTime.Now });

                _ctx.TrainJobs.RemoveRange(tj);
                _ctx.SaveChanges();
            }

            int noOfRowUpdated = _ctx.Database.ExecuteSqlRaw(@"DELETE FROM TrainJobs
                WHERE JobId NOT IN
                (
                    SELECT MAX(JobId) AS MaxRecordID
                    FROM TrainJobs
                    GROUP BY [Sample]
                )");

            return Accepted();
        }
    }
}
