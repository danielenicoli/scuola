using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

// Digest prodotto da "La 5AI è la classe migliore (forse) :)"
byte[] sentHashValue = Convert.FromHexString("C77755865D76D07B77E8C683BB2716D6311D56D48F4E0DF31CED739F15FF9EAD"); 

// Stringa che dovrebbe corrispondere all'hash
string messageString = "La 5AI è la classe migliore (forse) :)";
byte[] messageBytes = Encoding.UTF8.GetBytes(messageString);

// Crea un hash a partire dalla stringa da confrontare
byte[] compareHashValue = SHA256.HashData(messageBytes);

bool same = sentHashValue.SequenceEqual(compareHashValue);

if (same)
    Console.WriteLine("I digest corrispondono, le due stringhe sono uguali");
else
    Console.WriteLine("I digest non corrispondono, la stringa è stata alterata");