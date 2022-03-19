namespace WebApi.Features
{
    using FastEndpoints;

    public class JournalEntriesSearchRequest
    {
        public string SearchValue { get; set; } = string.Empty;

        [FromClaim]
        public int UserId { get; set; }
    }
}