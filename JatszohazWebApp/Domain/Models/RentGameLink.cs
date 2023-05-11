using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    [Table("RentGameLinks")]
    public class RentGameLink
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [ForeignKey("GameId")]
        public Game Game { get; set; }
        [ForeignKey("RentId")]
        public Rent Rent { get; set; }
    }
}
