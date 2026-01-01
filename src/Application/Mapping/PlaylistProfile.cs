namespace Application.Mapping
{
    public class PlaylistProfile : Profile
    {

        public PlaylistProfile() 
        {
            CreateMap<Playlist, PlaylistDto>().ReverseMap();

        }    
    }
}
