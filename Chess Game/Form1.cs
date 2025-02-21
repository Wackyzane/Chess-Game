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

        public void EnablePromotionPanel()
        {
            Board.gamePause = true;
            PromotionPanel.Enabled = true;
            PromotionPanel.Visible = true;
            PromotionPanel.BringToFront();
        }

        public void DisablePromotionPanel()
        {
            Board.gamePause = false;
            PromotionPanel.Enabled = false;
            PromotionPanel.Visible = false;
        }

        public void ShowEndGame()
        {
            Board.gamePause = true;
            WinGameScreen.Enabled = true;
            WinGameScreen.Visible = true;
            WinGameScreen.BringToFront();
        }

        private void ChangePawnToQueen(object sender, System.EventArgs e)
        {
            Board.PawnPromotion(ChessPiece.Queen);
            DisablePromotionPanel();
        }

        private void ChangePawnToKnight(object sender, System.EventArgs e)
        {
            Board.PawnPromotion(ChessPiece.Knight);
            DisablePromotionPanel();
        }

        private void ChangePawnToBishop(object sender, System.EventArgs e)
        {
            Board.PawnPromotion(ChessPiece.Bishop);
            DisablePromotionPanel();
        }

        private void ChangePawnToRook(object sender, System.EventArgs e)
        {
            Board.PawnPromotion(ChessPiece.Rook);
            DisablePromotionPanel();
        }
    }
}
