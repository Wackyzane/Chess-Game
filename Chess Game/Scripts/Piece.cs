using System.Drawing;

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

        public bool IsPossibleMove()
        {
            if (Board.pieces[Board.oldTileSelected.X, Board.oldTileSelected.Y].type == ChessPiece.Pawn)
                return Pawn.PawnPossibleMoves();
            if (Board.pieces[Board.oldTileSelected.X, Board.oldTileSelected.Y].type == ChessPiece.Rook)
                return Rook.RookPossibleMoves();

            return false;
        }
    }
}
