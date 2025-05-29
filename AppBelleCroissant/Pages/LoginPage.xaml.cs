using AppBelleCroissant.ViewModels;

namespace AppBelleCroissant.Pages
{
    public partial class LoginPage : ContentPage
    {
        private readonly LoginViewModel _viewModel;

        public LoginPage(LoginViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            // Resetar os campos ao aparecer a página
            _viewModel.Email = string.Empty;
            _viewModel.Password = string.Empty;
            _viewModel.ErrorMessage = string.Empty;
        }
    }
}
