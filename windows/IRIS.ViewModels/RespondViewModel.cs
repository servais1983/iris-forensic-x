using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using IRIS.Core;
using IRIS.Models;
using IRIS.Services;
using IRIS.Helpers;

namespace IRIS.ViewModels
{
    /// <summary>
    /// ViewModel pour la page de réponse aux incidents
    /// </summary>
    public class RespondViewModel : INotifyPropertyChanged
    {
        private readonly ILogService _logService;
        private readonly IResponseService _responseService;
        private readonly IAIService _aiService;
        private readonly IBlockchainService _blockchainService;
        
        // Propriétés privées
        private bool _useAI;
        private bool _useBlockchainVerification;
        private string _selectedCategory;
        private ResponsePlaybook _selectedPlaybook;
        private bool _isPlaybookRunning;
        private string _playbookStatusMessage;
        private double _playbookProgress;
        private int _actionsCompleted;
        private int _totalActions;
        
        // Propriétés publiques
        public bool UseAI
        {
            get => _useAI;
            set => SetProperty(ref _useAI, value);
        }
        
        public bool UseBlockchainVerification
        {
            get => _useBlockchainVerification;
            set => SetProperty(ref _useBlockchainVerification, value);
        }
        
        public string SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                if (SetProperty(ref _selectedCategory, value))
                {
                    FilterPlaybooks();
                }
            }
        }
        
        public ResponsePlaybook SelectedPlaybook
        {
            get => _selectedPlaybook;
            set => SetProperty(ref _selectedPlaybook, value);
        }
        
        public bool IsPlaybookRunning
        {
            get => _isPlaybookRunning;
            set => SetProperty(ref _isPlaybookRunning, value);
        }
        
        public string PlaybookStatusMessage
        {
            get => _playbookStatusMessage;
            set => SetProperty(ref _playbookStatusMessage, value);
        }
        
        public double PlaybookProgress
        {
            get => _playbookProgress;
            set => SetProperty(ref _playbookProgress, value);
        }
        
        public int ActionsCompleted
        {
            get => _actionsCompleted;
            set => SetProperty(ref _actionsCompleted, value);
        }
        
        public int TotalActions
        {
            get => _totalActions;
            set => SetProperty(ref _totalActions, value);
        }
        
        // Collections
        public ObservableCollection<string> Categories { get; } = new ObservableCollection<string>();
        public ObservableCollection<ResponsePlaybook> AllPlaybooks { get; } = new ObservableCollection<ResponsePlaybook>();
        public ObservableCollection<ResponsePlaybook> FilteredPlaybooks { get; } = new ObservableCollection<ResponsePlaybook>();
        public ObservableCollection<ResponseAction> RecentActions { get; } = new ObservableCollection<ResponseAction>();
        
        // Commandes
        public ICommand ExecutePlaybookCommand { get; }
        public ICommand CancelPlaybookCommand { get; }
        public ICommand ViewPlaybookDetailsCommand { get; }
        public ICommand CreatePlaybookCommand { get; }
        public ICommand ImportPlaybookCommand { get; }
        public ICommand ExportPlaybookCommand { get; }
        public ICommand EditPlaybookCommand { get; }
        public ICommand DeletePlaybookCommand { get; }
        public ICommand GenerateReportCommand { get; }
        public ICommand ViewActionDetailsCommand { get; }
        
        // Constructeur
        public RespondViewModel(ILogService logService)
        {
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));
            _responseService = ServiceLocator.GetService<IResponseService>();
            _aiService = ServiceLocator.GetService<IAIService>();
            _blockchainService = ServiceLocator.GetService<IBlockchainService>();
            
            // Initialisation des commandes
            ExecutePlaybookCommand = new RelayCommand<ResponsePlaybook>(ExecutePlaybook, CanExecutePlaybook);
            CancelPlaybookCommand = new RelayCommand<ResponsePlaybook>(CancelPlaybook, p => IsPlaybookRunning);
            ViewPlaybookDetailsCommand = new RelayCommand<ResponsePlaybook>(ViewPlaybookDetails);
            CreatePlaybookCommand = new RelayCommand(CreatePlaybook);
            ImportPlaybookCommand = new RelayCommand(ImportPlaybook);
            ExportPlaybookCommand = new RelayCommand<ResponsePlaybook>(ExportPlaybook);
            EditPlaybookCommand = new RelayCommand<ResponsePlaybook>(EditPlaybook);
            DeletePlaybookCommand = new RelayCommand<ResponsePlaybook>(DeletePlaybook);
            GenerateReportCommand = new RelayCommand<ResponseAction>(GenerateReport);
            ViewActionDetailsCommand = new RelayCommand<ResponseAction>(ViewActionDetails);
            
            // Initialisation des valeurs par défaut
            _useAI = true;
            _useBlockchainVerification = true;
            _isPlaybookRunning = false;
            _playbookStatusMessage = "Prêt à exécuter un playbook";
            _playbookProgress = 0;
            _actionsCompleted = 0;
            _totalActions = 0;
        }
        
        // Méthodes publiques
        public void Initialize()
        {
            LoadCategories();
            LoadPlaybooks();
            LoadRecentActions();
            
            _logService.LogInfo("RespondViewModel initialisé");
        }
        
        public void Cleanup()
        {
            // Nettoyage des ressources si nécessaire
            _logService.LogInfo("RespondViewModel nettoyé");
        }
        
        // Méthodes privées
        private void LoadCategories()
        {
            Categories.Clear();
            
            // Ajout de la catégorie "Toutes"
            Categories.Add("Toutes");
            
            // Simulation de catégories (à remplacer par l'implémentation réelle)
            Categories.Add("Malware");
            Categories.Add("Ransomware");
            Categories.Add("Phishing");
            Categories.Add("Accès non autorisé");
            Categories.Add("Exfiltration de données");
            Categories.Add("Déni de service");
            
            SelectedCategory = "Toutes";
        }
        
        private void LoadPlaybooks()
        {
            AllPlaybooks.Clear();
            
            // Simulation de playbooks (à remplacer par l'implémentation réelle)
            AllPlaybooks.Add(new ResponsePlaybook
            {
                Id = "PB001",
                Name = "Isolation de poste infecté",
                Description = "Isole un poste de travail infecté du réseau et collecte les preuves nécessaires",
                Category = "Malware",
                Author = "Équipe SOC",
                CreationDate = DateTime.Now.AddDays(-30),
                LastModified = DateTime.Now.AddDays(-5),
                ActionCount = 8,
                EstimatedDuration = TimeSpan.FromMinutes(15),
                AutomationLevel = "Élevé",
                MITRETactics = new List<string> { "Containment", "Eradication" }
            });
            
            AllPlaybooks.Add(new ResponsePlaybook
            {
                Id = "PB002",
                Name = "Réponse Ransomware",
                Description = "Procédure complète de réponse à une attaque ransomware",
                Category = "Ransomware",
                Author = "Équipe CERT",
                CreationDate = DateTime.Now.AddDays(-60),
                LastModified = DateTime.Now.AddDays(-2),
                ActionCount = 12,
                EstimatedDuration = TimeSpan.FromMinutes(45),
                AutomationLevel = "Moyen",
                MITRETactics = new List<string> { "Containment", "Eradication", "Recovery" }
            });
            
            AllPlaybooks.Add(new ResponsePlaybook
            {
                Id = "PB003",
                Name = "Analyse de phishing",
                Description = "Analyse d'un email de phishing et recherche d'impacts",
                Category = "Phishing",
                Author = "Équipe SOC",
                CreationDate = DateTime.Now.AddDays(-45),
                LastModified = DateTime.Now.AddDays(-45),
                ActionCount = 6,
                EstimatedDuration = TimeSpan.FromMinutes(10),
                AutomationLevel = "Élevé",
                MITRETactics = new List<string> { "Detection", "Analysis" }
            });
            
            AllPlaybooks.Add(new ResponsePlaybook
            {
                Id = "PB004",
                Name = "Révocation d'accès compromis",
                Description = "Révocation des accès suite à une compromission de compte",
                Category = "Accès non autorisé",
                Author = "Équipe IAM",
                CreationDate = DateTime.Now.AddDays(-20),
                LastModified = DateTime.Now.AddDays(-20),
                ActionCount = 7,
                EstimatedDuration = TimeSpan.FromMinutes(12),
                AutomationLevel = "Élevé",
                MITRETactics = new List<string> { "Containment", "Recovery" }
            });
            
            AllPlaybooks.Add(new ResponsePlaybook
            {
                Id = "PB005",
                Name = "Investigation d'exfiltration",
                Description = "Investigation complète suite à une suspicion d'exfiltration de données",
                Category = "Exfiltration de données",
                Author = "Équipe CERT",
                CreationDate = DateTime.Now.AddDays(-15),
                LastModified = DateTime.Now.AddDays(-10),
                ActionCount = 10,
                EstimatedDuration = TimeSpan.FromMinutes(30),
                AutomationLevel = "Moyen",
                MITRETactics = new List<string> { "Detection", "Analysis", "Containment" }
            });
            
            FilterPlaybooks();
        }
        
        private void LoadRecentActions()
        {
            RecentActions.Clear();
            
            // Simulation d'actions récentes (à remplacer par l'implémentation réelle)
            RecentActions.Add(new ResponseAction
            {
                Id = "ACT001",
                PlaybookId = "PB001",
                PlaybookName = "Isolation de poste infecté",
                ActionName = "Isolation réseau",
                Status = "Terminé",
                StartTime = DateTime.Now.AddHours(-3),
                EndTime = DateTime.Now.AddHours(-3).AddMinutes(2),
                ExecutedBy = "Système",
                Result = "Succès",
                Details = "Poste WKST-045 isolé du réseau via GPO de quarantaine"
            });
            
            RecentActions.Add(new ResponseAction
            {
                Id = "ACT002",
                PlaybookId = "PB001",
                PlaybookName = "Isolation de poste infecté",
                ActionName = "Capture mémoire",
                Status = "Terminé",
                StartTime = DateTime.Now.AddHours(-3).AddMinutes(2),
                EndTime = DateTime.Now.AddHours(-3).AddMinutes(8),
                ExecutedBy = "Système",
                Result = "Succès",
                Details = "Capture mémoire de 8GB réalisée sur WKST-045"
            });
            
            RecentActions.Add(new ResponseAction
            {
                Id = "ACT003",
                PlaybookId = "PB003",
                PlaybookName = "Analyse de phishing",
                ActionName = "Extraction d'indicateurs",
                Status = "Terminé",
                StartTime = DateTime.Now.AddHours(-2),
                EndTime = DateTime.Now.AddHours(-2).AddMinutes(1),
                ExecutedBy = "Système",
                Result = "Succès",
                Details = "5 URLs et 3 hashes extraits de l'email suspect"
            });
            
            RecentActions.Add(new ResponseAction
            {
                Id = "ACT004",
                PlaybookId = "PB002",
                PlaybookName = "Réponse Ransomware",
                ActionName = "Blocage IOCs",
                Status = "En cours",
                StartTime = DateTime.Now.AddMinutes(-15),
                EndTime = null,
                ExecutedBy = "Système",
                Result = "En attente",
                Details = "Blocage des IOCs sur les firewalls et proxys en cours"
            });
        }
        
        private void FilterPlaybooks()
        {
            FilteredPlaybooks.Clear();
            
            if (string.IsNullOrEmpty(SelectedCategory) || SelectedCategory == "Toutes")
            {
                foreach (var playbook in AllPlaybooks)
                {
                    FilteredPlaybooks.Add(playbook);
                }
            }
            else
            {
                foreach (var playbook in AllPlaybooks)
                {
                    if (playbook.Category == SelectedCategory)
                    {
                        FilteredPlaybooks.Add(playbook);
                    }
                }
            }
        }
        
        private bool CanExecutePlaybook(ResponsePlaybook playbook)
        {
            return playbook != null && !IsPlaybookRunning;
        }
        
        private void ExecutePlaybook(ResponsePlaybook playbook)
        {
            if (playbook == null || IsPlaybookRunning)
                return;
                
            IsPlaybookRunning = true;
            PlaybookProgress = 0;
            PlaybookStatusMessage = $"Initialisation du playbook {playbook.Name}...";
            ActionsCompleted = 0;
            TotalActions = playbook.ActionCount;
            SelectedPlaybook = playbook;
            
            _logService.LogInfo($"Démarrage du playbook: {playbook.Name}");
            
            // Configuration de l'exécution
            var config = new PlaybookExecutionConfig
            {
                PlaybookId = playbook.Id,
                UseAI = UseAI,
                UseBlockchainVerification = UseBlockchainVerification
            };
            
            // Démarrage de l'exécution en arrière-plan
            Task.Run(() => RunPlaybook(config));
        }
        
        private async Task RunPlaybook(PlaybookExecutionConfig config)
        {
            try
            {
                // Intégration avec le Core CLI
                var cliService = ServiceLocator.GetService<ICoreCliService>();
                
                // Préparation de la commande CLI
                var cliCommand = new CliCommand
                {
                    Command = "respond",
                    Arguments = new Dictionary<string, string>
                    {
                        { "playbook", config.PlaybookId },
                        { "ai", config.UseAI.ToString().ToLower() },
                        { "blockchain", config.UseBlockchainVerification.ToString().ToLower() }
                    }
                };
                
                // Exécution via le Core CLI
                var cliTask = cliService.ExecuteCommandAsync(cliCommand);
                
                // Abonnement aux mises à jour de progression
                _responseService.ProgressUpdated += OnPlaybookProgressUpdated;
                
                // Attente de la fin de l'exécution
                var result = await cliTask;
                
                // Vérification blockchain si activée
                if (config.UseBlockchainVerification)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke(() => 
                    {
                        PlaybookStatusMessage = "Vérification blockchain en cours...";
                    });
                    
                    await _blockchainService.VerifyActionIntegrityAsync(result.ActionIds);
                }
                
                // Mise à jour de l'interface sur le thread UI
                System.Windows.Application.Current.Dispatcher.Invoke(() => 
                {
                    IsPlaybookRunning = false;
                    PlaybookProgress = 1.0;
                    PlaybookStatusMessage = "Playbook terminé";
                    ActionsCompleted = TotalActions;
                    
                    // Rafraîchissement des actions récentes
                    LoadRecentActions();
                    
                    _logService.LogInfo($"Playbook terminé: {config.PlaybookId}");
                });
            }
            catch (Exception ex)
            {
                System.Windows.Application.Current.Dispatcher.Invoke(() => 
                {
                    IsPlaybookRunning = false;
                    PlaybookStatusMessage = "Erreur lors de l'exécution du playbook";
                    
                    _logService.LogError($"Erreur d'exécution du playbook: {ex.Message}");
                });
            }
            finally
            {
                // Désabonnement des événements
                _responseService.ProgressUpdated -= OnPlaybookProgressUpdated;
            }
        }
        
        private void OnPlaybookProgressUpdated(object sender, PlaybookProgressEventArgs e)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() => 
            {
                PlaybookProgress = e.Progress;
                PlaybookStatusMessage = e.StatusMessage;
                ActionsCompleted = e.ActionsCompleted;
            });
        }
        
        private void CancelPlaybook(ResponsePlaybook playbook)
        {
            if (!IsPlaybookRunning)
                return;
                
            _responseService.CancelPlaybook(playbook?.Id);
            
            IsPlaybookRunning = false;
            PlaybookStatusMessage = "Playbook annulé";
            
            _logService.LogInfo("Playbook annulé par l'utilisateur");
        }
        
        private void ViewPlaybookDetails(ResponsePlaybook playbook)
        {
            if (playbook == null)
                return;
                
            _logService.LogInfo($"Affichage des détails du playbook: {playbook.Name}");
            
            // Implémentation à compléter
            // Ouverture d'une fenêtre de détails ou navigation vers une page de détails
        }
        
        private void CreatePlaybook()
        {
            _logService.LogInfo("Création d'un nouveau playbook");
            
            // Implémentation à compléter
            // Ouverture d'une fenêtre de création de playbook
        }
        
        private void ImportPlaybook()
        {
            // Ouverture d'une boîte de dialogue pour sélectionner le fichier de playbook
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Title = "Importer un playbook",
                Filter = "Fichiers de playbook (*.yaml;*.json)|*.yaml;*.json",
                DefaultExt = ".yaml"
            };
            
            bool? result = dialog.ShowDialog();
            
            if (result == true)
            {
                string filename = dialog.FileName;
                
                // Import du playbook
                var importedPlaybook = _responseService.ImportPlaybook(filename);
                
                if (importedPlaybook != null)
                {
                    // Rafraîchissement de la liste
                    LoadPlaybooks();
                    
                    _logService.LogInfo($"Playbook importé: {filename}");
                }
            }
        }
        
        private void ExportPlaybook(ResponsePlaybook playbook)
        {
            if (playbook == null)
                return;
                
            // Ouverture d'une boîte de dialogue pour sélectionner l'emplacement de sauvegarde
            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                Title = "Exporter le playbook",
                Filter = "Fichier YAML (*.yaml)|*.yaml|Fichier JSON (*.json)|*.json",
                DefaultExt = ".yaml",
                FileName = $"{playbook.Name.Replace(" ", "_")}"
            };
            
            bool? result = dialog.ShowDialog();
            
            if (result == true)
            {
                string filename = dialog.FileName;
                
                // Export du playbook
                _responseService.ExportPlaybook(playbook.Id, filename);
                
                _logService.LogInfo($"Playbook exporté: {filename}");
            }
        }
        
        private void EditPlaybook(ResponsePlaybook playbook)
        {
            if (playbook == null)
                return;
                
            _logService.LogInfo($"Édition du playbook: {playbook.Name}");
            
            // Implémentation à compléter
            // Ouverture d'une fenêtre d'édition de playbook
        }
        
        private void DeletePlaybook(ResponsePlaybook playbook)
        {
            if (playbook == null)
                return;
                
            // Confirmation de suppression
            var result = System.Windows.MessageBox.Show(
                $"Êtes-vous sûr de vouloir supprimer le playbook '{playbook.Name}' ?",
                "Confirmation de suppression",
                System.Windows.MessageBoxButton.YesNo,
                System.Windows.MessageBoxImage.Warning);
                
            if (result == System.Windows.MessageBoxResult.Yes)
            {
                // Suppression du playbook
                _responseService.DeletePlaybook(playbook.Id);
                
                // Rafraîchissement de la liste
                LoadPlaybooks();
                
                _logService.LogInfo($"Playbook supprimé: {playbook.Name}");
            }
        }
        
        private void GenerateReport(ResponseAction action)
        {
            if (action == null)
                return;
                
            _logService.LogInfo($"Génération de rapport pour l'action: {action.ActionName}");
            
            // Ouverture d'une boîte de dialogue pour sélectionner l'emplacement de sauvegarde
            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                Title = "Enregistrer le rapport",
                Filter = "Fichier PDF (*.pdf)|*.pdf|Fichier HTML (*.html)|*.html",
                DefaultExt = ".pdf",
                FileName = $"Rapport_{action.PlaybookName}_{action.ActionName}".Replace(" ", "_")
            };
            
            bool? result = dialog.ShowDialog();
            
            if (result == true)
            {
                string filename = dialog.FileName;
                
                // Génération du rapport
                _responseService.GenerateActionReport(action.Id, filename);
                
                _logService.LogInfo($"Rapport généré: {filename}");
            }
        }
        
        private void ViewActionDetails(ResponseAction action)
        {
            if (action == null)
                return;
                
            _logService.LogInfo($"Affichage des détails de l'action: {action.ActionName}");
            
            // Implémentation à compléter
            // Ouverture d'une fenêtre de détails ou navigation vers une page de détails
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
