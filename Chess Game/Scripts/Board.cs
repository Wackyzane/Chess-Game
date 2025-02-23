using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

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

        private static BackgroundWindow baseForm;
        public static bool gamePause = false;

        private static Panel ChessBoard;
        private readonly static PictureBox[,] tiles = new PictureBox[8, 8];
        public static Piece[,] pieces { get; private set; }
        private readonly static string[,] startingPosition = new string[8, 8]
        {
            { "RookB", "KnightB", "BishopB", "QueenB", "KingB", "BishopB", "KnightB", "RookB" },
            { "PawnB", "PawnB", "PawnB", "PawnB", "PawnB", "PawnB", "PawnB", "PawnB" },
            { "", "", "", "", "", "", "", "" },
            { "", "", "", "", "", "", "", "" },
            { "", "", "", "", "", "", "", "" },
            { "", "", "", "", "", "", "", "" },
            { "PawnW", "PawnW", "PawnW", "PawnW", "PawnW", "PawnW", "PawnW", "PawnW" },
            { "RookW", "KnightW", "BishopW", "QueenW", "KingW", "BishopW", "KnightW", "RookW" }
        };

        public static void Initialize(Panel board, BackgroundWindow backgroundWindow)
        {
            baseForm = backgroundWindow;
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
                        BorderStyle = BorderStyle.FixedSingle,
                        Parent = form
                    };

                    tiles[row, col] = tile;
                    tiles[row, col].SizeMode = PictureBoxSizeMode.StretchImage;
                    form.Controls.Add(tile);
                    tile.BringToFront();
                    tile.Click += (sender, e) =>
                    {
                        SelectTile(sender, e);
                        MovePiece(sender, e);
                    };

                    SetupPictureBox(row, col);
                }
            }

            ChessBoard.SendToBack();
        }

        private static void SetupPictureBox(int row, int col)
        {
            string basePath = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;

            string imagePath = $"Assets\\{startingPosition[row, col]}.png";
            imagePath = Path.Combine(basePath, imagePath);

            if (File.Exists(imagePath))
            {
                tiles[row, col].Image = Image.FromFile(imagePath);
            }

            SetupPiece(row, col, imagePath);
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
            if (gamePause)
                return;

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

            ShowPossibleMoves();
        }

        public static void ShowPossibleMoves()
        {
            if (SelectedYourPiece())
            {
                HighlightTile(tileSelected.X, tileSelected.Y);
                HighlightMoves();
            }
        }

        public static void MovePiece(object sender, EventArgs e)
        {
            if (CancelMovePiece())
                return;

            if (DoesMoveCauseCheck())
                return;

            CheckPromotion();
            ChangeSides();

            // Update Board
            tiles[tileSelected.X, tileSelected.Y].Image = Image.FromFile(pieces[tileSelected.X, tileSelected.Y].imagePath);
            tiles[oldTileSelected.X, oldTileSelected.Y].Image = null;
        }

        private static bool CancelMovePiece()
        {
            if (gamePause)
                return true;

            if (pieces[oldTileSelected.X, oldTileSelected.Y] == null)
                return true; // Is last select a piece

            if (SelectedYourPiece())
                return true; // You did not select your own piece again

            if (pieces[oldTileSelected.X, oldTileSelected.Y].color != turn)
                return true; // Trying to move enemy piece

            if (!pieces[oldTileSelected.X, oldTileSelected.Y].IsPossibleMove(oldTileSelected, tileSelected))
                return true; // Illegal movement of your piece

            return false;
        }

        private static bool DoesMoveCauseCheck()
        {
            // Check new Tile if occupied and add piece to kill list if so

            Piece tempPiece = pieces[tileSelected.X, tileSelected.Y];
            pieces[tileSelected.X, tileSelected.Y] = pieces[oldTileSelected.X, oldTileSelected.Y];
            pieces[oldTileSelected.X, oldTileSelected.Y] = null;

            if (CheckSideForCheck(GetKingPosition(turn), turn))
            {
                pieces[oldTileSelected.X, oldTileSelected.Y] = pieces[tileSelected.X, tileSelected.Y];
                pieces[tileSelected.X, tileSelected.Y] = tempPiece;
                CheckSideForCheckmate(GetKingPosition(turn), turn);
                return true;
            }

            pieces[tileSelected.X, tileSelected.Y].hasMoved = true;

            return false;
        }

        private static void ChangeSides()
        {
            if (turn == Color.White)
                turn = Color.Black;
            else
                turn = Color.White;
        }

        private static void CheckPromotion()
        {
            if (pieces[tileSelected.X, tileSelected.Y].type == ChessPiece.Pawn && (tileSelected.X == 0 || tileSelected.X == 7))
                baseForm.EnableDisablePromotionPanel(true);
        }

        public static void PawnPromotion(ChessPiece type)
        {
            Piece piece = pieces[tileSelected.X, tileSelected.Y];

            UpdateImagePath(piece, type);

            piece.type = type;
            tiles[tileSelected.X, tileSelected.Y].Image = Image.FromFile(piece.imagePath);
        }

        private static void UpdateImagePath(Piece piece, ChessPiece type)
        {
            string stringAddition = type.ToString();

            stringAddition += piece.color == Color.White ? "W.png" : "B.png";

            piece.imagePath = piece.imagePath.Substring(0, piece.imagePath.Length - 9) + stringAddition;
        }

        public static void HighlightTile(int row, int col)
        {
            if (row > 7 || row < 0) return; // Within Bounds
            if (col > 7 || col < 0) return;
            tiles[row, col].BackColor = Color.FromArgb(150, Color.LightGreen);
        }

        public static void HighlightKillTile(int row, int col)
        {
            if (row > 7 || row < 0) return; // Within Bounds
            if (col > 7 || col < 0) return;
            tiles[row, col].BackColor = Color.FromArgb(225, Color.LightPink);
        }

        public static void HighlightMoves()
        {
            Point[] validMoves = GetValidMoves(tileSelected, pieces[tileSelected.X, tileSelected.Y]);

            foreach (Point tile in validMoves)
            {
                if (IsTileOccupied(tile.X, tile.Y))
                    HighlightKillTile(tile.X, tile.Y);
                else
                    HighlightTile(tile.X, tile.Y);
            }
        }

        private static Point[] GetValidMoves(Point start, Piece piece)
        {
            Point[] validMoves = new Point[0];

            if (piece == null) return validMoves;

            if (piece.type == ChessPiece.Pawn)
                validMoves = Pawn.PawnHighlightedMoves(start, piece).ToArray();

            if (piece.type == ChessPiece.Rook)
                validMoves = Rook.RookHighlightedMoves(start, piece).ToArray();

            if (piece.type == ChessPiece.Bishop)
                validMoves = Bishop.BishopHighlightedMoves(start, piece).ToArray();

            if (piece.type == ChessPiece.Knight)
                validMoves = Knight.KnightHighlightedMoves(start, piece).ToArray();

            if (piece.type == ChessPiece.Queen)
                validMoves = Queen.QueenHighlightedMoves(start, piece).ToArray();

            if (piece.type == ChessPiece.King)
                validMoves = King.KingHighlightedMoves(start, piece).ToArray();

            return validMoves;
        }

        public static void RemoveHighlight(int row, int col)
        {
            if (row > 7 || row < 0) return; // Within Bounds
            if (col > 7 || col < 0) return;
            tiles[row, col].BackColor = Color.FromArgb(0, Color.LightGreen);
        }

        public static bool SelectedYourPiece()
        {
            if (pieces[tileSelected.X, tileSelected.Y] == null) return false;
            return pieces[tileSelected.X, tileSelected.Y].color == turn;
        }

        public static bool IsTileOccupied(int row, int col)
        {
            if (row > 7 || row < 0) return false; // Within bounds
            if (col > 7 || col < 0) return false;

            if (pieces[row, col] != null) return true; // Occupied

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

        public static void CheckSideForCheckmate(Point kingPosition, Color side) // Refactor
        {
            bool temp = false;

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Piece piece = pieces[row, col];

                    if (piece == null || piece.color != side)
                        continue;

                    Point[] validMoves = GetValidMoves(new Point(row, col), piece);

                    foreach (Point move in validMoves)
                    {
                        Piece tempPiece = pieces[move.X, move.Y];
                        pieces[move.X, move.Y] = pieces[row, col]; // Move Piece to new tile
                        pieces[row, col] = null;


                        Point newKingPosition = kingPosition;

                        if (pieces[move.X, move.Y].type == ChessPiece.King)
                            newKingPosition = move;

                        if (!CheckSideForCheck(newKingPosition, side))
                        {
                            HighlightKillTile(move.X, move.Y);
                            temp = true;
                        }

                        // Revert Piece back
                        pieces[row, col] = pieces[move.X, move.Y]; 
                        pieces[move.X, move.Y] = tempPiece;
                    }
                }
            }
            if (temp)
                return;

            baseForm.ShowEndGame();
        }
        
        private static Point GetKingPosition(Color side) // Save in Memory
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
