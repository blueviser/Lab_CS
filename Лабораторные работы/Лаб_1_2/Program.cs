using System;
using System.Threading;

namespace SynchronizedThreadsApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Создаем событие для синхронизации потоков
            AutoResetEvent autoResetEvent = new AutoResetEvent(false);

            // Создаем и запускаем первый поток
            Thread thread1 = new Thread(() => PrintNumbers(1, 100, autoResetEvent));
            thread1.Start();

            // Ждем 1 секунду перед запуском второго потока
            Thread.Sleep(1000);

            // Создаем и запускаем второй поток
            Thread thread2 = new Thread(() => PrintNumbers(1, 100, autoResetEvent));
            thread2.Start();

            // Ждем завершения обоих потоков
            thread1.Join();
            thread2.Join();

            Console.WriteLine("Оба потока завершили выполнение.");
        }

        static void PrintNumbers(int start, int end, AutoResetEvent autoResetEvent)
        {
            // Если это второй поток, ждем сигнала от первого потока
            if (Thread.CurrentThread.ManagedThreadId == 2)
            {
                Console.WriteLine("Поток 2 ожидает завершения потока 1...");
                autoResetEvent.WaitOne();
                Console.WriteLine("Поток 2 начал выполнение.");
            }

            // Выводим числа в указанном диапазоне
            for (int i = start; i <= end; i++)
            {
                Console.WriteLine($"Поток {Thread.CurrentThread.ManagedThreadId}: {i}");
                Thread.Sleep(10); // Задержка для наглядности
            }

            // Сигнализируем о завершении вывода чисел
            autoResetEvent.Set();
        }
    }
}