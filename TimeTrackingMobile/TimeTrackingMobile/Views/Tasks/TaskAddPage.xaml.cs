using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TimeTrackingMobile.Models;
using TimeTrackingMobile.Services;

namespace TimeTrackingMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TaskAddPage : ContentPage
    {
        private readonly TaskService _service = new TaskService();

        public TaskAddPage()
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
            if (!int.TryParse(ProjectIdEntry.Text, out int projId))
            {
                await DisplayAlert("Validation", "Invalid Project ID.", "OK");
                return;
            }
            if (!int.TryParse(PriorityEntry.Text, out int pr))
            {
                await DisplayAlert("Validation", "Invalid priority.", "OK");
                return;
            }

            var task = new TaskModel
            {
                TaskName = NameEntry.Text.Trim(),
                Description = DescEditor.Text?.Trim(),
                ProjectID = projId,
                Status = StatusEntry.Text?.Trim(),
                Priority = pr
            };

            bool ok = await _service.CreateTask(task);
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
