using System;
using System.IO;
using System.Threading;

class Program
{
    private static Mutex mutex = new Mutex();
    private static string filePath = "fileCondiviso.txt";

    static void Main(string[] args)
    {
        Thread t1 = new Thread(() => ScriviNelFile("Primo thread"));
        Thread t2 = new Thread(() => ScriviNelFile("Secondo thread"));
        Thread t3 = new Thread(() => ScriviNelFile("Terzo thread"));

        t1.Start();
        t2.Start();
        t3.Start();

        t1.Join();
        t2.Join();
        t3.Join();
    }

    static void ScriviNelFile(string messaggio)
    {
        for (int i = 0; i < 5; i++)
        {
            // Aspetta finché non otteniamo il Mutex
            mutex.WaitOne();

            try
            {
                using (StreamWriter sw = File.AppendText(filePath))
                {
                    sw.WriteLine("{0} scrive: {1}", messaggio, i);
                    Console.WriteLine("{0} scrive: {1}", messaggio, i);
                }
            }
            finally
            {
                // Rilascia il Mutex, permettendo ad altri processi di utilizzare la risorsa
                mutex.ReleaseMutex();
            }
        }
    }
}