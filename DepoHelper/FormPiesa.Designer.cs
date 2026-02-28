namespace DepoHelper
{
    partial class FormPiesa
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPiesa));
            textBoxDen = new TextBox();
            textBoxCod = new TextBox();
            textBoxUm = new TextBox();
            button1 = new Button();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            SuspendLayout();
            // 
            // textBoxDen
            // 
            textBoxDen.Font = new Font("Segoe UI", 10F);
            textBoxDen.Location = new Point(12, 46);
            textBoxDen.Name = "textBoxDen";
            textBoxDen.Size = new Size(358, 30);
            textBoxDen.TabIndex = 0;
            // 
            // textBoxCod
            // 
            textBoxCod.Font = new Font("Segoe UI", 10F);
            textBoxCod.Location = new Point(12, 112);
            textBoxCod.Name = "textBoxCod";
            textBoxCod.Size = new Size(358, 30);
            textBoxCod.TabIndex = 1;
            // 
            // textBoxUm
            // 
            textBoxUm.Font = new Font("Segoe UI", 10F);
            textBoxUm.Location = new Point(12, 180);
            textBoxUm.Name = "textBoxUm";
            textBoxUm.Size = new Size(358, 30);
            textBoxUm.TabIndex = 2;
            // 
            // button1
            // 
            button1.Font = new Font("Segoe UI", 11F);
            button1.Location = new Point(12, 234);
            button1.Name = "button1";
            button1.Size = new Size(358, 40);
            button1.TabIndex = 3;
            button1.Text = "Salvează";
            button1.UseVisualStyleBackColor = false;
            button1.Click += btnSalveaza_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 10F);
            label1.Location = new Point(12, 21);
            label1.Name = "label1";
            label1.Size = new Size(129, 23);
            label1.TabIndex = 4;
            label1.Text = "Denumire Piesă";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 10F);
            label2.Location = new Point(12, 87);
            label2.Name = "label2";
            label2.Size = new Size(85, 23);
            label2.TabIndex = 5;
            label2.Text = "Cod Piesă";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 10F);
            label3.Location = new Point(12, 155);
            label3.Name = "label3";
            label3.Size = new Size(151, 23);
            label3.TabIndex = 6;
            label3.Text = "Unitate de Măsură";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.Transparent;
            label4.ForeColor = Color.Red;
            label4.Location = new Point(136, 24);
            label4.Name = "label4";
            label4.Size = new Size(15, 20);
            label4.TabIndex = 7;
            label4.Text = "*";
            // 
            // FormPiesa
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(382, 286);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(button1);
            Controls.Add(textBoxUm);
            Controls.Add(textBoxCod);
            Controls.Add(textBoxDen);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximumSize = new Size(400, 333);
            MinimumSize = new Size(400, 333);
            Name = "FormPiesa";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Piesă";
            Load += FormPiesa_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBoxDen;
        private TextBox textBoxCod;
        private TextBox textBoxUm;
        private Button button1;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
    }
}