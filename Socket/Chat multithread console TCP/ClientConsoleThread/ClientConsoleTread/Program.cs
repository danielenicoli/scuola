using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        TcpClient client = new TcpClient();
        client.Connect(IPAddress.Parse("127.0.0.1"), 12345);
        //La porta del client viene comunque assegnata dinamicamente, per ottenere la porta effettivamente usata ((IPEndPoint)client.Client.LocalEndPoint).Port

        NetworkStream stream = client.GetStream();
        string username;

        Console.Write("Inserire un nome utente: ");
        username = Console.ReadLine();
        byte[] usernameData = Encoding.ASCII.GetBytes(username);
        stream.Write(usernameData, 0, usernameData.Length);

        //Da questo punto in poi il client riceverà i messaggi inviati dagli altri utenti

        Thread receiveThread = new Thread(ReceiveMessages);
        receiveThread.Start(client);

        string message;

        while (true)
        {
            Console.Write("Inserire un messaggio da inviare ai client connessi: ");
            message = Console.ReadLine();
            byte[] messageData = Encoding.ASCII.GetBytes(message);
            stream.Write(messageData, 0, messageData.Length);
        }

        client.Close();
    }

    static void ReceiveMessages(object clientObj)
    {
        TcpClient client = (TcpClient)clientObj;
        byte[] buffer = new byte[1024];
        int bytesRead;

        while (true)
        {
            bytesRead = client.GetStream().Read(buffer, 0, buffer.Length);

            if (bytesRead == 0)
            {
                // La connessione è stata chiusa (ricevo byte uguali a 0) -> esci dal ciclo
                break;
            }

            string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            Console.WriteLine("Messaggio ricevuto: {0}", message);
            Array.Clear(buffer, 0, buffer.Length);
        }
    }
}
