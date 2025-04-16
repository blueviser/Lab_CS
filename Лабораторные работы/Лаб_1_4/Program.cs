using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketServerMT
{
    class SocketServer
    {
        private const int Port = 11000;
        private static int _requestCount = 0;

        static void Main(string[] args)
        {
            Console.WriteLine("Многопоточный сервер запущен...");

            var listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(new IPEndPoint(IPAddress.Any, Port));
            listener.Listen(10); // Максимальная очередь подключений

            try
            {
                while (true)
                {
                    Console.WriteLine("Ожидание подключения...");
                    var clientSocket = listener.Accept();
                    Task.Run(() => HandleClient(clientSocket));
                }
            }
            finally
            {
                listener.Close();
            }
        }

        static void HandleClient(Socket clientSocket)
        {
            try
            {
                _requestCount++;
                Console.WriteLine($"Обработка запроса #{_requestCount}");

                // Получение данных
                var buffer = new byte[1024];
                int received = clientSocket.Receive(buffer);
                string request = Encoding.ASCII.GetString(buffer, 0, received);
                Console.WriteLine($"Получено: {request}");

                // Имитация обработки (блокирующая операция)
                System.Threading.Thread.Sleep(100);

                // Отправка ответа
                string response = $"Ответ на запрос #{_requestCount}: {request.ToUpper()}";
                byte[] responseData = Encoding.ASCII.GetBytes(response);
                clientSocket.Send(responseData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка обработки запроса: {ex.Message}");
            }
            finally
            {
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            }
        }
    }
}
