using System.Collections.Generic;
using System.Drawing;
using System;

namespace Chess_Game.Scripts
{
    internal static class Pawn
    {
        public static bool PawnPossibleMoves(Point start, Point end)
        {
            int direction = 0;
            int moveDistance = Math.Abs(end.X - start.X);
            if (Board.turn == Color.White)
                direction--;
            else
                direction++;

            if (end.Y != start.Y && !Board.IsTileOccupied(end.X, end.Y))
                return false; // Trying to move diagonally without taking a piece

            if (end.Y == start.Y && Board.IsTileOccupied(end.X, end.Y))
                return false; // Can't move into another piece in front of you

            if (moveDistance > 2)
                return false; // Can't move more than 3 spaces

            if ((moveDistance == 2) && Board.pieces[start.X, start.Y].hasMoved)
                return false; // Pawns Can't move 2 spaces if they have moved from their starting positions


            return true;
        }

        public static List<Point> PawnHighlightedMoves(Piece piece)
        {
            List<Point> validMoves = new List<Point>();

            int direction = 0;
            if (piece.color == Color.White)
                direction--;
            else
                direction++;

            // Forward Movement
            if (!Board.IsTileOccupied(Board.tileSelected.X + direction, Board.tileSelected.Y)) {
                validMoves.Add(new Point(Board.tileSelected.X + direction, Board.tileSelected.Y));

                // Double Movement
                if (!piece.hasMoved && !Board.IsTileOccupied(Board.tileSelected.X + (direction * 2), Board.tileSelected.Y))
                {
                    validMoves.Add(new Point(Board.tileSelected.X + (direction * 2), Board.tileSelected.Y));
                }
            }

            // Diagonal Captures
            if (Board.IsTileOccupied(Board.tileSelected.X + direction, Board.tileSelected.Y - 1) && Board.pieces[Board.tileSelected.X + direction, Board.tileSelected.Y - 1].color != piece.color)
                validMoves.Add(new Point(Board.tileSelected.X + direction, Board.tileSelected.Y - 1));
            if (Board.IsTileOccupied(Board.tileSelected.X + direction, Board.tileSelected.Y + 1) && Board.pieces[Board.tileSelected.X + direction, Board.tileSelected.Y + 1].color != piece.color)
                validMoves.Add(new Point(Board.tileSelected.X + direction, Board.tileSelected.Y + 1));

            return validMoves;
        }
    }
}
