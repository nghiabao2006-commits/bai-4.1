using System.Data.SqlClient;

namespace bai_4._1
{
    public static class AuthService
    {
        // Tạo tài khoản mẫu admin / 123456 nếu bảng đang trống
        public static void EnsureDefaultUser()
        {
            using (var c = Database.Open())
            using (var cmd = new SqlCommand("SELECT COUNT(*) FROM Users", c))
            {
                if ((int)cmd.ExecuteScalar() == 0)
                    Register("admin", "123456");
            }
        }

        public static bool Register(string username, string password)
        {
            string salt = PasswordHelper.NewSalt();
            string hash = PasswordHelper.Hash(password, salt);

            using (var c = Database.Open())
            using (var cmd = new SqlCommand(
                "INSERT INTO Users (Username, PasswordHash, Salt) VALUES (@u,@h,@s)", c))
            {
                cmd.Parameters.AddWithValue("@u", username);
                cmd.Parameters.AddWithValue("@h", hash);
                cmd.Parameters.AddWithValue("@s", salt);
                try { return cmd.ExecuteNonQuery() > 0; }
                catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
                { return false; } 
            }
        }

        public static bool Login(string username, string password)
        {
            using (var c = Database.Open())
            using (var cmd = new SqlCommand(
                "SELECT PasswordHash, Salt FROM Users WHERE Username=@u", c))
            {
                cmd.Parameters.AddWithValue("@u", username);
                using (var rd = cmd.ExecuteReader())
                {
                    if (!rd.Read()) return false;
                    string hash = rd.GetString(0);
                    string salt = rd.GetString(1);
                    return PasswordHelper.Hash(password, salt) == hash;
                }
            }
        }
    }
}