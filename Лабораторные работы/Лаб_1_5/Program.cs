using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DisciplineStudentGradeApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Создаем данные
            var disciplines = GenerateSampleData();

            // Измеряем время выполнения синхронного метода
            Stopwatch syncStopwatch = Stopwatch.StartNew();
            var syncAverageGrades = CalculateAverageGradesSync(disciplines);
            syncStopwatch.Stop();
            Console.WriteLine($"Синхронный метод: Время выполнения = {syncStopwatch.ElapsedMilliseconds} мс");

            // Измеряем время выполнения асинхронного метода
            Stopwatch asyncStopwatch = Stopwatch.StartNew();
            var asyncAverageGrades = CalculateAverageGradesAsync(disciplines).GetAwaiter().GetResult();
            asyncStopwatch.Stop();
            Console.WriteLine($"Асинхронный метод: Время выполнения = {asyncStopwatch.ElapsedMilliseconds} мс");

            // Проверяем результаты
            Console.WriteLine("Результаты синхронного метода:");
            PrintAverageGrades(syncAverageGrades);

            Console.WriteLine("Результаты асинхронного метода:");
            PrintAverageGrades(asyncAverageGrades);
        }

        static List<Discipline> GenerateSampleData()
        {
            var disciplines = new List<Discipline>
            {
                new Discipline
                {
                    Name = "Math",
                    Students = new List<Student>
                    {
                        new Student { Name = "Alice", Grades = new List<int> { 90, 85, 92 } },
                        new Student { Name = "Bob", Grades = new List<int> { 80, 88, 84 } }
                    }
                },
                new Discipline
                {
                    Name = "Science",
                    Students = new List<Student>
                    {
                        new Student { Name = "Charlie", Grades = new List<int> { 95, 91, 93 } },
                        new Student { Name = "Dave", Grades = new List<int> { 85, 89, 87 } }
                    }
                }
            };

            return disciplines;
        }

        static Dictionary<string, double> CalculateAverageGradesSync(List<Discipline> disciplines)
        {
            var averageGrades = new Dictionary<string, double>();

            foreach (var discipline in disciplines)
            {
                double totalGrades = 0;
                int totalCount = 0;

                foreach (var student in discipline.Students)
                {
                    totalGrades += student.Grades.Sum();
                    totalCount += student.Grades.Count;
                }

                averageGrades[discipline.Name] = totalCount > 0 ? totalGrades / totalCount : 0;
            }

            return averageGrades;
        }

        static async Task<Dictionary<string, double>> CalculateAverageGradesAsync(List<Discipline> disciplines)
        {
            var tasks = disciplines.Select(async discipline =>
            {
                double totalGrades = 0;
                int totalCount = 0;

                foreach (var student in discipline.Students)
                {
                    totalGrades += student.Grades.Sum();
                    totalCount += student.Grades.Count;
                }

                await Task.Delay(10); // Симуляция асинхронной работы

                return new { discipline.Name, AverageGrade = totalCount > 0 ? totalGrades / totalCount : 0 };
            }).ToList();

            var results = await Task.WhenAll(tasks);

            return results.ToDictionary(r => r.Name, r => r.AverageGrade);
        }

        static void PrintAverageGrades(Dictionary<string, double> averageGrades)
        {
            foreach (var kvp in averageGrades)
            {
                Console.WriteLine($"{kvp.Key}: {kvp.Value:F2}");
            }
        }
    }

    class Discipline
    {
        public string Name { get; set; }
        public List<Student> Students { get; set; }
    }

    class Student
    {
        public string Name { get; set; }
        public List<int> Grades { get; set; }
    }
}