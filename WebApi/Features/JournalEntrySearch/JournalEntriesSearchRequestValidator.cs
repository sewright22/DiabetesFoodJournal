namespace WebApi.Features
{
    using FastEndpoints.Validation;

    public class JournalEntriesSearchRequestValidator : Validator<JournalEntriesSearchRequest>
    {
        public JournalEntriesSearchRequestValidator()
        {
            this.RuleFor(x => x.SearchValue)
                .NotEmpty().WithMessage("search value is required.");
        }
    }
}