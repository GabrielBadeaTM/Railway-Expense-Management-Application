namespace DepoHelper
{
    partial class FormVagon
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormVagon));
            button1 = new Button();
            label1 = new Label();
            label2 = new Label();
            textBoxNV = new TextBox();
            comboBoxC = new ComboBox();
            label3 = new Label();
            label4 = new Label();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Font = new Font("Segoe UI", 11F);
            button1.Location = new Point(12, 115);
            button1.Name = "button1";
            button1.Size = new Size(358, 40);
            button1.TabIndex = 5;
            button1.Text = "Salvează";
            button1.UseVisualStyleBackColor = false;
            button1.Click += ButtonSave;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(103, 20);
            label1.TabIndex = 6;
            label1.Text = "Număr Vagon:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 76);
            label2.Name = "label2";
            label2.Size = new Size(117, 20);
            label2.TabIndex = 7;
            label2.Text = "Clinet Deținător:";
            // 
            // textBoxNV
            // 
            textBoxNV.Location = new Point(12, 39);
            textBoxNV.Name = "textBoxNV";
            textBoxNV.Size = new Size(358, 27);
            textBoxNV.TabIndex = 8;
            // 
            // comboBoxC
            // 
            comboBoxC.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxC.FormattingEnabled = true;
            comboBoxC.Location = new Point(150, 72);
            comboBoxC.Name = "comboBoxC";
            comboBoxC.Size = new Size(220, 28);
            comboBoxC.TabIndex = 9;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.ForeColor = Color.Red;
            label3.Location = new Point(114, 9);
            label3.Name = "label3";
            label3.Size = new Size(15, 20);
            label3.TabIndex = 10;
            label3.Text = "*";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.ForeColor = Color.Red;
            label4.Location = new Point(129, 75);
            label4.Name = "label4";
            label4.Size = new Size(15, 20);
            label4.TabIndex = 11;
            label4.Text = "*";
            // 
            // FormVagon
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(382, 167);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(comboBoxC);
            Controls.Add(textBoxNV);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(button1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximumSize = new Size(400, 214);
            MinimumSize = new Size(400, 214);
            Name = "FormVagon";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form2";
            Load += FormVagon_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Label label1;
        private Label label2;
        private TextBox textBoxNV;
        private ComboBox comboBoxC;
        private Label label3;
        private Label label4;
    }
}