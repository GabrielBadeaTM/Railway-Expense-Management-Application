namespace DepoHelper
{
    partial class FormLocomotiva
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLocomotiva));
            textBoxNL = new TextBox();
            button1 = new Button();
            textBoxNE = new TextBox();
            textBoxS = new TextBox();
            textBoxT = new TextBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label5 = new Label();
            label6 = new Label();
            textBoxP = new TextBox();
            comboBoxPL = new ComboBox();
            label4 = new Label();
            label7 = new Label();
            label8 = new Label();
            label9 = new Label();
            label10 = new Label();
            SuspendLayout();
            // 
            // textBoxNL
            // 
            textBoxNL.Location = new Point(12, 37);
            textBoxNL.Name = "textBoxNL";
            textBoxNL.Size = new Size(358, 27);
            textBoxNL.TabIndex = 0;
            // 
            // button1
            // 
            button1.Font = new Font("Segoe UI", 11F);
            button1.Location = new Point(11, 302);
            button1.Name = "button1";
            button1.Size = new Size(358, 40);
            button1.TabIndex = 4;
            button1.Text = "Salvează";
            button1.UseVisualStyleBackColor = false;
            button1.Click += ButtonSave;
            // 
            // textBoxNE
            // 
            textBoxNE.Location = new Point(12, 97);
            textBoxNE.Name = "textBoxNE";
            textBoxNE.Size = new Size(358, 27);
            textBoxNE.TabIndex = 5;
            // 
            // textBoxS
            // 
            textBoxS.Location = new Point(12, 218);
            textBoxS.Name = "textBoxS";
            textBoxS.Size = new Size(358, 27);
            textBoxS.TabIndex = 6;
            // 
            // textBoxT
            // 
            textBoxT.Location = new Point(63, 260);
            textBoxT.Name = "textBoxT";
            textBoxT.Size = new Size(125, 27);
            textBoxT.TabIndex = 8;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 10F);
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(154, 23);
            label1.TabIndex = 9;
            label1.Text = "Nume Locomotivă:";
            label1.Click += label1_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 10F);
            label2.Location = new Point(12, 69);
            label2.Name = "label2";
            label2.Size = new Size(107, 23);
            label2.TabIndex = 10;
            label2.Text = "Număr Euro:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 10F);
            label3.Location = new Point(12, 190);
            label3.Name = "label3";
            label3.Size = new Size(51, 23);
            label3.TabIndex = 11;
            label3.Text = "Serie:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 10F);
            label5.Location = new Point(12, 262);
            label5.Name = "label5";
            label5.Size = new Size(37, 23);
            label5.TabIndex = 13;
            label5.Text = "Tip:";
            label5.Click += label5_Click;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 10F);
            label6.Location = new Point(202, 262);
            label6.Name = "label6";
            label6.Size = new Size(64, 23);
            label6.TabIndex = 14;
            label6.Text = "Putere:";
            // 
            // textBoxP
            // 
            textBoxP.Location = new Point(280, 260);
            textBoxP.Name = "textBoxP";
            textBoxP.Size = new Size(89, 27);
            textBoxP.TabIndex = 15;
            // 
            // comboBoxPL
            // 
            comboBoxPL.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxPL.FormattingEnabled = true;
            comboBoxPL.Location = new Point(12, 157);
            comboBoxPL.Name = "comboBoxPL";
            comboBoxPL.Size = new Size(358, 28);
            comboBoxPL.TabIndex = 16;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 10F);
            label4.Location = new Point(12, 129);
            label4.Name = "label4";
            label4.Size = new Size(129, 23);
            label4.TabIndex = 17;
            label4.Text = "Punct de Lucru:";
            label4.Click += label4_Click;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.ForeColor = Color.Red;
            label7.Location = new Point(163, 12);
            label7.Name = "label7";
            label7.Size = new Size(15, 20);
            label7.TabIndex = 18;
            label7.Text = "*";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.ForeColor = Color.Red;
            label8.Location = new Point(138, 129);
            label8.Name = "label8";
            label8.Size = new Size(15, 20);
            label8.TabIndex = 19;
            label8.Text = "*";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.ForeColor = Color.Red;
            label9.Location = new Point(42, 260);
            label9.Name = "label9";
            label9.Size = new Size(15, 20);
            label9.TabIndex = 20;
            label9.Text = "*";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.ForeColor = Color.Red;
            label10.Location = new Point(259, 260);
            label10.Name = "label10";
            label10.Size = new Size(15, 20);
            label10.TabIndex = 21;
            label10.Text = "*";
            // 
            // FormLocomotiva
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(382, 353);
            Controls.Add(label10);
            Controls.Add(label9);
            Controls.Add(label8);
            Controls.Add(label7);
            Controls.Add(label4);
            Controls.Add(comboBoxPL);
            Controls.Add(textBoxP);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(textBoxT);
            Controls.Add(textBoxS);
            Controls.Add(textBoxNE);
            Controls.Add(button1);
            Controls.Add(textBoxNL);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximumSize = new Size(400, 400);
            MinimumSize = new Size(400, 400);
            Name = "FormLocomotiva";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form2";
            Load += Form2_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBoxNL;
        private Button button1;
        private TextBox textBoxNE;
        private TextBox textBoxS;
        private TextBox textBoxT;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label5;
        private Label label6;
        private TextBox textBoxP;
        private ComboBox comboBoxPL;
        private Label label4;
        private Label label7;
        private Label label8;
        private Label label9;
        private Label label10;
    }
}