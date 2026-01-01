namespace Application.Mapping
{
    public class AlbumViewProfile : Profile
    {
        public AlbumViewProfile()
        {
            CreateMap<AlbumView, AlbumViewDto>().ReverseMap();
        }
    }
}
