using SharedProject.DbModels;
using Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/game")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly GameService gameService;

        public GameController(GameService gameService)
        {
            this.gameService = gameService;
        }

        [HttpGet("all")]
        public async Task<ActionResult<ICollection<Game>>> GetAllGames()
        {
            var games = await gameService.GetAllGames();
            return Ok(games);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetGame(int id)
        {
            var game = await gameService.GetGameById(id);
            Console.WriteLine(game.ParentGame?.Name);
            return Ok(game);
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<int>> AddOrUpdateGame([FromBody] Game game)
        {
            //Console.WriteLine(game.ParentGame.ID + "\n" + game.ParentGame.Name + "\n" + game.PlayersFrom + "\n" + game.PlayersTo + "\n" + game.PlaytimeFrom + "\n" + game.PlaytimeTo + "\n" + game.SubName + "\n" + game.ParentGame);
            var newGame = await gameService.AddOrUpdateGame(game);
            return Ok(newGame.ID);
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        [HttpPut("image/{gameId}")]
        public async Task<IActionResult> SetImage([FromForm(Name = "image")] IFormFile image, int gameId)
        {
            await gameService.AddImage(gameId, image);
            return Ok();
        }

        [HttpGet("image/{imageId}")]
        public IActionResult GetImage(int imageId)
        {
            var files = Directory.GetFiles("..\\game_images", $"{imageId}.*");
            if (files.Length > 0)
            {
                string extension = Path.GetExtension(files.First());
                return new FileStreamResult(new FileStream(files.First(), FileMode.Open, FileAccess.Read, FileShare.Read), $"image/{extension}");
            }
            return NotFound();
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await gameService.Delete(id);
            return Ok();
        }
    }
}
