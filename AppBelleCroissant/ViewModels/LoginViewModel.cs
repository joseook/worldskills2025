using AppBelleCroissant.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace AppBelleCroissant.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;
        private string _email = string.Empty;
        private string _password = string.Empty;
        private string _errorMessage = string.Empty;

        public LoginViewModel(ApiService apiService)
        {
            _apiService = apiService;
            Title = "Login";
            
            // Usar AsyncCommand para lidar melhor com chamadas assíncronas
            LoginCommand = new Command(
                execute: async () => {
                    try {
                        await LoginAsync();
                    } catch (Exception ex) {
                        ErrorMessage = $"Erro ao processar login: {ex.Message}";
                    }
                }, 
                canExecute: () => !IsBusy && !string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(Password)
            );
            
            RegisterCommand = new Command(
                execute: async () => {
                    try {
                        await GoToRegisterAsync();
                    } catch (Exception ex) {
                        Console.WriteLine($"Erro na navegação: {ex.Message}");
                    }
                }
            );
            
            ForgotPasswordCommand = new Command(
                execute: async () => {
                    try {
                        await GoToForgotPasswordAsync();
                    } catch (Exception ex) {
                        Console.WriteLine($"Erro na navegação: {ex.Message}");
                    }
                }
            );
        }

        public string Email
        {
            get => _email;
            set
            {
                SetProperty(ref _email, value);
                (LoginCommand as Command)?.ChangeCanExecute();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                SetProperty(ref _password, value);
                (LoginCommand as Command)?.ChangeCanExecute();
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }
        public ICommand ForgotPasswordCommand { get; }

        private async Task LoginAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            ErrorMessage = string.Empty;

            try
            {
                var result = await _apiService.LoginAsync(Email, Password);
                bool success = result.success;
                string message = result.message;
                var user = result.user;

                if (success)
                {
                    // Navegar para a página principal após login bem-sucedido
                    await Shell.Current.GoToAsync("//MainPage");
                }
                else
                {
                    ErrorMessage = message;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erro no login: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task GoToRegisterAsync()
        {
            await Shell.Current.GoToAsync("RegisterPage");
        }

        private async Task GoToForgotPasswordAsync()
        {
            await Shell.Current.GoToAsync("ForgotPasswordPage");
        }
    }
}
