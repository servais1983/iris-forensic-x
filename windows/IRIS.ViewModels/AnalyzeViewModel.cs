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
    /// ViewModel pour la page d'analyse forensique
    /// </summary>
    public class AnalyzeViewModel : INotifyPropertyChanged
    {
        private readonly ILogService _logService;
        private readonly IAnalysisService _analysisService;
        private readonly IEvidenceService _evidenceService;
        private readonly IAIService _aiService;
        
        // Propriétés privées
        private AnalysisType _selectedAnalysisType;
        private bool _isAnalysisRunning;
        private string _analysisStatusMessage;
        private double _analysisProgress;
        private string _timeRemaining;
        private bool _useAI;
        private bool _showAdvancedOptions;
        private int _analysisDepth;
        private bool _useMalwareDetection;
        private bool _useBehavioralAnalysis;
        private bool _useTimelineReconstruction;
        private string _selectedExportFormat;
        private int _cpuUsageLimit;
        private int _ramUsageLimit;
        private string _selectedResultFilter;
        private bool _hasResults;
        
        // Propriétés publiques
        public AnalysisType SelectedAnalysisType
        {
            get => _selectedAnalysisType;
            set => SetProperty(ref _selectedAnalysisType, value);
        }
        
        public bool IsAnalysisRunning
        {
            get => _isAnalysisRunning;
            set => SetProperty(ref _isAnalysisRunning, value);
        }
        
        public string AnalysisStatusMessage
        {
            get => _analysisStatusMessage;
            set => SetProperty(ref _analysisStatusMessage, value);
        }
        
        public double AnalysisProgress
        {
            get => _analysisProgress;
            set => SetProperty(ref _analysisProgress, value);
        }
        
        public string TimeRemaining
        {
            get => _timeRemaining;
            set => SetProperty(ref _timeRemaining, value);
        }
        
        public bool UseAI
        {
            get => _useAI;
            set => SetProperty(ref _useAI, value);
        }
        
        public bool ShowAdvancedOptions
        {
            get => _showAdvancedOptions;
            set => SetProperty(ref _showAdvancedOptions, value);
        }
        
        public int AnalysisDepth
        {
            get => _analysisDepth;
            set => SetProperty(ref _analysisDepth, value);
        }
        
        public bool UseMalwareDetection
        {
            get => _useMalwareDetection;
            set => SetProperty(ref _useMalwareDetection, value);
        }
        
        public bool UseBehavioralAnalysis
        {
            get => _useBehavioralAnalysis;
            set => SetProperty(ref _useBehavioralAnalysis, value);
        }
        
        public bool UseTimelineReconstruction
        {
            get => _useTimelineReconstruction;
            set => SetProperty(ref _useTimelineReconstruction, value);
        }
        
        public string SelectedExportFormat
        {
            get => _selectedExportFormat;
            set => SetProperty(ref _selectedExportFormat, value);
        }
        
        public int CpuUsageLimit
        {
            get => _cpuUsageLimit;
            set => SetProperty(ref _cpuUsageLimit, value);
        }
        
        public int RamUsageLimit
        {
            get => _ramUsageLimit;
            set => SetProperty(ref _ramUsageLimit, value);
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
        public ObservableCollection<AnalysisType> AnalysisTypes { get; } = new ObservableCollection<AnalysisType>();
        public ObservableCollection<EvidenceItem> EvidenceItems { get; } = new ObservableCollection<EvidenceItem>();
        public ObservableCollection<string> ExportFormats { get; } = new ObservableCollection<string>();
        public ObservableCollection<string> ResultFilters { get; } = new ObservableCollection<string>();
        public ObservableCollection<AnalysisResult> AnalysisResults { get; } = new ObservableCollection<AnalysisResult>();
        
        // Commandes
        public ICommand StartAnalysisCommand { get; }
        public ICommand CancelAnalysisCommand { get; }
        public ICommand ViewResultCommand { get; }
        public ICommand ExportResultCommand { get; }
        public ICommand DeleteResultCommand { get; }
        public ICommand AddEvidenceCommand { get; }
        public ICommand RemoveEvidenceCommand { get; }
        
        // Constructeur
        public AnalyzeViewModel(ILogService logService)
        {
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));
            _analysisService = ServiceLocator.GetService<IAnalysisService>();
            _evidenceService = ServiceLocator.GetService<IEvidenceService>();
            _aiService = ServiceLocator.GetService<IAIService>();
            
            // Initialisation des commandes
            StartAnalysisCommand = new RelayCommand(StartAnalysis, CanStartAnalysis);
            CancelAnalysisCommand = new RelayCommand(CancelAnalysis, () => IsAnalysisRunning);
            ViewResultCommand = new RelayCommand<AnalysisResult>(ViewResult);
            ExportResultCommand = new RelayCommand<AnalysisResult>(ExportResult);
            DeleteResultCommand = new RelayCommand<AnalysisResult>(DeleteResult);
            AddEvidenceCommand = new RelayCommand(AddEvidence);
            RemoveEvidenceCommand = new RelayCommand<EvidenceItem>(RemoveEvidence);
            
            // Initialisation des valeurs par défaut
            _selectedAnalysisType = AnalysisType.Standard;
            _isAnalysisRunning = false;
            _analysisStatusMessage = "Prêt à démarrer l'analyse";
            _analysisProgress = 0;
            _timeRemaining = "00:00:00";
            _useAI = true;
            _showAdvancedOptions = false;
            _analysisDepth = 3;
            _useMalwareDetection = true;
            _useBehavioralAnalysis = true;
            _useTimelineReconstruction = true;
            _selectedExportFormat = "PDF";
            _cpuUsageLimit = 70;
            _ramUsageLimit = 8;
            _selectedResultFilter = "Tous";
            _hasResults = false;
        }
        
        // Méthodes publiques
        public void Initialize()
        {
            LoadAnalysisTypes();
            LoadExportFormats();
            LoadResultFilters();
            LoadSavedResults();
            
            _logService.LogInfo("AnalyzeViewModel initialisé");
        }
        
        public void Cleanup()
        {
            // Nettoyage des ressources si nécessaire
            _logService.LogInfo("AnalyzeViewModel nettoyé");
        }
        
        // Méthodes privées
        private void LoadAnalysisTypes()
        {
            AnalysisTypes.Clear();
            AnalysisTypes.Add(AnalysisType.Quick);
            AnalysisTypes.Add(AnalysisType.Standard);
            AnalysisTypes.Add(AnalysisType.Comprehensive);
            AnalysisTypes.Add(AnalysisType.Custom);
            AnalysisTypes.Add(AnalysisType.Malware);
            AnalysisTypes.Add(AnalysisType.Memory);
            AnalysisTypes.Add(AnalysisType.Network);
            AnalysisTypes.Add(AnalysisType.Timeline);
        }
        
        private void LoadExportFormats()
        {
            ExportFormats.Clear();
            ExportFormats.Add("PDF");
            ExportFormats.Add("HTML");
            ExportFormats.Add("XML");
            ExportFormats.Add("JSON");
            ExportFormats.Add("CSV");
        }
        
        private void LoadResultFilters()
        {
            ResultFilters.Clear();
            ResultFilters.Add("Tous");
            ResultFilters.Add("Récents");
            ResultFilters.Add("Malware");
            ResultFilters.Add("Mémoire");
            ResultFilters.Add("Réseau");
            ResultFilters.Add("Timeline");
        }
        
        private void LoadSavedResults()
        {
            try
            {
                AnalysisResults.Clear();
                
                // Simulation de résultats (à remplacer par l'implémentation réelle)
                var results = _analysisService.GetSavedResults();
                
                foreach (var result in results)
                {
                    AnalysisResults.Add(result);
                }
                
                HasResults = AnalysisResults.Count > 0;
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
                AnalysisResults.Clear();
                
                // Simulation de filtrage (à remplacer par l'implémentation réelle)
                var results = _analysisService.GetFilteredResults(SelectedResultFilter);
                
                foreach (var result in results)
                {
                    AnalysisResults.Add(result);
                }
                
                HasResults = AnalysisResults.Count > 0;
            }
            catch (Exception ex)
            {
                _logService.LogError($"Erreur lors du filtrage des résultats: {ex.Message}");
            }
        }
        
        private bool CanStartAnalysis()
        {
            return !IsAnalysisRunning && EvidenceItems.Count > 0;
        }
        
        private void StartAnalysis()
        {
            if (IsAnalysisRunning || EvidenceItems.Count == 0)
                return;
                
            IsAnalysisRunning = true;
            AnalysisProgress = 0;
            AnalysisStatusMessage = "Initialisation de l'analyse...";
            TimeRemaining = "Calcul en cours...";
            
            // Configuration de l'analyse
            var config = new AnalysisConfig
            {
                Type = SelectedAnalysisType,
                UseAI = UseAI,
                Depth = AnalysisDepth,
                UseMalwareDetection = UseMalwareDetection,
                UseBehavioralAnalysis = UseBehavioralAnalysis,
                UseTimelineReconstruction = UseTimelineReconstruction,
                CpuUsageLimit = CpuUsageLimit,
                RamUsageLimit = RamUsageLimit
            };
            
            // Ajout des éléments de preuve
            foreach (var evidence in EvidenceItems)
            {
                config.EvidenceItems.Add(evidence);
            }
            
            _logService.LogInfo($"Démarrage de l'analyse: {SelectedAnalysisType}");
            
            // Démarrage de l'analyse en arrière-plan
            Task.Run(() => RunAnalysis(config));
        }
        
        private async Task RunAnalysis(AnalysisConfig config)
        {
            try
            {
                // Simulation d'analyse (à remplacer par l'implémentation réelle)
                var analysisTask = _analysisService.StartAnalysisAsync(config);
                
                // Abonnement aux mises à jour de progression
                _analysisService.ProgressUpdated += OnAnalysisProgressUpdated;
                
                // Attente de la fin de l'analyse
                var result = await analysisTask;
                
                // Mise à jour de l'interface sur le thread UI
                System.Windows.Application.Current.Dispatcher.Invoke(() => 
                {
                    IsAnalysisRunning = false;
                    AnalysisProgress = 1.0;
                    AnalysisStatusMessage = "Analyse terminée";
                    TimeRemaining = "00:00:00";
                    
                    // Ajout du résultat à la liste
                    AnalysisResults.Insert(0, result);
                    HasResults = true;
                    
                    _logService.LogInfo($"Analyse terminée: {result.Name}");
                });
            }
            catch (Exception ex)
            {
                System.Windows.Application.Current.Dispatcher.Invoke(() => 
                {
                    IsAnalysisRunning = false;
                    AnalysisStatusMessage = "Erreur lors de l'analyse";
                    
                    _logService.LogError($"Erreur d'analyse: {ex.Message}");
                });
            }
            finally
            {
                // Désabonnement des événements
                _analysisService.ProgressUpdated -= OnAnalysisProgressUpdated;
            }
        }
        
        private void OnAnalysisProgressUpdated(object sender, AnalysisProgressEventArgs e)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() => 
            {
                AnalysisProgress = e.Progress;
                AnalysisStatusMessage = e.StatusMessage;
                TimeRemaining = e.TimeRemaining;
            });
        }
        
        private void CancelAnalysis()
        {
            if (!IsAnalysisRunning)
                return;
                
            _analysisService.CancelAnalysis();
            
            IsAnalysisRunning = false;
            AnalysisStatusMessage = "Analyse annulée";
            
            _logService.LogInfo("Analyse annulée par l'utilisateur");
        }
        
        private void ViewResult(AnalysisResult result)
        {
            if (result == null)
                return;
                
            _logService.LogInfo($"Affichage du résultat: {result.Name}");
            
            // Implémentation à compléter
            // Ouverture d'une fenêtre de détails ou navigation vers une page de détails
        }
        
        private void ExportResult(AnalysisResult result)
        {
            if (result == null)
                return;
                
            _logService.LogInfo($"Export du résultat: {result.Name} au format {SelectedExportFormat}");
            
            // Implémentation à compléter
            // Exportation du résultat dans le format sélectionné
        }
        
        private void DeleteResult(AnalysisResult result)
        {
            if (result == null)
                return;
                
            // Confirmation de suppression
            var confirmResult = System.Windows.MessageBox.Show(
                $"Êtes-vous sûr de vouloir supprimer le résultat '{result.Name}' ?",
                "Confirmation de suppression",
                System.Windows.MessageBoxButton.YesNo,
                System.Windows.MessageBoxImage.Question);
                
            if (confirmResult == System.Windows.MessageBoxResult.Yes)
            {
                _analysisService.DeleteResult(result.Id);
                AnalysisResults.Remove(result);
                HasResults = AnalysisResults.Count > 0;
                
                _logService.LogInfo($"Résultat supprimé: {result.Name}");
            }
        }
        
        private void AddEvidence()
        {
            // Ouverture d'une boîte de dialogue pour sélectionner un élément de preuve
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Title = "Sélectionner un élément de preuve",
                Filter = "Tous les fichiers (*.*)|*.*|Images disque (*.dd;*.raw;*.img;*.001;*.E01)|*.dd;*.raw;*.img;*.001;*.E01|Fichiers mémoire (*.dmp;*.mem;*.vmem)|*.dmp;*.mem;*.vmem|Captures réseau (*.pcap;*.pcapng)|*.pcap;*.pcapng",
                Multiselect = true
            };
            
            bool? result = dialog.ShowDialog();
            
            if (result == true)
            {
                foreach (string filename in dialog.FileNames)
                {
                    // Création d'un nouvel élément de preuve
                    var evidence = new EvidenceItem
                    {
                        Name = System.IO.Path.GetFileName(filename),
                        Path = filename,
                        Type = DetermineEvidenceType(filename)
                    };
                    
                    EvidenceItems.Add(evidence);
                    _logService.LogInfo($"Élément de preuve ajouté: {evidence.Name}");
                }
            }
        }
        
        private string DetermineEvidenceType(string filename)
        {
            string extension = System.IO.Path.GetExtension(filename).ToLower();
            
            if (extension == ".dd" || extension == ".raw" || extension == ".img" || extension == ".001" || extension == ".e01")
                return "Image disque";
            else if (extension == ".dmp" || extension == ".mem" || extension == ".vmem")
                return "Mémoire";
            else if (extension == ".pcap" || extension == ".pcapng")
                return "Capture réseau";
            else
                return "Fichier";
        }
        
        private void RemoveEvidence(EvidenceItem evidence)
        {
            if (evidence == null)
                return;
                
            EvidenceItems.Remove(evidence);
            _logService.LogInfo($"Élément de preuve supprimé: {evidence.Name}");
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
    public enum AnalysisType
    {
        Quick,
        Standard,
        Comprehensive,
        Custom,
        Malware,
        Memory,
        Network,
        Timeline
    }
}
