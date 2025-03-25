using Xamarin.Forms;
using TimeTrackingMobile.Models;
using TimeTrackingMobile.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace TimeTrackingMobile.Views
{
    public partial class TasksPage : ContentPage
    {
        private readonly TaskService _taskService;
        private List<TaskModel> _tasks;

        public TasksPage()
        {
            InitializeComponent();
            _taskService = new TaskService();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadTasks();
        }

        private async void LoadTasksClicked(object sender, EventArgs e)
        {
            await LoadTasks();
        }

        private async Task LoadTasks()
        {
            try
            {
                _tasks = await _taskService.GetAllTasks();
                TasksList.ItemsSource = _tasks;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void AddTaskClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(TaskAddPage));
        }

        private async void TasksList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is TaskModel tm)
            {
                var action = await DisplayActionSheet(
                    $"Selected Task: {tm.TaskName}",
                    "Cancel",
                    null,
                    "Edit",
                    "Delete");
                if (action == "Edit")
                {
                    await Shell.Current.GoToAsync($"{nameof(TaskEditPage)}?taskId={tm.TaskID}");
                }
                else if (action == "Delete")
                {
                    bool confirm = await DisplayAlert("Confirm", $"Delete '{tm.TaskName}'?", "Yes", "No");
                    if (confirm)
                    {
                        bool deleted = await _taskService.DeleteTask(tm.TaskID);
                        if (deleted)
                        {
                            await DisplayAlert("OK", "Task deleted", "OK");
                            await LoadTasks();
                        }
                        else
                        {
                            await DisplayAlert("Error", "Delete failed", "OK");
                        }
                    }
                }
                TasksList.SelectedItem = null;
            }
        }
    }
}
