namespace Application.Mapping
{
    public class InvoiceProfile : Profile
    {

        public InvoiceProfile() 
        {
            // Invoice -> InvoiceDto
            CreateMap<Invoice, InvoiceDto>();
            
            // InvoiceDto -> Invoice
            CreateMap<InvoiceDto, Invoice>()
                .ForMember(d => d.InvoiceLines, o => o.Ignore())
                ;

        }    
    }
}
