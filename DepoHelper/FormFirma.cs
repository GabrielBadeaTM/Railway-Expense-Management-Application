using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DepoHelper
{
    public partial class FormFirma : Form
    {
        private bool isEditMode = false;
        private int idFirma, idPersCont, idAdr;

        public FormFirma()
        {
            InitializeComponent();
            isEditMode = false;
            this.Text = "Adaugă Firmă";

            comboBoxTipFirma.Items.Add("Furnizor");
            comboBoxTipFirma.Items.Add("Client");
            comboBoxTipFirma.SelectedIndex = 0;

            LoadJudețe();

            if (comboBoxJudA.Items.Count > 0)
            {
                comboBoxJudA.SelectedIndex = 0;
                int judA = (int)comboBoxJudA.SelectedValue;
                LoadOrase(judA);
            }
        }

        public FormFirma(int id, int idPC, int idA, string numeFirma, string codFiscal, string regCom, string tipRelatie, string numePC, string prenumePC, string tel, string email, int judA, int orasA, string stradaA, string nrA, string codA)
        {
            InitializeComponent();
            isEditMode = true;
            this.Text = "Modifică Firmă";

            comboBoxTipFirma.Items.Add("Furnizor");
            comboBoxTipFirma.Items.Add("Client");

            idFirma = id;
            idPersCont = idPC;
            idAdr = idA;

            textBoxNumeFirma.Text = numeFirma;
            textBoxCodFiscal.Text = codFiscal;
            textBoxRegCom.Text = regCom;

            if (tipRelatie == "F")
                comboBoxTipFirma.SelectedIndex = 0;
            else
                comboBoxTipFirma.SelectedIndex = 1;


            textBoxNumePC.Text = numePC;
            textBoxPrenumePC.Text = prenumePC;
            textBoxTelefonPC.Text = tel;
            textBoxEmailPC.Text = email;

            LoadJudețe();
            comboBoxJudA.SelectedValue = judA;
            LoadOrase(judA);
            comboBoxOrasA.SelectedValue = orasA;

            textBoxStrA.Text = stradaA;
            textBoxNrA.Text = nrA;
            textBoxCodPostA.Text = codA;
        }
        
        //the more you know, asta se apelează după constructor...
        private void FormFirma_Load(object sender, EventArgs e)
        {

        }

        private void Save(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxNumeFirma.Text) ||
                string.IsNullOrWhiteSpace(textBoxCodFiscal.Text) ||
                string.IsNullOrWhiteSpace(textBoxRegCom.Text) ||
                string.IsNullOrWhiteSpace(textBoxNumePC.Text) ||
                string.IsNullOrWhiteSpace(textBoxPrenumePC.Text) ||
                string.IsNullOrWhiteSpace(textBoxTelefonPC.Text) ||
                string.IsNullOrWhiteSpace(textBoxEmailPC.Text) ||
                comboBoxJudA.SelectedValue == null ||
                comboBoxOrasA.SelectedValue == null ||
                string.IsNullOrWhiteSpace(textBoxStrA.Text) ||
                string.IsNullOrWhiteSpace(textBoxNrA.Text))
            {
                MessageBox.Show("Completează toate câmpurile obligatorii! (*)");
                return;
            }

            using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();

                try
                {
                    if (isEditMode)
                    {
                        // UPDATE Firme
                        string updateFirma = @"
                            UPDATE Firme
                            SET numeFirma = @numeFirma,
                                codFiscal = @codFiscal,
                                registruComercial = @regCom,
                                tipRelatie = @tipRelatie
                            WHERE idFirma = @idFirma
                            ";
                        SqlCommand cmd = new SqlCommand(updateFirma, conn, tran);
                        cmd.Parameters.AddWithValue("@numeFirma", textBoxNumeFirma.Text);
                        cmd.Parameters.AddWithValue("@codFiscal", textBoxCodFiscal.Text);
                        cmd.Parameters.AddWithValue("@regCom", textBoxRegCom.Text);
                        cmd.Parameters.AddWithValue("@tipRelatie", comboBoxTipFirma.SelectedIndex == 0 ? 'F' : 'C');
                        cmd.Parameters.AddWithValue("@idFirma", idFirma);
                        cmd.ExecuteNonQuery();

                        // UPDATE PersoaneContact
                        string updatePC = @"
                            UPDATE PersoaneContact
                            SET nume = @numePC,
                                prenume = @prenumePC,
                                telefon = @telefonPC,
                                email = @emailPC
                            WHERE idPersoanaContact = @idPersCont
                            ";
                        cmd = new SqlCommand(updatePC, conn, tran);
                        cmd.Parameters.AddWithValue("@numePC", textBoxNumePC.Text);
                        cmd.Parameters.AddWithValue("@prenumePC", textBoxPrenumePC.Text);
                        cmd.Parameters.AddWithValue("@telefonPC", textBoxTelefonPC.Text);
                        cmd.Parameters.AddWithValue("@emailPC", textBoxEmailPC.Text);
                        cmd.Parameters.AddWithValue("@idPersCont", idPersCont);
                        cmd.ExecuteNonQuery();

                        // UPDATE Adrese
                        string updateAdresa = @"
                            UPDATE Adrese
                            SET idOras = @idOras,
                                strada = @strada,
                                numar = @numar,
                                codPostal = @codPostal
                            WHERE idAdresa = @idAdr
                            ";
                        cmd = new SqlCommand(updateAdresa, conn, tran);
                        cmd.Parameters.AddWithValue("@idOras", comboBoxOrasA.SelectedValue);
                        cmd.Parameters.AddWithValue("@strada", textBoxStrA.Text);
                        cmd.Parameters.AddWithValue("@numar", textBoxNrA.Text);
                        cmd.Parameters.AddWithValue("@codPostal", textBoxCodPostA.Text);
                        cmd.Parameters.AddWithValue("@idAdr", idAdr);
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        // 1. INSERT PersoaneContact și preluare ID
                        string insertPC = @"
                            INSERT INTO PersoaneContact (nume, prenume, telefon, email)
                            VALUES (@numePC, @prenumePC, @telefonPC, @emailPC);
                            SELECT SCOPE_IDENTITY();
                            ";
                        SqlCommand cmd = new SqlCommand(insertPC, conn, tran);
                        cmd.Parameters.AddWithValue("@numePC", textBoxNumePC.Text);
                        cmd.Parameters.AddWithValue("@prenumePC", textBoxPrenumePC.Text);
                        cmd.Parameters.AddWithValue("@telefonPC", textBoxTelefonPC.Text);
                        cmd.Parameters.AddWithValue("@emailPC", textBoxEmailPC.Text);
                        int newIdPC = Convert.ToInt32(cmd.ExecuteScalar());

                        // 2. INSERT Adrese și preluare ID
                        string insertAdresa = @"
                            INSERT INTO Adrese (idOras, strada, numar, codPostal)
                            VALUES (@idOras, @strada, @numar, @codPostal);
                            SELECT SCOPE_IDENTITY();
                            ";
                        cmd = new SqlCommand(insertAdresa, conn, tran);
                        cmd.Parameters.AddWithValue("@idOras", comboBoxOrasA.SelectedValue);
                        cmd.Parameters.AddWithValue("@strada", textBoxStrA.Text);
                        cmd.Parameters.AddWithValue("@numar", textBoxNrA.Text);
                        cmd.Parameters.AddWithValue("@codPostal", textBoxCodPostA.Text);
                        int newIdAdresa = Convert.ToInt32(cmd.ExecuteScalar());

                        // 3. INSERT Firme cu ID-urile obținute
                        string insertFirma = @"
                            INSERT INTO Firme (numeFirma, codFiscal, registruComercial, tipRelatie, idPersoanaContact, idAdresa)
                            VALUES (@numeFirma, @codFiscal, @regCom, @tipRelatie, @idPC, @idAdresa)
                            ";
                        cmd = new SqlCommand(insertFirma, conn, tran);
                        cmd.Parameters.AddWithValue("@numeFirma", textBoxNumeFirma.Text);
                        cmd.Parameters.AddWithValue("@codFiscal", textBoxCodFiscal.Text);
                        cmd.Parameters.AddWithValue("@regCom", textBoxRegCom.Text);
                        cmd.Parameters.AddWithValue("@tipRelatie", comboBoxTipFirma.SelectedIndex == 0 ? 'F' : 'C');
                        cmd.Parameters.AddWithValue("@idPC", newIdPC);
                        cmd.Parameters.AddWithValue("@idAdresa", newIdAdresa);
                        cmd.ExecuteNonQuery();
                    }

                    // Commit tranzacție dacă totul a mers
                    tran.Commit();
                    this.Close();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    MessageBox.Show("Eroare la salvarea firmei: " + ex.Message);
                }
            }

            this.Close();
        }


        private void LoadJudețe()
        {
            try
            {

                using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
                {
                    conn.Open();
                    string query = "SELECT idJudet, numeJudet FROM Judete ORDER BY numeJudet";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    DataTable dt_j = new DataTable();
                    dt_j.Load(reader);

                    comboBoxJudA.DataSource = null;
                    comboBoxJudA.DisplayMember = "numeJudet";
                    comboBoxJudA.ValueMember = "idJudet";
                    comboBoxJudA.DataSource = dt_j;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la încărcarea Judetelor: " + ex.Message);
            }
        }

        private void LoadOrase(int idJudet)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
                {
                    conn.Open();
                    string query = "SELECT idOras, numeOras FROM Orase WHERE idJudet = @idJudet ORDER BY numeOras";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@idJudet", idJudet);
                    SqlDataReader reader = cmd.ExecuteReader();

                    DataTable dt_o = new DataTable();
                    dt_o.Load(reader);

                    comboBoxOrasA.DataSource = null;
                    comboBoxOrasA.DisplayMember = "numeOras";
                    comboBoxOrasA.ValueMember = "idOras";
                    comboBoxOrasA.DataSource = dt_o;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la încărcarea Oraselor: " + ex.Message);
            }
        }

        private void comboBoxJudA_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxJudA.SelectedValue != null) // protecție la null
            {
                int idJudet = (int)comboBoxJudA.SelectedValue;
                LoadOrase(idJudet);
            }
        }


        // to delete later:
        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }
    }
}
