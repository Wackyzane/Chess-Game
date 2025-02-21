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
            int moveDistance = end.X - start.X;

            if (Board.pieces[start.X, start.Y].color == Color.White)
                direction = -1;
            else
                direction = 1;

            if (end.Y != start.Y) // Moving Diagonally
            {
                if (Math.Abs(end.Y - start.Y) > 1 || moveDistance != direction)
                    return false; // Pawns can only move 1 tile diagonally while capturing

                if (!Board.IsTileOccupied(end.X, end.Y) || Board.pieces[end.X, end.Y].color == Board.pieces[start.X, start.Y].color)
                    return false; // Can only move Diagonally if capturing enemy piece
            }
            else // Moving forward
            {
                if (Board.IsTileOccupied(end.X, end.Y))
                    return false; // Can't move into occupied tile

                if (moveDistance != direction && moveDistance != 2 * direction)
                    return false; // Pawns can only move 1 or 2 spaces forward

                if (moveDistance == 2 * direction && Board.pieces[start.X, start.Y].hasMoved)
                    return false; // Can't move 2 spaces if pawn has moved before

                if (moveDistance == 2 * direction && Board.IsTileOccupied(start.X + direction, start.Y))
                    return false; // Can't jump over piece
            }

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
