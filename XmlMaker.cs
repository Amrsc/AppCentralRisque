using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AppCentralRisque
{
    class XmlMaker
    {
        //;Connect Timeout=200&quot;
        public static bool MakeXML(string dd)
        {
            bool ok = false;
            
            String[] aaaa = dd.Split('/');
            dd = aaaa[2] + '-' + aaaa[1] + '-' + aaaa[0];
            string paa = aaaa[2] + "" + aaaa[1];

            DateTime MaDate = DateTime.Now;
            string type_f = "DCRE";
            //string type_f = "DCCR";
            string C21 = "025";//code Sofinance
            string C22 = "025";//code déclarant sofinance
            string C23 = string.Format("{0:yyyy-MM-ddTHH:mm:ss}", MaDate);//Date creation fichier
            string C24;
            string hseconde = string.Format("{0:HHmmss}", MaDate);
            string dtC24 = string.Format("{0:yyyyMMdd}", MaDate);
            string C25 = "111"; // code banque centrale
                                // string S1 = string.Format("{0:yyyy-MM-dd}", MaDate);//Date déclaration
            string S1 = dd;
            string sSerie = "";
            int nSerie = 0;
            string perioddcl = string.Format("{0:yyyyMM}", MaDate);
            string sdtdcl = string.Format("{0:0:dd-MM-yyyy}", MaDate);
            //DateTime dtdcl = MaDate.Date;
            DateTime dtdcl = Convert.ToDateTime(S1);

            // Definition de la connexion
            OleDbConnection conn = Connexion.GetDBCXMLCREM();
            conn.Open();
            //extraction du numero de serie du fichier
            OleDbCommand CS = conn.CreateCommand();
            CS.CommandText = "select * from sequences_files where type_file='DCRE'";
            //CS.CommandText = "select * from sequences_files where type_file='DCCR'";
            CS.ExecuteNonQuery();
            OleDbDataReader DataReaderS = CS.ExecuteReader();
            while (DataReaderS.Read())
            {
                nSerie = Convert.ToInt16(DataReaderS[1]);
            }
            if (nSerie < 9)
            {
                nSerie = nSerie + 1;
                sSerie = "00" + nSerie;
            }
            else
                if (nSerie < 99)
            {
                nSerie = nSerie + 1;
                sSerie = "0" + nSerie;
            }
            else
                    if (nSerie < 999)
            {
                nSerie = nSerie + 1;
                sSerie = Convert.ToString(nSerie);
            }
            else
                        if (nSerie == 999)
            {
                sSerie = "001";
                nSerie = 1;
            }

            DataReaderS.Close();
            DataReaderS.Dispose();

            //CS.Dispose();

            C24 = dtC24 + sSerie;
            string rep = "C:\\Nouveau dossier\\";
            string nomfile = "CREM." + C21 + "." + C24 + "." + type_f + "." + dtC24 + "." + hseconde + ".xml";
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "\t";
            string chemin = rep + nomfile;
            XmlWriter MyWriter = XmlWriter.Create(chemin, settings);

            MyWriter.WriteStartDocument();
            MyWriter.WriteStartElement("crem");
            MyWriter.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
            MyWriter.WriteAttributeString("c1", "1.0");
            MyWriter.WriteStartElement("c2");//debut c2
            MyWriter.WriteAttributeString("c21", C21);
            MyWriter.WriteAttributeString("c22", C22);
            MyWriter.WriteAttributeString("c23", C23);
            MyWriter.WriteAttributeString("c24", C24);
            MyWriter.WriteAttributeString("c25", C25);
            MyWriter.WriteEndElement();//fin c2

            MyWriter.WriteStartElement("c3");//debut c3

            MyWriter.WriteStartElement("c31");//debut c31
            MyWriter.WriteAttributeString("s1", S1);//date arrete comptable



            //Extraction des données

            OleDbCommand Command = conn.CreateCommand();
            Command.CommandText = "SELECT distinct([code_client_1]), [identifiant_4],[type_id_5] FROM temp_declarations where periode_dcl_2 = '"+paa+"' and type_f_3='" + type_f + "' order by [code_client_1]";
            Command.CommandTimeout = 300000;
            Command.ExecuteNonQuery();
            OleDbDataReader DataReader1 = Command.ExecuteReader();


            string champ = null;
            string codeclt = null;
            string numdos = null;
            Int32 montant = 0;
            int i, iy, iz;
            i = 0;
            iy = 0;
            iz = 0;
            while (DataReader1.Read())
            {
                codeclt = Convert.ToString(DataReader1[0]);

                Console.WriteLine("ligne n°: " + i); i++;
                MyWriter.WriteStartElement("s2");//debut s2 faire pour chaque client

                MyWriter.WriteStartElement("d32");//debut D32
                champ = Convert.ToString(DataReader1[2]);
                MyWriter.WriteAttributeString("xsi", "type", null, champ);
                champ = Convert.ToString(DataReader1[1]);
                MyWriter.WriteString(champ); //ecrire identifiant Nif ou cle onomastique
                MyWriter.WriteEndElement();//fin D32

                MyWriter.WriteStartElement("s11");//debut s11 Liste des credits
                                                  //Lister les credits
                codeclt = Convert.ToString(DataReader1[0]);
                OleDbCommand C2 = conn.CreateCommand();
                C2.CommandText = "SELECT * FROM temp_declarations where periode_dcl_2='"+paa+"' and code_client_1=" + codeclt + " and type_f_3='" + type_f + "' order by num_dos_0";
                C2.ExecuteNonQuery();
                OleDbDataReader DataReader2 = C2.ExecuteReader();
                iy = 0;
                iz = 0;
                while (DataReader2.Read())
                {
                    Console.WriteLine("sous ligne n°: " + iy); iy++;
                    MyWriter.WriteStartElement("s20");//debut s20 pour chaque credit
                                                      //ecrire les attributs de s20
                    champ = Convert.ToString(DataReader2[7]);
                    MyWriter.WriteAttributeString("s101", champ);//Encours
                    champ = Convert.ToString(DataReader2[8]);
                    MyWriter.WriteAttributeString("s102", champ);//Niveau responsabilite
                    champ = Convert.ToString(DataReader2[9]);
                    MyWriter.WriteAttributeString("s103", champ);//Situation credit

                    champ = Convert.ToString(DataReader2[10]);
                    if (champ != "")
                    {
                        MyWriter.WriteAttributeString("s104", champ);//code retard paiement
                    }

                    champ = Convert.ToString(DataReader2[11]);
                    if (champ != "")
                    {
                        MyWriter.WriteAttributeString("s105", champ); //duree initiale
                    }

                    champ = Convert.ToString(DataReader2[12]);
                    if (champ != "")
                    {

                        MyWriter.WriteAttributeString("s106", champ); //duree restante
                    }

                    champ = Convert.ToString(DataReader2[13]);
                    MyWriter.WriteAttributeString("s107", champ); //Type credit
                    champ = Convert.ToString(DataReader2[14]);
                    MyWriter.WriteAttributeString("s108", champ);//Code pays agence declarante

                    champ = Convert.ToString(DataReader2[13]);
                    if (champ == "052")
                    {
                        champ = Convert.ToString(DataReader2[15]);
                        MyWriter.WriteAttributeString("s110", champ);
                    }//Mensualité
                    champ = Convert.ToString(DataReader2[16]);
                    MyWriter.WriteAttributeString("s111", champ);//Devise
                    champ = Convert.ToString(DataReader2[17]);
                    if (champ != "")
                    { MyWriter.WriteAttributeString("s113", champ); }//Rib du client
                    champ = Convert.ToString(DataReader2[18]);
                    MyWriter.WriteAttributeString("s114", champ);//Code agence declarante
                    champ = Convert.ToString(DataReader2[19]);
                    if (champ != "")
                    { MyWriter.WriteAttributeString("s115", champ); }//Activite financee
                    champ = Convert.ToString(DataReader2[20]);
                    MyWriter.WriteAttributeString("s117", champ);//Montant accordé


                    champ = Convert.ToString(DataReader2[21]);
                    if (champ != "")
                    {
                        champ = champ.Replace(',', '.');
                        MyWriter.WriteAttributeString("s118", champ); }//Taux du credit

                    champ = Convert.ToString(DataReader2[13]);
                    // if (champ == "052")
                    // {
                    champ = Convert.ToString(DataReader2[22]);
                    MyWriter.WriteAttributeString("s119", champ);//Cout du credit
                                                                 // }
                    champ = Convert.ToString(DataReader2[23]);
                    if (champ != "")
                    {
                        champ = string.Format("{0:yyyy-MM-dd}", DataReader2[23]);//formater la date
                        MyWriter.WriteAttributeString("s120", champ);//date 1ere echeance impayé
                    }

                    montant = Convert.ToInt32(DataReader2[24]);
                    if (montant >= 0)
                    {
                        champ = Convert.ToString(montant);
                        MyWriter.WriteAttributeString("s121", champ);
                    }//Principal impayé

                    montant = Convert.ToInt32(DataReader2[25]);
                    if (montant >= 0)
                    {
                        champ = Convert.ToString(montant);
                        MyWriter.WriteAttributeString("s122", champ);//Interet impayé
                    }

                    champ = Convert.ToString(DataReader2[26]);
                    if (champ != "")
                    {
                        champ = string.Format("{0:yyyy-MM-dd}", DataReader2[26]);
                        MyWriter.WriteAttributeString("s123", champ); //date rejet
                    }

                    champ = Convert.ToString(DataReader2[27]);
                    if (champ != "")
                    {
                        champ = string.Format("{0:yyyy-MM-dd}", DataReader2[27]);
                        MyWriter.WriteAttributeString("s124", champ); //date octroi credit
                    }

                    champ = Convert.ToString(DataReader2[28]);
                    if (champ != "")
                    {
                        champ = string.Format("{0:yyyy-MM-dd}", DataReader2[28]);
                        MyWriter.WriteAttributeString("s125", champ); //date expiration credit
                    }

                    montant = Convert.ToInt32(DataReader2[29]);
                    if (montant >= 0)
                    {
                        champ = Convert.ToString(montant);
                        MyWriter.WriteAttributeString("s126", champ);//Intéret couru
                    }

                    champ = Convert.ToString(DataReader2[0]);
                    MyWriter.WriteAttributeString("s128", champ);//Num dossier
                                                                 //champ = Convert.ToString(DataReader2[33]);
                                                                 // MyWriter.WriteAttributeString("s129", champ);//Id Plafond
                    montant = Convert.ToInt32(DataReader2[30]);
                    if (montant > 0)
                    {
                        champ = Convert.ToString(montant);
                        MyWriter.WriteAttributeString("s130", champ);
                    }//Nbre écheances impayées
                    champ = Convert.ToString(DataReader2[31]);
                    MyWriter.WriteAttributeString("s131", champ);//Code wilaya beneficiair


                    numdos = Convert.ToString(DataReader2[0]);
                    OleDbCommand C3 = conn.CreateCommand();
                    C3.CommandText = "SELECT * FROM tempgaranties where num_dos=" + numdos;
                    C3.ExecuteNonQuery();
                    OleDbDataReader DataReader3 = C3.ExecuteReader();

                    //Si garanties existent faire
                    if (DataReader3.HasRows)
                    {
                        MyWriter.WriteStartElement("s109");//debut s109 Liste Garanties
                                                           //Lister les garantie
                        while (DataReader3.Read())
                        {
                            Console.WriteLine("sous sous ligne n°: " + iz);
                            MyWriter.WriteStartElement("g");//debut g pour chaque garantie
                                                            //Lister les attributs de la garantie g
                            champ = Convert.ToString(DataReader3[1]);
                            MyWriter.WriteAttributeString("g1", champ);
                            champ = Convert.ToString(DataReader3[3]);
                            MyWriter.WriteAttributeString("g2", champ);
                            MyWriter.WriteEndElement();//Fin g pour chaque garantie
                        }
                        MyWriter.WriteEndElement();//fin s109 Liste  des Garantie
                    }
                    DataReader3.Close();
                    DataReader3.Dispose();
                    //C3.Dispose();
                    C3.CommandText = "SELECT * FROM caracteristiques where num_dos=" + numdos;
                    C3.ExecuteNonQuery();
                    DataReader3 = C3.ExecuteReader();

                    //Si caracteristiques existent faire
                    if (DataReader3.HasRows)
                    {
                        MyWriter.WriteStartElement("s112");//Debut s112 liste des caracteristiques speciales
                                                           //Lister les caracteristiques spéciales
                        while (DataReader3.Read())
                        {
                            MyWriter.WriteStartElement("k");//debut k pour chaque caracteristique
                                                            //Lister les attributs de la caracteristique
                            champ = Convert.ToString(DataReader3[1]);
                            MyWriter.WriteAttributeString("k1", champ);
                            champ = Convert.ToString(DataReader3[2]);
                            MyWriter.WriteAttributeString("k2", champ);
                            MyWriter.WriteEndElement();//Fin k pour chaque caracteristique
                        }
                        MyWriter.WriteEndElement();//Fin S112 Liste des caracteristiques
                    }
                    DataReader3.Close();
                    DataReader3.Dispose();
                    C3.Dispose();
                    MyWriter.WriteEndElement();//fin s20 Pour chaque credit

                }

                DataReader2.Close();
                DataReader2.Dispose();
                C2.Dispose();

                MyWriter.WriteEndElement();//fin s11 Liste des credits du client 
                MyWriter.WriteEndElement();//fin s2 pour chaque client

            }

            DataReader1.Close();
            DataReader1.Dispose();
            Command.Dispose();


            //finaly 
            MyWriter.WriteEndElement();//fin c31
            MyWriter.WriteEndElement();//fin c3

            MyWriter.WriteEndElement();//crem
            MyWriter.WriteEndDocument();

            MyWriter.Flush();
            MyWriter.Close();

            //Mettre à jour la declaration

            CS.CommandText = "UPDATE temp_declarations SET date_dcl_6 ='" + dtdcl + "' where num_file_32='xxxxx'";
            CS.ExecuteNonQuery();

            CS.CommandText = "UPDATE temp_declarations SET periode_dcl_2 ='" + perioddcl + "' where num_file_32='xxxxx'";
            CS.ExecuteNonQuery();

            CS.CommandText = "UPDATE temp_declarations SET num_file_32='" + nomfile + "' where num_file_32='xxxxx'";
            CS.ExecuteNonQuery();
            //MAJ numero sequentiel
            CS.CommandText = "UPDATE sequences_files SET num_seq=" + nSerie + " where type_file='DCRE'";
            //CS.CommandText = "UPDATE sequences_files SET num_seq=" + nSerie + " where type_file='DCCR'";
            CS.ExecuteNonQuery();
            CS.Dispose();


            return ok;
        }
    }
}
