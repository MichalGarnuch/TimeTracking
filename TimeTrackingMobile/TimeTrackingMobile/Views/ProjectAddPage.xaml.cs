using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;
using TimeTrackingMobile.Models;
using TimeTrackingMobile.Services;

namespace TimeTrackingMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProjectAddPage : ContentPage
    {
        private readonly ProjectService _projectService;

        public ProjectAddPage()
        {
            InitializeComponent();
            _projectService = new ProjectService();

            // Domyślne wartości, jeśli chcesz
            StartDatePicker.Date = DateTime.Now;
            EndDatePicker.Date = DateTime.Now.AddDays(7);
        }

        private async void SaveButtonClicked(object sender, EventArgs e)
        {
            string name = ProjectNameEntry.Text;
            if (string.IsNullOrWhiteSpace(name))
            {
                await DisplayAlert("Validation", "Project Name cannot be empty.", "OK");
                return;
            }

            // Parsowanie ProjectTypeID i Budget
            int projTypeId = 0;
            int.TryParse(ProjectTypeIdEntry.Text, out projTypeId);

            decimal budget = 0m;
            decimal.TryParse(BudgetEntry.Text, out budget);

            var newProject = new ProjectModel
            {
                ProjectName = name,
                ProjectTypeID = projTypeId,
                StartDate = StartDatePicker.Date,
                EndDate = EndDatePicker.Date,
                Budget = budget
            };

            bool success = await _projectService.CreateProject(newProject);
            if (success)
            {
                await DisplayAlert("Success", "Project created", "OK");
                await Shell.Current.GoToAsync(".."); // powrót
            }
            else
            {
                await DisplayAlert("Error", "Failed to create project", "OK");
            }
        }

        private async void CancelButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
