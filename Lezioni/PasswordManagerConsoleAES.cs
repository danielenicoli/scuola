using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

class Program
{
    static void Main()
    {
        string passwordInChiaro = "Questo è il testo da cifrare";
        string masterPassword = "masterPassword";
        string inputPassword = "";

        // Cifratura
        string encrypted = EncryptText(passwordInChiaro, masterPassword);
        Console.WriteLine("Password cifrata: {0}", encrypted);

        // Autenticazione
        do
        {
            Console.WriteLine("Inserire master password per visualizzare le credenziali salvate: ");
            inputPassword = Console.ReadLine();
        } while (inputPassword != masterPassword);

        //Decifratura
        string decrypted = DecryptText(encrypted, masterPassword);
        Console.WriteLine("Password decifrata: {0}", decrypted);
    }

    static string EncryptText(string text, string password)
    {
        byte[] encrypted;
        using (Aes aesAlg = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(password, new byte[] { });
            aesAlg.Key = pdb.GetBytes(32);
            aesAlg.IV = pdb.GetBytes(16);

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(text);
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }
        }

        return Convert.ToBase64String(encrypted);
    }

    static string DecryptText(string testoCifrato, string password)
    {
        string testoDecifrato = null;
        using (Aes aesAlg = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(password, new byte[] { });
            aesAlg.Key = pdb.GetBytes(32);
            aesAlg.IV = pdb.GetBytes(16);

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(testoCifrato)))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        testoDecifrato = srDecrypt.ReadToEnd();
                    }
                }
            }
        }

        return testoDecifrato;
    }
}
