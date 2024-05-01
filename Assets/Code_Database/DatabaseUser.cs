using UnityEngine;
using System.Collections;
using MySql.Data.MySqlClient;

public class MySqlConnectionTest : MonoBehaviour
{
    private MySqlConnection connection;
    private string connectionString = "Server=localhost;Database=userandpassword;User=root;Password='';SslMode=none;";

    // Start is called before the first frame update
    void Start()
    {
        connection = new MySqlConnection(connectionString);
        try
        {
            connection.Open();
            Debug.Log("Connected to MySQL Server!");

            // Perform database operations here

        }
        catch (MySqlException ex)
        {
            Debug.LogError("Error: " + ex.Message);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
