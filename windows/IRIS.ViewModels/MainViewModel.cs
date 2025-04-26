using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using IRIS.Views;
using IRIS.Services;
using System.IO;
using System.Windows;

namespace IRIS.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly LogService _logService;
        private readonly CoreCliService _coreCliService;
        private readonly AIService _aiService;
        private readonly BlockchainService _blockchainService;
        private readonly YaraService _yaraService;
        private readonly ReportService _reportService;
        
        private object _currentPage;

        public object CurrentPage
        {
            get => _currentPage;
            set
            {
                if (_currentPage != value)
                {
                    _currentPage = value;
                    OnPropertyChanged();
                }
            }
        }

        public MainViewModel()
        {
            try
            {
                // Initialisation des chemins
                string appDataPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "IRIS-Forensic-X");
                
                string logsPath = Path.Combine(appDataPath, "Logs");
                string coreCliPath = Path.Combine(appDataPath, "CoreCLI");
                string aiModelsPath = Path.Combine(appDataPath, "AIModels");
                string blockchainConfigPath = Path.Combine(appDataPath, "Blockchain");
                string yaraRulesPath = Path.Combine(appDataPath, "YaraRules");
                string yaraExecutablePath = Path.Combine(appDataPath, "bin", "yara.exe");
                string reportsPath = Path.Combine(appDataPath, "Reports");
                string templatesPath = Path.Combine(appDataPath, "Templates");
                
                // Création des répertoires si nécessaire
                Directory.CreateDirectory(logsPath);
                Directory.CreateDirectory(coreCliPath);
                Directory.CreateDirectory(aiModelsPath);
                Directory.CreateDirectory(blockchainConfigPath);
                Directory.CreateDirectory(yaraRulesPath);
                Directory.CreateDirectory(Path.Combine(appDataPath, "bin"));
                Directory.CreateDirectory(reportsPath);
                Directory.CreateDirectory(templatesPath);
                
                // Initialisation des services
                _logService = new LogService(Path.Combine(logsPath, "iris.log"));
                _logService.LogInfo("Démarrage de IRIS-Forensic X");
                
                _coreCliService = new CoreCliService(coreCliPath, _logService);
                _aiService = new AIService(aiModelsPath, _logService);
                _blockchainService = new BlockchainService(blockchainConfigPath, _logService);
                _yaraService = new YaraService(yaraRulesPath, yaraExecutablePath, _logService);
                _reportService = new ReportService(reportsPath, templatesPath, _logService);
                
                // Création des règles YARA par défaut si nécessaire
                if (Directory.GetFiles(yaraRulesPath, "*.yar").Length == 0)
                {
                    _logService.LogInfo("Aucune règle YARA trouvée, création des règles par défaut");
                    _yaraService.CreateDefaultRules();
                }
                
                _logService.LogInfo("Initialisation des services terminée");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'initialisation de l'application: {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void NavigateToPage(string pageName)
        {
            try
            {
                _logService.LogInfo($"Navigation vers la page: {pageName}");
                
                switch (pageName)
                {
                    case "Dashboard":
                        // CurrentPage = new DashboardPage();
                        break;
                    case "Capture":
                        // CurrentPage = new CapturePage(_coreCliService, _logService);
                        break;
                    case "Analyze":
                        // CurrentPage = new AnalyzePage(_coreCliService, _aiService, _yaraService, _logService);
                        break;
                    case "Triage":
                        // CurrentPage = new TriagePage(_coreCliService, _logService);
                        break;
                    case "Respond":
                        // CurrentPage = new RespondPage(_coreCliService, _blockchainService, _logService);
                        break;
                    case "Yara":
                        // CurrentPage = new YaraPage(_yaraService, _logService);
                        break;
                    case "Reports":
                        // CurrentPage = new ReportsPage(_reportService, _blockchainService, _logService);
                        break;
                    case "Blockchain":
                        // CurrentPage = new BlockchainPage(_blockchainService, _logService);
                        break;
                    default:
                        _logService.LogWarning($"Page inconnue: {pageName}");
                        break;
                }
            }
            catch (Exception ex)
            {
                _logService.LogError($"Erreur lors de la navigation vers {pageName}: {ex.Message}");
                MessageBox.Show($"Erreur lors de la navigation: {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
