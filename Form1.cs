using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace AppCentralRisque
{
    public partial class Form1 : Form
    {
        DossierCloturer cdc = new DossierCloturer();
        public Form1()
        {
            InitializeComponent();
        }

        public Cursor No { get; private set; }

        public void SetLabel(string newText)
        {
            label1.Text = newText;
            label1.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //step 1 initialize variables
            BtnXml.Cursor = No;
            string dp, dd, fp;
            dd = dateTimePicker1.Text;
            dp = dateTimePicker2.Text;
            fp = dateTimePicker3.Text;
            label1.Text ="Initialisation et démmarage du programme d'extraction de données ...";
            label1.Refresh();
            DossierEngagEtExploit.clean();
            string mdg = "";
            bool ok = false;
            DataSet table = new DataSet();
            List<string> s = new List<string>();
            progressBar1.Visible = true;
            progressBar1.Value = 0;
            progressBar1.Refresh();
            progressBar1.Maximum = 18;
            progressBar1.Minimum = 0;
            progressBar1.Step = 1;
            //before step 2 process engage 
            label1.Text = "Récupération des dossiers en phase engagement & exploitation ... ";
            label1.Refresh();
            table = DossierEngagEtExploit.Getbdd( fp, dd);
            label1.Text = "Contrôle des données des dossiers en phase engagement & exploitation ... ";
            label1.Refresh();
            progressBar1.PerformStep();
            progressBar1.Refresh();
            Refresh();
            s = DossierEngagEtExploit.AfficherErreur(table);
            progressBar1.PerformStep();
            progressBar1.Refresh();
            Refresh();
            if (s.Count == 0)
            {
                ok = Persistance.persist(table);
                progressBar1.PerformStep();
                progressBar1.Refresh();
                Refresh();
                if (ok)
                {
                    label1.Text = "Dossiers clôturers enregistrer avec succes !! ";
                    label1.Refresh();
                }
            }
            else
            {
                foreach (var item in s)
                {
                    mdg = mdg + item + "-";
                }
                label3.Text = mdg;
                label3.Refresh();
                MessageBox.Show("veuillez connriger vos erreurs !!!");
                MessageBox.Show(mdg);
                this.Close();
            }
            //step 2 process dossier cloturés
            label1.Text = "Récupération des dossiers clôturés ... ";
            label1.Refresh();
            table = DossierCloturer.DosClot(dp, fp, dd);
            label1.Text = "Contrôle des données des dossiers clôturés ... ";
            label1.Refresh();
            progressBar1.PerformStep();
            progressBar1.Refresh();
            Refresh();
            s = DossierCloturer.DosClotCtrl(table);
            progressBar1.PerformStep();
            progressBar1.Refresh();
            Refresh();
            if (s.Count == 0)
            {
                ok = Persistance.persist(table);
                progressBar1.PerformStep();
                progressBar1.Refresh();
                Refresh();
                if (ok)
                {
                    label1.Text = "Dossiers clôturers enregistrer avec succes !! ";
                    label1.Refresh();
                }
            }
            else
            {
                foreach (var item in s)
                {
                    mdg = mdg + item + "-";
                }
                label3.Text = mdg;
                label3.Refresh();

                MessageBox.Show("veuillez connriger vos erreurs !!!");
                MessageBox.Show(mdg);
                this.Close();
            }
            //step 3 process dossier cloturés avec impayé
            progressBar1.PerformStep();
            progressBar1.Refresh();
            Refresh();
            label1.Text = "Récupération des dossiers clôturés avec impayé ... ";
            label1.Refresh();
            table = DosCedeImp.DosClotImp(fp, dd);
            progressBar1.PerformStep();
            progressBar1.Refresh();
            Refresh();
            label1.Text = "Contrôle des données des dossier clôturés avec impayé ... ";
            label1.Refresh();
            s = DosCedeImp.DosClotImpCtrl(table);
            if (s.Count == 0)
            {
                progressBar1.PerformStep();
                progressBar1.Refresh();
                Refresh();
                ok = Persistance.persist(table);
                if (ok)
                {
                    label1.Text = "Donnée enregistrer avec succes !! ";
                    label1.Refresh();
                }
            }
            else
            {
                foreach (var item in s)
                {
                    mdg = mdg + item + "-";
                }
                label1.Text = mdg;
                label1.Refresh();
                MessageBox.Show("veuillez connriger vos erreurs !!!");
                MessageBox.Show(mdg);
                this.Close();
            }
            //step 4 get garanties
            progressBar1.PerformStep();
            progressBar1.Refresh();
            Refresh();
            label1.Text = "Récupération des garanties ... ";
            label1.Refresh();
            table = Garanties.garantis(dd);
            progressBar1.PerformStep();
            progressBar1.Refresh();
            Refresh();
            label1.Text = "Contrôle des données des garanties ... ";
            label1.Refresh();
            s = Garanties.GarantieCtrl(table);
            if (s.Count == 0)
            {
                progressBar1.PerformStep();
                progressBar1.Refresh();
                Refresh();
                ok = Persistance.persistG(table , dd);
                if (ok)
                {
                    label1.Text = "Donnée enregistrer avec succes !! ";
                    label1.Refresh();
                }
            }
            else
            {
                foreach (var item in s)
                {
                    mdg = mdg + item + "-";
                }
                label1.Text = mdg;
                label1.Refresh();
                MessageBox.Show("veuillez connriger vos erreurs !!!");
                MessageBox.Show(mdg);
                this.Close();
            }
            //step 4.1 get dossier Cession additionnel
            progressBar1.PerformStep();
            progressBar1.Refresh();
            Refresh();
            label1.Text = "Récupération des dossier Cession Additionnelle ... ";
            label1.Refresh();
            table = CessionAdditionnel.declarationN(dp,fp,dd);
            progressBar1.PerformStep();
            progressBar1.Refresh();
            Refresh();
            label1.Text = "Contrôle des données des Dossier cession Additionnelle ... ";
            label1.Refresh();
            s = CessionAdditionnel.DosCdImpClotCtrl(table);
            if (s.Count == 0)
            {
                progressBar1.PerformStep();
                progressBar1.Refresh();
                Refresh();
                ok = Persistance.persist(table);
                if (ok)
                {
                    label1.Text = "Donnée enregistrer avec succes !! ";
                    label1.Refresh();
                }
            }
            else
            {
                foreach (var item in s)
                {
                    mdg = mdg + item + "-";
                }
                label1.Text = mdg;
                label1.Refresh();
                MessageBox.Show("veuillez connriger vos erreurs !!!");
                MessageBox.Show(mdg);
                this.Close();
            }
            
            //step 5 make xml
            label1.Text = "Ecriture du fichier XML ... ";
            label1.Refresh(); 
            progressBar1.PerformStep();
            progressBar1.Refresh();
            Refresh();
             XmlMaker.MakeXML(dd);
            progressBar1.PerformStep();
            progressBar1.Refresh();
            Refresh();
            label1.Text = "Déclaration générer ... ";
            label1.Refresh();
            Connexion.addtocrem();
            progressBar1.PerformStep();
            progressBar1.Refresh();
            int nbrncl = NewClient.getNewClient();
            MessageBox.Show("Le nombre de nouveau client trouvé est :" + nbrncl);
            MessageBox.Show("Opération terminer !!"); 
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            InitializeComponent();
        }

        private void label1_Click_2(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
