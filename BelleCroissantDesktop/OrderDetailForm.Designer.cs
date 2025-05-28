namespace BelleCroissantDesktop {
    partial class OrderDetailForm {
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
            btnCancel = new Button();
            btnSave = new Button();
            label1 = new Label();
            lblOrderId = new Label();
            label2 = new Label();
            lblCustomerName = new Label();
            label3 = new Label();
            lblOrderDate = new Label();
            label4 = new Label();
            lblTotalAmount = new Label();
            label5 = new Label();
            lblOrderStatus = new Label();
            label6 = new Label();
            cmbStatus = new ComboBox();
            label7 = new Label();
            dgvOrderItems = new DataGridView();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvOrderItems).BeginInit();
            SuspendLayout();
          
           
            panel1.BackColor = Color.FromArgb(51, 51, 76);
            panel1.Controls.Add(lblTitle);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(484, 60);
            panel1.TabIndex = 0;
            
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(12, 18);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(161, 25);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Order details view";
           
            panel2.Controls.Add(btnCancel);
            panel2.Controls.Add(btnSave);
            panel2.Dock = DockStyle.Bottom;
            panel2.Location = new Point(0, 441);
            panel2.Name = "panel2";
            panel2.Size = new Size(484, 60);
            panel2.TabIndex = 1;
         
            btnCancel.BackColor = Color.FromArgb(192, 0, 0);
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            btnCancel.ForeColor = Color.White;
            btnCancel.Location = new Point(250, 15);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(100, 30);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
         
            btnSave.BackColor = Color.FromArgb(0, 150, 136);
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(144, 15);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(100, 30);
            btnSave.TabIndex = 0;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;

            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            label1.Location = new Point(30, 80);
            label1.Name = "label1";
            label1.Size = new Size(65, 17);
            label1.TabIndex = 2;
            label1.Text = "Order ID:";
           
            lblOrderId.AutoSize = true;
            lblOrderId.Font = new Font("Segoe UI", 9.75F);
            lblOrderId.Location = new Point(150, 80);
            lblOrderId.Name = "lblOrderId";
            lblOrderId.Size = new Size(28, 17);
            lblOrderId.TabIndex = 3;
            lblOrderId.Text = "802";
          

            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            label2.Location = new Point(30, 110);
            label2.Name = "label2";
            label2.Size = new Size(114, 17);
            label2.TabIndex = 2;
            label2.Text = "Customer Name:";
            
            lblCustomerName.AutoSize = true;
            lblCustomerName.Font = new Font("Segoe UI", 9.75F);
            lblCustomerName.Location = new Point(150, 110);
            lblCustomerName.Name = "lblCustomerName";
            lblCustomerName.Size = new Size(97, 17);
            lblCustomerName.TabIndex = 3;
            lblCustomerName.Text = "Manon Dupont";
           
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            label3.Location = new Point(30, 140);
            label3.Name = "label3";
            label3.Size = new Size(82, 17);
            label3.TabIndex = 2;
            label3.Text = "Order Date:";
           
            lblOrderDate.AutoSize = true;
            lblOrderDate.Font = new Font("Segoe UI", 9.75F);
            lblOrderDate.Location = new Point(150, 140);
            lblOrderDate.Name = "lblOrderDate";
            lblOrderDate.Size = new Size(70, 17);
            lblOrderDate.TabIndex = 3;
            lblOrderDate.Text = "4/17/2024";
           
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            label4.Location = new Point(30, 170);
            label4.Name = "label4";
            label4.Size = new Size(96, 17);
            label4.TabIndex = 2;
            label4.Text = "Total Amount:";
           
            lblTotalAmount.AutoSize = true;
            lblTotalAmount.Font = new Font("Segoe UI", 9.75F);
            lblTotalAmount.Location = new Point(150, 170);
            lblTotalAmount.Name = "lblTotalAmount";
            lblTotalAmount.Size = new Size(36, 17);
            lblTotalAmount.TabIndex = 3;
            lblTotalAmount.Text = "€6.17";
          
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            label5.Location = new Point(30, 200);
            label5.Name = "label5";
            label5.Size = new Size(92, 17);
            label5.TabIndex = 2;
            label5.Text = "Order Status:";
            
            lblOrderStatus.AutoSize = true;
            lblOrderStatus.Font = new Font("Segoe UI", 9.75F);
            lblOrderStatus.Location = new Point(150, 200);
            lblOrderStatus.Name = "lblOrderStatus";
            lblOrderStatus.Size = new Size(56, 17);
            lblOrderStatus.TabIndex = 3;
            lblOrderStatus.Text = "Pending";
             
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            label6.Location = new Point(30, 230);
            label6.Name = "label6";
            label6.Size = new Size(103, 17);
            label6.TabIndex = 2;
            label6.Text = "Update Status:";
           
            cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStatus.Font = new Font("Segoe UI", 9.75F);
            cmbStatus.FormattingEnabled = true;
            cmbStatus.Location = new Point(150, 227);
            cmbStatus.Name = "cmbStatus";
            cmbStatus.Size = new Size(200, 25);
            cmbStatus.TabIndex = 4;
          
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            label7.Location = new Point(30, 270);
            label7.Name = "label7";
            label7.Size = new Size(86, 17);
            label7.TabIndex = 2;
            label7.Text = "List of items:";
            
            dgvOrderItems.AllowUserToAddRows = false;
            dgvOrderItems.AllowUserToDeleteRows = false;
            dgvOrderItems.BackgroundColor = SystemColors.Control;
            dgvOrderItems.BorderStyle = BorderStyle.None;
            dgvOrderItems.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvOrderItems.Location = new Point(30, 290);
            dgvOrderItems.Name = "dgvOrderItems";
            dgvOrderItems.ReadOnly = true;
            dgvOrderItems.Size = new Size(424, 130);
            dgvOrderItems.TabIndex = 5;
           
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(484, 501);
            Controls.Add(dgvOrderItems);
            Controls.Add(cmbStatus);
            Controls.Add(lblOrderStatus);
            Controls.Add(label5);
            Controls.Add(lblTotalAmount);
            Controls.Add(label4);
            Controls.Add(lblOrderDate);
            Controls.Add(label3);
            Controls.Add(lblCustomerName);
            Controls.Add(label2);
            Controls.Add(lblOrderId);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label1);
            Controls.Add(panel2);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "OrderDetailForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Order Details";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvOrderItems).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }


        private Panel panel1;
        private Label lblTitle;
        private Panel panel2;
        private Button btnCancel;
        private Button btnSave;
        private Label label1;
        private Label lblOrderId;
        private Label label2;
        private Label lblCustomerName;
        private Label label3;
        private Label lblOrderDate;
        private Label label4;
        private Label lblTotalAmount;
        private Label label5;
        private Label lblOrderStatus;
        private Label label6;
        private ComboBox cmbStatus;
        private Label label7;
        private DataGridView dgvOrderItems;
    }
}
