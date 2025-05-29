namespace AppBelleCroissant
{
    public partial class App : Application
    {
        public App(AppShell appShell)
        {
            InitializeComponent();

            MainPage = appShell;
            
            // Registre os manipuladores de exceções não tratadas
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
        }
        
        protected override async void OnStart()
        {
            base.OnStart();
            
            try
            {
                // Garantir que o usuário comece na página de login
                await Shell.Current.GoToAsync("//LoginPage");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao navegar para página inicial: {ex.Message}");
            }
        }
        
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            System.Diagnostics.Debug.WriteLine($"Erro não tratado: {ex?.Message}");
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"Erro de task não observado: {e.Exception.Message}");
            e.SetObserved(); // Marcar como observada para não quebrar o app
        }
    }
}
