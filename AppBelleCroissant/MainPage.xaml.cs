namespace AppBelleCroissant
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }
        
        private async void OnNavigateToPostsClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//PostsPage");
        }
    }

}
