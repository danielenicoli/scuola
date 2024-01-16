using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class ChatServer
{
    static TcpListener server;
    static TcpClient client;
    static NetworkStream stream;
    static List<NetworkStream> connectedClients = new List<NetworkStream>();

    static void Main(string[] args)
    {
        server = new TcpListener(IPAddress.Any, 12345);
        server.Start();
        Console.WriteLine("Server avviato. In attesa di connessioni...");

        while (true)
        {
            client = server.AcceptTcpClient();
            stream = client.GetStream();

            // Aggiungi il client alla lista dei client connessi
            connectedClients.Add(stream);

            Thread clientThread = new Thread(() => ComunicazioneClient(stream));
            clientThread.Start();
            Console.WriteLine("Client {0} connesso", connectedClients.Count);
        }
    }

    static void ComunicazioneClient(NetworkStream clientStream)
    {
        byte[] buffer = new byte[1024];
        int bytesRead;

        while ((bytesRead = clientStream.Read(buffer, 0, buffer.Length)) > 0)
        {
            string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            Console.WriteLine("Messaggio dal client: {0}", message);

            // Invia il messaggio a tutti i client connessi (tranne il mittente)
            BroadcastMessage(message, clientStream);

            Array.Clear(buffer, 0, buffer.Length);
        }

        connectedClients.Remove(clientStream);

        clientStream.Close();
        client.Close();
    }

    static void BroadcastMessage(string message, NetworkStream senderStream)
    {
        byte[] buffer = Encoding.ASCII.GetBytes(message);

        foreach (var clientStream in connectedClients)
        {
            // Non inviare il messaggio al mittente
            if (clientStream != senderStream)
            {
                clientStream.Write(buffer, 0, buffer.Length);
                clientStream.Flush();
            }
        }
    }
}
