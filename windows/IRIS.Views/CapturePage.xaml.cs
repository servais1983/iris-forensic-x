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
    /// Page de capture de preuves numériques
    /// </summary>
    public partial class CapturePage : Page
    {
        private readonly CaptureViewModel _viewModel;
        private readonly ILogService _logService;

        public CapturePage()
        {
            InitializeComponent();
            
            _logService = ServiceLocator.GetService<ILogService>();
            _viewModel = new CaptureViewModel(_logService);
            DataContext = _viewModel;
            
            Loaded += CapturePage_Loaded;
            Unloaded += CapturePage_Unloaded;
        }

        private void CapturePage_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel.Initialize();
            _logService.LogInfo("Page de capture chargée");
        }

        private void CapturePage_Unloaded(object sender, RoutedEventArgs e)
        {
            _viewModel.Cleanup();
            _logService.LogInfo("Page de capture déchargée");
        }

        private void CaptureTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.SelectedItem is CaptureType selectedType)
            {
                _viewModel.SelectedCaptureType = selectedType;
            }
        }

        private void StartCaptureButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.StartCaptureCommand.Execute(null);
        }

        private void CancelCaptureButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.CancelCaptureCommand.Execute(null);
        }

        private void ViewCaptureButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is CaptureResult result)
            {
                _viewModel.ViewCaptureCommand.Execute(result);
            }
        }

        private void ExportCaptureButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is CaptureResult result)
            {
                _viewModel.ExportCaptureCommand.Execute(result);
            }
        }

        private void DeleteCaptureButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is CaptureResult result)
            {
                _viewModel.DeleteCaptureCommand.Execute(result);
            }
        }

        private void AddTargetButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.AddTargetCommand.Execute(null);
        }

        private void RemoveTargetButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is CaptureTarget target)
            {
                _viewModel.RemoveTargetCommand.Execute(target);
            }
        }

        private void SmartCaptureToggle_Checked(object sender, RoutedEventArgs e)
        {
            _viewModel.UseSmartCapture = true;
        }

        private void SmartCaptureToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            _viewModel.UseSmartCapture = false;
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
