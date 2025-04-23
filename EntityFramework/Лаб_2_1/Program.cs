using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Лаб_2_1;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        IConfiguration configuration = builder.Build();
        string connectionString = configuration.GetConnectionString("DefaultConnection");

        Console.WriteLine("Connection String: " + connectionString);

        // Создание контекста
        var context = new CourseContext();

        // Добавление данных
        var course = new Course
        {
            Id = 1,
            Title = "C# Programming",
            Duration = 30,
            Description = "Learn C# programming from scratch"
        };
        context.AddCourse(course);

        // Чтение данных
        var courses = context.GetCourses();
        foreach (var c in courses)
        {
            Console.WriteLine($"Id: {c.Id}, Title: {c.Title}, Duration: {c.Duration}, Description: {c.Description}");
        }

        // Модификация данных
        course.Title = "Advanced C# Programming";
        context.UpdateCourse(course);

        // Чтение данных после модификации
        var updatedCourse = context.GetCourseById(1);
        if (updatedCourse != null)
        {
            Console.WriteLine($"Updated Course - Id: {updatedCourse.Id}, Title: {updatedCourse.Title}, Duration: {updatedCourse.Duration}, Description: {updatedCourse.Description}");
        }
    }
}
