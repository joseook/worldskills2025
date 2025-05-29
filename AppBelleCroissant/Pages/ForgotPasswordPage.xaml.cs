using AppBelleCroissant.ViewModels;

namespace AppBelleCroissant.Pages
{
    public partial class ForgotPasswordPage : ContentPage
    {
        private readonly ForgotPasswordViewModel _viewModel;

        public ForgotPasswordPage(ForgotPasswordViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            // Resetar os campos e mensagens ao aparecer a página
            _viewModel.ErrorMessage = string.Empty;
            _viewModel.SuccessMessage = string.Empty;
            _viewModel.IsTokenReceived = false;
            _viewModel.Token = string.Empty;
            _viewModel.NewPassword = string.Empty;
        }
    }
}
