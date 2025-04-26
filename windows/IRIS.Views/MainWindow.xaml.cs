using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using IRIS.Models;
using IRIS.ViewModels;

namespace IRIS.Views
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new MainViewModel();
            DataContext = _viewModel;
        }

        private void NavigationButton_Click(object sender, RoutedEventArgs e)
        {
            // Réinitialiser tous les boutons
            foreach (var button in NavigationPanel.Children)
            {
                if (button is Button navButton)
                {
                    navButton.Background = new SolidColorBrush(Colors.Transparent);
                    navButton.Foreground = new SolidColorBrush(Colors.White);
                }
            }

            // Mettre en évidence le bouton sélectionné
            if (sender is Button selectedButton)
            {
                selectedButton.Background = new SolidColorBrush(Color.FromRgb(52, 152, 219));
                selectedButton.Foreground = new SolidColorBrush(Colors.White);
            }

            // Naviguer vers la page correspondante
            string pageName = (sender as Button)?.Tag?.ToString();
            if (!string.IsNullOrEmpty(pageName))
            {
                _viewModel.NavigateToPage(pageName);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Sélectionner le bouton Dashboard par défaut
            DashboardButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }
    }
}
