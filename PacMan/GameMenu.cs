using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PacMan
{
    public partial class PACMAN : Form
    {
        public PACMAN()
        {
            InitializeComponent();
        }

        private void LoadGame(object sender, EventArgs e)
        {
            Form1 gameWindow = new Form1();

            gameWindow.Show();
        }

        private void LoadLeaderboard(object sender, EventArgs e)
        {
            Leaderboard leaderboard = new Leaderboard();

            leaderboard.Show();
        }
    }
}
