using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobsTrainer.Models
{
    [Index(nameof(Title), nameof(CreatedAt))]
    public class TrainJob
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public uint JobId { get; set; }

        [StringLength(300)]
        public string Title { get; set; }

        public string Sample { get; set; }

        [Display( Name = "Positive")]
        public bool Sentiment { get; set; }

        [StringLength(100)]
        public string Country { get; set; }

        [StringLength(200)]
        public string Company { get; set; }

        [StringLength(200)]
        public string CompanyLink { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool IsEasy { get; set; }
    }
}
