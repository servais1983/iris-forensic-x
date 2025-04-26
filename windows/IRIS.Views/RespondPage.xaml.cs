using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using IRIS.Models;
using IRIS.ViewModels;
using IRIS.Core;
using IRIS.Services;

namespace IRIS.Views
{
    /// <summary>
    /// Logique d'interaction pour RespondPage.xaml
    /// </summary>
    public partial class RespondPage : Page
    {
        private readonly ILogService _logService;
        private RespondViewModel _viewModel;

        public RespondPage()
        {
            InitializeComponent();
            
            _logService = ServiceLocator.GetService<ILogService>();
            _viewModel = new RespondViewModel(_logService);
            
            DataContext = _viewModel;
            
            Loaded += RespondPage_Loaded;
            Unloaded += RespondPage_Unloaded;
            
            _logService.LogInfo("Page de réponse initialisée");
        }

        private void RespondPage_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel.Initialize();
        }

        private void RespondPage_Unloaded(object sender, RoutedEventArgs e)
        {
            _viewModel.Cleanup();
        }

        private void ExecutePlaybookButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is ResponsePlaybook playbook)
            {
                _viewModel.ExecutePlaybookCommand.Execute(playbook);
            }
        }

        private void CancelPlaybookButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is ResponsePlaybook playbook)
            {
                _viewModel.CancelPlaybookCommand.Execute(playbook);
            }
        }

        private void ViewPlaybookDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is ResponsePlaybook playbook)
            {
                _viewModel.ViewPlaybookDetailsCommand.Execute(playbook);
            }
        }

        private void CreatePlaybookButton_Click(object sender, RoutedEventArgs e)
        {
            // Logique supplémentaire si nécessaire
        }

        private void ImportPlaybookButton_Click(object sender, RoutedEventArgs e)
        {
            // Logique supplémentaire si nécessaire
        }

        private void ExportPlaybookButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is ResponsePlaybook playbook)
            {
                _viewModel.ExportPlaybookCommand.Execute(playbook);
            }
        }

        private void EditPlaybookButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is ResponsePlaybook playbook)
            {
                _viewModel.EditPlaybookCommand.Execute(playbook);
            }
        }

        private void DeletePlaybookButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is ResponsePlaybook playbook)
            {
                _viewModel.DeletePlaybookCommand.Execute(playbook);
            }
        }

        private void GenerateReportButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is ResponseAction action)
            {
                _viewModel.GenerateReportCommand.Execute(action);
            }
        }

        private void ViewActionDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is ResponseAction action)
            {
                _viewModel.ViewActionDetailsCommand.Execute(action);
            }
        }

        private void AIAssistToggle_Checked(object sender, RoutedEventArgs e)
        {
            _logService.LogInfo("Assistance IA activée pour la réponse");
        }

        private void AIAssistToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            _logService.LogInfo("Assistance IA désactivée pour la réponse");
        }

        private void BlockchainVerifyToggle_Checked(object sender, RoutedEventArgs e)
        {
            _logService.LogInfo("Vérification blockchain activée pour la réponse");
        }

        private void BlockchainVerifyToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            _logService.LogInfo("Vérification blockchain désactivée pour la réponse");
        }
    }
}
