using System.ComponentModel.DataAnnotations.Schema;

namespace JatszohazBlazor.Shared.Models
{
    public class RentComment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
        [ForeignKey("RentId")]
        public Rent Rent { get; set; }
        [ForeignKey("UserId")]
        public User? Creator { get; set; }
    }
}
