using System;
using System.IO;
using System.Threading;

class Program
{
    // Oggetto su cui eseguire il lock
    private static readonly object lockObject = new object();
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
            // Utilizza il lock sull'oggetto specificato
            lock (lockObject)
            {
                using (StreamWriter sw = File.AppendText(filePath))
                {
                    sw.WriteLine("{0} scrive: {1}", messaggio, i);
                    Console.WriteLine("{0} scrive: {1}", messaggio, i);
                }

                // Il blocco viene automaticamente rilasciato al termine del blocco lock
            }

            // Simula un po' di lavoro
            Thread.Sleep(500);
        }
    }
}
