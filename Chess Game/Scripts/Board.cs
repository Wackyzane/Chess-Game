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
        
        private static int[,] grid = new int[8,8];

        public static void CreateBoard(Form form)
        {
            string imagePath = Directory.GetParent(Application.StartupPath).Parent.FullName;
            imagePath = Path.Combine(imagePath, "Assets", "PawnW.png");
            for (int row = 0; row < grid.GetLength(0); row++)
            {
                for (int col = 0; col < grid.GetLength(1); col++)
                {
                    PictureBox chessPiece = new PictureBox();

                    chessPiece.Size = new Size(56, 56);
                    chessPiece.Location = new Point(226, 15);
                    chessPiece.SizeMode = PictureBoxSizeMode.StretchImage;
                    chessPiece.Name = Convert.ToChar('A' + col).ToString() + (row + 1);
                    chessPiece.Image = Image.FromFile(imagePath);

                    form.Controls.Add(chessPiece);
                }
            }
        }

        public static void ResetBoard()
        {
            Array.Clear(grid, 0, grid.Length);

            for (int column = 0; column < grid.GetLength(1); column++)
            {
                grid[1, column] = 0;
                grid[7, column] = 0;
            }
        }

    }
}
