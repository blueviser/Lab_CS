using System.Collections.Generic;
using System.Linq;
using Лаб_2_1;

public class CourseContext
{
    private List<Course> courses = new List<Course>();

    public void AddCourse(Course course)
    {
        courses.Add(course);
    }

    public List<Course> GetCourses()
    {
        return courses;
    }

    public Course GetCourseById(int id)
    {
        return courses.FirstOrDefault(c => c.Id == id);
    }

    public void UpdateCourse(Course course)
    {
        var existingCourse = GetCourseById(course.Id);
        if (existingCourse != null)
        {
            existingCourse.Title = course.Title;
            existingCourse.Duration = course.Duration;
            existingCourse.Description = course.Description;
        }
    }
}
