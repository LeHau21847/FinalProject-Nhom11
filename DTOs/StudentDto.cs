using System;

namespace StudentManagementApi.DTOs
{
    public class StudentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string ClassName { get; set; } // Chỉ hiển thị tên lớp, không phải cả object Class
    }
}