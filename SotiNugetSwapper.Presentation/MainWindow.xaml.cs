using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Xml;
using EnvDTE;
using SotiNugetSwapper.Presentation.ViewModels;
using VSLangProj;
using Window = System.Windows.Window;

namespace SotiNugetSwapper.Presentation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(DTE application)
        {
            InitializeComponent();
            Model.Application = application;
            Model.Initialize();
        }

        /// <summary>Gets the view model. </summary>
        public MainViewModel Model
        {
            get { return (MainViewModel)Resources["ViewModel"]; }
        }
    }
}
