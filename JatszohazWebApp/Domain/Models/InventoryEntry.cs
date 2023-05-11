using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    [Table("InventoryEntries")]
    public class InventoryEntry
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string MissingPieces { get; set; }
        public string Message { get; set; }
        [ForeignKey("GameId")]
        public Game Game { get; set; }
    }
}
