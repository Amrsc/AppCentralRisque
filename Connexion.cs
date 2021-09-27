using System.Configuration;
using System.Data.OleDb;
using System.Data.SqlClient;
using Oracle.ManagedDataAccess.Client;

namespace AppCentralRisque
{
    class Connexion
    {
        public static SqlConnection GetDBConnectionCREM()
        {
            string connString = ConfigurationManager.ConnectionStrings["ConnexionCREM"].ConnectionString;
            SqlConnection conn = new SqlConnection(connString);

            return conn;
        }
         public static OracleConnection GetDBConnectionKSIOP()
        {
            string connString = ConfigurationManager.ConnectionStrings["ConnexionKSIOP"].ConnectionString;
            OracleConnection myConnection = new OracleConnection();
            myConnection.ConnectionString = connString;

            return myConnection;
        }
        public static OleDbConnection GetDBCXMLCREM()
        {
            string connString = ConfigurationManager.ConnectionStrings["ConnexionCREMxml"].ConnectionString;
            OleDbConnection conn = new OleDbConnection(connString);

            return conn;
        }
        public static void addtocrem()
        {
            SqlConnection con = GetDBConnectionCREM();
            con.Open();
            string query = "insert into declarations select * from temp_declarations";
            SqlCommand ins = new SqlCommand(query, con);

            string query2 = "insert into garanties select * from tempgaranties";
            SqlCommand ins2 = new SqlCommand(query2, con);

            ins.ExecuteNonQuery();
            ins2.ExecuteNonQuery();
            con.Close();
        }
    }
}
