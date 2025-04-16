using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StudentsGradeCalculation
{
    class Program
    {
        static void Main(string[] args)
        {
            // Создаем данные
            var students = GenerateSampleData();

            // Измеряем время выполнения синхронного метода
            Stopwatch syncStopwatch = Stopwatch.StartNew();
            var syncAverageGrades = CalculateAverageGradesSync(students);
            syncStopwatch.Stop();
            Console.WriteLine($"Синхронный метод: Время выполнения = {syncStopwatch.ElapsedMilliseconds} мс");

            // Измеряем время выполнения параллельного метода с использованием Parallel
            Stopwatch parallelStopwatch = Stopwatch.StartNew();
            var parallelAverageGrades = CalculateAverageGradesParallel(students);
            parallelStopwatch.Stop();
            Console.WriteLine($"Параллельный метод (Parallel): Время выполнения = {parallelStopwatch.ElapsedMilliseconds} мс");

            // Измеряем время выполнения асинхронного метода с использованием Task
            Stopwatch taskStopwatch = Stopwatch.StartNew();
            var taskAverageGrades = CalculateAverageGradesTaskAsync(students).GetAwaiter().GetResult();
            taskStopwatch.Stop();
            Console.WriteLine($"Асинхронный метод (Task): Время выполнения = {taskStopwatch.ElapsedMilliseconds} мс");

            // Проверяем результаты
            Console.WriteLine("Результаты синхронного метода:");
            PrintAverageGrades(syncAverageGrades);

            Console.WriteLine("Результаты параллельного метода (Parallel):");
            PrintAverageGrades(parallelAverageGrades);

            Console.WriteLine("Результаты асинхронного метода (Task):");
            PrintAverageGrades(taskAverageGrades);
        }

        static List<Student> GenerateSampleData()
        {
            var students = new List<Student>
            {
                new Student { Name = "Alice", Grades = new List<int> { 90, 85, 92 } },
                new Student { Name = "Bob", Grades = new List<int> { 80, 88, 84 } },
                new Student { Name = "Charlie", Grades = new List<int> { 95, 91, 93 } },
                new Student { Name = "Dave", Grades = new List<int> { 85, 89, 87 } }
            };

            return students;
        }

        static Dictionary<string, double> CalculateAverageGradesSync(List<Student> students)
        {
            var averageGrades = new Dictionary<string, double>();

            foreach (var student in students)
            {
                double averageGrade = student.Grades.Average();
                averageGrades[student.Name] = averageGrade;
            }

            return averageGrades;
        }

        static Dictionary<string, double> CalculateAverageGradesParallel(List<Student> students)
        {
            var averageGrades = new Dictionary<string, double>();

            Parallel.ForEach(students, student =>
            {
                double averageGrade = student.Grades.Average();
                lock (averageGrades)
                {
                    averageGrades[student.Name] = averageGrade;
                }
            });

            return averageGrades;
        }

        static async Task<Dictionary<string, double>> CalculateAverageGradesTaskAsync(List<Student> students)
        {
            var tasks = students.Select(async student =>
            {
                await Task.Delay(10); // Симуляция асинхронной работы
                double averageGrade = student.Grades.Average();
                return new { student.Name, AverageGrade = averageGrade };
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

    class Student
    {
        public string Name { get; set; }
        public List<int> Grades { get; set; }
    }
}