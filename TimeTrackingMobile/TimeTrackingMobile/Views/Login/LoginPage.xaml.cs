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
            // Po kliknięciu idziemy na stronę AboutPage (Home)
            await Shell.Current.GoToAsync("//AboutPage");
        }
    }
}
