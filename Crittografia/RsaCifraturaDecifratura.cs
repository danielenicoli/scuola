using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

class Program
{
    static void Main()
    {
        try
        {
            // Genera una coppia di chiavi RSA
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                // Salva la chiave pubblica in un file
                string publicKeyXml = rsa.ToXmlString(false);
                File.WriteAllText("PublicKey.xml", publicKeyXml);

                // Salva la chiave privata in un file
                string privateKeyXml = rsa.ToXmlString(true);
                File.WriteAllText("PrivateKey.xml", privateKeyXml);
            }

            string plaintext = File.ReadAllText("Plaintext.txt");

            // Carica la chiave pubblica dal file
            string publicKey = File.ReadAllText("PublicKey.xml");
            RSACryptoServiceProvider rsaEncrypt = new RSACryptoServiceProvider();
            rsaEncrypt.FromXmlString(publicKey);

            // Crittografa il testo
            byte[] encryptedData = rsaEncrypt.Encrypt(Encoding.UTF8.GetBytes(plaintext), true);

            // Salva il testo crittografato su un file
            File.WriteAllBytes("Encrypted.txt", encryptedData);

            // Carica la chiave privata dal file
            string privateKey = File.ReadAllText("PrivateKey.xml");
            RSACryptoServiceProvider rsaDecrypt = new RSACryptoServiceProvider();
            rsaDecrypt.FromXmlString(privateKey);

            // Decrittografa il testo
            byte[] decryptedData = rsaDecrypt.Decrypt(encryptedData, true);

            // Stampa il testo decrittografato
            Console.WriteLine("Testo decifrato: " + Encoding.UTF8.GetString(decryptedData));
        }
        catch (Exception e)
        {
            Console.WriteLine("Errore: " + e.Message);
        }
    }
}
