using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    [Table("RentComments")]
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
