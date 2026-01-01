namespace Application.Mapping
{
    public class LoginProfile : Profile
    {
        public LoginProfile()
        {
            // Map LoginDto to ApplicationUser if needed
            CreateMap<LoginDto, ApplicationUser>()
                .ForMember(dest => dest.FirstName,
                    opt => opt.MapFrom(src => src.Username));

            // You can add additional mapping configurations if required
        }
    }
}
