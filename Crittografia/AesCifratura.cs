using System;
using System.IO;
using System.Security.Cryptography;
using System.Text; // Encoding.UTF8

try
{
    byte[] key = Encoding.UTF8.GetBytes("abcdefghijklmnop"); // Chiave da 128 bit = 16 bytes

    using (Aes aes = Aes.Create())
    {
        aes.Key = key; // Impostazione della chiave (in byte)

        File.WriteAllBytes("Key.txt", aes.Key); // Salvataggio chiave in un file

        if (!File.Exists("TestData.txt"))
        {
            byte[] stringa = Encoding.UTF8.GetBytes("Hello world");
            File.WriteAllBytes("TestData.txt", stringa);
        }

        byte[] plainText = File.ReadAllBytes("TestData.txt");

        // Cifratura dei dati
        using (MemoryStream memoryStream = new())
        {
            // Scrittura del vettore di inizializzazione (IV) all'inizio del file cifrato
            memoryStream.Write(aes.IV, 0, aes.IV.Length);

            using (CryptoStream cryptoStream = new(
                memoryStream,
                aes.CreateEncryptor(),
                CryptoStreamMode.Write))
            {
                cryptoStream.Write(plainText, 0, plainText.Length);
                cryptoStream.FlushFinalBlock();
            }

            // Sovrascrivi il file di testo con i dati cifrati
            File.WriteAllBytes("TestData.txt", memoryStream.ToArray());
        }

        Console.WriteLine("Il file è stato cifrato");
    }
}
catch (Exception ex)
{
    Console.WriteLine("La cifratura è fallita " + ex);
}