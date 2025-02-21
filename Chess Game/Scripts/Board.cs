using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Diagnostics;

namespace Chess_Game.Scripts
{
    enum Player
    {
        None,
        White,
        Black
    }

    internal static class Board
    {
        public static Color turn { get; private set; }
        public static Point oldTileSelected { get; private set; }
        public static Point tileSelected { get; private set; }

        private static PictureBox ChessBoard;
        private static PictureBox[,] tiles = new PictureBox[8, 8];
        public static Piece[,] pieces { get; private set; }
        private static string[,] startingPosition = new string[8, 8]
        {
            { "RookB", "KnightB", "BishopB", "QueenB", "KingB", "BishopB", "KnightB", "RookB" },
            { "PawnB", "PawnW", "PawnB", "PawnB", "PawnB", "PawnB", "PawnB", "PawnB" },
            { "", "", "", "", "", "", "", "" },
            { "", "", "", "", "", "", "", "" },
            { "", "", "", "", "", "", "", "" },
            { "", "", "", "", "", "", "", "" },
            { "PawnW", "PawnW", "PawnB", "PawnW", "PawnW", "PawnW", "PawnW", "PawnW" },
            { "RookW", "KnightW", "BishopW", "QueenW", "KingW", "BishopW", "KnightW", "RookW" }
        };

        public static void Initialize(PictureBox board)
        {
            ChessBoard = board;
            pieces = new Piece[8, 8];
            turn = Color.White;
            oldTileSelected = new Point(5, 5);
            tileSelected = new Point(5, 5);
        }

        public static void CreateBoard(Form form)
        {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    PictureBox tile = new PictureBox
                    {
                        Size = new Size(54, 54),
                        Location = new Point(ChessBoard.Left + col * 53, ChessBoard.Top + row * 53),
                        BackColor = Color.FromArgb(0, Color.LightGreen),
                        BorderStyle = BorderStyle.None, // Debugging grid (optional)
                        Parent = ChessBoard
                    };

                    tiles[row, col] = tile;
                    tiles[row, col].SizeMode = PictureBoxSizeMode.StretchImage;
                    form.Controls.Add(tile);
                    tile.BringToFront();
                    tile.Click += (sender, e) =>
                    {
                        SelectTile(sender, e);
                        MovePiece(sender, e);
                        HighlightPiece(sender, e);
                    };
                }
            }

            InitialBoardConfiguration();
        }

        public static void InitialBoardConfiguration()
        {
            string basePath = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    string imagePath = $"Assets\\{startingPosition[row, col]}.png";
                    imagePath = Path.Combine(basePath, imagePath);

                    if (File.Exists(imagePath))
                    {
                        tiles[row, col].Image = Image.FromFile(imagePath);
                    }

                    SetupPiece(row, col, imagePath);
                }
            }
        }

        private static void SetupPiece(int row, int col, string imagePath)
        {
            string piece = Path.GetFileName(imagePath);
            if (piece == ".png") return; // Ignore Empty Tiles

            piece = piece.Substring(0, piece.Length - 4); // Remove .png

            Color color = piece[piece.Length - 1] == 'W' ? Color.White : Color.Black;

            piece = piece.Substring(0, piece.Length - 1); // Remove W or B from end of piece string
            if (Enum.TryParse(piece, out ChessPiece classification))
                pieces[row, col] = new Piece(classification, color, imagePath);
        }

        public static void SelectTile(object sender, EventArgs e)
        {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    RemoveHighlight(row, col); // Reset Tile
                    if (ReferenceEquals(sender, tiles[row, col]))
                    {
                        oldTileSelected = tileSelected;
                        tileSelected = new Point(row, col);
                    }
                }
            }
        }

        public static void MovePiece(object sender, EventArgs e)
        {
            if (pieces[oldTileSelected.X, oldTileSelected.Y] == null)
                return; // Is last select a piece

            if (SelectedYourPiece())
                return; // You did not select your own piece again

            if (pieces[oldTileSelected.X, oldTileSelected.Y].color != turn)
                return; // Trying to move enemy piece

            if (!pieces[oldTileSelected.X, oldTileSelected.Y].IsPossibleMove(oldTileSelected, tileSelected))
                return; // Illegal movement of your piece

            // Need to Check Piece Taken
            Piece tempPiece = pieces[tileSelected.X, tileSelected.Y];
            pieces[tileSelected.X, tileSelected.Y] = pieces[oldTileSelected.X, oldTileSelected.Y];
            pieces[oldTileSelected.X, oldTileSelected.Y] = null;

            if (CheckSideForCheck(GetKingPosition(turn), turn))
            {
                pieces[oldTileSelected.X, oldTileSelected.Y] = pieces[tileSelected.X, tileSelected.Y];
                pieces[tileSelected.X, tileSelected.Y] = tempPiece;
                return;
            }

            pieces[tileSelected.X, tileSelected.Y].hasMoved = true;

            // Check Promotion
            if (pieces[tileSelected.X, tileSelected.Y].type == ChessPiece.Pawn && (tileSelected.X == 0 || tileSelected.X == 7))
            {
                string newImagePath = "";

                if (pieces[tileSelected.X, tileSelected.Y].color == Color.White)
                    newImagePath = pieces[tileSelected.X, tileSelected.Y].imagePath.Substring(0, pieces[tileSelected.X, tileSelected.Y].imagePath.Length - 9) + "QueenW.png";
                else
                    newImagePath = pieces[tileSelected.X, tileSelected.Y].imagePath.Substring(0, pieces[tileSelected.X, tileSelected.Y].imagePath.Length - 9) + "QueenB.png";

                pieces[tileSelected.X, tileSelected.Y].type = ChessPiece.Queen;
                pieces[tileSelected.X, tileSelected.Y].imagePath = newImagePath;
                tiles[tileSelected.X, tileSelected.Y].Image = Image.FromFile(pieces[tileSelected.X, tileSelected.Y].imagePath);
            }

            // Change Sides
            if (turn == Color.White)
                turn = Color.Black;
            else
                turn = Color.White;

            // Update Board
            tiles[tileSelected.X, tileSelected.Y].Image = Image.FromFile(pieces[tileSelected.X, tileSelected.Y].imagePath);
            tiles[oldTileSelected.X, oldTileSelected.Y].Image = null;
        }

        public static void HighlightPiece(object sender, EventArgs e)
        {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    if (SelectedYourPiece())
                    {
                        if (ReferenceEquals(sender, tiles[row, col]))
                        {
                            HighlightTile(row, col);
                            HighlightMoves();
                            return;
                        }
                    }
                }
            }
        }

        public static void HighlightTile(int row, int col)
        {
            if (row > 7 || row < 0) return;
            if (col > 7 || col < 0) return;
            tiles[row, col].BackColor = Color.FromArgb(150, Color.LightGreen);
        }

        public static void HighlightKillTile(int row, int col)
        {
            if (row > 7 || row < 0) return;
            if (col > 7 || col < 0) return;
            tiles[row, col].BackColor = Color.FromArgb(225, Color.LightPink);
        }

        public static void HighlightMoves()
        {
            Point[] validMoves = new Point[0];
            if (pieces[tileSelected.X, tileSelected.Y].type == ChessPiece.Pawn)
                validMoves = Pawn.PawnHighlightedMoves(pieces[tileSelected.X, tileSelected.Y]).ToArray();
            if (pieces[tileSelected.X, tileSelected.Y].type == ChessPiece.Rook)
                validMoves = Rook.RookHighlightedMoves(pieces[tileSelected.X, tileSelected.Y]).ToArray();
            if (pieces[tileSelected.X, tileSelected.Y].type == ChessPiece.Bishop)
                validMoves = Bishop.BishopHighlightedMoves(pieces[tileSelected.X, tileSelected.Y]).ToArray();
            if (pieces[tileSelected.X, tileSelected.Y].type == ChessPiece.Knight)
                validMoves = Knight.KnightHighlightedMoves(pieces[tileSelected.X, tileSelected.Y]).ToArray();
            if (pieces[tileSelected.X, tileSelected.Y].type == ChessPiece.Queen)
                validMoves = Queen.QueenHighlightedMoves(pieces[tileSelected.X, tileSelected.Y]).ToArray();
            if (pieces[tileSelected.X, tileSelected.Y].type == ChessPiece.King)
                validMoves = King.KingHighlightedMoves(pieces[tileSelected.X, tileSelected.Y]).ToArray();

            foreach (Point tile in validMoves)
            {
                if (IsTileOccupied(tile.X, tile.Y))
                    HighlightKillTile(tile.X, tile.Y);
                else
                    HighlightTile(tile.X, tile.Y);
            }
        }

        public static void RemoveHighlight(int row, int col)
        {
            tiles[row, col].BackColor = Color.FromArgb(0, Color.LightGreen);
        }

        public static bool SelectedYourPiece()
        {
            if (pieces[tileSelected.X, tileSelected.Y] == null) return false;
            return pieces[tileSelected.X, tileSelected.Y].color == turn;
        }

        public static bool IsTileOccupied(int row, int col)
        {
            if (row > 7 || row < 0) return false;
            if (col > 7 || col < 0) return false;
            if (pieces[row, col] != null) return true;
            return false;
        }

        public static bool CheckSideForCheck(Point kingPosition, Color side)
        {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Piece piece = pieces[row, col];

                    if (piece == null || piece.color == side)
                        continue; // Don't check empty spaces and your own pieces

                    if (piece.IsPossibleMove(new Point(row, col), kingPosition)) // Can piece put king in check
                        return true;
                }
            }

            return false;
        }
        
        private static Point GetKingPosition(Color side)
        {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Piece piece = pieces[row, col];

                    if (piece == null || piece.color != side)
                        continue;

                    if (piece.type == ChessPiece.King)
                        return new Point(row, col);
                }
            }

            return new Point();
        }
    }
}
