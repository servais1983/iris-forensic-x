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
    /// ViewModel pour la page de triage des preuves numériques
    /// </summary>
    public class TriageViewModel : INotifyPropertyChanged
    {
        private readonly ILogService _logService;
        private readonly ITriageService _triageService;
        private readonly IAIService _aiService;
        private readonly IEvidenceService _evidenceService;
        
        // Propriétés privées
        private string _selectedEvidenceSource;
        private bool _includeFiles;
        private bool _includeLogs;
        private bool _includeRegistry;
        private bool _includeMemory;
        private bool _includeNetwork;
        private DateTime? _startDate;
        private DateTime? _endDate;
        private string _minimumThreatLevel;
        private bool _useAI;
        private bool _isTriageRunning;
        private string _triageStatusMessage;
        private double _triageProgress;
        private int _itemsProcessed;
        private bool _hasEvidence;
        private int _totalItems;
        private int _highPriorityItems;
        private int _threatsDetected;
        private int _excludedItems;
        
        // Propriétés publiques
        public string SelectedEvidenceSource
        {
            get => _selectedEvidenceSource;
            set => SetProperty(ref _selectedEvidenceSource, value);
        }
        
        public bool IncludeFiles
        {
            get => _includeFiles;
            set => SetProperty(ref _includeFiles, value);
        }
        
        public bool IncludeLogs
        {
            get => _includeLogs;
            set => SetProperty(ref _includeLogs, value);
        }
        
        public bool IncludeRegistry
        {
            get => _includeRegistry;
            set => SetProperty(ref _includeRegistry, value);
        }
        
        public bool IncludeMemory
        {
            get => _includeMemory;
            set => SetProperty(ref _includeMemory, value);
        }
        
        public bool IncludeNetwork
        {
            get => _includeNetwork;
            set => SetProperty(ref _includeNetwork, value);
        }
        
        public DateTime? StartDate
        {
            get => _startDate;
            set => SetProperty(ref _startDate, value);
        }
        
        public DateTime? EndDate
        {
            get => _endDate;
            set => SetProperty(ref _endDate, value);
        }
        
        public string MinimumThreatLevel
        {
            get => _minimumThreatLevel;
            set => SetProperty(ref _minimumThreatLevel, value);
        }
        
        public bool UseAI
        {
            get => _useAI;
            set => SetProperty(ref _useAI, value);
        }
        
        public bool IsTriageRunning
        {
            get => _isTriageRunning;
            set => SetProperty(ref _isTriageRunning, value);
        }
        
        public string TriageStatusMessage
        {
            get => _triageStatusMessage;
            set => SetProperty(ref _triageStatusMessage, value);
        }
        
        public double TriageProgress
        {
            get => _triageProgress;
            set => SetProperty(ref _triageProgress, value);
        }
        
        public int ItemsProcessed
        {
            get => _itemsProcessed;
            set => SetProperty(ref _itemsProcessed, value);
        }
        
        public bool HasEvidence
        {
            get => _hasEvidence;
            set => SetProperty(ref _hasEvidence, value);
        }
        
        public int TotalItems
        {
            get => _totalItems;
            set => SetProperty(ref _totalItems, value);
        }
        
        public int HighPriorityItems
        {
            get => _highPriorityItems;
            set => SetProperty(ref _highPriorityItems, value);
        }
        
        public int ThreatsDetected
        {
            get => _threatsDetected;
            set => SetProperty(ref _threatsDetected, value);
        }
        
        public int ExcludedItems
        {
            get => _excludedItems;
            set => SetProperty(ref _excludedItems, value);
        }
        
        // Collections
        public ObservableCollection<string> EvidenceSources { get; } = new ObservableCollection<string>();
        public ObservableCollection<string> ThreatLevels { get; } = new ObservableCollection<string>();
        public ObservableCollection<EvidenceItem> EvidenceItems { get; } = new ObservableCollection<EvidenceItem>();
        public ObservableCollection<string> SelectedMITRETechniques { get; } = new ObservableCollection<string>();
        
        // Commandes
        public ICommand StartTriageCommand { get; }
        public ICommand CancelTriageCommand { get; }
        public ICommand ViewEvidenceCommand { get; }
        public ICommand ExcludeEvidenceCommand { get; }
        public ICommand IncludeEvidenceCommand { get; }
        public ICommand ApplyFilterCommand { get; }
        public ICommand ResetFilterCommand { get; }
        public ICommand SaveTriageCommand { get; }
        public ICommand LoadTriageCommand { get; }
        
        // Constructeur
        public TriageViewModel(ILogService logService)
        {
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));
            _triageService = ServiceLocator.GetService<ITriageService>();
            _aiService = ServiceLocator.GetService<IAIService>();
            _evidenceService = ServiceLocator.GetService<IEvidenceService>();
            
            // Initialisation des commandes
            StartTriageCommand = new RelayCommand(StartTriage, CanStartTriage);
            CancelTriageCommand = new RelayCommand(CancelTriage, () => IsTriageRunning);
            ViewEvidenceCommand = new RelayCommand<EvidenceItem>(ViewEvidence);
            ExcludeEvidenceCommand = new RelayCommand<EvidenceItem>(ExcludeEvidence);
            IncludeEvidenceCommand = new RelayCommand<EvidenceItem>(IncludeEvidence);
            ApplyFilterCommand = new RelayCommand(ApplyFilter);
            ResetFilterCommand = new RelayCommand(ResetFilter);
            SaveTriageCommand = new RelayCommand(SaveTriage, () => HasEvidence && !IsTriageRunning);
            LoadTriageCommand = new RelayCommand(LoadTriage, () => !IsTriageRunning);
            
            // Initialisation des valeurs par défaut
            _includeFiles = true;
            _includeLogs = true;
            _includeRegistry = true;
            _includeMemory = true;
            _includeNetwork = true;
            _startDate = DateTime.Now.AddDays(-30);
            _endDate = DateTime.Now;
            _minimumThreatLevel = "Faible";
            _useAI = true;
            _isTriageRunning = false;
            _triageStatusMessage = "Prêt à démarrer le triage";
            _triageProgress = 0;
            _itemsProcessed = 0;
            _hasEvidence = false;
            _totalItems = 0;
            _highPriorityItems = 0;
            _threatsDetected = 0;
            _excludedItems = 0;
        }
        
        // Méthodes publiques
        public void Initialize()
        {
            LoadEvidenceSources();
            LoadThreatLevels();
            
            _logService.LogInfo("TriageViewModel initialisé");
        }
        
        public void Cleanup()
        {
            // Nettoyage des ressources si nécessaire
            _logService.LogInfo("TriageViewModel nettoyé");
        }
        
        public void UpdatePriority(EvidenceItem evidence, int priority)
        {
            if (evidence == null)
                return;
                
            evidence.Priority = priority;
            UpdateStatistics();
            
            _logService.LogInfo($"Priorité mise à jour pour {evidence.Name}: {priority}");
        }
        
        public void AddMITRETechnique(string technique)
        {
            if (string.IsNullOrEmpty(technique) || SelectedMITRETechniques.Contains(technique))
                return;
                
            SelectedMITRETechniques.Add(technique);
            _logService.LogInfo($"Technique MITRE ajoutée au filtre: {technique}");
        }
        
        public void RemoveMITRETechnique(string technique)
        {
            if (string.IsNullOrEmpty(technique) || !SelectedMITRETechniques.Contains(technique))
                return;
                
            SelectedMITRETechniques.Remove(technique);
            _logService.LogInfo($"Technique MITRE retirée du filtre: {technique}");
        }
        
        // Méthodes privées
        private void LoadEvidenceSources()
        {
            EvidenceSources.Clear();
            
            // Simulation de sources (à remplacer par l'implémentation réelle)
            EvidenceSources.Add("Capture mémoire - Poste de travail 1 (25/04/2025)");
            EvidenceSources.Add("Capture disque - Serveur Web (24/04/2025)");
            EvidenceSources.Add("Capture réseau - Réseau interne (23/04/2025)");
            EvidenceSources.Add("Logs système - Contrôleur de domaine (22/04/2025)");
            EvidenceSources.Add("Registre Windows - Poste compromis (21/04/2025)");
            
            if (EvidenceSources.Count > 0)
            {
                SelectedEvidenceSource = EvidenceSources[0];
            }
        }
        
        private void LoadThreatLevels()
        {
            ThreatLevels.Clear();
            ThreatLevels.Add("Aucun");
            ThreatLevels.Add("Faible");
            ThreatLevels.Add("Moyen");
            ThreatLevels.Add("Élevé");
            ThreatLevels.Add("Critique");
            
            MinimumThreatLevel = "Faible";
        }
        
        private bool CanStartTriage()
        {
            return !IsTriageRunning && !string.IsNullOrEmpty(SelectedEvidenceSource);
        }
        
        private void StartTriage()
        {
            if (IsTriageRunning || string.IsNullOrEmpty(SelectedEvidenceSource))
                return;
                
            IsTriageRunning = true;
            TriageProgress = 0;
            TriageStatusMessage = "Initialisation du triage...";
            ItemsProcessed = 0;
            
            // Configuration du triage
            var config = new TriageConfig
            {
                EvidenceSource = SelectedEvidenceSource,
                IncludeFiles = IncludeFiles,
                IncludeLogs = IncludeLogs,
                IncludeRegistry = IncludeRegistry,
                IncludeMemory = IncludeMemory,
                IncludeNetwork = IncludeNetwork,
                StartDate = StartDate,
                EndDate = EndDate,
                MinimumThreatLevel = MinimumThreatLevel,
                UseAI = UseAI
            };
            
            // Ajout des techniques MITRE sélectionnées
            foreach (var technique in SelectedMITRETechniques)
            {
                config.MITRETechniques.Add(technique);
            }
            
            _logService.LogInfo($"Démarrage du triage: {SelectedEvidenceSource}");
            
            // Démarrage du triage en arrière-plan
            Task.Run(() => RunTriage(config));
        }
        
        private async Task RunTriage(TriageConfig config)
        {
            try
            {
                // Simulation de triage (à remplacer par l'implémentation réelle)
                var triageTask = _triageService.StartTriageAsync(config);
                
                // Abonnement aux mises à jour de progression
                _triageService.ProgressUpdated += OnTriageProgressUpdated;
                
                // Attente de la fin du triage
                var results = await triageTask;
                
                // Analyse IA si activée
                if (config.UseAI)
                {
                    TriageStatusMessage = "Analyse IA en cours...";
                    results = await _aiService.EnhanceTriageResultsAsync(results);
                }
                
                // Mise à jour de l'interface sur le thread UI
                System.Windows.Application.Current.Dispatcher.Invoke(() => 
                {
                    EvidenceItems.Clear();
                    
                    foreach (var item in results)
                    {
                        EvidenceItems.Add(item);
                    }
                    
                    HasEvidence = EvidenceItems.Count > 0;
                    IsTriageRunning = false;
                    TriageProgress = 1.0;
                    TriageStatusMessage = "Triage terminé";
                    
                    UpdateStatistics();
                    
                    _logService.LogInfo($"Triage terminé: {results.Count} éléments trouvés");
                });
            }
            catch (Exception ex)
            {
                System.Windows.Application.Current.Dispatcher.Invoke(() => 
                {
                    IsTriageRunning = false;
                    TriageStatusMessage = "Erreur lors du triage";
                    
                    _logService.LogError($"Erreur de triage: {ex.Message}");
                });
            }
            finally
            {
                // Désabonnement des événements
                _triageService.ProgressUpdated -= OnTriageProgressUpdated;
            }
        }
        
        private void OnTriageProgressUpdated(object sender, TriageProgressEventArgs e)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() => 
            {
                TriageProgress = e.Progress;
                TriageStatusMessage = e.StatusMessage;
                ItemsProcessed = e.ItemsProcessed;
            });
        }
        
        private void CancelTriage()
        {
            if (!IsTriageRunning)
                return;
                
            _triageService.CancelTriage();
            
            IsTriageRunning = false;
            TriageStatusMessage = "Triage annulé";
            
            _logService.LogInfo("Triage annulé par l'utilisateur");
        }
        
        private void ViewEvidence(EvidenceItem evidence)
        {
            if (evidence == null)
                return;
                
            _logService.LogInfo($"Affichage de la preuve: {evidence.Name}");
            
            // Implémentation à compléter
            // Ouverture d'une fenêtre de détails ou navigation vers une page de détails
        }
        
        private void ExcludeEvidence(EvidenceItem evidence)
        {
            if (evidence == null)
                return;
                
            evidence.IsExcluded = true;
            UpdateStatistics();
            
            _logService.LogInfo($"Preuve exclue: {evidence.Name}");
        }
        
        private void IncludeEvidence(EvidenceItem evidence)
        {
            if (evidence == null)
                return;
                
            evidence.IsExcluded = false;
            UpdateStatistics();
            
            _logService.LogInfo($"Preuve incluse: {evidence.Name}");
        }
        
        private void ApplyFilter()
        {
            if (string.IsNullOrEmpty(SelectedEvidenceSource))
                return;
                
            _logService.LogInfo("Application des filtres de triage");
            
            // Configuration du filtre
            var filter = new TriageFilter
            {
                EvidenceSource = SelectedEvidenceSource,
                IncludeFiles = IncludeFiles,
                IncludeLogs = IncludeLogs,
                IncludeRegistry = IncludeRegistry,
                IncludeMemory = IncludeMemory,
                IncludeNetwork = IncludeNetwork,
                StartDate = StartDate,
                EndDate = EndDate,
                MinimumThreatLevel = MinimumThreatLevel
            };
            
            // Ajout des techniques MITRE sélectionnées
            foreach (var technique in SelectedMITRETechniques)
            {
                filter.MITRETechniques.Add(technique);
            }
            
            // Application du filtre
            var filteredItems = _evidenceService.GetFilteredEvidence(filter);
            
            EvidenceItems.Clear();
            
            foreach (var item in filteredItems)
            {
                EvidenceItems.Add(item);
            }
            
            HasEvidence = EvidenceItems.Count > 0;
            UpdateStatistics();
        }
        
        private void ResetFilter()
        {
            _includeFiles = true;
            _includeLogs = true;
            _includeRegistry = true;
            _includeMemory = true;
            _includeNetwork = true;
            _startDate = DateTime.Now.AddDays(-30);
            _endDate = DateTime.Now;
            _minimumThreatLevel = "Faible";
            
            SelectedMITRETechniques.Clear();
            
            OnPropertyChanged(nameof(IncludeFiles));
            OnPropertyChanged(nameof(IncludeLogs));
            OnPropertyChanged(nameof(IncludeRegistry));
            OnPropertyChanged(nameof(IncludeMemory));
            OnPropertyChanged(nameof(IncludeNetwork));
            OnPropertyChanged(nameof(StartDate));
            OnPropertyChanged(nameof(EndDate));
            OnPropertyChanged(nameof(MinimumThreatLevel));
            
            _logService.LogInfo("Filtres de triage réinitialisés");
        }
        
        private void SaveTriage()
        {
            if (!HasEvidence)
                return;
                
            // Ouverture d'une boîte de dialogue pour sélectionner l'emplacement de sauvegarde
            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                Title = "Enregistrer le triage",
                Filter = "Fichiers de triage (*.triage)|*.triage",
                DefaultExt = ".triage"
            };
            
            bool? result = dialog.ShowDialog();
            
            if (result == true)
            {
                string filename = dialog.FileName;
                
                // Sauvegarde du triage
                _triageService.SaveTriage(EvidenceItems, filename);
                
                _logService.LogInfo($"Triage enregistré: {filename}");
            }
        }
        
        private void LoadTriage()
        {
            // Ouverture d'une boîte de dialogue pour sélectionner le fichier de triage
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Title = "Charger un triage",
                Filter = "Fichiers de triage (*.triage)|*.triage",
                DefaultExt = ".triage"
            };
            
            bool? result = dialog.ShowDialog();
            
            if (result == true)
            {
                string filename = dialog.FileName;
                
                // Chargement du triage
                var loadedItems = _triageService.LoadTriage(filename);
                
                EvidenceItems.Clear();
                
                foreach (var item in loadedItems)
                {
                    EvidenceItems.Add(item);
                }
                
                HasEvidence = EvidenceItems.Count > 0;
                UpdateStatistics();
                
                _logService.LogInfo($"Triage chargé: {filename}");
            }
        }
        
        private void UpdateStatistics()
        {
            TotalItems = EvidenceItems.Count;
            
            int highPriority = 0;
            int threats = 0;
            int excluded = 0;
            
            foreach (var item in EvidenceItems)
            {
                if (item.Priority >= 4)
                {
                    highPriority++;
                }
                
                if (item.ThreatLevel != "Aucun")
                {
                    threats++;
                }
                
                if (item.IsExcluded)
                {
                    excluded++;
                }
            }
            
            HighPriorityItems = highPriority;
            ThreatsDetected = threats;
            ExcludedItems = excluded;
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
}
