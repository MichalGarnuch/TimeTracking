using Xamarin.Forms;
using TimeTrackingMobile.Services;
using TimeTrackingMobile.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace TimeTrackingMobile.Views
{
    public partial class ProjectTypesPage : ContentPage
    {
        private readonly ProjectTypeService _service;
        private List<ProjectTypeModel> _items;

        public ProjectTypesPage()
        {
            InitializeComponent();
            _service = new ProjectTypeService();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadProjectTypes();
        }

        private async void LoadProjectTypesClicked(object sender, EventArgs e)
        {
            await LoadProjectTypes();
        }

        private async Task LoadProjectTypes()
        {
            try
            {
                _items = await _service.GetAllProjectTypes();
                ProjectTypesList.ItemsSource = _items;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void AddProjectTypeClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(ProjectTypeAddPage));
        }

        private async void ProjectTypesList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is ProjectTypeModel pt)
            {
                var action = await DisplayActionSheet(
                    $"Selected: {pt.TypeName}",
                    "Cancel",
                    null,
                    "Edit",
                    "Delete");

                if (action == "Edit")
                {
                    await Shell.Current.GoToAsync($"{nameof(ProjectTypeEditPage)}?projectTypeId={pt.ProjectTypeID}");
                }
                else if (action == "Delete")
                {
                    bool confirm = await DisplayAlert("Confirm", $"Delete '{pt.TypeName}'?", "Yes", "No");
                    if (confirm)
                    {
                        bool deleted = await _service.DeleteProjectType(pt.ProjectTypeID);
                        if (deleted)
                        {
                            await DisplayAlert("OK", "Deleted", "OK");
                            await LoadProjectTypes();
                        }
                        else
                        {
                            await DisplayAlert("Error", "Delete failed", "OK");
                        }
                    }
                }

                ProjectTypesList.SelectedItem = null;
            }
        }

        private async void OnSyncClicked(object sender, EventArgs e)
        {
            await LoadProjectTypes();
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
