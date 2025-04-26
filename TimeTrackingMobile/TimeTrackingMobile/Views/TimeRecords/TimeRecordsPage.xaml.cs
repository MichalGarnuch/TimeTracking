using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TimeTrackingMobile.Models;
using TimeTrackingMobile.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace TimeTrackingMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TimeRecordsPage : ContentPage
    {
        private readonly TimeRecordService _service = new TimeRecordService();

        public ObservableCollection<TimeRecordModel> TimeRecords { get; }
            = new ObservableCollection<TimeRecordModel>();

        public TimeRecordsPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (TimeRecords.Count == 0)
                await LoadAsync();
        }

        private async void OnRefreshClicked(object sender, EventArgs e)
            => await LoadAsync();

        private async void OnAddClicked(object sender, EventArgs e)
            => await Shell.Current.GoToAsync(nameof(TimeRecordAddPage));

        private async Task LoadAsync()
        {
            TimeRecords.Clear();
            try
            {
                var list = await _service.GetAllTimeRecords();
                foreach (var tr in list)
                    TimeRecords.Add(tr);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count == 0) return;
            var tr = e.CurrentSelection[0] as TimeRecordModel;
            ((CollectionView)sender).SelectedItem = null;
            if (tr == null) return;

            string action = await DisplayActionSheet(
                $"TimeRecord {tr.TimeRecordID}",
                "Cancel", null,
                "Edit", "Delete");

            switch (action)
            {
                case "Edit":
                    await Shell.Current.GoToAsync(
                        $"{nameof(TimeRecordEditPage)}?recordId={tr.TimeRecordID}");
                    break;
                case "Delete":
                    bool c = await DisplayAlert(
                        "Confirm",
                        $"Delete record {tr.TimeRecordID}?",
                        "Yes", "No");
                    if (c)
                    {
                        bool ok = await _service.DeleteTimeRecord(tr.TimeRecordID);
                        if (ok)
                        {
                            await DisplayAlert("OK", "Deleted", "OK");
                            await LoadAsync();
                        }
                        else
                        {
                            await DisplayAlert("Error", "Delete failed", "OK");
                        }
                    }
                    break;
            }
        }
    }
}
