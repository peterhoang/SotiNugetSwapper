using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Xml;
using EnvDTE;
using VSLangProj;
using Window = System.Windows.Window;

namespace SotiNugetSwapper.Presentation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly DTE _app;

        public MainWindow(DTE application)
        {
            InitializeComponent();

            _app = application;

            LoadReferences();
        }

        private void LoadReferences()
        {
            var packageDirs = new List<string>();
            packageDirs.Add("/packages/");
            packageDirs.Add("\\packages\\");

            var solutionfile = new FileInfo(_app.Solution.FileName);

            if (solutionfile.Exists)
            {
                var nuGetRepositoryPath = GetNuGetRepositoryPath(solutionfile.Directory);
                if (!string.IsNullOrEmpty(nuGetRepositoryPath))
                {
                    packageDirs.Add(nuGetRepositoryPath);
                }
            }

            foreach (Project project in _app.Solution.Projects.OfType<Project>())
            {
                var vsproject = (VSProject)project.Object;

                var item1 = new TreeViewItem { Header = project.Name + " NuGet References Path" };
                listBox.Items.Add(item1);

                foreach (Reference vsReference in vsproject.References)
                {
                    var vsReferencePath = vsReference.Path.ToLower();
                    foreach (string packageDir in packageDirs)
                    {
                        if (vsReferencePath.Contains(packageDir))
                        {
                            var item = new ListBoxItem
                            {
                                Content = vsReferencePath
                            };
                            listBox.Items.Add(item);
                        }
                    }
                }
            }
        }

        private string GetNuGetRepositoryPath(DirectoryInfo dir)
        {
            var nuGetConfigFile = dir.GetFiles("NuGet.Config").FirstOrDefault();

            if (nuGetConfigFile != null)
            {
                var nuGetConfig = new XmlDocument();
                nuGetConfig.Load(nuGetConfigFile.FullName);
                var pathNode = nuGetConfig.SelectSingleNode("//config//add[contains(@key,'repositoryPath')]");

                if (pathNode?.Attributes?["value"] != null)
                {
                    var repositoryPath = pathNode.Attributes["value"].Value;
                    repositoryPath = Environment.ExpandEnvironmentVariables(repositoryPath);

                    if (System.IO.Path.IsPathRooted(repositoryPath))
                    {
                        return repositoryPath;
                    }

                    var repositoryDirectory = new DirectoryInfo(System.IO.Path.Combine(nuGetConfigFile.DirectoryName, repositoryPath));
                    if (repositoryDirectory.Exists)
                    {
                        return repositoryDirectory.FullName.ToLower();
                    }
                }
            }

            if (dir.Parent != null)
            {
                return GetNuGetRepositoryPath(dir.Parent);
            }

            return null;
        }
    }
}
