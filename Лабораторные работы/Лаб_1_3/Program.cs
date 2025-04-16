using System;
using System.Threading;

class Program
{
    private static double sharedValue = 0.5; // Начальное значение
    private static readonly object lockObject = new object();
    private static bool isCosineTurn = true; // Флаг для определения очередности потоков

    static void Main()
    {
        Thread cosineThread = new Thread(CalculateCosine);
        Thread arccosineThread = new Thread(CalculateArccosine);

        cosineThread.Start();
        arccosineThread.Start();

        cosineThread.Join();
        arccosineThread.Join();
    }

    static void CalculateCosine()
    {
        while (true)
        {
            lock (lockObject)
            {
                while (!isCosineTurn)
                {
                    Monitor.Wait(lockObject);
                }

                double result = Math.Cos(sharedValue);
                Console.WriteLine($"Cosine: {result}");
                sharedValue = result;

                isCosineTurn = false;
                Monitor.Pulse(lockObject);
            }
        }
    }

    static void CalculateArccosine()
    {
        while (true)
        {
            lock (lockObject)
            {
                while (isCosineTurn)
                {
                    Monitor.Wait(lockObject);
                }

                double result = Math.Acos(sharedValue);
                Console.WriteLine($"Arccosine: {result}");
                sharedValue = result;

                isCosineTurn = true;
                Monitor.Pulse(lockObject);
            }
        }
    }
}
