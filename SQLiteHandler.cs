using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mono.Data.Sqlite;
using System.Data;

namespace SQLiteHandler {

    [System.Serializable]
    public struct UserSchema
    {
        public string id;
        public string names;
        public string lastnames;
        public string avatar;
        public string email;
        public int active;
        public string role;
    }
    
    public static class DbHandler {

        private static string databaseName = "game.db";

        public static IDbConnection CreateAndOpenDatabase(string connectionPath)
        {
            string dbUri = connectionPath + databaseName;
            IDbConnection dbConnection = new SqliteConnection(dbUri);
            dbConnection.Open();

            Debug.Log("Successfully opened a connection to the database.");

            CreateTables(dbConnection);

            return dbConnection;
        }

        public static void CreateTables(IDbConnection dbConnection)
        {
            IDbCommand dbCommandCreateTable = dbConnection.CreateCommand();

            dbCommandCreateTable.CommandText = @"
                CREATE TABLE IF NOT EXISTS User (
                    id VARCHAR(24) PRIMARY KEY,
                    names VARCHAR(255) NOT NULL,
                    lastnames VARCHAR(255) NOT NULL,
                    avatar VARCHAR(255),
                    email VARCHAR(255) NOT NULL,
                    active INTEGER NOT NULL,
                    role VARCHAR(255) NOT NULL
                )";

            dbCommandCreateTable.ExecuteReader();

            Debug.Log("Successfully created the table.");
        }

        public static IDbConnection OpenDatabase(string connectionPath)
        {
            string dbUri = connectionPath + databaseName;
            IDbConnection dbConnection = new SqliteConnection(dbUri);
            dbConnection.Open();

            Debug.Log("Successfully opened a connection to the database.");

            return dbConnection;
        }

        public static void InsertUser (IDbConnection dbConnection, string id, string names, string lastnames, string avatar, string email, int active, string role)
        {
            IDbCommand dbCommand = dbConnection.CreateCommand();

            dbCommand.CommandText = @"INSERT INTO User (id, names, lastnames, avatar, email, active, role) 
                VALUES ('" + id + @"',
                '" + names + @"',
                '" + lastnames + @"',
                '" + avatar + @"',
                '" + email + @"',
                " + active + @",
                '" + role + @"')";

            dbCommand.ExecuteReader();

            Debug.Log("Successfully inserted a new user.");

            dbCommand.Dispose();
            dbCommand = null;
        }

        public static UserSchema getUserById(IDbConnection dbConnection, string id)
        {
            IDbCommand dbCommand = dbConnection.CreateCommand();

            dbCommand.CommandText = "SELECT * FROM User WHERE id = '" + id + "'";

            IDataReader dataReader = dbCommand.ExecuteReader();

            UserSchema user = new UserSchema();

            while (dataReader.Read())
            {
                user.id = dataReader.GetString(0);
                user.names = dataReader.GetString(1);
                user.lastnames = dataReader.GetString(2);
                user.avatar = dataReader.GetString(3);
                user.email = dataReader.GetString(4);
                user.active = dataReader.GetInt32(5);
                user.role = dataReader.GetString(6);
            }

            dataReader.Close();
            dataReader = null;

            dbCommand.Dispose();
            dbCommand = null;

            return user;
        }
    }
}
