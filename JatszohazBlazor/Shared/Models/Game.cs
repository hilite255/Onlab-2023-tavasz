using System.ComponentModel.DataAnnotations.Schema;

namespace JatszohazBlazor.Shared.Models
{
    public class Game
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string? SubName { get; set; }
        public int PlayersFrom { get; set; }
        public int PlayersTo { get; set; }
        public int PlaytimeFrom { get; set; }
        public int PlaytimeTo { get; set; }
        public Game? ParentGame { get; set; }
    }
}
