using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;
using TimeTrackingMobile.Models;
using TimeTrackingMobile.Services;

namespace TimeTrackingMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TimeRecordAddPage : ContentPage
    {
        private readonly TimeRecordService _service;

        public TimeRecordAddPage()
        {
            InitializeComponent();
            _service = new TimeRecordService();

            StartDatePicker.Date = DateTime.Now;
            EndDatePicker.Date = DateTime.Now.AddHours(1);
        }

        private async void SaveButtonClicked(object sender, EventArgs e)
        {
            int empId = 0;
            int.TryParse(EmployeeIdEntry.Text, out empId);

            int taskId = 0;
            int.TryParse(TaskIdEntry.Text, out taskId);

            decimal hours = 0;
            decimal.TryParse(HoursSpentEntry.Text, out hours);

            var newRec = new TimeRecordModel
            {
                EmployeeID = empId,
                TaskID = taskId,
                StartTime = StartDatePicker.Date,
                EndTime = EndDatePicker.Date,
                HoursSpent = hours
            };

            bool success = await _service.CreateTimeRecord(newRec);
            if (success)
            {
                await DisplayAlert("OK", "TimeRecord created", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await DisplayAlert("Error", "Create failed", "OK");
            }
        }

        private async void CancelButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
