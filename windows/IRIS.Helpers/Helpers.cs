using System;
using System.Collections.Generic;
using System.Windows.Controls;
using IRIS.Core.Models;

namespace IRIS.Helpers
{
    public static class NavigationHelper
    {
        public static void NavigateTo(Frame frame, Page page)
        {
            frame.Navigate(page);
        }

        public static void GoBack(Frame frame)
        {
            if (frame.CanGoBack)
            {
                frame.GoBack();
            }
        }

        public static void GoForward(Frame frame)
        {
            if (frame.CanGoForward)
            {
                frame.GoForward();
            }
        }
    }

    public static class UIHelper
    {
        public static string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            return string.Format("{0:0.##} {1}", len, sizes[order]);
        }

        public static string GetThreatLevelText(int threatScore)
        {
            if (threatScore < 20) return "Faible";
            if (threatScore < 50) return "Moyen";
            if (threatScore < 80) return "Élevé";
            return "Critique";
        }

        public static string GetStatusColor(string status)
        {
            switch (status.ToLower())
            {
                case "success":
                case "completed":
                    return "#4CAF50";
                case "warning":
                case "pending":
                    return "#FFC107";
                case "error":
                case "failed":
                    return "#F44336";
                default:
                    return "#2196F3";
            }
        }
    }

    public static class ValidationHelper
    {
        public static bool IsValidPath(string path)
        {
            try
            {
                System.IO.Path.GetFullPath(path);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValidIpAddress(string ipAddress)
        {
            if (string.IsNullOrWhiteSpace(ipAddress))
                return false;

            string[] parts = ipAddress.Split('.');
            if (parts.Length != 4)
                return false;

            foreach (string part in parts)
            {
                if (!int.TryParse(part, out int value) || value < 0 || value > 255)
                    return false;
            }

            return true;
        }

        public static bool IsValidPort(string port)
        {
            if (string.IsNullOrWhiteSpace(port))
                return false;

            if (!int.TryParse(port, out int value))
                return false;

            return value > 0 && value < 65536;
        }
    }

    public static class ReportHelper
    {
        public static string GenerateHtmlReport(List<AnalysisResult> results, string title, string author)
        {
            string html = $@"
<!DOCTYPE html>
<html lang=""fr"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>{title}</title>
    <style>
        body {{ font-family: Arial, sans-serif; margin: 0; padding: 20px; }}
        h1, h2, h3 {{ color: #2c3e50; }}
        .header {{ background-color: #3498db; color: white; padding: 20px; margin-bottom: 20px; }}
        .section {{ margin-bottom: 30px; }}
        .threat-high {{ background-color: #f8d7da; padding: 10px; border-radius: 5px; }}
        .threat-medium {{ background-color: #fff3cd; padding: 10px; border-radius: 5px; }}
        .threat-low {{ background-color: #d1e7dd; padding: 10px; border-radius: 5px; }}
        table {{ width: 100%; border-collapse: collapse; margin-bottom: 20px; }}
        th, td {{ padding: 12px; text-align: left; border-bottom: 1px solid #ddd; }}
        th {{ background-color: #f2f2f2; }}
        tr:hover {{ background-color: #f5f5f5; }}
        .footer {{ margin-top: 50px; text-align: center; font-size: 12px; color: #7f8c8d; }}
    </style>
</head>
<body>
    <div class=""header"">
        <h1>{title}</h1>
        <p>Généré le {DateTime.Now.ToString("dd/MM/yyyy à HH:mm")}</p>
        <p>Auteur: {author}</p>
    </div>

    <div class=""section"">
        <h2>Résumé</h2>
        <p>Nombre total d'éléments analysés: {results.Count}</p>
        <p>Niveau de menace moyen: {CalculateAverageThreatScore(results)}/100</p>
    </div>

    <div class=""section"">
        <h2>Détails des analyses</h2>
        <table>
            <tr>
                <th>ID</th>
                <th>Date d'analyse</th>
                <th>Score de menace</th>
                <th>Menaces détectées</th>
                <th>Statut</th>
            </tr>";

            foreach (var result in results)
            {
                string threatClass = result.ThreatScore >= 80 ? "threat-high" : 
                                    result.ThreatScore >= 50 ? "threat-medium" : "threat-low";
                
                html += $@"
            <tr class=""{threatClass}"">
                <td>{result.Id}</td>
                <td>{result.AnalysisDate.ToString("dd/MM/yyyy HH:mm")}</td>
                <td>{result.ThreatScore}/100</td>
                <td>{string.Join(", ", result.DetectedThreats ?? new List<string>())}</td>
                <td>{result.Status}</td>
            </tr>";
            }

            html += $@"
        </table>
    </div>

    <div class=""section"">
        <h2>Recommandations</h2>
        <ul>";

            foreach (var result in results)
            {
                foreach (var recommendation in result.Recommendations ?? new List<string>())
                {
                    html += $@"
            <li>{recommendation}</li>";
                }
            }

            html += $@"
        </ul>
    </div>

    <div class=""footer"">
        <p>Rapport généré par IRIS-Forensic X - Copyright © 2025</p>
    </div>
</body>
</html>";

            return html;
        }

        private static int CalculateAverageThreatScore(List<AnalysisResult> results)
        {
            if (results == null || results.Count == 0)
                return 0;

            int sum = 0;
            foreach (var result in results)
            {
                sum += result.ThreatScore;
            }

            return sum / results.Count;
        }
    }
}
