using System;
using System.Data;
using System.Data.SqlClient;

namespace AppCentralRisque
{
    class Persistance
    {
        public static bool persist(DataSet ds)
        {
            bool ok;
            string sql = "";
            SqlConnection con = Connexion.GetDBConnectionCREM();
            con.Open();
            int i = 0;
            foreach(DataTable dt in ds.Tables)
            {
                foreach (DataRow r in dt.Rows)
                {
                    Console.WriteLine("etape :" + i + "commence ");
                    sql = "";
                    foreach (var item in r.ItemArray)
                    {
                        sql = sql + "'" + item + "'" + ',';
                    }
                    sql = sql.Remove(sql.Length - 1, 1);
                    SqlCommand ins = new SqlCommand("use crem insert into temp_declarations values (" + sql + ")", con);
                    ins.ExecuteNonQuery();
                    Console.WriteLine("etape :" + i + "ok ");
                    i++;
                }
            }
            con.Close();
            ok = true;
            return ok;
        }
        public static bool persistG(DataSet ds, string dd)
        {
            bool ok;
            string[] dts = dd.Split('/');
            string periode = dts[2] + dts[1];
            string sql = "";
            SqlConnection con = Connexion.GetDBConnectionCREM();
            con.Open();
            SqlCommand insd = new SqlCommand("use crem delete from tempgaranties", con);
            insd.ExecuteNonQuery();
            int i = 0;
            foreach (DataTable dt in ds.Tables)
            {
                foreach (DataRow r in dt.Rows)
                {
                    Console.WriteLine("etape :" + i + "commence ");

                    sql ="'" + r["DOSNUM"].ToString() + "'" + ','+"'" + r["TYPEG"].ToString() + "'" + ',' + "'" + r["LIBELLEG"].ToString() + "'" + ',' + "'" + r["VALEURG"].ToString() + "'" + ',' + "'" + periode +"'";

                    SqlCommand ins = new SqlCommand("use crem insert into tempgaranties values (" + sql + ")", con);
                    ins.ExecuteNonQuery();
                    Console.WriteLine("etape :" + i + "ok ");
                    i++;
                }
            }

            con.Close();
            ok = true;
            return ok;
        }
    }
}
