namespace Application.Validators
{
    public class MediaTypeDtoValidator : AbstractValidator<MediaTypeDto>
    {
        public MediaTypeDtoValidator()
        {

        }
    }
}

/* Example:

        public MediaTypeDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(160);

            RuleFor(x => x.ArtistId)
                .GreaterThan(0);
        }
*/
