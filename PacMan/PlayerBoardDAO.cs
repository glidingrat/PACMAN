using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacMan
{
    public class PlayerBoardDAO : IDAO<PlayerBoard>
    {
        /// <summary>
        /// Uloží záznam hráče do tabulky Leaderboard v databázi.
        /// </summary>
        /// <param name="leaderboardEntry">Objekt PlayerBoard obsahující jméno a skóre hráče.</param>
        public void Save(PlayerBoard leaderboardEntry)
        {
            SqlConnection conn = DatabaseSingleton.GetInstance();

            SqlCommand command = null;

                using (command = new SqlCommand("INSERT INTO Leaderboard (Name, Score) VALUES (@Name, @Score)", conn))
                {
                    command.Parameters.Add(new SqlParameter("@Name", leaderboardEntry.Name));
                    command.Parameters.Add(new SqlParameter("@Score", leaderboardEntry.Score));
                    command.ExecuteNonQuery();
                    command.CommandText = "Select @@Identity";
                    leaderboardEntry.ID = Convert.ToInt32(command.ExecuteScalar());
                }
        }

        /// <summary>
        /// Načte všechny záznamy z tabulky Leaderboard z databáze, seřazené podle skóre sestupně.
        /// </summary>
        /// <returns>Výčet všech záznamů hráčů jako sekvence objektů PlayerBoard (IEnumerable&lt;PlayerBoard&gt;).</returns>
        public IEnumerable<PlayerBoard> GetAll()
        {
            SqlConnection conn = DatabaseSingleton.GetInstance();

            using (SqlCommand command = new SqlCommand("SELECT * FROM Leaderboard ORDER BY score DESC", conn))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        PlayerBoard leaderboardEntry = new PlayerBoard(
                            Convert.ToInt32(reader[0].ToString()),
                            reader[1].ToString(),
                            Convert.ToInt32(reader[2].ToString())
                        );
                        yield return leaderboardEntry;
                    }
                }
            }
        }

    }
}
