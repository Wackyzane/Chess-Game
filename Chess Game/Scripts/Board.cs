using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace Chess_Game.Scripts
{
    enum ChessPiece
    {
        Empty,
        King,
        Queen,
        Bishop,
        Knight,
        Rook,
        Pawn
    }

    enum Player
    {
        None,
        White,
        Black
    }

    internal static class Board
    {
        private static PictureBox ChessBoard;
        private static PictureBox[,] tiles = new PictureBox[8,8];
        private static string[,] startingPosition = new string[8, 8]
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

        public static void Initialize(PictureBox board)
        {
            ChessBoard = board;
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
                    form.Controls.Add(tile);
                    tile.BringToFront();
                    tile.Click += (sender, e) =>
                    {
                        HighlightMoves(sender, e);
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
                    string piece = startingPosition[row, col];
                    string imagePath = $"Assets\\{piece}.png";
                    imagePath = Path.Combine(basePath, imagePath);
                    System.Diagnostics.Debug.WriteLine($"File Path: {imagePath}");

                    if (File.Exists(imagePath))
                    {
                        tiles[row, col].Image = Image.FromFile(imagePath);
                        tiles[row, col].SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                }
            }
        }

        public static void HighlightMoves(object sender, EventArgs e)
        {
            Control ctrl = (Control)sender;

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    tiles[row, col].BackColor = Color.FromArgb(0, Color.LightGreen);
                    if (ReferenceEquals(sender, tiles[row, col]))
                        tiles[row, col].BackColor = Color.FromArgb(150, Color.LightGreen);
                }
            }
            
            ctrl.BackColor = Color.FromArgb(150, Color.LightGreen);
        }
    }
}
