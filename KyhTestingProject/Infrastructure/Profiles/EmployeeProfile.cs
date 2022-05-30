using AutoMapper;
using KyhTestingProject.Data;
using KyhTestingProject.ViewModels;

namespace KyhTestingProject.Infrastructure.Profiles;

public class EmployeeProfile : Profile
{
    public EmployeeProfile()
    {
        CreateMap<Employee, EmployeeIndexViewModel.EmployeeItem>()
            .ForMember(e => e.Name, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName));
    }
}