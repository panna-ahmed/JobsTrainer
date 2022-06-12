﻿using AutoMapper;
using JobsTrainer.DTOs;
using JobsTrainer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobsTrainer.Controllers
{
    [ApiController]
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

        [HttpPost("train/create")]
        public async Task<IActionResult> Create(IEnumerable<TrainJobDto> data)
        {
            var _mappedJobs = _mapper.Map<IEnumerable<TrainJob>>(data);

            foreach (var j in _mappedJobs) {
                if (!_ctx.TrainJobs.Any(t => t.JobId == j.JobId))
                {
                    j.CreatedAt = DateTime.Now;

                    _ctx.TrainJobs.Add(j);
                    await _ctx.SaveChangesAsync();
                }
            }

            return Accepted();
        }
    }
}
