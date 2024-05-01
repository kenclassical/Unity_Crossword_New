using UnityEngine;
using MySql.Data.MySqlClient;
using TMPro;
using UnityEngine.SceneManagement;

public class registerCode : MonoBehaviour
{
    public TMP_InputField Username_Register;
    public TMP_InputField Password_Register;
    public TMP_InputField Email_Register;
    public TMP_Text verify_Register;
    private MySqlConnection connection;
    private string connectionString = "Server=localhost;Database=userandpassword;User=root;Password='';SslMode=none;";

    public void ButtonClicked(){
        connection = new MySqlConnection(connectionString);
        connection.Open();
        string UsernameRe = Username_Register.text;
        string PasswordRe = Password_Register.text;
        string emailRe = Email_Register.text;
        string verifyQuery = "SELECT * FROM users WHERE username = @UsernameRe";
        MySqlCommand verifyCmd = new MySqlCommand(verifyQuery, connection);
        verifyCmd.Parameters.AddWithValue("@UsernameRe", UsernameRe);
        var reader = verifyCmd.ExecuteReader();
        

        if (string.IsNullOrEmpty(UsernameRe) || string.IsNullOrEmpty(PasswordRe) || string.IsNullOrEmpty(emailRe) ){
            verify_Register.text = "Please enter the complete information.";
            verify_Register.color = Color.red;
            reader.Close();
            connection.Close();
            return;
        }
        else if(PasswordRe.Length < 8){
            verify_Register.text = "Enter a password of 8 or more characters.";
            verify_Register.color = Color.red;
            reader.Close();
            connection.Close();
            return;
        }
        else if(!emailRe.Contains("@")){
            verify_Register.text = "Email Please Enter @";
            verify_Register.color = Color.red;
            reader.Close();
            connection.Close();
            return;
        }
        else if(reader.HasRows){
            verify_Register.text = "This username is already in use.";
            verify_Register.color = Color.red;
            reader.Close();
            connection.Close();
            return; // ออกจากเมท็อดเพื่อไม่ทำคำสั่ง INSERT
        }
        else
        {
            reader.Close(); // ปิด reader ก่อนที่จะใช้ connection สร้างคำสั่งใหม่

            string insertQuery = "INSERT INTO users (username,password,email) VALUES (@UsernameRe,@PasswordRe,@EmailRe)";
            MySqlCommand insertCmd = new MySqlCommand(insertQuery, connection);
            insertCmd.Parameters.AddWithValue("@UsernameRe", UsernameRe);
            insertCmd.Parameters.AddWithValue("@PasswordRe", PasswordRe);
            insertCmd.Parameters.AddWithValue("@EmailRe", emailRe);
            insertCmd.ExecuteNonQuery();
            verify_Register.text = "Registration successful!";
            verify_Register.color = Color.green;
            connection.Close();
            SceneManager.LoadScene("login");
        }
        
    }

    public void Buttonshow(){
        if(Password_Register.contentType == TMP_InputField.ContentType.Password){
            Password_Register.contentType = TMP_InputField.ContentType.Standard;
            Password_Register.ForceLabelUpdate();
        }
        else
        {
            Password_Register.contentType = TMP_InputField.ContentType.Password;
            Password_Register.ForceLabelUpdate();
        }
    }

    public void ButtonLogin(){
        SceneManager.LoadScene("login");
    }
}
