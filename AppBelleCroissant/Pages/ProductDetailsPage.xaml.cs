using AppBelleCroissant.ViewModels;

namespace AppBelleCroissant.Pages
{
    public partial class ProductDetailsPage : ContentPage
    {
        private readonly ProductDetailsViewModel _viewModel;

        public ProductDetailsPage(ProductDetailsViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }
    }
}
