using AppBelleCroissant.Models;
using AppBelleCroissant.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace AppBelleCroissant.ViewModels
{
    [QueryProperty(nameof(ProductId), "productId")]
    public class ProductDetailsViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;
        private int _productId;
        private Product _product;
        
        public ProductDetailsViewModel(ApiService apiService)
        {
            _apiService = apiService;
            Title = "Detalhes do Produto";
            
            ToggleFavoriteCommand = new Command(ToggleFavorite);
            BackCommand = new Command(async () => await GoBackAsync());
        }

        public int ProductId
        {
            get => _productId;
            set
            {
                SetProperty(ref _productId, value);
                _ = LoadProductAsync(value); 
            }
        }

        public Product Product
        {
            get => _product;
            set => SetProperty(ref _product, value);
        }

        public ICommand ToggleFavoriteCommand { get; }
        public ICommand BackCommand { get; }

        private async Task LoadProductAsync(int productId)
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                // Carregar detalhes do produto da API
                var product = await _apiService.GetProductByIdAsync(productId);
                
                if (product != null)
                {
                    // Verificar se é um favorito no armazenamento local
                    await CheckIfFavoriteAsync(product);
                    Product = product;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar detalhes do produto: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task CheckIfFavoriteAsync(Product product)
        {
            try
            {
                // Em um cenário real, verificar favoritos no armazenamento local
                var favoritesJson = await SecureStorage.GetAsync("favorites");
                if (!string.IsNullOrEmpty(favoritesJson))
                {
                    var favoriteIds = System.Text.Json.JsonSerializer.Deserialize<List<int>>(favoritesJson);
                    product.IsFavorite = favoriteIds.Contains(product.ProductId);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao verificar favoritos: {ex.Message}");
            }
        }

        private async void ToggleFavorite()
        {
            if (Product == null)
                return;

            Product.IsFavorite = !Product.IsFavorite;
            
            try
            {
                // Em um cenário real, salvar favoritos no armazenamento local
                var favoritesJson = await SecureStorage.GetAsync("favorites");
                var favoriteIds = string.IsNullOrEmpty(favoritesJson) 
                    ? new List<int>() 
                    : System.Text.Json.JsonSerializer.Deserialize<List<int>>(favoritesJson);
                
                if (Product.IsFavorite && !favoriteIds.Contains(Product.ProductId))
                {
                    favoriteIds.Add(Product.ProductId);
                }
                else if (!Product.IsFavorite && favoriteIds.Contains(Product.ProductId))
                {
                    favoriteIds.Remove(Product.ProductId);
                }
                
                var updatedFavoritesJson = System.Text.Json.JsonSerializer.Serialize(favoriteIds);
                await SecureStorage.SetAsync("favorites", updatedFavoritesJson);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao salvar favoritos: {ex.Message}");
            }
            
            OnPropertyChanged(nameof(Product));
        }

        private async Task GoBackAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
