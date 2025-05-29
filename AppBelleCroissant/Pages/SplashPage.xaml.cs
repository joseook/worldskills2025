using System;
using Microsoft.Maui.Controls;

namespace AppBelleCroissant.Pages
{
    public partial class SplashPage : ContentPage
    {
        public SplashPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Simular tempo de carregamento
            await Task.Delay(1500);

            try
            {
                // Navegar para a página de login após a splash screen
                await Shell.Current.GoToAsync("//LoginPage");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao navegar da SplashPage: {ex.Message}");
                
                // Se falhar, tente de outra forma
                await Navigation.PushModalAsync(new NavigationPage(new LoginPage(
                    new ViewModels.LoginViewModel(
                        new Services.ApiService()))));
            }
        }
    }
}
