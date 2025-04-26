using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TimeTrackingMobile.Models;
using TimeTrackingMobile.Services;

namespace TimeTrackingMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProjectTypeAddPage : ContentPage
    {
        private readonly ProjectTypeService _service = new ProjectTypeService();

        public ProjectTypeAddPage()
        {
            InitializeComponent();
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            var name = TypeNameEntry.Text?.Trim();
            if (string.IsNullOrEmpty(name))
            {
                await DisplayAlert("Validation", "Name required.", "OK");
                return;
            }

            var type = new ProjectTypeModel { TypeName = name };
            bool ok = await _service.CreateProjectType(type);
            if (ok)
            {
                await DisplayAlert("Success", "Created", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await DisplayAlert("Error", "Create failed", "OK");
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
            => await Shell.Current.GoToAsync("..");
    }
}
