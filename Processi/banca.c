#include "stdio.h"
#include "stdlib.h"

char NOME[]="saldo.txt";

int leggi() {
  FILE * fp = fopen(NOME, "r");
  if(fp==NULL) {
    printf("Errore nell'apertura del file %s", NOME);
    return -1;
  }
  
  char buffer[10];
  fgets(buffer, sizeof(buffer), fp);
  
  fclose(fp);
  return atoi(buffer);
}

int scrivi(int VAL) {
  FILE * fp = fopen(NOME, "w");
  
  if(fp==NULL) {
    printf("Errore nell'apertura del file %s", NOME);
    return -1;
  }
  
  fprintf(fp, "%d", VAL);
  return 1;
}



int main() {
  pid_t pid = fork();
  
  if(pid<0) {
    printf("Errore nella creazione del processo");
  }
  else if(pid==0) {
    //Figlio --> DEPOSITA
    int saldo = leggi();
    int nuovo_deposito = 19;
    saldo += nuovo_deposito;
    if(scrivi(saldo)) printf("Deposito registrato");
  }
  else {
    //Padre --> PRELEVA
    wait(NULL);
    int saldo = leggi();
    int prelievo = 10;
    if(saldo >= prelievo) {
      saldo -= prelievo;
      if(scrivi(saldo)) printf("Prelievo registrato");
    }
    else {
      printf("Saldo insufficiente per il prelievo");
    }
  }
  
  return 1;
}