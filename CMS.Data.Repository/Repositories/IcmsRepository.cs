using CMS.WebAPi.CMS.Data.Repository.Models;

namespace CMS.WebAPi.CMS.Data.Repository.Repositories
{
    public interface ICmsRepository
    {
        IEnumerable<Course> GetAllCourses();
        Task<IEnumerable<Course>> GetAllCoursesAsync();
        Course AddCourse(Course newCourse);

        bool IsCourseExists(int courseId);

        Course GetCourse(int courseId);

        Course UpdateCourse(int courseId, Course newCourse);

        Course DeleteCourse(int courseId);

        //Association
        IEnumerable<Student> GetStudents(int courseId);
        Student AddStudent(Student student);
    }
}
