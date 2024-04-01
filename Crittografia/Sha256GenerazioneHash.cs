using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

string messageString = "La 5AI è la classe migliore (forse) :)";

byte[] messageBytes = Encoding.UTF8.GetBytes(messageString);

byte[] hashValue = SHA256.HashData(messageBytes);

Console.WriteLine("Hash: " + Convert.ToHexString(hashValue));