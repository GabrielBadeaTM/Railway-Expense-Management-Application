using System;
using System.Data;
using System.Net.Sockets;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DepoHelper
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadLocomotive();
            LoadFirme();

            ShowPiese();
            ShowServicii();

            LoadJudete();
            LoadOrase();
            LoadOraseInPuncteLucru();
            LoadPuncteLucru();

            // Nou
            LoadFacturi();
            LoadCentreCost(); // (de pe pagina facturi)
            comboBoxTip.SelectedIndex = 0;

            // Interfata Vagoane
            LoadClienti();

            // Interfata Central
            UpdateListViewCheltuieli2();

            //Interfata Manevre
            UpdateListViewCheltuieli3();

            ShowLocomotive();
            if (comboBoxV.SelectedValue != null)
                LoadVagoaneSettings(Convert.ToInt32(comboBoxV.SelectedValue));

            // Altele;
            listBoxPL.ClearSelected(); // face ca la inceput sa nu fie selectat nimic in lista de puncte de lucru in setari
        }

        private void RefreshToateTaburile()
        {
            LoadFacturi();
            RefreshDetaliiLocomotive();
            UpdateListViewVagoane();
            UpdateListViewCheltuieli2();
            UpdateListViewCheltuieli3();
        }

        #region LocomotiveInterfata
        private void LoadLocomotive()
        {
            try
            {
                listBoxLocMain.Items.Clear();

                using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
                {
                    conn.Open();
                    string query = "SELECT idLocomotiva, numeLocomotiva FROM Locomotive";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    listBoxLocMain.DisplayMember = "numeLocomotiva";   // afișaj pentru utilizator
                    listBoxLocMain.ValueMember = "idLocomotiva";       // valoare folosită în cod
                    listBoxLocMain.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la încărcarea locomotivelor: " + ex.Message);
            }
        }
        // 🔹 Încarcă locomotivele în listBox

        private void DisplayLocomotivaDetails(SqlConnection conn, int idLocomotiva)
        {
            string query = @"
                SELECT nrEuro, tip, serie, putere 
                FROM Locomotive 
                WHERE idLocomotiva = @idLocomotiva";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@idLocomotiva", idLocomotiva);

            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                labelInfoLocMain.Text = $"Nr Euro: {reader["nrEuro"]}\n" +
                              $"Tip: {reader["tip"]}\n" +
                              $"Serie Șasiu: {reader["serie"]}\n" +
                              $"Putere: {reader["putere"]} kW";
            }
            reader.Close();

            query = @"
                SELECT _oras.numeOras
                FROM Locomotive _locomotiva
                JOIN PuncteLucru _punct ON _locomotiva.idPunctLucru = _punct.idPunctLucru
                JOIN Orase _oras ON _punct.idOras = _oras.idOras
                WHERE _locomotiva.idLocomotiva = @idLocomotiva";
            cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@idLocomotiva", idLocomotiva);

            object oras = cmd.ExecuteScalar();
            labelInfoLoc2Main.Text = oras != null ? "Punct Lucru: " + oras.ToString() : "Punct Lucru: N/A";
        }

        private void PopulateListViewLocomotive(
            SqlConnection conn,
            int idLocomotiva,
            DateTime dataStart,
            DateTime dataEnd,
            int tipIndex   // 0=Toate, 1=Piese, 2=Servicii
        )
        {
            listView1.Items.Clear();

            string queryAfisare = @"
                SELECT *
                FROM
                (
                    -- PIESE
                    SELECT
                        'Piesa' AS Tip,
                        p.denumirePiesa AS Denumire,
                        p.codPiesa AS Cod,
                        pl.cantitate,
                        pl.pretUnitar,
                        (pl.cantitate * pl.pretUnitar) AS Valoare,
                        f.numeFirma,
                        fa.dataFactura
                    FROM PieseLocomotive pl
                    JOIN Piese p ON pl.idPiesa = p.idPiesa
                    JOIN Facturi fa ON pl.idFactura = fa.idFactura
                    JOIN Firme f ON fa.idFurnizor = f.idFirma
                    WHERE pl.idLocomotiva = @idLocomotiva
                      AND CAST(fa.dataFactura AS DATE) BETWEEN @dataStart AND @dataEnd

                    UNION ALL

                    -- SERVICII
                    SELECT
                        'Serviciu' AS Tip,
                        s.denumireServiciu AS Denumire,
                        s.tipServiciu AS Cod,
                        1 AS cantitate,
                        sp.cost AS pretUnitar,
                        sp.cost AS Valoare,
                        f.numeFirma,
                        fa.dataFactura
                    FROM ServiciiPrestate sp
                    JOIN Servicii s ON sp.idServiciu = s.idServiciu
                    JOIN Facturi fa ON sp.idFactura = fa.idFactura
                    JOIN Firme f ON fa.idFurnizor = f.idFirma
                    WHERE sp.idLocomotiva = @idLocomotiva
                      AND CAST(fa.dataFactura AS DATE) BETWEEN @dataStart AND @dataEnd
                ) X
                WHERE
                    @tip = 0
                    OR (@tip = 1 AND X.Tip = 'Piesa')
                    OR (@tip = 2 AND X.Tip = 'Serviciu')
                ORDER BY X.dataFactura;
            ";

            using (SqlCommand cmd = new SqlCommand(queryAfisare, conn))
            {
                cmd.Parameters.AddWithValue("@idLocomotiva", idLocomotiva);
                cmd.Parameters.AddWithValue("@dataStart", dataStart.Date);
                cmd.Parameters.AddWithValue("@dataEnd", dataEnd.Date);
                cmd.Parameters.AddWithValue("@tip", tipIndex);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ListViewItem item = new ListViewItem(reader["Denumire"].ToString());
                        item.SubItems.Add(reader["Cod"] == DBNull.Value ? "" : reader["Cod"].ToString());
                        item.SubItems.Add(reader["cantitate"].ToString());
                        item.SubItems.Add(Convert.ToDecimal(reader["pretUnitar"]).ToString("N2"));
                        item.SubItems.Add(Convert.ToDecimal(reader["Valoare"]).ToString("N2") + " RON");
                        item.SubItems.Add(reader["numeFirma"].ToString());
                        item.SubItems.Add(
                            Convert.ToDateTime(reader["dataFactura"]).ToString("dd.MM.yyyy")
                        );

                        listView1.Items.Add(item);
                    }
                }
            }

            string queryTotaluri = @"
                SELECT
                    -- TOTAL PIESE
                    (
                        SELECT ISNULL(SUM(pl.cantitate * pl.pretUnitar), 0)
                        FROM PieseLocomotive pl
                        JOIN Facturi fa ON pl.idFactura = fa.idFactura
                        WHERE pl.idLocomotiva = @idLocomotiva
                          AND CAST(fa.dataFactura AS DATE) BETWEEN @dataStart AND @dataEnd
                    ) AS TotalPiese,

                    -- TOTAL SERVICII
                    (
                        SELECT ISNULL(SUM(sp.cost), 0)
                        FROM ServiciiPrestate sp
                        JOIN Facturi fa ON sp.idFactura = fa.idFactura
                        WHERE sp.idLocomotiva = @idLocomotiva
                          AND CAST(fa.dataFactura AS DATE) BETWEEN @dataStart AND @dataEnd
                    ) AS TotalServicii;
            ";

            using (SqlCommand cmdTotal = new SqlCommand(queryTotaluri, conn))
            {
                cmdTotal.Parameters.AddWithValue("@idLocomotiva", idLocomotiva);
                cmdTotal.Parameters.AddWithValue("@dataStart", dataStart.Date);
                cmdTotal.Parameters.AddWithValue("@dataEnd", dataEnd.Date);

                using (SqlDataReader r = cmdTotal.ExecuteReader())
                {
                    if (r.Read())
                    {
                        decimal totalPiese = Convert.ToDecimal(r["TotalPiese"]);
                        decimal totalServicii = Convert.ToDecimal(r["TotalServicii"]);
                        decimal totalGeneral = totalPiese + totalServicii;

                        label7.Text =
                            "Total general:\n" + totalGeneral.ToString("N2") + " RON\n\n" +
                            "Total piese:\n" + totalPiese.ToString("N2") + " RON\n\n" +
                            "Total servicii:\n" + totalServicii.ToString("N2") + " RON";
                    }
                }
            }
        }
        // 🔹 Populează ListView cu piese și facturi din main

        private void RefreshDetaliiLocomotive()
        {
            if (listBoxLocMain.SelectedValue == null)
                return;

            int idLocomotiva = Convert.ToInt32(listBoxLocMain.SelectedValue);

            // Dacă nu e selectat nimic în combo, implicit "Toate"
            int tipIndex = comboBoxTip.SelectedIndex;
            if (tipIndex < 0)
                tipIndex = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
                {
                    conn.Open();

                    // Detalii locomotivă (sus)
                    DisplayLocomotivaDetails(conn, idLocomotiva);

                    // Interval date
                    DateTime dataStart = dateTimePicker1.Value.Date;
                    DateTime dataEnd = dateTimePicker2.Value.Date;

                    // Listă + totaluri (jos)
                    PopulateListViewLocomotive(
                        conn,
                        idLocomotiva,
                        dataStart,
                        dataEnd,
                        tipIndex
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la afișarea detaliilor: " + ex.Message);
            }
        }
        // 🔹 Eveniment când selectăm o locomotivă sau schimbăm datele


        private void comboBoxTip_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshDetaliiLocomotive();
        }

        #endregion

        #region Vagoane Interfata

        // 🔹 Clasă simplă pentru client
        public class ClientItem
        {
            public int Id { get; set; }
            public string Nume { get; set; }

            public override string ToString() => Nume; // afișaj default în comboBox
        }

        // 🔹 Încarcă clienții în comboBoxClienti
        private void LoadClienti()
        {
            try
            {
                comboBoxClienti.Items.Clear();

                using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT idFirma, numeFirma
                        FROM Firme
                        WHERE TipRelatie = 'C'
                        ORDER BY numeFirma";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            comboBoxClienti.Items.Add(new ClientItem
                            {
                                Id = Convert.ToInt32(reader["idFirma"]),
                                Nume = reader["numeFirma"].ToString()
                            });
                        }
                    }

                    if (comboBoxClienti.Items.Count > 0)
                        comboBoxClienti.SelectedIndex = 0; // selectăm primul client
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la încărcarea clienților: " + ex.Message);
            }
        }

        // 🔹 Populează listBox-ul cu vagoane pentru clientul selectat
        private void RefreshListBoxVagoane()
        {
            var selectedClient = comboBoxClienti.SelectedItem as ClientItem;
            if (selectedClient == null) return;

            int idClient = selectedClient.Id;

            listBoxVagoane.Items.Clear();
            labelNrVag.Text = "Nr. Vagoane: 0";

            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
                {
                    conn.Open();

                    // Lista vagoane
                    string queryVagoane = @"
                        SELECT numarVagon
                        FROM Vagoane
                        WHERE idProprietar = @idClient
                        ORDER BY numarVagon";

                    using (SqlCommand cmd = new SqlCommand(queryVagoane, conn))
                    {
                        cmd.Parameters.AddWithValue("@idClient", idClient);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                                listBoxVagoane.Items.Add(reader["numarVagon"].ToString());
                        }
                    }

                    // Număr total vagoane
                    string queryCount = @"
                        SELECT COUNT(*) 
                        FROM Vagoane
                        WHERE idProprietar = @idClient";

                    using (SqlCommand cmdCount = new SqlCommand(queryCount, conn))
                    {
                        cmdCount.Parameters.AddWithValue("@idClient", idClient);
                        int nrVagoane = (int)cmdCount.ExecuteScalar();
                        labelNrVag.Text = $"Nr. Vagoane: {nrVagoane}";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la afișarea vagoanelor: " + ex.Message);
            }

            // Setează primul vagon selectat dacă există
            if (listBoxVagoane.Items.Count > 0)
                listBoxVagoane.SelectedIndex = 0;
        }

        // 🔹 Event pentru comboBoxClienti
        private void comboBoxClienti_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshListBoxVagoane();
        }

        // 🔹 Populează listView-ul pieselor vagonului și calculează total
        private void PopulateListViewVagoane(int idClient, string numarVagon, DateTime dataStart, DateTime dataEnd)
        {
            listViewVagoane.Items.Clear();
            labelTotal1.Text = "0 RON";

            if (string.IsNullOrEmpty(numarVagon)) return;

            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
                {
                    conn.Open();

                    // Lista pieselor
                    string query = @"
                        SELECT 
                            p.denumirePiesa AS Piesa,
                            p.codPiesa AS Cod,
                            pv.cantitate AS Cantitate,
                            pv.pretUnitar AS PretUnitar,
                            (pv.cantitate * pv.pretUnitar) AS Total,
                            f.numeFirma AS Furnizor,
                            fct.numarFactura AS NumarFactura,
                            fct.dataFactura AS DataAchizitie
                        FROM PieseVagoane pv
                        JOIN Piese p ON pv.idPiesa = p.idPiesa
                        JOIN Vagoane v ON pv.idVagon = v.idVagon
                        JOIN Facturi fct ON pv.idFactura = fct.idFactura
                        JOIN Firme f ON fct.idFurnizor = f.idFirma
                        WHERE v.idProprietar = @idClient
                          AND v.numarVagon = @numarVagon
                          AND CAST(fct.dataFactura AS DATE) BETWEEN @dataStart AND @dataEnd
                        ORDER BY fct.dataFactura;";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@idClient", idClient);
                        cmd.Parameters.AddWithValue("@numarVagon", numarVagon);
                        cmd.Parameters.AddWithValue("@dataStart", dataStart.Date);
                        cmd.Parameters.AddWithValue("@dataEnd", dataEnd.Date);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ListViewItem item = new ListViewItem(reader["Piesa"].ToString());
                                item.SubItems.Add(reader["Cod"].ToString());
                                item.SubItems.Add(reader["Cantitate"].ToString());
                                item.SubItems.Add(Convert.ToDecimal(reader["PretUnitar"]).ToString("N2"));
                                item.SubItems.Add(Convert.ToDecimal(reader["Total"]).ToString("N2") + " RON");
                                item.SubItems.Add(Convert.ToDateTime(reader["DataAchizitie"]).ToString("dd.MM.yyyy"));
                                item.SubItems.Add(reader["Furnizor"].ToString());
                                item.SubItems.Add(reader["NumarFactura"].ToString());

                                listViewVagoane.Items.Add(item);
                            }
                        }
                    }

                    // Total general direct din SQL
                    string queryTotal = @"
                        SELECT ISNULL(SUM(pv.cantitate * pv.pretUnitar), 0) AS TotalGeneral
                        FROM PieseVagoane pv
                        JOIN Vagoane v ON pv.idVagon = v.idVagon
                        JOIN Facturi fct ON pv.idFactura = fct.idFactura
                        WHERE v.idProprietar = @idClient
                          AND v.numarVagon = @numarVagon
                          AND CAST(fct.dataFactura AS DATE) BETWEEN @dataStart AND @dataEnd;";

                    using (SqlCommand cmdTotal = new SqlCommand(queryTotal, conn))
                    {
                        cmdTotal.Parameters.AddWithValue("@idClient", idClient);
                        cmdTotal.Parameters.AddWithValue("@numarVagon", numarVagon);
                        cmdTotal.Parameters.AddWithValue("@dataStart", dataStart.Date);
                        cmdTotal.Parameters.AddWithValue("@dataEnd", dataEnd.Date);

                        object totalObj = cmdTotal.ExecuteScalar();
                        decimal total = totalObj != DBNull.Value ? Convert.ToDecimal(totalObj) : 0m;
                        labelTotal1.Text = total.ToString("N2") + " RON";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la afișarea pieselor vagonului: " + ex.Message);
            }
        }

        // 🔹 Funcție comună pentru actualizare listView
        private void UpdateListViewVagoane()
        {
            var selectedClient = comboBoxClienti.SelectedItem as ClientItem;
            var selectedVagon = listBoxVagoane.SelectedItem as string;

            if (selectedClient == null || string.IsNullOrEmpty(selectedVagon))
                return;

            int idClient = selectedClient.Id;
            string numarVagon = selectedVagon;
            DateTime dataStart = dtp1.Value.Date;
            DateTime dataEnd = dtp2.Value.Date;

            PopulateListViewVagoane(idClient, numarVagon, dataStart, dataEnd);
        }

        // 🔹 Event pentru listBoxVagoane
        private void listBoxVagoane_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateListViewVagoane();
        }

        // 🔹 Evenimente pentru DateTimePickers
        private void dtp1_ValueChanged(object sender, EventArgs e)
        {
            UpdateListViewVagoane();
        }

        private void dtp2_ValueChanged(object sender, EventArgs e)
        {
            UpdateListViewVagoane();
        }

        #endregion

        #region Central Interfata

        private void PopulateListViewCheltuieli2(DateTime dataStart, DateTime dataEnd)
        {
            listViewCheltuieli2.Items.Clear();
            labelCheltuieli2.Text = "0 RON";

            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
                {
                    conn.Open();

                    // Îmi pare foarte rău, sunt obligat să fac mizeria asta... de profii de la facultate... Știu că nu așa se face un querry. I AM SORRY. FORGIVE ME
                    string query = @"
                        SELECT 
                            s.denumireServiciu AS Serviciu,
                            sp.cost AS Cost,
                            f.numeFirma AS Furnizor,
                            fct.numarFactura AS NumarFactura,
                            fct.dataFactura AS DataPlatii
                        FROM ServiciiPrestate sp
                        JOIN Servicii s ON sp.idServiciu = s.idServiciu
                        JOIN Facturi fct ON sp.idFactura = fct.idFactura
                        JOIN Firme f ON fct.idFurnizor = f.idFirma
                        WHERE CAST(fct.dataFactura AS DATE) BETWEEN @dataStart AND @dataEnd
                          AND fct.idCentruCost = (
                              SELECT idCentruCost 
                              FROM CentreCost 
                              WHERE denumireCentruCost = 'Central'
                          )
                        ORDER BY fct.dataFactura;
                        ";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@dataStart", dataStart.Date);
                        cmd.Parameters.AddWithValue("@dataEnd", dataEnd.Date);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ListViewItem item = new ListViewItem(reader["Serviciu"].ToString());
                                item.SubItems.Add(Convert.ToDecimal(reader["Cost"]).ToString("N2"));
                                item.SubItems.Add(Convert.ToDateTime(reader["DataPlatii"]).ToString("dd.MM.yyyy"));
                                item.SubItems.Add(reader["Furnizor"].ToString());
                                item.SubItems.Add(reader["NumarFactura"].ToString());

                                listViewCheltuieli2.Items.Add(item);
                            }
                        }
                    }

                    // 🔹 Query pentru totalul direct din SQL
                    string queryTotal = @"
                        SELECT ISNULL(SUM(sp.cost),0) AS TotalGeneral
                        FROM ServiciiPrestate sp
                        JOIN Facturi fct ON sp.idFactura = fct.idFactura
                        WHERE CAST(fct.dataFactura AS DATE) BETWEEN @dataStart AND @dataEnd
                          AND fct.idCentruCost = (
                              SELECT idCentruCost 
                              FROM CentreCost 
                              WHERE denumireCentruCost = 'Central'
                          );
                        ";

                    using (SqlCommand cmdTotal = new SqlCommand(queryTotal, conn))
                    {
                        cmdTotal.Parameters.AddWithValue("@dataStart", dataStart.Date);
                        cmdTotal.Parameters.AddWithValue("@dataEnd", dataEnd.Date);

                        object totalObj = cmdTotal.ExecuteScalar();
                        decimal total = totalObj != DBNull.Value ? Convert.ToDecimal(totalObj) : 0m;
                        labelCheltuieli2.Text = total.ToString("N2") + " RON";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la afișarea cheltuielilor: " + ex.Message);
            }
        }

        private void dtp3_ValueChanged(object sender, EventArgs e)
        {
            UpdateListViewCheltuieli2();
        }

        private void dtp4_ValueChanged(object sender, EventArgs e)
        {
            UpdateListViewCheltuieli2();
        }

        private void UpdateListViewCheltuieli2()
        {
            DateTime dataStart = dtp3.Value.Date;
            DateTime dataEnd = dtp4.Value.Date;

            PopulateListViewCheltuieli2(dataStart, dataEnd);
        }

        #endregion

        #region Manevre Interfata

        private void PopulateListViewManevre(DateTime dataStart, DateTime dataEnd)
        {
            listViewPiese3.Items.Clear();
            listViewServicii3.Items.Clear();
            labelTotal3.Text = "Total general: 0 RON\n\nTotal piese: 0 RON\n\nTotal servicii: 0 RON";

            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
                {
                    conn.Open();

                    string queryPiese = @"
                        SELECT 
                            p.denumirePiesa AS Piesa,
                            p.codPiesa AS Cod,
                            pl.cantitate AS Cantitate,
                            pl.pretUnitar AS PretUnitar,
                            (pl.cantitate * pl.pretUnitar) AS Total,
                            f.numeFirma AS Furnizor,
                            fct.numarFactura AS NumarFactura,
                            fct.dataFactura AS DataAchizitie
                        FROM PieseLocomotive pl
                        JOIN Piese p ON pl.idPiesa = p.idPiesa
                        JOIN Facturi fct ON pl.idFactura = fct.idFactura
                        JOIN Firme f ON fct.idFurnizor = f.idFirma
                        WHERE CAST(fct.dataFactura AS DATE) BETWEEN @dataStart AND @dataEnd
                          AND fct.idCentruCost = (
                              SELECT idCentruCost 
                              FROM CentreCost 
                              WHERE denumireCentruCost = 'Manevre'
                          )
                        ORDER BY fct.dataFactura;";

                    using (SqlCommand cmd = new SqlCommand(queryPiese, conn))
                    {
                        cmd.Parameters.AddWithValue("@dataStart", dataStart.Date);
                        cmd.Parameters.AddWithValue("@dataEnd", dataEnd.Date);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ListViewItem item = new ListViewItem(reader["Piesa"].ToString());
                                item.SubItems.Add(reader["Cod"].ToString());
                                item.SubItems.Add(reader["Cantitate"].ToString());
                                item.SubItems.Add(Convert.ToDecimal(reader["PretUnitar"]).ToString("N2"));
                                item.SubItems.Add(Convert.ToDecimal(reader["Total"]).ToString("N2") + " RON");
                                item.SubItems.Add(Convert.ToDateTime(reader["DataAchizitie"]).ToString("dd.MM.yyyy"));
                                item.SubItems.Add(reader["Furnizor"].ToString());
                                item.SubItems.Add(reader["NumarFactura"].ToString());

                                listViewPiese3.Items.Add(item);
                            }
                        }
                    }

                    // 🔹 Servicii
                    string queryServicii = @"
                        SELECT 
                            s.denumireServiciu AS Serviciu,
                            sp.cost AS Cost,
                            f.numeFirma AS Furnizor,
                            fct.numarFactura AS NumarFactura,
                            fct.dataFactura AS DataPlatii
                        FROM ServiciiPrestate sp
                        JOIN Servicii s ON sp.idServiciu = s.idServiciu
                        JOIN Facturi fct ON sp.idFactura = fct.idFactura
                        JOIN Firme f ON fct.idFurnizor = f.idFirma
                        WHERE CAST(fct.dataFactura AS DATE) BETWEEN @dataStart AND @dataEnd
                          AND fct.idCentruCost = (
                              SELECT idCentruCost 
                              FROM CentreCost 
                              WHERE denumireCentruCost = 'Manevre'
                          )
                        ORDER BY fct.dataFactura;";

                    using (SqlCommand cmd = new SqlCommand(queryServicii, conn))
                    {
                        cmd.Parameters.AddWithValue("@dataStart", dataStart.Date);
                        cmd.Parameters.AddWithValue("@dataEnd", dataEnd.Date);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ListViewItem item = new ListViewItem(reader["Serviciu"].ToString());
                                item.SubItems.Add(Convert.ToDecimal(reader["Cost"]).ToString("N2") + " RON");
                                item.SubItems.Add(Convert.ToDateTime(reader["DataPlatii"]).ToString("dd.MM.yyyy"));
                                item.SubItems.Add(reader["Furnizor"].ToString());
                                item.SubItems.Add(reader["NumarFactura"].ToString());

                                listViewServicii3.Items.Add(item);
                            }
                        }
                    }

                    // 🔹 Totaluri direct din SQL cu subqueries
                    string queryTotal = @"
                        SELECT
                            (SELECT ISNULL(SUM(pl.cantitate * pl.pretUnitar),0)
                             FROM PieseLocomotive pl
                             JOIN Facturi fct ON pl.idFactura = fct.idFactura
                             WHERE CAST(fct.dataFactura AS DATE) BETWEEN @dataStart AND @dataEnd
                               AND fct.idCentruCost = (
                                   SELECT idCentruCost FROM CentreCost WHERE denumireCentruCost = 'Manevre'
                               )) AS TotalPiese,

                            (SELECT ISNULL(SUM(sp.cost),0)
                             FROM ServiciiPrestate sp
                             JOIN Facturi fct ON sp.idFactura = fct.idFactura
                             WHERE CAST(fct.dataFactura AS DATE) BETWEEN @dataStart AND @dataEnd
                               AND fct.idCentruCost = (
                                   SELECT idCentruCost FROM CentreCost WHERE denumireCentruCost = 'Manevre'
                               )) AS TotalServicii;";

                    using (SqlCommand cmdTotal = new SqlCommand(queryTotal, conn))
                    {
                        cmdTotal.Parameters.AddWithValue("@dataStart", dataStart.Date);
                        cmdTotal.Parameters.AddWithValue("@dataEnd", dataEnd.Date);

                        using (SqlDataReader r = cmdTotal.ExecuteReader())
                        {
                            if (r.Read())
                            {
                                decimal totalPiese = Convert.ToDecimal(r["TotalPiese"]);
                                decimal totalServicii = Convert.ToDecimal(r["TotalServicii"]);
                                decimal totalGeneral = totalPiese + totalServicii;

                                labelTotal3.Text =
                                    "Total general: " + totalGeneral.ToString("N2") + " RON\n\n" +
                                    "Total piese: " + totalPiese.ToString("N2") + " RON\n\n" +
                                    "Total servicii: " + totalServicii.ToString("N2") + " RON";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la afișarea pieselor și serviciilor pentru Manevre: " + ex.Message);
            }
        }

        private void UpdateListViewCheltuieli3()
        {
            DateTime dataStart = dtp5.Value.Date;
            DateTime dataEnd = dtp6.Value.Date;

            PopulateListViewManevre(dataStart, dataEnd);
        }

        // 🔹 Eventuri pentru dtp5 și dtp6
        private void dtp5_ValueChanged(object sender, EventArgs e)
        {
            UpdateListViewCheltuieli3();
        }

        private void dtp6_ValueChanged(object sender, EventArgs e)
        {
            UpdateListViewCheltuieli3();
        }

        #endregion

        #region Firme
        private void LoadFirme()
        {
            try
            {
                listBoxF.DataSource = null;
                listBoxC.DataSource = null;

                using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT idFirma, numeFirma FROM Firme
                        WHERE tipRelatie = 'F'
                    ";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt2 = new DataTable();
                    dt2.Load(reader);

                    listBoxF.DisplayMember = "numeFirma";
                    listBoxF.ValueMember = "idFirma";
                    listBoxF.DataSource = dt2;

                    query = @"
                        SELECT idFirma, numeFirma FROM Firme
                        WHERE tipRelatie = 'C'
                    ";

                    cmd = new SqlCommand(query, conn);
                    reader = cmd.ExecuteReader();
                    DataTable dt3 = new DataTable();
                    dt3.Load(reader);

                    listBoxC.DisplayMember = "numeFirma";
                    listBoxC.ValueMember = "idFirma";
                    listBoxC.DataSource = dt3;


                    // pentru afisarea vagoanelor
                    comboBoxV.DisplayMember = "numeFirma";
                    comboBoxV.ValueMember = "idFirma";
                    comboBoxV.DataSource = dt3;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la încărcarea Firmelor: " + ex.Message);
            }
        }

        private void DisplayFirmaDetails(int idFirma)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
                {

                    conn.Open();
                    string query = @"
                        SELECT _firma.numeFirma, _firma.codFiscal, _firma.registruComercial,
                               _psc.nume, _psc.prenume, _psc.telefon, _psc.email,
                               _adresa.strada, _adresa.numar, _adresa.codPostal,
                               _oras.numeOras, _judet.numeJudet
                        FROM Firme _firma
                        JOIN PersoaneContact _psc ON _firma.idPersoanaContact = _psc.idPersoanaContact
                        JOIN Adrese _adresa ON _firma.idAdresa = _adresa.idAdresa
                        JOIN Orase _oras ON _adresa.idOras = _oras.idOras
                        JOIN JUdete _judet ON _oras.idJudet = _judet.idJudet 
                        WHERE idFirma = @id
                        ";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", idFirma);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        textBoxFirmeDetails.Text =
                           $"Firma: {reader["numeFirma"]}\r\n" +
                           $"Cod fiscal: {reader["codFiscal"]}\r\n" +
                           $"Registru comercial: {reader["registruComercial"]}\r\n\r\n" +

                           $"Persoana contact:\r\n" +
                           $"  Nume: {reader["nume"]} {reader["prenume"]}\r\n" +
                           $"  Telefon: {reader["telefon"]}\r\n" +
                           $"  Email: {reader["email"]}\r\n\r\n" +

                           $"Adresă:\r\n" +
                           $"  Județul {reader["numeJudet"]}, Orașul {reader["numeOras"]},\r\n" +
                           $"  Strada {reader["strada"]} Nr. {reader["numar"]}\r\n\r\n" +
                           $"  Cod postal: {reader["codPostal"]}\r\n";
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la încărcarea firmei: " + ex.Message);
            }

        }
        private void AddFirma(object sender, EventArgs e)
        {
            FormFirma addForm = new FormFirma();
            addForm.ShowDialog();
            LoadFirme();
        }

        private void EditFirma(object sender, EventArgs e)
        {
            if (listBoxF.SelectedItems.Count == 0 && listBoxC.SelectedItems.Count == 0)
                return;

            int idFirma = listBoxF.SelectedItems.Count != 0
                ? Convert.ToInt32(listBoxF.SelectedValue)
                : Convert.ToInt32(listBoxC.SelectedValue);

            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT _firma.numeFirma, _firma.codFiscal, _firma.registruComercial, _firma.tipRelatie, 
                                    _firma.idPersoanaContact, _firma.idAdresa,
                               _psc.nume, _psc.prenume, _psc.telefon, _psc.email,
                               _adresa.strada, _adresa.numar, _adresa.codPostal,
                               _oras.idOras, _oras.numeOras,
                               _judet.idJudet, _judet.numeJudet
                        FROM Firme _firma
                        JOIN PersoaneContact _psc ON _firma.idPersoanaContact = _psc.idPersoanaContact
                        JOIN Adrese _adresa ON _firma.idAdresa = _adresa.idAdresa
                        JOIN Orase _oras ON _adresa.idOras = _oras.idOras
                        JOIN Judete _judet ON _oras.idJudet = _judet.idJudet 
                        WHERE _firma.idFirma = @id
                        ";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", idFirma);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                MessageBox.Show("Nu s-au găsit date pentru această firmă.");
                                return;
                            }

                            // extragem valorile din reader
                            string numeFirma = reader["numeFirma"].ToString();
                            string codFiscal = reader["codFiscal"].ToString();
                            string registruComercial = reader["registruComercial"].ToString();
                            string tipRelatie = reader["tipRelatie"].ToString();

                            int idPersCont = Convert.ToInt32(reader["idPersoanaContact"]);
                            int idAdr = Convert.ToInt32(reader["idAdresa"]);

                            string numeContact = reader["nume"].ToString();
                            string prenumeContact = reader["prenume"].ToString();
                            string telefon = reader["telefon"].ToString();
                            string email = reader["email"].ToString();

                            int idJudet = Convert.ToInt32(reader["idJudet"]);
                            int idOras = Convert.ToInt32(reader["idOras"]);
                            //string numeJudet = reader["numeJudet"].ToString();
                            //string numeOras = reader["numeOras"].ToString();
                            string strada = reader["strada"].ToString();
                            string numar = reader["numar"].ToString();
                            string codPostal = reader["codPostal"].ToString();

                            // creăm formularul de editare
                            FormFirma editForm = new FormFirma(
                                idFirma,
                                idPersCont,
                                idAdr,
                                numeFirma,
                                codFiscal,
                                registruComercial,
                                tipRelatie,
                                numeContact,
                                prenumeContact,
                                telefon,
                                email,
                                idJudet,
                                idOras,
                                strada,
                                numar,
                                codPostal
                            );

                            // afișăm formularul după ce toate datele sunt sigure
                            editForm.ShowDialog();
                            LoadFirme();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la încărcarea firmei: " + ex.Message);
            }
        }

        private void DeleteFirma(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
                {
                    if (listBoxF.SelectedItems.Count == 0 && listBoxC.SelectedItems.Count == 0)
                    {
                        MessageBox.Show("Selectează o firmă pentru ștergere!");
                        return;
                    }

                    // Confirmare
                    DialogResult result = MessageBox.Show(
                        "Sigur doriți să ștergeți firma selectată?",
                        "Confirmare ștergere",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );

                    if (result != DialogResult.Yes)
                        return;

                    conn.Open();
                    SqlTransaction tran = conn.BeginTransaction();

                    try
                    {
                        int idFirma = listBoxF.SelectedItems.Count != 0
                            ? Convert.ToInt32(listBoxF.SelectedValue)
                            : Convert.ToInt32(listBoxC.SelectedValue);

                        // Preluăm idPersoanaContact și idAdresa
                        string selectQuery = "SELECT idPersoanaContact, idAdresa FROM Firme WHERE idFirma = @id";
                        SqlCommand cmd = new SqlCommand(selectQuery, conn, tran);
                        cmd.Parameters.AddWithValue("@id", idFirma);
                        SqlDataReader reader = cmd.ExecuteReader();

                        int idPC = -1, idAdresa = -1;
                        if (reader.Read())
                        {
                            idPC = Convert.ToInt32(reader["idPersoanaContact"]);
                            idAdresa = Convert.ToInt32(reader["idAdresa"]);
                        }
                        reader.Close();

                        // Ștergem firma
                        string deleteFirma = "DELETE FROM Firme WHERE idFirma = @id";
                        cmd = new SqlCommand(deleteFirma, conn, tran);
                        cmd.Parameters.AddWithValue("@id", idFirma);
                        cmd.ExecuteNonQuery();

                        // Ștergem persoana de contact
                        string deletePC = "DELETE FROM PersoaneContact WHERE idPersoanaContact = @idPC";
                        cmd = new SqlCommand(deletePC, conn, tran);
                        cmd.Parameters.AddWithValue("@idPC", idPC);
                        cmd.ExecuteNonQuery();

                        // Ștergem adresa
                        string deleteAdresa = "DELETE FROM Adrese WHERE idAdresa = @idAdresa";
                        cmd = new SqlCommand(deleteAdresa, conn, tran);
                        cmd.Parameters.AddWithValue("@idAdresa", idAdresa);
                        cmd.ExecuteNonQuery();

                        // Commit tranzacție
                        tran.Commit();
                        LoadFirme();

                        //MessageBox.Show("Firma a fost ștearsă cu succes!");
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        MessageBox.Show("Eroare la ștergerea firmei: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare: " + ex.Message);
            }
        }

        #endregion

        #region Piese
        private void ShowPiese()
        {
            try
            {
                listViewP.Items.Clear();

                using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT idPiesa, denumirePiesa, codPiesa, unitateMasuraPiesa
                        FROM Piese
                    ";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        ListViewItem item = new ListViewItem(reader["denumirePiesa"].ToString());
                        item.SubItems.Add(reader["codPiesa"].ToString());
                        item.SubItems.Add(reader["unitateMasuraPiesa"].ToString());
                        item.Tag = reader["idPiesa"];

                        listViewP.Items.Add(item);
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la afișarea detaliilor: " + ex.Message);
            }
        }

        private void EditPiesa(object sender, EventArgs e)
        {
            if (listViewP.SelectedItems.Count == 0) return;

            ListViewItem item = listViewP.SelectedItems[0];
            int id = (int)item.Tag;
            string denumire = item.SubItems[0].Text;
            string cod = item.SubItems[1].Text;
            string unitate = item.SubItems[2].Text;

            FormPiesa editForm = new FormPiesa(id, denumire, cod, unitate);
            editForm.ShowDialog();
            ShowPiese();
        }

        private void AddPiesa(object sender, EventArgs e)
        {
            FormPiesa addForm = new FormPiesa();
            addForm.ShowDialog();
            ShowPiese();
        }

        private void DeletePiesa(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
                {
                    conn.Open();
                    if (listViewP.SelectedItems.Count == 0)
                    {
                        MessageBox.Show("Selectează o piesă pentru ștergere!");
                        return;
                    }

                    // Confirmare
                    DialogResult result = MessageBox.Show(
                        "Sigur doriți să ștergeți piesa selectată?",
                        "Confirmare ștergere",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );

                    if (result != DialogResult.Yes)
                        return;

                    // Luăm ID-ul piesei din Tag
                    ListViewItem selectedItem = listViewP.SelectedItems[0];
                    int idPiesa = Convert.ToInt32(selectedItem.Tag);

                    try
                    {
                        string query = "DELETE FROM Piese WHERE idPiesa = @id";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@id", idPiesa);
                        cmd.ExecuteNonQuery();

                        listViewP.Items.Remove(selectedItem);

                        //MessageBox.Show("Piesa a fost ștearsă cu succes!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Piesa se află probabil într-o factură\n" + "Eroare la ștergere: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare: " + ex.Message);
            }
        }
        #endregion

        #region Servicii
        private void ShowServicii()
        {
            try
            {
                listViewS.Items.Clear();

                using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
                {
                    conn.Open();

                    string query = @"
                        SELECT idServiciu, denumireServiciu, tipServiciu
                        FROM Servicii
                    ";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        ListViewItem item = new ListViewItem(reader["denumireServiciu"].ToString());
                        item.SubItems.Add(reader["tipServiciu"].ToString());
                        item.Tag = reader["idServiciu"];

                        listViewS.Items.Add(item);
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la afișarea detaliilor: " + ex.Message);
            }
        }

        private void EditServiciu(object sender, EventArgs e)
        {
            if (listViewS.SelectedItems.Count == 0) return;

            ListViewItem item = listViewS.SelectedItems[0];
            int id = (int)item.Tag;
            string denumire = item.SubItems[0].Text;
            string tip = item.SubItems[1].Text;

            FormServiciu editForm = new FormServiciu(id, denumire, tip);
            editForm.ShowDialog();
            ShowServicii();
        }

        private void AddServiciu(object sender, EventArgs e)
        {
            FormServiciu addForm = new FormServiciu();
            addForm.ShowDialog();
            ShowServicii();
        }

        private void DeleteServiciu(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
                {
                    conn.Open();
                    if (listViewS.SelectedItems.Count == 0)
                    {
                        MessageBox.Show("Selectează o piesă pentru ștergere!");
                        return;
                    }

                    // Confirmare
                    DialogResult result = MessageBox.Show(
                        "Sigur doriți să ștergeți serviciul selectat?",
                        "Confirmare ștergere",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );

                    if (result != DialogResult.Yes)
                        return;

                    // Luăm ID-ul piesei din Tag
                    ListViewItem selectedItem = listViewS.SelectedItems[0];
                    int idServiciu = Convert.ToInt32(selectedItem.Tag);

                    try
                    {
                        string query = "DELETE FROM Servicii WHERE idServiciu = @id";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@id", idServiciu);
                        cmd.ExecuteNonQuery();

                        listViewS.Items.Remove(selectedItem);

                        //MessageBox.Show("Serviciul a fost șters cu succes!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Serviciul se află probabil într-o factură\n" + "Eroare la ștergere: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare: " + ex.Message);
            }
        }
        #endregion

        #region Județe

        private bool edittingJudete = false;
        private void LoadJudete()
        {
            try
            {
                //listBoxJ.Items.Clear();

                using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
                {
                    conn.Open();
                    string query = "SELECT idJudet, numeJudet FROM Judete ORDER BY numeJudet";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    DataTable dt_j = new DataTable();
                    dt_j.Load(reader);

                    listBoxJ.DisplayMember = "numeJudet";   // afișaj pentru utilizator
                    listBoxJ.ValueMember = "idJudet";       // valoare folosită în cod
                    listBoxJ.DataSource = dt_j;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la încărcarea Judetelor: " + ex.Message);
            }
        }

        private void DoubleClickJudete(object sender, EventArgs e)
        {
            if (listBoxJ.SelectedItems.Count > 0)
            {
                textBoxJ.BackColor = SystemColors.HotTrack;
                textBoxJ.ForeColor = Color.White;
                edittingJudete = true;
                textBoxJ.Text = listBoxJ.Text;
            }
        }
        private void listBoxJ_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBoxJ.BackColor = SystemColors.Window;
            textBoxJ.ForeColor = SystemColors.WindowText;
            edittingJudete = false;
            textBoxJ.Clear();

            LoadOrase();
        }

        private void textBoxJ_Selected(object sender, EventArgs e)
        {
            if (!edittingJudete)
            {
                listBoxJ.ClearSelected();
                listBoxJ.ClearSelected();
            }
        }

        private void AddModifyJudet(object sender, EventArgs e)
        {
            int idJudet = Convert.ToInt32(listBoxJ.SelectedValue);

            if (string.IsNullOrWhiteSpace(textBoxJ.Text))
            {
                MessageBox.Show("Completează Numele Județului");
                return;
            }

            using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
            {
                conn.Open();

                SqlCommand cmd;
                if (edittingJudete)
                {
                    // UPDATE pentru edit
                    string query = @"
                        UPDATE Judete
                        SET numeJudet = @denumire
                        WHERE idJudet = @id
                        ";

                    cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", idJudet); //pentru că doar ăsta are nevoie de el, dar pentru simplitate îl puteam pune și jos.
                }
                else
                {
                    // INSERT pentru add
                    string query = @"
                        INSERT INTO Judete (numeJudet)
                        VALUES (@denumire)
                        ";
                    cmd = new SqlCommand(query, conn);
                }

                cmd.Parameters.AddWithValue("@denumire", textBoxJ.Text);

                cmd.ExecuteNonQuery();
            }

            LoadJudete();
        }

        private void DeleteJudet(object sender, EventArgs e)
        {
            try
            {
                if (listBoxJ.SelectedIndex == -1)
                {
                    MessageBox.Show("Niciun județ selectat!");
                    return;
                }

                DialogResult result = MessageBox.Show(
                    "Sigur doriți să ștergeți județul selectat?",
                    "Confirmare ștergere",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (result != DialogResult.Yes)
                    return;

                using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
                {
                    conn.Open();

                    int idJudet = Convert.ToInt32(listBoxJ.SelectedValue);

                    string query = "DELETE FROM Judete WHERE idJudet = @id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", idJudet);

                    int rows = cmd.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        //MessageBox.Show("Județul a fost șters cu succes!");
                        LoadJudete();
                    }
                    else
                    {
                        MessageBox.Show("Nu s-a găsit județul în baza de date!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare: " + ex.Message);
            }
        }

        #endregion

        #region Orașe

        private bool edittingOrase = false;

        private void LoadOrase()
        {
            if (listBoxJ.SelectedValue == null)
            {
                listBoxO.DataSource = null;
                listBoxO.Items.Clear();
                groupBoxO.Text = "Orașe";
                return;
            }
            try
            {
                groupBoxO.Text = "Orașe din " + listBoxJ.Text;
                int idJudet = Convert.ToInt32(listBoxJ.SelectedValue);

                using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
                {
                    conn.Open();
                    string query = "SELECT idOras, numeOras FROM Orase WHERE idJudet = @idJudet ORDER BY numeOras";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@idJudet", idJudet);

                    SqlDataReader reader = cmd.ExecuteReader();

                    DataTable dt_o = new DataTable();
                    dt_o.Load(reader);

                    listBoxO.DisplayMember = "numeOras";
                    listBoxO.ValueMember = "idOras";
                    listBoxO.DataSource = dt_o;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la încărcarea Orașelor: " + ex.Message);
            }
        }

        private void DoubleClickOrase(object sender, EventArgs e)
        {
            if (listBoxO.SelectedItems.Count > 0)
            {
                textBoxO.BackColor = SystemColors.HotTrack;
                textBoxO.ForeColor = Color.White;
                edittingOrase = true;
                textBoxO.Text = listBoxO.Text;
            }
        }

        private void listBoxO_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBoxO.BackColor = SystemColors.Window;
            textBoxO.ForeColor = SystemColors.WindowText;
            edittingOrase = false;
            textBoxO.Clear();
        }

        private void textBoxO_Selected(object sender, EventArgs e)
        {
            if (!edittingOrase)
                listBoxO.ClearSelected();
        }

        private void AddModifyOras(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxO.Text))
            {
                MessageBox.Show("Completează Numele Orașului");
                return;
            }

            if (listBoxJ.SelectedValue == null)
            {
                MessageBox.Show("Selectează un județ înainte de a adăuga/modifica un oraș.");
                return;
            }

            int idJudet = Convert.ToInt32(listBoxJ.SelectedValue);

            using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
            {
                conn.Open();

                SqlCommand cmd;
                if (edittingOrase)
                {
                    // UPDATE pentru edit
                    int idOras = Convert.ToInt32(listBoxO.SelectedValue);
                    string query = @"
                        UPDATE Orase
                        SET numeOras = @denumire, idJudet = @idJudet
                        WHERE idOras = @id
                        ";

                    cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", idOras);
                    cmd.Parameters.AddWithValue("@idJudet", idJudet);
                }
                else
                {
                    // INSERT pentru add
                    string query = @"
                        INSERT INTO Orase (numeOras, idJudet)
                        VALUES (@denumire, @idJudet)
                        ";
                    cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@idJudet", idJudet); // Adăugăm ID-ul județului părinte
                }

                cmd.Parameters.AddWithValue("@denumire", textBoxO.Text);
                cmd.ExecuteNonQuery();
            }

            LoadOrase(); // Reîncarcă lista de orașe pentru județul curent
            LoadPuncteLucru();
            LoadOraseInPuncteLucru();
        }

        private void DeleteOras(object sender, EventArgs e)
        {
            try
            {
                if (listBoxO.SelectedIndex == -1)
                {
                    MessageBox.Show("Niciun oraș selectat!");
                    return;
                }

                DialogResult result = MessageBox.Show(
                    "Sigur doriți să ștergeți orașul selectat?",
                    "Confirmare ștergere",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (result != DialogResult.Yes)
                    return;

                using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
                {
                    conn.Open();

                    int idOras = Convert.ToInt32(listBoxO.SelectedValue);

                    string query = "DELETE FROM Orase WHERE idOras = @id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", idOras);

                    int rows = cmd.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        //MessageBox.Show("Orașul a fost șters cu succes!");
                        LoadOrase(); // Reîncarcă lista de orașe
                        LoadPuncteLucru();
                        LoadOraseInPuncteLucru();
                    }
                    else
                    {
                        MessageBox.Show("Nu s-a găsit orașul în baza de date!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare: " + ex.Message);
            }
        }

        #endregion

        #region Puncte_Lucru
        private void LoadOraseInPuncteLucru()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT _oras.idOras, _oras.numeOras 
                        FROM Orase _oras
                        LEFT JOIN PuncteLucru _pl ON _oras.idOras = _pl.idOras
                        WHERE _pl.idOras IS NULL
                        ORDER BY numeOras
                        ";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    DataTable dt_opl = new DataTable();

                    dt_opl.Load(reader);

                    comboBoxPL.DisplayMember = "numeOras";
                    comboBoxPL.ValueMember = "idOras";
                    comboBoxPL.DataSource = dt_opl;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la încărcarea Punctelor de Lucru: " + ex.Message);
            }
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

                    listBoxPL.DisplayMember = "numeOras";
                    listBoxPL.ValueMember = "idPunctLucru";
                    listBoxPL.DataSource = dt_pl;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la încărcarea Punctelor de Lucru: " + ex.Message);
            }
        }


        private void AddPunctLucru(object sender, EventArgs e)
        {
            int idPunctLucru = Convert.ToInt32(comboBoxPL.SelectedValue);

            if (comboBoxPL.SelectedIndex == -1)
            {
                MessageBox.Show("Selectează un punct de lucru!");
                return;
            }

            using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
            {
                conn.Open();

                SqlCommand cmd;
                string query = @"
                    INSERT INTO PuncteLucru (idOras)
                    VALUES (@idOras)
                    ";
                cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@idOras", idPunctLucru);

                cmd.ExecuteNonQuery();
            }

            LoadPuncteLucru();
            LoadOraseInPuncteLucru();
        }

        private void DeletePunctLucru(object sender, EventArgs e)
        {
            try
            {
                if (listBoxPL.SelectedIndex == -1)
                {
                    MessageBox.Show("Niciun punct de lucru selectat!");
                    return;
                }

                DialogResult result = MessageBox.Show(
                    "Sigur doriți să ștergeți punctul de lucru selectat?",
                    "Confirmare ștergere",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (result != DialogResult.Yes)
                    return;

                using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
                {
                    conn.Open();

                    int idPunctLucru = Convert.ToInt32(listBoxPL.SelectedValue);

                    string query = "DELETE FROM PuncteLucru WHERE idPunctLucru = @id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", idPunctLucru);

                    int rows = cmd.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        //MessageBox.Show("Punctul de Lucru a fost șters cu succes!");
                        LoadPuncteLucru();
                        LoadOraseInPuncteLucru();
                    }
                    else
                    {
                        MessageBox.Show("Nu s-a găsit punctul de lucru în baza de date!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare: " + ex.Message);
            }
        }

        #endregion

        #region Locomotive & Vagoane

        private void ShowLocomotive()
        {
            try
            {
                listViewL.Items.Clear();

                using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
                {
                    conn.Open();
                    string query = @" 
                        SELECT _locomotiva.idLocomotiva, _locomotiva.numeLocomotiva, _locomotiva.nrEuro, 
                            _locomotiva.tip, _locomotiva.serie, _locomotiva.putere, 
                            _oras.numeOras,
                            _punct.idPunctLucru
                        FROM Locomotive _locomotiva 
                        JOIN PuncteLucru _punct ON _locomotiva.idPunctLucru = _punct.idPunctLucru 
                        JOIN Orase _oras ON _punct.idOras = _oras.idOras 
                    ";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ListViewItem item = new ListViewItem(reader["numeLocomotiva"].ToString());
                        item.SubItems.Add(reader["nrEuro"].ToString());
                        item.SubItems.Add(reader["serie"].ToString());
                        item.SubItems.Add(reader["numeOras"].ToString());
                        item.SubItems.Add(reader["tip"].ToString());
                        item.SubItems.Add(reader["putere"].ToString());
                        //item.Tag = reader["idLocomotiva"]; listViewL.Items.Add(item);

                        item.Tag = new
                        {
                            IdLocomotiva = reader["idLocomotiva"],
                            IdPunctLucru = reader["idPunctLucru"]
                        };
                        listViewL.Items.Add(item);
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la afișarea detaliilor pt locomotive: " + ex.Message);
            }
        }

        private void LoadVagoaneSettings(int idClient)
        {
            try
            {
                listBoxV.DataSource = null;

                using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT idVagon, numarVagon 
                        FROM Vagoane
                        WHERE idProprietar = @idProp
                        ";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@idProp", idClient);
                    SqlDataReader reader = cmd.ExecuteReader();

                    DataTable dt_v = new DataTable();
                    dt_v.Load(reader);

                    listBoxV.DisplayMember = "numarVagon";
                    listBoxV.ValueMember = "idVagon";
                    listBoxV.DataSource = dt_v;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la încărcarea locomotivelor: " + ex.Message);
            }
        }
        private void EditVagon(object sender, EventArgs e)
        {
            if (listBoxV.SelectedItems.Count == 0) return;

            int id = Convert.ToInt32(listBoxV.SelectedValue);
            string numarVagon = ((DataRowView)listBoxV.SelectedItem)["numarVagon"].ToString();
            int idClient = Convert.ToInt32(comboBoxV.SelectedValue);

            FormVagon editForm = new FormVagon(id, numarVagon, idClient);
            editForm.ShowDialog();
            LoadVagoaneSettings(Convert.ToInt32(comboBoxV.SelectedValue));
        }

        private void AddVagon(object sender, EventArgs e)
        {
            int idClient = Convert.ToInt32(comboBoxV.SelectedValue);

            FormVagon addForm = new FormVagon(idClient);
            addForm.ShowDialog();
            LoadVagoaneSettings(Convert.ToInt32(comboBoxV.SelectedValue));
        }

        private void DeleteVagon(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
                {
                    conn.Open();
                    if (listBoxV.SelectedItems.Count == 0)
                    {
                        MessageBox.Show("Selectează un vagon pentru ștergere!");
                        return;
                    }

                    // Confirmare
                    DialogResult result = MessageBox.Show(
                        "Sigur doriți să ștergeți vagonul selectat?",
                        "Confirmare ștergere",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );

                    if (result != DialogResult.Yes)
                        return;

                    int idVagon = Convert.ToInt32(listBoxV.SelectedValue);

                    try
                    {
                        string query = "DELETE FROM Vagoane WHERE idVagon = @idVagon";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@idVagon", idVagon);
                        cmd.ExecuteNonQuery();

                        DataRowView selectedRow = (DataRowView)listBoxV.SelectedItem;
                        selectedRow.Row.Delete();

                        //MessageBox.Show("Vagonul a fost șters cu succes!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Vagonul se află probabil într-o factură\n" + "Eroare la ștergere: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare: " + ex.Message);
            }
        }

        //aici e buba
        private void EditLocomotiva(object sender, EventArgs e)
        {
            if (listViewL.SelectedItems.Count == 0)
            {
                MessageBox.Show("Selectează o locomotivă pentru editare!");
                return;
            }

            ListViewItem selectedItem = listViewL.SelectedItems[0];

            var tag = (dynamic)listViewL.SelectedItems[0].Tag;
            int idLocomotiva = Convert.ToInt32(tag.IdLocomotiva);
            int idPunctLucru = Convert.ToInt32(tag.IdPunctLucru);

            string numeLoc = selectedItem.SubItems[0].Text;
            string nrEuro = selectedItem.SubItems[1].Text;
            string serie = selectedItem.SubItems[2].Text;

            string tip = selectedItem.SubItems[4].Text;
            string putere = selectedItem.SubItems[5].Text;

            FormLocomotiva editForm = new FormLocomotiva(idLocomotiva, numeLoc, nrEuro, idPunctLucru, serie, tip, putere);
            editForm.ShowDialog();
            ShowLocomotive();
        }

        private void AddLocomotiva(object sender, EventArgs e)
        {
            FormLocomotiva addForm = new FormLocomotiva();
            addForm.ShowDialog();
            ShowLocomotive();
        }


        private void DeleteLocomotiva(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
                {
                    conn.Open();
                    if (listViewL.SelectedItems.Count == 0)
                    {
                        MessageBox.Show("Selectează o locomotivă pentru ștergere!");
                        return;
                    }

                    // Confirmare
                    DialogResult result = MessageBox.Show(
                        "Sigur doriți să ștergeți locomotiva selectată?",
                        "Confirmare ștergere",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );

                    if (result != DialogResult.Yes)
                        return;

                    ListViewItem selectedItem = listViewL.SelectedItems[0];
                    var tag = (dynamic)listViewL.SelectedItems[0].Tag;
                    int idLocomotiva = Convert.ToInt32(tag.IdLocomotiva);

                    try
                    {
                        string query = "DELETE FROM Locomotive WHERE idLocomotiva = @idLocomotiva";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@idLocomotiva", idLocomotiva);
                        cmd.ExecuteNonQuery();

                        listViewL.Items.Remove(selectedItem);

                        //MessageBox.Show("Locomotiva a fost ștearsă cu succes!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Locomotiva se află probabil într-o factură\n" + "Eroare la ștergere: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare: " + ex.Message);
            }
        }
        private void comboBoxV_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadVagoaneSettings(Convert.ToInt32(comboBoxV.SelectedValue));
        }

        #endregion

        #region Facturi
        private void LoadFacturi()
        {
            try
            {
                listViewFacturi.Items.Clear();

                using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
                {
                    conn.Open();

                    string query = @"
                        SELECT 
                            f.idFactura,
                            f.dataFactura,
                            cc.denumireCentruCost AS CentruCost, 
                            f.numarFactura,
                            f.sumaTotal,
                            frm.numeFirma
                        FROM Facturi f
                        JOIN CentreCost cc ON f.idCentruCost = cc.idCentruCost
                        JOIN Firme frm ON f.idFurnizor = frm.idFirma
                        ORDER BY f.dataFactura DESC"; // Cele mai recente primele

                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        // Prima coloană: Data
                        DateTime data = Convert.ToDateTime(reader["dataFactura"]);
                        ListViewItem item = new ListViewItem(data.ToString("dd.MM.yyyy"));

                        // A doua coloană: Centru Cost
                        item.SubItems.Add(reader["CentruCost"].ToString());

                        // A treia coloană: Nr. Factură
                        item.SubItems.Add(reader["numarFactura"].ToString());

                        // A patra coloană: Total (formatat ca bani)
                        decimal total = Convert.ToDecimal(reader["sumaTotal"]);
                        item.SubItems.Add(total.ToString("N2") + " RON");

                        // A cincea coloană: Furnizor
                        item.SubItems.Add(reader["numeFirma"].ToString());

                        // 4. IMPORTANT: Salvăm ID-ul în TAG (invizibil) pentru a ști ce să încărcăm jos când dai click
                        item.Tag = reader["idFactura"];

                        // Adăugăm rândul în listă
                        listViewFacturi.Items.Add(item);
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la încărcarea facturilor: " + ex.Message);
            }
        }

        // ASTA FACE MISTO SĂ SE DEA CLEAR MEREU CAND NU E SELECTAT NIMIC
        private void listViewFacturi_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewFacturi.SelectedItems.Count > 0)
            {
                // Recuperăm ID-ul ascuns în Tag
                int idFacturaSelectata = Convert.ToInt32(listViewFacturi.SelectedItems[0].Tag);

                // Aici vom apela metodele pentru listele de jos (urmează să le scriem)
                LoadPieseFactura(idFacturaSelectata);
                LoadServiciiFactura(idFacturaSelectata);
            }
            else
            {
                // Dacă nu e nimic selectat, golim listele de jos
                listViewPiese.Items.Clear();
                listViewServicii.Items.Clear();
            }
        }

        private void LoadPieseFactura(int idFactura)
        {
            try
            {
                listViewPiese.Items.Clear();
                decimal totalPiese = 0; // Vom citi asta din primul rând

                using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
                {
                    conn.Open();

                    // Modificarea: Am pus totul într-un subquery "T" și calculăm TotalGeneral
                    string query = @"
                        SELECT 
                            T.Destinatar, 
                            T.denumirePiesa, 
                            T.cantitate, 
                            T.pretUnitar, 
                            T.Valoare,
                            SUM(T.Valoare) OVER() as TotalGeneral
                        FROM 
                        (
                            SELECT 
                                L.numeLocomotiva AS Destinatar, 
                                P.denumirePiesa, 
                                PL.cantitate, 
                                PL.pretUnitar,
                                (PL.cantitate * PL.pretUnitar) AS Valoare
                            FROM PieseLocomotive PL
                            JOIN Piese P ON PL.idPiesa = P.idPiesa
                            LEFT JOIN Locomotive L ON PL.idLocomotiva = L.idLocomotiva
                            WHERE PL.idFactura = @idFactura

                            UNION ALL

                            SELECT 
                                'Vagon ' + V.numarVagon AS Destinatar, 
                                P.denumirePiesa, 
                                PV.cantitate, 
                                PV.pretUnitar,
                                (PV.cantitate * PV.pretUnitar) AS Valoare
                            FROM PieseVagoane PV
                            JOIN Piese P ON PV.idPiesa = P.idPiesa
                            JOIN Vagoane V ON PV.idVagon = V.idVagon
                            WHERE PV.idFactura = @idFactura
                        ) AS T";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@idFactura", idFactura);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        // Citim restul datelor pentru ListView
                        ListViewItem item = new ListViewItem(reader["Destinatar"].ToString());
                        item.SubItems.Add(reader["denumirePiesa"].ToString());
                        item.SubItems.Add(reader["cantitate"].ToString());

                        decimal pret = Convert.ToDecimal(reader["pretUnitar"]);
                        item.SubItems.Add(pret.ToString("N2"));

                        // Aici doar preluăm totalul calculat deja de SQL
                        // Îl suprascriem la fiecare pas (va fi aceeași valoare), dar e ok, că s-a cerut să se folosească SQL pt asta
                        if (reader["TotalGeneral"] != DBNull.Value)
                        {
                            totalPiese = Convert.ToDecimal(reader["TotalGeneral"]);
                        }

                        listViewPiese.Items.Add(item);
                    }
                    reader.Close();
                }

                // Afișăm totalul preluat din SQL
                labelTotalPiese.Text = "Total Piese: " + totalPiese.ToString("N2") + " RON";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la încărcarea pieselor: " + ex.Message);
            }
        }

        private void LoadServiciiFactura(int idFactura)
        {
            try
            {
                listViewServicii.Items.Clear();
                decimal totalServicii = 0;

                using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
                {
                    conn.Open();

                    string query = @"
                        SELECT 
                            L.numeLocomotiva, 
                            S.denumireServiciu, 
                            SP.cost,
                            (
                                SELECT SUM(cost) 
                                FROM ServiciiPrestate 
                                WHERE idFactura = @idFactura
                            ) AS TotalGeneral
                        FROM ServiciiPrestate SP
                        JOIN Servicii S ON SP.idServiciu = S.idServiciu
                        LEFT JOIN Locomotive L ON SP.idLocomotiva = L.idLocomotiva
                        WHERE SP.idFactura = @idFactura";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@idFactura", idFactura);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        // Partea de citire rămâne IDENTICĂ
                        string numeLocomotiva = reader["numeLocomotiva"] != DBNull.Value
                            ? reader["numeLocomotiva"].ToString()
                            : " ";//General

                        ListViewItem item = new ListViewItem(numeLocomotiva);
                        item.SubItems.Add(reader["denumireServiciu"].ToString());

                        decimal cost = Convert.ToDecimal(reader["cost"]);
                        item.SubItems.Add(cost.ToString("N2") + " RON");

                        // Citim totalul calculat de subcerere
                        if (reader["TotalGeneral"] != DBNull.Value)
                        {
                            totalServicii = Convert.ToDecimal(reader["TotalGeneral"]);
                        }

                        listViewServicii.Items.Add(item);
                    }
                    reader.Close();
                }

                labelTotalServicii.Text = "Total Servicii: " + totalServicii.ToString("N2") + " RON";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la încărcarea serviciilor: " + ex.Message);
            }
        }


        private void LoadCentreCost()
        {
            try
            {
                comboBoxCC.DataSource = null;

                using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
                {
                    conn.Open();
                    string query = "SELECT idCentruCost, denumireCentruCost FROM CentreCost";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    comboBoxCC.DisplayMember = "denumireCentruCost"; // Ce vede utilizatorul
                    comboBoxCC.ValueMember = "idCentruCost";         // Ce folosim în cod (ID-ul)
                    comboBoxCC.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la încărcarea centrelor de cost: " + ex.Message);
            }
        }

        private void btnDeschide_Click(object sender, EventArgs e)
        {
            // 1. Validare: Verificăm dacă utilizatorul a selectat ceva
            if (comboBoxCC.SelectedIndex == -1)
            {
                MessageBox.Show("Te rog selectează un tip de factură din listă!");
                return;
            }

            // 2. Switch în funcție de ce e selectat (Indexul începe de la 0)
            switch (comboBoxCC.SelectedIndex)
            {
                case 0:
                    AdăugareFactură1 form1 = new AdăugareFactură1();
                    form1.ShowDialog(); // Folosim ShowDialog ca să blocheze fereastra din spate până termini
                    break;

                case 1:
                    AdăugareFactură2 form2 = new AdăugareFactură2();
                    form2.ShowDialog();
                    break;

                case 2:
                    AdăugareFactură3 form3 = new AdăugareFactură3();
                    form3.ShowDialog();
                    break;

                case 3:
                    AdăugareFactură4 form4 = new AdăugareFactură4();
                    form4.ShowDialog();
                    break;

                default:
                    MessageBox.Show("Opțiune necunoscută!");
                    break;
            }
            RefreshToateTaburile();
        }

        private void btnStergeFactura_Click(object sender, EventArgs e)
        {
            if (listViewFacturi.SelectedItems.Count == 0)
            {
                MessageBox.Show("Selectează o factură mai întâi!");
                return;
            }

            int idFactura = Convert.ToInt32(listViewFacturi.SelectedItems[0].Tag);

            var confirm = MessageBox.Show(
                "Sigur vrei să ștergi factura selectată?",
                "Confirmare ștergere",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
                {
                    conn.Open();

                    string queryCopil = @"
                        DELETE FROM PieseVagoane WHERE idFactura = @idFactura;
                        DELETE FROM PieseLocomotive WHERE idFactura = @idFactura;
                        DELETE FROM ServiciiPrestate WHERE idFactura = @idFactura;
                    ";

                    using (SqlCommand cmd = new SqlCommand(queryCopil, conn))
                    {
                        cmd.Parameters.AddWithValue("@idFactura", idFactura);
                        cmd.ExecuteNonQuery();
                    }

                    string queryFactura = "DELETE FROM Facturi WHERE idFactura = @idFactura;";
                    using (SqlCommand cmd = new SqlCommand(queryFactura, conn))
                    {
                        cmd.Parameters.AddWithValue("@idFactura", idFactura);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Factura și toate referințele ei au fost șterse!");

                RefreshToateTaburile();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la ștergerea facturii: " + ex.Message);
            }
        }

        private void RefreshListViewFacturi()
        {
            listViewFacturi.Items.Clear();

            using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
            {
                conn.Open();

                string query = @"
                    SELECT 
                        f.idFactura,
                        f.dataFactura,
                        cc.denumireCentruCost AS CentruCost, 
                        f.numarFactura,
                        f.sumaTotal,
                        frm.numeFirma
                    FROM Facturi f
                    JOIN CentreCost cc ON f.idCentruCost = cc.idCentruCost
                    JOIN Firme frm ON f.idFurnizor = frm.idFirma
                    ORDER BY f.dataFactura DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DateTime data = Convert.ToDateTime(reader["dataFactura"]);
                        ListViewItem item = new ListViewItem(data.ToString("dd.MM.yyyy"));
                        item.SubItems.Add(reader["CentruCost"].ToString());
                        item.SubItems.Add(reader["numarFactura"].ToString());
                        decimal total = Convert.ToDecimal(reader["sumaTotal"]);
                        item.SubItems.Add(total.ToString("N2") + " RON");
                        item.SubItems.Add(reader["numeFirma"].ToString());
                        item.Tag = reader["idFactura"];

                        listViewFacturi.Items.Add(item);
                    }
                }
            }

        }


        #endregion

        //-------------------------------------WasteLand-----------------------------------------//
        #region Wasteland
        // 🔹 Eveniment ListBox select
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshDetaliiLocomotive();
        }

        // 🔹 Evenimente DateTimePicker
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            RefreshDetaliiLocomotive();
        }
        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            RefreshDetaliiLocomotive();
        }


        // 🔹 Funcțiile goale rămân neschimbate
        private void label1_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void panel1_Paint(object sender, PaintEventArgs e) { }
        private void listView1_SelectedIndexChanged(object sender, EventArgs e) { }

        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void domainUpDown1_SelectedItemChanged(object sender, EventArgs e)
        {

        }

        private void listBox5_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void listBoxF_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxF.SelectedIndex != -1)
            {
                listBoxC.ClearSelected();
                DisplayFirmaDetails((int)listBoxF.SelectedValue);
            }
        }

        private void listBoxC_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxC.SelectedIndex != -1)
            {
                listBoxF.ClearSelected();
                DisplayFirmaDetails((int)listBoxC.SelectedValue);
            }
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer1_Panel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer1_Panel1_Paint_2(object sender, PaintEventArgs e)
        {

        }

        private void label13_Click_1(object sender, EventArgs e)
        {

        }

        private void listView2_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void label12_Click_1(object sender, EventArgs e)
        {

        }

        private void splitContainer1_Panel1_Paint_3(object sender, PaintEventArgs e)
        {

        }

        private void listViewS_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void splitContainer2_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer2_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void listBoxPL_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBoxFirmeDetails_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBoxDet_Enter(object sender, EventArgs e)
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void panel6_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void button7_Click_1(object sender, EventArgs e)
        {

        }

        private void listViewL_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void splitContainer2_Panel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void split_main_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void gb_servicii_Enter(object sender, EventArgs e)
        {

        }

        private void label3_Click_1(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listViewPiese_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void groupBox7_Enter(object sender, EventArgs e)
        {

        }

        private void label22_Click(object sender, EventArgs e)
        {

        }

        private void panel10_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label18_Click_1(object sender, EventArgs e)
        {

        }

        private void labelInfoLocMain_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
    #endregion
}
//MessageBox.Show("H");
//SoFtWare GHOst
