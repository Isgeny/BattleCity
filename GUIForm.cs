using System.Windows.Forms;

namespace BattleCity
{
    public partial class GUIForm : Form
    {
        public GUIForm()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }

        private void GUIForm_MouseMove(object sender, MouseEventArgs e)
        {
            Text = "Battle City " + "X:" + e.X.ToString() + " Y:" + e.Y.ToString();
        }
    }
}