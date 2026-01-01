namespace Application.Mapping
{
    public class EmployeeProfile : Profile
    {

        public EmployeeProfile() 
        {
            // Employee -> EmployeeDto
            CreateMap<Employee, EmployeeDto>();
            
            // EmployeeDto -> Employee
            CreateMap<EmployeeDto, Employee>()
                .ForMember(d => d.Customers, o => o.Ignore())
                .ForMember(d => d.ReportsToNavigation, o => o.Ignore())
                .ForMember(d => d.InverseReportsToNavigation, o => o.Ignore())
                ;

        }    
    }
}
