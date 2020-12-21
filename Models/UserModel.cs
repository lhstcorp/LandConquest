using LandConquest.Entities;
using System;
using System.Data.SqlClient;


namespace LandConquest.Models
{
    public class UserModel
    {
        public User UserAuthorisation(AuthorisationWindow window, SqlConnection connection)
        {
            User user = new User();

            String query = "SELECT * FROM dbo.UserData WHERE user_login = @user_login AND user_pass = @user_pass";

            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@user_login", window.textBoxLogin.Text);
            command.Parameters.AddWithValue("@user_pass", window.textBoxPass.Password);

            using (var reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    var userId = reader.GetOrdinal("user_id");
                    var userLogin = reader.GetOrdinal("user_login");
                    var userEmail = reader.GetOrdinal("user_email");
                    var userPass = reader.GetOrdinal("user_pass");
                    while (reader.Read())
                    {
                        user.UserId = reader.GetString(userId);
                        user.UserLogin = reader.GetString(userLogin);
                        user.UserEmail = reader.GetString(userEmail);
                        user.UserPass = reader.GetString(userPass);

                    }
                }
            }
            return user;
        }

        public int CreateUser(AuthorisationWindow window, SqlConnection connection, string userId)
        {
            String userQuery = "INSERT INTO dbo.UserData (user_id,user_login,user_email,user_pass) VALUES (@user_id, @user_login, @user_email, @user_pass)";
            var userCommand = new SqlCommand(userQuery, connection);


            userCommand.Parameters.AddWithValue("@user_id", userId);
            userCommand.Parameters.AddWithValue("@user_login", window.textBoxNewLogin.Text);
            userCommand.Parameters.AddWithValue("@user_email", window.textBoxNewEmail.Text);
            userCommand.Parameters.AddWithValue("@user_pass", window.textBoxNewPass.Text);

            int userResult = userCommand.ExecuteNonQuery();
            return userResult;
        }

        public User GetUserInfo(string user_id, SqlConnection connection)
        {
            User user = new User();

            String query = "SELECT * FROM dbo.UserData WHERE user_id = @user_id";

            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@user_id", user_id);

            using (var reader = command.ExecuteReader())
            {
                var userId = reader.GetOrdinal("user_id");
                var userLogin = reader.GetOrdinal("user_login");
                var userEmail = reader.GetOrdinal("user_email");
                var userPass = reader.GetOrdinal("user_pass");
                while (reader.Read())
                {
                    user.UserId = reader.GetString(userId);
                    user.UserLogin = reader.GetString(userLogin);
                    user.UserEmail = reader.GetString(userEmail);
                    user.UserPass = reader.GetString(userPass);
                }
            }
            return user;
        }

        public void UpdateUserEmail(SqlConnection connection, string userId, string newUserEmail)
        {
            String userQuery = "UPDATE dbo.UserData SET user_email = @user_email WHERE user_id = @user_id";
            var userCommand = new SqlCommand(userQuery, connection);


            userCommand.Parameters.AddWithValue("@user_id", userId);
            userCommand.Parameters.AddWithValue("@user_email", newUserEmail);

            userCommand.ExecuteNonQuery();
        }

        public void UpdateUserPass(SqlConnection connection, string userId, string newUserPass)
        {
            String userQuery = "UPDATE dbo.UserData SET user_pass = @user_pass WHERE user_id = @user_id";
            var userCommand = new SqlCommand(userQuery, connection);


            userCommand.Parameters.AddWithValue("@user_id", userId);
            userCommand.Parameters.AddWithValue("@user_pass", newUserPass);

            userCommand.ExecuteNonQuery();
        }

        public bool ValidateUserByLogin(string user_login, SqlConnection connection)
        {
            String query = "SELECT * FROM dbo.UserData WHERE user_login = @user_login";

            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@user_login", user_login);

            using (var reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public bool ValidateUserByEmail(string user_email, SqlConnection connection)
        {
            String query = "SELECT * FROM dbo.UserData WHERE user_email = @user_email";

            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@user_email", user_email);

            using (var reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }
}
