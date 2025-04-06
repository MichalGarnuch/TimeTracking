using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;
using TimeTrackingMobile.Models;
using TimeTrackingMobile.Services;

namespace TimeTrackingMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty(nameof(ProjectTypeId), "projectTypeId")]
    public partial class ProjectTypeEditPage : ContentPage
    {
        private readonly ProjectTypeService _service;
        private ProjectTypeModel currentPT;

        private int _projectTypeId;
        public int ProjectTypeId
        {
            get => _projectTypeId;
            set
            {
                _projectTypeId = value;
                _ = LoadData();
            }
        }

        public ProjectTypeEditPage()
        {
            InitializeComponent();
            _service = new ProjectTypeService();
        }

        private async System.Threading.Tasks.Task LoadData()
        {
            try
            {
                currentPT = await _service.GetProjectType(_projectTypeId);
                if (currentPT == null)
                {
                    await DisplayAlert("Error", "Not found", "OK");
                    await Shell.Current.GoToAsync("..");
                    return;
                }

                TypeNameEntry.Text = currentPT.TypeName;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
                await Shell.Current.GoToAsync("..");
            }
        }

        private async void SaveButtonClicked(object sender, EventArgs e)
        {
            if (currentPT == null) return;

            var newName = TypeNameEntry.Text;
            if (string.IsNullOrWhiteSpace(newName))
            {
                await DisplayAlert("Validation", "Type Name cannot be empty", "OK");
                return;
            }

            currentPT.TypeName = newName;
            bool success = await _service.UpdateProjectType(_projectTypeId, currentPT);
            if (success)
            {
                await DisplayAlert("Success", "Updated", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await DisplayAlert("Error", "Update failed", "OK");
            }
        }

        private async void CancelButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
