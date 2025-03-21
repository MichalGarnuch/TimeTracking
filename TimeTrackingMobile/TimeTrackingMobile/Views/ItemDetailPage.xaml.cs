using System.ComponentModel;
using Xamarin.Forms;
using TimeTrackingMobile.ViewModels;

namespace TimeTrackingMobile.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}