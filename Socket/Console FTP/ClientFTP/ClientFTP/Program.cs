using System;
using System.Net.Sockets;
using System.Text;

class TCPClient
{
    static void Main()
    {
        TcpClient client = null;

        try
        {
            string ipAddress = "127.0.0.1";
            int port = 2121;

            client = new TcpClient(ipAddress, port);
            Console.WriteLine("Connesso al server su {0}:{1}", ipAddress, port);

            NetworkStream stream = client.GetStream();

            byte[] buffer = new byte[1024];

            while (true)
            {
                Console.Write("\nInserisci un comando (GetList, ReadFile, Exit per terminare): ");
                string comando = Console.ReadLine();

                // Invio comando al server
                byte[] comandoBuffer = Encoding.ASCII.GetBytes(comando);
                stream.Write(comandoBuffer, 0, comandoBuffer.Length);

                // Ricezione risposta dal server
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Console.Clear();
                Console.WriteLine($"Risposta dal server {ipAddress}:{port} \n{response}");

                if (comando.ToLower() == "exit")
                    break;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Errore: {0}", e.Message);
        }
        finally
        {
            // Chiudi la connessione
            client.Close();
        }
    }
}
