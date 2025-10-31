using System;

namespace dotnet.Models
{
    /// <summary>
    /// Response payload for game status.
    /// </summary>
    public class GameStatusResponse
    {
        public Guid GameId { get; set; }
        public string[][] Board { get; set; } = default!;
        public string Status { get; set; } = "in_progress";
        public string? NextPlayer { get; set; }
        public string? Winner { get; set; }
    }
}
