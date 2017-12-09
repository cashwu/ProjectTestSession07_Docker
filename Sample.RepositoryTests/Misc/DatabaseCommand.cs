using System;
using System.Data.SqlClient;
using System.Text;
using Dapper;

namespace Sample.RepositoryTests.Misc
{
    public class DatabaseCommand
    {
        /// <summary>
        /// 建立 Database.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="database">The database.</param>
        public static void CreateDatabase(string connectionString, string database)
        {
            var exists = DatabaseExists(connectionString, database);
            if (exists)
            {
                return;
            }

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var sqlCommand = $"CREATE DATABASE [{database}];";
                conn.Execute(sqlCommand);
            }
        }

        /// <summary>
        /// 檢查指定的 Database 是否存在.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="database">The database.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private static bool DatabaseExists(string connectionString, string database)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var sqlCommand = new StringBuilder();
                sqlCommand.AppendLine($"if exists(select * from sys.databases where name = '{database}')");
                sqlCommand.AppendLine("select 'true'");
                sqlCommand.AppendLine("else ");
                sqlCommand.AppendLine("select 'false'");

                var result = conn.QueryFirstOrDefault<string>(sqlCommand.ToString());
                return result.Equals("true", StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}