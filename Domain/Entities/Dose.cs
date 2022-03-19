using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Dose
    {
        public int Extended { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public decimal InsulinAmount { get; set; }
        public decimal TimeExtended { get; set; }
        public int TimeOffset { get; set; }
        public int UpFront { get; set; }
    }
}