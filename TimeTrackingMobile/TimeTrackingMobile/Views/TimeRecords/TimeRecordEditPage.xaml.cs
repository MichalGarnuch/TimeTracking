using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TimeTrackingMobile.Models;
using TimeTrackingMobile.Services;

namespace TimeTrackingMobile.Views.TimeRecords
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty(nameof(RecordId), "recordId")]
    public partial class TimeRecordEditPage : ContentPage
    {
        // serwisy
        private readonly TimeRecordService _recordSvc = new TimeRecordService();
        private readonly EmployeeService _empSvc = new EmployeeService();
        private readonly TaskService _taskSvc = new TaskService();

        private List<EmployeeModel> _emps;
        private List<TaskModel> _tasks;
        private TimeRecordModel _current;
        private int _recordId;

        public int RecordId
        {
            get => _recordId;
            set => _recordId = value;
        }

        public TimeRecordEditPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // 1) Załaduj listy do pickerów
            _emps = await _empSvc.GetAllEmployees();
            EmpPicker.ItemsSource = _emps;

            _tasks = await _taskSvc.GetAllTasks();
            TaskPicker.ItemsSource = _tasks;

            // 2) Załaduj rekord
            await LoadRecordAsync(_recordId);
        }

        private async System.Threading.Tasks.Task LoadRecordAsync(int id)
        {
            try
            {
                _current = await _recordSvc.GetTimeRecord(id);
                if (_current == null)
                    throw new Exception("Nie znaleziono rekordu.");

                // ustawienia w UI
                EmpPicker.SelectedItem = _emps.FirstOrDefault(e => e.EmployeeID == _current.EmployeeID);
                TaskPicker.SelectedItem = _tasks.FirstOrDefault(t => t.TaskID == _current.TaskID);
                StartPicker.Date = _current.StartTime;
                EndPicker.Date = _current.EndTime;
                HoursEntry.Text = _current.HoursSpent.ToString("F2");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Błąd", ex.Message, "OK");
                await Shell.Current.GoToAsync("..");
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            if (!(EmpPicker.SelectedItem is EmployeeModel emp) ||
                !(TaskPicker.SelectedItem is TaskModel task) ||
                 !decimal.TryParse(HoursEntry.Text, out var hrs))
            {
                await DisplayAlert("Validation", "Wypełnij wszystkie pola poprawnie.", "OK");
                return;
            }

            _current.EmployeeID = emp.EmployeeID;
            _current.TaskID = task.TaskID;
            _current.StartTime = StartPicker.Date;
            _current.EndTime = EndPicker.Date;
            _current.HoursSpent = hrs;

            bool ok = await _recordSvc.UpdateTimeRecord(_current.TimeRecordID, _current);
            if (ok)
            {
                await DisplayAlert("Sukces", "Zapisano zmiany.", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await DisplayAlert("Błąd", "Aktualizacja nie powiodła się.", "OK");
            }
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            bool confirmed = await DisplayAlert(
                "Potwierdź",
                $"Usunąć rekord #{_current.TimeRecordID}?",
                "Tak", "Nie");
            if (!confirmed) return;

            if (await _recordSvc.DeleteTimeRecord(_current.TimeRecordID))
            {
                await DisplayAlert("Sukces", "Usunięto.", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await DisplayAlert("Błąd", "Usuwanie nie powiodło się.", "OK");
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
            => await Shell.Current.GoToAsync("..");
    }
}
