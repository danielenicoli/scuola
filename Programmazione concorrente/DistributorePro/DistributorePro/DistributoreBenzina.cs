using System;
using System.Threading;

class DistributoreBenzina
{
    private int capacitaCisterna;
    private int benzinaDisponibile;
    private int numeroPompe;
    private SemaphoreSlim semaforoPompe;
    private object bloccoCisterna = new object();

    public DistributoreBenzina(int capacita, int numPompe)
    {
        capacitaCisterna = capacita;
        benzinaDisponibile = capacita;  // Si assume che all'inizio la cisterna sia piena
        numeroPompe = numPompe;
        semaforoPompe = new SemaphoreSlim(numPompe, numPompe);
    }

    public void Rifornisci(int litriRichiesti)
    {
        // Attende che una pompa si liberi
        semaforoPompe.Wait();
        try
        {
            bool rifornimentoCompletato = false;
            while (!rifornimentoCompletato)
            {
                //mutex.WaitOne();
                lock (bloccoCisterna)
                {
                    if (benzinaDisponibile >= litriRichiesti)
                    {
                        benzinaDisponibile -= litriRichiesti;
                        Thread.Sleep(1000);
                        rifornimentoCompletato = true;
                        Console.WriteLine("Richiesta di: {0} litri \t Benzina rimanente: {1} litri, rifornimento effettuato", litriRichiesti, benzinaDisponibile);
                    }
                }
                //mutex.ReleaseMutex();
                if (!rifornimentoCompletato)
                {
                    Console.WriteLine("Richiesta di: {0} litri \t Benzina insufficiente: in attesa...", litriRichiesti);
                    Thread.Sleep(5000);  // Verifica nuovamente dopo 5 secondi
                }
            }
        }
        finally
        {
            // Rilasciare la pompa dopo il rifornimento
            semaforoPompe.Release();
        }
    }

    public void RiempiCisterna()
    {
        while (true)
        {
            if (benzinaDisponibile != capacitaCisterna)
            {
                lock (bloccoCisterna)
                {
                    benzinaDisponibile = capacitaCisterna;
                    Console.WriteLine("Cisterna riempita \t\t Disponibili: {0} litri", benzinaDisponibile);
                }
            }
            Thread.Sleep(15000);
        }
    }
}