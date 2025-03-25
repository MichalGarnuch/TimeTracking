using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;
using TimeTrackingMobile.Models;
using TimeTrackingMobile.Services;

namespace TimeTrackingMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty(nameof(ProjectId), "projectId")]
    public partial class ProjectEditPage : ContentPage
    {
        private readonly ProjectService _projectService;
        private ProjectModel currentProject;

        private int _projectId;
        public int ProjectId
        {
            get => _projectId;
            set
            {
                _projectId = value;
                _ = LoadProject();
            }
        }

        public ProjectEditPage()
        {
            InitializeComponent();
            _projectService = new ProjectService();
        }

        private async System.Threading.Tasks.Task LoadProject()
        {
            try
            {
                currentProject = await _projectService.GetProject(_projectId);
                if (currentProject == null)
                {
                    await DisplayAlert("Error", "Project not found", "OK");
                    await Shell.Current.GoToAsync("..");
                    return;
                }

                // Wyświetlamy wartości w polach
                ProjectNameEntry.Text = currentProject.ProjectName;
                ProjectTypeIdEntry.Text = currentProject.ProjectTypeID.ToString();
                StartDatePicker.Date = currentProject.StartDate;
                EndDatePicker.Date = currentProject.EndDate;
                BudgetEntry.Text = currentProject.Budget.ToString();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
                await Shell.Current.GoToAsync("..");
            }
        }

        private async void SaveButtonClicked(object sender, EventArgs e)
        {
            if (currentProject == null) return;

            currentProject.ProjectName = ProjectNameEntry.Text;

            int projTypeId = 0;
            int.TryParse(ProjectTypeIdEntry.Text, out projTypeId);
            currentProject.ProjectTypeID = projTypeId;

            currentProject.StartDate = StartDatePicker.Date;
            currentProject.EndDate = EndDatePicker.Date;

            decimal budget = 0m;
            decimal.TryParse(BudgetEntry.Text, out budget);
            currentProject.Budget = budget;

            bool updated = await _projectService.UpdateProject(_projectId, currentProject);
            if (updated)
            {
                await DisplayAlert("Success", "Project updated", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await DisplayAlert("Error", "Failed to update project", "OK");
            }
        }

        private async void CancelButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
