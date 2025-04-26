@echo off
setlocal enabledelayedexpansion

echo ===================================================
echo Installation de IRIS-Forensic X avec dotnet CLI
echo ===================================================

:: Vérifier si dotnet est installé
where dotnet >nul 2>nul
if %ERRORLEVEL% NEQ 0 (
    echo ERREUR: .NET SDK n'est pas installé ou n'est pas dans le PATH.
    echo Veuillez installer .NET SDK depuis https://dotnet.microsoft.com/download
    pause
    exit /b 1
)

:: Vérifier si Python est installé
where python >nul 2>nul
if %ERRORLEVEL% NEQ 0 (
    echo ERREUR: Python n'est pas installé ou n'est pas dans le PATH.
    echo Veuillez installer Python depuis https://www.python.org/downloads/
    pause
    exit /b 1
)

:: Installer les dépendances Python
echo Installation des dépendances Python...
python -m pip install -r ..\requirements.txt
if %ERRORLEVEL% NEQ 0 (
    echo AVERTISSEMENT: Certaines dépendances Python n'ont pas pu être installées.
    echo L'application pourrait ne pas fonctionner correctement.
    pause
)

:: Restaurer et compiler avec dotnet
echo Restauration des packages NuGet avec dotnet...
dotnet restore IRIS.sln
if %ERRORLEVEL% NEQ 0 (
    echo ERREUR: Impossible de restaurer les packages NuGet.
    pause
    exit /b 1
)

echo Compilation de la solution avec dotnet...
dotnet build IRIS.sln --configuration Release
if %ERRORLEVEL% NEQ 0 (
    echo ERREUR: Échec de la compilation.
    pause
    exit /b 1
)

echo ===================================================
echo Compilation terminée avec succès!
echo ===================================================
echo L'exécutable se trouve dans: IRIS.Windows\bin\Release\net6.0-windows\IRIS-Forensic-X.exe
echo.
echo Vous pouvez maintenant lancer l'application.
echo.

pause
