using Xamarin.Forms;
using TimeTrackingMobile.Models;
using TimeTrackingMobile.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace TimeTrackingMobile.Views
{
    public partial class TimeRecordsPage : ContentPage
    {
        private readonly TimeRecordService _service;
        private List<TimeRecordModel> _records;

        public TimeRecordsPage()
        {
            InitializeComponent();
            _service = new TimeRecordService();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadTimeRecords();
        }

        private async void LoadTimeRecordsClicked(object sender, EventArgs e)
        {
            await LoadTimeRecords();
        }

        private async Task LoadTimeRecords()
        {
            try
            {
                _records = await _service.GetAllTimeRecords();
                TimeRecordsList.ItemsSource = _records;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void AddTimeRecordClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(TimeRecordAddPage));
        }

        private async void TimeRecordsList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is TimeRecordModel rec)
            {
                var action = await DisplayActionSheet(
                    $"Selected ID: {rec.TimeRecordID}",
                    "Cancel",
                    null,
                    "Edit",
                    "Delete");

                if (action == "Edit")
                {
                    await Shell.Current.GoToAsync($"{nameof(TimeRecordEditPage)}?timeRecordId={rec.TimeRecordID}");
                }
                else if (action == "Delete")
                {
                    bool confirm = await DisplayAlert("Confirm", $"Delete TimeRecord {rec.TimeRecordID}?", "Yes", "No");
                    if (confirm)
                    {
                        bool deleted = await _service.DeleteTimeRecord(rec.TimeRecordID);
                        if (deleted)
                        {
                            await DisplayAlert("OK", "Deleted", "OK");
                            await LoadTimeRecords();
                        }
                        else
                        {
                            await DisplayAlert("Error", "Delete failed", "OK");
                        }
                    }
                }
                TimeRecordsList.SelectedItem = null;
            }
        }
    }
}
