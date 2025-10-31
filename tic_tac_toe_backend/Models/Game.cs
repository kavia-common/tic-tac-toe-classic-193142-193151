using System;
using System.Collections.Generic;

namespace dotnet.Models
{
    /// <summary>
    /// Represents a Tic Tac Toe game with in-memory board state and basic rules.
    /// </summary>
    public class Game
    {
        /// <summary>
        /// Unique identifier for the game.
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// 3x3 board. Each cell can be "X", "O", or empty "".
        /// </summary>
        public string[][] Board { get; set; } =
        [
            ["", "", ""],
            ["", "", ""],
            ["", "", ""]
        ];

        /// <summary>
        /// Current status of the game.
        /// </summary>
        public string Status { get; set; } = "in_progress";

        /// <summary>
        /// The next player to move: "X" or "O". Only applicable when Status == in_progress.
        /// </summary>
        public string NextPlayer { get; set; } = "X";

        /// <summary>
        /// The winner symbol if any ("X" or "O"), otherwise null.
        /// </summary>
        public string? Winner { get; set; } = null;

        /// <summary>
        /// Count of moves made so far.
        /// </summary>
        public int MovesCount { get; set; } = 0;

        /// <summary>
        /// Places a mark on the board if valid.
        /// </summary>
        /// <param name="row">Row index (0-2)</param>
        /// <param name="col">Column index (0-2)</param>
        /// <param name="symbol">"X" or "O"</param>
        public void PlaceMark(int row, int col, string symbol)
        {
            Board[row][col] = symbol;
            MovesCount++;
        }

        /// <summary>
        /// Checks for a win and sets status/winner accordingly.
        /// Returns true if a win is detected.
        /// </summary>
        public bool CheckWin()
        {
            string[][] b = Board;
            string[,] grid = new string[3, 3];
            for (int r = 0; r < 3; r++)
            {
                for (int c = 0; c < 3; c++)
                {
                    grid[r, c] = b[r][c];
                }
            }

            // Rows and columns
            for (int i = 0; i < 3; i++)
            {
                if (!string.IsNullOrEmpty(grid[i, 0]) &&
                    grid[i, 0] == grid[i, 1] && grid[i, 1] == grid[i, 2])
                {
                    Winner = grid[i, 0];
                    Status = Winner.ToLower() + "_won";
                    return true;
                }
                if (!string.IsNullOrEmpty(grid[0, i]) &&
                    grid[0, i] == grid[1, i] && grid[1, i] == grid[2, i])
                {
                    Winner = grid[0, i];
                    Status = Winner.ToLower() + "_won";
                    return true;
                }
            }

            // Diagonals
            if (!string.IsNullOrEmpty(grid[0, 0]) &&
                grid[0, 0] == grid[1, 1] && grid[1, 1] == grid[2, 2])
            {
                Winner = grid[0, 0];
                Status = Winner.ToLower() + "_won";
                return true;
            }
            if (!string.IsNullOrEmpty(grid[0, 2]) &&
                grid[0, 2] == grid[1, 1] && grid[1, 1] == grid[2, 0])
            {
                Winner = grid[0, 2];
                Status = Winner.ToLower() + "_won";
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks and updates draw status if board is full and no winner.
        /// </summary>
        public void CheckDraw()
        {
            if (MovesCount >= 9 && Winner == null && Status == "in_progress")
            {
                Status = "draw";
            }
        }
    }
}
