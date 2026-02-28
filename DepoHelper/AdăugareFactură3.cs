using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace DepoHelper
{
    public partial class AdăugareFactură3 : Form
    {

        private List<FacturaItem> listaTranzactii = new List<FacturaItem>();

        public class FacturaItem
        {
            public int IdServiciu { get; set; }
            public string DenumireServiciu { get; set; }

            public decimal Cost { get; set; }
        }

        public AdăugareFactură3()
        {
            InitializeComponent();
        }


        private void AdăugareFactură3_Load(object sender, EventArgs e)
        {
            LoadFirmeFurnizor();
            ShowServiciiListBox();
        }

        private void LoadFirmeFurnizor()
        {
            try
            {
                cbFurnizor.Items.Clear();

                using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
                {
                    conn.Open();
                    string query = @"SELECT idFirma, numeFirma FROM Firme WHERE tipRelatie = 'F'";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cbFurnizor.Items.Add(
                                new KeyValuePair<int, string>(
                                    (int)reader["idFirma"],
                                    reader["numeFirma"].ToString()
                                ));
                        }
                    }
                }

                cbFurnizor.DisplayMember = "Value";
                cbFurnizor.ValueMember = "Key";

                if (cbFurnizor.Items.Count > 0)
                    cbFurnizor.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare furnizori: " + ex.Message);
            }
        }


        private void ShowServiciiListBox()
        {
            try
            {
                listBoxServicii.Items.Clear();

                using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
                {
                    conn.Open();
                    string query = @"SELECT idServiciu, denumireServiciu, tipServiciu FROM Servicii";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string den = reader["denumireServiciu"].ToString();
                            string tip = reader["tipServiciu"] != DBNull.Value
                                ? reader["tipServiciu"].ToString()
                                : null;

                            string text = string.IsNullOrEmpty(tip)
                                ? den
                                : $"{den} ({tip})";

                            listBoxServicii.Items.Add(new
                            {
                                Text = text,
                                IdServiciu = (int)reader["idServiciu"]
                            });
                        }
                    }
                }

                listBoxServicii.DisplayMember = "Text";

                if (listBoxServicii.Items.Count > 0)
                    listBoxServicii.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare servicii: " + ex.Message);
            }
        }

        private void btnAdaugaServiciu_Click(object sender, EventArgs e)
        {
            if (listBoxServicii.SelectedItem == null)
            {
                MessageBox.Show("Selectează un serviciu!");
                return;
            }

            if (numCost.Value <= 0)
            {
                MessageBox.Show("Introdu costul serviciului!");
                return;
            }

            dynamic serv = listBoxServicii.SelectedItem;

            FacturaItem item = new FacturaItem
            {
                IdServiciu = serv.IdServiciu,
                DenumireServiciu = serv.Text,
                Cost = numCost.Value
            };

            listaTranzactii.Add(item);

            ListViewItem row = new ListViewItem(item.DenumireServiciu);
            row.SubItems.Add(item.Cost.ToString("0.00"));
            row.Tag = item;

            listViewServicii.Items.Add(row);

            numCost.Value = 0;
        }


        private void btnSterge_Click(object sender, EventArgs e)
        {
            if (listViewServicii.SelectedItems.Count == 0)
            {
                MessageBox.Show("Selectează un rând!");
                return;
            }

            var row = listViewServicii.SelectedItems[0];
            var item = (FacturaItem)row.Tag;

            listaTranzactii.Remove(item);
            listViewServicii.Items.Remove(row);
        }

        private void btnSalveaza_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNumarFactura.Text))
            {
                MessageBox.Show("Completează numărul facturii!");
                return;
            }

            if (cbFurnizor.SelectedItem == null)
            {
                MessageBox.Show("Selectează un furnizor!");
                return;
            }

            if (listaTranzactii.Count == 0)
            {
                MessageBox.Show("Nu ai adăugat niciun serviciu!");
                return;
            }

            decimal sumaTotal = listaTranzactii.Sum(x => x.Cost);
            int idFurnizor = ((KeyValuePair<int, string>)cbFurnizor.SelectedItem).Key;

            using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
            {
                conn.Open();
                SqlTransaction tr = conn.BeginTransaction();

                try
                {
                    // Inserăm factura (cap de tabel)
                    string qFactura = @"
                        INSERT INTO Facturi (numarFactura, dataFactura, idFurnizor, sumaTotal, idCentruCost)
                        VALUES (@nr, @data, @idF, @suma, 3);
                        SELECT SCOPE_IDENTITY();";

                    int idFactura;
                    using (SqlCommand cmd = new SqlCommand(qFactura, conn, tr))
                    {
                        cmd.Parameters.AddWithValue("@nr", txtNumarFactura.Text);
                        cmd.Parameters.AddWithValue("@data", dtpData.Value);
                        cmd.Parameters.AddWithValue("@idF", idFurnizor);
                        cmd.Parameters.AddWithValue("@suma", sumaTotal);

                        idFactura = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    // Inserăm serviciile în ServiciiPrestate
                    foreach (var item in listaTranzactii)
                    {
                        string q = @"
                            INSERT INTO ServiciiPrestate
                            (idServiciu, idLocomotiva, idFactura, cost)
                            VALUES (@serv, NULL, @f, @cost)";

                        using (SqlCommand cmd = new SqlCommand(q, conn, tr))
                        {
                            cmd.Parameters.AddWithValue("@serv", item.IdServiciu);
                            cmd.Parameters.AddWithValue("@f", idFactura);
                            cmd.Parameters.AddWithValue("@cost", item.Cost);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    tr.Commit();
                    MessageBox.Show("Factura salvată cu succes!");
                    this.Close();
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    MessageBox.Show("Eroare la salvare: " + ex.Message);
                }
            }
        }
    }
}
