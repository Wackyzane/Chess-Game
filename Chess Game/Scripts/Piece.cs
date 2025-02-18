using System.Drawing;
using System;

namespace Chess_Game.Scripts
{
    enum ChessPiece
    {
        King,
        Queen,
        Bishop,
        Knight,
        Rook,
        Pawn
    }

    internal class Piece
    {
        public ChessPiece type = ChessPiece.Pawn;
        public Color color = Color.White;
        public bool hasMoved = false;

        public string imagePath {  get; private set; }

        public Piece(ChessPiece type, Color pieceColor, string imagePath)
        {
            this.type = type;
            this.color = pieceColor;
            this.imagePath = imagePath;
        }

        public bool IsPossibleMove(Point start, Point end)
        {
            // Check to see if you are in check, if you are you can only move pieces to get yourself out of check

            if (Board.pieces[start.X, start.Y].type == ChessPiece.Pawn)
                return Pawn.PawnPossibleMoves(start, end);
            if (Board.pieces[start.X, start.Y].type == ChessPiece.Rook)
                return Rook.RookPossibleMoves(start, end);
            if (Board.pieces[start.X, start.Y].type == ChessPiece.Bishop)
                return Bishop.BishopPossibleMoves(start, end);
            if (Board.pieces[start.X, start.Y].type == ChessPiece.Knight)
                return Knight.KnightPossibleMoves(start, end);
            if (Board.pieces[start.X, start.Y].type == ChessPiece.Queen)
                return Queen.QueenPossibleMoves(start, end);
            if (Board.pieces[start.X, start.Y].type == ChessPiece.King)
                return King.KingPossibleMoves(start, end);

            return false;
        }
    }
}
