using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedProject.DbModels;
using Domain.RepositoryInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Database.Repositories
{
    public class GameRepo : IGameRepo
    {
        private readonly string imagePath = "..\\game_images";
        private readonly DatabaseContext dbcontext;

        public GameRepo(DatabaseContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public async Task<ICollection<Game>> List()
        {
            return await dbcontext.Games.ToListAsync();
        }

        public async Task<Game> FindById(int id)
        {
            //var game = await dbcontext.Games.FindAsync(id);
            var games = await dbcontext.Games.ToListAsync();
            var game = games.SingleOrDefault(g => g.ID == id);
            if (game != null)
                return game;
            else
                throw new KeyNotFoundException();
        }

        public async Task<Game> Insert(Game game)
        {
            var currentGame = await dbcontext.Games.SingleOrDefaultAsync(g => g.ID == game.ID);
            if (currentGame != null)
            {
                currentGame.Name = game.Name;
                currentGame.SubName = game.SubName;
                currentGame.PlayersFrom = game.PlayersFrom;
                currentGame.PlayersTo = game.PlayersTo;
                currentGame.PlaytimeFrom = game.PlaytimeFrom;
                currentGame.PlaytimeTo = game.PlaytimeTo;
                currentGame.ParentGame = await dbcontext.Games.SingleOrDefaultAsync(g => game.ParentGame == null ? false : game.ParentGame.ID == g.ID);
                dbcontext.SaveChanges();
                return currentGame;
            }
            var newGame = await dbcontext.Games.AddAsync(new Game()
            {
                Name = game.Name,
                SubName = game.SubName,
                PlayersFrom = game.PlayersFrom,
                PlayersTo = game.PlayersTo,
                PlaytimeFrom = game.PlaytimeFrom,
                PlaytimeTo = game.PlaytimeTo,
                ParentGame = await dbcontext.Games.SingleOrDefaultAsync(g => game.ParentGame == null ? false : game.ParentGame.ID == g.ID),
            });
            dbcontext.SaveChanges();
            return newGame.Entity;
        }

        public async Task Delete(int id)
        {
            var game = await dbcontext.Games.SingleAsync(game => game.ID == id);
            dbcontext.Games.Remove(game);
            await dbcontext.SaveChangesAsync();
        }

        public async Task AddImage(int gameId, IFormFile image)
        {
            if (image == null)
                return;
            string extension = Path.GetExtension(image.FileName);
            using (var stream = new FileStream($"{imagePath}\\{gameId}{extension}", FileMode.OpenOrCreate))
            {
                await image.CopyToAsync(stream);
            }
        }
    }
}
