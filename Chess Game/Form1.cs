using Chess_Game.Scripts;
using System.Windows.Forms;

namespace Chess_Game
{
    public partial class BackgroundWindow : Form
    {
        public BackgroundWindow()
        {
            InitializeComponent();
            Board.Initialize(ChessBoard);
            Board.CreateBoard(this);
        }
    }
}
