using System;

namespace JobsTrainer.DTOs
{
    public class TrainJobDto
    {
        public string Title { get; set; }

        public string Sample { get; set; }

        public uint JobId { get; set; }

        public uint? NextJobId { get; set; }
        public uint? PrevJobId { get; set; }

        public bool Sentiment { get; set; }

        public string Country { get; set; }

        public string Company { get; set; }

        public string CompanyLink { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsEasy { get; set; }
    }
}
