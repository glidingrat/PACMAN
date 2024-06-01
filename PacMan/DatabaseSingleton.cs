using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacMan
{
    public class DatabaseSingleton
    {

        private static SqlConnection? conn;
        /// <summary>
        /// Vrací instanci SqlConnection. Pokud připojení ještě neexistuje, vytvoří nové připojení 
        /// s použitím konfiguračních údajů pro připojení k databázi.
        /// </summary>
        /// <returns>Instance SqlConnection.</returns>
        public static SqlConnection GetInstance()
        {
            if (conn == null)
            {
                SqlConnectionStringBuilder consStringBuilder = new SqlConnectionStringBuilder();
                consStringBuilder.UserID = ReadSetting("Name");
                consStringBuilder.Password = ReadSetting("Password");
                consStringBuilder.InitialCatalog = ReadSetting("Database");
                consStringBuilder.DataSource = ReadSetting("DataSource");
                consStringBuilder.ConnectTimeout = 30;
                conn = new SqlConnection(consStringBuilder.ConnectionString);

                conn.Open();
            }
            return conn;
        }

        /// <summary>
        /// Načte hodnotu z konfigurace aplikace podle zadaného klíče.
        /// </summary>
        /// <param name="key">Klíč nastavení, jehož hodnotu chcete načíst.</param>
        /// <returns>Hodnota nastavení odpovídající zadanému klíči, nebo "Not Found", pokud není klíč nalezen.</returns>
        private static string ReadSetting(string key)
        {
            var appSettings = ConfigurationManager.AppSettings;
            string result = appSettings[key] ?? "Not Found";
            return result;
        }
    }
}
