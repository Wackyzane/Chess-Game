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

        public string imagePath;

        public Piece(ChessPiece type, Color pieceColor, string imagePath)
        {
            this.type = type;
            color = pieceColor;
            this.imagePath = imagePath;
        }

        public bool IsPossibleMove(Point start, Point end)
        {
            if (Board.pieces[start.X, start.Y].type == ChessPiece.Pawn) // Pawn
                return Pawn.PawnPossibleMoves(start, end);

            if (Board.pieces[start.X, start.Y].type == ChessPiece.Rook) // Rook
                return Rook.RookPossibleMoves(start, end);

            if (Board.pieces[start.X, start.Y].type == ChessPiece.Bishop) // Bishop
                return Bishop.BishopPossibleMoves(start, end);

            if (Board.pieces[start.X, start.Y].type == ChessPiece.Knight) // Knight
                return Knight.KnightPossibleMoves(start, end);

            if (Board.pieces[start.X, start.Y].type == ChessPiece.Queen) // Queen
                return Queen.QueenPossibleMoves(start, end);

            if (Board.pieces[start.X, start.Y].type == ChessPiece.King) // King
                return King.KingPossibleMoves(start, end);

            return false;
        }
    }
}
