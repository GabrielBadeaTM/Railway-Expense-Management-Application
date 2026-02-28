using System;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;


namespace DepoHelper
{
    public partial class FormPiesa : Form
    {
        private bool isEditMode = false;
        private int idPiesa;
        
        public FormPiesa()  // constructor pentru ADD
        {
            InitializeComponent();
            isEditMode = false;
            this.Text = "Adaugă Piesă";
        }

        public FormPiesa(int id, string denumire, string cod, string unitate)  // constructor pentru EDIT
        {
            InitializeComponent();
            isEditMode = true;
            this.Text = "Modifică Piesă";

            idPiesa = id;
            textBoxDen.Text = denumire;
            textBoxCod.Text = cod;
            textBoxUm.Text = unitate;
        }

        private void btnSalveaza_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxDen.Text))
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
                        UPDATE Piese
                        SET denumirePiesa = @denumire,
                            codPiesa = @cod,
                            unitateMasuraPiesa = @unitate
                        WHERE idPiesa = @id
                        ";

                    cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", idPiesa); //pentru că doar ăsta are nevoie de el, dar pentru simplitate îl puteam pune și jos.
                }
                else
                {
                    // INSERT pentru add
                    string query = @"
                        INSERT INTO Piese (denumirePiesa, codPiesa, unitateMasuraPiesa)
                        VALUES (@denumire, @cod, @unitate)
                        ";
                    cmd = new SqlCommand(query, conn);
                }

                cmd.Parameters.AddWithValue("@denumire", textBoxDen.Text);
                cmd.Parameters.AddWithValue("@cod", textBoxCod.Text);
                cmd.Parameters.AddWithValue("@unitate", textBoxUm.Text);

                cmd.ExecuteNonQuery();
            }

            //MessageBox.Show(isEditMode ? "Piesa modificată cu succes!" : "Piesa adăugată cu succes!");
            this.Close();
        }

        private void FormPiesa_Load(object sender, EventArgs e)
        {

        }
    }

}
