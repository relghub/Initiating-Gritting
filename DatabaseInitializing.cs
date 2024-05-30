using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace GritClicker
{
    public class DBInitializing
    {
        public static void Init(SqliteConnection connection)
        {

            try
            {
                connection.Open();
                Console.WriteLine("З'єднано.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Не вдалося з'єднатися: {ex.Message}");
            }

            SqliteCommand checkCommand = new("SELECT ID FROM Users", connection);
            using (SqliteDataReader reader = checkCommand.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    Console.WriteLine("Користувач наявний.");
                }
                else
                {
                    UserCreation.NewUser(new User("Default", 1, 0));
                    Console.WriteLine("Користувач відсутній. Створено стандартний профіль.");
                }
            }
        }
        public static List<string> GameDataRetrieve(SqliteConnection conn)
        {
            List<string> users = new();
            try
            {
                conn.Open();
                Console.WriteLine("З'єднано.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Не вдалося з'єднатися: {ex.Message}");
            }
            SqliteCommand retrieveCommand = new("SELECT u.Nickname, u.Level, u.Money, u.Boost, p.Upgrade_cost, p.Location FROM Users u INNER JOIN Players p ON u.Level = p.Level WHERE u.ID = 1", conn);
            using (SqliteDataReader reader = retrieveCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    users.Add(reader[0].ToString());
                    users.Add(reader[1].ToString());
                    users.Add(reader[2].ToString());
                    users.Add(reader[3].ToString());
                    users.Add(reader[4].ToString());
                    users.Add(reader[5].ToString());
                }
            }
            return users;
        }
    }
}