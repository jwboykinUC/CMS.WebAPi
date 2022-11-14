using AutoMapper;
using CMS.WebAPi.DTO;
using CMS.WebAPi.CMS.Data.Repository.Models;

namespace CMS.WebAPi.Mappers    
{
    public class CmsMapper: Profile
    {
        public CmsMapper()
        {
            CreateMap<CourseDto, Course>()
                .ReverseMap();
            //CreateMap<Course, CourseDto>();

            CreateMap<StudentDto, Student>()
                .ReverseMap();
        }
    }
}
