using System;
using System.Threading;

class Program
{
    static Mutex mutex = new Mutex(false, "MyMutex");

    static void Main(string[] args)
    {
        // Controllo se un altro processo ha già acquisito il mutex
        if (!mutex.WaitOne(TimeSpan.FromSeconds(1), false))
        {
            Console.WriteLine("Un altro processo sta utilizzando l'applicazione");
            Console.ReadLine();
        }

        try
        {
            Console.WriteLine("Mutex acquisito, processo in esecuzione...");
            // Simulazione di un lavoro lungo
            Thread.Sleep(10000);
        }
        finally
        {
            // Rilascio del mutex quando il lavoro è completo
            mutex.ReleaseMutex();
            Console.WriteLine("Mutex rilasciato");
        }
    }
}