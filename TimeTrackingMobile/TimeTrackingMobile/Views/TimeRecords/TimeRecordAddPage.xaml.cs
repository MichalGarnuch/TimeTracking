using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TimeTrackingMobile.Models;
using TimeTrackingMobile.Services;

namespace TimeTrackingMobile.Views.TimeRecords
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TimeRecordAddPage : ContentPage
    {
        private readonly TimeRecordService _trSvc = new TimeRecordService();
        private readonly EmployeeService _eSvc = new EmployeeService();
        private readonly TaskService _tSvc = new TaskService();

        private List<EmployeeModel> _emps;
        private List<TaskModel> _tasks;

        public TimeRecordAddPage()
        {
            InitializeComponent();
            LoadPickers();
        }

        private async void LoadPickers()
        {
            _emps = await _eSvc.GetAllEmployees();
            EmpPicker.ItemsSource = _emps;

            _tasks = await _tSvc.GetAllTasks();
            TaskPicker.ItemsSource = _tasks;
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            if (!(EmpPicker.SelectedItem is EmployeeModel emp) ||
                !(TaskPicker.SelectedItem is TaskModel task) ||
                !decimal.TryParse(HoursEntry.Text, out var hrs))
            {
                await DisplayAlert("Validation", "Fill all fields correctly.", "OK");
                return;
            }

            var rec = new TimeRecordModel
            {
                EmployeeID = emp.EmployeeID,
                TaskID = task.TaskID,
                StartTime = StartPicker.Date,
                EndTime = EndPicker.Date,
                HoursSpent = hrs
            };

            if (await _trSvc.CreateTimeRecord(rec))
            {
                await DisplayAlert("Success", "Record created.", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await DisplayAlert("Error", "Creation failed.", "OK");
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
            => await Shell.Current.GoToAsync("..");
    }
}
