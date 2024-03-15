namespace CQA
{
    partial class TelaInicial
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dataGridView1 = new DataGridView();
            dataGridView2 = new DataGridView();
            tlpCentral = new TableLayoutPanel();
            panel1 = new Panel();
            btnRecalcular = new Button();
            lblTotalReceber = new Label();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).BeginInit();
            tlpCentral.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(3, 37);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(398, 598);
            dataGridView1.TabIndex = 1;
            // 
            // dataGridView2
            // 
            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.AllowUserToDeleteRows = false;
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView2.Dock = DockStyle.Fill;
            dataGridView2.Location = new Point(407, 37);
            dataGridView2.Name = "dataGridView2";
            dataGridView2.Size = new Size(1315, 598);
            dataGridView2.TabIndex = 2;
            // 
            // tlpCentral
            // 
            tlpCentral.ColumnCount = 2;
            tlpCentral.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 23.42029F));
            tlpCentral.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 76.57971F));
            tlpCentral.Controls.Add(dataGridView1, 0, 1);
            tlpCentral.Controls.Add(panel1, 0, 2);
            tlpCentral.Controls.Add(dataGridView2, 1, 1);
            tlpCentral.Dock = DockStyle.Fill;
            tlpCentral.Location = new Point(0, 0);
            tlpCentral.Name = "tlpCentral";
            tlpCentral.RowCount = 3;
            tlpCentral.RowStyles.Add(new RowStyle(SizeType.Percent, 5.32915354F));
            tlpCentral.RowStyles.Add(new RowStyle(SizeType.Percent, 94.670845F));
            tlpCentral.RowStyles.Add(new RowStyle(SizeType.Absolute, 72F));
            tlpCentral.Size = new Size(1725, 711);
            tlpCentral.TabIndex = 3;
            // 
            // panel1
            // 
            tlpCentral.SetColumnSpan(panel1, 2);
            panel1.Controls.Add(btnRecalcular);
            panel1.Controls.Add(lblTotalReceber);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(3, 641);
            panel1.Name = "panel1";
            panel1.Size = new Size(1719, 67);
            panel1.TabIndex = 3;
            // 
            // btnRecalcular
            // 
            btnRecalcular.Location = new Point(677, 13);
            btnRecalcular.Name = "btnRecalcular";
            btnRecalcular.Size = new Size(259, 45);
            btnRecalcular.TabIndex = 1;
            btnRecalcular.Text = "Recalcular";
            btnRecalcular.UseVisualStyleBackColor = true;
            btnRecalcular.Click += btnRecalcular_Click;
            // 
            // lblTotalReceber
            // 
            lblTotalReceber.AutoSize = true;
            lblTotalReceber.Font = new Font("Segoe UI", 15F);
            lblTotalReceber.Location = new Point(1451, 39);
            lblTotalReceber.Name = "lblTotalReceber";
            lblTotalReceber.Size = new Size(147, 28);
            lblTotalReceber.TabIndex = 0;
            lblTotalReceber.Text = "TOTAL MENSAL";
            // 
            // TelaInicial
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1725, 711);
            Controls.Add(tlpCentral);
            Name = "TelaInicial";
            Text = "Form1";
            Load += TelaInicial_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).EndInit();
            tlpCentral.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private DataGridView dataGridView1;
        private DataGridView dataGridView2;
        private TableLayoutPanel tlpCentral;
        private Panel panel1;
        private Label lblTotalReceber;
        private Button btnRecalcular;
    }
}
