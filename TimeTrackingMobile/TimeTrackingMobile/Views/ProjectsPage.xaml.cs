using Xamarin.Forms;
using TimeTrackingMobile.Models;
using TimeTrackingMobile.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace TimeTrackingMobile.Views
{
    public partial class ProjectsPage : ContentPage
    {
        private readonly ProjectService _projectService;
        private List<ProjectModel> _projects;

        public ProjectsPage()
        {
            InitializeComponent();
            _projectService = new ProjectService();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            // Automatycznie wczytamy listę
            await LoadProjects();
        }

        private async void LoadProjectsClicked(object sender, EventArgs e)
        {
            await LoadProjects();
        }

        private async Task LoadProjects()
        {
            try
            {
                _projects = await _projectService.GetAllProjects();
                ProjectsList.ItemsSource = _projects;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void AddProjectClicked(object sender, EventArgs e)
        {
            // Przejście do strony dodawania nowego projektu
            await Shell.Current.GoToAsync(nameof(ProjectAddPage));
        }

        private async void ProjectsList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is ProjectModel project)
            {
                // Po kliknięciu w element listy => ActionSheet
                var action = await DisplayActionSheet(
                    $"Selected: {project.ProjectName}",
                    "Cancel",
                    null,
                    "Edit",
                    "Delete");

                if (action == "Edit")
                {
                    // Nawigujemy do ProjectEditPage, przekazujemy ID w query param
                    await Shell.Current.GoToAsync($"{nameof(ProjectEditPage)}?projectId={project.ProjectID}");
                }
                else if (action == "Delete")
                {
                    bool confirm = await DisplayAlert(
                        "Confirm",
                        $"Delete project '{project.ProjectName}'?",
                        "Yes", "No");
                    if (confirm)
                    {
                        bool deleted = await _projectService.DeleteProject(project.ProjectID);
                        if (deleted)
                        {
                            await DisplayAlert("OK", "Project deleted", "OK");
                            await LoadProjects();
                        }
                        else
                        {
                            await DisplayAlert("Error", "Delete failed", "OK");
                        }
                    }
                }

                // Odznaczenie
                ProjectsList.SelectedItem = null;
            }
        }
    }
}
