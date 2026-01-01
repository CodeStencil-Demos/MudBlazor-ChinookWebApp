namespace Application.Validators
{
    public class InvoiceLineDtoValidator : AbstractValidator<InvoiceLineDto>
    {
        public InvoiceLineDtoValidator()
        {

        }
    }
}

/* Example:

        public InvoiceLineDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(160);

            RuleFor(x => x.ArtistId)
                .GreaterThan(0);
        }
*/
