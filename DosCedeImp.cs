using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace AppCentralRisque
{
    class DosCedeImp
    {
        public static DataSet DosClotImp(string fp, string dd)
        {
            DataSet dt = new DataSet();
            OracleConnection con = Connexion.GetDBConnectionKSIOP();
            OracleCommand gdc = new OracleCommand();
            gdc.CommandText = "SELECT dos.dosnum as Num_DOS_128, SOF_CR_GET_CODE_CLIENT(dos.dosid, '"+fp+"') as codeClient, SOF_CR_Mois_Annee('"+dd+"') as Periode_DCL, 'DCRE' as Type_F, SOF_CR_GET_Identif_CLIENT(dos.dosid, '"+fp+"') as ID_Client_D32, SOF_CR_GET_Type_IdentIFiant(dos.dosid, '"+fp+"') as Type_Id_CLT, '"+fp+"' as Date_Arrete, floor(SOF_CR_GET_ENCOURS(dos.dosid, '"+fp+"')) as encours_s101, SOF_CR_GET_NIV_RESPONSABILITE(dos.dosid) as NIV_RESP_S102, SOF_CR_GET_SITUATION_CREDIT(dosid, '"+fp+"') as Etat_CRE_S103, SOF_CR_GET_TypeRetard(dos.dosid, '"+fp+"') as Type_Retard_S104, SOF_CR_GET_DUREE_INITIALE(dos.dosid) as Duree_INIT_S105, SOF_CR_GET_DUREE_RESTANTE(dos.dosid, '"+fp+"') as Duree_Rest_S106, SOF_CR_GET_Type_CREDIT(dos.dosid) as Type_CRE_S107, '012' as Code_Pays_S108, 0 as Mensualité_S110, dos.devcode as DEVISE_S111, SOF_CR_GET_RIB_CLIENT(dos.dosid, '"+fp+"') as RIB_S113, '00001' as Code_Agence_S114, SOF_CR_GET_NAFCODE(dos.dosid, '"+fp+"') as Activite_S115, floor(SOF_CR_GET_MT_ACCORD(dos.dosid, '"+fp+"')) as Montant_Accord_S117, NVL(SOF_CR_GET_TAUX_DOS(dos.dosid), SOF_GET_TAUX_DOS_V3(dos.dosid)) as Taux_S118, 0 as Cout_CRE_S119, SOF_CR_GET_FIRST_ECH_IMP(dos.dosid, '"+fp+"') as Date_First_Imp_S120, floor(SOF_CR_GET_CAPITAL_IMP(dos.dosid, '"+fp+"')) as Capital_Imp_S121, floor(SOF_CR_GET_INT_IMP(dos.dosid, '"+fp+"')) as Int_Imp_S122, NULL as Date_Rejet_S123, SOF_CR_GET_DATE_OCTROI(dos.dosid) as Date_Octroi_S124, SOF_CR_GET_DATE_EXPIR(dos.dosid, '"+fp+"') as Date_Exp_S125, floor(SOF_CR_GET_INT_COURU(dos.dosid, '"+fp+"')) as int_couru_S126, SOF_CR_GET_NBRE_ECH_IMP(dos.dosid, '"+fp+"') as Nbre_Ech_Imp_S130, SOF_CR_GET_CODE_WILAYA(dos.dosid, '"+fp+"') as Wilaya_S131, 'xxxxx' as Num_File, NULL as Id_Plafond_s129, '1' as cede_imp FROM dossier dos, dual WHERE sof_phase_dos(dos.dosid, '"+fp+"') = 'Terminé' and SOF_CR_Mois_Annee(SOF_CR_GET_DATE_CLOTURE(dos.dosid))<= SOF_CR_Mois_Annee('"+fp+"') and(SOF_CR_GET_TypeRetard(DosId, '"+fp+"') is not null AND SOF_CR_GET_TypeRetard(DosId, '"+fp+"') <> '100') and(sof_CR_GET_DATE_sans_suite(dos.dosid) is null or sof_CR_GET_DATE_sans_suite(dos.dosid) > '"+fp+"') and dos.dosnum not in (2012060008, 2012030002, 2014030017, 2014030018, 2014050001) ORDER BY codeclient,dos.dosnum";
            //gdc.CommandText = "select * from dossier";
            gdc.Connection = con;
            con.Open();
            OracleDataAdapter x = new OracleDataAdapter(gdc);
            x.Fill(dt);
            con.Close();
            Form1 f = new Form1();
            f.SetLabel("rin");
            return dt;
        }
        public static List<string> DosClotImpCtrl(DataSet ds)
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
                        err = "Le client dans le dossier n° : " + (string)r["NUM_DOS_128"] + "n'existe pas !!";
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
                    if ((string)r["CEDE_IMP"] == "0")
                    {
                        err = "La valeure de cede impayé dans le dossier n° : " + (string)r["NUM_DOS_128"] + "est erroné";
                        Console.WriteLine(err);
                        erreurs.Add(err);
                    }
                    if ((decimal)r["NBRE_ECH_IMP_S130"] == 0)
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
                    if ((decimal)r["CAPITAL_IMP_S121"] == -1)
                    {
                        err = "La valeure du capital impayé dans le dossier n° : " + (string)r["NUM_DOS_128"] + "est erroné";
                        Console.WriteLine(err);
                        erreurs.Add(err);
                    }
                    if ((decimal)r["INT_IMP_S122"] == -1)
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
                    /*if ((string)r["DUREE_REST_S106"] != "000")
                    {
                        err = "La valeure de la durée restante dans le dossier n° : " + (string)r["NUM_DOS_128"] + "est erroné";
                        Console.WriteLine(err);
                        erreurs.Add(err);
                    }*/
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
