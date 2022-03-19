using FastEndpoints;
using Infrastructure.Persistence;

namespace WebApi.Features
{
    public class JournalEntriesEndpoint : Endpoint<JournalEntriesSearchRequest>
    {
        private FoodJournalContext context;

        public JournalEntriesEndpoint(FoodJournalContext dbContext)
        {
            this.context = dbContext;
        }

        public override void Configure()
        {
            Verbs(Http.GET);
            Routes("api/journalentries");
        }

        public override async Task HandleAsync(JournalEntriesSearchRequest req, CancellationToken ct)
        {
            int userId = req.UserId;
            string upperSearchValue = req.SearchValue.ToUpper();
            var user = this.context.Users.FirstOrDefault();
            await SendAsync(new
            {
                Id = 1,
                UserId = user.Id,
                EntryName = req.SearchValue,
            });
            //var retVal = new List<JournalEntryModel>();
            //try
            //{
            //    var results = this.context.UserJournalEntries.Where(u => u.UserId == userId).Select(uje => uje.JournalEntry).OrderByDescending(j => j.Logged).AsQueryable();

            //    if (!string.IsNullOrEmpty(upperSearchValue))
            //    {
            //        results = results.Where(entry => entry.Title.ToUpper().Contains(upperSearchValue) ||
            //                                                    entry.JournalEntryTags.Where(t => t.Tag.Description.ToUpper().Contains(upperSearchValue)).Any());
            //    }

            //    if (results.Any())
            //    {
            //        foreach (var result in await results.Take(10).ToListAsync())
            //        {
            //            var currentEntry = this.journalEntryModelFactory.Build(result);

            //            retVal.Add(currentEntry);
            //        }
            //    }
            //}
        }
    }
}