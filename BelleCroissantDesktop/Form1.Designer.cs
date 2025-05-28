namespace BelleCroissantDesktop
{
    partial class Form1
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
        private void InitializeComponent() {
            panel1 = new Panel();
            lblTitle = new Label();
            panel2 = new Panel();
            btnAddProduct = new Button();
            txtSearch = new TextBox();
            label1 = new Label();
            dgvProducts = new DataGridView();
            btnOrders = new Button();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvProducts).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(51, 51, 76);
            panel1.Controls.Add(btnOrders);
            panel1.Controls.Add(lblTitle);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(967, 80);
            panel1.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(12, 24);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(227, 30);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Product Management";
            // 
            // panel2
            // 
            panel2.Controls.Add(btnAddProduct);
            panel2.Controls.Add(txtSearch);
            panel2.Controls.Add(label1);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 80);
            panel2.Name = "panel2";
            panel2.Size = new Size(967, 60);
            panel2.TabIndex = 1;
            // 
            // btnAddProduct
            // 
            btnAddProduct.BackColor = Color.FromArgb(0, 150, 136);
            btnAddProduct.FlatAppearance.BorderSize = 0;
            btnAddProduct.FlatStyle = FlatStyle.Flat;
            btnAddProduct.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            btnAddProduct.ForeColor = Color.White;
            btnAddProduct.Location = new Point(845, 15);
            btnAddProduct.Name = "btnAddProduct";
            btnAddProduct.Size = new Size(127, 30);
            btnAddProduct.TabIndex = 2;
            btnAddProduct.Text = "Add new product";
            btnAddProduct.UseVisualStyleBackColor = false;
            btnAddProduct.Click += btnAddProduct_Click;
            // 
            // txtSearch
            // 
            txtSearch.Font = new Font("Segoe UI", 9.75F);
            txtSearch.Location = new Point(183, 18);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(312, 25);
            txtSearch.TabIndex = 1;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9.75F);
            label1.Location = new Point(12, 21);
            label1.Name = "label1";
            label1.Size = new Size(172, 17);
            label1.TabIndex = 0;
            label1.Text = "Search on product/category";
            // 
            // dgvProducts
            // 
            dgvProducts.AllowUserToAddRows = false;
            dgvProducts.AllowUserToDeleteRows = false;
            dgvProducts.BackgroundColor = SystemColors.Control;
            dgvProducts.BorderStyle = BorderStyle.None;
            dgvProducts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvProducts.Dock = DockStyle.Fill;
            dgvProducts.Location = new Point(0, 140);
            dgvProducts.Name = "dgvProducts";
            dgvProducts.ReadOnly = true;
            dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProducts.Size = new Size(967, 421);
            dgvProducts.TabIndex = 2;
            // 
            // btnOrders
            // 
            btnOrders.BackColor = Color.FromArgb(0, 150, 136);
            btnOrders.FlatAppearance.BorderSize = 0;
            btnOrders.FlatStyle = FlatStyle.Flat;
            btnOrders.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            btnOrders.ForeColor = Color.White;
            btnOrders.Location = new Point(800, 24);
            btnOrders.Name = "btnOrders";
            btnOrders.Size = new Size(150, 30);
            btnOrders.TabIndex = 1;
            btnOrders.Text = "Manage Orders";
            btnOrders.UseVisualStyleBackColor = false;
            btnOrders.Click += btnOrders_Click;
            
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(967, 561);
            Controls.Add(dgvProducts);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Belle Croissant Lyonnais - Product Management";
            Load += Form1_Load_1;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvProducts).EndInit();
            ResumeLayout(false);

        }

        #endregion

        private Panel panel1;
        private Label lblTitle;
        private Panel panel2;
        private Button btnAddProduct;
        private TextBox txtSearch;
        private Label label1;
        private DataGridView dgvProducts;
        private Button btnOrders;
    }
}
