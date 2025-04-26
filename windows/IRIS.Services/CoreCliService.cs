using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using IRIS.Models;
using System.Collections.Generic;

namespace IRIS.Services
{
    public class CoreCliService
    {
        private readonly string _coreCliPath;
        private readonly LogService _logService;

        public CoreCliService(string coreCliPath, LogService logService)
        {
            _coreCliPath = coreCliPath;
            _logService = logService;
        }

        /// <summary>
        /// Exécute une commande du Core CLI
        /// </summary>
        /// <param name="command">Commande à exécuter</param>
        /// <param name="arguments">Arguments de la commande</param>
        /// <returns>Résultat de la commande</returns>
        public async Task<string> ExecuteCommandAsync(string command, string arguments)
        {
            try
            {
                _logService.LogInfo($"Exécution de la commande Core CLI: {command} {arguments}");
                
                var startInfo = new ProcessStartInfo
                {
                    FileName = Path.Combine(_coreCliPath, "iris"),
                    Arguments = $"{command} {arguments}",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using var process = new Process { StartInfo = startInfo };
                var outputBuilder = new StringBuilder();
                var errorBuilder = new StringBuilder();

                process.OutputDataReceived += (sender, args) =>
                {
                    if (args.Data != null)
                    {
                        outputBuilder.AppendLine(args.Data);
                    }
                };

                process.ErrorDataReceived += (sender, args) =>
                {
                    if (args.Data != null)
                    {
                        errorBuilder.AppendLine(args.Data);
                    }
                };

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                await process.WaitForExitAsync();

                if (process.ExitCode != 0)
                {
                    var errorMessage = errorBuilder.ToString();
                    _logService.LogError($"Erreur lors de l'exécution de la commande Core CLI: {errorMessage}");
                    throw new Exception($"Erreur lors de l'exécution de la commande Core CLI: {errorMessage}");
                }

                var output = outputBuilder.ToString();
                _logService.LogInfo($"Commande Core CLI exécutée avec succès: {output}");
                return output;
            }
            catch (Exception ex)
            {
                _logService.LogError($"Exception lors de l'exécution de la commande Core CLI: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Capture des données forensiques
        /// </summary>
        /// <param name="sourcePath">Chemin de la source à analyser</param>
        /// <param name="outputPath">Chemin de sortie pour les résultats</param>
        /// <param name="captureType">Type de capture (memory, logs, network)</param>
        /// <returns>Résultat de la capture</returns>
        public async Task<string> CaptureAsync(string sourcePath, string outputPath, string captureType = "smart")
        {
            var arguments = $"--{captureType} \"{sourcePath}\" --output \"{outputPath}\"";
            return await ExecuteCommandAsync("capture", arguments);
        }

        /// <summary>
        /// Analyse des données forensiques capturées
        /// </summary>
        /// <param name="sourcePath">Chemin des données à analyser</param>
        /// <param name="outputPath">Chemin de sortie pour les résultats</param>
        /// <param name="analysisType">Type d'analyse (all, classic, ai)</param>
        /// <returns>Résultat de l'analyse</returns>
        public async Task<string> AnalyzeAsync(string sourcePath, string outputPath, string analysisType = "all")
        {
            var arguments = $"--{analysisType} \"{sourcePath}\" --output \"{outputPath}\"";
            return await ExecuteCommandAsync("analyze", arguments);
        }

        /// <summary>
        /// Triage des cas forensiques
        /// </summary>
        /// <param name="casesPath">Chemin des cas à trier</param>
        /// <param name="triageType">Type de triage (auto, manual)</param>
        /// <returns>Résultat du triage</returns>
        public async Task<string> TriageAsync(string casesPath, string triageType = "auto")
        {
            var arguments = $"--{triageType} \"{casesPath}\"";
            return await ExecuteCommandAsync("triage", arguments);
        }

        /// <summary>
        /// Réponse aux incidents détectés
        /// </summary>
        /// <param name="caseId">ID du cas</param>
        /// <param name="responseType">Type de réponse (auto, manual)</param>
        /// <param name="rule">Règle à appliquer</param>
        /// <returns>Résultat de la réponse</returns>
        public async Task<string> RespondAsync(string caseId, string responseType = "auto", string rule = "")
        {
            var arguments = $"--{responseType} --case \"{caseId}\"";
            if (!string.IsNullOrEmpty(rule))
            {
                arguments += $" --rule \"{rule}\"";
            }
            return await ExecuteCommandAsync("respond", arguments);
        }

        /// <summary>
        /// Analyse d'un fichier VMDK
        /// </summary>
        /// <param name="vmdkPath">Chemin du fichier VMDK</param>
        /// <param name="outputPath">Chemin de sortie pour les résultats</param>
        /// <returns>Résultat de l'analyse VMDK</returns>
        public async Task<VmdkConfiguration> AnalyzeVmdkAsync(string vmdkPath, string outputPath)
        {
            try
            {
                _logService.LogInfo($"Début de l'analyse VMDK: {vmdkPath}");
                
                var arguments = $"--vmdk \"{vmdkPath}\" --output \"{outputPath}\"";
                var result = await ExecuteCommandAsync("analyze", arguments);
                
                // Création d'une configuration VMDK à partir du résultat
                var vmdkConfig = new VmdkConfiguration
                {
                    Name = Path.GetFileName(vmdkPath),
                    Path = vmdkPath,
                    Size = new FileInfo(vmdkPath).Length,
                    IsAnalyzed = true
                };
                
                // Analyse du résultat pour extraire les informations sur les partitions
                // (Dans une implémentation réelle, cela serait fait en analysant la sortie JSON du CLI)
                vmdkConfig.Partitions = new List<string> { "Partition 1", "Partition 2" };
                vmdkConfig.FileSystemTypes = new List<string> { "NTFS", "FAT32" };
                
                _logService.LogInfo($"Analyse VMDK terminée avec succès: {vmdkPath}");
                return vmdkConfig;
            }
            catch (Exception ex)
            {
                _logService.LogError($"Erreur lors de l'analyse VMDK: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Analyse de fichiers logs
        /// </summary>
        /// <param name="logPath">Chemin du fichier log</param>
        /// <param name="outputPath">Chemin de sortie pour les résultats</param>
        /// <returns>Résultat de l'analyse de logs</returns>
        public async Task<string> AnalyzeLogsAsync(string logPath, string outputPath)
        {
            var arguments = $"--logs \"{logPath}\" --output \"{outputPath}\"";
            return await ExecuteCommandAsync("analyze", arguments);
        }

        /// <summary>
        /// Analyse de fichiers CSV
        /// </summary>
        /// <param name="csvPath">Chemin du fichier CSV</param>
        /// <param name="outputPath">Chemin de sortie pour les résultats</param>
        /// <returns>Résultat de l'analyse CSV</returns>
        public async Task<string> AnalyzeCsvAsync(string csvPath, string outputPath)
        {
            var arguments = $"--csv \"{csvPath}\" --output \"{outputPath}\"";
            return await ExecuteCommandAsync("analyze", arguments);
        }

        /// <summary>
        /// Génération d'un rapport forensique
        /// </summary>
        /// <param name="caseId">ID du cas</param>
        /// <param name="outputPath">Chemin de sortie pour le rapport</param>
        /// <param name="format">Format du rapport (html, pdf, docx)</param>
        /// <returns>Chemin du rapport généré</returns>
        public async Task<string> GenerateReportAsync(string caseId, string outputPath, string format = "html")
        {
            var arguments = $"--case \"{caseId}\" --output \"{outputPath}\" --format {format}";
            var result = await ExecuteCommandAsync("report", arguments);
            
            // Dans une implémentation réelle, on extrairait le chemin du rapport généré
            return Path.Combine(outputPath, $"report_{caseId}.{format}");
        }
    }
}
