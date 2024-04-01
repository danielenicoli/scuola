using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace SendTelegramMessage
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string botToken = "TUO_BOT_TOKEN";
            long chatId = TUO_CHAT_ID_NUMERICO;
            Console.WriteLine("Scrivi il messaggio da inviare: ");
            string messageText = Console.ReadLine();

            await SendMessageAsync(botToken, chatId, messageText);
        }

        static async Task SendMessageAsync(string botToken, long chatId, string messageText)
        {
            string url = "https://api.telegram.org/bot" + botToken + "/sendMessage";
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("chat_id", chatId.ToString()),
                new KeyValuePair<string, string>("text", messageText)
            });

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.PostAsync(url, content);
                    response.EnsureSuccessStatusCode();

                    string responseBody = await response.Content.ReadAsStringAsync();
                    var responseJson = JsonSerializer.Deserialize<JsonElement>(responseBody);

                    if (responseJson.GetProperty("ok").GetBoolean())
                    {
                        Console.WriteLine("Messaggio '{0}' inviato con successo!", messageText);
                    }
                    else
                    {
                        Console.WriteLine("Errore nell'invio del messaggio:");
                        Console.WriteLine(responseBody);
                    }
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("Message: {0}", e.Message);
                }
            }
        }
    }
}