using SharedProject.DbModels;
using Domain.RepositoryInterfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class GameService
    {
        private readonly IGameRepo gameRepo;

        public GameService(IGameRepo gameRepo)
        {
            this.gameRepo = gameRepo;
        }

        public async Task<ICollection<Game>> GetAllGames()
        {
            return await gameRepo.List();
        }

        public async Task<Game> GetGameById(int id)
        {
            return await gameRepo.FindById(id);
        }

        public async Task<Game> AddOrUpdateGame(Game game)
        {
            var newGame = await gameRepo.Insert(game);
            return newGame;
        }

        public async Task AddImage(int gameId, IFormFile image)
        {
            await gameRepo.AddImage(gameId, image);
        }

        public async Task Delete(int id)
        {
            await gameRepo.Delete(id);
        }
    }
}
