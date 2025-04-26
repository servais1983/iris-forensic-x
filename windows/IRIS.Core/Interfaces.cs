using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IRIS.Models;

namespace IRIS.Core.Interfaces
{
    public interface ILogService
    {
        void LogInfo(string message);
        void LogWarning(string message);
        void LogError(string message);
        void LogDebug(string message);
        List<LogEntry> GetRecentLogs(int count = 100);
        void SaveLogs(string filePath);
    }

    public interface ITriageService
    {
        event EventHandler<TriageProgressEventArgs> ProgressChanged;
        Task<List<EvidenceItem>> TriageEvidenceAsync(List<EvidenceItem> evidenceItems, TriageConfig config);
        Task<bool> SaveTriageResultsAsync(string filePath, List<EvidenceItem> results);
        Task<List<EvidenceItem>> LoadTriageResultsAsync(string filePath);
        Task<TriageConfig> GetDefaultConfigAsync();
    }

    public interface IAnalysisService
    {
        event EventHandler<AnalysisProgressEventArgs> ProgressChanged;
        Task<List<AnalysisResult>> AnalyzeEvidenceAsync(List<EvidenceItem> evidenceItems, AnalysisConfig config);
        Task<bool> SaveAnalysisResultsAsync(string filePath, List<AnalysisResult> results);
        Task<List<AnalysisResult>> LoadAnalysisResultsAsync(string filePath);
        Task<AnalysisConfig> GetDefaultConfigAsync();
    }

    public interface ICaptureService
    {
        event EventHandler<CaptureProgressEventArgs> ProgressChanged;
        Task<List<CaptureResult>> CaptureEvidenceAsync(List<CaptureTarget> targets, CaptureConfig config);
        Task<bool> SaveCaptureResultsAsync(string filePath, List<CaptureResult> results);
        Task<List<CaptureResult>> LoadCaptureResultsAsync(string filePath);
        Task<CaptureConfig> GetDefaultConfigAsync();
    }

    public interface IAIService
    {
        Task<string> AnalyzeTextAsync(string text);
        Task<List<string>> DetectThreatsAsync(byte[] data);
        Task<double> CalculateThreatScoreAsync(EvidenceItem evidence);
        Task<List<string>> GenerateRecommendationsAsync(List<AnalysisResult> results);
        Task<string> SummarizeReportAsync(string reportContent);
    }

    public interface IEvidenceService
    {
        Task<List<EvidenceItem>> GetAllEvidenceAsync();
        Task<EvidenceItem> GetEvidenceByIdAsync(string id);
        Task<bool> SaveEvidenceAsync(EvidenceItem evidence);
        Task<bool> DeleteEvidenceAsync(string id);
        Task<bool> ExportEvidenceAsync(string id, string exportPath);
        Task<string> CalculateHashAsync(string filePath);
    }

    public interface ITargetService
    {
        Task<List<CaptureTarget>> GetAvailableTargetsAsync();
        Task<CaptureTarget> GetTargetByIdAsync(string id);
        Task<bool> SaveTargetAsync(CaptureTarget target);
        Task<bool> DeleteTargetAsync(string id);
        Task<bool> TestConnectionAsync(CaptureTarget target);
    }

    public interface IResponseService
    {
        event EventHandler<PlaybookProgressEventArgs> ProgressChanged;
        Task<List<ResponsePlaybook>> GetAvailablePlaybooksAsync();
        Task<ResponsePlaybook> GetPlaybookByIdAsync(string id);
        Task<bool> SavePlaybookAsync(ResponsePlaybook playbook);
        Task<bool> DeletePlaybookAsync(string id);
        Task<bool> ExecutePlaybookAsync(ResponsePlaybook playbook, PlaybookExecutionConfig config);
    }
}
