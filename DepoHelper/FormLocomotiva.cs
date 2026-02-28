using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Microsoft.Data.SqlClient;

namespace DepoHelper
{
    public partial class FormLocomotiva : Form
    {
        private bool isEditMode = false;
        private int idLocomotiva;

        public FormLocomotiva()
        {
            InitializeComponent();
            LoadPuncteLucru();
            isEditMode = false;
            this.Text = "Adaugă Locomotivă";

        }

        public FormLocomotiva(int id, string numeLoc, string numarEuro, int idPctL, string serie, string tip, string putere)
        {
            InitializeComponent();
            LoadPuncteLucru();
            isEditMode = true;
            this.Text = "Modifică Locomotivă";

            idLocomotiva = id;

            textBoxNL.Text = numeLoc;
            textBoxNE.Text = numarEuro;
            comboBoxPL.SelectedValue = idPctL;
            textBoxS.Text = serie;
            textBoxT.Text = tip;
            textBoxP.Text = putere;

        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void LoadPuncteLucru()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT _pl.idPunctLucru, _oras.numeOras 
                        FROM PuncteLucru _pl
                        JOIN Orase _oras ON _pl.idOras = _oras.idOras
                        ORDER BY numeOras
                        ";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    DataTable dt_pl = new DataTable();

                    dt_pl.Load(reader);

                    comboBoxPL.DisplayMember = "numeOras";
                    comboBoxPL.ValueMember = "idPunctLucru";
                    comboBoxPL.DataSource = dt_pl;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la încărcarea Punctelor de Lucru: " + ex.Message);
            }

        }

        private void ButtonSave(object sender, EventArgs e)
        {
            // validare
            if (string.IsNullOrWhiteSpace(textBoxNL.Text) ||
                string.IsNullOrWhiteSpace(textBoxT.Text) ||
                string.IsNullOrWhiteSpace(textBoxP.Text)
                )
            {
                MessageBox.Show("Completează toate câmpurile obligatorii!");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
                {
                    conn.Open();

                    SqlCommand cmd;

                    if (isEditMode)
                    {
                        // UPDATE
                        string query = @"
                            UPDATE Locomotive
                            SET numeLocomotiva = @numeLocomotiva,
                                nrEuro = @nrEuro,
                                idPunctLucru = @idPunctLucru,
                                tip = @tip,
                                serie = @serie,
                                putere = @putere
                            WHERE idLocomotiva = @idLocomotiva
                        ";

                        cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@idLocomotiva", idLocomotiva);
                    }
                    else
                    {
                        // INSERT
                        string query = @"
                            INSERT INTO Locomotive 
                                (numeLocomotiva, nrEuro, idPunctLucru, tip, serie, putere)
                            VALUES
                                (@numeLocomotiva, @nrEuro, @idPunctLucru, @tip, @serie, @putere)
                        ";
                        cmd = new SqlCommand(query, conn);
                    }

                    // adaugă parametrii corect
                    cmd.Parameters.AddWithValue("@numeLocomotiva", textBoxNL.Text);
                    cmd.Parameters.AddWithValue("@nrEuro", textBoxNE.Text);
                    cmd.Parameters.AddWithValue("@idPunctLucru", Convert.ToInt32(comboBoxPL.SelectedValue));
                    cmd.Parameters.AddWithValue("@tip", textBoxT.Text);
                    cmd.Parameters.AddWithValue("@serie", textBoxS.Text);
                    cmd.Parameters.AddWithValue("@putere", textBoxP.Text);

                    cmd.ExecuteNonQuery();
                }

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la salvare: " + ex.Message);
            }
        }


        // delete later
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
