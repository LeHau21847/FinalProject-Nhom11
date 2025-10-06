using System;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementApi.DTOs
{
    public class CreateStudentDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public int ClassId { get; set; }
    }
}