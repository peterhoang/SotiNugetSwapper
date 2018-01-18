using System.Collections.Generic;
using System.Collections.ObjectModel;
using SotiNugetSwapper.Presentation.Enums;
using SotiNugetSwapper.Presentation.Models;

namespace SotiNugetSwapper.Presentation.ViewModels
{
    public class MockViewModel
    {
        public ObservableCollection<NuGetReferenceModel> NuGetReferences { get; set; }
        
        public NuGetReferenceModel SelectedNuGetReference { get; set; }

        public MockViewModel()
        {
            NuGetReferences = new ObservableCollection<NuGetReferenceModel>();

            for (var i = 0; i < 10; i++)
            {
                var mock = new NuGetReferenceModel
                {
                    Name = "mock nuget name " + i,
                    Path = "mock nuget path " + i,
                    Status = StatusEnum.Replace
                };
                NuGetReferences.Add(mock);
            }
        }
    }
}
