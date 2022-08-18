using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace JobsTrainer.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class Skill
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
