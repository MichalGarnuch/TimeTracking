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
    public partial class TaskAddPage : ContentPage
    {
        private readonly TaskService _taskSvc = new TaskService();
        private readonly ProjectService _projSvc = new ProjectService();
        private List<ProjectModel> _projects;

        public TaskAddPage()
        {
            InitializeComponent();
            LoadProjects();
        }

        private async void LoadProjects()
        {
            _projects = await _projSvc.GetAllProjects();
            ProjPicker.ItemsSource = _projects;
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

            var task = new TaskModel
            {
                TaskName = NameEntry.Text.Trim(),
                Description = DescEditor.Text?.Trim(),
                ProjectID = selProj.ProjectID,
                Status = StatusEntry.Text?.Trim(),
                Priority = pr
            };

            bool ok = await _taskSvc.CreateTask(task);
            if (ok)
            {
                await DisplayAlert("Success", "Zadanie dodane.", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await DisplayAlert("Error", "Dodanie nie powiodło się.", "OK");
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
            => await Shell.Current.GoToAsync("..");
    }
}
