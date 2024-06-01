
using System.Windows.Forms;
using Priority_Queue;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PacMan
{
    public partial class Form1 : Form
    {
        private PlayerBoardDAO dao;
        private Coin coinManager;

        bool moveLeft, moveRight, moveUp, moveDown;
        int playerSpeed = 5;
        int playerLives = 3;
        private List<PictureBox> lifePictureBoxes;
        int ghostSpeed = 5;
        int score = 0;
        private bool NameBoxClicked = false;

        bool gameOver = false;

        private System.Windows.Forms.Timer delayTimerOrange;
        private System.Windows.Forms.Timer delayTimerPink;
        private System.Windows.Forms.Timer delayTimerBlue;


        public Form1()
        {
            InitializeComponent();

            delayTimerPink = new System.Windows.Forms.Timer();
            delayTimerPink.Interval = 7000; // 7 sekund pro ghostRed
            delayTimerPink.Tick += DelayTimerPink_Tick;

            delayTimerBlue = new System.Windows.Forms.Timer();
            delayTimerBlue.Interval = 14000; // 14 sekund pro ghostPink
            delayTimerBlue.Tick += DelayTimerBlue_Tick;

            delayTimerOrange = new System.Windows.Forms.Timer();
            delayTimerOrange.Interval = 21000; // 21 sekund pro ghostBlue
            delayTimerOrange.Tick += DelayTimerOrange_Tick;


            delayTimerOrange.Start();
            delayTimerPink.Start();
            delayTimerBlue.Start();


            coinManager = new Coin(this);
            dao = new PlayerBoardDAO();

            NameBox.Click += new EventHandler(NameBox_Click);
            NameBox.KeyPress += new KeyPressEventHandler(NameBox_KeyPress);
            lifePictureBoxes = new List<PictureBox> { life1, life2, life3 };

        }
        private void NameBox_Click(object sender, EventArgs e)
        {
            NameBoxClicked = true;
        }

        private void NameBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!NameBoxClicked)
            {
                e.Handled = true; // Zamezí se dalšímu zpracování stisknuté klávesy
            }
        }


        private void DelayTimerOrange_Tick(object sender, EventArgs e)
        {
            delayTimerOrange.Stop();
        }

        private void DelayTimerPink_Tick(object sender, EventArgs e)
        {
            delayTimerPink.Stop();
        }

        private void DelayTimerBlue_Tick(object sender, EventArgs e)
        {
            delayTimerBlue.Stop();
        }


        private void mainTimerEvent(object sender, EventArgs e)
        {
            // Pohyb hráče doleva
            if (moveLeft)
            {
                if (player.Left - playerSpeed < 0) // Pokud hráč přejde levý okraj okna
                {
                    player.Left = this.ClientSize.Width - player.Width; // Nastavíme hráče na pravý okraj okna
                }
                else if (!CheckWallCollision(player.Left - playerSpeed, player.Top, player.Width, player.Height))
                {
                    player.Left -= playerSpeed; // Pohyb hráče doleva, pokud není kolize s zdmi
                }
            }

            // Pohyb hráče doprava
            if (moveRight)
            {
                if (player.Right + playerSpeed > this.ClientSize.Width) // Pokud hráč přejde pravý okraj okna
                {
                    player.Left = 0; // Nastavíme hráče na levý okraj okna
                }
                else if (!CheckWallCollision(player.Left + playerSpeed, player.Top, player.Width, player.Height))
                {
                    player.Left += playerSpeed; // Pohyb hráče doprava, pokud není kolize s zdmi
                }
            }

            // Pohyb hráče nahoru
            if (moveUp && player.Top > 0)
            {
                if (!CheckWallCollision(player.Left, player.Top - playerSpeed, player.Width, player.Height))
                {
                    player.Top -= playerSpeed; // Pohyb hráče nahoru, pokud není kolize s zdmi
                }
            }

            // Pohyb hráče dolů
            if (moveDown && player.Bottom < 1080)
            {
                if (!CheckWallCollision(player.Left, player.Top + playerSpeed, player.Width, player.Height))
                {
                    player.Top += playerSpeed; // Pohyb hráče dolů, pokud není kolize s zdmi
                }
            }

            // Detekce kolize s mincemi
            coinManager.DetectCoinCollision(player, ref score, labelScore);
            // resetuje vsechny mince kdyz se seberou vsechny
            if (coinManager.RemainingCoins == 0)
            {
                coinManager.ResetCoins();
            }



            // Pohyb ducha
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox)
                {
                    string name = x.Name;
                    if (name == "ghostRed")
                    {
                        MoveGhost((PictureBox)x, 4, 5, 6, 7);
                    }
                    else if (name == "ghostPink" && !delayTimerPink.Enabled)
                    {
                        MoveGhost((PictureBox)x, 8, 9, 10, 11);
                    }
                    else if (name == "ghostBlue" && !delayTimerBlue.Enabled)
                    {
                        MoveGhost((PictureBox)x, 12, 13, 14, 15);
                    }
                    else if (name == "ghostOrange" && !delayTimerOrange.Enabled)
                    {
                        MoveGhost((PictureBox)x, 16, 17, 18, 19);
                    }
                }
            }

        }


        private void keyisdown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A)
            {
                moveLeft = true;
                player.Image = imageList.Images[0];
            }

            if (e.KeyCode == Keys.D)
            {
                moveRight = true;
                player.Image = imageList.Images[1];
            }

            if (e.KeyCode == Keys.W)
            {
                moveUp = true;
                player.Image = imageList.Images[2];
            }

            if (e.KeyCode == Keys.S)
            {
                moveDown = true;
                player.Image = imageList.Images[3];
            }

        }

        private void keyisup(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A) moveLeft = false;
            if (e.KeyCode == Keys.D) moveRight = false;
            if (e.KeyCode == Keys.W) moveUp = false;
            if (e.KeyCode == Keys.S) moveDown = false;

            if (e.KeyCode == Keys.R && gameOver)
            {
                RestartGame();

                coinManager.ResetCoins();
                // Resetování seznamu PictureBoxů a jejich přidání zpět do formuláře
                lifePictureBoxes.Clear();
                lifePictureBoxes.AddRange(new PictureBox[] { life1, life2, life3 });

                foreach (PictureBox lifeBox in lifePictureBoxes)
                {
                    if (!this.Controls.Contains(lifeBox))
                    {
                        this.Controls.Add(lifeBox);
                    }
                    lifeBox.Visible = true; // Ujistěte se, že jsou viditelné
                }

                labelScore.Text = "Score: 0";
                score = 0;
            }
        }

        /// <summary>
        /// Kontroluje kolizi s zdmi.
        /// </summary>
        /// <param name="x">X-ová souřadnice prvku.</param>
        /// <param name="y">Y-ová souřadnice prvku.</param>
        /// <param name="width">Šířka prvku.</param>
        /// <param name="height">Výška prvku.</param>
        /// <returns>True, pokud došlo ke kolizi se zdí, jinak false.</returns>
        private bool CheckWallCollision(int x, int y, int width, int height)
        {
            foreach (Control wall in this.Controls)
            {
                if (wall is PictureBox && (string)wall.Tag == "wall")
                {
                    if (x < wall.Right && x + width > wall.Left &&
                        y < wall.Bottom && y + height > wall.Top)
                    {
                        return true; 
                    }
                }
            }
            return false; 
        }

        /// <summary>
        /// Vrátí seznam sousedních bodů okolo zadaného bodu.
        /// </summary>
        /// <param name="point">Vstupní bod, pro který se hledají sousední body.</param>
        /// <returns>Seznam sousedních bodů.</returns>
        private List<Point> GetNeighbors(Point point)
        {
            List<Point> neighbors = new List<Point>();

            foreach (Point neighbor in new Point[] {
               new Point(point.X - ghostSpeed, point.Y),
               new Point(point.X + ghostSpeed, point.Y),
               new Point(point.X, point.Y - ghostSpeed),
               new Point(point.X, point.Y + ghostSpeed)
            })
            {
                // Pokud sousední bod není kolizním bodem (zeď)
                if (!CheckWallCollision(neighbor.X, neighbor.Y, ghostRed.Width, ghostRed.Height))
                {
                    neighbors.Add(neighbor); // Přidej sousední bod do seznamu
                }
            }

            return neighbors; // Vrať seznam sousedních bodů
        }

        /// <summary>
        /// Implementace algoritmu A* pro hledání nejkratší cesty mezi dvěma body.
        /// </summary>
        /// <param name="start">Počáteční bod hledané cesty.</param>
        /// <param name="goal">Cílový bod hledané cesty.</param>
        /// <returns>Seznam bodů tvořící nejkratší nalezenou cestu.</returns>
        private List<Point> AStar(Point start, Point goal)
        {
            List<Point> path = new List<Point>();

            SimplePriorityQueue<Point> openSet = new SimplePriorityQueue<Point>();// Otevřený seznam, prioritní fronta
            openSet.Enqueue(start, 0); // Přidání počátečního uzlu do otevřeného seznamu
            Dictionary<Point, Point> cameFrom = new Dictionary<Point, Point>(); // Uložení předcházejících bodů
            Dictionary<Point, float> gScore = new Dictionary<Point, float>(); // Skóre cesty z počátečního bodu k danému bodu
            gScore[start] = 0; // Skóre cesty z počátečního bodu do něj samotného je 0

            // Hlavní smyčka A*
            while (openSet.Count > 0)
            {
                Point current = openSet.Dequeue(); // Vyber uzel s nejnižším fScore z openSet

                if (current == goal)
                {
                    // Rekonstrukce cesty
                    while (cameFrom.ContainsKey(current))
                    {
                        path.Add(current);
                        current = cameFrom[current];
                    }
                    path.Reverse();
                    return path;
                }

                foreach (Point neighbor in GetNeighbors(current))
                {
                    float tentative_gScore = gScore[current] + Distance(current, neighbor); // Skóre cesty od start k sousedovi

                    if (!gScore.ContainsKey(neighbor) || tentative_gScore < gScore[neighbor])
                    {
                        cameFrom[neighbor] = current;
                        gScore[neighbor] = tentative_gScore;
                        float fScore = tentative_gScore + Distance(neighbor, goal); // Celkové skóre = skóre cesty + heuristika
                        if (!openSet.Contains(neighbor))
                        {
                            openSet.Enqueue(neighbor, fScore); // Přidání souseda do otevřeného seznamu
                        }
                    }
                }
            }

            return path; // Pokud není nalezena žádná cesta
        }

        /// <summary>
        /// Vypočítá Euklidovskou vzdálenost mezi dvěma body v rovině.
        /// </summary>
        /// <param name="a">První bod.</param>
        /// <param name="b">Druhý bod.</param>
        /// <returns>Euklidovská vzdálenost mezi body <paramref name="a"/> a <paramref name="b"/>.</returns>
        private float Distance(Point a, Point b)
        {
            return (float)Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2)); // Euklidovská vzdálenost
        }

        /// <summary>
        /// Pohybuje duchem podle algoritmu A*.
        /// </summary>
        /// <param name="ghost">PictureBox reprezentující ducha.</param>
        /// <param name="leftImageIndex">Index obrázku pro pohyb doleva.</param>
        /// <param name="rightImageIndex">Index obrázku pro pohyb doprava.</param>
        /// <param name="upImageIndex">Index obrázku pro pohyb nahoru.</param>
        /// <param name="downImageIndex">Index obrázku pro pohyb dolů.</param>
        private void MoveGhost(PictureBox ghost, int leftImageIndex, int rightImageIndex, int upImageIndex, int downImageIndex)
        {
            Point ghostPos = new Point(ghost.Left, ghost.Top);
            Point playerPos = new Point(player.Left, player.Top);

            List<Point> path = AStar(ghostPos, playerPos);

            if (path.Count > 0)
            {
                Point nextMove = path[0];

                // Provádění kontroly kolize před každým krokem
                if (!CheckWallCollision(nextMove.X, nextMove.Y, ghost.Width, ghost.Height))
                {
                    // Nastavení obrázku podle směru pohybu
                    if (nextMove.X < ghost.Left) // Duch jde doleva
                    {
                        ghost.Image = imageList.Images[leftImageIndex];
                    }
                    else if (nextMove.X > ghost.Left) // Duch jde doprava
                    {
                        ghost.Image = imageList.Images[rightImageIndex];
                    }
                    else if (nextMove.Y < ghost.Top) // Duch jde nahoru
                    {
                        ghost.Image = imageList.Images[upImageIndex];
                    }
                    else if (nextMove.Y > ghost.Top) // Duch jde dolů
                    {
                        ghost.Image = imageList.Images[downImageIndex];
                    }

                    ghost.Left = nextMove.X;
                    ghost.Top = nextMove.Y;
                }
            }

            // Detekce kolize s hráčem
            CheckCollision(ghost);
        }

        /// <summary>
        /// Kontroluje kolizi mezi hráčem a duchem.
        /// </summary>
        /// <param name="ghost">PictureBox reprezentující ducha.</param>
        private void CheckCollision(PictureBox ghost)
        {
            if (player.Bounds.IntersectsWith(ghost.Bounds))
            {
                playerLives--;

                // Odstranění posledního PictureBoxu v seznamu
                if (lifePictureBoxes.Count > 0)
                {
                    PictureBox lifeToRemove = lifePictureBoxes[lifePictureBoxes.Count - 1];
                    this.Controls.Remove(lifeToRemove);
                    lifePictureBoxes.Remove(lifeToRemove);
                }

                // Resetování a spuštění timeru při doteku ducha
                delayTimerOrange.Stop();
                delayTimerOrange.Start();
                delayTimerBlue.Stop();
                delayTimerBlue.Start();
                delayTimerPink.Stop();
                delayTimerPink.Start();

                if (playerLives > 0)
                {
                    RestartGame();
                }
                else
                {
                    EndGame();
                }
            }
        }

        /// <summary>
        /// Restartuje hru a nastavuje počáteční pozice hráče a duchů.
        /// </summary>
        private void RestartGame()
        {
            gameOver = false;


            player.Location = new Point(390, 735);
            ghostRed.Location = new Point(390, 435);
            ghostPink.Location = new Point(420, 435);
            ghostBlue.Location = new Point(450, 435);
            ghostOrange.Location = new Point(360, 435);

            GameOverText.Visible = false;
            NameBox.Visible = false;
            SaveButton.Visible = false;
            mainTimer.Start();
        }

        /// <summary>
        /// Ukončí hru a zobrazí konečné obrazovky.
        /// </summary>
        private void EndGame()
        {
            // Nastaví počet životů na výchozí hodnotu
            playerLives = 3;

            gameOver = true;

            mainTimer.Stop();

            GameOverText.Visible = true;
            NameBox.Visible = true;
            SaveButton.Visible = true;
        }

        /// <summary>
        /// Obsluhuje událost kliknutí na tlačítko uložení.
        /// </summary>
        /// <param name="sender">Odesílatel události.</param>
        /// <param name="e">Argumenty události.</param>
        private void SaveButton_Click(object sender, EventArgs e)
        {
            string name = NameBox.Text.ToString();

            PlayerBoard pb = new PlayerBoard(name, score);
            dao.Save(pb);
            score = 0;
            this.Close();
        }


    }
}