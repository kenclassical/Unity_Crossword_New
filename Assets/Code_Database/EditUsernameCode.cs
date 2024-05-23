using UnityEngine;
using MySql.Data.MySqlClient;
using TMPro;
using UnityEngine.SceneManagement;

public class EditUsernameCode : MonoBehaviour
{
    private MySqlConnection connection;
    private string connectionString = "Server=192.168.1.163;Database=userandpassword;User=root;Password='';SslMode=none;";
    public TMP_Text Username_Edit;
    public TMP_Text Password_Edit;
    public TMP_Text Email_Edit;
    // Start is called before the first frame update
    void Start()
    {
        connection = new MySqlConnection(connectionString);
        connection.Open();
        string loggedInUsername = PlayerPrefs.GetString("LoggedInUsername", "Guest");
        string query = "SELECT * FROM users WHERE username = @loggedInUsername";
        MySqlCommand cmd = new MySqlCommand(query,connection);
        cmd.Parameters.AddWithValue("@loggedInUsername",loggedInUsername);
        MySqlDataReader reader = cmd.ExecuteReader();

        if(reader.Read()){
            string Username = reader.GetString("username");
            string Password = reader.GetString("password");
            string Email = reader.GetString("email");
            Username_Edit.text = Username;
            Password_Edit.text = Password;
            Email_Edit.text = Email;
        }

        reader.Close();
    }

    // Update is called once per frame
    public void Back(){
        SceneManager.LoadScene("Home");
    }

    public void Editdata(){
        SceneManager.LoadScene("EditData");
    }

    public void Logout(){
        PlayerPrefs.DeleteKey("LoggedInUsername");
        SceneManager.LoadScene("login");
    }
}
