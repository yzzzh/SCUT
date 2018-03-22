using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data; 
using System.Data.SqlClient;

namespace SCUT
{
    class DB : IDisposable
    {
        private SqlConnection sqlConnection;

        public DB()
        {
            sqlConnection = new SqlConnection(@"server=DESKTOP-3UIQV75;database=SCUT;Trusted_Connection=SSPI;");
            sqlConnection.Open();
        }

        public DataTable getBySql(string sql)
        {
            SqlDataAdapter adapter = new SqlDataAdapter(new SqlCommand(sql, sqlConnection));
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            return dataTable;
        }

        public void setBySql(string sql)
        {
            new SqlCommand(sql, sqlConnection).ExecuteNonQuery();
        }

        public void Dispose()
        {
            sqlConnection.Close();
        }
    }
}
