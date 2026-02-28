using System;
using System.Collections;
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
using System.Net.Sockets;


namespace DepoHelper
{
    public partial class FormVagon : Form
    {
        private bool isEditMode = false;
        private int idVagon;
        
        public FormVagon(int idClient)  // constructor pentru ADD
        {
            InitializeComponent();
            LoadClienti();
            isEditMode = false;
            this.Text = "Adaugă Vagon";

            comboBoxC.SelectedValue = idClient;
        }

        public FormVagon(int id, string numarVagon, int idClient)  // constructor pentru EDIT
        {
            InitializeComponent();
            LoadClienti();
            isEditMode = true;
            this.Text = "Modifică Vagon";

            idVagon = id;
            textBoxNV.Text = numarVagon;
            comboBoxC.SelectedValue = idClient;
        }

        private void LoadClienti()
        {
            try
            {
                comboBoxC.DataSource = null;

                using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT idFirma, numeFirma FROM Firme
                        WHERE tipRelatie = 'C'
                    ";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt3 = new DataTable();
                    dt3.Load(reader);

                    comboBoxC.DisplayMember = "numeFirma";
                    comboBoxC.ValueMember = "idFirma";
                    comboBoxC.DataSource = dt3;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la încărcarea Firmelor client: " + ex.Message);
            }
        }

        private void ButtonSave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxNV.Text))
            {
                MessageBox.Show("Completează toate câmpurile obligatorii! (*)");
                return;
            }

            using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
            {
                conn.Open();

                SqlCommand cmd;
                if (isEditMode)
                {
                    // UPDATE pentru edit
                    string query = @"
                        UPDATE Vagoane
                        SET numarVagon = @numarVagon,
                            idProprietar = @idProprietar
                        WHERE idVagon = @idVagon
                        ";

                    cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@idVagon", idVagon); //pentru că doar ăsta are nevoie de el, dar pentru simplitate îl puteam pune și jos.
                }
                else
                {
                    // INSERT pentru add
                    string query = @"
                        INSERT INTO Vagoane (numarVagon, idProprietar)
                        VALUES (@numarVagon, @idProprietar)
                        ";
                    cmd = new SqlCommand(query, conn);
                }

                cmd.Parameters.AddWithValue("@numarVagon", textBoxNV.Text);
                cmd.Parameters.AddWithValue("@idProprietar", comboBoxC.SelectedValue);

                cmd.ExecuteNonQuery();
            }

            this.Close();
        }

        private void FormVagon_Load(object sender, EventArgs e)
        {

        }
    }
}
