using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TimeTrackingMobile.Models;
using TimeTrackingMobile.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace TimeTrackingMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProjectsPage : ContentPage
    {
        private readonly ProjectService _service = new ProjectService();

        public ObservableCollection<ProjectModel> Projects { get; }
            = new ObservableCollection<ProjectModel>();

        public ProjectsPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (Projects.Count == 0)
                await LoadProjectsAsync();
        }

        private async void OnRefreshClicked(object sender, EventArgs e)
            => await LoadProjectsAsync();

        private async void OnAddClicked(object sender, EventArgs e)
            => await Shell.Current.GoToAsync(nameof(ProjectAddPage));

        private async Task LoadProjectsAsync()
        {
            Projects.Clear();
            try
            {
                var list = await _service.GetAllProjects();
                foreach (var p in list)
                    Projects.Add(p);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count == 0) return;
            var p = e.CurrentSelection[0] as ProjectModel;
            ((CollectionView)sender).SelectedItem = null;

            if (p == null) return;

            string action = await DisplayActionSheet(
                p.ProjectName,
                "Cancel", null,
                "Edit", "Delete");

            switch (action)
            {
                case "Edit":
                    await Shell.Current.GoToAsync(
                        $"{nameof(ProjectEditPage)}?projectId={p.ProjectID}");
                    break;
                case "Delete":
                    bool confirm = await DisplayAlert(
                        "Confirm",
                        $"Delete '{p.ProjectName}'?",
                        "Yes", "No");
                    if (confirm)
                    {
                        bool ok = await _service.DeleteProject(p.ProjectID);
                        if (ok)
                        {
                            await DisplayAlert("OK", "Deleted", "OK");
                            await LoadProjectsAsync();
                        }
                        else
                        {
                            await DisplayAlert("Error", "Delete failed", "OK");
                        }
                    }
                    break;
            }
        }
    }
}
