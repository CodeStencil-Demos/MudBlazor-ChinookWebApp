namespace Application.Mapping
{
    public class AuthResultProfile : Profile
    {
        public AuthResultProfile()
        {
            CreateMap<AuthResult, AuthResponseDto>()
                .ForMember(dest => dest.IsAuthenticated, opt => opt.MapFrom(src => src.Success))
                .ForMember(dest => dest.Token, opt => opt.MapFrom(src => src.Token))
                .ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.Message));
        }
    }
}
