using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCentralRisque
{
    class Garanties
    {
        public static DataSet garantis( string dd)
        {
            DataSet ds = new DataSet();
            string date = dd.Replace("/", "");
            OracleConnection con = Connexion.GetDBConnectionKSIOP();
            OracleCommand gdc = new OracleCommand();
            gdc.CommandText = "select d.dosid,d.dosnum,dag.tgacode as TypeG,g.tgalibelle as LibelleG,floor(dag.dagmtassiette) as ValeurG,d.dosnom,SOF_CR_GET_CODE_CLIENT(d.dosid, '"+date+"') as codeClient,dag.dagdtdeb as Debut,dag.dagdtfin as Fin from dosactgarantie dag, dossier d, lantgarantie g where d.dosid = dag.dosid and dag.tgacode = g.tgacode and floor(dag.dagmtassiette)> 0 and sof_phase_dos(dag.dosid,'"+date+"') in ('Engagement', 'Réalisation', 'Exploitation')";
            gdc.Connection = con;
            con.Open();
            OracleDataAdapter x = new OracleDataAdapter(gdc);
            x.Fill(ds);
            con.Close();
            Form1 f = new Form1();
            f.SetLabel("rin");
            return ds;
        }
        public static List<string> GarantieCtrl(DataSet ds)
        {
            List<string> erreurs = new List<string>();
            string err = "";
            Form1 f = new Form1();
            foreach (DataTable dt in ds.Tables)
            {
                foreach (DataRow r in dt.Rows)
                {
                    f.SetLabel("Traitement de la garantie n° :" + (int)r["DOSID"] + " ...");
                    if (r["DOSNUM"].ToString() == "")
                    {
                        err = "Le code dossier n'existe pas";
                        Console.WriteLine(err);
                        erreurs.Add(err);
                    }
                    if ((string)r["TYPEG"] == "")
                    {
                        err = "Le type de garantie est introuvable !!";
                        Console.WriteLine(err);
                        erreurs.Add(err);
                    }
                    if ((decimal) r["VALEURG"] == 0)
                    {
                        err = "La garantie ne contient pas de valeurs !!";
                        Console.WriteLine(err);
                        erreurs.Add(err);
                    }
                    if (r["CODECLIENT"].ToString() == "")
                    {
                        err = "Le code client est introuvable !!";
                        Console.WriteLine(err);
                        erreurs.Add(err);
                    }
                }

            }
            return erreurs;
        }
    }
}
