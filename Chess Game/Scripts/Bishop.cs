
using System;
using System.Drawing;
using System.Collections.Generic;

namespace Chess_Game.Scripts
{
    internal static class Bishop
    {
        public static bool BishopPossibleMoves()
        {
            if (Math.Abs(Board.tileSelected.X - Board.oldTileSelected.X) != Math.Abs(Board.tileSelected.Y - Board.oldTileSelected.Y))
                return false; // Is Diagonal Movement

            if (PieceInWayOfMovement())
                return false; // No Piece in-between start and end point

            return true;
        }

        private static bool PieceInWayOfMovement()
        {
            int rowDirection = Math.Sign(Board.tileSelected.X - Board.oldTileSelected.X); // -1 (up), 1 (down)
            int colDirection = Math.Sign(Board.tileSelected.Y - Board.oldTileSelected.Y); // -1 (left), 1 (right)

            int row = Board.oldTileSelected.X + rowDirection;
            int col = Board.oldTileSelected.Y + colDirection;

            // Move step by step toward the end point, stopping before reaching it
            while (row != Board.tileSelected.X && col != Board.tileSelected.Y)
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
