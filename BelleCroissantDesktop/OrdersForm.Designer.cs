namespace BelleCroissantDesktop {
    partial class OrdersForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            panel1 = new Panel();
            lblTitle = new Label();
            panel2 = new Panel();
            btnAddOrder = new Button();
            txtSearch = new TextBox();
            label1 = new Label();
            dgvOrders = new DataGridView();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvOrders).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(51, 51, 76);
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
            lblTitle.Size = new Size(204, 30);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Order Management";
            // 
            // panel2
            // 
            panel2.Controls.Add(btnAddOrder);
            panel2.Controls.Add(txtSearch);
            panel2.Controls.Add(label1);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 80);
            panel2.Name = "panel2";
            panel2.Size = new Size(967, 60);
            panel2.TabIndex = 1;
            // 
            // btnAddOrder
            // 
            btnAddOrder.BackColor = Color.FromArgb(0, 150, 136);
            btnAddOrder.FlatAppearance.BorderSize = 0;
            btnAddOrder.FlatStyle = FlatStyle.Flat;
            btnAddOrder.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            btnAddOrder.ForeColor = Color.White;
            btnAddOrder.Location = new Point(845, 15);
            btnAddOrder.Name = "btnAddOrder";
            btnAddOrder.Size = new Size(110, 30);
            btnAddOrder.TabIndex = 2;
            btnAddOrder.Text = "Add new order";
            btnAddOrder.UseVisualStyleBackColor = false;
            btnAddOrder.Click += btnAddOrder_Click;
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
            label1.Size = new Size(165, 17);
            label1.TabIndex = 0;
            label1.Text = "Search on ID/Customer/Date";
            // 
            // dgvOrders
            // 
            dgvOrders.AllowUserToAddRows = false;
            dgvOrders.AllowUserToDeleteRows = false;
            dgvOrders.BackgroundColor = SystemColors.Control;
            dgvOrders.BorderStyle = BorderStyle.None;
            dgvOrders.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvOrders.Dock = DockStyle.Fill;
            dgvOrders.Location = new Point(0, 140);
            dgvOrders.Name = "dgvOrders";
            dgvOrders.ReadOnly = true;
            dgvOrders.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvOrders.Size = new Size(967, 421);
            dgvOrders.TabIndex = 2;
            // 
            // OrdersForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(967, 561);
            Controls.Add(dgvOrders);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Name = "OrdersForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Belle Croissant Lyonnais - Order Management";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvOrders).EndInit();
            ResumeLayout(false);
        }

       

        private Panel panel1;
        private Label lblTitle;
        private Panel panel2;
        private Button btnAddOrder;
        private TextBox txtSearch;
        private Label label1;
        private DataGridView dgvOrders;
    }
}
