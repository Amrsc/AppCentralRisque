using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCentralRisque
{
    class CessionAdditionnel
    {
        public static DataSet declarationN(string per, string fp , string dd)
        {
            DataSet dt = new DataSet();
            String[] dt1 = per.Split('/');
            string period = dt1[2] + dt1[1];
            DataTable DtN = new DataTable();
            SqlConnection con = Connexion.GetDBConnectionCREM();
            con.Open();
            //string query = "select num_dos_0 from declarations where cede_imp_34='1' and periode_dcl_2='" + period + "' AND NOT EXISTS( select num_dos_0 from temp_declarations where cede_imp_34='1') ";


            string query = "use crem select num_dos_0 from declarations where cede_imp_34='1' and periode_dcl_2='"+period+"' AND num_dos_0  NOT in( select num_dos_0 from temp_declarations where cede_imp_34='1') ";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            string inst = "";
            da.Fill(DtN);
            foreach (DataRow row in DtN.Rows)
            {
                foreach (DataColumn col in DtN.Columns)
                {
                    inst = inst  + row[col] + ",";
                }
            }
            if (inst != "") { 
            inst = inst.Remove(inst.Length - 1,1);
            Console.WriteLine("voici la liste des dossier de crem :"+inst);
            con.Close();
            da.Dispose();
            
            if (inst != "")
            {
                OracleConnection conn = Connexion.GetDBConnectionKSIOP();
                OracleCommand cmd2 = new OracleCommand();
                conn.Open();
                //cmd2.CommandText = "SELECT dos.dosnum as Num_DOS_128,SOF_CR_GET_CODE_CLIENT(dos.dosid,'" + fp + "') as codeClient,SOF_CR_Mois_Annee('" + dd + "') as Periode_DCL,'DCRE' as Type_F,SOF_CR_GET_Identif_CLIENT(dos.dosid,'" + fp + "') as ID_Client_D32,SOF_CR_GET_Type_IdentIFiant(dos.dosid,'" + fp + "') as Type_Id_CLT,'" + fp + "' as Date_Arrete,floor(SOF_CR_GET_ENCOURS(dos.dosid,'" + fp + "')) as encours_s101,SOF_CR_GET_NIV_RESPONSABILITE(dos.dosid) as NIV_RESP_S102,SOF_CR_GET_SITUATION_CREDIT(dosid,'" + fp + "') as Etat_CRE_S103,SOF_CR_GET_TypeRetard(dos.dosid,'" + fp + "') as Type_Retard_S104,SOF_CR_GET_DUREE_INITIALE(dos.dosid) as Duree_INIT_S105,SOF_CR_GET_DUREE_RESTANTE(dos.dosid,'" + fp + "') as Duree_Rest_S106,SOF_CR_GET_Type_CREDIT(dos.dosid) as Type_CRE_S107,'012' as Code_Pays_S108, 0 as Mensualité_S110,dos.devcode as DEVISE_S111,SOF_CR_GET_RIB_CLIENT(dos.dosid,'" + fp + "') as RIB_S113,'00001' as Code_Agence_S114,SOF_CR_GET_NAFCODE(dos.dosid,'" + fp + "') as Activite_S115,floor(SOF_CR_GET_MT_ACCORD(dos.dosid,'" + fp + "')) as Montant_Accord_S117,NVL(SOF_CR_GET_TAUX_DOS(dos.dosid),SOF_GET_TAUX_DOS_V3(dos.dosid)) as Taux_S118,0 as Cout_CRE_S119,SOF_CR_GET_FIRST_ECH_IMP(dos.dosid,'" + fp + "') as Date_First_Imp_S120,floor(SOF_CR_GET_CAPITAL_IMP(dos.dosid,'" + fp + "')) as Capital_Imp_S121,floor(SOF_CR_GET_INT_IMP(dos.dosid,'" + fp + "')) as Int_Imp_S122,NULL as Date_Rejet_S123,SOF_CR_GET_DATE_OCTROI(dos.dosid) as Date_Octroi_S124,SOF_CR_GET_DATE_EXPIR(dos.dosid,'" + fp + "') as Date_Exp_S125,floor(SOF_CR_GET_INT_COURU(dos.dosid,'" + fp + "')) as int_couru_S126,SOF_CR_GET_NBRE_ECH_IMP(dos.dosid,'" + fp + "') as Nbre_Ech_Imp_S130,SOF_CR_GET_CODE_WILAYA(dos.dosid,'" + fp + "') as Wilaya_S131,'xxxxx' as Num_File,NULL as Id_Plafond_s129,'0' as cede_imp FROM dossier dos, dual WHERE dos.dosnum in (" + inst + ") ORDER BY codeclient,dos.dosnum";
                cmd2.CommandText = "SELECT dos.dosnum as Num_DOS_128, SOF_CR_GET_CODE_CLIENT(dos.dosid, '"+fp+"') as codeClient, SOF_CR_Mois_Annee('"+dd+"') as Periode_DCL, 'DCRE' as Type_F, SOF_CR_GET_Identif_CLIENT(dos.dosid, '"+fp+"') as ID_Client_D32, SOF_CR_GET_Type_IdentIFiant(dos.dosid, '"+fp+"') as Type_Id_CLT, '"+fp+"' as Date_Arrete, floor(SOF_CR_GET_ENCOURS(dos.dosid, '"+fp+"')) as encours_s101, SOF_CR_GET_NIV_RESPONSABILITE(dos.dosid) as NIV_RESP_S102, SOF_CR_GET_SITUATION_CREDIT(dosid, '"+fp+"') as Etat_CRE_S103, SOF_CR_GET_TypeRetard(dos.dosid, '"+fp+"') as Type_Retard_S104, SOF_CR_GET_DUREE_INITIALE(dos.dosid) as Duree_INIT_S105, SOF_CR_GET_DUREE_RESTANTE(dos.dosid, '"+fp+"') as Duree_Rest_S106, SOF_CR_GET_Type_CREDIT(dos.dosid) as Type_CRE_S107, '012' as Code_Pays_S108, 0 as Mensualité_S110, dos.devcode as DEVISE_S111, SOF_CR_GET_RIB_CLIENT(dos.dosid, '"+fp+"') as RIB_S113, '00001' as Code_Agence_S114, SOF_CR_GET_NAFCODE(dos.dosid, '"+fp+"') as Activite_S115, floor(SOF_CR_GET_MT_ACCORD(dos.dosid, '"+fp+"')) as Montant_Accord_S117, NVL(SOF_CR_GET_TAUX_DOS(dos.dosid), SOF_GET_TAUX_DOS_V3(dos.dosid)) as Taux_S118, 0 as Cout_CRE_S119, SOF_CR_GET_FIRST_ECH_IMP(dos.dosid, '"+fp+"') as Date_First_Imp_S120, floor(SOF_CR_GET_CAPITAL_IMP(dos.dosid, '"+fp+"')) as Capital_Imp_S121, floor(SOF_CR_GET_INT_IMP(dos.dosid, '"+fp+"')) as Int_Imp_S122, NULL as Date_Rejet_S123, SOF_CR_GET_DATE_OCTROI(dos.dosid) as Date_Octroi_S124, SOF_CR_GET_DATE_EXPIR(dos.dosid, '"+fp+"') as Date_Exp_S125, floor(SOF_CR_GET_INT_COURU(dos.dosid, '"+fp+"')) as int_couru_S126, SOF_CR_GET_NBRE_ECH_IMP(dos.dosid, '"+fp+"') as Nbre_Ech_Imp_S130, SOF_CR_GET_CODE_WILAYA(dos.dosid, '"+fp+"') as Wilaya_S131, 'xxxxx' as Num_File, NULL as Id_Plafond_s129, '0' as cede_imp FROM dossier dos, dual WHERE dos.dosnum in ("+inst+") ORDER BY codeclient,dos.dosnum";

                cmd2.Connection = conn;
                OracleDataAdapter x = new OracleDataAdapter(cmd2);
                x.Fill(dt);
                conn.Close();
            }
            }
            return dt;

        }
        public static List<string> DosCdImpClotCtrl(DataSet ds)
        {
            List<string> erreurs = new List<string>();
            string err = "";
            Form1 f = new Form1();
            foreach (DataTable dt in ds.Tables)
            {
                foreach (DataRow r in dt.Rows)
                {
                    f.SetLabel("Traitement du dossier n° :" + (string)r["NUM_DOS_128"] + " ...");
                    if (r["ID_CLIENT_D32"].ToString() == "NO CLIENT DANS DOS")
                    {
                        err = "Le client dans le dossier n° : " + (string)r["NUM_DOS_128"] + "n'existe pas";
                        Console.WriteLine(err);
                        erreurs.Add(err);
                    }
                    if ((string)r["TYPE_ID_CLT"] == "i1" && r["ID_CLIENT_D32"].ToString().Length != 15 || (string)r["TYPE_ID_CLT"] == "i3" && r["ID_CLIENT_D32"].ToString().Length != 20)
                    {
                        err = "L'identifiant du client dans le dossier n° : " + (string)r["NUM_DOS_128"] + "est erroné";
                        Console.WriteLine(err);
                        erreurs.Add(err);
                    }
                    if (r["RIB_S113"].ToString().Length != 20)
                    {
                        err = "Le RIB du client dans le dossier n° : " + (string)r["NUM_DOS_128"] + "est erroné";
                        Console.WriteLine(err);
                        erreurs.Add(err);
                    }
                    if ((string)r["CEDE_IMP"] != "0")
                    {
                        err = "La valeure de cede impayé dans le dossier n° : " + (string)r["NUM_DOS_128"] + "est erroné";
                        Console.WriteLine(err);
                        erreurs.Add(err);
                    }
                    if ((decimal)r["NBRE_ECH_IMP_S130"] != 0)
                    {
                        err = "Le nombre d'écheances impayés dans le dossier n° : " + (string)r["NUM_DOS_128"] + "est erroné";
                        Console.WriteLine(err);
                        erreurs.Add(err);
                    }
                    if ((decimal)r["INT_COURU_S126"] != 0)
                    {
                        err = "La valeure de l'interet couru dans le dossier n° : " + (string)r["NUM_DOS_128"] + "est erroné";
                        Console.WriteLine(err);
                        erreurs.Add(err);
                    }
                    if ((decimal)r["CAPITAL_IMP_S121"] != -1)
                    {
                        err = "La valeure du capital impayé dans le dossier n° : " + (string)r["NUM_DOS_128"] + "est erroné";
                        Console.WriteLine(err);
                        erreurs.Add(err);
                    }
                    if ((decimal)r["INT_IMP_S122"] != -1)
                    {
                        err = "La valeurs de l'interet impayé dans le dossier n° : " + (string)r["NUM_DOS_128"] + "est erroné";
                        Console.WriteLine(err);
                        erreurs.Add(err);
                    }
                    if ((decimal)r["COUT_CRE_S119"] != 0)
                    {
                        err = "Le cout de créance dans le dossier n° : " + (string)r["NUM_DOS_128"] + "est erroné";
                        Console.WriteLine(err);
                        erreurs.Add(err);
                    }
                    if ((string)r["DUREE_REST_S106"] != "000")
                    {
                        err = "La valeure de la durée restante dans le dossier n° : " + (string)r["NUM_DOS_128"] + "est erroné";
                        Console.WriteLine(err);
                        erreurs.Add(err);
                    }
                    if ((decimal)r["ENCOURS_S101"] != 0)
                    {
                        err = "La valeure de l'encours dans le dossier n° : " + (string)r["NUM_DOS_128"] + "est erroné";
                        Console.WriteLine(err);
                        erreurs.Add(err);
                    }

                }

            }
            return erreurs;
        }
    }
}
