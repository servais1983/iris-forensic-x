using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using IRIS.Core;
using IRIS.Models;
using IRIS.ViewModels;

namespace IRIS.Views
{
    /// <summary>
    /// Page d'analyse forensique
    /// </summary>
    public partial class AnalyzePage : Page
    {
        private readonly AnalyzeViewModel _viewModel;
        private readonly ILogService _logService;

        public AnalyzePage()
        {
            InitializeComponent();
            
            _logService = ServiceLocator.GetService<ILogService>();
            _viewModel = new AnalyzeViewModel(_logService);
            DataContext = _viewModel;
            
            Loaded += AnalyzePage_Loaded;
            Unloaded += AnalyzePage_Unloaded;
        }

        private void AnalyzePage_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel.Initialize();
            _logService.LogInfo("Page d'analyse chargée");
        }

        private void AnalyzePage_Unloaded(object sender, RoutedEventArgs e)
        {
            _viewModel.Cleanup();
            _logService.LogInfo("Page d'analyse déchargée");
        }

        private void AnalysisTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.SelectedItem is AnalysisType selectedType)
            {
                _viewModel.SelectedAnalysisType = selectedType;
            }
        }

        private void StartAnalysisButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.StartAnalysisCommand.Execute(null);
        }

        private void CancelAnalysisButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.CancelAnalysisCommand.Execute(null);
        }

        private void ViewResultsButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is AnalysisResult result)
            {
                _viewModel.ViewResultCommand.Execute(result);
            }
        }

        private void ExportResultsButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is AnalysisResult result)
            {
                _viewModel.ExportResultCommand.Execute(result);
            }
        }

        private void DeleteResultButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is AnalysisResult result)
            {
                _viewModel.DeleteResultCommand.Execute(result);
            }
        }

        private void AddEvidenceButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.AddEvidenceCommand.Execute(null);
        }

        private void RemoveEvidenceButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is EvidenceItem evidence)
            {
                _viewModel.RemoveEvidenceCommand.Execute(evidence);
            }
        }

        private void AIAssistToggle_Checked(object sender, RoutedEventArgs e)
        {
            _viewModel.UseAI = true;
        }

        private void AIAssistToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            _viewModel.UseAI = false;
        }

        private void AdvancedOptionsExpander_Expanded(object sender, RoutedEventArgs e)
        {
            _viewModel.ShowAdvancedOptions = true;
        }

        private void AdvancedOptionsExpander_Collapsed(object sender, RoutedEventArgs e)
        {
            _viewModel.ShowAdvancedOptions = false;
        }
    }
}
