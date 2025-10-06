using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementApi.Data;
using StudentManagementApi.DTOs;
using StudentManagementApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ClassesController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClassDto>>> GetClasses()
        {
            var classes = await _context.Classes.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<ClassDto>>(classes));
        }

        [HttpPost]
        public async Task<ActionResult<ClassDto>> PostClass(ClassDto classDto)
        {
            var @class = _mapper.Map<Class>(classDto);
            _context.Classes.Add(@class);
            await _context.SaveChangesAsync();
            
            var createdClassDto = _mapper.Map<ClassDto>(@class);
            return CreatedAtAction(nameof(GetClasses), new { id = createdClassDto.Id }, createdClassDto);
        }
    }
}