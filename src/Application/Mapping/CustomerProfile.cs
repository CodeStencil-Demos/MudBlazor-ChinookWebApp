namespace Application.Mapping
{
    public class CustomerProfile : Profile
    {

        public CustomerProfile() 
        {
            // Customer -> CustomerDto
            CreateMap<Customer, CustomerDto>();
            
            // CustomerDto -> Customer
            CreateMap<CustomerDto, Customer>()
                .ForMember(d => d.Invoices, o => o.Ignore())
                ;

        }    
    }
}
