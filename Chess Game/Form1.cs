using Chess_Game.Scripts;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Chess_Game
{
    public partial class BackgroundWindow : Form
    {
        public BackgroundWindow()
        {
            InitializeComponent();
            Board.Initialize(ChessBoard, this);
            Board.CreateBoard(this);
        }

        public void EnableDisablePromotionPanel(bool enableStatus)
        {
            Board.gamePause = enableStatus;
            PromotionPanel.Enabled = enableStatus;
            PromotionPanel.Visible = enableStatus;
            PromotionPanel.BringToFront();
        }

        public void ShowEndGame()
        {
            Board.gamePause = true;
            WinGameScreen.Enabled = true;
            WinGameScreen.Visible = true;
            WinGameScreen.BringToFront();
        }

        public void PromotePawn(object sender, EventArgs e)
        {
            PictureBox button = sender as PictureBox;

            if (Enum.TryParse(button.Name, out ChessPiece piece))
            {
                Board.PawnPromotion(piece);
                EnableDisablePromotionPanel(false);
            }
        }
    }
}
