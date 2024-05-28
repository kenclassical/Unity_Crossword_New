using UnityEngine;
using MySql.Data.MySqlClient;
using TMPro;
using UnityEngine.SceneManagement;

public class LoginCode : MonoBehaviour
{
    public TMP_InputField Username_Login;
    public TMP_InputField Password_Login;
    public TMP_Text Verify_Login;
    
    private MySqlConnection connection;
    private string connectionString = "Server=192.168.1.163;Database=userandpassword;User=root;Password='';SslMode=none;";
    // private string connectionString = "Server=localhost;Database=userandpassword;User=root;Password='';SslMode=none;";

   public void ButtonClickedLogin(){
        connection = new MySqlConnection(connectionString);
        connection.Open();
        string UsernameRe = Username_Login.text;
        string PasswordRe = Password_Login.text;
        string query = "SELECT * FROM users WHERE username = @UsernameRe AND password = @PasswordRe";
        MySqlCommand cmd = new MySqlCommand(query,connection);
        cmd.Parameters.AddWithValue("@UsernameRe",UsernameRe);
        cmd.Parameters.AddWithValue("@PasswordRe",PasswordRe);
        MySqlDataReader reader = cmd.ExecuteReader();
        if (reader.HasRows){
            Verify_Login.text = "Login successful!";
            Verify_Login.color = Color.green;  // Load another copy to keep the original scene
            PlayerPrefs.SetString("LoggedInUsername", UsernameRe);
            SceneManager.LoadScene("Home");
        }
        else if(string.IsNullOrEmpty(UsernameRe) || string.IsNullOrEmpty(PasswordRe)){
            Verify_Login.text = "Please enter the complete information.";
            Verify_Login.color = Color.red;
        }
        else if(PasswordRe.Length < 8){
            Verify_Login.text = "Enter a password of 8 or more characters.";
            Verify_Login.color = Color.red;
        }
        else
        {
            Verify_Login.text = "Username and Password incorrect";
            Verify_Login.color = Color.red;
        }
        
   }

    public void ButtonshowLogin(){
        if(Password_Login.contentType == TMP_InputField.ContentType.Password){
            Password_Login.contentType = TMP_InputField.ContentType.Standard;
            Password_Login.ForceLabelUpdate();
        }
        else
        {
            Password_Login.contentType = TMP_InputField.ContentType.Password;
            Password_Login.ForceLabelUpdate();
        }
    }
    public void ButtonRegister(){
        SceneManager.LoadScene("Register");
    }
}
