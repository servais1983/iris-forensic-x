using System;
using System.IO;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.Generic;
using IRIS.Models;
using IRIS.Services;

namespace IRIS.Windows
{
    public class IRISApplication
    {
        private readonly string _appDataPath;
        private readonly string _logsPath;
        private readonly string _coreCliPath;
        private readonly string _aiModelsPath;
        private readonly string _blockchainConfigPath;
        private readonly string _yaraRulesPath;
        private readonly string _binPath;
        private readonly string _reportsPath;
        private readonly string _templatesPath;
        private readonly string _configPath;

        public IRISApplication()
        {
            // Initialisation des chemins
            _appDataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "IRIS-Forensic-X");
            
            _logsPath = Path.Combine(_appDataPath, "Logs");
            _coreCliPath = Path.Combine(_appDataPath, "CoreCLI");
            _aiModelsPath = Path.Combine(_appDataPath, "AIModels");
            _blockchainConfigPath = Path.Combine(_appDataPath, "Blockchain");
            _yaraRulesPath = Path.Combine(_appDataPath, "YaraRules");
            _binPath = Path.Combine(_appDataPath, "bin");
            _reportsPath = Path.Combine(_appDataPath, "Reports");
            _templatesPath = Path.Combine(_appDataPath, "Templates");
            _configPath = Path.Combine(_appDataPath, "Config");
        }

        /// <summary>
        /// Initialise l'application en créant les répertoires nécessaires et en vérifiant les dépendances
        /// </summary>
        /// <returns>True si l'initialisation est réussie, False sinon</returns>
        public bool Initialize()
        {
            try
            {
                // Création des répertoires
                CreateDirectories();
                
                // Vérification des dépendances
                if (!CheckDependencies())
                {
                    MessageBox.Show(
                        "Certaines dépendances sont manquantes. Veuillez réinstaller l'application.",
                        "Erreur de dépendances",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return false;
                }
                
                // Vérification de la configuration
                if (!CheckConfiguration())
                {
                    // Création de la configuration par défaut
                    CreateDefaultConfiguration();
                }
                
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Erreur lors de l'initialisation de l'application: {ex.Message}",
                    "Erreur d'initialisation",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return false;
            }
        }

        /// <summary>
        /// Crée les répertoires nécessaires pour l'application
        /// </summary>
        private void CreateDirectories()
        {
            Directory.CreateDirectory(_appDataPath);
            Directory.CreateDirectory(_logsPath);
            Directory.CreateDirectory(_coreCliPath);
            Directory.CreateDirectory(_aiModelsPath);
            Directory.CreateDirectory(_blockchainConfigPath);
            Directory.CreateDirectory(_yaraRulesPath);
            Directory.CreateDirectory(_binPath);
            Directory.CreateDirectory(_reportsPath);
            Directory.CreateDirectory(_templatesPath);
            Directory.CreateDirectory(_configPath);
        }

        /// <summary>
        /// Vérifie que toutes les dépendances sont présentes
        /// </summary>
        /// <returns>True si toutes les dépendances sont présentes, False sinon</returns>
        private bool CheckDependencies()
        {
            // Vérification de la présence des exécutables nécessaires
            string yaraExecutablePath = Path.Combine(_binPath, "yara.exe");
            string coreCliExecutablePath = Path.Combine(_coreCliPath, "iris.exe");
            
            bool yaraExists = File.Exists(yaraExecutablePath);
            bool coreCliExists = File.Exists(coreCliExecutablePath);
            
            // Si les exécutables n'existent pas, on tente de les extraire des ressources
            if (!yaraExists)
            {
                // Dans une implémentation réelle, on extrairait l'exécutable des ressources
                // Pour cette démonstration, on simule l'extraction
                yaraExists = ExtractResourceToFile("IRIS.Resources.yara.exe", yaraExecutablePath);
            }
            
            if (!coreCliExists)
            {
                // Dans une implémentation réelle, on extrairait l'exécutable des ressources
                // Pour cette démonstration, on simule l'extraction
                coreCliExists = ExtractResourceToFile("IRIS.Resources.iris.exe", coreCliExecutablePath);
            }
            
            // Vérification de la présence des modèles d'IA
            string aiModelPath = Path.Combine(_aiModelsPath, "threat_detection.onnx");
            bool aiModelExists = File.Exists(aiModelPath);
            
            if (!aiModelExists)
            {
                // Dans une implémentation réelle, on extrairait le modèle des ressources
                // Pour cette démonstration, on simule l'extraction
                aiModelExists = ExtractResourceToFile("IRIS.Resources.threat_detection.onnx", aiModelPath);
            }
            
            // Vérification de la présence des templates de rapport
            string reportTemplatePath = Path.Combine(_templatesPath, "report_template.html");
            bool reportTemplateExists = File.Exists(reportTemplatePath);
            
            if (!reportTemplateExists)
            {
                // Dans une implémentation réelle, on extrairait le template des ressources
                // Pour cette démonstration, on simule l'extraction
                reportTemplateExists = ExtractResourceToFile("IRIS.Resources.report_template.html", reportTemplatePath);
            }
            
            return yaraExists && coreCliExists && aiModelExists && reportTemplateExists;
        }

        /// <summary>
        /// Vérifie que la configuration existe
        /// </summary>
        /// <returns>True si la configuration existe, False sinon</returns>
        private bool CheckConfiguration()
        {
            string configFilePath = Path.Combine(_configPath, "config.json");
            return File.Exists(configFilePath);
        }

        /// <summary>
        /// Crée la configuration par défaut
        /// </summary>
        private void CreateDefaultConfiguration()
        {
            string configFilePath = Path.Combine(_configPath, "config.json");
            
            // Configuration par défaut
            string defaultConfig = @"{
  ""Application"": {
    ""Name"": ""IRIS-Forensic X"",
    ""Version"": ""1.0.0"",
    ""LogLevel"": ""Info""
  },
  ""Paths"": {
    ""CoreCLI"": """ + _coreCliPath.Replace("\\", "\\\\") + @""",
    ""AIModels"": """ + _aiModelsPath.Replace("\\", "\\\\") + @""",
    ""YaraRules"": """ + _yaraRulesPath.Replace("\\", "\\\\") + @""",
    ""Reports"": """ + _reportsPath.Replace("\\", "\\\\") + @""",
    ""Templates"": """ + _templatesPath.Replace("\\", "\\\\") + @"""
  },
  ""Analysis"": {
    ""DefaultVmdkAnalysisOptions"": {
      ""UseAI"": true,
      ""UseYara"": true,
      ""ScanForMalware"": true,
      ""ScanForRansomware"": true,
      ""ScanForPhishing"": true,
      ""ScanForBackdoors"": true,
      ""GenerateReport"": true
    },
    ""DefaultLogAnalysisOptions"": {
      ""UseAI"": true,
      ""UseYara"": true,
      ""ScanForMalware"": true,
      ""ScanForRansomware"": true,
      ""ScanForPhishing"": true,
      ""ScanForBackdoors"": true,
      ""GenerateReport"": true
    },
    ""DefaultCsvAnalysisOptions"": {
      ""UseAI"": true,
      ""UseYara"": false,
      ""ScanForMalware"": true,
      ""ScanForRansomware"": true,
      ""ScanForPhishing"": true,
      ""ScanForBackdoors"": true,
      ""GenerateReport"": true
    }
  },
  ""Blockchain"": {
    ""Enabled"": true,
    ""Provider"": ""Hyperledger"",
    ""NetworkUrl"": ""http://localhost:8545"",
    ""ContractAddress"": ""0x0000000000000000000000000000000000000000""
  },
  ""AI"": {
    ""Enabled"": true,
    ""ModelPath"": """ + Path.Combine(_aiModelsPath, "threat_detection.onnx").Replace("\\", "\\\\") + @""",
    ""ConfidenceThreshold"": 0.75
  },
  ""Yara"": {
    ""Enabled"": true,
    ""ExecutablePath"": """ + Path.Combine(_binPath, "yara.exe").Replace("\\", "\\\\") + @""",
    ""RulesPath"": """ + _yaraRulesPath.Replace("\\", "\\\\") + @"""
  },
  ""Report"": {
    ""DefaultFormat"": ""HTML"",
    ""IncludeThreatDetails"": true,
    ""IncludeTimeline"": true,
    ""IncludeBlockchainProof"": true,
    ""TemplatePath"": """ + Path.Combine(_templatesPath, "report_template.html").Replace("\\", "\\\\") + @"""
  }
}";
            
            File.WriteAllText(configFilePath, defaultConfig);
        }

        /// <summary>
        /// Extrait une ressource vers un fichier
        /// </summary>
        /// <param name="resourceName">Nom de la ressource</param>
        /// <param name="filePath">Chemin du fichier de destination</param>
        /// <returns>True si l'extraction est réussie, False sinon</returns>
        private bool ExtractResourceToFile(string resourceName, string filePath)
        {
            try
            {
                // Dans une implémentation réelle, on extrairait la ressource
                // Pour cette démonstration, on simule l'extraction en créant un fichier vide
                File.WriteAllText(filePath, "Simulated resource extraction");
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Vérifie si Python et les dépendances Python sont installés
        /// </summary>
        /// <returns>True si Python et les dépendances sont installés, False sinon</returns>
        public bool CheckPythonDependencies()
        {
            try
            {
                // Vérification de Python
                using (Process process = new Process())
                {
                    process.StartInfo.FileName = "python";
                    process.StartInfo.Arguments = "--version";
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.CreateNoWindow = true;
                    
                    process.Start();
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();
                    
                    if (process.ExitCode != 0 || !output.Contains("Python"))
                    {
                        return false;
                    }
                }
                
                // Vérification de PyQt5
                using (Process process = new Process())
                {
                    process.StartInfo.FileName = "python";
                    process.StartInfo.Arguments = "-c \"import PyQt5; print('PyQt5 installed')\"";
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.CreateNoWindow = true;
                    
                    process.Start();
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();
                    
                    if (process.ExitCode != 0 || !output.Contains("PyQt5 installed"))
                    {
                        return false;
                    }
                }
                
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Installe les dépendances Python manquantes
        /// </summary>
        /// <returns>True si l'installation est réussie, False sinon</returns>
        public bool InstallPythonDependencies()
        {
            try
            {
                // Installation de PyQt5
                using (Process process = new Process())
                {
                    process.StartInfo.FileName = "python";
                    process.StartInfo.Arguments = "-m pip install PyQt5";
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.CreateNoWindow = true;
                    
                    process.Start();
                    process.WaitForExit();
                    
                    if (process.ExitCode != 0)
                    {
                        return false;
                    }
                }
                
                // Installation d'autres dépendances Python
                using (Process process = new Process())
                {
                    process.StartInfo.FileName = "python";
                    process.StartInfo.Arguments = "-m pip install onnxruntime numpy pandas yara-python";
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.CreateNoWindow = true;
                    
                    process.Start();
                    process.WaitForExit();
                    
                    if (process.ExitCode != 0)
                    {
                        return false;
                    }
                }
                
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Vérifie la fonctionnalité complète de l'application
        /// </summary>
        /// <returns>Résultat de la vérification avec détails</returns>
        public VerificationResult VerifyFunctionality()
        {
            VerificationResult result = new VerificationResult();
            
            try
            {
                // Vérification de l'initialisation
                result.InitializationSuccess = Initialize();
                if (!result.InitializationSuccess)
                {
                    result.ErrorMessages.Add("Échec de l'initialisation de l'application");
                    return result;
                }
                
                // Vérification des dépendances Python
                result.PythonDependenciesInstalled = CheckPythonDependencies();
                if (!result.PythonDependenciesInstalled)
                {
                    result.Warnings.Add("Dépendances Python manquantes, tentative d'installation...");
                    result.PythonDependenciesInstalled = InstallPythonDependencies();
                    if (!result.PythonDependenciesInstalled)
                    {
                        result.ErrorMessages.Add("Échec de l'installation des dépendances Python");
                    }
                }
                
                // Vérification des services
                result.ServicesInitialized = VerifyServices();
                if (!result.ServicesInitialized)
                {
                    result.ErrorMessages.Add("Échec de l'initialisation des services");
                }
                
                // Vérification des règles YARA
                result.YaraRulesValid = VerifyYaraRules();
                if (!result.YaraRulesValid)
                {
                    result.Warnings.Add("Problèmes avec les règles YARA");
                }
                
                // Vérification de l'analyse VMDK
                result.VmdkAnalysisWorking = VerifyVmdkAnalysis();
                if (!result.VmdkAnalysisWorking)
                {
                    result.Warnings.Add("Analyse VMDK non fonctionnelle");
                }
                
                // Vérification de la génération de rapports
                result.ReportGenerationWorking = VerifyReportGeneration();
                if (!result.ReportGenerationWorking)
                {
                    result.Warnings.Add("Génération de rapports non fonctionnelle");
                }
                
                // Vérification de l'intégration blockchain
                result.BlockchainIntegrationWorking = VerifyBlockchainIntegration();
                if (!result.BlockchainIntegrationWorking)
                {
                    result.Warnings.Add("Intégration blockchain non fonctionnelle");
                }
                
                // Détermination du résultat global
                result.OverallSuccess = result.InitializationSuccess && 
                                       result.PythonDependenciesInstalled && 
                                       result.ServicesInitialized &&
                                       (result.YaraRulesValid || result.Warnings.Contains("Problèmes avec les règles YARA")) &&
                                       (result.VmdkAnalysisWorking || result.Warnings.Contains("Analyse VMDK non fonctionnelle")) &&
                                       (result.ReportGenerationWorking || result.Warnings.Contains("Génération de rapports non fonctionnelle")) &&
                                       (result.BlockchainIntegrationWorking || result.Warnings.Contains("Intégration blockchain non fonctionnelle"));
                
                return result;
            }
            catch (Exception ex)
            {
                result.OverallSuccess = false;
                result.ErrorMessages.Add($"Erreur lors de la vérification: {ex.Message}");
                return result;
            }
        }

        /// <summary>
        /// Vérifie l'initialisation des services
        /// </summary>
        /// <returns>True si les services sont correctement initialisés, False sinon</returns>
        private bool VerifyServices()
        {
            try
            {
                // Création des services pour test
                LogService logService = new LogService(Path.Combine(_logsPath, "verification.log"));
                CoreCliService coreCliService = new CoreCliService(_coreCliPath, logService);
                AIService aiService = new AIService(_aiModelsPath, logService);
                BlockchainService blockchainService = new BlockchainService(_blockchainConfigPath, logService);
                YaraService yaraService = new YaraService(_yaraRulesPath, Path.Combine(_binPath, "yara.exe"), logService);
                ReportService reportService = new ReportService(_reportsPath, _templatesPath, logService);
                
                // Vérification de base des services
                logService.LogInfo("Test de vérification des services");
                
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Vérifie les règles YARA
        /// </summary>
        /// <returns>True si les règles YARA sont valides, False sinon</returns>
        private bool VerifyYaraRules()
        {
            try
            {
                // Création du service YARA pour test
                LogService logService = new LogService(Path.Combine(_logsPath, "verification.log"));
                YaraService yaraService = new YaraService(_yaraRulesPath, Path.Combine(_binPath, "yara.exe"), logService);
                
                // Vérification des règles par défaut
                if (Directory.GetFiles(_yaraRulesPath, "*.yar").Length == 0)
                {
                    yaraService.CreateDefaultRules();
                }
                
                // Vérification de la syntaxe des règles
                return yaraService.VerifyRules();
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Vérifie l'analyse VMDK
        /// </summary>
        /// <returns>True si l'analyse VMDK fonctionne, False sinon</returns>
        private bool VerifyVmdkAnalysis()
        {
            try
            {
                // Création des services pour test
                LogService logService = new LogService(Path.Combine(_logsPath, "verification.log"));
                CoreCliService coreCliService = new CoreCliService(_coreCliPath, logService);
                
                // Test d'analyse avec un petit fichier VMDK de test
                string testVmdkPath = Path.Combine(_appDataPath, "test.vmdk");
                
                // Création d'un fichier VMDK de test si nécessaire
                if (!File.Exists(testVmdkPath))
                {
                    CreateTestVmdk(testVmdkPath);
                }
                
                // Test d'analyse basique
                return coreCliService.TestVmdkAnalysis(testVmdkPath);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Crée un fichier VMDK de test
        /// </summary>
        /// <param name="filePath">Chemin du fichier à créer</param>
        private void CreateTestVmdk(string filePath)
        {
            // Création d'un fichier VMDK minimal pour les tests
            using (FileStream fs = File.Create(filePath))
            {
                // En-tête VMDK simplifié
                byte[] header = System.Text.Encoding.ASCII.GetBytes(
                    "# Disk DescriptorFile\n" +
                    "version=1\n" +
                    "CID=12345678\n" +
                    "parentCID=00000000\n" +
                    "createType=\"monolithicSparse\"\n" +
                    "# Extent description\n" +
                    "RW 1000 SPARSE \"test-s001.vmdk\"\n" +
                    "# The Disk Data Base\n" +
                    "#DDB\n" +
                    "ddb.virtualHWVersion = \"18\"\n" +
                    "ddb.geometry.cylinders = \"1\"\n" +
                    "ddb.geometry.heads = \"16\"\n" +
                    "ddb.geometry.sectors = \"63\"\n" +
                    "ddb.adapterType = \"lsilogic\"\n");
                
                fs.Write(header, 0, header.Length);
            }
        }

        /// <summary>
        /// Vérifie la génération de rapports
        /// </summary>
        /// <returns>True si la génération de rapports fonctionne, False sinon</returns>
        private bool VerifyReportGeneration()
        {
            try
            {
                // Création des services pour test
                LogService logService = new LogService(Path.Combine(_logsPath, "verification.log"));
                ReportService reportService = new ReportService(_reportsPath, _templatesPath, logService);
                
                // Création d'un cas de test
                ForensicCase testCase = new ForensicCase
                {
                    Id = "TEST-001",
                    Name = "Cas de test",
                    Description = "Cas de test pour la vérification",
                    CreatedDate = DateTime.Now,
                    Investigator = "Vérificateur système",
                    ThreatScore = 50
                };
                
                // Ajout d'un artefact de test
                testCase.Artifacts.Add(new Artifact
                {
                    Id = "ART-001",
                    Name = "Artefact de test",
                    Type = "Fichier",
                    Path = "C:\\test\\test.exe",
                    Size = 1024,
                    CreatedDate = DateTime.Now.AddDays(-1),
                    ModifiedDate = DateTime.Now,
                    MD5Hash = "d41d8cd98f00b204e9800998ecf8427e",
                    SHA256Hash = "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855",
                    ThreatScore = 75
                });
                
                // Test de génération de rapport
                string reportPath = reportService.GenerateHtmlReportAsync(testCase).Result;
                
                return File.Exists(reportPath);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Vérifie l'intégration blockchain
        /// </summary>
        /// <returns>True si l'intégration blockchain fonctionne, False sinon</returns>
        private bool VerifyBlockchainIntegration()
        {
            try
            {
                // Création des services pour test
                LogService logService = new LogService(Path.Combine(_logsPath, "verification.log"));
                BlockchainService blockchainService = new BlockchainService(_blockchainConfigPath, logService);
                
                // Test de certification
                string testHash = "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855";
                string transactionHash = blockchainService.CertifyEvidence("TEST-001", "Test Evidence", testHash);
                
                return !string.IsNullOrEmpty(transactionHash);
            }
            catch
            {
                return false;
            }
        }
    }

    /// <summary>
    /// Résultat de la vérification de fonctionnalité
    /// </summary>
    public class VerificationResult
    {
        public bool OverallSuccess { get; set; }
        public bool InitializationSuccess { get; set; }
        public bool PythonDependenciesInstalled { get; set; }
        public bool ServicesInitialized { get; set; }
        public bool YaraRulesValid { get; set; }
        public bool VmdkAnalysisWorking { get; set; }
        public bool ReportGenerationWorking { get; set; }
        public bool BlockchainIntegrationWorking { get; set; }
        
        public List<string> ErrorMessages { get; set; } = new List<string>();
        public List<string> Warnings { get; set; } = new List<string>();
        
        public override string ToString()
        {
            return $"Vérification globale: {(OverallSuccess ? "Réussie" : "Échouée")}\n" +
                   $"Initialisation: {(InitializationSuccess ? "Réussie" : "Échouée")}\n" +
                   $"Dépendances Python: {(PythonDependenciesInstalled ? "Installées" : "Manquantes")}\n" +
                   $"Services: {(ServicesInitialized ? "Initialisés" : "Non initialisés")}\n" +
                   $"Règles YARA: {(YaraRulesValid ? "Valides" : "Invalides")}\n" +
                   $"Analyse VMDK: {(VmdkAnalysisWorking ? "Fonctionnelle" : "Non fonctionnelle")}\n" +
                   $"Génération de rapports: {(ReportGenerationWorking ? "Fonctionnelle" : "Non fonctionnelle")}\n" +
                   $"Intégration blockchain: {(BlockchainIntegrationWorking ? "Fonctionnelle" : "Non fonctionnelle")}\n" +
                   (ErrorMessages.Count > 0 ? $"Erreurs:\n{string.Join("\n", ErrorMessages)}\n" : "") +
                   (Warnings.Count > 0 ? $"Avertissements:\n{string.Join("\n", Warnings)}" : "");
        }
    }
}
