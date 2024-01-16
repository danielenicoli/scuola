# Chat multithread
Questo progetto comprende due programmi scritti in C# usando socket TCP e programmazione multithreaded


## Server
Il programma server gestisce la connessione di n client usando i thread. Riceve i messaggi da tutti i client connessi e si occupa di inoltrarli a tutti i client con cui ha aperto un NetworkStream tranne che al client mittente del messaggio

## Client
Il client si connette al server e gli invia i messaggi da recapitare agli altri client connessi. Contemporaneamente, è in grado di ricevere i messaggi dagli altri client (tramite thread)

## Usage
Avviare il server, quindi n istanze del client

![Esempio di esecuzione](Esempio%20di%20esecuzione.png)

## Roadmap
- [ ] Miglioramento della gestione e della terminazione delle connessioni
- [ ] Aggiunta interfaccia grafica 

## Authors and acknowledgment
Daniele Nicoli

## License
Creative commons

## Project status
In revisione
