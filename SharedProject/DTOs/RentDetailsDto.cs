using SharedProject.DbModels;

namespace SharedProject.DTOs
{
    public class RentDetailsDto
    {
        public int Id { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string Status { get; set; }
        public User Renter { get; set; }
        public List<Game> Games { get; set; }
        public List<RentComment> Comments { get; set; }
    }
}
