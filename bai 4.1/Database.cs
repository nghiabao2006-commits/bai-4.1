using System.Data.SqlClient;

namespace bai_4._1
{
    public static class Database
    {
        private const string Server = @"DESKTOP-KMNS09Q\MSSQLSERVER03";
        private const string DbName = "Bai41DB";

        private static readonly string MasterConn =
            "Server=" + Server + ";Database=master;Integrated Security=True;";
        public static readonly string Conn =
            "Server=" + Server + ";Database=" + DbName + ";Integrated Security=True;";

        public static SqlConnection Open()
        {
            var c = new SqlConnection(Conn);
            c.Open();
            return c;
        }

        public static void Init()
        {
            using (var c = new SqlConnection(MasterConn))
            {
                c.Open();
                using (var cmd = new SqlCommand(
                    "IF DB_ID(N'" + DbName + "') IS NULL CREATE DATABASE " + DbName, c))
                    cmd.ExecuteNonQuery();
            }

            using (var c = Open())
            {
                using (var cmd = new SqlCommand(@"
                    IF OBJECT_ID(N'dbo.Users', N'U') IS NULL
                    CREATE TABLE dbo.Users (
                        Id           INT IDENTITY(1,1) PRIMARY KEY,
                        Username     NVARCHAR(50)  NOT NULL UNIQUE,
                        PasswordHash NVARCHAR(200) NOT NULL,
                        Salt         NVARCHAR(100) NOT NULL,
                        CreatedAt    DATETIME      NOT NULL DEFAULT GETDATE()
                    )", c))
                    cmd.ExecuteNonQuery();
            }

            AuthService.EnsureDefaultUser();
        }
    }
}