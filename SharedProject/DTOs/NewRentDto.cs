using SharedProject.DbModels;

namespace SharedProject.DTOs
{
    public class NewRentDto
    {
        public string Comment { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public List<Game> Games { get; set; }
    }
}
