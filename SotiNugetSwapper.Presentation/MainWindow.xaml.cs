using System;
using EnvDTE;
using SotiNugetSwapper.Presentation.ViewModels;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using SotiNugetSwapper.Presentation.Models;

namespace SotiNugetSwapper.Presentation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        public MainWindow(DTE application)
        {
            InitializeComponent();
            ViewModel.Application = application;
            ViewModel.Initialize();
        }

        /// <summary>Gets the view model. </summary>
        public MainViewModel ViewModel => (MainViewModel)Resources["ViewModel"];

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var model = ((ComboBox)sender).Tag as NuGetReferenceModel;
            if (model != null)
            {
                var status = model.Status;
            }
        }
    }
}
