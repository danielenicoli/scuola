using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class TCPServer
{
    static void Main()
    {
        TcpListener server = null;

        try
        {
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            int port = 2121;

            server = new TcpListener(ipAddress, port);

            server.Start();
            Console.WriteLine("Server in ascolto su {0}:{1}", ipAddress, port);

            TcpClient client = server.AcceptTcpClient();
            Console.WriteLine("Client connesso!");

            NetworkStream stream = client.GetStream();

            byte[] buffer = new byte[1024];

            int bytesRead;
            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                string comando = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Console.WriteLine("Comando ricevuto: {0}", comando);

                // Esegui comando e invia risposta al client
                string response = EseguiComando(comando);
                byte[] responseBuffer = Encoding.ASCII.GetBytes(response);
                stream.Write(responseBuffer, 0, responseBuffer.Length);

                // Se il comando è "exit" termina il ciclo di lettura
                if (comando.Trim().ToLower() == "exit")
                    break;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Errore: {0}", e.Message);
        }
        finally
        {
            server.Stop();
        }
    }

    static string EseguiComando(string comando)
    {
        if (comando.ToLower() == "getlist")
        {
            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory());
            return string.Join("\n", files);
        }
        else if (comando.ToLower().StartsWith("readfile"))
        {
            // Estrazione nome del file dalla sintassi ReadFile [nomeFile]
            string fileName = comando.Substring("readfile".Length).Trim();
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);

            if (File.Exists(filePath))
            {
                string fileContent = File.ReadAllText(filePath);
                return fileContent;
            }
            else
            {
                return $"Il file '{filePath}' non esiste.";
            }
        }
        else
        {
            return "Comando sconosciuto";
        }
    }
}
