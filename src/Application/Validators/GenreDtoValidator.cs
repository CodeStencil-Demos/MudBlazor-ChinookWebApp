namespace Application.Validators
{
    public class GenreDtoValidator : AbstractValidator<GenreDto>
    {
        public GenreDtoValidator()
        {

        }
    }
}

/* Example:

        public GenreDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(160);

            RuleFor(x => x.ArtistId)
                .GreaterThan(0);
        }
*/
