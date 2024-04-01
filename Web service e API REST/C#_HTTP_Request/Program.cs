using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;

namespace GetHttpRequest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Inserisci il codice a barre:");
            string barcode = Console.ReadLine();

            await GetProductData(barcode);
        }

        static async Task GetProductData(string barcode)
        {
            string url = "https://world.openfoodfacts.net/api/v2/product/" + barcode + "/?fields=product_name,brands,quantity,image_url,ingredients_text";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    if((int)response.StatusCode == 200)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();

                        var jsonDocument = JsonDocument.Parse(responseBody);
                        var root = jsonDocument.RootElement;
                        var product = root.GetProperty("product");

                        Console.WriteLine("\nNome prodotto: " + product.GetProperty("product_name").GetString());
                        Console.WriteLine("Marca: " + product.GetProperty("brands").GetString());
                        Console.WriteLine("Quantità: " + product.GetProperty("quantity").GetString());
                        Console.WriteLine("URL immagine: " + product.GetProperty("image_url").GetString());
                        Console.WriteLine("Ingredienti: " + product.GetProperty("ingredients_text").GetString());
                    }
                    
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("Errore: {0}", e.Message);
                }
            }
        }
    }
}
