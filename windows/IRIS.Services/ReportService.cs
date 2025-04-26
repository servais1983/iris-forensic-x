using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using IRIS.Models;

namespace IRIS.Services
{
    public class ReportService
    {
        private readonly LogService _logService;
        private readonly string _reportsPath;
        private readonly string _templatesPath;

        public ReportService(string reportsPath, string templatesPath, LogService logService)
        {
            _reportsPath = reportsPath;
            _templatesPath = templatesPath;
            _logService = logService;
            
            // Création des répertoires si nécessaire
            Directory.CreateDirectory(_reportsPath);
            Directory.CreateDirectory(_templatesPath);
        }

        /// <summary>
        /// Génère un rapport HTML pour un cas forensique
        /// </summary>
        /// <param name="forensicCase">Cas forensique</param>
        /// <param name="includeThreatDetails">Inclure les détails des menaces</param>
        /// <param name="includeTimeline">Inclure la timeline</param>
        /// <param name="includeBlockchainProof">Inclure les preuves blockchain</param>
        /// <returns>Chemin du rapport généré</returns>
        public async Task<string> GenerateHtmlReportAsync(ForensicCase forensicCase, bool includeThreatDetails = true, bool includeTimeline = true, bool includeBlockchainProof = true)
        {
            try
            {
                _logService.LogInfo($"Génération du rapport HTML pour le cas: {forensicCase.Id}");
                
                string reportFileName = $"Rapport_{forensicCase.Id}_{DateTime.Now:yyyyMMdd_HHmmss}.html";
                string reportFilePath = Path.Combine(_reportsPath, reportFileName);
                
                // Chargement du template HTML
                string templatePath = Path.Combine(_templatesPath, "report_template.html");
                string template;
                
                if (File.Exists(templatePath))
                {
                    template = File.ReadAllText(templatePath);
                }
                else
                {
                    // Template par défaut si le fichier n'existe pas
                    template = GetDefaultHtmlTemplate();
                }
                
                // Remplacement des placeholders dans le template
                string report = template
                    .Replace("{{REPORT_TITLE}}", $"Rapport d'analyse forensique - {forensicCase.Name}")
                    .Replace("{{CASE_NAME}}", forensicCase.Name)
                    .Replace("{{CASE_ID}}", forensicCase.Id)
                    .Replace("{{CASE_DESCRIPTION}}", forensicCase.Description)
                    .Replace("{{CASE_DATE}}", forensicCase.CreatedDate.ToString("dd/MM/yyyy HH:mm:ss"))
                    .Replace("{{INVESTIGATOR}}", forensicCase.Investigator)
                    .Replace("{{GENERATION_DATE}}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"))
                    .Replace("{{THREAT_SCORE}}", forensicCase.ThreatScore.ToString())
                    .Replace("{{THREAT_LEVEL}}", GetThreatLevelText(forensicCase.ThreatScore));
                
                // Génération du contenu des artefacts
                StringBuilder artifactsContent = new StringBuilder();
                if (forensicCase.Artifacts != null && forensicCase.Artifacts.Count > 0)
                {
                    foreach (var artifact in forensicCase.Artifacts)
                    {
                        artifactsContent.AppendLine("<div class='artifact-item'>");
                        artifactsContent.AppendLine($"<h4>{artifact.Name}</h4>");
                        artifactsContent.AppendLine("<table class='artifact-details'>");
                        artifactsContent.AppendLine($"<tr><td>Type:</td><td>{artifact.Type}</td></tr>");
                        artifactsContent.AppendLine($"<tr><td>Taille:</td><td>{FormatFileSize(artifact.Size)}</td></tr>");
                        artifactsContent.AppendLine($"<tr><td>Chemin:</td><td>{artifact.Path}</td></tr>");
                        artifactsContent.AppendLine($"<tr><td>Date de création:</td><td>{artifact.CreatedDate:dd/MM/yyyy HH:mm:ss}</td></tr>");
                        artifactsContent.AppendLine($"<tr><td>Date de modification:</td><td>{artifact.ModifiedDate:dd/MM/yyyy HH:mm:ss}</td></tr>");
                        artifactsContent.AppendLine($"<tr><td>Hash MD5:</td><td>{artifact.MD5Hash}</td></tr>");
                        artifactsContent.AppendLine($"<tr><td>Hash SHA256:</td><td>{artifact.SHA256Hash}</td></tr>");
                        
                        if (artifact.Tags != null && artifact.Tags.Count > 0)
                        {
                            artifactsContent.AppendLine($"<tr><td>Tags:</td><td>{string.Join(", ", artifact.Tags)}</td></tr>");
                        }
                        
                        artifactsContent.AppendLine($"<tr><td>Score de menace:</td><td>{artifact.ThreatScore}</td></tr>");
                        artifactsContent.AppendLine("</table>");
                        
                        // Ajout des menaces détectées si demandé
                        if (includeThreatDetails && artifact.DetectedThreats != null && artifact.DetectedThreats.Count > 0)
                        {
                            artifactsContent.AppendLine("<div class='threats-container'>");
                            artifactsContent.AppendLine("<h5>Menaces détectées:</h5>");
                            
                            foreach (var threat in artifact.DetectedThreats)
                            {
                                artifactsContent.AppendLine("<div class='threat-item'>");
                                artifactsContent.AppendLine($"<div class='threat-header severity-{threat.Severity}'>");
                                artifactsContent.AppendLine($"<span class='threat-name'>{threat.Name}</span>");
                                artifactsContent.AppendLine($"<span class='threat-severity'>Sévérité: {threat.Severity}/5</span>");
                                artifactsContent.AppendLine("</div>");
                                artifactsContent.AppendLine("<div class='threat-details'>");
                                artifactsContent.AppendLine($"<p><strong>Type:</strong> {threat.Type}</p>");
                                artifactsContent.AppendLine($"<p><strong>Description:</strong> {threat.Description}</p>");
                                
                                if (!string.IsNullOrEmpty(threat.MitreTactic) || !string.IsNullOrEmpty(threat.MitreTechnique))
                                {
                                    artifactsContent.AppendLine("<p><strong>MITRE ATT&CK:</strong> ");
                                    if (!string.IsNullOrEmpty(threat.MitreTactic))
                                    {
                                        artifactsContent.Append($"Tactique: {threat.MitreTactic}");
                                    }
                                    if (!string.IsNullOrEmpty(threat.MitreTechnique))
                                    {
                                        artifactsContent.Append($" | Technique: {threat.MitreTechnique}");
                                    }
                                    artifactsContent.AppendLine("</p>");
                                }
                                
                                if (!string.IsNullOrEmpty(threat.YaraRule))
                                {
                                    artifactsContent.AppendLine($"<p><strong>Règle YARA:</strong> {threat.YaraRule}</p>");
                                }
                                
                                if (threat.Indicators != null && threat.Indicators.Count > 0)
                                {
                                    artifactsContent.AppendLine("<p><strong>Indicateurs:</strong></p>");
                                    artifactsContent.AppendLine("<ul>");
                                    foreach (var indicator in threat.Indicators)
                                    {
                                        artifactsContent.AppendLine($"<li>{indicator}</li>");
                                    }
                                    artifactsContent.AppendLine("</ul>");
                                }
                                
                                if (threat.Recommendations != null && threat.Recommendations.Count > 0)
                                {
                                    artifactsContent.AppendLine("<p><strong>Recommandations:</strong></p>");
                                    artifactsContent.AppendLine("<ul>");
                                    foreach (var recommendation in threat.Recommendations)
                                    {
                                        artifactsContent.AppendLine($"<li>{recommendation}</li>");
                                    }
                                    artifactsContent.AppendLine("</ul>");
                                }
                                
                                artifactsContent.AppendLine("</div>"); // threat-details
                                artifactsContent.AppendLine("</div>"); // threat-item
                            }
                            
                            artifactsContent.AppendLine("</div>"); // threats-container
                        }
                        
                        artifactsContent.AppendLine("</div>"); // artifact-item
                    }
                }
                else
                {
                    artifactsContent.AppendLine("<p>Aucun artefact trouvé.</p>");
                }
                
                report = report.Replace("{{ARTIFACTS_CONTENT}}", artifactsContent.ToString());
                
                // Génération du contenu des preuves
                StringBuilder evidencesContent = new StringBuilder();
                if (forensicCase.Evidences != null && forensicCase.Evidences.Count > 0)
                {
                    foreach (var evidence in forensicCase.Evidences)
                    {
                        evidencesContent.AppendLine("<div class='evidence-item'>");
                        evidencesContent.AppendLine($"<h4>{evidence.Name}</h4>");
                        evidencesContent.AppendLine("<table class='evidence-details'>");
                        evidencesContent.AppendLine($"<tr><td>Type:</td><td>{evidence.Type}</td></tr>");
                        evidencesContent.AppendLine($"<tr><td>Description:</td><td>{evidence.Description}</td></tr>");
                        evidencesContent.AppendLine($"<tr><td>Date de découverte:</td><td>{evidence.DiscoveredDate:dd/MM/yyyy HH:mm:ss}</td></tr>");
                        evidencesContent.AppendLine($"<tr><td>Source:</td><td>{evidence.Source}</td></tr>");
                        evidencesContent.AppendLine($"<tr><td>Sévérité:</td><td>{evidence.Severity}/5</td></tr>");
                        
                        if (!string.IsNullOrEmpty(evidence.MitreTactic) || !string.IsNullOrEmpty(evidence.MitreTechnique))
                        {
                            evidencesContent.AppendLine("<tr><td>MITRE ATT&CK:</td><td>");
                            if (!string.IsNullOrEmpty(evidence.MitreTactic))
                            {
                                evidencesContent.Append($"Tactique: {evidence.MitreTactic}");
                            }
                            if (!string.IsNullOrEmpty(evidence.MitreTechnique))
                            {
                                evidencesContent.Append($" | Technique: {evidence.MitreTechnique}");
                            }
                            evidencesContent.AppendLine("</td></tr>");
                        }
                        
                        // Ajout des informations blockchain si demandé
                        if (includeBlockchainProof && !string.IsNullOrEmpty(evidence.BlockchainHash))
                        {
                            evidencesContent.AppendLine("<tr><td>Preuve blockchain:</td><td>");
                            evidencesContent.AppendLine($"<div class='blockchain-proof'>{evidence.BlockchainHash}</div>");
                            evidencesContent.AppendLine($"<div class='verification-status {(evidence.IsVerified ? "verified" : "unverified")}'>");
                            evidencesContent.AppendLine(evidence.IsVerified ? "✓ Vérifié" : "⚠ Non vérifié");
                            evidencesContent.AppendLine("</div>");
                            evidencesContent.AppendLine("</td></tr>");
                        }
                        
                        if (evidence.RelatedArtifacts != null && evidence.RelatedArtifacts.Count > 0)
                        {
                            evidencesContent.AppendLine($"<tr><td>Artefacts liés:</td><td>{string.Join(", ", evidence.RelatedArtifacts)}</td></tr>");
                        }
                        
                        evidencesContent.AppendLine("</table>");
                        evidencesContent.AppendLine("</div>");
                    }
                }
                else
                {
                    evidencesContent.AppendLine("<p>Aucune preuve trouvée.</p>");
                }
                
                report = report.Replace("{{EVIDENCES_CONTENT}}", evidencesContent.ToString());
                
                // Génération de la timeline si demandée
                if (includeTimeline)
                {
                    StringBuilder timelineContent = new StringBuilder();
                    
                    // Dans une implémentation réelle, on construirait une timeline à partir des artefacts et preuves
                    // Pour cette démonstration, nous créons une timeline fictive
                    timelineContent.AppendLine("<div class='timeline'>");
                    
                    if (forensicCase.Artifacts != null && forensicCase.Artifacts.Count > 0)
                    {
                        var timelineEvents = new List<(DateTime Date, string Event, string Type, int Severity)>();
                        
                        // Ajout des artefacts à la timeline
                        foreach (var artifact in forensicCase.Artifacts)
                        {
                            timelineEvents.Add((artifact.CreatedDate, $"Création de l'artefact: {artifact.Name}", "artifact-creation", 0));
                            timelineEvents.Add((artifact.ModifiedDate, $"Modification de l'artefact: {artifact.Name}", "artifact-modification", 0));
                            
                            // Ajout des menaces détectées
                            if (artifact.DetectedThreats != null)
                            {
                                foreach (var threat in artifact.DetectedThreats)
                                {
                                    timelineEvents.Add((threat.DetectedDate, $"Détection de menace: {threat.Name} dans {artifact.Name}", "threat-detection", threat.Severity));
                                }
                            }
                        }
                        
                        // Ajout des preuves à la timeline
                        if (forensicCase.Evidences != null)
                        {
                            foreach (var evidence in forensicCase.Evidences)
                            {
                                timelineEvents.Add((evidence.DiscoveredDate, $"Découverte de preuve: {evidence.Name}", "evidence-discovery", evidence.Severity));
                            }
                        }
                        
                        // Tri des événements par date
                        timelineEvents.Sort((a, b) => a.Date.CompareTo(b.Date));
                        
                        // Génération du HTML pour la timeline
                        foreach (var evt in timelineEvents)
                        {
                            timelineContent.AppendLine("<div class='timeline-item'>");
                            timelineContent.AppendLine($"<div class='timeline-date'>{evt.Date:dd/MM/yyyy HH:mm:ss}</div>");
                            timelineContent.AppendLine($"<div class='timeline-event {evt.Type} {(evt.Severity > 0 ? $"severity-{evt.Severity}" : "")}'>");
                            timelineContent.AppendLine($"<span>{evt.Event}</span>");
                            if (evt.Severity > 0)
                            {
                                timelineContent.AppendLine($"<span class='event-severity'>Sévérité: {evt.Severity}/5</span>");
                            }
                            timelineContent.AppendLine("</div>");
                            timelineContent.AppendLine("</div>");
                        }
                    }
                    else
                    {
                        timelineContent.AppendLine("<p>Aucun événement dans la timeline.</p>");
                    }
                    
                    timelineContent.AppendLine("</div>");
                    report = report.Replace("{{TIMELINE_CONTENT}}", timelineContent.ToString());
                }
                else
                {
                    report = report.Replace("{{TIMELINE_CONTENT}}", "<p>Timeline non incluse dans ce rapport.</p>");
                }
                
                // Génération des recommandations
                StringBuilder recommendationsContent = new StringBuilder();
                recommendationsContent.AppendLine("<div class='recommendations'>");
                recommendationsContent.AppendLine("<h3>Recommandations</h3>");
                
                // Dans une implémentation réelle, on générerait des recommandations basées sur les menaces détectées
                // Pour cette démonstration, nous créons des recommandations génériques
                var recommendations = new List<(string Title, string Description, int Priority)>();
                
                if (forensicCase.Artifacts != null)
                {
                    foreach (var artifact in forensicCase.Artifacts)
                    {
                        if (artifact.DetectedThreats != null)
                        {
                            foreach (var threat in artifact.DetectedThreats)
                            {
                                if (threat.Recommendations != null)
                                {
                                    foreach (var rec in threat.Recommendations)
                                    {
                                        recommendations.Add((rec, $"Basé sur la menace {threat.Name} détectée dans {artifact.Name}", threat.Severity));
                                    }
                                }
                            }
                        }
                    }
                }
                
                // Ajout de recommandations génériques si nécessaire
                if (recommendations.Count == 0)
                {
                    recommendations.Add(("Effectuer une analyse complète du système", "Utiliser des outils forensiques avancés pour analyser en profondeur le système", 3));
                    recommendations.Add(("Mettre à jour les logiciels de sécurité", "S'assurer que tous les logiciels de sécurité sont à jour", 4));
                    recommendations.Add(("Renforcer les politiques de mot de passe", "Implémenter des politiques de mot de passe plus strictes", 3));
                    recommendations.Add(("Activer l'authentification à deux facteurs", "Activer l'authentification à deux facteurs pour tous les comptes critiques", 5));
                    recommendations.Add(("Former les utilisateurs à la sécurité", "Organiser des sessions de formation sur la sécurité informatique", 2));
                }
                
                // Tri des recommandations par priorité (décroissante)
                recommendations.Sort((a, b) => b.Priority.CompareTo(a.Priority));
                
                // Génération du HTML pour les recommandations
                foreach (var rec in recommendations)
                {
                    recommendationsContent.AppendLine("<div class='recommendation-item'>");
                    recommendationsContent.AppendLine($"<div class='recommendation-header priority-{rec.Priority}'>");
                    recommendationsContent.AppendLine($"<span class='recommendation-title'>{rec.Title}</span>");
                    recommendationsContent.AppendLine($"<span class='recommendation-priority'>Priorité: {rec.Priority}/5</span>");
                    recommendationsContent.AppendLine("</div>");
                    recommendationsContent.AppendLine("<div class='recommendation-description'>");
                    recommendationsContent.AppendLine($"<p>{rec.Description}</p>");
                    recommendationsContent.AppendLine("</div>");
                    recommendationsContent.AppendLine("</div>");
                }
                
                recommendationsContent.AppendLine("</div>");
                report = report.Replace("{{RECOMMENDATIONS_CONTENT}}", recommendationsContent.ToString());
                
                // Écriture du rapport dans un fichier
                await File.WriteAllTextAsync(reportFilePath, report);
                
                _logService.LogInfo($"Rapport HTML généré avec succès: {reportFilePath}");
                return reportFilePath;
            }
            catch (Exception ex)
            {
                _logService.LogError($"Erreur lors de la génération du rapport HTML: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Formate la taille d'un fichier en unités lisibles
        /// </summary>
        /// <param name="bytes">Taille en octets</param>
        /// <returns>Taille formatée</returns>
        private string FormatFileSize(long bytes)
        {
            string[] suffixes = { "B", "KB", "MB", "GB", "TB", "PB" };
            int counter = 0;
            decimal number = bytes;
            
            while (Math.Round(number / 1024) >= 1)
            {
                number /= 1024;
                counter++;
            }
            
            return $"{number:n2} {suffixes[counter]}";
        }

        /// <summary>
        /// Obtient le texte du niveau de menace en fonction du score
        /// </summary>
        /// <param name="score">Score de menace (0-100)</param>
        /// <returns>Texte du niveau de menace</returns>
        private string GetThreatLevelText(int score)
        {
            if (score >= 80) return "Critique";
            if (score >= 60) return "Élevé";
            if (score >= 40) return "Moyen";
            if (score >= 20) return "Faible";
            return "Très faible";
        }

        /// <summary>
        /// Obtient le template HTML par défaut pour les rapports
        /// </summary>
        /// <returns>Template HTML</returns>
        private string GetDefaultHtmlTemplate()
        {
            return @"<!DOCTYPE html>
<html lang='fr'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>{{REPORT_TITLE}}</title>
    <style>
        :root {
            --primary-color: #2c3e50;
            --secondary-color: #3498db;
            --accent-color: #e74c3c;
            --background-color: #f5f5f5;
            --text-color: #333;
            --border-color: #ddd;
        }
        
        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            line-height: 1.6;
            color: var(--text-color);
            background-color: var(--background-color);
            margin: 0;
            padding: 0;
        }
        
        .container {
            max-width: 1200px;
            margin: 0 auto;
            padding: 20px;
            background-color: white;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
        }
        
        header {
            background-color: var(--primary-color);
            color: white;
            padding: 20px;
            text-align: center;
            margin-bottom: 30px;
        }
        
        h1, h2, h3, h4, h5, h6 {
            color: var(--primary-color);
            margin-top: 0;
        }
        
        .report-info {
            background-color: #f9f9f9;
            border: 1px solid var(--border-color);
            padding: 15px;
            margin-bottom: 30px;
        }
        
        .report-info table {
            width: 100%;
            border-collapse: collapse;
        }
        
        .report-info td {
            padding: 8px;
            border-bottom: 1px solid var(--border-color);
        }
        
        .report-info td:first-child {
            font-weight: bold;
            width: 200px;
        }
        
        .section {
            margin-bottom: 30px;
            border: 1px solid var(--border-color);
            border-radius: 5px;
            overflow: hidden;
        }
        
        .section-header {
            background-color: var(--secondary-color);
            color: white;
            padding: 10px 15px;
            font-size: 18px;
            font-weight: bold;
        }
        
        .section-content {
            padding: 15px;
        }
        
        .artifact-item, .evidence-item {
            margin-bottom: 20px;
            border: 1px solid var(--border-color);
            border-radius: 5px;
            overflow: hidden;
        }
        
        .artifact-item h4, .evidence-item h4 {
            background-color: #f0f0f0;
            margin: 0;
            padding: 10px;
            border-bottom: 1px solid var(--border-color);
        }
        
        .artifact-details, .evidence-details {
            width: 100%;
            border-collapse: collapse;
        }
        
        .artifact-details td, .evidence-details td {
            padding: 8px;
            border-bottom: 1px solid var(--border-color);
        }
        
        .artifact-details td:first-child, .evidence-details td:first-child {
            font-weight: bold;
            width: 200px;
            background-color: #f9f9f9;
        }
        
        .threats-container {
            margin-top: 15px;
        }
        
        .threat-item {
            margin-bottom: 15px;
            border: 1px solid var(--border-color);
            border-radius: 5px;
            overflow: hidden;
        }
        
        .threat-header {
            padding: 10px;
            display: flex;
            justify-content: space-between;
            align-items: center;
        }
        
        .threat-name {
            font-weight: bold;
        }
        
        .threat-severity {
            font-size: 14px;
        }
        
        .threat-details {
            padding: 10px;
            background-color: #f9f9f9;
        }
        
        .severity-1 { background-color: #4CAF50; color: white; }
        .severity-2 { background-color: #8BC34A; color: white; }
        .severity-3 { background-color: #FFC107; color: black; }
        .severity-4 { background-color: #FF9800; color: white; }
        .severity-5 { background-color: #F44336; color: white; }
        
        .blockchain-proof {
            font-family: monospace;
            word-break: break-all;
            background-color: #f0f0f0;
            padding: 5px;
            border-radius: 3px;
            margin-bottom: 5px;
        }
        
        .verification-status {
            display: inline-block;
            padding: 3px 8px;
            border-radius: 3px;
            font-size: 14px;
        }
        
        .verified {
            background-color: #4CAF50;
            color: white;
        }
        
        .unverified {
            background-color: #FFC107;
            color: black;
        }
        
        .timeline {
            margin-top: 20px;
        }
        
        .timeline-item {
            display: flex;
            margin-bottom: 15px;
            position: relative;
        }
        
        .timeline-item:before {
            content: '';
            position: absolute;
            left: 120px;
            top: 0;
            bottom: 0;
            width: 2px;
            background-color: var(--border-color);
            z-index: 0;
        }
        
        .timeline-date {
            width: 120px;
            padding-right: 20px;
            font-weight: bold;
            text-align: right;
            position: relative;
            z-index: 1;
        }
        
        .timeline-event {
            flex: 1;
            background-color: white;
            border: 1px solid var(--border-color);
            border-radius: 5px;
            padding: 10px;
            margin-left: 20px;
            position: relative;
            z-index: 1;
        }
        
        .timeline-event:before {
            content: '';
            position: absolute;
            left: -10px;
            top: 50%;
            transform: translateY(-50%);
            width: 10px;
            height: 10px;
            background-color: white;
            border-top: 1px solid var(--border-color);
            border-left: 1px solid var(--border-color);
            transform: translateY(-50%) rotate(-45deg);
        }
        
        .artifact-creation { border-left: 4px solid #2196F3; }
        .artifact-modification { border-left: 4px solid #9C27B0; }
        .threat-detection { border-left: 4px solid #F44336; }
        .evidence-discovery { border-left: 4px solid #FF9800; }
        
        .event-severity {
            float: right;
            font-size: 14px;
            font-weight: bold;
        }
        
        .recommendations {
            margin-top: 30px;
        }
        
        .recommendation-item {
            margin-bottom: 15px;
            border: 1px solid var(--border-color);
            border-radius: 5px;
            overflow: hidden;
        }
        
        .recommendation-header {
            padding: 10px;
            display: flex;
            justify-content: space-between;
            align-items: center;
        }
        
        .recommendation-title {
            font-weight: bold;
        }
        
        .recommendation-priority {
            font-size: 14px;
        }
        
        .recommendation-description {
            padding: 10px;
            background-color: #f9f9f9;
        }
        
        .priority-1 { background-color: #4CAF50; color: white; }
        .priority-2 { background-color: #8BC34A; color: white; }
        .priority-3 { background-color: #FFC107; color: black; }
        .priority-4 { background-color: #FF9800; color: white; }
        .priority-5 { background-color: #F44336; color: white; }
        
        footer {
            text-align: center;
            margin-top: 30px;
            padding: 20px;
            background-color: var(--primary-color);
            color: white;
        }
        
        @media print {
            body {
                background-color: white;
            }
            
            .container {
                box-shadow: none;
                max-width: none;
                padding: 0;
            }
            
            .section, .artifact-item, .evidence-item, .threat-item, .recommendation-item {
                break-inside: avoid;
            }
        }
    </style>
</head>
<body>
    <div class='container'>
        <header>
            <h1>{{REPORT_TITLE}}</h1>
            <p>Généré par IRIS-Forensic X</p>
        </header>
        
        <div class='report-info'>
            <table>
                <tr>
                    <td>Nom du cas:</td>
                    <td>{{CASE_NAME}}</td>
                </tr>
                <tr>
                    <td>ID du cas:</td>
                    <td>{{CASE_ID}}</td>
                </tr>
                <tr>
                    <td>Description:</td>
                    <td>{{CASE_DESCRIPTION}}</td>
                </tr>
                <tr>
                    <td>Date du cas:</td>
                    <td>{{CASE_DATE}}</td>
                </tr>
                <tr>
                    <td>Investigateur:</td>
                    <td>{{INVESTIGATOR}}</td>
                </tr>
                <tr>
                    <td>Date de génération:</td>
                    <td>{{GENERATION_DATE}}</td>
                </tr>
                <tr>
                    <td>Score de menace:</td>
                    <td>{{THREAT_SCORE}}/100 ({{THREAT_LEVEL}})</td>
                </tr>
            </table>
        </div>
        
        <div class='section'>
            <div class='section-header'>Artefacts analysés</div>
            <div class='section-content'>
                {{ARTIFACTS_CONTENT}}
            </div>
        </div>
        
        <div class='section'>
            <div class='section-header'>Preuves découvertes</div>
            <div class='section-content'>
                {{EVIDENCES_CONTENT}}
            </div>
        </div>
        
        <div class='section'>
            <div class='section-header'>Timeline des événements</div>
            <div class='section-content'>
                {{TIMELINE_CONTENT}}
            </div>
        </div>
        
        <div class='section'>
            <div class='section-header'>Recommandations</div>
            <div class='section-content'>
                {{RECOMMENDATIONS_CONTENT}}
            </div>
        </div>
        
        <footer>
            <p>Ce rapport a été généré automatiquement par IRIS-Forensic X.</p>
            <p>© 2025 IRIS-Forensic X - Tous droits réservés</p>
        </footer>
    </div>
</body>
</html>";
        }
    }
}
