using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCentralRisque
{
    class NewClient
    {
        public static int getNewClient()
        {
            int ncl = 0;
            string selection = "";
            if (File.Exists(@"C:\Nouveau dossier\\NouveauClient.csv"))
            {
                File.Delete(@"C:\Nouveau dossier\\NouveauClient.csv");
            }
            StreamWriter sw = new StreamWriter(@"C:\Nouveau dossier\\NouveauClient.csv", false);
            DataSet dataset = new DataSet();
            SqlConnection con = Connexion.GetDBConnectionCREM();
            con.Open();
            SqlCommand insd = new SqlCommand("use crem select distinct code_client_1 from temp_declarations where code_client_1 not in (select code_client_1 from declarations )", con);
            insd.ExecuteNonQuery();
            using (SqlDataReader reader = insd.ExecuteReader())
            {
                // Check is the reader has any rows at all before starting to read.
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ncl++;
                        selection = selection + ',' + reader["code_client_1"].ToString();
                    }
                }
            }
            con.Close();
            if (selection != "")
            {
                OracleConnection conn = Connexion.GetDBConnectionKSIOP();
                OracleCommand cmd = new OracleCommand();
                conn.Open();
                cmd.CommandText = "select actcode , actnom , actnumrcm from acteur";
                cmd.Connection = conn;
                OracleDataAdapter adapter = new OracleDataAdapter(cmd);

                adapter.Fill(dataset);
            }
            foreach (DataTable dt in dataset.Tables)
            {
                sw.Write("Code Client, Nom Client , Numéro du registre de commerce");
                sw.Write(sw.NewLine);
                foreach (DataRow r in dt.Rows)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        if (!Convert.IsDBNull(r[i]))
                        {
                            string value = r[i].ToString();
                            if (value.Contains(','))
                            {
                                value = String.Format("\"{0}\"", value);
                                sw.Write(value);
                            }
                            else
                            {
                                sw.Write(r[i].ToString());
                            }
                        }
                        if (i < dt.Columns.Count - 1)
                        {
                            sw.Write(",");
                        }
                    }
                    sw.Write(sw.NewLine);
                }
            }
            sw.Close();
            return ncl;
        }
    }
}
