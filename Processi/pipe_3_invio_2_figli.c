#include <stdio.h>fd1
#include <stdlib.h>
#include <unistd.h>

int main() {
    int fd1[2], fd2[2];
    pid_t child_pid1, child_pid2;

    if (pipe(fd1) == -1) {
        perror("Errore nella creazione della pipe");
        exit(EXIT_FAILURE);
    }

    if (pipe(fd2) == -1) {
        perror("Errore nella creazione della pipe");
        exit(EXIT_FAILURE);
    }

    if ((child_pid1 = fork()) == -1) {
        perror("Errore nell'esecuzione della fork");
        exit(EXIT_FAILURE);
    }

    if (child_pid1 > 0) {
        // Fork del secondo figlio
        if ((child_pid2 = fork()) == -1) {
            perror("Errore nell'esecuzione della fork");
            exit(EXIT_FAILURE);
        }

        if (child_pid2 > 0) {
            close(fd1[0]); // Chiudere il lato di lettura della prima pipe nel padre
            close(fd2[0]); // Chiudere il lato di lettura della seconda pipe nel padre

            // Valori da inviare ai 2 processi figli
            int valori_da_inviare[] = {10, 20, 30};

            write(fd1[1], valori_da_inviare, sizeof(valori_da_inviare));
            close(fd1[1]);

            write(fd2[1], valori_da_inviare, sizeof(valori_da_inviare));
            close(fd2[1]);

            wait(NULL);
            wait(NULL);
        } else {  
            // Codice del secondo processo figlio
            close(fd1[0]); // Chiudere la prima pipe nel secondo figlio sia in lettura che in scrittura
            close(fd1[1]);
            close(fd2[1]); // Chiudere il lato di scrittura della seconda pipe nel secondo figlio

            // Buffer per ricevere i valori dalla seconda pipe
            int valori_ricevuti[3];

            read(fd2[0], valori_ricevuti, sizeof(valori_ricevuti));
            close(fd2[0]);

            printf("Valori inviati dal padre al secondo figlio: %d, %d, %d\n", valori_ricevuti[0], valori_ricevuti[1], valori_ricevuti[2]);
        }
    } else {  
        // Codice del primo processo figlio
        close(fd1[1]); // Chiudere il lato di scrittura della prima pipe nel primo figlio
        close(fd2[0]); // Chiudere la seconda pipe nel primo figlio
        close(fd2[1]);

        int valori_ricevuti[3];

        read(fd1[0], valori_ricevuti, sizeof(valori_ricevuti));
        close(fd1[0]);

        printf("Valori inviati dal padre al secondo figlio: %d, %d, %d\n", valori_ricevuti[0], valori_ricevuti[1], valori_ricevuti[2]);
    }

    return 0;
}
