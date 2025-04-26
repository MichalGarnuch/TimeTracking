using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly TaskService _taskSvc = new TaskService();
        private readonly ProjectService _projSvc = new ProjectService();
        private TaskModel _current;
        private List<ProjectModel> _projects;

        public int TaskId
        {
            set { _id = value; }
            get => _id;
        }
        private int _id;

        public TaskEditPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadProjectsAsync();
            await LoadTaskAsync();
        }

        private async System.Threading.Tasks.Task LoadProjectsAsync()
        {
            _projects = await _projSvc.GetAllProjects();
            ProjPicker.ItemsSource = _projects;
        }

        private async System.Threading.Tasks.Task LoadTaskAsync()
        {
            _current = await _taskSvc.GetTask(_id);
            if (_current == null)
            {
                await DisplayAlert("Error", "Nie znaleziono zadania.", "OK");
                await Shell.Current.GoToAsync("..");
                return;
            }

            NameEntry.Text = _current.TaskName;
            DescEditor.Text = _current.Description;
            StatusEntry.Text = _current.Status;
            PriorityEntry.Text = _current.Priority.ToString();

            ProjPicker.SelectedItem =
              _projects.FirstOrDefault(p => p.ProjectID == _current.ProjectID);
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameEntry.Text)
             || !(ProjPicker.SelectedItem is ProjectModel selProj)
             || !int.TryParse(PriorityEntry.Text, out int pr))
            {
                await DisplayAlert("Validation", "Wypełnij wszystkie pola poprawnie.", "OK");
                return;
            }

            _current.TaskName = NameEntry.Text.Trim();
            _current.Description = DescEditor.Text?.Trim();
            _current.ProjectID = selProj.ProjectID;
            _current.Status = StatusEntry.Text?.Trim();
            _current.Priority = pr;

            bool ok = await _taskSvc.UpdateTask(_current.TaskID, _current);
            if (ok)
            {
                await DisplayAlert("Success", "Zapisano zmiany.", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await DisplayAlert("Error", "Aktualizacja nie powiodła się.", "OK");
            }
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert("Confirm", $"Usuń zadanie '{_current.TaskName}'?", "Tak", "Nie");
            if (!confirm) return;

            if (await _taskSvc.DeleteTask(_current.TaskID))
            {
                await DisplayAlert("Success", "Usunięto.", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await DisplayAlert("Error", "Usuwanie nie powiodło się.", "OK");
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
            => await Shell.Current.GoToAsync("..");
    }
}
