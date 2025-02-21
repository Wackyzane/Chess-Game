using System.Drawing;
using System.Collections.Generic;

namespace Chess_Game.Scripts
{
    internal static class Queen
    {
        public static bool QueenPossibleMoves(Point start, Point end)
        {
            if (!Rook.RookPossibleMoves(start, end) && !Bishop.BishopPossibleMoves(start, end))
                return false; // Can move Straight or Diagonal and is not interrupted by piece

            return true;
        }

        public static List<Point> QueenHighlightedMoves(Point start, Piece piece)
        {
            List<Point> validMoves = new List<Point>();

            // Get Rook HighlightedMoves
            validMoves.AddRange(Rook.RookHighlightedMoves(start, piece));

            // Get Bishop HighlightedMoves
            validMoves.AddRange(Bishop.BishopHighlightedMoves(start, piece));

            return validMoves;
        }
    }
}
