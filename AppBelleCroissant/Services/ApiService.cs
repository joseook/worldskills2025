using AppBelleCroissant.Models;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace AppBelleCroissant.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _serializerOptions;
        private readonly string _baseUrl = "http://57.151.99.162:8081/api";
        private string _authToken;

        public ApiService()
        {
            var handler = new HttpClientHandler();
            // Permitir certificados auto-assinados ou inválidos para desenvolvimento
            // NOTA: Isso não deve ser usado em produção!
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            
            _httpClient = new HttpClient(handler);
            _httpClient.Timeout = TimeSpan.FromSeconds(30); // Aumentar timeout para 30 segundos
            
            // Adicionar cabeçalhos padrão
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            
            Console.WriteLine($"ApiService inicializado com URL base: {_baseUrl}");
        }

        public string AuthToken
        {
            get => _authToken;
            set
            {
                _authToken = value;
                if (!string.IsNullOrEmpty(_authToken))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = 
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _authToken);
                }
                else
                {
                    _httpClient.DefaultRequestHeaders.Authorization = null;
                }
            }
        }

        public bool IsAuthenticated => !string.IsNullOrEmpty(AuthToken);

        public User CurrentUser { get; private set; }

        // Métodos de autenticação
        public async Task<(bool success, string message, User user)> LoginAsync(string email, string password)
        {
            try
            {
                Console.WriteLine($"Iniciando login para o email: {email}");
                
                var loginData = new
                {
                    Email = email,
                    Password = password
                };

                var json = JsonSerializer.Serialize(loginData, _serializerOptions);
                Console.WriteLine($"Dados de login serializados: {json}");
                
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                Console.WriteLine($"Enviando requisição para: {_baseUrl}/login");
                
                try
                {
                    var response = await _httpClient.PostAsync($"{_baseUrl}/login", content);
                    Console.WriteLine($"Resposta recebida. Status: {response.StatusCode}");
                    
                    var responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Conteúdo da resposta: {responseContent}");

                    if (response.IsSuccessStatusCode)
                    {
                        try {
                            var result = JsonSerializer.Deserialize<AuthResponse>(responseContent, _serializerOptions);
                            if (result != null && result.Token != null)
                            {
                                AuthToken = result.Token;
                                CurrentUser = result.User;
                                Console.WriteLine("Login realizado com sucesso. Token obtido.");
                                return (true, "Login realizado com sucesso!", result.User);
                            }
                            else
                            {
                                Console.WriteLine("Token ou usuário nulo na resposta de autenticação.");
                                return (false, "Erro ao processar resposta do servidor. Tente novamente.", null);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Erro na deserialização: {ex.Message}\nStack: {ex.StackTrace}");
                            return (false, "Erro ao processar resposta do servidor. Tente novamente.", null);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Falha na requisição: {response.StatusCode} - {response.ReasonPhrase}");
                        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                        {
                            return (false, "Credenciais inválidas. Por favor, tente novamente.", null);
                        }
                        else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                        {
                            return (false, "Serviço de autenticação não encontrado. Verifique sua conexão.", null);
                        }
                        else
                        {
                            return (false, $"Erro no servidor: {response.StatusCode}. Tente novamente mais tarde.", null);
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"Erro de requisição HTTP: {ex.Message}\nInner: {ex.InnerException?.Message}\nStack: {ex.StackTrace}");
                    if (ex.InnerException is System.Net.Sockets.SocketException socketEx)
                    {
                        Console.WriteLine($"Erro de Socket: {socketEx.SocketErrorCode}");
                        return (false, $"Falha na conexão: Não foi possível se conectar ao servidor. Verifique sua conexão com a internet.", null);
                    }
                    else
                    {
                        return (false, $"Falha na conexão: {ex.Message}", null);
                    }
                }
                catch (TaskCanceledException ex)
                {
                    Console.WriteLine($"Requisição cancelada (timeout): {ex.Message}");
                    return (false, "Tempo limite excedido. Verifique sua conexão e tente novamente.", null);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro não tratado no login: {ex.Message}\nStack: {ex.StackTrace}");
                return (false, $"Erro ao fazer login: {ex.Message}", null);
            }
        }

        public async Task<(bool success, string message, User user)> RegisterAsync(
            string email, string password, string confirmPassword, string firstName, string lastName)
        {
            try
            {
                Console.WriteLine($"Iniciando registro para o email: {email}");
                
                var registerData = new
                {
                    Email = email,
                    Password = password,
                    ConfirmPassword = confirmPassword,
                    FirstName = firstName,
                    LastName = lastName
                };

                var json = JsonSerializer.Serialize(registerData, _serializerOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                Console.WriteLine($"Enviando requisição para: {_baseUrl}/register");
                
                try
                {
                    var response = await _httpClient.PostAsync($"{_baseUrl}/register", content);
                    Console.WriteLine($"Resposta recebida. Status: {response.StatusCode}");
                    
                    var responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Conteúdo da resposta: {responseContent}");

                    if (response.IsSuccessStatusCode)
                    {
                        try {
                            var result = JsonSerializer.Deserialize<AuthResponse>(responseContent, _serializerOptions);
                            if (result != null && result.Token != null)
                            {
                                AuthToken = result.Token;
                                CurrentUser = result.User;
                                Console.WriteLine("Registro realizado com sucesso. Token obtido.");
                                return (true, "Registro realizado com sucesso!", result.User);
                            }
                            else
                            {
                                Console.WriteLine("Token ou usuário nulo na resposta de registro.");
                                return (false, "Erro ao processar resposta do servidor. Tente novamente.", null);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Erro na deserialização: {ex.Message}\nStack: {ex.StackTrace}");
                            return (false, "Erro ao processar resposta do servidor. Tente novamente.", null);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Falha na requisição: {response.StatusCode} - {response.ReasonPhrase}");
                        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                        {
                            return (false, "Dados inválidos ou email já registrado. Verifique as informações.", null);
                        }
                        else
                        {
                            return (false, $"Erro no servidor: {response.StatusCode}. Tente novamente mais tarde.", null);
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"Erro de requisição HTTP: {ex.Message}\nInner: {ex.InnerException?.Message}");
                    return (false, $"Falha na conexão: {ex.Message}", null);
                }
                catch (TaskCanceledException ex)
                {
                    Console.WriteLine($"Requisição cancelada (timeout): {ex.Message}");
                    return (false, "Tempo limite excedido. Verifique sua conexão e tente novamente.", null);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro não tratado no registro: {ex.Message}\nStack: {ex.StackTrace}");
                return (false, $"Erro ao fazer o registro: {ex.Message}", null);
            }
        }

        public async Task<(bool success, string message, string token)> ForgotPasswordAsync(string email)
        {
            try
            {
                Console.WriteLine($"Iniciando recuperação de senha para o email: {email}");
                
                var forgotPasswordData = new
                {
                    Email = email
                };

                var json = JsonSerializer.Serialize(forgotPasswordData, _serializerOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                Console.WriteLine($"Enviando requisição para: {_baseUrl}/forgot-password");
                
                try
                {
                    var response = await _httpClient.PostAsync($"{_baseUrl}/forgot-password", content);
                    Console.WriteLine($"Resposta recebida. Status: {response.StatusCode}");
                    
                    var responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Conteúdo da resposta: {responseContent}");

                    if (response.IsSuccessStatusCode)
                    {
                        try {
                            var result = JsonSerializer.Deserialize<ForgotPasswordResponse>(responseContent, _serializerOptions);
                            if (result != null)
                            {
                                string message = result.Message ?? "Instruções enviadas para seu email.";
                                Console.WriteLine("Recuperação de senha processada com sucesso.");
                                return (true, message, result.Token);
                            }
                            else
                            {
                                Console.WriteLine("Resposta nula na recuperação de senha.");
                                return (false, "Erro ao processar resposta do servidor. Tente novamente.", null);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Erro na deserialização: {ex.Message}\nStack: {ex.StackTrace}");
                            return (false, "Erro ao processar resposta do servidor. Tente novamente.", null);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Falha na requisição: {response.StatusCode} - {response.ReasonPhrase}");
                        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                        {
                            return (false, "Email não encontrado. Verifique se digitou corretamente.", null);
                        }
                        else
                        {
                            return (false, $"Erro no servidor: {response.StatusCode}. Tente novamente mais tarde.", null);
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"Erro de requisição HTTP: {ex.Message}\nInner: {ex.InnerException?.Message}");
                    return (false, $"Falha na conexão: {ex.Message}", null);
                }
                catch (TaskCanceledException ex)
                {
                    Console.WriteLine($"Requisição cancelada (timeout): {ex.Message}");
                    return (false, "Tempo limite excedido. Verifique sua conexão e tente novamente.", null);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro não tratado na recuperação de senha: {ex.Message}\nStack: {ex.StackTrace}");
                return (false, $"Erro ao solicitar recuperação de senha: {ex.Message}", null);
            }
        }

        public async Task<(bool success, string message)> ResetPasswordAsync(
            string token, string email, string password, string confirmPassword)
        {
            try
            {
                Console.WriteLine($"Iniciando redefinição de senha para o email: {email}");
                
                var resetPasswordData = new
                {
                    Token = token,
                    Email = email,
                    Password = password,
                    ConfirmPassword = confirmPassword
                };

                var json = JsonSerializer.Serialize(resetPasswordData, _serializerOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                Console.WriteLine($"Enviando requisição para: {_baseUrl}/reset-password");
                
                try
                {
                    var response = await _httpClient.PostAsync($"{_baseUrl}/reset-password", content);
                    Console.WriteLine($"Resposta recebida. Status: {response.StatusCode}");
                    
                    var responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Conteúdo da resposta: {responseContent}");

                    if (response.IsSuccessStatusCode)
                    {
                        try {
                            var result = JsonSerializer.Deserialize<ResetPasswordResponse>(responseContent, _serializerOptions);
                            if (result != null)
                            {
                                string message = result.Message ?? "Senha redefinida com sucesso.";
                                Console.WriteLine("Senha redefinida com sucesso.");
                                return (true, message);
                            }
                            else
                            {
                                Console.WriteLine("Resposta nula na redefinição de senha.");
                                return (false, "Erro ao processar resposta do servidor. Tente novamente.");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Erro na deserialização: {ex.Message}\nStack: {ex.StackTrace}");
                            return (false, "Erro ao processar resposta do servidor. Tente novamente.");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Falha na requisição: {response.StatusCode} - {response.ReasonPhrase}");
                        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                        {
                            return (false, "Token inválido ou expirado. Solicite uma nova redefinição de senha.");
                        }
                        else
                        {
                            return (false, $"Erro no servidor: {response.StatusCode}. Tente novamente mais tarde.");
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"Erro de requisição HTTP: {ex.Message}\nInner: {ex.InnerException?.Message}");
                    return (false, $"Falha na conexão: {ex.Message}");
                }
                catch (TaskCanceledException ex)
                {
                    Console.WriteLine($"Requisição cancelada (timeout): {ex.Message}");
                    return (false, "Tempo limite excedido. Verifique sua conexão e tente novamente.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro não tratado na redefinição de senha: {ex.Message}\nStack: {ex.StackTrace}");
                return (false, $"Erro ao redefinir senha: {ex.Message}");
            }
        }

        // Métodos para produtos
        public async Task<List<Product>> GetProductsAsync()
        {
            try
            {
                Console.WriteLine($"Buscando produtos da API...");
                
                try
                {
                    var response = await _httpClient.GetAsync($"{_baseUrl}/products");
                    Console.WriteLine($"Resposta recebida. Status: {response.StatusCode}");
                    
                    if (response.IsSuccessStatusCode)
                    {
                        try
                        {
                            var responseContent = await response.Content.ReadAsStringAsync();
                            Console.WriteLine($"Recebidos {responseContent.Length} bytes de dados");
                            
                            var products = await response.Content.ReadFromJsonAsync<List<Product>>(_serializerOptions);
                            Console.WriteLine($"Produtos deserializados com sucesso. Total: {products?.Count ?? 0}");
                            return products ?? new List<Product>();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Erro na deserialização dos produtos: {ex.Message}\nStack: {ex.StackTrace}");
                            return new List<Product>();
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Falha na requisição de produtos: {response.StatusCode} - {response.ReasonPhrase}");
                        return new List<Product>();
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"Erro de requisição HTTP ao buscar produtos: {ex.Message}\nInner: {ex.InnerException?.Message}");
                    return new List<Product>();
                }
                catch (TaskCanceledException ex)
                {
                    Console.WriteLine($"Requisição de produtos cancelada (timeout): {ex.Message}");
                    return new List<Product>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro não tratado ao buscar produtos: {ex.Message}\nStack: {ex.StackTrace}");
                return new List<Product>();
            }
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            try
            {
                Console.WriteLine($"Buscando produto com ID: {id}");
                
                try
                {
                    var response = await _httpClient.GetAsync($"{_baseUrl}/products/{id}");
                    Console.WriteLine($"Resposta recebida. Status: {response.StatusCode}");
                    
                    if (response.IsSuccessStatusCode)
                    {
                        try
                        {
                            var responseContent = await response.Content.ReadAsStringAsync();
                            Console.WriteLine($"Recebidos {responseContent.Length} bytes de dados");
                            
                            var product = await response.Content.ReadFromJsonAsync<Product>(_serializerOptions);
                            if (product != null)
                            {
                                Console.WriteLine($"Produto deserializado com sucesso. ID: {product.ProductId}, Nome: {product.ProductName}");
                            }
                            else
                            {
                                Console.WriteLine("Produto deserializado como null");
                            }
                            return product;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Erro na deserialização do produto: {ex.Message}\nStack: {ex.StackTrace}");
                            return null;
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Falha na requisição do produto: {response.StatusCode} - {response.ReasonPhrase}");
                        return null;
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"Erro de requisição HTTP ao buscar produto: {ex.Message}\nInner: {ex.InnerException?.Message}");
                    return null;
                }
                catch (TaskCanceledException ex)
                {
                    Console.WriteLine($"Requisição do produto cancelada (timeout): {ex.Message}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro não tratado ao buscar produto: {ex.Message}\nStack: {ex.StackTrace}");
                return null;
            }
        }


    }

    public class AuthResponse
    {
        public string Token { get; set; }
        public User User { get; set; }
    }

    public class ForgotPasswordResponse
    {
        public string Message { get; set; }
        public string Token { get; set; }
    }

    public class ResetPasswordResponse
    {
        public string Message { get; set; }
    }
}
