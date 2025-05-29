using AppBelleCroissant.Models;
using AppBelleCroissant.Services;

using System.Collections.ObjectModel;

using System.Windows.Input;


namespace AppBelleCroissant.ViewModels
{
    public class ProductsViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;
        private ObservableCollection<Product> _products;
        private ObservableCollection<Product> _allProducts;
        private ObservableCollection<string> _categories;
        private string _searchText;
        private bool _isRefreshing;
        private List<string> _selectedCategories;
        private bool _showOnlyFavorites;
        private bool _isLoading;
        private string _selectedCategory;
            
        public ProductsViewModel(ApiService apiService)
        {
            _apiService = apiService;
            Title = "Produtos";
            Products = new ObservableCollection<Product>();
            AllProducts = new ObservableCollection<Product>();
            Categories = new ObservableCollection<string>();
            SelectedCategories = new List<string>();
            
            LoadProductsCommand = new Command(async () => await LoadProductsAsync());
            RefreshCommand = new Command(async () => await RefreshAsync());
            ToggleFavoriteCommand = new Command<Product>(ToggleFavorite);
            SearchCommand = new Command<string>(SearchProducts);
            FilterByCategoryCommand = new Command<string>(FilterByCategory);
            ShowOnlyFavoritesCommand = new Command(ShowOnlyFavorites);
            ViewProductDetailsCommand = new Command<Product>(async (product) => await ViewProductDetailsAsync(product));
        }

        public ObservableCollection<Product> Products
        {
            get => _products;
            set => SetProperty(ref _products, value);
        }

        public ObservableCollection<Product> AllProducts
        {
            get => _allProducts;
            set => SetProperty(ref _allProducts, value);
        }

        public ObservableCollection<string> Categories
        {
            get => _categories;
            set => SetProperty(ref _categories, value);
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value);
                SearchProducts(value);
            }
        }

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        public List<string> SelectedCategories
        {
            get => _selectedCategories;
            set => SetProperty(ref _selectedCategories, value);
        }

        public bool ShowOnlyFavoritesFlag
        {
            get => _showOnlyFavorites;
            set
            {
                SetProperty(ref _showOnlyFavorites, value);
                ApplyFilters();
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public string SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                SetProperty(ref _selectedCategory, value);
                ApplyFilters();
            }
        }

        public ICommand LoadProductsCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand ToggleFavoriteCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand FilterByCategoryCommand { get; }
        public ICommand ShowOnlyFavoritesCommand { get; }
        public ICommand ViewProductDetailsCommand { get; }

        public async Task LoadProductsAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            IsLoading = true;

            try
            {
                var products = await _apiService.GetProductsAsync();
                
                // Carregar favoritos do armazenamento local
                await LoadFavoritesAsync(products);
                
                AllProducts.Clear();
                foreach (var product in products)
                {
                    AllProducts.Add(product);
                }

                // Extrair categorias únicas
                var uniqueCategories = products
                    .Select(p => p.Category)
                    .Distinct()
                    .OrderBy(c => c)
                    .ToList();

                Categories.Clear();
                foreach (var category in uniqueCategories)
                {
                    Categories.Add(category);
                }

                // Adicionar "Todas" no início
                Categories.Insert(0, "Todas");
                SelectedCategory = "Todas";
                
                // Aplicar filtros iniciais
                ApplyFilters();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar produtos: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
                IsLoading = false;
                IsRefreshing = false;
            }
        }

        private async Task RefreshAsync()
        {
            IsRefreshing = true;
            await LoadProductsAsync();
            IsRefreshing = false;
        }

        private async void ToggleFavorite(Product product)
        {
            if (product == null)
                return;

            // Encontrar o produto correspondente na lista de todos os produtos
            var productInAll = AllProducts.FirstOrDefault(p => p.ProductId == product.ProductId);
            if (productInAll != null)
            {
                productInAll.IsFavorite = !productInAll.IsFavorite;
                
                // Salvar favoritos no armazenamento local
                try
                {
                    await SaveFavoritesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao salvar favoritos: {ex.Message}");
                }
                
                // Reaplica filtros para atualizar a UI
                ApplyFilters();
            }
        }

        private void SearchProducts(string searchText)
        {
            ApplyFilters();
        }

        private void FilterByCategory(string category)
        {
            if (SelectedCategories.Contains(category))
            {
                SelectedCategories.Remove(category);
            }
            else
            {
                SelectedCategories.Add(category);
            }
            
            ApplyFilters();
        }

        private void ShowOnlyFavorites()
        {
            ShowOnlyFavoritesFlag = !ShowOnlyFavoritesFlag;
        }

        private void ApplyFilters()
        {
            IEnumerable<Product> filteredProducts = AllProducts;

            // Filtrar por texto de pesquisa
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                filteredProducts = filteredProducts.Where(p => 
                    p.ProductName.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
            }

            // Filtrar por categorias selecionadas
            if (SelectedCategories.Any())
            {
                filteredProducts = filteredProducts.Where(p => 
                    SelectedCategories.Contains(p.Category));
            }

            // Filtrar por favoritos
            if (ShowOnlyFavoritesFlag)
            {
                filteredProducts = filteredProducts.Where(p => p.IsFavorite);
            }

            Products.Clear();
            foreach (var product in filteredProducts)
            {
                Products.Add(product);
            }
        }

        private async Task LoadFavoritesAsync(List<Product> products)
        {
            try
            {
                // Em um cenário real, os favoritos seriam carregados de um armazenamento local
                // como SecureStorage ou Preferences
                
                // Por enquanto, vamos simular isso com um mock
                var favoritesJson = await SecureStorage.GetAsync("favorites");
                if (!string.IsNullOrEmpty(favoritesJson))
                {
                    var favoriteIds = System.Text.Json.JsonSerializer.Deserialize<List<int>>(favoritesJson);
                    foreach (var product in products)
                    {
                        product.IsFavorite = favoriteIds.Contains(product.ProductId);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar favoritos: {ex.Message}");
            }
        }

        private async Task SaveFavoritesAsync()
        {
            try
            {
                var favoriteIds = AllProducts
                    .Where(p => p.IsFavorite)
                    .Select(p => p.ProductId)
                    .ToList();
                
                var favoritesJson = System.Text.Json.JsonSerializer.Serialize(favoriteIds);
                await SecureStorage.SetAsync("favorites", favoritesJson);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao salvar favoritos: {ex.Message}");
            }
        }

        private async Task ViewProductDetailsAsync(Product product)
        {
            if (product == null)
                return;

            // Passar o ID do produto para a página de detalhes
            var parameters = new Dictionary<string, object>
            {
                { "productId", product.ProductId }
            };

            await Shell.Current.GoToAsync("ProductDetailsPage", parameters);
        }
    }
}
