using System.Collections.Generic;
using System.Drawing;
using System;

namespace Chess_Game.Scripts
{
    internal static class Rook
    {
        public static bool RookPossibleMoves(Point start, Point end)
        {
            if (end.X != start.X && end.Y != start.Y)
                return false; // Can't move diagonally

            if (PieceInWayOfMovement(start, end))
                return false; // If Piece in-between tile you selected and rook position

            return true;
        }

        private static bool PieceInWayOfMovement(Point start, Point end)
        {
            int rowDirection = Math.Sign(end.X - start.X); // -1 (up), 1 (down), 0 (no change)
            int colDirection = Math.Sign(end.Y - start.Y); // -1 (left), 1 (right), 0 (no change)

            int row = start.X + rowDirection;
            int col = start.Y + colDirection;

            while (row != end.X || col != end.Y)
            {
                if (Board.IsTileOccupied(row, col))
                    return true; // Found an obstacle

                row += rowDirection;
                col += colDirection;
            }

            return false;
        }

        public static List<Point> RookHighlightedMoves(Point start, Piece piece)
        {
            List<Point> validMoves = new List<Point>();

            int[][] directions = {
                new int[] { -1, 0 }, // Up
                new int[] { 1, 0 },  // Down
                new int[] { 0, -1 }, // Left
                new int[] { 0, 1 }   // Right
            };

            foreach (var dir in directions)
            {
                int row = start.X + dir[0];
                int col = start.Y + dir[1];

                while (row >= 0 && row < 8 && col >= 0 && col < 8) // Stay within board limits
                {
                    if (Board.IsTileOccupied(row, col))
                    {
                        if (Board.pieces[row, col].color != piece.color)
                            validMoves.Add(new Point(row, col));
                        break; // Stop if we hit an occupied tile
                    }

                    validMoves.Add(new Point(row, col));

                    row += dir[0];
                    col += dir[1];
                }
            }

            return validMoves;
        }
    }
}
