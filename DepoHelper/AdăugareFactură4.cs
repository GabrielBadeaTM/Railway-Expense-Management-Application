using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace DepoHelper
{
    public partial class AdăugareFactură4 : Form
    {

        private List<FacturaItem> listaTranzactii = new List<FacturaItem>();


        public class FacturaItem
        {
            public string Tip { get; set; } // "Piesa" sau "Serviciu"

            public int IdElement { get; set; } // IdPiesa sau IdServiciu
            public string DenumireElement { get; set; }

            public decimal Cantitate { get; set; }   // pentru piese
            public decimal PretUnitar { get; set; }
            public decimal CostTotal { get; set; }
        }
        public AdăugareFactură4()
        {
            InitializeComponent();
        }

        private void AdăugareFactură4_Load(object sender, EventArgs e)
        {
            LoadFirmeFurnizor();
            ShowPieseListBox();
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

        private void ShowPieseListBox()
        {
            try
            {
                listBoxPiese.Items.Clear();

                using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
                {
                    conn.Open();
                    string query = @"SELECT idPiesa, denumirePiesa, codPiesa FROM Piese";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string denumire = reader["denumirePiesa"].ToString();
                            string cod = reader["codPiesa"] != DBNull.Value ? reader["codPiesa"].ToString() : null;

                            string text = !string.IsNullOrEmpty(cod) ? $"{denumire} ({cod})" : denumire;

                            listBoxPiese.Items.Add(new
                            {
                                Text = text,
                                IdPiesa = (int)reader["idPiesa"]
                            });
                        }
                    }
                }

                listBoxPiese.DisplayMember = "Text";

                if (listBoxPiese.Items.Count > 0)
                    listBoxPiese.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare piese: " + ex.Message);
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
                            string tip = reader["tipServiciu"] != DBNull.Value ? reader["tipServiciu"].ToString() : null;

                            string text = string.IsNullOrEmpty(tip) ? den : $"{den} ({tip})";

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

        private void btnAdaugaPiesa_Click(object sender, EventArgs e)
        {
            if (listBoxPiese.SelectedItem == null) { MessageBox.Show("Selectează o piesă!"); return; }
            if (numCantitate_tt.Value <= 0) { MessageBox.Show("Introdu cantitatea!"); return; }
            if (numPret_tt.Value <= 0) { MessageBox.Show("Introdu prețul unitar!"); return; }

            dynamic piesa = listBoxPiese.SelectedItem;

            FacturaItem item = new FacturaItem
            {
                Tip = "Piesa",
                IdElement = piesa.IdPiesa,
                DenumireElement = piesa.Text,
                Cantitate = numCantitate_tt.Value,
                PretUnitar = numPret_tt.Value,
                CostTotal = numCantitate_tt.Value * numPret_tt.Value
            };

            listaTranzactii.Add(item);

            ListViewItem row = new ListViewItem(item.DenumireElement);
            row.SubItems.Add(item.Cantitate.ToString());
            row.SubItems.Add(item.PretUnitar.ToString("0.00"));
            row.SubItems.Add(item.CostTotal.ToString("0.00"));
            row.Tag = item;

            listViewPiese.Items.Add(row);

            numCantitate_tt.Value = 0;
            numPret_tt.Value = 0;
        }

        private void btnAdaugaServiciu_Click(object sender, EventArgs e)
        {
            if (listBoxServicii.SelectedItem == null) { MessageBox.Show("Selectează un serviciu!"); return; }
            if (numCost.Value <= 0) { MessageBox.Show("Introdu costul serviciului!"); return; }

            dynamic serv = listBoxServicii.SelectedItem;

            FacturaItem item = new FacturaItem
            {
                Tip = "Serviciu",
                IdElement = serv.IdServiciu,
                DenumireElement = serv.Text,
                Cantitate = 1,
                PretUnitar = numCost.Value,
                CostTotal = numCost.Value
            };

            listaTranzactii.Add(item);

            ListViewItem row = new ListViewItem(item.DenumireElement);
            row.SubItems.Add(item.CostTotal.ToString("0.00"));
            row.Tag = item;

            listViewServicii.Items.Add(row);

            numCost.Value = 0;
        }

        private void btnSterge_Click(object sender, EventArgs e)
        {
            if (listViewPiese.SelectedItems.Count > 0)
            {
                var row = listViewPiese.SelectedItems[0];
                var item = (FacturaItem)row.Tag;
                listaTranzactii.Remove(item);
                listViewPiese.Items.Remove(row);
            }
            else if (listViewServicii.SelectedItems.Count > 0)
            {
                var row = listViewServicii.SelectedItems[0];
                var item = (FacturaItem)row.Tag;
                listaTranzactii.Remove(item);
                listViewServicii.Items.Remove(row);
            }
            else
            {
                MessageBox.Show("Selectează un rând!");
            }
        }

        private void btnSalveaza_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNumarFactura.Text)) { MessageBox.Show("Completează numărul facturii!"); return; }
            if (cbFurnizor.SelectedItem == null) { MessageBox.Show("Selectează un furnizor!"); return; }
            if (listaTranzactii.Count == 0) { MessageBox.Show("Nu ai adăugat nimic!"); return; }

            int idFurnizor = ((KeyValuePair<int, string>)cbFurnizor.SelectedItem).Key;
            decimal sumaTotal = listaTranzactii.Sum(x => x.CostTotal);

            using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
            {
                conn.Open();
                SqlTransaction tr = conn.BeginTransaction();

                try
                {
                    // INSERT FACTURA
                    string qFactura = @"
                        INSERT INTO Facturi (numarFactura, dataFactura, idFurnizor, sumaTotal, idCentruCost)
                        VALUES (@nr, @data, @idF, @suma, 4);
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

                    // INSERT PIESE
                    foreach (var item in listaTranzactii.Where(x => x.Tip == "Piesa"))
                    {
                        string q = @"
                            INSERT INTO PieseLocomotive
                            (idPiesa, idLocomotiva, idFactura, cantitate, pretUnitar)
                            VALUES (@idP, NULL, @f, @cant, @pret)";
                        using (SqlCommand cmd = new SqlCommand(q, conn, tr))
                        {
                            cmd.Parameters.AddWithValue("@idP", item.IdElement);
                            cmd.Parameters.AddWithValue("@f", idFactura);
                            cmd.Parameters.AddWithValue("@cant", item.Cantitate);
                            cmd.Parameters.AddWithValue("@pret", item.PretUnitar);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    // INSERT SERVICII
                    foreach (var item in listaTranzactii.Where(x => x.Tip == "Serviciu"))
                    {
                        string q = @"
                            INSERT INTO ServiciiPrestate
                            (idServiciu, idLocomotiva, idFactura, cost)
                            VALUES (@idS, NULL, @f, @cost)";
                        using (SqlCommand cmd = new SqlCommand(q, conn, tr))
                        {
                            cmd.Parameters.AddWithValue("@idS", item.IdElement);
                            cmd.Parameters.AddWithValue("@f", idFactura);
                            cmd.Parameters.AddWithValue("@cost", item.CostTotal);
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
