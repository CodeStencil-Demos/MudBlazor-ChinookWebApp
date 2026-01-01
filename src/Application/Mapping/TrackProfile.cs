namespace Application.Mapping
{
    public class TrackProfile : Profile
    {

        public TrackProfile() 
        {
            // Track -> TrackDto
            CreateMap<Track, TrackDto>();
            
            // TrackDto -> Track
            CreateMap<TrackDto, Track>()
                .ForMember(d => d.InvoiceLines, o => o.Ignore())
                ;

        }    
    }
}
