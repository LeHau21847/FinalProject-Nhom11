using System;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementApi.DTOs
{
    public class UpdateStudentDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }
    }
}