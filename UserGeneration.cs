using System;
using System.Diagnostics;
using Microsoft.Data.Sqlite;

namespace GritClicker
{
    public class UserCreation
    {
        public static void NewUser(User user)
        {
            // Assuming Nuggeteering.connection is a valid, already opened connection.
            SqliteConnection curConn = Nuggeteering.connection;

            // Ensure connection is open
            if (curConn.State != System.Data.ConnectionState.Open)
            {
                curConn.Open();
            }

            // Prepare the SQL command
            SqliteCommand insertCommand = new SqliteCommand(
                "INSERT INTO Users (Nickname, Level, Money, Boost) VALUES (@name, @lvl, @money, @boost)", curConn);

            // Add parameters
            insertCommand.Parameters.Add("@name", SqliteType.Text).Value = user.Name;
            insertCommand.Parameters.Add("@lvl", SqliteType.Integer).Value = user.Level;
            insertCommand.Parameters.Add("@money", SqliteType.Real).Value = user.Money;
            insertCommand.Parameters.Add("@boost", SqliteType.Integer).Value = 1; // Assuming a default value for Boost

            try
            {
                using var transact = curConn.BeginTransaction();
                insertCommand.Transaction = transact;

                // Execute the command
                insertCommand.ExecuteNonQuery();

                // Commit the transaction
                transact.Commit();
            }
            catch (SqliteException e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public static void UpgradeUser(User user, string option)
        {
            // Assuming Nuggeteering.connection is a valid, already opened connection.
            SqliteConnection curConn = Nuggeteering.connection;

            // Ensure connection is open
            if (curConn.State != System.Data.ConnectionState.Open)
            {
                curConn.Open();
            }

            // Prepare the SQL command and add parameters to it
            SqliteCommand insertCommand;
            switch (option)
            {
                case "up":
                    insertCommand = new SqliteCommand(
                "UPDATE Users SET Level = @lvl, Money = @money", curConn);
                    insertCommand.Parameters.Add("@lvl", SqliteType.Integer).Value = user.Level;
                    insertCommand.Parameters.Add("@money", SqliteType.Real).Value = user.Money;
                    break;
                case "exit":
                    insertCommand = new SqliteCommand(
                "UPDATE Users SET Money = @money", curConn);
                    insertCommand.Parameters.Add("@money", SqliteType.Real).Value = user.Money; break;
                default: return;
            }

            try
            {
                using var transact = curConn.BeginTransaction();
                insertCommand.Transaction = transact;

                // Execute the command
                insertCommand.ExecuteNonQuery();

                // Commit the transaction
                transact.Commit();
            }
            catch (SqliteException e)
            {
                Debug.WriteLine(e.Message);
            }
        }

    }
}


public class User
{
    protected internal string Name { get; set; }
    protected internal int Level { get; set; }
    protected internal long Money { get; set; }

    public User(string nickname, int level, long money)
    {
        Name = nickname;
        Level = level;
        Money = money;
    }

}


