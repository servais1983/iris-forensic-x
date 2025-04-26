using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using IRIS.Models;

namespace IRIS.Core.Models
{
    public class EvidenceItem : INotifyPropertyChanged
    {
        private string _id;
        private string _name;
        private string _path;
        private string _type;
        private long _size;
        private DateTime _createdDate;
        private DateTime _modifiedDate;
        private string _hash;
        private int _threatScore;
        private List<string> _tags;
        private string _notes;
        private bool _isSelected;

        public string Id 
        { 
            get => _id; 
            set 
            { 
                _id = value; 
                OnPropertyChanged(nameof(Id)); 
            } 
        }

        public string Name 
        { 
            get => _name; 
            set 
            { 
                _name = value; 
                OnPropertyChanged(nameof(Name)); 
            } 
        }

        public string Path 
        { 
            get => _path; 
            set 
            { 
                _path = value; 
                OnPropertyChanged(nameof(Path)); 
            } 
        }

        public string Type 
        { 
            get => _type; 
            set 
            { 
                _type = value; 
                OnPropertyChanged(nameof(Type)); 
            } 
        }

        public long Size 
        { 
            get => _size; 
            set 
            { 
                _size = value; 
                OnPropertyChanged(nameof(Size)); 
            } 
        }

        public DateTime CreatedDate 
        { 
            get => _createdDate; 
            set 
            { 
                _createdDate = value; 
                OnPropertyChanged(nameof(CreatedDate)); 
            } 
        }

        public DateTime ModifiedDate 
        { 
            get => _modifiedDate; 
            set 
            { 
                _modifiedDate = value; 
                OnPropertyChanged(nameof(ModifiedDate)); 
            } 
        }

        public string Hash 
        { 
            get => _hash; 
            set 
            { 
                _hash = value; 
                OnPropertyChanged(nameof(Hash)); 
            } 
        }

        public int ThreatScore 
        { 
            get => _threatScore; 
            set 
            { 
                _threatScore = value; 
                OnPropertyChanged(nameof(ThreatScore)); 
            } 
        }

        public List<string> Tags 
        { 
            get => _tags; 
            set 
            { 
                _tags = value; 
                OnPropertyChanged(nameof(Tags)); 
            } 
        }

        public string Notes 
        { 
            get => _notes; 
            set 
            { 
                _notes = value; 
                OnPropertyChanged(nameof(Notes)); 
            } 
        }

        public bool IsSelected 
        { 
            get => _isSelected; 
            set 
            { 
                _isSelected = value; 
                OnPropertyChanged(nameof(IsSelected)); 
            } 
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class CaptureTarget : INotifyPropertyChanged
    {
        private string _id;
        private string _name;
        private string _type;
        private string _path;
        private string _connectionString;
        private Dictionary<string, string> _parameters;
        private bool _isConnected;
        private bool _isSelected;

        public string Id 
        { 
            get => _id; 
            set 
            { 
                _id = value; 
                OnPropertyChanged(nameof(Id)); 
            } 
        }

        public string Name 
        { 
            get => _name; 
            set 
            { 
                _name = value; 
                OnPropertyChanged(nameof(Name)); 
            } 
        }

        public string Type 
        { 
            get => _type; 
            set 
            { 
                _type = value; 
                OnPropertyChanged(nameof(Type)); 
            } 
        }

        public string Path 
        { 
            get => _path; 
            set 
            { 
                _path = value; 
                OnPropertyChanged(nameof(Path)); 
            } 
        }

        public string ConnectionString 
        { 
            get => _connectionString; 
            set 
            { 
                _connectionString = value; 
                OnPropertyChanged(nameof(ConnectionString)); 
            } 
        }

        public Dictionary<string, string> Parameters 
        { 
            get => _parameters; 
            set 
            { 
                _parameters = value; 
                OnPropertyChanged(nameof(Parameters)); 
            } 
        }

        public bool IsConnected 
        { 
            get => _isConnected; 
            set 
            { 
                _isConnected = value; 
                OnPropertyChanged(nameof(IsConnected)); 
            } 
        }

        public bool IsSelected 
        { 
            get => _isSelected; 
            set 
            { 
                _isSelected = value; 
                OnPropertyChanged(nameof(IsSelected)); 
            } 
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class CaptureResult : INotifyPropertyChanged
    {
        private string _id;
        private string _targetId;
        private DateTime _captureDate;
        private string _outputPath;
        private long _size;
        private string _status;
        private string _errorMessage;
        private List<EvidenceItem> _evidenceItems;

        public string Id 
        { 
            get => _id; 
            set 
            { 
                _id = value; 
                OnPropertyChanged(nameof(Id)); 
            } 
        }

        public string TargetId 
        { 
            get => _targetId; 
            set 
            { 
                _targetId = value; 
                OnPropertyChanged(nameof(TargetId)); 
            } 
        }

        public DateTime CaptureDate 
        { 
            get => _captureDate; 
            set 
            { 
                _captureDate = value; 
                OnPropertyChanged(nameof(CaptureDate)); 
            } 
        }

        public string OutputPath 
        { 
            get => _outputPath; 
            set 
            { 
                _outputPath = value; 
                OnPropertyChanged(nameof(OutputPath)); 
            } 
        }

        public long Size 
        { 
            get => _size; 
            set 
            { 
                _size = value; 
                OnPropertyChanged(nameof(Size)); 
            } 
        }

        public string Status 
        { 
            get => _status; 
            set 
            { 
                _status = value; 
                OnPropertyChanged(nameof(Status)); 
            } 
        }

        public string ErrorMessage 
        { 
            get => _errorMessage; 
            set 
            { 
                _errorMessage = value; 
                OnPropertyChanged(nameof(ErrorMessage)); 
            } 
        }

        public List<EvidenceItem> EvidenceItems 
        { 
            get => _evidenceItems; 
            set 
            { 
                _evidenceItems = value; 
                OnPropertyChanged(nameof(EvidenceItems)); 
            } 
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class AnalysisResult : INotifyPropertyChanged
    {
        private string _id;
        private string _evidenceId;
        private DateTime _analysisDate;
        private int _threatScore;
        private List<string> _detectedThreats;
        private List<string> _recommendations;
        private string _analysisDetails;
        private string _status;
        private string _errorMessage;

        public string Id 
        { 
            get => _id; 
            set 
            { 
                _id = value; 
                OnPropertyChanged(nameof(Id)); 
            } 
        }

        public string EvidenceId 
        { 
            get => _evidenceId; 
            set 
            { 
                _evidenceId = value; 
                OnPropertyChanged(nameof(EvidenceId)); 
            } 
        }

        public DateTime AnalysisDate 
        { 
            get => _analysisDate; 
            set 
            { 
                _analysisDate = value; 
                OnPropertyChanged(nameof(AnalysisDate)); 
            } 
        }

        public int ThreatScore 
        { 
            get => _threatScore; 
            set 
            { 
                _threatScore = value; 
                OnPropertyChanged(nameof(ThreatScore)); 
            } 
        }

        public List<string> DetectedThreats 
        { 
            get => _detectedThreats; 
            set 
            { 
                _detectedThreats = value; 
                OnPropertyChanged(nameof(DetectedThreats)); 
            } 
        }

        public List<string> Recommendations 
        { 
            get => _recommendations; 
            set 
            { 
                _recommendations = value; 
                OnPropertyChanged(nameof(Recommendations)); 
            } 
        }

        public string AnalysisDetails 
        { 
            get => _analysisDetails; 
            set 
            { 
                _analysisDetails = value; 
                OnPropertyChanged(nameof(AnalysisDetails)); 
            } 
        }

        public string Status 
        { 
            get => _status; 
            set 
            { 
                _status = value; 
                OnPropertyChanged(nameof(Status)); 
            } 
        }

        public string ErrorMessage 
        { 
            get => _errorMessage; 
            set 
            { 
                _errorMessage = value; 
                OnPropertyChanged(nameof(ErrorMessage)); 
            } 
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class ResponsePlaybook : INotifyPropertyChanged
    {
        private string _id;
        private string _name;
        private string _description;
        private List<ResponseAction> _actions;
        private bool _isEnabled;
        private bool _isSelected;

        public string Id 
        { 
            get => _id; 
            set 
            { 
                _id = value; 
                OnPropertyChanged(nameof(Id)); 
            } 
        }

        public string Name 
        { 
            get => _name; 
            set 
            { 
                _name = value; 
                OnPropertyChanged(nameof(Name)); 
            } 
        }

        public string Description 
        { 
            get => _description; 
            set 
            { 
                _description = value; 
                OnPropertyChanged(nameof(Description)); 
            } 
        }

        public List<ResponseAction> Actions 
        { 
            get => _actions; 
            set 
            { 
                _actions = value; 
                OnPropertyChanged(nameof(Actions)); 
            } 
        }

        public bool IsEnabled 
        { 
            get => _isEnabled; 
            set 
            { 
                _isEnabled = value; 
                OnPropertyChanged(nameof(IsEnabled)); 
            } 
        }

        public bool IsSelected 
        { 
            get => _isSelected; 
            set 
            { 
                _isSelected = value; 
                OnPropertyChanged(nameof(IsSelected)); 
            } 
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class ResponseAction : INotifyPropertyChanged
    {
        private string _id;
        private string _name;
        private string _description;
        private string _actionType;
        private Dictionary<string, string> _parameters;
        private bool _isEnabled;
        private int _order;

        public string Id 
        { 
            get => _id; 
            set 
            { 
                _id = value; 
                OnPropertyChanged(nameof(Id)); 
            } 
        }

        public string Name 
        { 
            get => _name; 
            set 
            { 
                _name = value; 
                OnPropertyChanged(nameof(Name)); 
            } 
        }

        public string Description 
        { 
            get => _description; 
            set 
            { 
                _description = value; 
                OnPropertyChanged(nameof(Description)); 
            } 
        }

        public string ActionType 
        { 
            get => _actionType; 
            set 
            { 
                _actionType = value; 
                OnPropertyChanged(nameof(ActionType)); 
            } 
        }

        public Dictionary<string, string> Parameters 
        { 
            get => _parameters; 
            set 
            { 
                _parameters = value; 
                OnPropertyChanged(nameof(Parameters)); 
            } 
        }

        public bool IsEnabled 
        { 
            get => _isEnabled; 
            set 
            { 
                _isEnabled = value; 
                OnPropertyChanged(nameof(IsEnabled)); 
            } 
        }

        public int Order 
        { 
            get => _order; 
            set 
            { 
                _order = value; 
                OnPropertyChanged(nameof(Order)); 
            } 
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class CaptureConfig
    {
        public bool CaptureMemory { get; set; }
        public bool CaptureRegistry { get; set; }
        public bool CaptureFileSystem { get; set; }
        public bool CaptureNetworkInfo { get; set; }
        public bool CaptureProcesses { get; set; }
        public bool CompressOutput { get; set; }
        public string OutputFormat { get; set; }
        public string OutputDirectory { get; set; }
        public Dictionary<string, string> AdditionalParameters { get; set; }
    }

    public class AnalysisConfig
    {
        public bool DetectMalware { get; set; }
        public bool DetectRansomware { get; set; }
        public bool DetectPhishing { get; set; }
        public bool DetectBackdoors { get; set; }
        public bool DetectPersistence { get; set; }
        public bool UseYaraRules { get; set; }
        public bool UseAI { get; set; }
        public int MinimumThreatScore { get; set; }
        public string YaraRulesDirectory { get; set; }
        public Dictionary<string, string> AdditionalParameters { get; set; }
    }

    public class TriageConfig
    {
        public bool PrioritizeByThreatScore { get; set; }
        public bool PrioritizeByFileType { get; set; }
        public bool PrioritizeByModificationDate { get; set; }
        public bool IncludeSystemFiles { get; set; }
        public bool IncludeHiddenFiles { get; set; }
        public int MaxItemsToProcess { get; set; }
        public List<string> FileTypePriorities { get; set; }
        public Dictionary<string, string> AdditionalParameters { get; set; }
    }

    public class PlaybookExecutionConfig
    {
        public bool SimulateOnly { get; set; }
        public bool RequireConfirmation { get; set; }
        public bool LogActions { get; set; }
        public string OutputDirectory { get; set; }
        public Dictionary<string, string> AdditionalParameters { get; set; }
    }

    public class LogEntry
    {
        public DateTime Timestamp { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
        public string Source { get; set; }
    }

    public class TriageProgressEventArgs : EventArgs
    {
        public int TotalItems { get; set; }
        public int ProcessedItems { get; set; }
        public string CurrentItemName { get; set; }
        public int ProgressPercentage { get; set; }
        public string Status { get; set; }
    }

    public class AnalysisProgressEventArgs : EventArgs
    {
        public int TotalItems { get; set; }
        public int ProcessedItems { get; set; }
        public string CurrentItemName { get; set; }
        public int ProgressPercentage { get; set; }
        public string Status { get; set; }
    }

    public class CaptureProgressEventArgs : EventArgs
    {
        public int TotalTargets { get; set; }
        public int ProcessedTargets { get; set; }
        public string CurrentTargetName { get; set; }
        public int ProgressPercentage { get; set; }
        public string Status { get; set; }
    }

    public class PlaybookProgressEventArgs : EventArgs
    {
        public int TotalActions { get; set; }
        public int ProcessedActions { get; set; }
        public string CurrentActionName { get; set; }
        public int ProgressPercentage { get; set; }
        public string Status { get; set; }
    }
}
