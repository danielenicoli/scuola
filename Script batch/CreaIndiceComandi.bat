@ECHO OFF
REM.-- Prepare the Command Processor
SETLOCAL ENABLEEXTENSIONS

REM --
REM -- Copyright note
REM -- This script is provided as is. No waranty is made, whatso ever.
REM -- You may use and modify the script as you like, but keep the version history with
REM -- recognition to http://www.dostips.com in it.
REM --

REM Version History:
REM XX.XXX YYYYMMDD Author Description
SET "version=02.000" &:20080316 p.h. SET "version=%version: =%"

for /f "delims=: tokens=2" %%a in ('chcp') do set "restore_codepage=%%a"
chcp 1252>NUL

set "z=%~dpn0.htm"

echo.^<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"^> >"%z%"

set "title=INDICE DEI COMANDI DOS"
for /f "tokens=*" %%a in ('ver') do set "winver=%%a"

:: --------------------------------------------------------------
echo.Creazione delle intestazioni ...
for %%A in (
            ""
            "<html><title>%title%</title>"
            "<body bgcolor='#FFFFCC'>"
                                                                                                             "<font color='darkblue'>"
                                                                                                             "<center>"
                                                                                                             "<h1>%title%</h1>"
                                                                                                             "<table border=1 cellspacing=1 cellpadding=3>"
                                                                                                             " <tr><td>Versione Windows</td><td>:</td><td>%winver%</td></tr>"
                                                                                                             " <tr><td>Documento originale</td><td>:</td><td><a href='http://www.dostips.com/%~n0.php'>"
                                                                                                             " <b>http://www.dostips.com/%~n0.php</b></a></td></tr>"
                                                                                                             " <tr><td>Creato da</td><td>:</td><td><a href='http://www.dostips.com/%~nx0'>"
                                                                                                             " <b>%~nx0</b></a><br><a href=#%~n0><b>Codice sorgente batch sottostante</b></a></td></tr>"
                                                                                                             "</table>"
                                                                                                             "<br><br>"
                                                                                                             "<table>"
) do echo.%%~A>>"%z%"

:: --------------------------------------------------------------
echo.Creazione dell'indice ...
set /a cnt=0
for /f "tokens=1,*" %%a in ('"help|findstr /v /b /c:" " /c:"riferimento" /c:"Per ulteriori""') do (
                                           for %%A in (
                                                                                                                                                                   " <tr><td><a href='#%%a'>%%a</a></td><td>%%b</td></tr>"
     ) do echo.%%~A>>"%z%"
     set /a cnt+=1
)
for %%A in (
                                                                                                             "</table>"
                                                                                                             "<br><br>"
                                                                                                             "</center>"
) do echo.%%~A>>"%z%"

:: --------------------------------------------------------------
echo.Estrazione del testo dell'HELP ...
call:initProgress cnt
for /f %%a in ('"help|findstr /v /b /c:" " /c:"riferimento" /c:"Per ulteriori""') do (
                                                                                                             echo.Elaborazione di: %%a
                                                                                                             for %%A in (
                                                                                                                                                                                                                               "<div style='float: right'><a href='#'>TOP</a></div>"
                                                                                                                                                                                                                               "<center><h2><a name='%%a'>%%a</a></h2></center>"
                                                                                                                                                                                                                               "<div style='background: #F8F8FF'><pre><xmp>"
                                                                                                             ) do echo.%%~A>>"%z%"
                                                                                                             call help %%a >>"%z%" 2>&1
                                                                                                             echo ^</xmp^> >>"%z%"
                                                                                                             for %%A in (
                                                                                                                                                                                                                               "</pre></div>"
                                                                                                             ) do echo.%%~A>>"%z%"
                                                                                                             call:tickProgress
)

:: --------------------------------------------------------------
echo.Aggiunta del sorgente relativo allo script di creazione ...
for %%A in (
                                                                                                             ""
                                                                                                             "<center>"
                                                                                                             "<br><br>"
                                                                                                             "<div style='float: right'><a href='#'>TOP</a></div>"
                                                                                                             "<a name='%~n0'><h2>DOS Batch Script con il quale è stato creato questo documento</h2></a>"
                                                                                                             "Questo indice è stato creato automaticamente il %date% alle %time% dal seguente script batch:"
                                                                                                             "<br><br>"
                                                                                                             "</center>"
                                                                                                             "<div style='background: #000000; color: #FFFFFF;'><pre><xmp>"
) do echo.%%~A>>"%z%"
type "%~f0" >>"%z%"

:: --------------------------------------------------------------
echo.Creazione del pie di pagina ...
echo ^</xmp^> >>"%z%"
for %%A in (
                                                                                                             "</pre></div>"
                                                                                                             "</center>"
                                                                                                             ""
                                                                                                             "</font>"
                                                                                                             "</body>"
                                                                                                             "</html>"
) do echo.%%~A>>"%z%"


chcp %restore_codepage%>NUL
explorer "%z%"

:SKIP
REM.-- End of application
FOR /l %%a in (5,-1,1) do (TITLE %title% -- closing in %%as&ping -n 2 -w 1 127.0.0.1>NUL)
TITLE Press any key to close the application&ECHO.&GOTO:EOF


::-----------------------------------------------------------
::helper functions follow below here
::-----------------------------------------------------------


:initProgress -- initialize an internal progress counter and display the progress in percent
:: -- %~1: in - progress counter maximum, equal to 100 percent
:: -- %~2: in - title string formatter, default is '[P] completed.'
set /a "ProgressCnt=-1"
set /a "ProgressMax=%~1"
set "ProgressFormat=%~2"
if "%ProgressFormat%"=="" set "ProgressFormat=[PPPP]"
set "ProgressFormat=%ProgressFormat:[PPPP]=[P] completed.%"
call :tickProgress
GOTO:EOF


:tickProgress -- display the next progress tick
set /a "ProgressCnt+=1"
SETLOCAL
set /a "per=100*ProgressCnt/ProgressMax"
set "per=%per%%%"
call title %%ProgressFormat:[P]=%per%%%
GOTO:EOF