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

    public partial class Leaderboard : Form
    {

        private PlayerBoardDAO dao;
        public Leaderboard()
        {
            InitializeComponent();
            dao = new PlayerBoardDAO();
            LoadPlayerBoard();

        }

        /// <summary>
        /// Načte informace o hráčích a jejich skóre z databáze a zobrazí je.
        /// </summary>
        private void LoadPlayerBoard()
        {
            StringBuilder sb = new StringBuilder();

            foreach (PlayerBoard pb in dao.GetAll())
            {
                sb.AppendLine($"{pb.Name} - {pb.Score} points");
            }

            BoardOfPlayers.Text = sb.ToString();
        }




    }
}
