using SharedProject.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SharedProject.DbModels.Rent;

namespace Domain.RepositoryInterfaces
{
    public interface IRentRepo
    {
        public Task<List<Game>> GetGamesInTimespan(DateTime from, DateTime to);
        public Task<Rent> CreateNewRent(DateTime from, DateTime to, RentStatus status, string renterId);
        public Task<bool> AddGameToRent(Rent rent, Game game);
        public Task<RentComment> AddCommentToRent(string userId, string comment, Rent rent);
        public Task<List<Rent>> List();
        public Task<List<Rent>> ListPaged(string status, int page);
        public List<Game> GetGamesForRent(Rent r);
        public Task<Rent> GetRentById(int id);
        public Task<List<RentComment>> GetCommentsForRent(Rent r);
        public void ChangeStatusOfRent(string status, Rent rent);
    }
}
