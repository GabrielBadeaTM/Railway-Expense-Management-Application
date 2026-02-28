namespace DepoHelper
{
    partial class AdăugareFactură1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdăugareFactură1));
            dtpData = new DateTimePicker();
            txtNumarFactura = new TextBox();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            cbFurnizor = new ComboBox();
            listViewPiese = new ListView();
            columnHeader1 = new ColumnHeader();
            columnHeader2 = new ColumnHeader();
            columnHeader3 = new ColumnHeader();
            columnHeader4 = new ColumnHeader();
            tabControl1 = new TabControl();
            tabPiese = new TabPage();
            panel2 = new Panel();
            btnAdauga1 = new Button();
            label8 = new Label();
            cbLocomotiveJos1 = new ComboBox();
            label6 = new Label();
            label1 = new Label();
            numPret = new NumericUpDown();
            numCantitate = new NumericUpDown();
            listBoxPiese = new ListBox();
            tabServicii = new TabPage();
            panel3 = new Panel();
            btnAdauga2 = new Button();
            numCost = new NumericUpDown();
            label10 = new Label();
            label9 = new Label();
            listBoxServicii = new ListBox();
            cbLocomotiveJos2 = new ComboBox();
            panel1 = new Panel();
            splitContainer1 = new SplitContainer();
            splitContainer2 = new SplitContainer();
            listViewServicii = new ListView();
            columnHeader5 = new ColumnHeader();
            columnHeader6 = new ColumnHeader();
            columnHeader7 = new ColumnHeader();
            label5 = new Label();
            label7 = new Label();
            btnSterge = new Button();
            btnSalveaza = new Button();
            label11 = new Label();
            label12 = new Label();
            label13 = new Label();
            tabControl1.SuspendLayout();
            tabPiese.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numPret).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numCantitate).BeginInit();
            tabServicii.SuspendLayout();
            panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numCost).BeginInit();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            SuspendLayout();
            // 
            // dtpData
            // 
            dtpData.Location = new Point(107, 52);
            dtpData.Name = "dtpData";
            dtpData.Size = new Size(264, 27);
            dtpData.TabIndex = 1;
            // 
            // txtNumarFactura
            // 
            txtNumarFactura.Location = new Point(107, 87);
            txtNumarFactura.Name = "txtNumarFactura";
            txtNumarFactura.Size = new Size(264, 27);
            txtNumarFactura.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 55);
            label2.Name = "label2";
            label2.Size = new Size(41, 20);
            label2.TabIndex = 3;
            label2.Text = "Data";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 90);
            label3.Name = "label3";
            label3.Size = new Size(79, 20);
            label3.TabIndex = 4;
            label3.Text = "Nr. Factură";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 125);
            label4.Name = "label4";
            label4.Size = new Size(62, 20);
            label4.TabIndex = 5;
            label4.Text = "Furnizor";
            // 
            // cbFurnizor
            // 
            cbFurnizor.DropDownStyle = ComboBoxStyle.DropDownList;
            cbFurnizor.FormattingEnabled = true;
            cbFurnizor.Location = new Point(107, 121);
            cbFurnizor.Name = "cbFurnizor";
            cbFurnizor.Size = new Size(264, 28);
            cbFurnizor.TabIndex = 6;
            // 
            // listViewPiese
            // 
            listViewPiese.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3, columnHeader4 });
            listViewPiese.Dock = DockStyle.Fill;
            listViewPiese.FullRowSelect = true;
            listViewPiese.Location = new Point(0, 0);
            listViewPiese.Name = "listViewPiese";
            listViewPiese.Size = new Size(454, 160);
            listViewPiese.TabIndex = 7;
            listViewPiese.UseCompatibleStateImageBehavior = false;
            listViewPiese.View = View.Details;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "Locomotivă";
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
            tabControl1.Controls.Add(tabServicii);
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
            panel2.Controls.Add(btnAdauga1);
            panel2.Controls.Add(label8);
            panel2.Controls.Add(cbLocomotiveJos1);
            panel2.Controls.Add(label6);
            panel2.Controls.Add(label1);
            panel2.Controls.Add(numPret);
            panel2.Controls.Add(numCantitate);
            panel2.Controls.Add(listBoxPiese);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(3, 3);
            panel2.Name = "panel2";
            panel2.Size = new Size(744, 157);
            panel2.TabIndex = 0;
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
            label8.Location = new Point(456, 13);
            label8.Name = "label8";
            label8.Size = new Size(90, 20);
            label8.TabIndex = 13;
            label8.Text = "Locomotivă:";
            // 
            // cbLocomotiveJos1
            // 
            cbLocomotiveJos1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cbLocomotiveJos1.DropDownStyle = ComboBoxStyle.DropDownList;
            cbLocomotiveJos1.FormattingEnabled = true;
            cbLocomotiveJos1.Location = new Point(564, 9);
            cbLocomotiveJos1.Name = "cbLocomotiveJos1";
            cbLocomotiveJos1.Size = new Size(177, 28);
            cbLocomotiveJos1.TabIndex = 12;
            // 
            // label6
            // 
            label6.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label6.AutoSize = true;
            label6.Location = new Point(564, 53);
            label6.Name = "label6";
            label6.Size = new Size(82, 20);
            label6.TabIndex = 11;
            label6.Text = "Preț Unitar:";
            label6.Click += label6_Click;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new Point(456, 53);
            label1.Name = "label1";
            label1.Size = new Size(72, 20);
            label1.TabIndex = 10;
            label1.Text = "Cantitate:";
            // 
            // numPret
            // 
            numPret.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            numPret.Location = new Point(564, 76);
            numPret.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            numPret.Name = "numPret";
            numPret.Size = new Size(177, 27);
            numPret.TabIndex = 9;
            // 
            // numCantitate
            // 
            numCantitate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            numCantitate.Location = new Point(456, 76);
            numCantitate.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            numCantitate.Name = "numCantitate";
            numCantitate.Size = new Size(102, 27);
            numCantitate.TabIndex = 8;
            numCantitate.ValueChanged += numericUpDown1_ValueChanged;
            // 
            // listBoxPiese
            // 
            listBoxPiese.FormattingEnabled = true;
            listBoxPiese.Location = new Point(0, 9);
            listBoxPiese.Name = "listBoxPiese";
            listBoxPiese.Size = new Size(450, 144);
            listBoxPiese.TabIndex = 0;
            listBoxPiese.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // tabServicii
            // 
            tabServicii.Controls.Add(panel3);
            tabServicii.Location = new Point(4, 29);
            tabServicii.Name = "tabServicii";
            tabServicii.Padding = new Padding(3);
            tabServicii.Size = new Size(750, 163);
            tabServicii.TabIndex = 1;
            tabServicii.Text = "Adaugă Servicii";
            tabServicii.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            panel3.Controls.Add(btnAdauga2);
            panel3.Controls.Add(numCost);
            panel3.Controls.Add(label10);
            panel3.Controls.Add(label9);
            panel3.Controls.Add(listBoxServicii);
            panel3.Controls.Add(cbLocomotiveJos2);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(3, 3);
            panel3.Name = "panel3";
            panel3.Size = new Size(744, 157);
            panel3.TabIndex = 0;
            // 
            // btnAdauga2
            // 
            btnAdauga2.Location = new Point(456, 113);
            btnAdauga2.Name = "btnAdauga2";
            btnAdauga2.Size = new Size(285, 29);
            btnAdauga2.TabIndex = 15;
            btnAdauga2.Text = "Adaugă";
            btnAdauga2.UseVisualStyleBackColor = true;
            btnAdauga2.Click += btnAdaugaServiciu_Click;
            // 
            // numCost
            // 
            numCost.Location = new Point(509, 65);
            numCost.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            numCost.Name = "numCost";
            numCost.Size = new Size(232, 27);
            numCost.TabIndex = 14;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(456, 68);
            label10.Name = "label10";
            label10.Size = new Size(41, 20);
            label10.TabIndex = 16;
            label10.Text = "Cost:";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(456, 23);
            label9.Name = "label9";
            label9.Size = new Size(90, 20);
            label9.TabIndex = 15;
            label9.Text = "Locomotivă:";
            // 
            // listBoxServicii
            // 
            listBoxServicii.FormattingEnabled = true;
            listBoxServicii.Location = new Point(0, 9);
            listBoxServicii.Name = "listBoxServicii";
            listBoxServicii.Size = new Size(450, 144);
            listBoxServicii.TabIndex = 0;
            // 
            // cbLocomotiveJos2
            // 
            cbLocomotiveJos2.DropDownStyle = ComboBoxStyle.DropDownList;
            cbLocomotiveJos2.FormattingEnabled = true;
            cbLocomotiveJos2.Location = new Point(552, 19);
            cbLocomotiveJos2.Name = "cbLocomotiveJos2";
            cbLocomotiveJos2.Size = new Size(189, 28);
            cbLocomotiveJos2.TabIndex = 14;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel1.Controls.Add(splitContainer1);
            panel1.Location = new Point(12, 180);
            panel1.Name = "panel1";
            panel1.Size = new Size(758, 361);
            panel1.TabIndex = 9;
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
            splitContainer1.Panel1.Controls.Add(splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(tabControl1);
            splitContainer1.Size = new Size(758, 361);
            splitContainer1.SplitterDistance = 160;
            splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer2.Location = new Point(0, 0);
            splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(listViewPiese);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(listViewServicii);
            splitContainer2.Size = new Size(758, 160);
            splitContainer2.SplitterDistance = 454;
            splitContainer2.TabIndex = 0;
            // 
            // listViewServicii
            // 
            listViewServicii.Columns.AddRange(new ColumnHeader[] { columnHeader5, columnHeader6, columnHeader7 });
            listViewServicii.Dock = DockStyle.Fill;
            listViewServicii.FullRowSelect = true;
            listViewServicii.Location = new Point(0, 0);
            listViewServicii.Name = "listViewServicii";
            listViewServicii.Size = new Size(300, 160);
            listViewServicii.TabIndex = 0;
            listViewServicii.UseCompatibleStateImageBehavior = false;
            listViewServicii.View = View.Details;
            // 
            // columnHeader5
            // 
            columnHeader5.Text = "Locomotiva";
            columnHeader5.Width = 100;
            // 
            // columnHeader6
            // 
            columnHeader6.Text = "Serviciu";
            columnHeader6.Width = 100;
            // 
            // columnHeader7
            // 
            columnHeader7.Text = "Cost";
            columnHeader7.Width = 90;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 12F, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point, 0);
            label5.Location = new Point(12, 9);
            label5.Name = "label5";
            label5.Size = new Size(229, 28);
            label5.TabIndex = 10;
            label5.Text = "Întreținere Locomotive";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(12, 157);
            label7.Name = "label7";
            label7.Size = new Size(118, 20);
            label7.TabIndex = 11;
            label7.Text = "Conținut factură:";
            // 
            // btnSterge
            // 
            btnSterge.Location = new Point(583, 148);
            btnSterge.Name = "btnSterge";
            btnSterge.Size = new Size(187, 29);
            btnSterge.TabIndex = 12;
            btnSterge.Text = "Șterge elementul selectat";
            btnSterge.UseVisualStyleBackColor = true;
            btnSterge.Click += btnSterge_Click;
            // 
            // btnSalveaza
            // 
            btnSalveaza.Location = new Point(381, 52);
            btnSalveaza.Name = "btnSalveaza";
            btnSalveaza.Size = new Size(382, 62);
            btnSalveaza.TabIndex = 13;
            btnSalveaza.Text = "Finalizează și Salvează";
            btnSalveaza.UseVisualStyleBackColor = true;
            btnSalveaza.Click += btnSalveaza_Click;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.ForeColor = Color.Red;
            label11.Location = new Point(47, 55);
            label11.Name = "label11";
            label11.Size = new Size(15, 20);
            label11.TabIndex = 17;
            label11.Text = "*";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.ForeColor = Color.Red;
            label12.Location = new Point(86, 90);
            label12.Name = "label12";
            label12.Size = new Size(15, 20);
            label12.TabIndex = 18;
            label12.Text = "*";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.ForeColor = Color.Red;
            label13.Location = new Point(76, 125);
            label13.Name = "label13";
            label13.Size = new Size(15, 20);
            label13.TabIndex = 19;
            label13.Text = "*";
            // 
            // AdăugareFactură1
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
            Name = "AdăugareFactură1";
            Text = "Adăugare Factură";
            Load += AdăugareFactură1_Load;
            tabControl1.ResumeLayout(false);
            tabPiese.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numPret).EndInit();
            ((System.ComponentModel.ISupportInitialize)numCantitate).EndInit();
            tabServicii.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numCost).EndInit();
            panel1.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
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
        private ListView listViewPiese;
        private TabControl tabControl1;
        private TabPage tabPiese;
        private TabPage tabServicii;
        private Panel panel1;
        private SplitContainer splitContainer1;
        private Label label5;
        private Panel panel2;
        private Panel panel3;
        private ListBox listBoxPiese;
        private Label label6;
        private Label label1;
        private NumericUpDown numPret;
        private NumericUpDown numCantitate;
        private Label label7;
        private SplitContainer splitContainer2;
        private ListView listViewServicii;
        private ListBox listBoxServicii;
        private Label label8;
        private ComboBox cbLocomotiveJos1;
        private Label label9;
        private ComboBox cbLocomotiveJos2;
        private NumericUpDown numCost;
        private Label label10;
        private Button btnAdauga1;
        private Button btnAdauga2;
        private Button btnSterge;
        private Button btnSalveaza;
        private Label label11;
        private Label label12;
        private Label label13;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private ColumnHeader columnHeader3;
        private ColumnHeader columnHeader4;
        private ColumnHeader columnHeader5;
        private ColumnHeader columnHeader6;
        private ColumnHeader columnHeader7;
    }
}