
using System;
using System.Drawing;
using System.Collections.Generic;

namespace Chess_Game.Scripts
{
    internal static class Bishop
    {
        public static bool BishopPossibleMoves(Point start, Point end)
        {
            if (Math.Abs(end.X - start.X) != Math.Abs(end.Y - start.Y))
                return false; // Is Diagonal Movement

            if (PieceInWayOfMovement(start, end))
                return false; // No Piece in-between start and end point

            return true;
        }

        private static bool PieceInWayOfMovement(Point start, Point end)
        {
            int rowDirection = Math.Sign(end.X - start.X); // -1 (up), 1 (down)
            int colDirection = Math.Sign(end.Y - start.Y); // -1 (left), 1 (right)

            int row = start.X + rowDirection;
            int col = start.Y + colDirection;

            // Move step by step toward the end point, stopping before reaching it
            while (row != end.X && col != end.Y)
            {
                if (Board.IsTileOccupied(row, col))
                    return true; // Found an obstacle

                row += rowDirection;
                col += colDirection;
            }

            return false;
        }

        public static List<Point> BishopHighlightedMoves(Piece piece)
        {
            List<Point> validMoves = new List<Point>();

            int[][] directions = {
                new int[] { -1, -1 }, // Up-Left
                new int[] { -1, 1 },  // Up-Right
                new int[] { 1, -1 },  // Down-Left
                new int[] { 1, 1 }    // Down-Right
            };

            foreach (var dir in directions)
            {
                int row = Board.tileSelected.X + dir[0];
                int col = Board.tileSelected.Y + dir[1];

                while (row >= 0 && row < 8 && col >= 0 && col < 8) // Stay within board limits
                {
                    if (Board.IsTileOccupied(row, col)) // Stop if we hit an occupied tile
                    {
                        if (Board.pieces[row, col].color != piece.color)
                            validMoves.Add(new Point(row, col));
                        break;
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
