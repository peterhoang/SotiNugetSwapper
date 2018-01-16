using System;
using VSLangProj;

namespace SotiNugetSwapper.Presentation.Models
{
    public class NuGetReferenceModel : IEquatable<NuGetReferenceModel>
    {
        public string Name { get; set; }

        public string Path { get; set; }

        public string ProjectName { get; set; }

        public NuGetReferenceModel(Reference reference)
        {
            Name = reference.Name;
            Path = reference.Path;
            ProjectName = reference.SourceProject?.Name;
        }

        public bool Equals(NuGetReferenceModel other)
        {
            return this.Name == other.Name;
        }
    }
}
