using System;
using System.Threading;

namespace ThreadRangeApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Определяем диапазоны для каждого потока
            (int start, int end) range1 = (1, 10);
            (int start, int end) range2 = (11, 20);

            // Создаем и запускаем первый поток
            Thread thread1 = new Thread(PrintRange);
            thread1.Start(range1);

            // Создаем и запускаем второй поток
            Thread thread2 = new Thread(PrintRange);
            thread2.Start(range2);

            // Ждем завершения обоих потоков
            thread1.Join();
            thread2.Join();

            Console.WriteLine("Оба потока завершили выполнение.");
        }

        static void PrintRange(object rangeObj)
        {
            // Извлекаем диапазон из объекта параметра
            var range = (ValueTuple<int, int>)rangeObj;
            int start = range.Item1;
            int end = range.Item2;

            // Выводим числа в указанном диапазоне
            for (int i = start; i <= end; i++)
            {
                Console.WriteLine($"Поток {Thread.CurrentThread.ManagedThreadId}: {i}");
                Thread.Sleep(100); // Задержка для наглядности
            }
        }
    }
}