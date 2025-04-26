using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TimeTrackingMobile.Models;
using TimeTrackingMobile.Services;

namespace TimeTrackingMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty(nameof(RecordId), "recordId")]
    public partial class TimeRecordEditPage : ContentPage
    {
        private readonly TimeRecordService _service = new TimeRecordService();
        private TimeRecordModel _current;

        public int RecordId
        {
            get => _id;
            set { _id = value; LoadRecord(value); }
        }
        private int _id;

        public TimeRecordEditPage()
        {
            InitializeComponent();
        }

        private async void LoadRecord(int id)
        {
            try
            {
                _current = await _service.GetTimeRecord(id);
                if (_current == null) throw new Exception("Not found");
                EmpEntry.Text = _current.EmployeeID.ToString();
                TaskEntry.Text = _current.TaskID.ToString();
                StartDatePicker.Date = _current.StartTime;
                EndDatePicker.Date = _current.EndTime;
                HoursEntry.Text = _current.HoursSpent.ToString();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
                await Shell.Current.GoToAsync("..");
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            if (!int.TryParse(EmpEntry.Text, out int empId)
             || !int.TryParse(TaskEntry.Text, out int taskId)
             || !decimal.TryParse(HoursEntry.Text, out decimal hrs))
            {
                await DisplayAlert("Validation", "Invalid numeric fields.", "OK");
                return;
            }

            _current.EmployeeID = empId;
            _current.TaskID = taskId;
            _current.StartTime = StartDatePicker.Date;
            _current.EndTime = EndDatePicker.Date;
            _current.HoursSpent = hrs;

            bool ok = await _service.UpdateTimeRecord(_current.TimeRecordID, _current);
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
            bool confirm = await DisplayAlert("Confirm", "Delete this record?", "Yes", "No");
            if (!confirm) return;

            bool ok = await _service.DeleteTimeRecord(_current.TimeRecordID);
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
