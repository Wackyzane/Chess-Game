using System.Collections.Generic;
using System.Drawing;
using System;

namespace Chess_Game.Scripts
{
    internal static class Knight
    {
        public static bool KnightPossibleMoves()
        {
            // Moving in an L shape
            if (!LShapeMove())
                return false;

            return true;
        }

        private static bool LShapeMove()
        {
            int dx = Math.Abs(Board.tileSelected.X - Board.oldTileSelected.X);
            int dy = Math.Abs(Board.tileSelected.Y - Board.oldTileSelected.Y);

            // A knight moves in an L-shape: (2,1) or (1,2)
            return (dx == 2 && dy == 1) || (dx == 1 && dy == 2);
        }

        public static List<Point> KnightHighlightedMoves(Piece piece)
        {
            List<Point> validMoves = new List<Point>();

            // All possible moves for a Knight (L-shape)
            int[][] moves = {
                new int[] { -2, -1 }, new int[] { -2, 1 }, // Up-Left, Up-Right
                new int[] { -1, -2 }, new int[] { -1, 2 }, // Left-Up, Right-Up
                new int[] { 1, -2 },  new int[] { 1, 2 },  // Left-Down, Right-Down
                new int[] { 2, -1 },  new int[] { 2, 1 }   // Down-Left, Down-Right
            };

            foreach (var move in moves)
            {
                int row = Board.tileSelected.X + move[0];
                int col = Board.tileSelected.Y + move[1];

                // Ensure move is within bounds
                if (row >= 0 && row < 8 && col >= 0 && col < 8)
                {
                    // If occupied, only add if enemy piece is there
                    if (Board.IsTileOccupied(row, col))
                    {
                        if (Board.pieces[row, col].color != piece.color)
                            validMoves.Add(new Point(row, col));
                    }
                    else
                    {
                        validMoves.Add(new Point(row, col)); // Unoccupied tiles are valid
                    }
                }
            }

            return validMoves;
        }
    }
}
