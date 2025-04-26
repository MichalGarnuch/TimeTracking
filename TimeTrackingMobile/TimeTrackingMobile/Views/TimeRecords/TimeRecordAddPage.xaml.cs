using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TimeTrackingMobile.Models;
using TimeTrackingMobile.Services;

namespace TimeTrackingMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TimeRecordAddPage : ContentPage
    {
        private readonly TimeRecordService _service = new TimeRecordService();

        public TimeRecordAddPage()
        {
            InitializeComponent();
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

            var record = new TimeRecordModel
            {
                EmployeeID = empId,
                TaskID = taskId,
                StartTime = StartDatePicker.Date,
                EndTime = EndDatePicker.Date,
                HoursSpent = hrs
            };

            bool ok = await _service.CreateTimeRecord(record);
            if (ok)
            {
                await DisplayAlert("Success", "Created", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await DisplayAlert("Error", "Create failed", "OK");
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
            => await Shell.Current.GoToAsync("..");
    }
}
