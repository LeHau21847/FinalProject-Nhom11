using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementApi.Models
{
    public class Class
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}