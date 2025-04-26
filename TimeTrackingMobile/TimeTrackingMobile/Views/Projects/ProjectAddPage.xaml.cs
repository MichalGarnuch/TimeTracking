using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TimeTrackingMobile.Models;
using TimeTrackingMobile.Services;

namespace TimeTrackingMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProjectAddPage : ContentPage
    {
        private readonly ProjectService _service = new ProjectService();

        public ProjectAddPage()
        {
            InitializeComponent();
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameEntry.Text))
            {
                await DisplayAlert("Validation", "Name required.", "OK");
                return;
            }
            if (!int.TryParse(TypeIdEntry.Text, out int typeId))
            {
                await DisplayAlert("Validation", "Invalid Type ID.", "OK");
                return;
            }
            if (!decimal.TryParse(BudgetEntry.Text, out decimal budget))
            {
                await DisplayAlert("Validation", "Invalid budget.", "OK");
                return;
            }

            var proj = new ProjectModel
            {
                ProjectName = NameEntry.Text.Trim(),
                ProjectTypeID = typeId,
                StartDate = StartPicker.Date,
                EndDate = EndPicker.Date,
                Budget = budget
            };

            bool ok = await _service.CreateProject(proj);
            if (ok)
            {
                await DisplayAlert("Success", "Created.", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await DisplayAlert("Error", "Create failed.", "OK");
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
            => await Shell.Current.GoToAsync("..");
    }
}
