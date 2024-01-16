#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>

int main() {
    int fd[2];
    pid_t child_pid;

    if (pipe(fd) == -1) {
        perror("Errore nella creazione della pipe");
        exit(EXIT_FAILURE);
    }

    if ((child_pid = fork()) == -1) {
        perror("Errore nella fork");
        exit(EXIT_FAILURE);
    }

    if (child_pid > 0) { 
        close(fd[0]);

        int valori_da_inviare[] = {10, 20, 30};

        write(fd[1], valori_da_inviare, sizeof(valori_da_inviare));
        close(fd[1]);

        wait(NULL);
    } else {
        close(fd[1]);

        int valori_ricevuti[3];

        read(fd[0], valori_ricevuti, sizeof(valori_ricevuti));
        close(fd[0]);

        printf("Valori ricevuti dal genitore: %d, %d, %d\n", valori_ricevuti[0], valori_ricevuti[1], valori_ricevuti[2]);
    }

    return 0;
}
