using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using IRIS.Core;
using IRIS.Models;
using IRIS.Helpers;

namespace IRIS.ViewModels
{
    /// <summary>
    /// ViewModel pour la page de capture de preuves numériques
    /// </summary>
    public class CaptureViewModel : INotifyPropertyChanged
    {
        private readonly ILogService _logService;
        private readonly ICaptureService _captureService;
        private readonly ITargetService _targetService;
        private readonly IBlockchainService _blockchainService;
        
        // Propriétés privées
        private CaptureType _selectedCaptureType;
        private bool _isCaptureRunning;
        private string _captureStatusMessage;
        private double _captureProgress;
        private string _timeRemaining;
        private string _dataCaptured;
        private bool _useSmartCapture;
        private bool _showAdvancedOptions;
        private int _detailLevel;
        private bool _captureMemory;
        private bool _captureNetwork;
        private bool _captureLogs;
        private bool _captureRegistry;
        private bool _captureArtifacts;
        private string _selectedExportFormat;
        private string _selectedCompressionLevel;
        private bool _verifyIntegrity;
        private bool _useBlockchainCertification;
        private string _selectedResultFilter;
        private bool _hasResults;
        
        // Propriétés publiques
        public CaptureType SelectedCaptureType
        {
            get => _selectedCaptureType;
            set => SetProperty(ref _selectedCaptureType, value);
        }
        
        public bool IsCaptureRunning
        {
            get => _isCaptureRunning;
            set => SetProperty(ref _isCaptureRunning, value);
        }
        
        public string CaptureStatusMessage
        {
            get => _captureStatusMessage;
            set => SetProperty(ref _captureStatusMessage, value);
        }
        
        public double CaptureProgress
        {
            get => _captureProgress;
            set => SetProperty(ref _captureProgress, value);
        }
        
        public string TimeRemaining
        {
            get => _timeRemaining;
            set => SetProperty(ref _timeRemaining, value);
        }
        
        public string DataCaptured
        {
            get => _dataCaptured;
            set => SetProperty(ref _dataCaptured, value);
        }
        
        public bool UseSmartCapture
        {
            get => _useSmartCapture;
            set => SetProperty(ref _useSmartCapture, value);
        }
        
        public bool ShowAdvancedOptions
        {
            get => _showAdvancedOptions;
            set => SetProperty(ref _showAdvancedOptions, value);
        }
        
        public int DetailLevel
        {
            get => _detailLevel;
            set => SetProperty(ref _detailLevel, value);
        }
        
        public bool CaptureMemory
        {
            get => _captureMemory;
            set => SetProperty(ref _captureMemory, value);
        }
        
        public bool CaptureNetwork
        {
            get => _captureNetwork;
            set => SetProperty(ref _captureNetwork, value);
        }
        
        public bool CaptureLogs
        {
            get => _captureLogs;
            set => SetProperty(ref _captureLogs, value);
        }
        
        public bool CaptureRegistry
        {
            get => _captureRegistry;
            set => SetProperty(ref _captureRegistry, value);
        }
        
        public bool CaptureArtifacts
        {
            get => _captureArtifacts;
            set => SetProperty(ref _captureArtifacts, value);
        }
        
        public string SelectedExportFormat
        {
            get => _selectedExportFormat;
            set => SetProperty(ref _selectedExportFormat, value);
        }
        
        public string SelectedCompressionLevel
        {
            get => _selectedCompressionLevel;
            set => SetProperty(ref _selectedCompressionLevel, value);
        }
        
        public bool VerifyIntegrity
        {
            get => _verifyIntegrity;
            set => SetProperty(ref _verifyIntegrity, value);
        }
        
        public bool UseBlockchainCertification
        {
            get => _useBlockchainCertification;
            set => SetProperty(ref _useBlockchainCertification, value);
        }
        
        public string SelectedResultFilter
        {
            get => _selectedResultFilter;
            set
            {
                if (SetProperty(ref _selectedResultFilter, value))
                {
                    FilterResults();
                }
            }
        }
        
        public bool HasResults
        {
            get => _hasResults;
            set => SetProperty(ref _hasResults, value);
        }
        
        // Collections
        public ObservableCollection<CaptureType> CaptureTypes { get; } = new ObservableCollection<CaptureType>();
        public ObservableCollection<CaptureTarget> CaptureTargets { get; } = new ObservableCollection<CaptureTarget>();
        public ObservableCollection<string> ExportFormats { get; } = new ObservableCollection<string>();
        public ObservableCollection<string> CompressionLevels { get; } = new ObservableCollection<string>();
        public ObservableCollection<string> ResultFilters { get; } = new ObservableCollection<string>();
        public ObservableCollection<CaptureResult> CaptureResults { get; } = new ObservableCollection<CaptureResult>();
        
        // Commandes
        public ICommand StartCaptureCommand { get; }
        public ICommand CancelCaptureCommand { get; }
        public ICommand ViewCaptureCommand { get; }
        public ICommand ExportCaptureCommand { get; }
        public ICommand DeleteCaptureCommand { get; }
        public ICommand AddTargetCommand { get; }
        public ICommand RemoveTargetCommand { get; }
        
        // Constructeur
        public CaptureViewModel(ILogService logService)
        {
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));
            _captureService = ServiceLocator.GetService<ICaptureService>();
            _targetService = ServiceLocator.GetService<ITargetService>();
            _blockchainService = ServiceLocator.GetService<IBlockchainService>();
            
            // Initialisation des commandes
            StartCaptureCommand = new RelayCommand(StartCapture, CanStartCapture);
            CancelCaptureCommand = new RelayCommand(CancelCapture, () => IsCaptureRunning);
            ViewCaptureCommand = new RelayCommand<CaptureResult>(ViewCapture);
            ExportCaptureCommand = new RelayCommand<CaptureResult>(ExportCapture);
            DeleteCaptureCommand = new RelayCommand<CaptureResult>(DeleteCapture);
            AddTargetCommand = new RelayCommand(AddTarget);
            RemoveTargetCommand = new RelayCommand<CaptureTarget>(RemoveTarget);
            
            // Initialisation des valeurs par défaut
            _selectedCaptureType = CaptureType.SmartCapture;
            _isCaptureRunning = false;
            _captureStatusMessage = "Prêt à démarrer la capture";
            _captureProgress = 0;
            _timeRemaining = "00:00:00";
            _dataCaptured = "0 MB";
            _useSmartCapture = true;
            _showAdvancedOptions = false;
            _detailLevel = 3;
            _captureMemory = true;
            _captureNetwork = true;
            _captureLogs = true;
            _captureRegistry = false;
            _captureArtifacts = false;
            _selectedExportFormat = "E01";
            _selectedCompressionLevel = "Standard";
            _verifyIntegrity = true;
            _useBlockchainCertification = false;
            _selectedResultFilter = "Tous";
            _hasResults = false;
        }
        
        // Méthodes publiques
        public void Initialize()
        {
            LoadCaptureTypes();
            LoadExportFormats();
            LoadCompressionLevels();
            LoadResultFilters();
            LoadSavedResults();
            
            _logService.LogInfo("CaptureViewModel initialisé");
        }
        
        public void Cleanup()
        {
            // Nettoyage des ressources si nécessaire
            _logService.LogInfo("CaptureViewModel nettoyé");
        }
        
        // Méthodes privées
        private void LoadCaptureTypes()
        {
            CaptureTypes.Clear();
            CaptureTypes.Add(CaptureType.SmartCapture);
            CaptureTypes.Add(CaptureType.Memory);
            CaptureTypes.Add(CaptureType.Disk);
            CaptureTypes.Add(CaptureType.Network);
            CaptureTypes.Add(CaptureType.Logs);
            CaptureTypes.Add(CaptureType.Registry);
            CaptureTypes.Add(CaptureType.Custom);
        }
        
        private void LoadExportFormats()
        {
            ExportFormats.Clear();
            ExportFormats.Add("E01");
            ExportFormats.Add("AFF");
            ExportFormats.Add("RAW");
            ExportFormats.Add("ZIP");
            ExportFormats.Add("TAR");
        }
        
        private void LoadCompressionLevels()
        {
            CompressionLevels.Clear();
            CompressionLevels.Add("Aucune");
            CompressionLevels.Add("Rapide");
            CompressionLevels.Add("Standard");
            CompressionLevels.Add("Maximum");
        }
        
        private void LoadResultFilters()
        {
            ResultFilters.Clear();
            ResultFilters.Add("Tous");
            ResultFilters.Add("Récents");
            ResultFilters.Add("Mémoire");
            ResultFilters.Add("Disque");
            ResultFilters.Add("Réseau");
            ResultFilters.Add("Logs");
        }
        
        private void LoadSavedResults()
        {
            try
            {
                CaptureResults.Clear();
                
                // Simulation de résultats (à remplacer par l'implémentation réelle)
                var results = _captureService.GetSavedResults();
                
                foreach (var result in results)
                {
                    CaptureResults.Add(result);
                }
                
                HasResults = CaptureResults.Count > 0;
            }
            catch (Exception ex)
            {
                _logService.LogError($"Erreur lors du chargement des résultats: {ex.Message}");
            }
        }
        
        private void FilterResults()
        {
            if (string.IsNullOrEmpty(SelectedResultFilter) || SelectedResultFilter == "Tous")
            {
                LoadSavedResults();
                return;
            }
            
            try
            {
                CaptureResults.Clear();
                
                // Simulation de filtrage (à remplacer par l'implémentation réelle)
                var results = _captureService.GetFilteredResults(SelectedResultFilter);
                
                foreach (var result in results)
                {
                    CaptureResults.Add(result);
                }
                
                HasResults = CaptureResults.Count > 0;
            }
            catch (Exception ex)
            {
                _logService.LogError($"Erreur lors du filtrage des résultats: {ex.Message}");
            }
        }
        
        private bool CanStartCapture()
        {
            return !IsCaptureRunning && CaptureTargets.Count > 0;
        }
        
        private void StartCapture()
        {
            if (IsCaptureRunning || CaptureTargets.Count == 0)
                return;
                
            IsCaptureRunning = true;
            CaptureProgress = 0;
            CaptureStatusMessage = "Initialisation de la capture...";
            TimeRemaining = "Calcul en cours...";
            DataCaptured = "0 MB";
            
            // Configuration de la capture
            var config = new CaptureConfig
            {
                Type = SelectedCaptureType,
                UseSmartCapture = UseSmartCapture,
                DetailLevel = DetailLevel,
                CaptureMemory = CaptureMemory,
                CaptureNetwork = CaptureNetwork,
                CaptureLogs = CaptureLogs,
                CaptureRegistry = CaptureRegistry,
                CaptureArtifacts = CaptureArtifacts,
                ExportFormat = SelectedExportFormat,
                CompressionLevel = SelectedCompressionLevel,
                VerifyIntegrity = VerifyIntegrity,
                UseBlockchainCertification = UseBlockchainCertification
            };
            
            // Ajout des cibles de capture
            foreach (var target in CaptureTargets)
            {
                config.CaptureTargets.Add(target);
            }
            
            _logService.LogInfo($"Démarrage de la capture: {SelectedCaptureType}");
            
            // Démarrage de la capture en arrière-plan
            Task.Run(() => RunCapture(config));
        }
        
        private async Task RunCapture(CaptureConfig config)
        {
            try
            {
                // Simulation de capture (à remplacer par l'implémentation réelle)
                var captureTask = _captureService.StartCaptureAsync(config);
                
                // Abonnement aux mises à jour de progression
                _captureService.ProgressUpdated += OnCaptureProgressUpdated;
                
                // Attente de la fin de la capture
                var result = await captureTask;
                
                // Certification blockchain si activée
                if (config.UseBlockchainCertification)
                {
                    await _blockchainService.CertifyEvidenceAsync(result.Id, result.Hash);
                }
                
                // Mise à jour de l'interface sur le thread UI
                System.Windows.Application.Current.Dispatcher.Invoke(() => 
                {
                    IsCaptureRunning = false;
                    CaptureProgress = 1.0;
                    CaptureStatusMessage = "Capture terminée";
                    TimeRemaining = "00:00:00";
                    
                    // Ajout du résultat à la liste
                    CaptureResults.Insert(0, result);
                    HasResults = true;
                    
                    _logService.LogInfo($"Capture terminée: {result.Name}");
                });
            }
            catch (Exception ex)
            {
                System.Windows.Application.Current.Dispatcher.Invoke(() => 
                {
                    IsCaptureRunning = false;
                    CaptureStatusMessage = "Erreur lors de la capture";
                    
                    _logService.LogError($"Erreur de capture: {ex.Message}");
                });
            }
            finally
            {
                // Désabonnement des événements
                _captureService.ProgressUpdated -= OnCaptureProgressUpdated;
            }
        }
        
        private void OnCaptureProgressUpdated(object sender, CaptureProgressEventArgs e)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() => 
            {
                CaptureProgress = e.Progress;
                CaptureStatusMessage = e.StatusMessage;
                TimeRemaining = e.TimeRemaining;
                DataCaptured = e.DataCaptured;
            });
        }
        
        private void CancelCapture()
        {
            if (!IsCaptureRunning)
                return;
                
            _captureService.CancelCapture();
            
            IsCaptureRunning = false;
            CaptureStatusMessage = "Capture annulée";
            
            _logService.LogInfo("Capture annulée par l'utilisateur");
        }
        
        private void ViewCapture(CaptureResult result)
        {
            if (result == null)
                return;
                
            _logService.LogInfo($"Affichage de la capture: {result.Name}");
            
            // Implémentation à compléter
            // Ouverture d'une fenêtre de détails ou navigation vers une page de détails
        }
        
        private void ExportCapture(CaptureResult result)
        {
            if (result == null)
                return;
                
            _logService.LogInfo($"Export de la capture: {result.Name} au format {SelectedExportFormat}");
            
            // Implémentation à compléter
            // Exportation de la capture dans le format sélectionné
        }
        
        private void DeleteCapture(CaptureResult result)
        {
            if (result == null)
                return;
                
            // Confirmation de suppression
            var confirmResult = System.Windows.MessageBox.Show(
                $"Êtes-vous sûr de vouloir supprimer la capture '{result.Name}' ?",
                "Confirmation de suppression",
                System.Windows.MessageBoxButton.YesNo,
                System.Windows.MessageBoxImage.Question);
                
            if (confirmResult == System.Windows.MessageBoxResult.Yes)
            {
                _captureService.DeleteResult(result.Id);
                CaptureResults.Remove(result);
                HasResults = CaptureResults.Count > 0;
                
                _logService.LogInfo($"Capture supprimée: {result.Name}");
            }
        }
        
        private void AddTarget()
        {
            // Ouverture d'une boîte de dialogue pour sélectionner une cible
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Title = "Sélectionner une cible de capture",
                Filter = "Tous les fichiers (*.*)|*.*|Disques (*.vhd;*.vmdk;*.vdi)|*.vhd;*.vmdk;*.vdi|Fichiers mémoire (*.dmp;*.mem;*.vmem)|*.dmp;*.mem;*.vmem|Captures réseau (*.pcap;*.pcapng)|*.pcap;*.pcapng",
                Multiselect = true
            };
            
            bool? result = dialog.ShowDialog();
            
            if (result == true)
            {
                foreach (string filename in dialog.FileNames)
                {
                    // Création d'une nouvelle cible de capture
                    var target = new CaptureTarget
                    {
                        Name = System.IO.Path.GetFileName(filename),
                        Path = filename,
                        Type = DetermineTargetType(filename)
                    };
                    
                    CaptureTargets.Add(target);
                    _logService.LogInfo($"Cible de capture ajoutée: {target.Name}");
                }
            }
        }
        
        private string DetermineTargetType(string filename)
        {
            string extension = System.IO.Path.GetExtension(filename).ToLower();
            
            if (extension == ".vhd" || extension == ".vmdk" || extension == ".vdi")
                return "Disque";
            else if (extension == ".dmp" || extension == ".mem" || extension == ".vmem")
                return "Mémoire";
            else if (extension == ".pcap" || extension == ".pcapng")
                return "Réseau";
            else
                return "Fichier";
        }
        
        private void RemoveTarget(CaptureTarget target)
        {
            if (target == null)
                return;
                
            CaptureTargets.Remove(target);
            _logService.LogInfo($"Cible de capture supprimée: {target.Name}");
        }
        
        // Implémentation de INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
                return false;
                
            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
    
    // Énumérations
    public enum CaptureType
    {
        SmartCapture,
        Memory,
        Disk,
        Network,
        Logs,
        Registry,
        Custom
    }
}
