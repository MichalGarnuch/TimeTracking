using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;
using TimeTrackingMobile.Models;
using TimeTrackingMobile.Services;

namespace TimeTrackingMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty(nameof(TaskId), "taskId")]
    public partial class TaskEditPage : ContentPage
    {
        private readonly TaskService _taskService;
        private TaskModel currentTask;

        private int _taskId;
        public int TaskId
        {
            get => _taskId;
            set
            {
                _taskId = value;
                _ = LoadTask();
            }
        }

        public TaskEditPage()
        {
            InitializeComponent();
            _taskService = new TaskService();
        }

        private async System.Threading.Tasks.Task LoadTask()
        {
            try
            {
                currentTask = await _taskService.GetTask(_taskId);
                if (currentTask == null)
                {
                    await DisplayAlert("Error", "Task not found", "OK");
                    await Shell.Current.GoToAsync("..");
                    return;
                }

                TaskNameEntry.Text = currentTask.TaskName;
                DescriptionEditor.Text = currentTask.Description;
                ProjectIdEntry.Text = currentTask.ProjectID.ToString();
                StatusEntry.Text = currentTask.Status;
                PriorityEntry.Text = currentTask.Priority.ToString();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
                await Shell.Current.GoToAsync("..");
            }
        }

        private async void SaveButtonClicked(object sender, EventArgs e)
        {
            if (currentTask == null) return;

            currentTask.TaskName = TaskNameEntry.Text;
            currentTask.Description = DescriptionEditor.Text;

            int projectId = 0;
            int.TryParse(ProjectIdEntry.Text, out projectId);
            currentTask.ProjectID = projectId;

            currentTask.Status = StatusEntry.Text;

            int priority = 0;
            int.TryParse(PriorityEntry.Text, out priority);
            currentTask.Priority = priority;

            bool updated = await _taskService.UpdateTask(_taskId, currentTask);
            if (updated)
            {
                await DisplayAlert("Success", "Task updated", "OK");
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
