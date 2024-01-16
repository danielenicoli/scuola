#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <unistd.h>

int main() {
    int fd[2];
    pid_t child_pid;

    char buffer[50];    //Buffer per lettura stringa

    if(pipe(fd) == -1) {
        printf("Errore nella creazione della pipe");
        exit(EXIT_FAILURE);
    }
    
    if(child_pid = fork() == -1) {
        printf("Errore nella creazione del processo figlio");
        exit(EXIT_FAILURE);
    }

    if(child_pid == 0) {
        char message[] = "Ciao mondo!";
        close(fd[0]);
        write(fd[1], message, strlen(message)+1);
        printf("Processo figlio ha inviato: %d\n", message);
        close(fd[1]);
    }
    else {
        close(fd[1]);
        read(fd[0], buffer, sizeof(buffer));
        printf("Processo padre ha letto: %s\n", buffer);
        close(fd[0]);
    }
    
    return 0;
}