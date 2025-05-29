using AppBelleCroissant.ViewModels;

namespace AppBelleCroissant.Pages
{
    public partial class RegisterPage : ContentPage
    {
        private readonly RegisterViewModel _viewModel;

        public RegisterPage(RegisterViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            // Resetar os campos e mensagens de erro ao aparecer a página
            _viewModel.ErrorMessage = string.Empty;
        }
    }
}
