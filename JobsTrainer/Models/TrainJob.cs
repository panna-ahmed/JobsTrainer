using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobsTrainer.Models
{
    public class TrainJob
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public uint JobId { get; set; }

        [StringLength(300)]
        public string Title { get; set; }

        public string Sample { get; set; }

        public bool Sentiment { get; set; }

        [StringLength(100)]
        public string Country { get; set; }

        [StringLength(200)]
        public string Company { get; set; }

        [StringLength(200)]
        public string CompanyLink { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsEasy { get; set; }
    }
}
