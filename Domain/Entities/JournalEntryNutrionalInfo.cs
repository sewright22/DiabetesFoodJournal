using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;

    public class JournalEntryNutritionalInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public virtual JournalEntry JournalEntry { get; set; }
        public int JournalEntryId { get; set; }
        public virtual NutritionalInfo NutritionalInfo { get; set; }
        public int NutritionalInfoId { get; set; }
    }
}