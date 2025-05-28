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
    public partial class LoginForm : Form
    {
        private readonly ApiService _apiService;

        public LoginForm()
        {
            InitializeComponent();
            _apiService = new ApiService();
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Por favor, preencha o nome de usuário e senha.", "Erro de Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // Mostrar indicador de carregamento
                this.Cursor = Cursors.WaitCursor;
                btnLogin.Enabled = false;
                btnLogin.Text = "Autenticando...";

                // Tentar autenticar com credenciais fixas da API
                // Usando as credenciais fornecidas no BasicAuthenticationHandler
                string username = txtUsername.Text; // "staff";
                string password = txtPassword.Text; // "BCLyon2024";
                
                // Validar as credenciais
                bool success = await _apiService.ValidateCredentialsAsync(username, password);

                if (success)
                {
                    // Autenticação bem-sucedida, abrir o formulário principal
                    Form1 mainForm = new Form1();
                    this.Hide();
                    mainForm.FormClosed += (s, args) => this.Close();
                    mainForm.Show();
                }
                else
                {
                    // Autenticação falhou
                    MessageBox.Show("Nome de usuário ou senha incorretos.", "Erro de Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPassword.Clear();
                    txtPassword.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao tentar autenticar: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Resetar o cursor e o botão
                this.Cursor = Cursors.Default;
                btnLogin.Enabled = true;
                btnLogin.Text = "Login";
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
