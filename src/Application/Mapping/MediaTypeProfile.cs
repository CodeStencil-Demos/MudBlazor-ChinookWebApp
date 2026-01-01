namespace Application.Mapping
{
    public class MediaTypeProfile : Profile
    {

        public MediaTypeProfile() 
        {
            CreateMap<MediaType, MediaTypeDto>().ReverseMap();

        }    
    }
}
