using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using IRIS.Models;

namespace IRIS.Services
{
    public class YaraService
    {
        private readonly LogService _logService;
        private readonly string _yaraRulesPath;
        private readonly string _yaraExecutablePath;
        private List<YaraRule> _loadedRules;

        public YaraService(string yaraRulesPath, string yaraExecutablePath, LogService logService)
        {
            _yaraRulesPath = yaraRulesPath;
            _yaraExecutablePath = yaraExecutablePath;
            _logService = logService;
            _loadedRules = new List<YaraRule>();
            
            // Création du répertoire de règles YARA si nécessaire
            Directory.CreateDirectory(_yaraRulesPath);
            
            // Chargement initial des règles
            LoadRules();
        }

        /// <summary>
        /// Charge toutes les règles YARA depuis le répertoire de règles
        /// </summary>
        public void LoadRules()
        {
            try
            {
                _logService.LogInfo($"Chargement des règles YARA depuis {_yaraRulesPath}");
                _loadedRules.Clear();
                
                if (!Directory.Exists(_yaraRulesPath))
                {
                    _logService.LogWarning($"Le répertoire de règles YARA n'existe pas: {_yaraRulesPath}");
                    return;
                }
                
                string[] ruleFiles = Directory.GetFiles(_yaraRulesPath, "*.yar");
                _logService.LogInfo($"Trouvé {ruleFiles.Length} fichiers de règles YARA");
                
                foreach (string ruleFile in ruleFiles)
                {
                    try
                    {
                        string content = File.ReadAllText(ruleFile);
                        string fileName = Path.GetFileNameWithoutExtension(ruleFile);
                        
                        // Extraction des métadonnées de la règle
                        string description = ExtractMetadata(content, "description");
                        string author = ExtractMetadata(content, "author");
                        string severityStr = ExtractMetadata(content, "severity");
                        int severity = 1;
                        if (!string.IsNullOrEmpty(severityStr) && int.TryParse(severityStr, out int parsedSeverity))
                        {
                            severity = parsedSeverity;
                        }
                        
                        // Extraction des tags
                        List<string> tags = ExtractTags(content);
                        
                        var rule = new YaraRule
                        {
                            Name = fileName,
                            Description = description,
                            Author = author,
                            Content = content,
                            CreatedDate = File.GetCreationTime(ruleFile),
                            ModifiedDate = File.GetLastWriteTime(ruleFile),
                            IsEnabled = true,
                            Tags = tags,
                            Severity = severity
                        };
                        
                        _loadedRules.Add(rule);
                        _logService.LogInfo($"Règle YARA chargée: {fileName}");
                    }
                    catch (Exception ex)
                    {
                        _logService.LogError($"Erreur lors du chargement de la règle YARA {ruleFile}: {ex.Message}");
                    }
                }
                
                _logService.LogInfo($"{_loadedRules.Count} règles YARA chargées avec succès");
            }
            catch (Exception ex)
            {
                _logService.LogError($"Erreur lors du chargement des règles YARA: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Extrait une métadonnée spécifique d'une règle YARA
        /// </summary>
        /// <param name="content">Contenu de la règle</param>
        /// <param name="metadataName">Nom de la métadonnée à extraire</param>
        /// <returns>Valeur de la métadonnée</returns>
        private string ExtractMetadata(string content, string metadataName)
        {
            try
            {
                // Recherche de la section metadata
                int metadataStart = content.IndexOf("metadata:");
                if (metadataStart == -1) return string.Empty;
                
                int metadataEnd = content.IndexOf("strings:", metadataStart);
                if (metadataEnd == -1) metadataEnd = content.IndexOf("condition:", metadataStart);
                if (metadataEnd == -1) return string.Empty;
                
                string metadataSection = content.Substring(metadataStart, metadataEnd - metadataStart);
                
                // Recherche de la métadonnée spécifique
                Regex regex = new Regex($"{metadataName}\\s*=\\s*\"([^\"]*)\"");
                Match match = regex.Match(metadataSection);
                
                if (match.Success && match.Groups.Count > 1)
                {
                    return match.Groups[1].Value;
                }
                
                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Extrait les tags d'une règle YARA
        /// </summary>
        /// <param name="content">Contenu de la règle</param>
        /// <returns>Liste des tags</returns>
        private List<string> ExtractTags(string content)
        {
            var tags = new List<string>();
            
            try
            {
                // Recherche de la déclaration de règle avec tags
                Regex regex = new Regex(@"rule\s+\w+\s*:\s*([\w\s]+)\s*\{");
                Match match = regex.Match(content);
                
                if (match.Success && match.Groups.Count > 1)
                {
                    string tagsStr = match.Groups[1].Value.Trim();
                    string[] tagArray = tagsStr.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    tags.AddRange(tagArray);
                }
            }
            catch
            {
                // En cas d'erreur, on retourne une liste vide
            }
            
            return tags;
        }

        /// <summary>
        /// Récupère toutes les règles YARA chargées
        /// </summary>
        /// <returns>Liste des règles YARA</returns>
        public List<YaraRule> GetAllRules()
        {
            return _loadedRules;
        }

        /// <summary>
        /// Récupère une règle YARA par son nom
        /// </summary>
        /// <param name="ruleName">Nom de la règle</param>
        /// <returns>Règle YARA ou null si non trouvée</returns>
        public YaraRule GetRuleByName(string ruleName)
        {
            return _loadedRules.Find(r => r.Name.Equals(ruleName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Ajoute ou met à jour une règle YARA
        /// </summary>
        /// <param name="rule">Règle à ajouter ou mettre à jour</param>
        /// <returns>True si succès, False sinon</returns>
        public bool SaveRule(YaraRule rule)
        {
            try
            {
                _logService.LogInfo($"Sauvegarde de la règle YARA: {rule.Name}");
                
                string filePath = Path.Combine(_yaraRulesPath, $"{rule.Name}.yar");
                File.WriteAllText(filePath, rule.Content);
                
                // Mise à jour de la date de modification
                rule.ModifiedDate = DateTime.Now;
                
                // Mise à jour de la liste des règles chargées
                int index = _loadedRules.FindIndex(r => r.Name.Equals(rule.Name, StringComparison.OrdinalIgnoreCase));
                if (index >= 0)
                {
                    _loadedRules[index] = rule;
                }
                else
                {
                    _loadedRules.Add(rule);
                }
                
                _logService.LogInfo($"Règle YARA sauvegardée avec succès: {rule.Name}");
                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError($"Erreur lors de la sauvegarde de la règle YARA {rule.Name}: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Supprime une règle YARA
        /// </summary>
        /// <param name="ruleName">Nom de la règle à supprimer</param>
        /// <returns>True si succès, False sinon</returns>
        public bool DeleteRule(string ruleName)
        {
            try
            {
                _logService.LogInfo($"Suppression de la règle YARA: {ruleName}");
                
                string filePath = Path.Combine(_yaraRulesPath, $"{ruleName}.yar");
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                
                // Suppression de la règle de la liste des règles chargées
                int index = _loadedRules.FindIndex(r => r.Name.Equals(ruleName, StringComparison.OrdinalIgnoreCase));
                if (index >= 0)
                {
                    _loadedRules.RemoveAt(index);
                }
                
                _logService.LogInfo($"Règle YARA supprimée avec succès: {ruleName}");
                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError($"Erreur lors de la suppression de la règle YARA {ruleName}: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Analyse un fichier avec les règles YARA
        /// </summary>
        /// <param name="filePath">Chemin du fichier à analyser</param>
        /// <returns>Liste des règles correspondantes</returns>
        public async Task<List<YaraRule>> ScanFileAsync(string filePath)
        {
            try
            {
                _logService.LogInfo($"Analyse du fichier avec YARA: {filePath}");
                
                if (!File.Exists(filePath))
                {
                    _logService.LogError($"Le fichier à analyser n'existe pas: {filePath}");
                    throw new FileNotFoundException($"Le fichier à analyser n'existe pas: {filePath}");
                }
                
                // Création d'un fichier temporaire contenant toutes les règles actives
                string tempRulesFile = Path.Combine(Path.GetTempPath(), $"iris_yara_rules_{Guid.NewGuid()}.yar");
                StringBuilder rulesContent = new StringBuilder();
                
                foreach (var rule in _loadedRules.FindAll(r => r.IsEnabled))
                {
                    rulesContent.AppendLine(rule.Content);
                    rulesContent.AppendLine();
                }
                
                File.WriteAllText(tempRulesFile, rulesContent.ToString());
                
                // Exécution de YARA
                var matchedRules = new List<YaraRule>();
                
                try
                {
                    // Dans une implémentation réelle, on exécuterait YARA via ProcessStartInfo
                    // Pour cette démonstration, nous simulons des correspondances
                    await Task.Delay(1000); // Simulation du temps de traitement
                    
                    // Simulation de correspondances pour certains types de fichiers
                    if (filePath.EndsWith(".exe", StringComparison.OrdinalIgnoreCase) || 
                        filePath.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
                    {
                        matchedRules.Add(_loadedRules.Find(r => r.Name.Contains("Malware")));
                    }
                    else if (filePath.EndsWith(".ps1", StringComparison.OrdinalIgnoreCase) || 
                             filePath.EndsWith(".bat", StringComparison.OrdinalIgnoreCase))
                    {
                        matchedRules.Add(_loadedRules.Find(r => r.Name.Contains("Script")));
                    }
                    else if (filePath.EndsWith(".vmdk", StringComparison.OrdinalIgnoreCase))
                    {
                        // Pour les fichiers VMDK, on simule la détection de ransomware
                        var ransomwareRule = new YaraRule
                        {
                            Name = "LockBit_Ransomware",
                            Description = "Détecte les signatures du ransomware LockBit 3.0",
                            Author = "IRIS-Forensic X",
                            Severity = 5,
                            Tags = new List<string> { "ransomware", "lockbit", "encryption" },
                            IsEnabled = true,
                            CreatedDate = DateTime.Now,
                            ModifiedDate = DateTime.Now
                        };
                        
                        matchedRules.Add(ransomwareRule);
                    }
                }
                finally
                {
                    // Suppression du fichier temporaire
                    if (File.Exists(tempRulesFile))
                    {
                        File.Delete(tempRulesFile);
                    }
                }
                
                _logService.LogInfo($"Analyse YARA terminée pour {filePath}: {matchedRules.Count} correspondances");
                return matchedRules;
            }
            catch (Exception ex)
            {
                _logService.LogError($"Erreur lors de l'analyse YARA du fichier {filePath}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Analyse un fichier VMDK avec les règles YARA
        /// </summary>
        /// <param name="vmdkPath">Chemin du fichier VMDK</param>
        /// <returns>Liste des règles correspondantes</returns>
        public async Task<List<YaraRule>> ScanVmdkAsync(string vmdkPath)
        {
            try
            {
                _logService.LogInfo($"Analyse du fichier VMDK avec YARA: {vmdkPath}");
                
                if (!File.Exists(vmdkPath))
                {
                    _logService.LogError($"Le fichier VMDK à analyser n'existe pas: {vmdkPath}");
                    throw new FileNotFoundException($"Le fichier VMDK à analyser n'existe pas: {vmdkPath}");
                }
                
                // Dans une implémentation réelle, on monterait le VMDK et on analyserait son contenu
                // Pour cette démonstration, nous simulons des correspondances
                await Task.Delay(3000); // Simulation du temps de traitement
                
                var matchedRules = new List<YaraRule>
                {
                    new YaraRule
                    {
                        Name = "LockBit_Ransomware",
                        Description = "Détecte les signatures du ransomware LockBit 3.0",
                        Author = "IRIS-Forensic X",
                        Severity = 5,
                        Tags = new List<string> { "ransomware", "lockbit", "encryption" },
                        IsEnabled = true,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now
                    },
                    new YaraRule
                    {
                        Name = "Persistence_Registry",
                        Description = "Détecte les modifications de registre pour la persistance",
                        Author = "IRIS-Forensic X",
                        Severity = 4,
                        Tags = new List<string> { "persistence", "registry", "startup" },
                        IsEnabled = true,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now
                    },
                    new YaraRule
                    {
                        Name = "Credential_Dumper",
                        Description = "Détecte les outils d'extraction de credentials",
                        Author = "IRIS-Forensic X",
                        Severity = 4,
                        Tags = new List<string> { "credential", "mimikatz", "lsass" },
                        IsEnabled = true,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now
                    }
                };
                
                _logService.LogInfo($"Analyse YARA terminée pour le VMDK {vmdkPath}: {matchedRules.Count} correspondances");
                return matchedRules;
            }
            catch (Exception ex)
            {
                _logService.LogError($"Erreur lors de l'analyse YARA du VMDK {vmdkPath}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Crée des règles YARA par défaut
        /// </summary>
        public void CreateDefaultRules()
        {
            try
            {
                _logService.LogInfo("Création des règles YARA par défaut");
                
                // Règle pour détecter LockBit 3.0
                var lockbitRule = new YaraRule
                {
                    Name = "LockBit_Ransomware",
                    Description = "Détecte les signatures du ransomware LockBit 3.0",
                    Author = "IRIS-Forensic X",
                    Severity = 5,
                    Tags = new List<string> { "ransomware", "lockbit", "encryption" },
                    IsEnabled = true,
                    Content = @"
rule LockBit_Ransomware : ransomware lockbit encryption
{
    meta:
        description = ""Détecte les signatures du ransomware LockBit 3.0""
        author = ""IRIS-Forensic X""
        severity = ""5""
        date = ""2025-04-26""
        
    strings:
        $lockbit_str1 = ""LockBit"" nocase
        $lockbit_str2 = "".lockbit"" nocase
        $lockbit_str3 = ""restore-my-files.txt"" nocase
        $lockbit_note = ""All your important files are encrypted"" nocase
        $lockbit_ext = { 2E 6C 6F 63 6B 62 69 74 }  // .lockbit in hex
        $encryption1 = { 83 F8 00 74 ?? 8B ?? 24 ?? 8B ?? 89 ?? 24 ?? E8 }
        $encryption2 = { 0F B6 ?? 83 E? 0F 83 F? 09 76 ?? 8D ?? 57 EB ?? 8D ?? 30 }
        
    condition:
        uint16(0) == 0x5A4D and
        2 of ($lockbit_str*) and
        (1 of ($lockbit_note, $lockbit_ext) or all of ($encryption*))
}"
                };
                
                // Règle pour détecter la persistance
                var persistenceRule = new YaraRule
                {
                    Name = "Persistence_Registry",
                    Description = "Détecte les modifications de registre pour la persistance",
                    Author = "IRIS-Forensic X",
                    Severity = 4,
                    Tags = new List<string> { "persistence", "registry", "startup" },
                    IsEnabled = true,
                    Content = @"
rule Persistence_Registry : persistence registry startup
{
    meta:
        description = ""Détecte les modifications de registre pour la persistance""
        author = ""IRIS-Forensic X""
        severity = ""4""
        date = ""2025-04-26""
        
    strings:
        $reg_run1 = ""\\Software\\Microsoft\\Windows\\CurrentVersion\\Run"" nocase
        $reg_run2 = ""\\Software\\Microsoft\\Windows\\CurrentVersion\\RunOnce"" nocase
        $reg_run3 = ""\\Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\StartupApproved"" nocase
        $reg_run4 = ""\\Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Shell Folders\\Startup"" nocase
        $schtasks = ""schtasks /create"" nocase
        $wmic = ""wmic /create"" nocase
        $startup_folder = ""\\AppData\\Roaming\\Microsoft\\Windows\\Start Menu\\Programs\\Startup"" nocase
        
    condition:
        2 of them
}"
                };
                
                // Règle pour détecter les outils d'extraction de credentials
                var credentialRule = new YaraRule
                {
                    Name = "Credential_Dumper",
                    Description = "Détecte les outils d'extraction de credentials",
                    Author = "IRIS-Forensic X",
                    Severity = 4,
                    Tags = new List<string> { "credential", "mimikatz", "lsass" },
                    IsEnabled = true,
                    Content = @"
rule Credential_Dumper : credential mimikatz lsass
{
    meta:
        description = ""Détecte les outils d'extraction de credentials""
        author = ""IRIS-Forensic X""
        severity = ""4""
        date = ""2025-04-26""
        
    strings:
        $mimikatz1 = ""mimikatz"" nocase
        $mimikatz2 = ""mimilib"" nocase
        $mimikatz3 = ""sekurlsa"" nocase
        $mimikatz4 = ""kerberos"" nocase
        $lsass1 = ""lsass.exe"" nocase
        $lsass2 = ""lsasrv"" nocase
        $lsass3 = ""wdigest"" nocase
        $lsass4 = ""livessp"" nocase
        $dump1 = ""procdump"" nocase
        $dump2 = ""dumpert"" nocase
        $dump3 = ""ProcessDump"" nocase
        $technique1 = ""sekurlsa::logonpasswords"" nocase
        $technique2 = ""privilege::debug"" nocase
        
    condition:
        (2 of ($mimikatz*) and 1 of ($technique*)) or
        (2 of ($lsass*) and 1 of ($dump*))
}"
                };
                
                // Règle pour détecter les backdoors
                var backdoorRule = new YaraRule
                {
                    Name = "Backdoor_Detection",
                    Description = "Détecte les backdoors et shells distants",
                    Author = "IRIS-Forensic X",
                    Severity = 5,
                    Tags = new List<string> { "backdoor", "shell", "remote" },
                    IsEnabled = true,
                    Content = @"
rule Backdoor_Detection : backdoor shell remote
{
    meta:
        description = ""Détecte les backdoors et shells distants""
        author = ""IRIS-Forensic X""
        severity = ""5""
        date = ""2025-04-26""
        
    strings:
        $shell1 = ""cmd.exe"" nocase
        $shell2 = ""powershell.exe"" nocase
        $shell3 = ""bash.exe"" nocase
        $shell4 = ""sh.exe"" nocase
        $connect1 = ""socket"" nocase
        $connect2 = ""connect("" nocase
        $connect3 = ""WSAConnect"" nocase
        $connect4 = ""InternetOpen"" nocase
        $payload1 = ""reverse shell"" nocase
        $payload2 = ""bind shell"" nocase
        $payload3 = ""meterpreter"" nocase
        $payload4 = ""netcat"" nocase
        $payload5 = ""nc.exe"" nocase
        
    condition:
        (1 of ($shell*) and 1 of ($connect*)) or
        (1 of ($payload*) and 1 of ($connect*))
}"
                };
                
                // Règle pour détecter le phishing
                var phishingRule = new YaraRule
                {
                    Name = "Phishing_Detection",
                    Description = "Détecte les indicateurs de phishing",
                    Author = "IRIS-Forensic X",
                    Severity = 3,
                    Tags = new List<string> { "phishing", "email", "social" },
                    IsEnabled = true,
                    Content = @"
rule Phishing_Detection : phishing email social
{
    meta:
        description = ""Détecte les indicateurs de phishing""
        author = ""IRIS-Forensic X""
        severity = ""3""
        date = ""2025-04-26""
        
    strings:
        $subject1 = ""urgent"" nocase
        $subject2 = ""password reset"" nocase
        $subject3 = ""account verification"" nocase
        $subject4 = ""suspicious activity"" nocase
        $subject5 = ""security alert"" nocase
        $content1 = ""click here"" nocase
        $content2 = ""verify your account"" nocase
        $content3 = ""update your information"" nocase
        $content4 = ""confirm your identity"" nocase
        $content5 = ""your account will be suspended"" nocase
        $domain1 = "".tk"" nocase
        $domain2 = "".top"" nocase
        $domain3 = "".xyz"" nocase
        $domain4 = "".ml"" nocase
        
    condition:
        (2 of ($subject*) and 2 of ($content*)) or
        (1 of ($subject*) and 1 of ($content*) and 1 of ($domain*))
}"
                };
                
                // Sauvegarde des règles
                SaveRule(lockbitRule);
                SaveRule(persistenceRule);
                SaveRule(credentialRule);
                SaveRule(backdoorRule);
                SaveRule(phishingRule);
                
                _logService.LogInfo("Règles YARA par défaut créées avec succès");
            }
            catch (Exception ex)
            {
                _logService.LogError($"Erreur lors de la création des règles YARA par défaut: {ex.Message}");
                throw;
            }
        }
    }
}
