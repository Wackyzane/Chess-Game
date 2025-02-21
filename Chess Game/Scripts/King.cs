using System;
using System.Collections.Generic;
using System.Drawing;

namespace Chess_Game.Scripts
{
    internal static class King
    {
        public static bool KingPossibleMoves(Point start, Point end)
        {
            if (Math.Abs(end.X - start.X) > 1 || Math.Abs(end.Y - start.Y) > 1)
                return false; // Distance Greater than 1

            return true;
        }

        public static List<Point> KingHighlightedMoves(Point start, Piece piece)
        {
            List<Point> validMoves = new List<Point>();

            int[][] directions = {
                new int[] { -1, 0 }, // Up
                new int[] { -1, -1 }, // Up-Left
                new int[] { 0, -1 }, // Left
                new int[] { -1, 1 },  // Up-Right
                new int[] { 1, 0 },  // Down
                new int[] { 1, -1 },  // Down-Left
                new int[] { 1, 1 },    // Down-Right
                new int[] { 0, 1 }   // Right
            };

            foreach (var dir in directions)
            {
                int row = start.X + dir[0];
                int col = start.Y + dir[1];

                if (row >= 0 && row < 8 && col >= 0 && col < 8) // Stay within board limits
                {
                    if (Board.IsTileOccupied(row, col)) // Stop if we hit an occupied tile
                    {
                        if (Board.pieces[row, col].color != piece.color)
                            validMoves.Add(new Point(row, col));
                    }
                    else
                    {
                        validMoves.Add(new Point(row, col));
                    }
                }
            }

            return validMoves;
        }
    }
}
