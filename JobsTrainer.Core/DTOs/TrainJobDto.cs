using System;
using System.ComponentModel.DataAnnotations;

namespace JobsTrainer.Core.DTOs
{
    public class TrainJobDto
    {
        public int Index { get; set; }
        public string Title { get; set; }

        public string Sample { get; set; }

        public uint JobId { get; set; }

        public uint? NextJobId { get; set; }
        public uint? PrevJobId { get; set; }

        [Display(Name = "Positive")]
        public bool Sentiment { get; set; }

        public string Country { get; set; }

        public string Company { get; set; }

        public string CompanyLink { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsEasy { get; set; }
    }
}
