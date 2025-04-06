using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;
using TimeTrackingMobile.Models;
using TimeTrackingMobile.Services;

namespace TimeTrackingMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty(nameof(TimeRecordId), "timeRecordId")]
    public partial class TimeRecordEditPage : ContentPage
    {
        private readonly TimeRecordService _service;
        private TimeRecordModel currentRecord;

        private int _timeRecordId;
        public int TimeRecordId
        {
            get => _timeRecordId;
            set
            {
                _timeRecordId = value;
                _ = LoadRecord();
            }
        }

        public TimeRecordEditPage()
        {
            InitializeComponent();
            _service = new TimeRecordService();
        }

        private async System.Threading.Tasks.Task LoadRecord()
        {
            try
            {
                currentRecord = await _service.GetTimeRecord(_timeRecordId);
                if (currentRecord == null)
                {
                    await DisplayAlert("Error", "Not found", "OK");
                    await Shell.Current.GoToAsync("..");
                    return;
                }

                EmployeeIdEntry.Text = currentRecord.EmployeeID.ToString();
                TaskIdEntry.Text = currentRecord.TaskID.ToString();
                StartDatePicker.Date = currentRecord.StartTime;
                EndDatePicker.Date = currentRecord.EndTime;
                HoursSpentEntry.Text = currentRecord.HoursSpent.ToString();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
                await Shell.Current.GoToAsync("..");
            }
        }

        private async void SaveButtonClicked(object sender, EventArgs e)
        {
            if (currentRecord == null) return;

            int empId = 0;
            int.TryParse(EmployeeIdEntry.Text, out empId);
            currentRecord.EmployeeID = empId;

            int taskId = 0;
            int.TryParse(TaskIdEntry.Text, out taskId);
            currentRecord.TaskID = taskId;

            currentRecord.StartTime = StartDatePicker.Date;
            currentRecord.EndTime = EndDatePicker.Date;

            decimal hours = 0;
            decimal.TryParse(HoursSpentEntry.Text, out hours);
            currentRecord.HoursSpent = hours;

            bool updated = await _service.UpdateTimeRecord(_timeRecordId, currentRecord);
            if (updated)
            {
                await DisplayAlert("Success", "Updated", "OK");
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
