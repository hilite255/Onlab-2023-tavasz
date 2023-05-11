using SharedProject.DbModels;
using Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class RentService
    {
        private readonly IRentRepo rentRepo;

        public RentService(IRentRepo rentRepo)
        {
            this.rentRepo = rentRepo;
        }
        
        public async Task<List<Game>> GetGamesInTimespan(DateTime from, DateTime to)
        {
            return await rentRepo.GetGamesInTimespan(from, to);
        }

        public async Task<Rent> CreateNewRent(string userId, DateTime from, DateTime to, List<Game> games)
        {
            var rent = await rentRepo.CreateNewRent(from, to, Rent.RentStatus.Pending, userId);
            foreach (var g in games)
            {
                await rentRepo.AddGameToRent(rent, g);
            }
            return rent;
        }

        public async Task<RentComment> AddCommentToRent(string userId, string comment, Rent rent)
        {
            return await rentRepo.AddCommentToRent(userId, comment, rent);
        }

        public async Task<List<Rent>> ListRents()
        {
            var rents = await rentRepo.List();
            return rents;
        }

        public async Task<List<Rent>> ListRentsPaged(string status, int page)
        {
            var rents = await rentRepo.ListPaged(status, page);
            return rents;
        }

        public List<Game> GetGamesForRent(Rent r)
        {
            if (r == null)
                return null;
            return rentRepo.GetGamesForRent(r);
        }

        public async Task<Rent> GetRentById(int id)
        {
            return await rentRepo.GetRentById(id);
        }

        public async Task<List<RentComment>> GetCommentsForRent(Rent r)
        {
            return await rentRepo.GetCommentsForRent(r);
        }

        // sok if helyett jobb megoldás kéne de nincs ötletem
        public async void ChangeStatusOfRent(string status, Rent rent)
        {
            // ha ugyan arra módosítanánk akkor nem kell semmit csinálni
            if(status == rent.Status.ToString())
            {
                return;
            }

            // cancelled-re bármikor át lehet állítani kivéve ha már ki van adva vagy vissza van hozva
            if(status == Rent.RentStatus.Declined.ToString() &&
                rent.Status != Rent.RentStatus.Back &&
                rent.Status != Rent.RentStatus.InMyRoom &&
                rent.Status != Rent.RentStatus.GaveOut)
            {
                rentRepo.ChangeStatusOfRent(status, rent);
                return;
            }

            // jelenlegi státusz alapján mire lehet módosítani
            if (rent.Status == Rent.RentStatus.Pending &&
                status == Rent.RentStatus.Approved.ToString())
            {
                rentRepo.ChangeStatusOfRent(status, rent);
                return;
            }
            if(rent.Status == Rent.RentStatus.Approved &&
                status == Rent.RentStatus.GaveOut.ToString())
            {
                rentRepo.ChangeStatusOfRent(status, rent);
                return;
            }
            if(rent.Status == Rent.RentStatus.GaveOut &&
                (status == Rent.RentStatus.Back.ToString() || status == Rent.RentStatus.InMyRoom.ToString()))
            {
                rentRepo.ChangeStatusOfRent(status, rent);
                return;
            }
            if(rent.Status == Rent.RentStatus.InMyRoom &&
                (status == Rent.RentStatus.GaveOut.ToString() || status == Rent.RentStatus.Back.ToString()))
            {
                rentRepo.ChangeStatusOfRent(status, rent);
                return;
            }
            if(rent.Status == Rent.RentStatus.Back &&
                status == Rent.RentStatus.GaveOut.ToString())
            {
                rentRepo.ChangeStatusOfRent(status, rent);
                return;
            }
            if(rent.Status == Rent.RentStatus.Declined &&
                status == Rent.RentStatus.Approved.ToString())
            {
                rentRepo.ChangeStatusOfRent(status, rent);
                return;
            }
        }
    }
}
