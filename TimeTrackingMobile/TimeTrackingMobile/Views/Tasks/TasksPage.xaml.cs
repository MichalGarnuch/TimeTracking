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
    public partial class TasksPage : ContentPage
    {
        private readonly TaskService _service = new TaskService();

        public ObservableCollection<TaskModel> Tasks { get; }
            = new ObservableCollection<TaskModel>();

        public TasksPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (Tasks.Count == 0)
                await LoadTasksAsync();
        }

        private async void OnRefreshClicked(object sender, EventArgs e)
            => await LoadTasksAsync();

        private async void OnAddClicked(object sender, EventArgs e)
            => await Shell.Current.GoToAsync(nameof(TaskAddPage));

        private async Task LoadTasksAsync()
        {
            Tasks.Clear();
            try
            {
                var list = await _service.GetAllTasks();
                foreach (var t in list)
                    Tasks.Add(t);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count == 0) return;
            var task = e.CurrentSelection[0] as TaskModel;
            ((CollectionView)sender).SelectedItem = null;
            if (task == null) return;

            string action = await DisplayActionSheet(
                task.TaskName,
                "Cancel", null,
                "Edit", "Delete");

            switch (action)
            {
                case "Edit":
                    await Shell.Current.GoToAsync(
                        $"{nameof(TaskEditPage)}?taskId={task.TaskID}");
                    break;
                case "Delete":
                    bool confirm = await DisplayAlert(
                        "Confirm", $"Delete '{task.TaskName}'?", "Yes", "No");
                    if (confirm)
                    {
                        bool ok = await _service.DeleteTask(task.TaskID);
                        if (ok)
                        {
                            await DisplayAlert("OK", "Deleted", "OK");
                            await LoadTasksAsync();
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
