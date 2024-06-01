using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace PacMan
{
    public class PlayerBoard : IBaseClass
    {
        private int id;
        private string name;
        private int score;

        public int ID { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public int Score { get => score; set => score = value; }

        public PlayerBoard(int id, string name, int score)
        {
            this.ID = id;
            this.name = name;
            this.score = score;
        }

        
        public PlayerBoard(string name, int score)
        {
            this.ID = 0;
            this.name = name;
            this.score = score;
        }
        
    }
}
