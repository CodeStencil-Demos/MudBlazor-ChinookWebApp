namespace Application.Mapping
{
    public class ArtistProfile : Profile
    {

        public ArtistProfile() 
        {
            CreateMap<Artist, ArtistDto>().ReverseMap();

        }    
    }
}
