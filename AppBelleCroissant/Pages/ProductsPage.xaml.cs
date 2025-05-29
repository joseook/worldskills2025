using AppBelleCroissant.ViewModels;

namespace AppBelleCroissant.Pages
{
    public partial class ProductsPage : ContentPage
    {
        private readonly ProductsViewModel _viewModel;

        public ProductsPage(ProductsViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            // Carregar produtos ao abrir a página se não houver nenhum
            if (_viewModel.Products.Count == 0)
            {
                _viewModel.LoadProductsCommand.Execute(null);
            }
        }

        private async void OnCategoriesButtonClicked(object sender, EventArgs e)
        {
            if (_viewModel.Categories.Count == 0)
                return;

            var categories = _viewModel.Categories.ToList();
            var selectedCategories = _viewModel.SelectedCategories;

            var actions = new List<string>();
            foreach (var category in categories)
            {
                var prefix = selectedCategories.Contains(category) ? "✓ " : "";
                actions.Add($"{prefix}{category}");
            }
            actions.Add("Cancelar");

            var result = await DisplayActionSheet("Selecione as Categorias", null, null, actions.ToArray());
            
            if (result != null && result != "Cancelar")
            {
                // Remove o prefixo de seleção se houver
                var selectedCategory = result.StartsWith("✓ ") ? result.Substring(2) : result;
                _viewModel.FilterByCategoryCommand.Execute(selectedCategory);
            }
        }
    }
}
