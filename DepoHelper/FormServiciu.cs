using System;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;


namespace DepoHelper
{
    public partial class FormServiciu : Form
    {
        private bool isEditMode = false;
        private int idServiciu;
        public FormServiciu()  // constructor pentru ADD
        {
            InitializeComponent();
            isEditMode = false;
            this.Text = "Adaugă Serviciu";
        }

        public FormServiciu(int id, string denumire, string tip)  // constructor pentru EDIT
        {
            InitializeComponent();
            isEditMode = true;
            this.Text = "Modifică Serviciu";

            idServiciu = id;
            textBoxDen.Text = denumire;
            textBoxTip.Text = tip;
        }

        private void ButtonSave(object sender, EventArgs e)
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
                        UPDATE Servicii
                        SET denumireServiciu = @denumire,
                            tipServiciu = @tip
                        WHERE idServiciu = @id
                        ";

                    cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", idServiciu); //pentru că doar ăsta are nevoie de el, dar pentru simplitate îl puteam pune și jos.
                }
                else
                {
                    // INSERT pentru add
                    string query = @"
                        INSERT INTO Servicii (denumireServiciu, tipServiciu)
                        VALUES (@denumire, @tip)
                        ";
                    cmd = new SqlCommand(query, conn);
                }

                cmd.Parameters.AddWithValue("@denumire", textBoxDen.Text);
                cmd.Parameters.AddWithValue("@tip", textBoxTip.Text);

                cmd.ExecuteNonQuery();
            }

            //MessageBox.Show(isEditMode ? "Piesa modificată cu succes!" : "Piesa adăugată cu succes!");
            this.Close();
        }

        private void FormServiciu_Load(object sender, EventArgs e)
        {

        }
    }

}
