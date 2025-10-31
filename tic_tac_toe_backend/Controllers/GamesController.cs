using System;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using dotnet.Services;
using dotnet.Models;

namespace dotnet.Controllers
{
    /// <summary>
    /// Controller exposing REST endpoints for Tic Tac Toe games.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [OpenApiTag("Games", Description = "Endpoints for creating games, making moves, and fetching status.")]
    public class GamesController : ControllerBase
    {
        private readonly GameService _service;

        public GamesController(GameService service)
        {
            _service = service;
        }

        /// <summary>
        /// Creates a new Tic Tac Toe game.
        /// </summary>
        /// <returns>Game status with gameId and empty board.</returns>
        [HttpPost]
        [Route("")]
        [OpenApiOperation("CreateGame")]
        [ProducesResponseType(typeof(GameStatusResponse), 201)]
        public IActionResult CreateGame()
        {
            var game = _service.CreateGame();
            var resp = _service.BuildStatus(game);
            return CreatedAtAction(nameof(GetGame), new { id = game.Id }, resp);
        }

        /// <summary>
        /// Gets the current status of a game by id.
        /// </summary>
        /// <param name="id">Game Id</param>
        /// <returns>Game status</returns>
        [HttpGet]
        [Route("{id:guid}")]
        [OpenApiOperation("GetGame")]
        [ProducesResponseType(typeof(GameStatusResponse), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetGame([FromRoute] Guid id)
        {
            var game = _service.GetGame(id);
            if (game == null)
            {
                return NotFound(new { message = "Game not found." });
            }
            return Ok(_service.BuildStatus(game));
        }

        /// <summary>
        /// Makes a move in the specified game. Turn is auto-determined (X then O).
        /// </summary>
        /// <param name="id">Game Id</param>
        /// <param name="request">Move coordinates</param>
        /// <returns>Updated game status</returns>
        [HttpPost]
        [Route("{id:guid}/moves")]
        [OpenApiOperation("MakeMove")]
        [ProducesResponseType(typeof(GameStatusResponse), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult MakeMove([FromRoute] Guid id, [FromBody] MoveRequest request)
        {
            var game = _service.GetGame(id);
            if (game == null)
            {
                return NotFound(new { message = "Game not found." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid move request. Row and Col must be between 0 and 2." });
            }

            if (!_service.TryMakeMove(id, request.Row, request.Col, out var error))
            {
                return BadRequest(new { message = error });
            }

            var updated = _service.GetGame(id)!;
            return Ok(_service.BuildStatus(updated));
        }
    }
}
