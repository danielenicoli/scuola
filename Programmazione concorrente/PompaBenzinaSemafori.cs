using System;
using System.Threading;

class Program
{
    // Creazione di un semaforo (benzinaio) che gestisce due risorse (pompe di benzina)
    static Semaphore benzinaio = new Semaphore(2, 2);
    static int NUM_AUTO = 10;

    static void Main()
    {
        Console.WriteLine("La stazione di benzina è aperta: 2 pompe disponibili");

        // Simulazione arrivo di NUM_AUTO auto
        for (int i = 1; i <= NUM_AUTO; i++)
        {
            Thread auto = new Thread(new ParameterizedThreadStart(Rifornimento));
            auto.Start(i);
        }

        Console.ReadLine();
    }

    private static void Rifornimento(object numeroAuto)
    {
        Console.WriteLine("L'auto {0} sta arrivando alla stazione di benzina", numeroAuto);

        // Richiesta del permesso per usare una delle pompe
        benzinaio.WaitOne();

        Console.WriteLine("L'auto {0} sta utilizzando una pompa", numeroAuto);
        Thread.Sleep(new Random().Next(1000, 3000));

        Console.WriteLine("L'auto {0} ha finito di rifornire e lascia la pompa", numeroAuto);

        // Rilascio del semaforo (pompa di benzina diventa disponibile per un'altra auto)
        benzinaio.Release();
    }
}