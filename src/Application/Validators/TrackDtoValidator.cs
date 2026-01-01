namespace Application.Validators
{
    public class TrackDtoValidator : AbstractValidator<TrackDto>
    {
        public TrackDtoValidator()
        {

        }
    }
}

/* Example:

        public TrackDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(160);

            RuleFor(x => x.ArtistId)
                .GreaterThan(0);
        }
*/
