namespace Application.Mapping
{
    public class InvoiceLineProfile : Profile
    {

        public InvoiceLineProfile() 
        {
            CreateMap<InvoiceLine, InvoiceLineDto>();

        }    
    }
}
