using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace IRIS.Models
{
    // Modèle de base avec notification de changement de propriété
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }

    // Modèle pour les cas forensiques
    public class ForensicCase : ObservableObject
    {
        private string _id;
        private string _name;
        private string _description;
        private DateTime _createdDate;
        private string _status;
        private string _investigator;
        private List<ForensicArtifact> _artifacts;
        private List<ForensicEvidence> _evidences;
        private int _threatScore;

        public string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public DateTime CreatedDate
        {
            get => _createdDate;
            set => SetProperty(ref _createdDate, value);
        }

        public string Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        public string Investigator
        {
            get => _investigator;
            set => SetProperty(ref _investigator, value);
        }

        public List<ForensicArtifact> Artifacts
        {
            get => _artifacts;
            set => SetProperty(ref _artifacts, value);
        }

        public List<ForensicEvidence> Evidences
        {
            get => _evidences;
            set => SetProperty(ref _evidences, value);
        }

        public int ThreatScore
        {
            get => _threatScore;
            set => SetProperty(ref _threatScore, value);
        }

        public ForensicCase()
        {
            Id = Guid.NewGuid().ToString();
            CreatedDate = DateTime.Now;
            Status = "Nouveau";
            Artifacts = new List<ForensicArtifact>();
            Evidences = new List<ForensicEvidence>();
            ThreatScore = 0;
        }
    }

    // Modèle pour les artefacts forensiques
    public class ForensicArtifact : ObservableObject
    {
        private string _id;
        private string _name;
        private string _path;
        private string _type;
        private long _size;
        private DateTime _createdDate;
        private DateTime _modifiedDate;
        private DateTime _accessedDate;
        private string _md5Hash;
        private string _sha1Hash;
        private string _sha256Hash;
        private bool _isAnalyzed;
        private int _threatScore;
        private List<string> _tags;
        private List<ForensicThreat> _detectedThreats;

        public string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string Path
        {
            get => _path;
            set => SetProperty(ref _path, value);
        }

        public string Type
        {
            get => _type;
            set => SetProperty(ref _type, value);
        }

        public long Size
        {
            get => _size;
            set => SetProperty(ref _size, value);
        }

        public DateTime CreatedDate
        {
            get => _createdDate;
            set => SetProperty(ref _createdDate, value);
        }

        public DateTime ModifiedDate
        {
            get => _modifiedDate;
            set => SetProperty(ref _modifiedDate, value);
        }

        public DateTime AccessedDate
        {
            get => _accessedDate;
            set => SetProperty(ref _accessedDate, value);
        }

        public string MD5Hash
        {
            get => _md5Hash;
            set => SetProperty(ref _md5Hash, value);
        }

        public string SHA1Hash
        {
            get => _sha1Hash;
            set => SetProperty(ref _sha1Hash, value);
        }

        public string SHA256Hash
        {
            get => _sha256Hash;
            set => SetProperty(ref _sha256Hash, value);
        }

        public bool IsAnalyzed
        {
            get => _isAnalyzed;
            set => SetProperty(ref _isAnalyzed, value);
        }

        public int ThreatScore
        {
            get => _threatScore;
            set => SetProperty(ref _threatScore, value);
        }

        public List<string> Tags
        {
            get => _tags;
            set => SetProperty(ref _tags, value);
        }

        public List<ForensicThreat> DetectedThreats
        {
            get => _detectedThreats;
            set => SetProperty(ref _detectedThreats, value);
        }

        public ForensicArtifact()
        {
            Id = Guid.NewGuid().ToString();
            CreatedDate = DateTime.Now;
            ModifiedDate = DateTime.Now;
            AccessedDate = DateTime.Now;
            IsAnalyzed = false;
            ThreatScore = 0;
            Tags = new List<string>();
            DetectedThreats = new List<ForensicThreat>();
        }
    }

    // Modèle pour les preuves forensiques
    public class ForensicEvidence : ObservableObject
    {
        private string _id;
        private string _name;
        private string _description;
        private DateTime _discoveredDate;
        private string _type;
        private string _source;
        private int _severity;
        private string _mitreTactic;
        private string _mitreTechnique;
        private string _blockchainHash;
        private bool _isVerified;
        private List<string> _relatedArtifacts;

        public string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public DateTime DiscoveredDate
        {
            get => _discoveredDate;
            set => SetProperty(ref _discoveredDate, value);
        }

        public string Type
        {
            get => _type;
            set => SetProperty(ref _type, value);
        }

        public string Source
        {
            get => _source;
            set => SetProperty(ref _source, value);
        }

        public int Severity
        {
            get => _severity;
            set => SetProperty(ref _severity, value);
        }

        public string MitreTactic
        {
            get => _mitreTactic;
            set => SetProperty(ref _mitreTactic, value);
        }

        public string MitreTechnique
        {
            get => _mitreTechnique;
            set => SetProperty(ref _mitreTechnique, value);
        }

        public string BlockchainHash
        {
            get => _blockchainHash;
            set => SetProperty(ref _blockchainHash, value);
        }

        public bool IsVerified
        {
            get => _isVerified;
            set => SetProperty(ref _isVerified, value);
        }

        public List<string> RelatedArtifacts
        {
            get => _relatedArtifacts;
            set => SetProperty(ref _relatedArtifacts, value);
        }

        public ForensicEvidence()
        {
            Id = Guid.NewGuid().ToString();
            DiscoveredDate = DateTime.Now;
            Severity = 0;
            IsVerified = false;
            RelatedArtifacts = new List<string>();
        }
    }

    // Modèle pour les menaces détectées
    public class ForensicThreat : ObservableObject
    {
        private string _id;
        private string _name;
        private string _type;
        private string _description;
        private int _severity;
        private string _mitreTactic;
        private string _mitreTechnique;
        private string _yaraRule;
        private DateTime _detectedDate;
        private bool _isConfirmed;
        private List<string> _indicators;
        private List<string> _recommendations;

        public string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string Type
        {
            get => _type;
            set => SetProperty(ref _type, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public int Severity
        {
            get => _severity;
            set => SetProperty(ref _severity, value);
        }

        public string MitreTactic
        {
            get => _mitreTactic;
            set => SetProperty(ref _mitreTactic, value);
        }

        public string MitreTechnique
        {
            get => _mitreTechnique;
            set => SetProperty(ref _mitreTechnique, value);
        }

        public string YaraRule
        {
            get => _yaraRule;
            set => SetProperty(ref _yaraRule, value);
        }

        public DateTime DetectedDate
        {
            get => _detectedDate;
            set => SetProperty(ref _detectedDate, value);
        }

        public bool IsConfirmed
        {
            get => _isConfirmed;
            set => SetProperty(ref _isConfirmed, value);
        }

        public List<string> Indicators
        {
            get => _indicators;
            set => SetProperty(ref _indicators, value);
        }

        public List<string> Recommendations
        {
            get => _recommendations;
            set => SetProperty(ref _recommendations, value);
        }

        public ForensicThreat()
        {
            Id = Guid.NewGuid().ToString();
            DetectedDate = DateTime.Now;
            Severity = 0;
            IsConfirmed = false;
            Indicators = new List<string>();
            Recommendations = new List<string>();
        }
    }

    // Modèle pour les rapports forensiques
    public class ForensicReport : ObservableObject
    {
        private string _id;
        private string _title;
        private string _caseId;
        private DateTime _generatedDate;
        private string _author;
        private string _format;
        private string _path;
        private bool _isEncrypted;
        private string _blockchainHash;
        private List<string> _includedEvidences;
        private List<string> _includedArtifacts;

        public string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string CaseId
        {
            get => _caseId;
            set => SetProperty(ref _caseId, value);
        }

        public DateTime GeneratedDate
        {
            get => _generatedDate;
            set => SetProperty(ref _generatedDate, value);
        }

        public string Author
        {
            get => _author;
            set => SetProperty(ref _author, value);
        }

        public string Format
        {
            get => _format;
            set => SetProperty(ref _format, value);
        }

        public string Path
        {
            get => _path;
            set => SetProperty(ref _path, value);
        }

        public bool IsEncrypted
        {
            get => _isEncrypted;
            set => SetProperty(ref _isEncrypted, value);
        }

        public string BlockchainHash
        {
            get => _blockchainHash;
            set => SetProperty(ref _blockchainHash, value);
        }

        public List<string> IncludedEvidences
        {
            get => _includedEvidences;
            set => SetProperty(ref _includedEvidences, value);
        }

        public List<string> IncludedArtifacts
        {
            get => _includedArtifacts;
            set => SetProperty(ref _includedArtifacts, value);
        }

        public ForensicReport()
        {
            Id = Guid.NewGuid().ToString();
            GeneratedDate = DateTime.Now;
            Format = "HTML";
            IsEncrypted = false;
            IncludedEvidences = new List<string>();
            IncludedArtifacts = new List<string>();
        }
    }

    // Modèle pour les configurations YARA
    public class YaraRule : ObservableObject
    {
        private string _id;
        private string _name;
        private string _description;
        private string _author;
        private DateTime _createdDate;
        private DateTime _modifiedDate;
        private string _content;
        private bool _isEnabled;
        private List<string> _tags;
        private int _severity;

        public string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public string Author
        {
            get => _author;
            set => SetProperty(ref _author, value);
        }

        public DateTime CreatedDate
        {
            get => _createdDate;
            set => SetProperty(ref _createdDate, value);
        }

        public DateTime ModifiedDate
        {
            get => _modifiedDate;
            set => SetProperty(ref _modifiedDate, value);
        }

        public string Content
        {
            get => _content;
            set => SetProperty(ref _content, value);
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

        public List<string> Tags
        {
            get => _tags;
            set => SetProperty(ref _tags, value);
        }

        public int Severity
        {
            get => _severity;
            set => SetProperty(ref _severity, value);
        }

        public YaraRule()
        {
            Id = Guid.NewGuid().ToString();
            CreatedDate = DateTime.Now;
            ModifiedDate = DateTime.Now;
            IsEnabled = true;
            Tags = new List<string>();
            Severity = 1;
        }
    }

    // Modèle pour les configurations VMDK
    public class VmdkConfiguration : ObservableObject
    {
        private string _id;
        private string _name;
        private string _path;
        private long _size;
        private bool _isMounted;
        private string _mountPoint;
        private List<string> _partitions;
        private List<string> _fileSystemTypes;
        private bool _isAnalyzed;

        public string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string Path
        {
            get => _path;
            set => SetProperty(ref _path, value);
        }

        public long Size
        {
            get => _size;
            set => SetProperty(ref _size, value);
        }

        public bool IsMounted
        {
            get => _isMounted;
            set => SetProperty(ref _isMounted, value);
        }

        public string MountPoint
        {
            get => _mountPoint;
            set => SetProperty(ref _mountPoint, value);
        }

        public List<string> Partitions
        {
            get => _partitions;
            set => SetProperty(ref _partitions, value);
        }

        public List<string> FileSystemTypes
        {
            get => _fileSystemTypes;
            set => SetProperty(ref _fileSystemTypes, value);
        }

        public bool IsAnalyzed
        {
            get => _isAnalyzed;
            set => SetProperty(ref _isAnalyzed, value);
        }

        public VmdkConfiguration()
        {
            Id = Guid.NewGuid().ToString();
            IsMounted = false;
            Partitions = new List<string>();
            FileSystemTypes = new List<string>();
            IsAnalyzed = false;
        }
    }
}
