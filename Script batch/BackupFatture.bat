@ECHO OFF
CLS

REM Gli spazi prima dell'assegnazione sono interpretati come nome della variabile

SET data=%date:~6,4%%date:~3,2%%date:~0,2%_%time:~0,2%%time:~3,2%
SET cartella_origine=%USERPROFILE%\Desktop\Fatture
SET cartella_backup=%USERPROFILE%\Desktop\Backup\


IF EXIST %cartella_origine% (GOTO :BACKUP) ELSE GOTO :ERRORE


:ERRORE
echo Impossibile eseguire il backup della cartella %cartella_origine%
echo [%data%] Errore nell'esecuzione del backup >> %cartella_backup%\log.txt
GOTO :FINE

:BACKUP
echo Backup della cartella %cartella_backup% in corso...
mkdir %cartella_backup%\%data%_Fatture
xcopy %cartella_origine%\*.pdf %cartella_backup%\%data%_Fatture /Y
echo [%data%] Backup effettuato con successo >> %cartella_backup%\log.txt

:FINE

