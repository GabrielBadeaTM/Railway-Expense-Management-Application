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
    public partial class AdăugareFactură1 : Form
    {
        // --- ADAUGĂ ASTA LA ÎNCEPUTUL CLASEI ---

        // Lista principală unde ținem minte tot ce adăugăm (coșul de cumpărături)
        private List<FacturaItem> listaTranzactii = new List<FacturaItem>();

        // Clasa care definește un rând (fie piesă, fie serviciu)
        public class FacturaItem
        {
            public string Tip { get; set; }          // "Piesă" sau "Serviciu"

            // Date identificare
            public int IdElement { get; set; }       // ID-ul din baza de date (IdPiesa sau IdServiciu)
            public string DenumireElement { get; set; }
            public int IdLocomotiva { get; set; }
            public string NumeLocomotiva { get; set; }

            // Valori
            public decimal Cantitate { get; set; }   // 1 pentru servicii
            public decimal PretUnitar { get; set; }
            public decimal CostTotal { get; set; }   // Cantitate * Pret
        }

        public AdăugareFactură1()
        {
            InitializeComponent();
        }

        private void AdăugareFactură1_Load(object sender, EventArgs e)
        {
            ShowLocomotive();
            LoadFirmeFurnizor();
            ShowPieseListBox();
            ShowServiciiListBox();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }



        // Excludere mutuala
        private void listViewPiese_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewPiese.SelectedItems.Count > 0)
            {
                listViewServicii.SelectedItems.Clear();
            }
        }

        private void listViewServicii_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewServicii.SelectedItems.Count > 0)
            {
                listViewPiese.SelectedItems.Clear();
            }
        }

        private void ShowLocomotive()
        {
            try
            {
                cbLocomotiveJos1.Items.Clear();
                cbLocomotiveJos2.Items.Clear();

                using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
                {
                    conn.Open();

                    string query = @"
                SELECT 
                    idLocomotiva, 
                    numeLocomotiva
                FROM Locomotive
            ";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = new KeyValuePair<int, string>(
                                (int)reader["idLocomotiva"],
                                reader["numeLocomotiva"].ToString()
                            );

                            cbLocomotiveJos1.Items.Add(item);
                            cbLocomotiveJos2.Items.Add(item);
                        }
                    }
                }

                cbLocomotiveJos1.DisplayMember = "Value";
                cbLocomotiveJos1.ValueMember = "Key";

                cbLocomotiveJos2.DisplayMember = "Value";
                cbLocomotiveJos2.ValueMember = "Key";

                if (cbLocomotiveJos1.Items.Count > 0)
                {
                    cbLocomotiveJos1.SelectedIndex = 0;
                    cbLocomotiveJos2.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la afișarea locomotivelor: " + ex.Message);
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

                    string query = @"
                        SELECT idFirma, numeFirma 
                        FROM Firme
                        WHERE tipRelatie = 'F'
                    ";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cbFurnizor.Items.Add(
                                new KeyValuePair<int, string>(
                                    (int)reader["idFirma"],
                                    reader["numeFirma"].ToString()
                                )
                            );
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
                MessageBox.Show("Eroare la încărcarea firmelor furnizor: " + ex.Message);
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

                    string query = @"
                        SELECT idPiesa, denumirePiesa, codPiesa
                        FROM Piese
                    ";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string denumire = reader["denumirePiesa"].ToString();
                            string cod = reader["codPiesa"] != DBNull.Value ? reader["codPiesa"].ToString() : null;

                            string displayText = !string.IsNullOrEmpty(cod)
                                ? $"{denumire} ({cod})"
                                : denumire;

                            listBoxPiese.Items.Add(new
                            {
                                Text = displayText,
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
                MessageBox.Show("Eroare la afișarea pieselor: " + ex.Message);
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

                    string query = @"
                        SELECT idServiciu, denumireServiciu, tipServiciu
                        FROM Servicii
                    ";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string denumire = reader["denumireServiciu"].ToString();
                            string tip = reader["tipServiciu"] != DBNull.Value ? reader["tipServiciu"].ToString() : null;

                            string displayText = !string.IsNullOrEmpty(tip)
                                ? $"{denumire} ({tip})"
                                : denumire;

                            listBoxServicii.Items.Add(new
                            {
                                Text = displayText,
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
                MessageBox.Show("Eroare la afișarea serviciilor: " + ex.Message);
            }
        }



        // --- EVENIMENT BUTON ADAUGĂ PIESA ---
        private void btnAdaugaPiesa_Click(object sender, EventArgs e)
        {
            if (cbLocomotiveJos1.SelectedItem == null) { MessageBox.Show("Selectează o locomotivă!"); return; }
            if (listBoxPiese.SelectedItem == null) { MessageBox.Show("Selectează o piesă!"); return; }

            decimal cantitate = numCantitate.Value;
            decimal pret = numPret.Value;

            if (cantitate <= 0) { MessageBox.Show("Introdu cantitatea!"); return; }
            if (pret <= 0) { MessageBox.Show("Introdu prețul unitar!"); return; }

            // Extragem datele
            var locSelection = (KeyValuePair<int, string>)cbLocomotiveJos1.SelectedItem;
            dynamic piesaSelection = listBoxPiese.SelectedItem; // Folosim dynamic pt că avem obiect anonim în ListBox

            // Creăm obiectul în memorie
            FacturaItem item = new FacturaItem
            {
                Tip = "Piesă",
                IdLocomotiva = locSelection.Key,
                NumeLocomotiva = locSelection.Value,
                IdElement = piesaSelection.IdPiesa,
                DenumireElement = piesaSelection.Text,
                Cantitate = cantitate,
                PretUnitar = pret,
                CostTotal = cantitate * pret
            };

            listaTranzactii.Add(item);

            // Afișăm în tabelul de sus (ListView Piese)
            ListViewItem rand = new ListViewItem(item.NumeLocomotiva);      // Col 1: Locomotiva
            rand.SubItems.Add(extractNume(item.DenumireElement));           // Col 2: Piesa (Curățăm textul dacă e nevoie)
            rand.SubItems.Add(item.Cantitate.ToString());                   // Col 3: Cantitate
            rand.SubItems.Add(item.PretUnitar.ToString("0.00"));            // Col 4: Preț Unitar

            rand.Tag = item;

            listViewPiese.Items.Add(rand);

            // Resetăm câmpurile
            numCantitate.Value = 0;
            numPret.Value = 0;
        }

        // Funcție mică ajutătoare pentru a scoate codul din paranteză dacă vrei doar numele curat
        private string extractNume(string textFull)
        {
            // Dacă textul e "Surub (1234)", returnează tot textul.
            return textFull;
        }


        // --- EVENIMENT BUTON ADAUGĂ SERVICIU ---
        private void btnAdaugaServiciu_Click(object sender, EventArgs e)
        {
            if (cbLocomotiveJos2.SelectedItem == null) { MessageBox.Show("Selectează o locomotivă!"); return; }
            if (listBoxServicii.SelectedItem == null) { MessageBox.Show("Selectează un serviciu!"); return; }

            decimal cost = numCost.Value;

            if (cost <= 0) { MessageBox.Show("Introdu costul serviciului!"); return; }

            // Extragem datele
            var locSelection = (KeyValuePair<int, string>)cbLocomotiveJos2.SelectedItem;
            dynamic servSelection = listBoxServicii.SelectedItem;

            // Creăm obiectul în memorie
            FacturaItem item = new FacturaItem
            {
                Tip = "Serviciu",
                IdLocomotiva = locSelection.Key,
                NumeLocomotiva = locSelection.Value,
                IdElement = servSelection.IdServiciu,
                DenumireElement = servSelection.Text,
                Cantitate = 1,              // Serviciile sunt de obicei unice pe linie
                PretUnitar = cost,
                CostTotal = cost
            };

            listaTranzactii.Add(item);

            // Afișăm în tabelul de sus (ListView Servicii)
            ListViewItem rand = new ListViewItem(item.NumeLocomotiva);      // Col 1: Locomotiva
            rand.SubItems.Add(item.DenumireElement);                        // Col 2: Serviciu
            rand.SubItems.Add(item.CostTotal.ToString("0.00"));             // Col 3: Cost

            rand.Tag = item;

            listViewServicii.Items.Add(rand);

            // Reset
            numCost.Value = 0;
        }

        private void btnSterge_Click(object sender, EventArgs e)
        {
            // Cazul 1: Avem ceva selectat la PIESE
            if (listViewPiese.SelectedItems.Count > 0)
            {
                ListViewItem randSelectat = listViewPiese.SelectedItems[0];
                FacturaItem itemDeSters = (FacturaItem)randSelectat.Tag;

                listaTranzactii.Remove(itemDeSters);
                listViewPiese.Items.Remove(randSelectat);
            }
            // Cazul 2: Avem ceva selectat la SERVICII
            else if (listViewServicii.SelectedItems.Count > 0)
            {
                ListViewItem randSelectat = listViewServicii.SelectedItems[0];
                FacturaItem itemDeSters = (FacturaItem)randSelectat.Tag;

                listaTranzactii.Remove(itemDeSters);
                listViewServicii.Items.Remove(randSelectat);
            }
            else
            {
                MessageBox.Show("Selectează un rând din tabel pentru a-l șterge!");
            }
        }




        private void btnSalveaza_Click(object sender, EventArgs e)
        {
            // ---------------------------------------------------------
            // 2. Validări obligatorii
            // ---------------------------------------------------------
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
                MessageBox.Show("Nu ai adăugat nicio piesă sau serviciu pe factură!");
                return;
            }

            // ---------------------------------------------------------
            // 3. Pregătire date
            // ---------------------------------------------------------
            string nrFactura = txtNumarFactura.Text;
            DateTime dataFactura = dtpData.Value;
            int idFurnizor = ((KeyValuePair<int, string>)cbFurnizor.SelectedItem).Key;

            // Calcul suma totală din lista de tranzacții
            decimal sumaTotal = listaTranzactii.Sum(x => x.CostTotal);

            using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // ---------------------------------------------------------
                    // A. Inserăm FACTURA (Capul de tabel) cu suma totală
                    // ---------------------------------------------------------
                    string queryFactura = @"
                        INSERT INTO Facturi (numarFactura, dataFactura, idFurnizor, sumaTotal, idCentruCost) 
                        VALUES (@nr, @data, @idF, @suma, 1);
                        SELECT SCOPE_IDENTITY();";

                    int idFacturaNoua = 0;

                    using (SqlCommand cmd = new SqlCommand(queryFactura, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@nr", nrFactura);
                        cmd.Parameters.AddWithValue("@data", dataFactura);
                        cmd.Parameters.AddWithValue("@idF", idFurnizor);
                        cmd.Parameters.AddWithValue("@suma", sumaTotal);

                        object result = cmd.ExecuteScalar();
                        idFacturaNoua = Convert.ToInt32(result);
                    }

                    // ---------------------------------------------------------
                    // B. Inserăm LINIILE (Piese și Servicii)
                    // ---------------------------------------------------------
                    foreach (var item in listaTranzactii)
                    {
                        if (item.Tip == "Piesă")
                        {
                            // Inserăm piesa
                            string queryPiesa = @"
                                INSERT INTO PieseLocomotive (idPiesa, idLocomotiva, idFactura, cantitate, pretUnitar)
                                VALUES (@idPiesa, @idLocomotiva, @idFactura, @cantitate, @pretUnitar)";

                            using (SqlCommand cmd = new SqlCommand(queryPiesa, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@idPiesa", item.IdElement);
                                cmd.Parameters.AddWithValue("@idLocomotiva", item.IdLocomotiva);
                                cmd.Parameters.AddWithValue("@idFactura", idFacturaNoua);
                                cmd.Parameters.AddWithValue("@cantitate", item.Cantitate);
                                cmd.Parameters.AddWithValue("@pretUnitar", item.PretUnitar);

                                cmd.ExecuteNonQuery();
                            }
                        }
                        else if (item.Tip == "Serviciu")
                        {
                            // Inserăm serviciul
                            string queryServiciu = @"
                                INSERT INTO ServiciiPrestate (idServiciu, idLocomotiva, idFactura, cost)
                                VALUES (@idServiciu, @idLocomotiva, @idFactura, @cost)";

                            using (SqlCommand cmd = new SqlCommand(queryServiciu, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@idServiciu", item.IdElement);
                                cmd.Parameters.AddWithValue("@idLocomotiva", item.IdLocomotiva);
                                cmd.Parameters.AddWithValue("@idFactura", idFacturaNoua);
                                cmd.Parameters.AddWithValue("@cost", item.PretUnitar);

                                cmd.ExecuteNonQuery();
                            }
                        }
                    }


                    // ---------------------------------------------------------
                    // C. COMMIT
                    // ---------------------------------------------------------
                    transaction.Commit();
                    MessageBox.Show("Factura a fost salvată cu succes!", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // ---------------------------------------------------------
                    // D. Curățare formular
                    // ---------------------------------------------------------
                    listaTranzactii.Clear();
                    listViewPiese.Items.Clear();
                    listViewServicii.Items.Clear();
                    txtNumarFactura.Clear();
                     
                    this.Close();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Eroare la salvare! Modificările au fost anulate.\n\nDetalii: " + ex.Message, "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


    }
}