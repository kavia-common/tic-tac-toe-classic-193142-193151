using System;
using System.Collections.Concurrent;
using dotnet.Models;

namespace dotnet.Services
{
    /// <summary>
    /// Manages Tic Tac Toe games in memory using a thread-safe store.
    /// </summary>
    public class GameService
    {
        private readonly ConcurrentDictionary<Guid, Game> _games = new();

        // PUBLIC_INTERFACE
        /// <summary>
        /// Creates a new game with an empty 3x3 board.
        /// </summary>
        /// <returns>The created Game object.</returns>
        public Game CreateGame()
        {
            var game = new Game();
            _games[game.Id] = game;
            return game;
        }

        // PUBLIC_INTERFACE
        /// <summary>
        /// Gets a game by id.
        /// </summary>
        /// <param name="id">Game Id</param>
        /// <returns>Game or null if not found.</returns>
        public Game? GetGame(Guid id)
        {
            _games.TryGetValue(id, out var game);
            return game;
        }

        // PUBLIC_INTERFACE
        /// <summary>
        /// Attempts to apply a move to the game.
        /// </summary>
        /// <param name="gameId">Game Id</param>
        /// <param name="row">Row index (0-2)</param>
        /// <param name="col">Column index (0-2)</param>
        /// <param name="error">If false, contains error message.</param>
        /// <returns>True if move was successful; otherwise, false.</returns>
        public bool TryMakeMove(Guid gameId, int row, int col, out string error)
        {
            error = string.Empty;
            if (!_games.TryGetValue(gameId, out var game))
            {
                error = "Game not found.";
                return false;
            }

            if (game.Status != "in_progress")
            {
                error = "Game is already over.";
                return false;
            }

            if (row < 0 || row > 2 || col < 0 || col > 2)
            {
                error = "Row and Column must be between 0 and 2.";
                return false;
            }

            if (!string.IsNullOrEmpty(game.Board[row][col]))
            {
                error = "Cell is already occupied.";
                return false;
            }

            string current = game.NextPlayer;
            game.PlaceMark(row, col, current);

            // Check for end states
            if (!game.CheckWin())
            {
                game.CheckDraw();
                if (game.Status == "in_progress")
                {
                    // Alternate player
                    game.NextPlayer = current == "X" ? "O" : "X";
                }
                else
                {
                    // no next player after draw
                    game.NextPlayer = null;
                }
            }
            else
            {
                // winner decided, no next player
                game.NextPlayer = null;
            }

            return true;
        }

        // PUBLIC_INTERFACE
        /// <summary>
        /// Builds a GameStatusResponse from the current game state.
        /// </summary>
        /// <param name="game">Game instance</param>
        /// <returns>GameStatusResponse DTO</returns>
        public GameStatusResponse BuildStatus(Game game)
        {
            return new GameStatusResponse
            {
                GameId = game.Id,
                Board = game.Board,
                Status = game.Status,
                NextPlayer = game.Status == "in_progress" ? game.NextPlayer : null,
                Winner = game.Winner
            };
        }
    }
}
