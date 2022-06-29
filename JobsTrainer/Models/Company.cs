using System.ComponentModel.DataAnnotations;

namespace JobsTrainer.Models
{
    public class Company
    {
        [Key]
        public int Id { get; set; }

        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(300)]
        public string Link { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsFriendly { get; set; }
    }
}
