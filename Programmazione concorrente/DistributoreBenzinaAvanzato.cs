using System;
using System.Threading;

namespace DistributoreBenzinaAvanzato
{
    public class DistributoreBenzina
    {
        private int numeroPompe;
        private int capacitaSerbatoio;
        private int livelloSerbatoio;
        private int numRifornimentiSerbatoio;
        private Semaphore semaforoPompe;
        private Mutex mutexRifornimento;
        private Mutex mutexLivelloSerbatoio;

        public DistributoreBenzina(int numeroPompe, int capacitaSerbatoio)
        {
            this.numeroPompe = numeroPompe;
            this.capacitaSerbatoio = capacitaSerbatoio;
            this.numRifornimentiSerbatoio = 3;
            this.livelloSerbatoio = 0;
            this.semaforoPompe = new Semaphore(numeroPompe, numeroPompe);
            this.mutexRifornimento = new Mutex();
            this.mutexLivelloSerbatoio = new Mutex();
        }

        public void ArrivaAuto(int quantitaRichiesta)
        {
            Console.WriteLine("Arriva l'auto {0} che richiede {1} litri di benzina", Thread.CurrentThread.Name, quantitaRichiesta);

            mutexLivelloSerbatoio.WaitOne();
            if (quantitaRichiesta <= livelloSerbatoio)
            {
                livelloSerbatoio -= quantitaRichiesta;
                mutexLivelloSerbatoio.ReleaseMutex();
                Thread.Sleep(2000);
                Console.WriteLine("L'auto {0} ha fatto benzina con successo", Thread.CurrentThread.Name);
            }
            else
            {
                mutexLivelloSerbatoio.ReleaseMutex();
                Console.WriteLine("L'auto {0} deve aspettare. Carburante insufficiente", Thread.CurrentThread.Name);
            }
        }


        public void RifornisciSerbatoio()
        {
            for (int i = 0; i < numRifornimentiSerbatoio; i++)
            {
                mutexRifornimento.WaitOne();

                mutexLivelloSerbatoio.WaitOne();
                livelloSerbatoio = capacitaSerbatoio;
                Console.WriteLine("Il serbatoio è stato rifornito con successo. Livello: {0}", livelloSerbatoio);
                mutexLivelloSerbatoio.ReleaseMutex();

                mutexRifornimento.ReleaseMutex();
                Thread.Sleep(5000);
            }
        }
    }
}
