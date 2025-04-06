using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;
using TimeTrackingMobile.Models;
using TimeTrackingMobile.Services;

namespace TimeTrackingMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TaskAddPage : ContentPage
    {
        private readonly TaskService _taskService;

        public TaskAddPage()
        {
            InitializeComponent();
            _taskService = new TaskService();
        }

        private async void SaveButtonClicked(object sender, EventArgs e)
        {
            var name = TaskNameEntry.Text;
            if (string.IsNullOrWhiteSpace(name))
            {
                await DisplayAlert("Validation", "Task Name cannot be empty", "OK");
                return;
            }

            int projectId = 0;
            int.TryParse(ProjectIdEntry.Text, out projectId);

            int priority = 0;
            int.TryParse(PriorityEntry.Text, out priority);

            var newTask = new TaskModel
            {
                TaskName = name,
                Description = DescriptionEditor.Text,
                ProjectID = projectId,
                Status = StatusEntry.Text,
                Priority = priority
            };

            bool success = await _taskService.CreateTask(newTask);
            if (success)
            {
                await DisplayAlert("Success", "Task created", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await DisplayAlert("Error", "Create failed", "OK");
            }
        }

        private async void CancelButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
