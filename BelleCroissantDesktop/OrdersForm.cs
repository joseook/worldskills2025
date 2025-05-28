using BelleCroissantDesktop.Models;
using BelleCroissantDesktop.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BelleCroissantDesktop {
    public partial class OrdersForm : Form {
        private readonly ApiService _apiService;
        private List<Order> _orders;
        private string _currentSortColumn = "";
        private ListSortDirection _currentSortDirection = ListSortDirection.Ascending;

        public OrdersForm() {
            InitializeComponent();
            _apiService = new ApiService();
            ConfigureDataGridView();
            this.Load += OrdersForm_Load;
        }

        private void ConfigureDataGridView() {
      
            dgvOrders.AutoGenerateColumns = false;

        
            dgvOrders.Columns.Add(new DataGridViewTextBoxColumn {
                HeaderText = "Order ID",
                DataPropertyName = "TransactionId",
                Name = "TransactionId",
                Width = 80
            });

            dgvOrders.Columns.Add(new DataGridViewTextBoxColumn {
                HeaderText = "Customer Name",
                DataPropertyName = "CustomerName",
                Name = "CustomerName",
                Width = 150
            });

            dgvOrders.Columns.Add(new DataGridViewTextBoxColumn {
                HeaderText = "Date",
                DataPropertyName = "Date",
                Name = "Date",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "d" }
            });

            dgvOrders.Columns.Add(new DataGridViewTextBoxColumn {
                HeaderText = "Total Amount",
                DataPropertyName = "TotalAmount",
                Name = "TotalAmount",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" }
            });

            dgvOrders.Columns.Add(new DataGridViewTextBoxColumn {
                HeaderText = "Status",
                DataPropertyName = "Status",
                Name = "Status",
                Width = 100
            });

            DataGridViewButtonColumn detailButtonColumn = new DataGridViewButtonColumn();
            detailButtonColumn.HeaderText = "Order Detail";
            detailButtonColumn.Text = "Detail View";
            detailButtonColumn.Name = "DetailButton";
            detailButtonColumn.UseColumnTextForButtonValue = true;
            detailButtonColumn.Width = 100;
            dgvOrders.Columns.Add(detailButtonColumn);

            dgvOrders.CellClick += DgvOrders_CellClick;
            dgvOrders.ColumnHeaderMouseClick += DgvOrders_ColumnHeaderMouseClick;
        }

        private async void OrdersForm_Load(object sender, EventArgs e) {
            await LoadOrders();
        }

        private async Task LoadOrders() {
            try {
           
                this.Cursor = Cursors.WaitCursor;

       
                _orders = await _apiService.GetOrdersAsync();

              
                if (!string.IsNullOrWhiteSpace(txtSearch.Text)) {
                    ApplyFilter();
                } else {
       
                    var displayOrders = _orders.Select(o => new {
                        o.TransactionId,
                        CustomerName = o.Customer?.FullName ?? "Unknown",
                        o.Date,
                        o.TotalAmount,
                        o.Status
                    }).ToList();

        
                    dgvOrders.DataSource = new BindingList<object>(displayOrders.Cast<object>().ToList());
                }

                this.Cursor = Cursors.Default;
            } catch (Exception ex) {
                this.Cursor = Cursors.Default;
                MessageBox.Show($"Erro ao carregar pedidos: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvOrders_CellClick(object sender, DataGridViewCellEventArgs e) {
       
            if (e.RowIndex < 0) return;

            Order selectedOrder = _orders[e.RowIndex];

            if (e.ColumnIndex == dgvOrders.Columns["DetailButton"].Index) {
                ShowOrderDetail(selectedOrder);
            }
        }

        private void DgvOrders_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e) {
       
            DataGridViewColumn column = dgvOrders.Columns[e.ColumnIndex];

            if (column.Name == "DetailButton")
                return;

   
            ListSortDirection direction;

          
            if (_currentSortColumn == column.Name) {
                direction = _currentSortDirection == ListSortDirection.Ascending ?
                    ListSortDirection.Descending : ListSortDirection.Ascending;
            } else {
              
                direction = ListSortDirection.Ascending;
            }

 
            _currentSortColumn = column.Name;
            _currentSortDirection = direction;

   
            SortData();
        }

        private void SortData() {
            if (string.IsNullOrEmpty(_currentSortColumn)) return;

            var displayOrders = _orders.Select(o => new {
                o.TransactionId,
                CustomerName = o.Customer?.FullName ?? "Unknown",
                o.Date,
                o.TotalAmount,
                o.Status
            }).ToList();

            IEnumerable<dynamic> sortedList;

            switch (_currentSortColumn) {
                case "TransactionId":
                    sortedList = _currentSortDirection == ListSortDirection.Ascending ?
                        displayOrders.OrderBy(o => o.TransactionId) :
                        displayOrders.OrderByDescending(o => o.TransactionId);
                    break;
                case "CustomerName":
                    sortedList = _currentSortDirection == ListSortDirection.Ascending ?
                        displayOrders.OrderBy(o => o.CustomerName) :
                        displayOrders.OrderByDescending(o => o.CustomerName);
                    break;
                case "Date":
                    sortedList = _currentSortDirection == ListSortDirection.Ascending ?
                        displayOrders.OrderBy(o => o.Date) :
                        displayOrders.OrderByDescending(o => o.Date);
                    break;
                case "TotalAmount":
                    sortedList = _currentSortDirection == ListSortDirection.Ascending ?
                        displayOrders.OrderBy(o => o.TotalAmount) :
                        displayOrders.OrderByDescending(o => o.TotalAmount);
                    break;
                case "Status":
                    sortedList = _currentSortDirection == ListSortDirection.Ascending ?
                        displayOrders.OrderBy(o => o.Status) :
                        displayOrders.OrderByDescending(o => o.Status);
                    break;
                default:
                    sortedList = displayOrders;
                    break;
            }

            dgvOrders.DataSource = new BindingList<object>(sortedList.Cast<object>().ToList());
        }

        private void ApplyFilter() {
            string searchText = txtSearch.Text.ToLower();

   
            var filteredOrders = _orders.Where(o =>
                o.TransactionId.ToString().Contains(searchText) ||
                (o.Customer != null && o.Customer.FullName.ToLower().Contains(searchText)) ||
                o.Date.ToString("d").Contains(searchText)).ToList();

            var displayOrders = filteredOrders.Select(o => new {
                o.TransactionId,
                CustomerName = o.Customer?.FullName ?? "Unknown",
                o.Date,
                o.TotalAmount,
                o.Status
            }).ToList();

            dgvOrders.DataSource = new BindingList<object>(displayOrders.Cast<object>().ToList());
        }

        private void txtSearch_TextChanged(object sender, EventArgs e) {
            ApplyFilter();
        }

        private void ShowOrderDetail(Order order) {
            OrderDetailForm detailForm = new OrderDetailForm(order, _apiService);
            if (detailForm.ShowDialog() == DialogResult.OK) {
                _ = LoadOrders();
            }
        }

        private void btnAddOrder_Click(object sender, EventArgs e) {
            MessageBox.Show("Add new order functionality not implemented yet.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
