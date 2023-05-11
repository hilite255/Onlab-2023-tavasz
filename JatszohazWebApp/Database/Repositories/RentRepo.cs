using SharedProject.DbModels;
using Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Repositories
{
    public class RentRepo : IRentRepo
    {
        private readonly DatabaseContext dbcontext;

        public RentRepo(DatabaseContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public async Task<List<Game>> GetGamesInTimespan(DateTime from, DateTime to)
        {
            var games = dbcontext.Games.Where(
                g => !dbcontext.RentGameLinks.Any(
                    gl => gl.Game == g &&
                    gl.Rent.DateFrom <= to &&
                    gl.Rent.DateTo >= from)
            );
            return await games.ToListAsync();
        }
            //var games = await dbcontext.Games.ToListAsync();
            //var rents = await dbcontext.Rents.ToListAsync();
            //var gameRentLinks = await dbcontext.RentGameLinks.ToListAsync();

            //var probRents = rents.FindAll(rent => TimesIntersect(from, to, rent.DateFrom, rent.DateTo));
            //var unavailableGames = gameRentLinks.FindAll(l => probRents.Contains(l.Rent)).Select(l => l.Game);

            //return games.FindAll(g => !unavailableGames.Contains(g));
        //}

        // returns true if the two timespans intersect
        // túl van bonyolítva, nincs használva
        private bool TimesIntersect(DateTime leftFrom, DateTime leftTo, DateTime rightFrom, DateTime rightTo)
        {
            if(DateTime.Compare(leftFrom, rightFrom) > 0)       // switch the times if right one starts before left one
            {
                var temp = leftFrom;
                leftFrom = rightFrom;
                rightFrom = temp;
                temp = leftTo;
                leftTo = rightTo;
                rightTo = temp;
            }
            if (DateTime.Compare(leftTo, rightFrom) >= 0)
                return true;
            return false;
        }

        public async Task<Rent> CreateNewRent(DateTime from, DateTime to, Rent.RentStatus status, string renterId)
        {
            var renter = await dbcontext.Users.FirstOrDefaultAsync(u => u.Id == renterId);
            if (renter == null)
                throw new ArgumentException("renter doesnt exist");
            var newRent = await dbcontext.Rents.AddAsync(new Rent()
            {
                DateFrom = from,
                DateTo = to,
                Status = status,
                Renter = renter,
            });
            dbcontext.SaveChanges();
            return newRent.Entity;
        }

        public async Task<bool> AddGameToRent(Rent rent, Game game)
        {
            var dbGame = await dbcontext.Games.FirstOrDefaultAsync(g => g.ID == game.ID);
            var newLink = await dbcontext.RentGameLinks.AddAsync(new RentGameLink()
            {
                Rent = rent,
                Game = dbGame
            });
            dbcontext.SaveChanges();
            return true;
        }

        public async Task<RentComment> AddCommentToRent(string userId, string comment, Rent rent)
        {
            var user = await dbcontext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            var dbRent = await dbcontext.Rents.FirstOrDefaultAsync(r => r.ID == rent.ID);
            if (user == null)
                throw new ArgumentException("renter doesnt exist");
            var newComment = await dbcontext.Comments.AddAsync(new RentComment()
            {
                Message = comment,
                Rent = dbRent,
                Creator = user,
                Date = DateTime.Now,
            });
            dbcontext.SaveChanges();
            return newComment.Entity;
        }

        public async Task<List<Rent>> List()
        {
            return await dbcontext.Rents.Include(r => r.Renter).OrderByDescending(r => r.ID).ToListAsync();
        }

        public async Task<List<Rent>> ListPaged(string status, int page)
        {
            if (status != null && status != "All")
            {
                Enum.TryParse(status, out Rent.RentStatus rentStatus);
                return await dbcontext.Rents
                    .Where(r => r.Status == rentStatus)
                    .OrderByDescending(r => r.ID)
                    .Skip((page - 1) * 20)
                    .Take(20)
                    .Include(r => r.Renter)
                    .ToListAsync();
            }
            return await dbcontext.Rents
                .OrderByDescending(r => r.ID)
                .Skip((page - 1) * 20)
                .Take(20)
                .Include(r => r.Renter)
                .ToListAsync();
        }

        public List<Game> GetGamesForRent(Rent r)
        {
            var links = dbcontext.RentGameLinks.Where(x => x.Rent.ID == r.ID).Select(x => x.Game.ID).ToListAsync();
            var games = dbcontext.Games.Where(g => links.Result.Contains(g.ID)).ToListAsync();
            return games.Result;
        }

        public async Task<Rent> GetRentById(int id)
        {
            return await dbcontext.Rents.Include(r => r.Renter).FirstOrDefaultAsync(r => r.ID == id);
        }

        public async Task<List<RentComment>> GetCommentsForRent(Rent r)
        {
            var comments = await dbcontext.Comments.Where(c => c.Rent.ID == r.ID).ToListAsync();
            return comments;
        }

        public async void ChangeStatusOfRent(string status, Rent rent)
        {
            Enum.TryParse(status, out Rent.RentStatus rentStatus);
            rent.Status = rentStatus;
            dbcontext.SaveChanges();
        }
    }
}
