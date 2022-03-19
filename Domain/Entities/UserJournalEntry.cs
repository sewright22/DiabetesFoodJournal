namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;

    public class UserJournalEntry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public virtual JournalEntry JournalEntry { get; set; }
        public int JournalEntryId { get; set; }
        public virtual User User { get; set; }
        public int UserId { get; set; }
    }
}