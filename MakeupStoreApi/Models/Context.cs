using Dapper;
using Microsoft.Data.SqlClient;

namespace MakeupStoreApi.Models
{
    public class Context
    {
        public static string ConnectionString =
            @"Server=(localdb)\MSSQLLocalDB;Database=MakeupStoreDb;Integrated Security=True;TrustServerCertificate=True;";

        // SELECT (List)
        public static IEnumerable<T> GetList<T>(string query, object param = null)
        {
            using (SqlConnection db = new SqlConnection(ConnectionString))
            {
                db.Open();
                return db.Query<T>(query, param);
            }
        }

        // SELECT (Single)
        public static T Get<T>(string query, object param = null)
        {
            using (SqlConnection db = new SqlConnection(ConnectionString))
            {
                db.Open();
                return db.QueryFirstOrDefault<T>(query, param);
            }
        }

        // INSERT - UPDATE - DELETE
        public static void Execute(string query, object param = null)
        {
            using (SqlConnection db = new SqlConnection(ConnectionString))
            {
                db.Open();
                db.Execute(query, param);
            }
        }
    }
}