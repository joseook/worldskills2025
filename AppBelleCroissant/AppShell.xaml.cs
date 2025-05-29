using AppBelleCroissant.Pages;
using AppBelleCroissant.Services;
using System.Windows.Input;

namespace AppBelleCroissant
{
    public partial class AppShell : Shell
    {
        private readonly ApiService _apiService;
        
        public ICommand LogoutCommand { get; }

        public AppShell(ApiService apiService)
        {
            InitializeComponent();
            _apiService = apiService;
            
            // Registrar rotas para navegação
            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
            Routing.RegisterRoute(nameof(ForgotPasswordPage), typeof(ForgotPasswordPage));
            Routing.RegisterRoute(nameof(ProductsPage), typeof(ProductsPage));
            Routing.RegisterRoute(nameof(ProductDetailsPage), typeof(ProductDetailsPage));
            
            // Comando para fazer logout
            LogoutCommand = new Command(OnLogoutClicked);
            BindingContext = this;
        }
        
        private async void OnLogoutClicked()
        {
            var confirm = await DisplayAlert("Confirmação", "Tem certeza que deseja sair?", "Sim", "Não");
            
            if (confirm)
            {
                // Limpar token e informações do usuário
                _apiService.AuthToken = null;
                
                // Navegar para a página de login
                await Current.GoToAsync("//LoginPage");
            }
        }
        
        protected override async void OnNavigating(ShellNavigatingEventArgs args)
        {
            try
            {
                base.OnNavigating(args);
                
                // Log para debug
                System.Diagnostics.Debug.WriteLine($"Navegando para: {args.Target.Location.OriginalString}");
                
                // Se o usuário não estiver autenticado e tentar acessar páginas que requerem autenticação,
                // redireciona para a página de login
                if (_apiService != null && !_apiService.IsAuthenticated && 
                    args.Target.Location.OriginalString != "//LoginPage" &&
                    !args.Target.Location.OriginalString.Contains("RegisterPage") &&
                    !args.Target.Location.OriginalString.Contains("ForgotPasswordPage"))
                {
                    args.Cancel();
                    await Current.GoToAsync("//LoginPage");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro na navegação: {ex.Message}");
            }
        }
    }
}
