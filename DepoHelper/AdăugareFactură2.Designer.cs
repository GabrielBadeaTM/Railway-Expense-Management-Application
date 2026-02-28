namespace DepoHelper
{
    partial class AdăugareFactură2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdăugareFactură2));
            dtpData = new DateTimePicker();
            txtNumarFactura = new TextBox();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            cbFurnizor = new ComboBox();
            splitContainer1 = new SplitContainer();
            listViewPiese = new ListView();
            columnHeader1 = new ColumnHeader();
            columnHeader2 = new ColumnHeader();
            columnHeader3 = new ColumnHeader();
            columnHeader4 = new ColumnHeader();
            tabControl1 = new TabControl();
            tabPiese = new TabPage();
            panel2 = new Panel();
            label14 = new Label();
            cbVagoane = new ComboBox();
            label15 = new Label();
            label16 = new Label();
            numPret_tt = new NumericUpDown();
            numCantitate_tt = new NumericUpDown();
            btnAdauga1 = new Button();
            label8 = new Label();
            label6 = new Label();
            label1 = new Label();
            listBoxPiese = new ListBox();
            panel1 = new Panel();
            label5 = new Label();
            label7 = new Label();
            btnSterge = new Button();
            btnSalveaza = new Button();
            label11 = new Label();
            label12 = new Label();
            label13 = new Label();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPiese.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numPret_tt).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numCantitate_tt).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // dtpData
            // 
            dtpData.Location = new Point(107, 53);
            dtpData.Name = "dtpData";
            dtpData.Size = new Size(264, 27);
            dtpData.TabIndex = 20;
            // 
            // txtNumarFactura
            // 
            txtNumarFactura.Location = new Point(107, 88);
            txtNumarFactura.Name = "txtNumarFactura";
            txtNumarFactura.Size = new Size(264, 27);
            txtNumarFactura.TabIndex = 21;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 56);
            label2.Name = "label2";
            label2.Size = new Size(41, 20);
            label2.TabIndex = 22;
            label2.Text = "Data";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 91);
            label3.Name = "label3";
            label3.Size = new Size(79, 20);
            label3.TabIndex = 23;
            label3.Text = "Nr. Factură";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 126);
            label4.Name = "label4";
            label4.Size = new Size(62, 20);
            label4.TabIndex = 24;
            label4.Text = "Furnizor";
            // 
            // cbFurnizor
            // 
            cbFurnizor.DropDownStyle = ComboBoxStyle.DropDownList;
            cbFurnizor.FormattingEnabled = true;
            cbFurnizor.Location = new Point(107, 122);
            cbFurnizor.Name = "cbFurnizor";
            cbFurnizor.Size = new Size(264, 28);
            cbFurnizor.TabIndex = 25;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(listViewPiese);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(tabControl1);
            splitContainer1.Size = new Size(758, 360);
            splitContainer1.SplitterDistance = 163;
            splitContainer1.TabIndex = 0;
            // 
            // listViewPiese
            // 
            listViewPiese.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3, columnHeader4 });
            listViewPiese.Dock = DockStyle.Fill;
            listViewPiese.FullRowSelect = true;
            listViewPiese.Location = new Point(0, 0);
            listViewPiese.Name = "listViewPiese";
            listViewPiese.Size = new Size(758, 163);
            listViewPiese.TabIndex = 7;
            listViewPiese.UseCompatibleStateImageBehavior = false;
            listViewPiese.View = View.Details;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "Vagon";
            columnHeader1.Width = 120;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "Piesa";
            columnHeader2.Width = 160;
            // 
            // columnHeader3
            // 
            columnHeader3.Text = "Cantitate";
            columnHeader3.Width = 80;
            // 
            // columnHeader4
            // 
            columnHeader4.Text = "P. Unit";
            columnHeader4.Width = 90;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPiese);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.MaximumSize = new Size(758, 196);
            tabControl1.MinimumSize = new Size(758, 196);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(758, 196);
            tabControl1.TabIndex = 8;
            // 
            // tabPiese
            // 
            tabPiese.Controls.Add(panel2);
            tabPiese.Location = new Point(4, 29);
            tabPiese.Name = "tabPiese";
            tabPiese.Padding = new Padding(3);
            tabPiese.Size = new Size(750, 163);
            tabPiese.TabIndex = 0;
            tabPiese.Text = "Adaugă Piese";
            tabPiese.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            panel2.Controls.Add(label14);
            panel2.Controls.Add(cbVagoane);
            panel2.Controls.Add(label15);
            panel2.Controls.Add(label16);
            panel2.Controls.Add(numPret_tt);
            panel2.Controls.Add(numCantitate_tt);
            panel2.Controls.Add(btnAdauga1);
            panel2.Controls.Add(label8);
            panel2.Controls.Add(label6);
            panel2.Controls.Add(label1);
            panel2.Controls.Add(listBoxPiese);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(3, 3);
            panel2.Name = "panel2";
            panel2.Size = new Size(744, 157);
            panel2.TabIndex = 0;
            // 
            // label14
            // 
            label14.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label14.AutoSize = true;
            label14.Location = new Point(456, 13);
            label14.Name = "label14";
            label14.Size = new Size(54, 20);
            label14.TabIndex = 20;
            label14.Text = "Vagon:";
            // 
            // cbVagoane
            // 
            cbVagoane.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cbVagoane.DropDownStyle = ComboBoxStyle.DropDownList;
            cbVagoane.FormattingEnabled = true;
            cbVagoane.Location = new Point(564, 9);
            cbVagoane.Name = "cbVagoane";
            cbVagoane.Size = new Size(177, 28);
            cbVagoane.TabIndex = 19;
            // 
            // label15
            // 
            label15.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label15.AutoSize = true;
            label15.Location = new Point(564, 51);
            label15.Name = "label15";
            label15.Size = new Size(82, 20);
            label15.TabIndex = 18;
            label15.Text = "Preț Unitar:";
            // 
            // label16
            // 
            label16.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label16.AutoSize = true;
            label16.Location = new Point(456, 51);
            label16.Name = "label16";
            label16.Size = new Size(72, 20);
            label16.TabIndex = 17;
            label16.Text = "Cantitate:";
            // 
            // numPret_tt
            // 
            numPret_tt.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            numPret_tt.Location = new Point(564, 74);
            numPret_tt.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            numPret_tt.Name = "numPret_tt";
            numPret_tt.Size = new Size(177, 27);
            numPret_tt.TabIndex = 16;
            // 
            // numCantitate_tt
            // 
            numCantitate_tt.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            numCantitate_tt.Location = new Point(456, 74);
            numCantitate_tt.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            numCantitate_tt.Name = "numCantitate_tt";
            numCantitate_tt.Size = new Size(102, 27);
            numCantitate_tt.TabIndex = 15;
            // 
            // btnAdauga1
            // 
            btnAdauga1.Location = new Point(456, 118);
            btnAdauga1.Name = "btnAdauga1";
            btnAdauga1.Size = new Size(285, 29);
            btnAdauga1.TabIndex = 14;
            btnAdauga1.Text = "Adaugă";
            btnAdauga1.UseVisualStyleBackColor = true;
            btnAdauga1.Click += btnAdaugaPiesa_Click;
            // 
            // label8
            // 
            label8.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label8.AutoSize = true;
            label8.Location = new Point(1000, 13);
            label8.Name = "label8";
            label8.Size = new Size(90, 20);
            label8.TabIndex = 13;
            label8.Text = "Locomotivă:";
            // 
            // label6
            // 
            label6.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label6.AutoSize = true;
            label6.Location = new Point(1108, 53);
            label6.Name = "label6";
            label6.Size = new Size(82, 20);
            label6.TabIndex = 11;
            label6.Text = "Preț Unitar:";
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new Point(1000, 53);
            label1.Name = "label1";
            label1.Size = new Size(72, 20);
            label1.TabIndex = 10;
            label1.Text = "Cantitate:";
            // 
            // listBoxPiese
            // 
            listBoxPiese.FormattingEnabled = true;
            listBoxPiese.Location = new Point(0, 9);
            listBoxPiese.Name = "listBoxPiese";
            listBoxPiese.Size = new Size(450, 144);
            listBoxPiese.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel1.Controls.Add(splitContainer1);
            panel1.Location = new Point(12, 181);
            panel1.Name = "panel1";
            panel1.Size = new Size(758, 360);
            panel1.TabIndex = 26;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 12F, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point, 0);
            label5.Location = new Point(12, 10);
            label5.Name = "label5";
            label5.Size = new Size(199, 28);
            label5.TabIndex = 27;
            label5.Text = "Întreținere Vagoane";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(12, 158);
            label7.Name = "label7";
            label7.Size = new Size(118, 20);
            label7.TabIndex = 28;
            label7.Text = "Conținut factură:";
            // 
            // btnSterge
            // 
            btnSterge.Location = new Point(583, 149);
            btnSterge.Name = "btnSterge";
            btnSterge.Size = new Size(187, 29);
            btnSterge.TabIndex = 29;
            btnSterge.Text = "Șterge elementul selectat";
            btnSterge.UseVisualStyleBackColor = true;
            btnSterge.Click += btnSterge_Click;
            // 
            // btnSalveaza
            // 
            btnSalveaza.Location = new Point(381, 53);
            btnSalveaza.Name = "btnSalveaza";
            btnSalveaza.Size = new Size(382, 62);
            btnSalveaza.TabIndex = 30;
            btnSalveaza.Text = "Finalizează și Salvează";
            btnSalveaza.UseVisualStyleBackColor = true;
            btnSalveaza.Click += btnSalveaza_Click;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.ForeColor = Color.Red;
            label11.Location = new Point(47, 56);
            label11.Name = "label11";
            label11.Size = new Size(15, 20);
            label11.TabIndex = 31;
            label11.Text = "*";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.ForeColor = Color.Red;
            label12.Location = new Point(86, 91);
            label12.Name = "label12";
            label12.Size = new Size(15, 20);
            label12.TabIndex = 32;
            label12.Text = "*";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.ForeColor = Color.Red;
            label13.Location = new Point(76, 126);
            label13.Name = "label13";
            label13.Size = new Size(15, 20);
            label13.TabIndex = 33;
            label13.Text = "*";
            // 
            // AdăugareFactură2
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(782, 553);
            Controls.Add(label13);
            Controls.Add(label12);
            Controls.Add(label11);
            Controls.Add(btnSalveaza);
            Controls.Add(btnSterge);
            Controls.Add(label7);
            Controls.Add(label5);
            Controls.Add(panel1);
            Controls.Add(cbFurnizor);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(txtNumarFactura);
            Controls.Add(dtpData);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximumSize = new Size(800, 600);
            MinimumSize = new Size(800, 600);
            Name = "AdăugareFactură2";
            Text = "Adăugare Factură";
            Load += AdăugareFactură2_Load;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            tabPiese.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numPret_tt).EndInit();
            ((System.ComponentModel.ISupportInitialize)numCantitate_tt).EndInit();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DateTimePicker dtpData;
        private TextBox txtNumarFactura;
        private Label label2;
        private Label label3;
        private Label label4;
        private ComboBox cbFurnizor;
        private SplitContainer splitContainer1;
        private ListView listViewPiese;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private ColumnHeader columnHeader3;
        private ColumnHeader columnHeader4;
        private TabControl tabControl1;
        private TabPage tabPiese;
        private Panel panel2;
        private Label label14;
        private ComboBox cbVagoane;
        private Label label15;
        private Label label16;
        private NumericUpDown numPret_tt;
        private NumericUpDown numCantitate_tt;
        private Button btnAdauga1;
        private Label label8;
        private Label label6;
        private Label label1;
        private ListBox listBoxPiese;
        private Panel panel1;
        private Label label5;
        private Label label7;
        private Button btnSterge;
        private Button btnSalveaza;
        private Label label11;
        private Label label12;
        private Label label13;
    }
}