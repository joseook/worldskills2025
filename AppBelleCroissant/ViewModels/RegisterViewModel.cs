using AppBelleCroissant.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace AppBelleCroissant.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;
        private string _email;
        private string _password;
        private string _confirmPassword;
        private string _firstName;
        private string _lastName;
        private string _errorMessage;

        public RegisterViewModel(ApiService apiService)
        {
            _apiService = apiService;
            Title = "Criar Conta";
            
            RegisterCommand = new Command(async () => await RegisterAsync(), CanRegister);
            LoginCommand = new Command(async () => await GoToLoginAsync());
        }

        public string Email
        {
            get => _email;
            set
            {
                SetProperty(ref _email, value);
                (RegisterCommand as Command)?.ChangeCanExecute();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                SetProperty(ref _password, value);
                (RegisterCommand as Command)?.ChangeCanExecute();
            }
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                SetProperty(ref _confirmPassword, value);
                (RegisterCommand as Command)?.ChangeCanExecute();
            }
        }

        public string FirstName
        {
            get => _firstName;
            set
            {
                SetProperty(ref _firstName, value);
                (RegisterCommand as Command)?.ChangeCanExecute();
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                SetProperty(ref _lastName, value);
                (RegisterCommand as Command)?.ChangeCanExecute();
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public ICommand RegisterCommand { get; }
        public ICommand LoginCommand { get; }

        private bool CanRegister()
        {
            return !IsBusy && 
                !string.IsNullOrWhiteSpace(Email) && 
                !string.IsNullOrWhiteSpace(Password) && 
                !string.IsNullOrWhiteSpace(ConfirmPassword) && 
                !string.IsNullOrWhiteSpace(FirstName) && 
                !string.IsNullOrWhiteSpace(LastName) && 
                Password == ConfirmPassword;
        }

        private async Task RegisterAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            ErrorMessage = string.Empty;

            try
            {
                var result = await _apiService.RegisterAsync(
                    Email, Password, ConfirmPassword, FirstName, LastName);
                bool success = result.success;
                string message = result.message;
                var user = result.user;

                if (success)
                {
                    // Navegar para a página principal após registro bem-sucedido
                    await Shell.Current.GoToAsync("//MainPage");
                }
                else
                {
                    ErrorMessage = message;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erro no registro: {ex.Message}";
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
