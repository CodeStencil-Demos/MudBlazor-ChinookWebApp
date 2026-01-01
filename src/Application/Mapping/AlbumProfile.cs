namespace Application.Mapping
{
    public class AlbumProfile : Profile
    {

        public AlbumProfile() 
        {
            // Album -> AlbumDto
            CreateMap<Album, AlbumDto>();
            
            // AlbumDto -> Album
            CreateMap<AlbumDto, Album>()
                .ForMember(d => d.Tracks, o => o.Ignore())
                ;

        }    
    }
}
