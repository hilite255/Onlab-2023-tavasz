using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    [Table("Games")]
    public class Game
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Name { get; set; }
        public string? SubName { get; set; }
        public int PlayersFrom { get; set; }
        public int PlayersTo { get; set; }
        public int PlaytimeFrom { get; set; }
        public int PlaytimeTo { get; set; }
        [ForeignKey("ParentID")]
        public Game? ParentGame { get; set; }
    }
}
