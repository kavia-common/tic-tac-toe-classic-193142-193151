using System.ComponentModel.DataAnnotations;

namespace dotnet.Models
{
    /// <summary>
    /// Request to place a move on the board.
    /// </summary>
    public class MoveRequest
    {
        /// <summary>
        /// Row index (0-2)
        /// </summary>
        [Range(0, 2)]
        public int Row { get; set; }

        /// <summary>
        /// Column index (0-2)
        /// </summary>
        [Range(0, 2)]
        public int Col { get; set; }
    }
}
