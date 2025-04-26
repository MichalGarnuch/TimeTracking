using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimeTrackingMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void OnLoginClicked(object sender, System.EventArgs e)
        {
            // Prosta symulacja: bez walidacji, od razu w Home
            await Shell.Current.GoToAsync("//Home");
        }
    }
}
