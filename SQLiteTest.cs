using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mono.Data.Sqlite;
using System.Data;

using SQLiteHandler;

public class SQLiteTest : MonoBehaviour
{
    private string connectionPath;

    void Awake()
    {
        connectionPath = "URI=file:" + Application.persistentDataPath + "/";
    }

    // Start is called before the first frame update
    void Start()
    {
        IDbConnection dbConnection = DbHandler.CreateAndOpenDatabase(connectionPath);
        Debug.Log("path: " + connectionPath);

        dbConnection.Close();
        dbConnection = null;
    }

    // Update is called once per frame
    void Update()
    {
        // IDbConnection dbConnection = CreateAndOpenDatabase();
        // IDbCommand dbCommand = dbConnection.CreateCommand();

        // dbCommand.CommandText = "SELECT * FROM User";
        // IDataReader reader = dbCommand.ExecuteReader();
        // while (reader.Read())
        // {
        //     string id = reader.GetString(0);
        //     string email = reader.GetString(1);
        //     int active = reader.GetInt32(2);
        //     string role = reader.GetString(3);
        //     Debug.Log("id: " + id + ", email: " + email + ", active: " + active + ", role: " + role);
        // }

        // reader.Close();
        // reader = null;
        // dbCommand.Dispose();
        // dbCommand = null;
        // dbConnection.Close();
        // dbConnection = null;

        // IDbCommand dbCommand = dbConnection.CreateCommand();
        // dbCommand.CommandText = @"INSERT INTO User (id, email, active, role) 
        //     VALUES ('6552fd85b2957a700d64df56',
        //     'luism.sanchezp@autonoma.edu.co',
        //     1,
        //     'admin')";
        // dbCommand.ExecuteReader();

        // dbCommand.Dispose();
        // dbCommand = null;
    }
}
