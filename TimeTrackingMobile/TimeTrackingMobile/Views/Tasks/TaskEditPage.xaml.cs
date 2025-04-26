using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TimeTrackingMobile.Models;
using TimeTrackingMobile.Services;

namespace TimeTrackingMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty(nameof(TaskId), "taskId")]
    public partial class TaskEditPage : ContentPage
    {
        private readonly TaskService _service = new TaskService();
        private TaskModel _current;

        public int TaskId
        {
            get => _id;
            set { _id = value; LoadTask(value); }
        }
        private int _id;

        public TaskEditPage()
        {
            InitializeComponent();
        }

        private async void LoadTask(int id)
        {
            try
            {
                _current = await _service.GetTask(id);
                if (_current == null) throw new Exception("Not found");
                NameEntry.Text = _current.TaskName;
                DescEditor.Text = _current.Description;
                ProjectIdEntry.Text = _current.ProjectID.ToString();
                StatusEntry.Text = _current.Status;
                PriorityEntry.Text = _current.Priority.ToString();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
                await Shell.Current.GoToAsync("..");
            }
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

            _current.TaskName = NameEntry.Text.Trim();
            _current.Description = DescEditor.Text?.Trim();
            _current.ProjectID = projId;
            _current.Status = StatusEntry.Text?.Trim();
            _current.Priority = pr;

            bool ok = await _service.UpdateTask(_current.TaskID, _current);
            if (ok)
            {
                await DisplayAlert("Success", "Saved", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await DisplayAlert("Error", "Update failed", "OK");
            }
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert("Confirm", "Delete this task?", "Yes", "No");
            if (!confirm) return;

            bool ok = await _service.DeleteTask(_current.TaskID);
            if (ok)
            {
                await DisplayAlert("Success", "Deleted", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await DisplayAlert("Error", "Delete failed", "OK");
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
            => await Shell.Current.GoToAsync("..");
    }
}
