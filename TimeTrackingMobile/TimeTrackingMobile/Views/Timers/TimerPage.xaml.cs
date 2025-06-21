using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TimeTrackingMobile.Models;
using TimeTrackingMobile.Services;

namespace TimeTrackingMobile.Views.Timers
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TimerPage : ContentPage
    {
        private readonly EmployeeService _empSvc = new EmployeeService();
        private readonly ProjectService _projSvc = new ProjectService();
        private readonly TaskService _taskSvc = new TaskService();
        private readonly TimeRecordService _trSvc = new TimeRecordService();
        private List<EmployeeModel> _employees;
        private List<ProjectModel> _projects;
        private List<TaskModel> _tasks;
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private bool _updating = false;
        private DateTime _startTime;

        public TimerPage()
        {
            InitializeComponent();
            LoadPickers();
        }

        private async void LoadPickers()
        {
            _employees = await _empSvc.GetAllEmployees();
            EmpPicker.ItemsSource = _employees;
            _projects = await _projSvc.GetAllProjects();
            ProjectPicker.ItemsSource = _projects;
            _tasks = await _taskSvc.GetAllTasks();
        }

        private void OnProjectChanged(object sender, EventArgs e)
        {
            if (ProjectPicker.SelectedItem is ProjectModel proj)
            {
                var list = _tasks.Where(t => t.ProjectID == proj.ProjectID).ToList();
                TaskPicker.ItemsSource = list;
            }
        }

        private void OnStartClicked(object sender, EventArgs e)
        {
            if (EmpPicker.SelectedItem == null || TaskPicker.SelectedItem == null)
            {
                DisplayAlert("Validation", "Wybierz pracownika i zadanie.", "OK");
                return;
            }
            _startTime = DateTime.Now;
            _stopwatch.Restart();
            _updating = true;
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                TimeLabel.Text = _stopwatch.Elapsed.ToString(@"hh\:mm\:ss");
                return _updating;
            });
            StartButton.IsEnabled = false;
            PauseButton.IsEnabled = true;
            StopButton.IsEnabled = true;
        }

        private void OnPauseClicked(object sender, EventArgs e)
        {
            if (_stopwatch.IsRunning)
            {
                _stopwatch.Stop();
                _updating = false;
                PauseButton.Text = "Resume";
            }
            else
            {
                _stopwatch.Start();
                _updating = true;
                Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                {
                    TimeLabel.Text = _stopwatch.Elapsed.ToString(@"hh\:mm\:ss");
                    return _updating;
                });
                PauseButton.Text = "Pause";
            }
        }

        private async void OnStopClicked(object sender, EventArgs e)
        {
            _stopwatch.Stop();
            _updating = false;
            StartButton.IsEnabled = true;
            PauseButton.IsEnabled = false;
            StopButton.IsEnabled = false;
            PauseButton.Text = "Pause";

            if (EmpPicker.SelectedItem is EmployeeModel emp && TaskPicker.SelectedItem is TaskModel task)
            {
                var record = new TimeRecordModel
                {
                    EmployeeID = emp.EmployeeID,
                    TaskID = task.TaskID,
                    StartTime = _startTime,
                    EndTime = DateTime.Now,
                    HoursSpent = (decimal)_stopwatch.Elapsed.TotalHours
                };
                await _trSvc.CreateTimeRecord(record);
            }
        }
    }
}