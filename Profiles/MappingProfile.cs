using AutoMapper;
using StudentManagementApi.DTOs;
using StudentManagementApi.Models;

namespace StudentManagementApi.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Map từ Model -> DTO
            CreateMap<Class, ClassDto>();
            CreateMap<Student, StudentDto>()
                .ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.Class.Name));

            // Map từ DTO -> Model
            CreateMap<CreateStudentDto, Student>();
            CreateMap<UpdateStudentDto, Student>();
            CreateMap<ClassDto, Class>(); // Dùng cho tạo lớp mới
        }
    }
}