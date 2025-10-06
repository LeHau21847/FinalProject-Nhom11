using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementApi.Data;
using StudentManagementApi.DTOs;
using StudentManagementApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public StudentsController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Students?pageNumber=1&pageSize=10 (Bonus Phân trang)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetStudents([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var totalStudents = await _context.Students.CountAsync();
            var students = await _context.Students
                .Include(s => s.Class)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            
            var studentDtos = _mapper.Map<IEnumerable<StudentDto>>(students);
            
            var result = new
            {
                TotalCount = totalStudents,
                PageSize = pageSize,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(totalStudents / (double)pageSize),
                Data = studentDtos
            };

            return Ok(result);
        }

        [HttpGet("/api/classes/{classId}/students")]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetStudentsByClass(int classId)
        {
            var students = await _context.Students
                .Where(s => s.ClassId == classId)
                .Include(s => s.Class)
                .ToListAsync();
            
            if (!students.Any()) return NotFound($"No students found for class with ID {classId}.");

            return Ok(_mapper.Map<IEnumerable<StudentDto>>(students));
        }

        [HttpPost]
        public async Task<ActionResult<StudentDto>> PostStudent(CreateStudentDto createStudentDto)
        {
            var existingClass = await _context.Classes.FindAsync(createStudentDto.ClassId);
            if (existingClass == null)
            {
                return BadRequest($"Class with ID {createStudentDto.ClassId} does not exist.");
            }

            var student = _mapper.Map<Student>(createStudentDto);
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            // Tải lại thông tin để map ClassName
            await _context.Entry(student).Reference(s => s.Class).LoadAsync();

            var studentDto = _mapper.Map<StudentDto>(student);
            return CreatedAtAction(nameof(GetStudents), new { id = studentDto.Id }, studentDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, UpdateStudentDto updateStudentDto)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound($"Student with ID {id} not found.");
            }
            
            // Map các giá trị từ DTO vào entity đã tồn tại
            _mapper.Map(updateStudentDto, student);
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Students.Any(e => e.Id == id)) return NotFound();
                else throw;
            }

            return NoContent();
        }
    }
}