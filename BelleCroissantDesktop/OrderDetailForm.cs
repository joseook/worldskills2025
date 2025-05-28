using BelleCroissantDesktop.Models;
using BelleCroissantDesktop.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BelleCroissantDesktop {
    public partial class OrderDetailForm : Form {
        private readonly ApiService _apiService;
        private readonly Order _order;
        private bool _statusChanged = false;

        public OrderDetailForm(Order order, ApiService apiService) {
            InitializeComponent();
            _order = order;
            _apiService = apiService;

            // Preencher os campos com os dados do pedido
            PopulateOrderDetails();

            // Configurar o ComboBox de status
            SetupStatusComboBox();
        }

        private void PopulateOrderDetails() {
            // Preencher os campos com os dados do pedido
            lblOrderId.Text = _order.TransactionId.ToString();
            lblCustomerName.Text = _order.Customer?.FullName ?? "Unknown";
            lblOrderDate.Text = _order.Date.ToString("d");
            lblTotalAmount.Text = _order.TotalAmount.ToString("C2");
            lblOrderStatus.Text = _order.Status;

            // Preencher a lista de itens
            // Como a API não retorna uma lista de itens, vamos mostrar apenas o produto principal
            if (_order.Product != null) {
                DataTable itemsTable = new DataTable();
                itemsTable.Columns.Add("Item", typeof(string));
                itemsTable.Columns.Add("Quantity", typeof(int));
                itemsTable.Columns.Add("Price", typeof(decimal));

                // Adicionar o produto principal
                itemsTable.Rows.Add(_order.Product.ProductName, _order.Quantity, _order.Price);

                // Vincular a tabela ao DataGridView
                dgvOrderItems.DataSource = itemsTable;

                // Configurar as colunas
                dgvOrderItems.Columns["Item"].Width = 200;
                dgvOrderItems.Columns["Quantity"].Width = 80;
                dgvOrderItems.Columns["Price"].Width = 100;
                dgvOrderItems.Columns["Price"].DefaultCellStyle.Format = "C2";
            }
        }

        private void SetupStatusComboBox() {
            // Adicionar os status disponíveis
            cmbStatus.Items.Add("Pending");
            cmbStatus.Items.Add("Processing");
            cmbStatus.Items.Add("Completed");
            cmbStatus.Items.Add("Cancelled");

            // Selecionar o status atual
            if (!string.IsNullOrEmpty(_order.Status)) {
                cmbStatus.SelectedItem = _order.Status;
            } else {
                cmbStatus.SelectedIndex = 0; // Padrão: Pending
            }

            // Adicionar evento para detectar mudanças
            cmbStatus.SelectedIndexChanged += CmbStatus_SelectedIndexChanged;
        }

        private void CmbStatus_SelectedIndexChanged(object sender, EventArgs e) {
            // Marcar que o status foi alterado
            if (cmbStatus.SelectedItem != null && cmbStatus.SelectedItem.ToString() != _order.Status) {
                _statusChanged = true;
            }
        }

        private async void btnSave_Click(object sender, EventArgs e) {
            // Verificar se houve alteração no status
            if (_statusChanged) {
                try {
                    // Mostrar indicador de carregamento
                    this.Cursor = Cursors.WaitCursor;
                    btnSave.Enabled = false;
                    btnSave.Text = "Salvando...";

                    // Atualizar o status do pedido
                    string newStatus = cmbStatus.SelectedItem?.ToString() ?? "Pending";
                    await _apiService.UpdateOrderStatusAsync(_order.TransactionId, newStatus);

                    // Mostrar mensagem de sucesso
                    MessageBox.Show("Status do pedido atualizado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Fechar o formulário com resultado OK
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                } catch (Exception ex) {
                    MessageBox.Show($"Erro ao atualizar status do pedido: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } finally {
                    // Resetar o cursor e o botão
                    this.Cursor = Cursors.Default;
                    btnSave.Enabled = true;
                    btnSave.Text = "Save";
                }
            } else {
                // Fechar o formulário sem fazer alterações
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            // Fechar o formulário sem fazer alterações
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
