using System;
using System.Collections.Generic;
using System.Drawing;

namespace Chess_Game.Scripts
{
    internal static class King
    {
        public static bool KingPossibleMoves(Point start, Point end)
        {
            // Distance Greater than 1
            if (Math.Abs(end.X - start.X) > 1 || Math.Abs(end.Y - start.Y) > 1)
                return false;

            // Puts you in Check
            //if (IsPointCheck(end, Board.pieces[start.X, start.Y].color))
            //    return false;

            return true;
        }

        public static bool IsKingInCheck(Point kingPosition, Color kingColor)
        {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Piece piece = Board.pieces[row, col];

                    if (piece == null || piece.color != kingColor)
                        continue;

                    if (piece.IsPossibleMove(new Point(row, col), kingPosition))
                        return true;
                }
            }

            return false;
        }

        public static List<Point> KingHighlightedMoves(Piece piece)
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
                int row = Board.tileSelected.X + dir[0];
                int col = Board.tileSelected.Y + dir[1];

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
