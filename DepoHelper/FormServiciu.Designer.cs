namespace DepoHelper
{
    partial class FormServiciu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormServiciu));
            button1 = new Button();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            textBoxDen = new TextBox();
            textBoxTip = new TextBox();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Font = new Font("Segoe UI", 11F);
            button1.Location = new Point(12, 161);
            button1.Name = "button1";
            button1.Size = new Size(358, 40);
            button1.TabIndex = 0;
            button1.Text = "Salvează";
            button1.UseVisualStyleBackColor = false;
            button1.Click += ButtonSave;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 10F);
            label1.Location = new Point(12, 18);
            label1.Name = "label1";
            label1.Size = new Size(148, 23);
            label1.TabIndex = 1;
            label1.Text = "Denumire Serviciu";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 10F);
            label2.Location = new Point(12, 83);
            label2.Name = "label2";
            label2.Size = new Size(96, 23);
            label2.TabIndex = 2;
            label2.Text = "Tip Serviciu";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 10F);
            label3.ForeColor = Color.Red;
            label3.Location = new Point(154, 18);
            label3.Name = "label3";
            label3.Size = new Size(17, 23);
            label3.TabIndex = 3;
            label3.Text = "*";
            // 
            // textBoxDen
            // 
            textBoxDen.Font = new Font("Segoe UI", 10F);
            textBoxDen.Location = new Point(12, 44);
            textBoxDen.Name = "textBoxDen";
            textBoxDen.Size = new Size(358, 30);
            textBoxDen.TabIndex = 4;
            // 
            // textBoxTip
            // 
            textBoxTip.Font = new Font("Segoe UI", 10F);
            textBoxTip.Location = new Point(12, 109);
            textBoxTip.Name = "textBoxTip";
            textBoxTip.Size = new Size(358, 30);
            textBoxTip.TabIndex = 5;
            // 
            // FormServiciu
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(382, 213);
            Controls.Add(textBoxTip);
            Controls.Add(textBoxDen);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(button1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximumSize = new Size(400, 260);
            MinimumSize = new Size(400, 260);
            Name = "FormServiciu";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form2";
            Load += FormServiciu_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox textBoxDen;
        private TextBox textBoxTip;
    }
}