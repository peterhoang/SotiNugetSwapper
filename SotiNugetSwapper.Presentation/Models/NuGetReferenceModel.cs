using System;
using System.Security.Policy;
using SotiNugetSwapper.Presentation.Enums;
using SotiNugetSwapper.Presentation.ViewModels;
using VSLangProj;

namespace SotiNugetSwapper.Presentation.Models
{
    public class NuGetReferenceModel : IEquatable<NuGetReferenceModel>
    {
        public string Name { get; set; }

        public string Path { get; set; }

        public string ProjectName { get; set; }

        public StatusEnum Status { get; set; }

        public bool IsReplaced { get; set; }

        public NuGetReferenceModel() { }

        public NuGetReferenceModel(Reference reference)
        {
            Name = reference.Name;
            Path = reference.Path;
            Status = StatusEnum.DoNotReplace;
            ProjectName = reference.SourceProject?.Name;
        }

        public bool Equals(NuGetReferenceModel other)
        {
            return this.Name == other.Name;
        }
    }
}
