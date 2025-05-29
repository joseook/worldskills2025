using AppBelleCroissant.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace AppBelleCroissant.ViewModels
{
    public class ForgotPasswordViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;
        private string _email;
        private string _errorMessage;
        private string _successMessage;
        private bool _isTokenReceived;
        private string _token;
        private string _newPassword;
        private string _confirmPassword;

        public ForgotPasswordViewModel(ApiService apiService)
        {
            _apiService = apiService;
            Title = "Esqueci a Senha";
            
            SendRequestCommand = new Command(async () => await SendRequestAsync(), () => !IsBusy && !string.IsNullOrWhiteSpace(Email));
            ResetPasswordCommand = new Command(async () => await ResetPasswordAsync(), CanResetPassword);
            BackToLoginCommand = new Command(async () => await GoToLoginAsync());
        }

        public string Email
        {
            get => _email;
            set
            {
                SetProperty(ref _email, value);
                (SendRequestCommand as Command)?.ChangeCanExecute();
            }
        }

        public string Token
        {
            get => _token;
            set
            {
                SetProperty(ref _token, value);
                (ResetPasswordCommand as Command)?.ChangeCanExecute();
            }
        }

        public string NewPassword
        {
            get => _newPassword;
            set
            {
                SetProperty(ref _newPassword, value);
                (ResetPasswordCommand as Command)?.ChangeCanExecute();
            }
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                SetProperty(ref _confirmPassword, value);
                (ResetPasswordCommand as Command)?.ChangeCanExecute();
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public string SuccessMessage
        {
            get => _successMessage;
            set => SetProperty(ref _successMessage, value);
        }

        public bool IsTokenReceived
        {
            get => _isTokenReceived;
            set => SetProperty(ref _isTokenReceived, value);
        }

        public ICommand SendRequestCommand { get; }
        public ICommand ResetPasswordCommand { get; }
        public ICommand BackToLoginCommand { get; }

        private bool CanResetPassword()
        {
            return !IsBusy && 
                !string.IsNullOrWhiteSpace(Email) && 
                !string.IsNullOrWhiteSpace(Token) && 
                !string.IsNullOrWhiteSpace(NewPassword) &&
                !string.IsNullOrWhiteSpace(ConfirmPassword);
        }

        private async Task SendRequestAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;

            try
            {
                var result = await _apiService.ForgotPasswordAsync(Email);
                bool success = result.success;
                string message = result.message;
                string token = result.token;

                if (success)
                {
                    SuccessMessage = message;
                    IsTokenReceived = true;
                    
                    // Em ambiente de produção, o token seria enviado por email.
                    // Para teste, podemos mostrar o token para o usuário
                    if (!string.IsNullOrEmpty(token))
                    {
                        Token = token;
                    }
                }
                else
                {
                    ErrorMessage = message;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erro ao processar solicitação: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task ResetPasswordAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;

            try
            {
                var result = await _apiService.ResetPasswordAsync(Token, Email, NewPassword, ConfirmPassword);
                bool success = result.success;
                string message = result.message;

                if (success)
                {
                    SuccessMessage = message;
                    await Task.Delay(2000); // Dar tempo para o usuário ler a mensagem
                    await GoToLoginAsync();
                }
                else
                {
                    ErrorMessage = message;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erro ao redefinir senha: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task GoToLoginAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
