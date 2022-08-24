using AutoMapper;
using JobsTrainer.Core.DTOs;
using JobsTrainer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Configuration;

namespace JobsTrainer.Controllers.api
{
    [ApiController]
    [Route("api/train")]
    public class TrainingController : ControllerBase
    {
        private readonly ILogger<TrainingController> _logger;
        private readonly IMapper _mapper;
        private readonly TrainingContext _ctx;

        private IConfiguration _configuration;

        public TrainingController(ILogger<TrainingController> logger, IMapper mapper, TrainingContext ctx, IConfiguration configuration)
        {
            _ctx = ctx;
            _mapper = mapper;
            _logger = logger;
            _configuration = configuration;
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

        [HttpPost("export")]
        public async Task<ActionResult> Export()
        {
            var connStr = _configuration.GetConnectionString("RealJobsConnections");

            var settings = MongoClientSettings.FromConnectionString(connStr);
            settings.WaitQueueTimeout = TimeSpan.FromMinutes(1);
            settings.MinConnectionPoolSize = 100;
            settings.MaxConnectionPoolSize = 500;
            // Set the version of the Stable API on the client.
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            var client = new MongoClient(settings);

            var database = client.GetDatabase("jobsPortalDb");
            var collection = database.GetCollection<BsonDocument>("jobs");

            var tj = _ctx.TrainJobs.Where(t => t.Sentiment == true && t.Exported == false).OrderByDescending(s => s.CreatedAt);

            while (tj.Count() > 0)
            {
                var jobs = tj.Take(25).ToList();
                var docs = jobs.Select(s => new BsonDocument
                {
                    { "jobId", s.JobId },
                    { "country", s.Country },
                    { "company", s.Company },
                    { "title", s.Title },
                    { "skills", s.Skills??"" },
                });

                await collection.InsertManyAsync(docs);

                jobs.ForEach(c => c.Exported = true);
                _ctx.SaveChanges();
            }

            return Accepted();
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
