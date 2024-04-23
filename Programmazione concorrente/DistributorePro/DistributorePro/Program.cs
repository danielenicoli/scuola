using System;
using System.Threading;

class Programma
{
    static int NUM_AUTO = 50;
    static List<Thread> clienti = new List<Thread>();

    static void Main()
    {
        DistributoreBenzina distributore = new DistributoreBenzina(1000, 2);

        // Simulazione arrivo auto
        Random rand = new Random();
        
        for (int i = 0; i < NUM_AUTO; i++)
        {
            Thread auto = new Thread(() => distributore.Rifornisci(rand.Next(100, 150)));
            auto.Start();
            clienti.Add(auto);
        }

        Thread autobotte = new Thread(() => distributore.RiempiCisterna());
        autobotte.Start();
                

        foreach (Thread item in clienti)
        {
            item.Join();
        }

        Console.WriteLine("Premi un tasto per uscire...");
        Console.ReadKey();
    }
}