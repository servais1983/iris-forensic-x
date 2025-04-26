@echo off
setlocal enabledelayedexpansion

echo ===================================================
echo Desinstallation de IRIS-Forensic X
echo ===================================================
echo.

:: Vérification des privilèges administrateur
net session >nul 2>&1
if %errorLevel% neq 0 (
    echo ERREUR: La desinstallation necessite des privileges administrateur.
    echo Veuillez relancer ce script en tant qu'administrateur.
    pause
    exit /b 1
)

echo Cette operation va supprimer IRIS-Forensic X et toutes ses donnees.
echo Les rapports generes seront conserves dans %APPDATA%\IRIS-Forensic-X\Reports
echo.
set /p CONFIRM=Etes-vous sur de vouloir continuer? (O/N): 

if /i "%CONFIRM%" neq "O" (
    echo Desinstallation annulee.
    pause
    exit /b 0
)

echo.
echo Desinstallation en cours...

:: Suppression des raccourcis
echo Suppression des raccourcis...
if exist "%PROGRAMDATA%\Microsoft\Windows\Start Menu\Programs\IRIS-Forensic X\IRIS-Forensic X.lnk" del /f /q "%PROGRAMDATA%\Microsoft\Windows\Start Menu\Programs\IRIS-Forensic X\IRIS-Forensic X.lnk"
if exist "%PROGRAMDATA%\Microsoft\Windows\Start Menu\Programs\IRIS-Forensic X\Désinstaller IRIS-Forensic X.lnk" del /f /q "%PROGRAMDATA%\Microsoft\Windows\Start Menu\Programs\IRIS-Forensic X\Désinstaller IRIS-Forensic X.lnk"
if exist "%PROGRAMDATA%\Microsoft\Windows\Start Menu\Programs\IRIS-Forensic X" rmdir /s /q "%PROGRAMDATA%\Microsoft\Windows\Start Menu\Programs\IRIS-Forensic X"
if exist "%PUBLIC%\Desktop\IRIS-Forensic X.lnk" del /f /q "%PUBLIC%\Desktop\IRIS-Forensic X.lnk"

:: Sauvegarde des rapports si l'utilisateur le souhaite
echo.
set /p BACKUP_REPORTS=Souhaitez-vous sauvegarder les rapports generes? (O/N): 

if /i "%BACKUP_REPORTS%" equ "O" (
    echo Sauvegarde des rapports...
    set BACKUP_DIR=%USERPROFILE%\Documents\IRIS-Forensic-X-Reports-Backup
    if not exist "%BACKUP_DIR%" mkdir "%BACKUP_DIR%"
    
    if exist "%APPDATA%\IRIS-Forensic-X\Reports" (
        xcopy /E /I /Y "%APPDATA%\IRIS-Forensic-X\Reports" "%BACKUP_DIR%"
        echo Rapports sauvegardes dans %BACKUP_DIR%
    ) else (
        echo Aucun rapport trouve.
    )
)

:: Suppression des fichiers d'application
echo Suppression des fichiers d'application...
set INSTALL_DIR=%ProgramFiles%\IRIS-Forensic X
if exist "%INSTALL_DIR%" (
    rmdir /s /q "%INSTALL_DIR%"
)

:: Suppression des données utilisateur
echo Suppression des donnees utilisateur...
set APPDATA_DIR=%APPDATA%\IRIS-Forensic-X

if /i "%BACKUP_REPORTS%" equ "O" (
    :: Si l'utilisateur a choisi de sauvegarder les rapports, on ne supprime pas le dossier Reports
    if exist "%APPDATA_DIR%\Logs" rmdir /s /q "%APPDATA_DIR%\Logs"
    if exist "%APPDATA_DIR%\CoreCLI" rmdir /s /q "%APPDATA_DIR%\CoreCLI"
    if exist "%APPDATA_DIR%\AIModels" rmdir /s /q "%APPDATA_DIR%\AIModels"
    if exist "%APPDATA_DIR%\Blockchain" rmdir /s /q "%APPDATA_DIR%\Blockchain"
    if exist "%APPDATA_DIR%\YaraRules" rmdir /s /q "%APPDATA_DIR%\YaraRules"
    if exist "%APPDATA_DIR%\bin" rmdir /s /q "%APPDATA_DIR%\bin"
    if exist "%APPDATA_DIR%\Templates" rmdir /s /q "%APPDATA_DIR%\Templates"
    if exist "%APPDATA_DIR%\Config" rmdir /s /q "%APPDATA_DIR%\Config"
) else (
    :: Si l'utilisateur n'a pas choisi de sauvegarder les rapports, on supprime tout le dossier
    if exist "%APPDATA_DIR%" rmdir /s /q "%APPDATA_DIR%"
)

:: Suppression des entrées de registre
echo Suppression des entrees de registre...
reg delete "HKCU\Software\IRIS-Forensic X" /f >nul 2>&1

echo.
echo ===================================================
echo Desinstallation terminee avec succes
echo ===================================================
echo.

pause
exit /b 0
