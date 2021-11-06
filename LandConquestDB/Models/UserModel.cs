using LandConquestDB.Entities;
using System;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;

namespace LandConquestDB.Models
{
    public class UserModel
    {
        public static User UserAuthorisation(string login, string pass)
        {
            User user = new User();

            string query = "SELECT * FROM dbo.UserData WHERE user_login = @user_login AND user_pass = @user_pass";

            var command = new SqlCommand(query, DbContext.GetSqlConnection());
            command.Parameters.AddWithValue("@user_login", login);
            command.Parameters.AddWithValue("@user_pass", pass);

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
                reader.Close();
            }
            command.Dispose();
            return user;
        }

        public static int CreateUser(string login, string email, string pass, string userId)
        {
            string userQuery = "INSERT INTO dbo.UserData (user_id,user_login,user_email,user_pass) VALUES (@user_id, @user_login, @user_email, @user_pass)";
            var userCommand = new SqlCommand(userQuery, DbContext.GetSqlConnection());


            userCommand.Parameters.AddWithValue("@user_id", userId);
            userCommand.Parameters.AddWithValue("@user_login", login);
            userCommand.Parameters.AddWithValue("@user_email", email);
            userCommand.Parameters.AddWithValue("@user_pass", pass);

            int userResult = userCommand.ExecuteNonQuery();
            userCommand.Dispose();
            return userResult;
        }

        public static User GetUserInfo(string user_id)
        {
            User user = new User();

            string query = "SELECT * FROM dbo.UserData WHERE user_id = @user_id";

            var command = new SqlCommand(query, DbContext.GetSqlConnection());
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
                reader.Close();
            }
            command.Dispose();
            return user;
        }

        public static void UpdateUserEmail(string userId, string newUserEmail)
        {
            string userQuery = "UPDATE dbo.UserData SET user_email = @user_email WHERE user_id = @user_id";
            var userCommand = new SqlCommand(userQuery, DbContext.GetSqlConnection());


            userCommand.Parameters.AddWithValue("@user_id", userId);
            userCommand.Parameters.AddWithValue("@user_email", newUserEmail);

            userCommand.ExecuteNonQuery();
            userCommand.Dispose();
        }

        public static void UpdateUserPass(string userId, string newUserPass)
        {
            string userQuery = "UPDATE dbo.UserData SET user_pass = @user_pass WHERE user_id = @user_id";
            var userCommand = new SqlCommand(userQuery, DbContext.GetSqlConnection());


            userCommand.Parameters.AddWithValue("@user_id", userId);
            userCommand.Parameters.AddWithValue("@user_pass", newUserPass);

            userCommand.ExecuteNonQuery();
            userCommand.Dispose();
        }

        public static bool CheckLoginExistence(string user_login)
        {
            string query = "SELECT * FROM dbo.UserData WHERE user_login = @user_login";

            var command = new SqlCommand(query, DbContext.GetSqlConnection());
            command.Parameters.AddWithValue("@user_login", user_login);

            using (var reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Close();
                    return false;
                }
                else
                {
                    reader.Close();
                    return true;
                }
            }
        }

        public static bool CheckEmailExistence(string user_email)
        {
            string query = "SELECT * FROM dbo.UserData WHERE user_email = @user_email";

            var command = new SqlCommand(query, DbContext.GetSqlConnection());
            command.Parameters.AddWithValue("@user_email", user_email);

            using (var reader = command.ExecuteReader())
            {
                bool exists = reader.HasRows;
                reader.Close();
                if (exists)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
