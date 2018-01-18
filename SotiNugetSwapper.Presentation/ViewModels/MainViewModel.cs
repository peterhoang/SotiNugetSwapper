using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml;
using EnvDTE;
using SotiNugetSwapper.Presentation.Enums;
using SotiNugetSwapper.Presentation.Models;
using VSLangProj;

namespace SotiNugetSwapper.Presentation.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged {

        public ObservableCollection<NuGetReferenceModel> NuGetReferences { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public DTE Application { get; set; }

        public FileInfo SolutionFile { get; set; }
        
        public NuGetReferenceModel SelectedNuGetReference { get; set; }

        public MainViewModel()
        {
        }

        public void Initialize()
        {
            SolutionFile = new FileInfo(Application.Solution.FileName);

            LoadReferences();
        }

        private void LoadReferences()
        {
            var packageDirs = new List<string> { "/packages/", "\\packages\\" };
            var nugetReferences = new Collection<NuGetReferenceModel>();

            if (SolutionFile.Exists)
            {
                var nuGetRepositoryPath = GetNuGetRepositoryPath(SolutionFile.Directory);
                if (!string.IsNullOrEmpty(nuGetRepositoryPath))
                {
                    packageDirs.Add(nuGetRepositoryPath);
                }
            }

            // Scan all projects and get their nuget references
            foreach (var project in Application.Solution.Projects.OfType<Project>())
            {
                var vsproject = (VSProject)project.Object;

                foreach (Reference vsReference in vsproject.References)
                {
                    var reference = new NuGetReferenceModel(vsReference);
                    var vsReferencePath = vsReference.Path.ToLower();
                    foreach (var packageDir in packageDirs)
                    {
                        if (vsReferencePath.Contains(packageDir))
                        {
                            nugetReferences.Add(reference);
                        }
                    }
                }
            }

            NuGetReferences = new ObservableCollection<NuGetReferenceModel>(nugetReferences.Distinct());
            NotifyPropertyChanged("NuGetReferences");
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

                    if (nuGetConfigFile.DirectoryName != null)
                    {
                        var repositoryDirectory = new DirectoryInfo(System.IO.Path.Combine(nuGetConfigFile.DirectoryName, repositoryPath));
                        if (repositoryDirectory.Exists)
                        {
                            return repositoryDirectory.FullName.ToLower();
                        }
                    }
                }
            }

            if (dir.Parent != null)
            {
                return GetNuGetRepositoryPath(dir.Parent);
            }

            return null;
        }

        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
