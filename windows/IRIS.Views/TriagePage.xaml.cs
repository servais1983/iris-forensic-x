using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IRIS.Models;
using IRIS.ViewModels;
using IRIS.Core;

namespace IRIS.Views
{
    /// <summary>
    /// Logique d'interaction pour TriagePage.xaml
    /// </summary>
    public partial class TriagePage : Page
    {
        private readonly ILogService _logService;
        private TriageViewModel _viewModel;

        public TriagePage()
        {
            InitializeComponent();
            
            _logService = ServiceLocator.GetService<ILogService>();
            _viewModel = new TriageViewModel(_logService);
            
            DataContext = _viewModel;
            
            Loaded += TriagePage_Loaded;
            Unloaded += TriagePage_Unloaded;
            
            _logService.LogInfo("Page de triage initialisée");
        }

        private void TriagePage_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel.Initialize();
        }

        private void TriagePage_Unloaded(object sender, RoutedEventArgs e)
        {
            _viewModel.Cleanup();
        }

        #region Gestionnaires d'événements

        private void MITREFilterCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox && checkBox.Tag is string technique)
            {
                _viewModel.AddMITRETechnique(technique);
            }
        }

        private void MITREFilterCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox && checkBox.Tag is string technique)
            {
                _viewModel.RemoveMITRETechnique(technique);
            }
        }

        private void AIAssistToggle_Checked(object sender, RoutedEventArgs e)
        {
            _logService.LogInfo("Assistance IA activée");
        }

        private void AIAssistToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            _logService.LogInfo("Assistance IA désactivée");
        }

        private void ApplyFilterButton_Click(object sender, RoutedEventArgs e)
        {
            // Logique supplémentaire si nécessaire
        }

        private void ResetFilterButton_Click(object sender, RoutedEventArgs e)
        {
            // Réinitialisation des CheckBox MITRE
            foreach (var child in FindVisualChildren<CheckBox>(this))
            {
                if (child.Tag is string && ((string)child.Tag).StartsWith("T"))
                {
                    child.IsChecked = false;
                }
            }
        }

        private void StartTriageButton_Click(object sender, RoutedEventArgs e)
        {
            // Logique supplémentaire si nécessaire
        }

        private void CancelTriageButton_Click(object sender, RoutedEventArgs e)
        {
            // Logique supplémentaire si nécessaire
        }

        private void LoadTriageButton_Click(object sender, RoutedEventArgs e)
        {
            // Logique supplémentaire si nécessaire
        }

        private void SaveTriageButton_Click(object sender, RoutedEventArgs e)
        {
            // Logique supplémentaire si nécessaire
        }

        private void ViewEvidenceButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is EvidenceItem evidence)
            {
                _viewModel.ViewEvidenceCommand.Execute(evidence);
            }
        }

        private void ExcludeEvidenceButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is EvidenceItem evidence)
            {
                _viewModel.ExcludeEvidenceCommand.Execute(evidence);
            }
        }

        private void IncludeEvidenceButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is EvidenceItem evidence)
            {
                _viewModel.IncludeEvidenceCommand.Execute(evidence);
            }
        }

        private void PrioritySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (sender is Slider slider && slider.Tag is EvidenceItem evidence)
            {
                _viewModel.UpdatePriority(evidence, (int)e.NewValue);
            }
        }

        #endregion

        #region Méthodes utilitaires

        /// <summary>
        /// Trouve tous les enfants visuels d'un type spécifique dans l'arbre visuel
        /// </summary>
        private static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        #endregion
    }
}
