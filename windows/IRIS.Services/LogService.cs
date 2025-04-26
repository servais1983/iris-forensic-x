using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using IRIS.Models;

namespace IRIS.Services
{
    public class LogService
    {
        private readonly string _logFilePath;
        private readonly bool _consoleOutput;
        private readonly object _lockObj = new object();

        public LogService(string logFilePath, bool consoleOutput = true)
        {
            _logFilePath = logFilePath;
            _consoleOutput = consoleOutput;
            
            // Création du répertoire de logs si nécessaire
            Directory.CreateDirectory(Path.GetDirectoryName(logFilePath));
            
            // Initialisation du fichier de log
            if (!File.Exists(logFilePath))
            {
                using (StreamWriter sw = File.CreateText(logFilePath))
                {
                    sw.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [INFO] Log initialized for IRIS-Forensic X");
                }
            }
        }

        /// <summary>
        /// Écrit un message de log de type Information
        /// </summary>
        /// <param name="message">Message à logger</param>
        public void LogInfo(string message)
        {
            WriteLog("INFO", message);
        }

        /// <summary>
        /// Écrit un message de log de type Warning
        /// </summary>
        /// <param name="message">Message à logger</param>
        public void LogWarning(string message)
        {
            WriteLog("WARNING", message);
        }

        /// <summary>
        /// Écrit un message de log de type Error
        /// </summary>
        /// <param name="message">Message à logger</param>
        public void LogError(string message)
        {
            WriteLog("ERROR", message);
        }

        /// <summary>
        /// Écrit un message de log de type Debug
        /// </summary>
        /// <param name="message">Message à logger</param>
        public void LogDebug(string message)
        {
            WriteLog("DEBUG", message);
        }

        /// <summary>
        /// Écrit un message de log avec le type spécifié
        /// </summary>
        /// <param name="logType">Type de log</param>
        /// <param name="message">Message à logger</param>
        private void WriteLog(string logType, string message)
        {
            string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{logType}] {message}";
            
            lock (_lockObj)
            {
                try
                {
                    // Écriture dans le fichier
                    using (StreamWriter sw = File.AppendText(_logFilePath))
                    {
                        sw.WriteLine(logEntry);
                    }
                    
                    // Affichage dans la console si activé
                    if (_consoleOutput)
                    {
                        ConsoleColor originalColor = Console.ForegroundColor;
                        
                        switch (logType)
                        {
                            case "INFO":
                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                            case "WARNING":
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                break;
                            case "ERROR":
                                Console.ForegroundColor = ConsoleColor.Red;
                                break;
                            case "DEBUG":
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                break;
                        }
                        
                        Console.WriteLine(logEntry);
                        Console.ForegroundColor = originalColor;
                    }
                }
                catch (Exception ex)
                {
                    // En cas d'erreur lors de l'écriture du log, on affiche l'erreur dans la console
                    if (_consoleOutput)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [ERROR] Erreur lors de l'écriture du log: {ex.Message}");
                        Console.ResetColor();
                    }
                }
            }
        }

        /// <summary>
        /// Récupère les dernières entrées du log
        /// </summary>
        /// <param name="count">Nombre d'entrées à récupérer</param>
        /// <returns>Liste des dernières entrées de log</returns>
        public List<string> GetRecentLogs(int count = 100)
        {
            var logs = new List<string>();
            
            try
            {
                if (File.Exists(_logFilePath))
                {
                    var allLines = File.ReadAllLines(_logFilePath);
                    int startIndex = Math.Max(0, allLines.Length - count);
                    
                    for (int i = startIndex; i < allLines.Length; i++)
                    {
                        logs.Add(allLines[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                if (_consoleOutput)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [ERROR] Erreur lors de la lecture des logs: {ex.Message}");
                    Console.ResetColor();
                }
            }
            
            return logs;
        }

        /// <summary>
        /// Efface le fichier de log
        /// </summary>
        public void ClearLogs()
        {
            try
            {
                File.WriteAllText(_logFilePath, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [INFO] Log cleared for IRIS-Forensic X\n");
                
                if (_consoleOutput)
                {
                    Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [INFO] Log cleared");
                }
            }
            catch (Exception ex)
            {
                if (_consoleOutput)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [ERROR] Erreur lors de l'effacement des logs: {ex.Message}");
                    Console.ResetColor();
                }
            }
        }
    }
}
