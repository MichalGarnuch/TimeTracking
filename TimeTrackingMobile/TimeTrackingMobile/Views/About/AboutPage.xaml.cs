using System.Collections.Generic;
using Xamarin.Forms;

namespace TimeTrackingMobile.Views
{
    public partial class AboutPage : ContentPage
    {
        public IList<string> Screenshots { get; }

        public AboutPage()
        {
            InitializeComponent();

            // Nazwy plików obrazów w Resources/Images
            Screenshots = new List<string>
            {
                "screen1.png",
                "screen2.png",
                "screen3.png"
            };
            BindingContext = this;
        }
    }
}
