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

namespace BelleCroissantDesktop
{
    public partial class ProductForm : Form
    {
        private readonly ApiService _apiService;
        private Product _product;
        private bool _isEditMode;

        // Lista de categorias
        private readonly List<string> _categories = new List<string>
        {
            "Bread",
            "Pastries",
            "Torte",
            "Cake",
            "Cookies",
            "Dessert"
        };

        public ProductForm()
        {
            InitializeComponent();
            _apiService = new ApiService();
            _product = new Product
            {
                IntroducedDate = DateTime.Today,
                Active = true
            };
            _isEditMode = false;

            // Configurar o formulário
            SetupForm();
        }

        public ProductForm(Product product)
        {
            InitializeComponent();
            _apiService = new ApiService();
            _product = product;
            _isEditMode = true;

            // Configurar o formulário
            SetupForm();
            
            // Preencher os campos com os dados do produto
            PopulateFormFields();
        }

        private void SetupForm()
        {
            // Configurar o título do formulário
            this.Text = _isEditMode ? "Editar Produto" : "Adicionar Produto";
            lblFormTitle.Text = _isEditMode ? "Editar Produto" : "Adicionar Produto";

            // Configurar o ComboBox de categorias
            cmbCategory.DataSource = _categories;
            
            // Configurar o DateTimePicker
            dtpIntroducedDate.Format = DateTimePickerFormat.Short;
            dtpIntroducedDate.Value = DateTime.Today;
        }

        private void PopulateFormFields()
        {
            txtProductName.Text = _product.ProductName;
            
            // Selecionar a categoria
            if (_categories.Contains(_product.Category))
            {
                cmbCategory.SelectedItem = _product.Category;
            }
            else if (!string.IsNullOrEmpty(_product.Category))
            {
                // Adicionar a categoria se não existir na lista
                _categories.Add(_product.Category);
                cmbCategory.DataSource = null;
                cmbCategory.DataSource = _categories;
                cmbCategory.SelectedItem = _product.Category;
            }
            
            txtPrice.Text = _product.Price.ToString("F2");
            txtCost.Text = _product.Cost.ToString("F2");
            txtDescription.Text = _product.Description;
            chkSeasonal.Checked = _product.Seasonal;
            chkActive.Checked = _product.Active;
            dtpIntroducedDate.Value = _product.IntroducedDate;
        }

        private bool ValidateForm()
        {
            // Validar nome do produto
            if (string.IsNullOrWhiteSpace(txtProductName.Text))
            {
                MessageBox.Show("O nome do produto é obrigatório.", "Erro de validação", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtProductName.Focus();
                return false;
            }

            if (txtProductName.Text.Length > 100)
            {
                MessageBox.Show("O nome do produto não pode ter mais de 100 caracteres.", "Erro de validação", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtProductName.Focus();
                return false;
            }

            // Validar categoria
            if (string.IsNullOrWhiteSpace(cmbCategory.Text))
            {
                MessageBox.Show("A categoria é obrigatória.", "Erro de validação", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cmbCategory.Focus();
                return false;
            }

            // Validar preço
            if (!decimal.TryParse(txtPrice.Text, out decimal price) || price <= 0)
            {
                MessageBox.Show("O preço deve ser um número positivo.", "Erro de validação", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPrice.Focus();
                return false;
            }

            // Validar custo
            if (!decimal.TryParse(txtCost.Text, out decimal cost) || cost <= 0)
            {
                MessageBox.Show("O custo deve ser um número positivo.", "Erro de validação", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtCost.Focus();
                return false;
            }

            // Validar que o custo é menor que o preço
            if (cost >= price)
            {
                MessageBox.Show("O custo deve ser menor que o preço.", "Erro de validação", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtCost.Focus();
                return false;
            }

            return true;
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
                return;

            try
            {
                // Atualizar o objeto de produto com os valores do formulário
                _product.ProductName = txtProductName.Text;
                _product.Category = cmbCategory.Text;
                _product.Price = decimal.Parse(txtPrice.Text);
                _product.Cost = decimal.Parse(txtCost.Text);
                _product.Description = txtDescription.Text;
                _product.Seasonal = chkSeasonal.Checked;
                _product.Active = chkActive.Checked;
                _product.IntroducedDate = dtpIntroducedDate.Value;

                // Mostrar indicador de carregamento
                this.Cursor = Cursors.WaitCursor;

                // Salvar o produto
                if (_isEditMode)
                {
                    await _apiService.UpdateProductAsync(_product.ProductId, _product);
                    MessageBox.Show("Produto atualizado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    await _apiService.CreateProductAsync(_product);
                    MessageBox.Show("Produto criado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // Resetar o cursor
                this.Cursor = Cursors.Default;

                // Fechar o formulário
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show($"Erro ao salvar produto: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
