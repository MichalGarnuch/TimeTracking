using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;
using TimeTrackingMobile.Models;
using TimeTrackingMobile.Services;

namespace TimeTrackingMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProjectTypeAddPage : ContentPage
    {
        private readonly ProjectTypeService _service;

        public ProjectTypeAddPage()
        {
            InitializeComponent();
            _service = new ProjectTypeService();
        }

        private async void SaveButtonClicked(object sender, EventArgs e)
        {
            var name = TypeNameEntry.Text;
            if (string.IsNullOrWhiteSpace(name))
            {
                await DisplayAlert("Validation", "Type Name is required", "OK");
                return;
            }

            var newPT = new ProjectTypeModel
            {
                TypeName = name
            };

            bool success = await _service.CreateProjectType(newPT);
            if (success)
            {
                await DisplayAlert("Success", "ProjectType created", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await DisplayAlert("Error", "Failed to create", "OK");
            }
        }

        private async void CancelButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
