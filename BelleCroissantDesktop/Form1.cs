using BelleCroissantDesktop.Models;
using BelleCroissantDesktop.Services;
using System.ComponentModel;
using System.Data;

namespace BelleCroissantDesktop
{
    public partial class Form1 : Form {
        private readonly ApiService _apiService;
        private List<Product> _products;
        private string _currentSortColumn = "";
        private ListSortDirection _currentSortDirection = ListSortDirection.Ascending;

        public Form1() {
            InitializeComponent();
            _apiService = new ApiService();
            ConfigureDataGridView();
            this.Load += Form1_Load;
        }

        private void ConfigureDataGridView() {

            dgvProducts.AutoGenerateColumns = false;


            DataGridViewCheckBoxColumn activeColumn = new DataGridViewCheckBoxColumn();
            activeColumn.HeaderText = "Active";
            activeColumn.DataPropertyName = "Active";
            activeColumn.Name = "Active";
            activeColumn.Width = 60;


            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn {
                HeaderText = "ProductName",
                DataPropertyName = "ProductName",
                Name = "ProductName",
                Width = 150
            });

            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn {
                HeaderText = "Category",
                DataPropertyName = "Category",
                Name = "Category",
                Width = 100
            });

            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn {
                HeaderText = "Price",
                DataPropertyName = "Price",
                Name = "Price",
                Width = 80,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" }
            });

            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn {
                HeaderText = "Cost",
                DataPropertyName = "Cost",
                Name = "Cost",
                Width = 80,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" }
            });

            dgvProducts.Columns.Add(activeColumn);


            DataGridViewButtonColumn editButtonColumn = new DataGridViewButtonColumn();
            editButtonColumn.HeaderText = "Action";
            editButtonColumn.Text = "edit";
            editButtonColumn.Name = "EditButton";
            editButtonColumn.UseColumnTextForButtonValue = true;
            editButtonColumn.Width = 60;
            dgvProducts.Columns.Add(editButtonColumn);

            DataGridViewButtonColumn deleteButtonColumn = new DataGridViewButtonColumn();
            deleteButtonColumn.Text = "delete";
            deleteButtonColumn.Name = "DeleteButton";
            deleteButtonColumn.UseColumnTextForButtonValue = true;
            deleteButtonColumn.Width = 60;
            dgvProducts.Columns.Add(deleteButtonColumn);

         
            dgvProducts.CellClick += DgvProducts_CellClick;
            dgvProducts.ColumnHeaderMouseClick += DgvProducts_ColumnHeaderMouseClick;
        }

        private async void Form1_Load(object sender, EventArgs e) {
            await LoadProducts();
        }

        private async Task LoadProducts() {
            try {

                this.Cursor = Cursors.WaitCursor;

       
                _products = await _apiService.GetProductsAsync();

     
                if (!string.IsNullOrWhiteSpace(txtSearch.Text)) {
                    ApplyFilter();
                }
                else {
              
                    dgvProducts.DataSource = new BindingList<Product>(_products);
                }

        
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex) {
                this.Cursor = Cursors.Default;
                MessageBox.Show($"Erro ao carregar produtos: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvProducts_CellClick(object sender, DataGridViewCellEventArgs e) {
      
            if (e.RowIndex < 0) return;

     
            Product selectedProduct = _products[e.RowIndex];

       
            if (e.ColumnIndex == dgvProducts.Columns["EditButton"].Index) {
                EditProduct(selectedProduct);
            }
  
            else if (e.ColumnIndex == dgvProducts.Columns["DeleteButton"].Index) {
                DeleteProduct(selectedProduct);
            }
        }

        private void DgvProducts_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e) {

            DataGridViewColumn column = dgvProducts.Columns[e.ColumnIndex];


            if (column.Name == "EditButton" || column.Name == "DeleteButton")
                return;

 
            ListSortDirection direction;

 
            if (_currentSortColumn == column.Name) {
                direction = _currentSortDirection == ListSortDirection.Ascending ?
                    ListSortDirection.Descending : ListSortDirection.Ascending;
            }
            else {
           
                direction = ListSortDirection.Ascending;
            }

            _currentSortColumn = column.Name;
            _currentSortDirection = direction;

            SortData();
        }

        private void SortData() {
            if (string.IsNullOrEmpty(_currentSortColumn)) return;

            List<Product> sortedList = new List<Product>(_products);

            switch (_currentSortColumn) {
                case "ProductName":
                    sortedList = _currentSortDirection == ListSortDirection.Ascending ?
                        sortedList.OrderBy(p => p.ProductName).ToList() :
                        sortedList.OrderByDescending(p => p.ProductName).ToList();
                    break;
                case "Category":
                    sortedList = _currentSortDirection == ListSortDirection.Ascending ?
                        sortedList.OrderBy(p => p.Category).ToList() :
                        sortedList.OrderByDescending(p => p.Category).ToList();
                    break;
                case "Price":
                    sortedList = _currentSortDirection == ListSortDirection.Ascending ?
                        sortedList.OrderBy(p => p.Price).ToList() :
                        sortedList.OrderByDescending(p => p.Price).ToList();
                    break;
                case "Cost":
                    sortedList = _currentSortDirection == ListSortDirection.Ascending ?
                        sortedList.OrderBy(p => p.Cost).ToList() :
                        sortedList.OrderByDescending(p => p.Cost).ToList();
                    break;
                case "Active":
                    sortedList = _currentSortDirection == ListSortDirection.Ascending ?
                        sortedList.OrderBy(p => p.Active).ToList() :
                        sortedList.OrderByDescending(p => p.Active).ToList();
                    break;
            }

            dgvProducts.DataSource = new BindingList<Product>(sortedList);
        }

        private void ApplyFilter() {
            string searchText = txtSearch.Text.ToLower();

  
            var filteredProducts = _products.Where(p =>
                p.ProductName.ToLower().Contains(searchText) ||
                p.Category.ToLower().Contains(searchText)).ToList();

            dgvProducts.DataSource = new BindingList<Product>(filteredProducts);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e) {
            ApplyFilter();
        }

        private void btnAddProduct_Click(object sender, EventArgs e) {
         
            ProductForm productForm = new ProductForm();
            if (productForm.ShowDialog() == DialogResult.OK) {
                _ = LoadProducts();
            }
        }

        private void EditProduct(Product product) {
            // Create a product form with the selected product
            ProductForm productForm = new ProductForm(product);

            // Show the form as a dialog
            if (productForm.ShowDialog() == DialogResult.OK) {
                // Reload products
                _ = LoadProducts();
            }
        }

        private async void DeleteProduct(Product product) {
            DialogResult result = MessageBox.Show(
                $"Tem certeza que deseja excluir o produto '{product.ProductName}'?",
                "Confirmar Exclusão",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes) {
                try {
             
                    await _apiService.DeleteProductAsync(product.ProductId);

        
                    MessageBox.Show("Produto excluído com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                  
                    await LoadProducts();
                }
                catch (Exception ex) {
                    MessageBox.Show($"Erro ao excluir produto: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Form1_Load_1(object sender, EventArgs e) {

        }
        
        private void btnOrders_Click(object sender, EventArgs e) {
            // Abrir o formulário de pedidos
            OrdersForm ordersForm = new OrdersForm();
            ordersForm.ShowDialog();
        }
    }
}
