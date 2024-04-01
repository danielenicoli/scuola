using System;
using System.IO;
using System.Security.Cryptography;

try
{
    byte[] key = File.ReadAllBytes("Key.txt");

    byte[] testoCifratoConIV = File.ReadAllBytes("TestData.txt");

    // Estrazione e rimozione di IV dalla parte cifrata
    byte[] iv = new byte[16];
    Array.Copy(testoCifratoConIV, 0, iv, 0, iv.Length);

    // Rimozione di IV dalla parte cifrata
    byte[] testoCifrato = new byte[testoCifratoConIV.Length - iv.Length];
    Array.Copy(testoCifratoConIV, iv.Length, testoCifrato, 0, testoCifrato.Length);

    // Decifra i dati utilizzando la chiave e il vettore di inizializzazione
    using (Aes aes = Aes.Create())
    {
        aes.Key = key;
        aes.IV = iv;

        using (MemoryStream memoryStream = new(testoCifrato))
        {
            using (CryptoStream cryptoStream = new(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
            {
                using (StreamReader decryptReader = new(cryptoStream))
                {
                    string testoDecifrato = decryptReader.ReadToEnd();
                    Console.WriteLine("Dati decifrati: " + testoDecifrato);
                }
            }
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine("La decifratura è fallita " + ex);
}