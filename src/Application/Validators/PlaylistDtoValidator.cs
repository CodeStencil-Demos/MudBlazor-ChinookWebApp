namespace Application.Validators
{
    public class PlaylistDtoValidator : AbstractValidator<PlaylistDto>
    {
        public PlaylistDtoValidator()
        {

        }
    }
}

/* Example:

        public PlaylistDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(160);

            RuleFor(x => x.ArtistId)
                .GreaterThan(0);
        }
*/
