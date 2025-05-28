using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BelleCroissantDesktop.Models;

namespace BelleCroissantDesktop.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "http://57.151.99.162:8081"; 

        public ApiService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_baseUrl);
        }

 
        public bool SetBasicAuthentication(string username, string password)
        {
            try
            {
                // Criar o token de autenticação básica
                string authValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));
                
                // Configurar o header de autenticação para todas as requisições futuras
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authValue);
                
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao configurar autenticação: {ex.Message}");
            }
        }

        public async Task<bool> ValidateCredentialsAsync(string username, string password)
        {
            try
            {
                // Configurar autenticação básica
                SetBasicAuthentication(username, password);
                
                // Tentar fazer uma requisição para verificar se as credenciais são válidas
                var response = await _httpClient.GetAsync("/api/products");
                
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao validar credenciais: {ex.Message}");
            }
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/products");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<Product>>() ?? new List<Product>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar produtos: {ex.Message}");
            }
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/products/{id}");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<Product>() ?? new Product();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar produto: {ex.Message}");
            }
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            try
            {
        
                var createDto = new CreateProductDto
                {
                    ProductName = product.ProductName,
                    Category = product.Category,
                    Ingredients = product.Ingredients,
                    Price = product.Price,
                    Cost = product.Cost,
                    Description = product.Description,
                    Seasonal = product.Seasonal,
                    Active = product.Active,
                    IntroducedDate = product.IntroducedDate
                };
                
                var response = await _httpClient.PostAsJsonAsync("/api/products", createDto);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<Product>() ?? new Product();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao criar produto: {ex.Message}");
            }
        }

        public async Task<Product?> UpdateProductAsync(int id, Product product)
        {
            try
            {
        
                var updateDto = new UpdateProductDto
                {
                    ProductName = product.ProductName,
                    Category = product.Category,
                    Ingredients = product.Ingredients,
                    Price = product.Price,
                    Cost = product.Cost,
                    Description = product.Description,
                    Seasonal = product.Seasonal,
                    Active = product.Active
                };
                
                var response = await _httpClient.PutAsJsonAsync($"/api/products/{id}", updateDto);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<Product>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao atualizar produto: {ex.Message}");
            }
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"/api/products/{id}");
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao excluir produto: {ex.Message}");
            }
        }

        public async Task<List<Order>> GetOrdersAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/orders");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<Order>>() ?? new List<Order>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar pedidos: {ex.Message}");
            }
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/orders/{id}");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<Order>() ?? new Order();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar pedido: {ex.Message}");
            }
        }

        public async Task<Order> CreateOrderAsync(CreateOrderDto orderDto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/orders", orderDto);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<Order>() ?? new Order();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao criar pedido: {ex.Message}");
            }
        }

        public async Task<Order> CompleteOrderAsync(int id)
        {
            try
            {
                var response = await _httpClient.PutAsync($"/api/orders/{id}/complete", null);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<Order>() ?? new Order();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao completar pedido: {ex.Message}");
            }
        }

        public async Task<bool> CancelOrderAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"/api/orders/{id}/cancel");
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao cancelar pedido: {ex.Message}");
            }
        }


        public async Task<Order?> UpdateOrderStatusAsync(int id, string status)
        {
            try
            {
                // Como a API não tem um endpoint específico para atualizar o status,
                // usamos os endpoints existentes
                switch (status.ToLower())
                {
                    case "completed":
                        return await CompleteOrderAsync(id);
                    case "cancelled":
                        await CancelOrderAsync(id);
                        return null;
                    case "processing":
                        // Como não há um endpoint específico para "processing", podemos
                        // implementar isso quando a API fornecer este endpoint
                        throw new NotImplementedException("Status 'Processing' não suportado pela API atual");
                    default:
                        throw new ArgumentException($"Status desconhecido: {status}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao atualizar status do pedido: {ex.Message}");
            }
        }
    }
}
