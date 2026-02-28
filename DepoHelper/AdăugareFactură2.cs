using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace DepoHelper
{
    public partial class AdăugareFactură2 : Form
    {
        private List<FacturaItem> listaTranzactii = new List<FacturaItem>();

        public class FacturaItem
        {
            public int IdPiesa { get; set; }
            public string DenumirePiesa { get; set; }

            public int IdVagon { get; set; }
            public string numarVagon { get; set; }

            public decimal Cantitate { get; set; }
            public decimal PretUnitar { get; set; }
            public decimal CostTotal { get; set; }
        }

        public AdăugareFactură2()
        {
            InitializeComponent();
        }

        private void AdăugareFactură2_Load(object sender, EventArgs e)
        {
            ShowVagoane();
            LoadFirmeFurnizor();
            ShowPieseListBox();
        }


        private void ShowVagoane()
        {
            try
            {
                cbVagoane.Items.Clear();

                using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
                {
                    conn.Open();
                    string query = @"SELECT idVagon, numarVagon FROM Vagoane";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cbVagoane.Items.Add(
                                new KeyValuePair<int, string>(
                                    (int)reader["idVagon"],
                                    reader["numarVagon"].ToString()
                                ));
                        }
                    }
                }

                cbVagoane.DisplayMember = "Value";
                cbVagoane.ValueMember = "Key";

                if (cbVagoane.Items.Count > 0)
                    cbVagoane.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare vagoane: " + ex.Message);
            }
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
                            string den = reader["denumirePiesa"].ToString();
                            string cod = reader["codPiesa"] != DBNull.Value
                                ? reader["codPiesa"].ToString()
                                : null;

                            string text = string.IsNullOrEmpty(cod)
                                ? den
                                : $"{den} ({cod})";

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


        private void btnAdaugaPiesa_Click(object sender, EventArgs e)
        {
            if (cbVagoane.SelectedItem == null || listBoxPiese.SelectedItem == null)
            {
                MessageBox.Show("Selectează vagon și piesă!");
                return;
            }

            if (numCantitate_tt.Value <= 0 || numPret_tt.Value <= 0)
            {
                MessageBox.Show("Cantitate și preț valide!");
                return;
            }

            var vagon = (KeyValuePair<int, string>)cbVagoane.SelectedItem;
            dynamic piesa = listBoxPiese.SelectedItem;

            FacturaItem item = new FacturaItem
            {
                IdVagon = vagon.Key,
                numarVagon = vagon.Value,
                IdPiesa = piesa.IdPiesa,
                DenumirePiesa = piesa.Text,
                Cantitate = numCantitate_tt.Value,
                PretUnitar = numPret_tt.Value,
                CostTotal = numCantitate_tt.Value * numPret_tt.Value
            };

            listaTranzactii.Add(item);

            ListViewItem row = new ListViewItem(item.numarVagon);
            row.SubItems.Add(item.DenumirePiesa);
            row.SubItems.Add(item.Cantitate.ToString());
            row.SubItems.Add(item.PretUnitar.ToString("0.00"));
            row.SubItems.Add(item.CostTotal.ToString("0.00"));
            row.Tag = item;

            listViewPiese.Items.Add(row);

            numCantitate_tt.Value = 0;
            numPret_tt.Value = 0;
        }

        private void btnSterge_Click(object sender, EventArgs e)
        {
            if (listViewPiese.SelectedItems.Count == 0)
            {
                MessageBox.Show("Selectează un rând!");
                return;
            }

            var row = listViewPiese.SelectedItems[0];
            var item = (FacturaItem)row.Tag;

            listaTranzactii.Remove(item);
            listViewPiese.Items.Remove(row);
        }
        private void btnSalveaza_Click(object sender, EventArgs e)
        {
            if (listaTranzactii.Count == 0)
            {
                MessageBox.Show("Factura e goală!");
                return;
            }

            decimal sumaTotal = listaTranzactii.Sum(x => x.CostTotal);
            int idFurnizor = ((KeyValuePair<int, string>)cbFurnizor.SelectedItem).Key;

            using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
            {
                conn.Open();
                SqlTransaction tr = conn.BeginTransaction();

                try
                {
                    string qFactura = @"
                        INSERT INTO Facturi (numarFactura, dataFactura, idFurnizor, sumaTotal, idCentruCost)
                        VALUES (@nr, @data, @idF, @suma, 2);
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

                    foreach (var item in listaTranzactii)
                    {
                        string q = @"
                            INSERT INTO PieseVagoane
                            (idPiesa, idVagon, idFactura, cantitate, pretUnitar)
                            VALUES (@p, @v, @f, @c, @pr)";

                        using (SqlCommand cmd = new SqlCommand(q, conn, tr))
                        {
                            cmd.Parameters.AddWithValue("@p", item.IdPiesa);
                            cmd.Parameters.AddWithValue("@v", item.IdVagon);
                            cmd.Parameters.AddWithValue("@f", idFactura);
                            cmd.Parameters.AddWithValue("@c", item.Cantitate);
                            cmd.Parameters.AddWithValue("@pr", item.PretUnitar);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    tr.Commit();
                    MessageBox.Show("Factura salvată!");
                    this.Close();
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    MessageBox.Show("Eroare: " + ex.Message);
                }
            }
        }
    }
}
