using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobsTrainer.Models
{
    public class TrainJob
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public uint JobId { get; set; }

        public string Link { get; set; }

        public string Sample { get; set; }

        public bool Sentiment { get; set; }

        [StringLength(100)]
        public string Country { get; set; }

        [StringLength(200)]
        public string Company { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
