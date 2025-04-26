@echo off
setlocal enabledelayedexpansion

echo ===================================================
echo Installation des dependances pour IRIS-Forensic X
echo ===================================================
echo.

:: Vérification des privilèges administrateur
net session >nul 2>&1
if %errorLevel% neq 0 (
    echo ERREUR: L'installation des dependances necessite des privileges administrateur.
    echo Veuillez relancer ce script en tant qu'administrateur.
    pause
    exit /b 1
)

echo Verification de l'installation de Python...
where python >nul 2>&1
if %errorLevel% neq 0 (
    echo Python n'est pas installe ou n'est pas dans le PATH.
    echo Installation de Python en cours...
    
    :: Téléchargement de Python
    echo Telechargement de Python...
    powershell -Command "& {Invoke-WebRequest -Uri 'https://www.python.org/ftp/python/3.10.0/python-3.10.0-amd64.exe' -OutFile '%TEMP%\python-installer.exe'}"
    
    if %errorLevel% neq 0 (
        echo ERREUR: Impossible de telecharger Python.
        echo Veuillez installer Python 3.10 ou superieur manuellement depuis https://www.python.org/downloads/
        pause
        exit /b 1
    )
    
    :: Installation de Python
    echo Installation de Python...
    %TEMP%\python-installer.exe /quiet InstallAllUsers=1 PrependPath=1 Include_test=0
    
    if %errorLevel% neq 0 (
        echo ERREUR: L'installation de Python a echoue.
        echo Veuillez installer Python 3.10 ou superieur manuellement depuis https://www.python.org/downloads/
        pause
        exit /b 1
    )
    
    echo Python a ete installe avec succes.
    
    :: Rafraîchir les variables d'environnement
    echo Mise a jour des variables d'environnement...
    call RefreshEnv.cmd >nul 2>&1
    if %errorLevel% neq 0 (
        echo Redemarrage des variables d'environnement...
        set PATH=%PATH%;C:\Program Files\Python310;C:\Program Files\Python310\Scripts
    )
) else (
    echo Python est deja installe.
)

:: Vérification de la version de Python
echo Verification de la version de Python...
for /f "tokens=2" %%i in ('python --version 2^>^&1') do set PYTHON_VERSION=%%i
echo Version de Python detectee: %PYTHON_VERSION%

:: Mise à jour de pip
echo Mise a jour de pip...
python -m pip install --upgrade pip

:: Installation des dépendances Python
echo Installation des dependances Python...

:: Tentative d'installation de PyQt5 avec gestion des erreurs
echo Installation de PyQt5...
python -m pip install PyQt5
if %errorLevel% neq 0 (
    echo AVERTISSEMENT: L'installation de PyQt5 a echoue. Tentative avec une version specifique...
    python -m pip install PyQt5==5.15.6
    
    if %errorLevel% neq 0 (
        echo AVERTISSEMENT: L'installation de PyQt5 a echoue. Tentative avec une approche alternative...
        
        :: Téléchargement et installation de la wheel PyQt5
        echo Telechargement de la wheel PyQt5...
        powershell -Command "& {Invoke-WebRequest -Uri 'https://files.pythonhosted.org/packages/4b/3b/56a7aec5bc0fd0b932dd3ce77b9cc1a170a53e9450fa0ad0842b9bd0a719/PyQt5-5.15.6-cp310-cp310-win_amd64.whl' -OutFile '%TEMP%\PyQt5-5.15.6-cp310-cp310-win_amd64.whl'}"
        
        if %errorLevel% neq 0 (
            echo ERREUR: Impossible de telecharger la wheel PyQt5.
            echo Veuillez installer PyQt5 manuellement avec: pip install PyQt5
            echo Vous pouvez continuer l'installation, mais certaines fonctionnalites pourraient ne pas fonctionner correctement.
        ) else {
            echo Installation de la wheel PyQt5...
            python -m pip install "%TEMP%\PyQt5-5.15.6-cp310-cp310-win_amd64.whl"
            
            if %errorLevel% neq 0 (
                echo ERREUR: L'installation de la wheel PyQt5 a echoue.
                echo Veuillez installer PyQt5 manuellement avec: pip install PyQt5
                echo Vous pouvez continuer l'installation, mais certaines fonctionnalites pourraient ne pas fonctionner correctement.
            ) else {
                echo PyQt5 a ete installe avec succes.
            )
        )
    ) else {
        echo PyQt5 a ete installe avec succes.
    )
) else {
    echo PyQt5 a ete installe avec succes.
)

:: Installation des autres dépendances Python
echo Installation des autres dependances Python...
python -m pip install numpy pandas onnxruntime yara-python requests pillow matplotlib

echo.
echo ===================================================
echo Installation des dependances terminee
echo ===================================================

:: Création des répertoires de données
echo Creation des repertoires de donnees...
set APPDATA_DIR=%APPDATA%\IRIS-Forensic-X
if not exist "%APPDATA_DIR%" mkdir "%APPDATA_DIR%"
if not exist "%APPDATA_DIR%\Logs" mkdir "%APPDATA_DIR%\Logs"
if not exist "%APPDATA_DIR%\CoreCLI" mkdir "%APPDATA_DIR%\CoreCLI"
if not exist "%APPDATA_DIR%\AIModels" mkdir "%APPDATA_DIR%\AIModels"
if not exist "%APPDATA_DIR%\Blockchain" mkdir "%APPDATA_DIR%\Blockchain"
if not exist "%APPDATA_DIR%\YaraRules" mkdir "%APPDATA_DIR%\YaraRules"
if not exist "%APPDATA_DIR%\bin" mkdir "%APPDATA_DIR%\bin"
if not exist "%APPDATA_DIR%\Reports" mkdir "%APPDATA_DIR%\Reports"
if not exist "%APPDATA_DIR%\Templates" mkdir "%APPDATA_DIR%\Templates"
if not exist "%APPDATA_DIR%\Config" mkdir "%APPDATA_DIR%\Config"

:: Copie des fichiers nécessaires
echo Copie des fichiers necessaires...
if exist "YaraRules\*.yar" xcopy /Y "YaraRules\*.yar" "%APPDATA_DIR%\YaraRules\"
if exist "bin\yara.exe" copy /Y "bin\yara.exe" "%APPDATA_DIR%\bin\"
if exist "CoreCLI\iris.exe" copy /Y "CoreCLI\iris.exe" "%APPDATA_DIR%\CoreCLI\"
if exist "AIModels\threat_detection.onnx" copy /Y "AIModels\threat_detection.onnx" "%APPDATA_DIR%\AIModels\"
if exist "Templates\report_template.html" copy /Y "Templates\report_template.html" "%APPDATA_DIR%\Templates\"
if exist "config.json" copy /Y "config.json" "%APPDATA_DIR%\Config\"

echo.
echo ===================================================
echo Installation terminee avec succes
echo ===================================================
echo Vous pouvez maintenant lancer IRIS-Forensic X depuis le menu Demarrer ou le bureau.
echo.

pause
exit /b 0
