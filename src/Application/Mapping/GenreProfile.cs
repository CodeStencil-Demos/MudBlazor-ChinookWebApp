namespace Application.Mapping
{
    public class GenreProfile : Profile
    {

        public GenreProfile() 
        {
            CreateMap<Genre, GenreDto>().ReverseMap();

        }    
    }
}
