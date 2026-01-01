namespace Application.Validators
{
    public class CustomerDtoValidator : AbstractValidator<CustomerDto>
    {
        public CustomerDtoValidator()
        {

        }
    }
}

/* Example:

        public CustomerDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(160);

            RuleFor(x => x.ArtistId)
                .GreaterThan(0);
        }
*/
