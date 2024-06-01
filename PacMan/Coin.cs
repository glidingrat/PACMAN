using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacMan
{
    public class Coin
    {
        private List<CoinData> coins = new List<CoinData>();
        private Form form;

        /// <summary>
        /// Vrací počet zbývajících (neposbíraných) mincí.
        /// </summary>
        public int RemainingCoins
        {
            get
            {
                int remaining = 0;
                foreach (var coinData in coins)
                {
                    if (coinData.PictureBox.Visible) 
                    {
                        remaining++;
                    }
                }
                return remaining;
            }
        }

        public Coin(Form form)
        {
            this.form = form;
            InitializeCoins();
        }

        /// <summary>
        /// Inicializuje seznam mincí přidáním všech PictureBoxů s tagem "coin" z formuláře do seznamu mincí.
        /// </summary>
        private void InitializeCoins()
        {
            foreach (Control x in form.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "coin")
                {
                    coins.Add(new CoinData((PictureBox)x, x.Location));
                }
            }
        }

        /// <summary>
        /// Resetuje všechny mince ve hře
        /// </summary>
        public void ResetCoins()
        {
            foreach (var coin in coins)
            {
                if (!form.Controls.Contains(coin.PictureBox))
                {
                    form.Controls.Add(coin.PictureBox);
                }
                coin.PictureBox.Location = coin.OriginalLocation;
                coin.PictureBox.Visible = true; 
            }
        }

        /// <summary>
        /// Detekuje kolizi mezi hráčem a mincemi. Pokud je kolize detekována,
        /// mince je odstraněna ze hry, její viditelnost je nastavena na false,
        /// skóre se zvýší a aktualizuje se štítek se skóre.
        /// </summary>
        /// <param name="player">Ovládací prvek hráče, se kterým se kontroluje kolize mincí.</param>
        /// <param name="score">Aktuální skóre hráče, které se zvýší při kolizi.</param>
        /// <param name="scoreLabel">Štítek zobrazující skóre hráče.</param>
        public void DetectCoinCollision(Control player, ref int score, Label scoreLabel)
        {
            foreach (Control x in form.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "coin")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds))
                    {
                        form.Controls.Remove(x);
                        x.Visible = false;
                        score++; 
                        scoreLabel.Text = "Score: " + score.ToString(); 
                    }
                }
            }
        }

        /// <summary>
        /// Reprezentuje data o minci, včetně PictureBoxu mince a její původní pozice.
        /// </summary>
        private class CoinData
        {
            public PictureBox PictureBox { get; private set; }
            public Point OriginalLocation { get; private set; }

            public CoinData(PictureBox pictureBox, Point originalLocation)
            {
                PictureBox = pictureBox;
                OriginalLocation = originalLocation;
            }
        }
    }
}
