using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TimeTrackingMobile.Models;
using TimeTrackingMobile.Services;

namespace TimeTrackingMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DepartmentAddPage : ContentPage
    {
        private readonly DepartmentService _service = new DepartmentService();

        public DepartmentAddPage()
        {
            InitializeComponent();
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            var name = DepartmentNameEntry.Text?.Trim();
            if (string.IsNullOrEmpty(name))
            {
                await DisplayAlert("Validation", "Name cannot be empty.", "OK");
                return;
            }

            var dept = new DepartmentModel { DepartmentName = name };
            bool success = await _service.CreateDepartment(dept);
            if (success)
            {
                await DisplayAlert("Success", "Department added.", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await DisplayAlert("Error", "Could not add department.", "OK");
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
