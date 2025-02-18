using System.Collections.Generic;
using System.Drawing;
using System;

namespace Chess_Game.Scripts
{
    internal static class Rook
    {
        public static bool RookPossibleMoves()
        {
            if (Board.tileSelected.X != Board.oldTileSelected.X && Board.tileSelected.Y != Board.oldTileSelected.Y)
                return false; // Can't move diagonally

            if (PieceInWayOfMovement())
                return false; // If Piece in-between tile you selected and rook position

            return true;
        }

        private static bool PieceInWayOfMovement()
        {
            int rowDirection = Math.Sign(Board.tileSelected.X - Board.oldTileSelected.X); // -1 (up), 1 (down), 0 (no change)
            int colDirection = Math.Sign(Board.tileSelected.Y - Board.oldTileSelected.Y); // -1 (left), 1 (right), 0 (no change)

            int row = Board.oldTileSelected.X + rowDirection;
            int col = Board.oldTileSelected.Y + colDirection;

            while (row != Board.tileSelected.X || col != Board.tileSelected.Y)
            {
                if (Board.IsTileOccupied(row, col))
                    return true; // Found an obstacle

                row += rowDirection;
                col += colDirection;
            }

            return false;
        }

        public static List<Point> RookHighlightedMoves(Piece piece)
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
                int row = Board.tileSelected.X + dir[0];
                int col = Board.tileSelected.Y + dir[1];

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
