using AutoMapper;
using CMS.WebAPi.CMS.Data.Repository.Models;
using CMS.WebAPi.CMS.Data.Repository.Repositories;
using CMS.WebAPi.DTO;
using Microsoft.AspNetCore.Mvc;

namespace CMS.WebAPi.Controllers
{
    [ApiController]
    [ApiVersion("2.0")]
    //[Route("v{version:apiVersion}/courses")]
    [Route("courses")]
    public class Courses2Controller : ControllerBase
    {
        private readonly ICmsRepository cmsRepository;
        private readonly IMapper mapper;

        public Courses2Controller(ICmsRepository cmsRepository, IMapper mapper)
        {
            this.cmsRepository = cmsRepository;
            this.mapper = mapper;
        }

        //Approach 1
        //[HttpGet]
        //public IEnumerable<Course> GetCourses()
        //{
        //    return cmsRepository.GetAllCourses();
        //}

        //Return type - Approach 1 - primitive or complex type
        /*[HttpGet]
        public IEnumerable<CourseDto> GetCourses()
        {
            try
            {
                IEnumerable<Course> courses = cmsRepository.GetAllCourses();
                var result = MapCourseToCourseDto(courses);
                return result;
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }*/


        //Return type - Approach 2 - IActionResult
        //[HttpGet]
        //public IActionResult GetCourses()
        //{
        //    try
        //    {
        //        IEnumerable<Course> courses = cmsRepository.GetAllCourses();
        //        var result = MapCourseToCourseDto(courses);
        //        return Ok(result);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        //    }
        //}


        //Return type - Approach 3 - ActionResult<T>
        [HttpGet]
        public ActionResult<IEnumerable<CourseDto>> GetCourses()
        {
            try
            {
                IEnumerable<Course> courses = cmsRepository.GetAllCourses();
                //var result = MapCourseToCourseDto(courses);
                var result = mapper.Map<CourseDto[]>(courses);

                // version 2 changes
                foreach (var item in result)
                {
                    item.CourseName += " (v2.0)";
                }
                return result.ToList(); // Convert to support ActionResult<T>
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpGet]
        [MapToApiVersion("3.0")]
        public ActionResult<IEnumerable<CourseDto>> GetCourses_v3()
        {
            try
            {
                IEnumerable<Course> courses = cmsRepository.GetAllCourses();
                //var result = MapCourseToCourseDto(courses);
                var result = mapper.Map<CourseDto[]>(courses);

                // version 2 changes
                foreach (var item in result)
                {
                    item.CourseName += " (v3.0)";
                }
                return result.ToList(); // Convert to support ActionResult<T>
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public ActionResult<CourseDto> AddCourse([FromBody]CourseDto course)
        {
            try 
            {
                //Model State automatically done by ASP.Net.  Unnecessary
                //if(!ModelState.IsValid)
                //{
                //    return BadRequest(ModelState);
                //}
                var newCourse = mapper.Map<Course>(course);
                newCourse = cmsRepository.AddCourse(newCourse);
                return mapper.Map<CourseDto>(newCourse);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{courseId}")]
        public ActionResult<CourseDto> GetCourse(int courseId)
        {
            try
            {
                if (!cmsRepository.IsCourseExists(courseId))
                {
                    return NotFound();
                }

                Course course = cmsRepository.GetCourse(courseId);
                var result = mapper.Map<CourseDto>(course);

               
                return result;  
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{courseId}")]
        public ActionResult<CourseDto> UpdateCourse(int courseId, CourseDto course)
        {
            try
            {
                if (!cmsRepository.IsCourseExists(courseId))
                {
                    return NotFound();
                }
                Course updatedCourse = mapper.Map<Course>(course);
                    updatedCourse = cmsRepository.UpdateCourse(courseId, updatedCourse);
                var result = mapper.Map<CourseDto>(updatedCourse);

                return result;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpDelete("{courseId}")]
        public ActionResult<CourseDto> DeleteCourse(int courseId)
        {
            try
            {
                if (!cmsRepository.IsCourseExists(courseId))
                {
                    return NotFound();
                }

                Course course = cmsRepository.DeleteCourse(courseId);

                if (course == null) return BadRequest();
                var result = mapper.Map<CourseDto>(course);
                return result;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // GET ../courses/1/students
        [HttpGet("{courseId}/students")]
        public ActionResult<IEnumerable<StudentDto>> GetStudents(int courseId)
        {
            try
            {
                if (!cmsRepository.IsCourseExists(courseId))
                {
                    return NotFound();
                }

                IEnumerable<Student> students = cmsRepository.GetStudents(courseId);
                var result = mapper.Map<StudentDto[]>(students);
                return result;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        #region Async methods
        //Return type - Approach 4 - async Task
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<CourseDto>>> GetCourses()
        //{
        //    try
        //    {
        //        IEnumerable<Course> courses = await cmsRepository.GetAllCoursesAsync();
        //        //var result = MapCourseToCourseDto(courses);
        //        var result = mapper.Map<CourseDto[]>(courses);
        //        return result.ToList(); // Convert to support ActionResult<T>
        //    }
        //    catch (System.Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        //    }
        //}


        // POST ../courses/1/students
        [HttpPost("{courseId}/students")]
        public ActionResult<IEnumerable<StudentDto>> AddStudent(int courseId, StudentDto student)
        {
            try
            {
                if (!cmsRepository.IsCourseExists(courseId))
                {
                    return NotFound();
                }

                Student newStudent = mapper.Map<Student>(student);
                //Assign course
                Course course = cmsRepository.GetCourse(courseId);
                newStudent.Course = course;
                newStudent = cmsRepository.AddStudent(newStudent);
                var result = mapper.Map<StudentDto>(newStudent);

                return StatusCode(StatusCodes.Status201Created, result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        #endregion Async methods


        #region Custom mapper functions
        //private CourseDto MapCourseToCourseDto(Course course)
        //{
        //    return new CourseDto()
        //    {
        //        CourseId = course.CourseId,
        //        CourseName = course.CourseName,
        //        CourseDuration = course.CourseDuration,
        //        CourseType = (CourseDto.COURSE_TYPE)course.CourseType
        //    };
        //}


        //private IEnumerable<CourseDto> MapCourseToCourseDto(IEnumerable<Course> courses)
        //{
        //    IEnumerable<CourseDto> result;

        //    result = courses.Select(c => new CourseDto()
        //    {
        //        CourseId = c.CourseId,
        //        CourseName = c.CourseName,
        //        CourseDuration = c.CourseDuration,
        //        CourseType = (CourseDto.COURSE_TYPE)c.CourseType
        //    });

        //    return result;
        //}
        #endregion Custom mapper functions
    }
}
