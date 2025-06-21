using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TimeTrackingMobile.Models;
using TimeTrackingMobile.Services;

namespace TimeTrackingMobile.Views.TimeRecords
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TimeRecordsPage : ContentPage
    {
        private readonly TimeRecordService _trSvc = new TimeRecordService();
        private readonly EmployeeService _empSvc = new EmployeeService();
        private readonly TaskService _taskSvc = new TaskService();

        private List<EmployeeModel> _employees;
        private List<TaskModel> _tasks;

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
            await LoadAsync();
        }

        private async Task LoadAsync()
        {
            TimeRecords.Clear();
            try
            {
                // pobierz listy pomocnicze
                _employees = await _empSvc.GetAllEmployees();
                _tasks = await _taskSvc.GetAllTasks();

                var list = await _trSvc.GetAllTimeRecords();
                foreach (var tr in list)
                {
                    tr.EmployeeName = _employees.FirstOrDefault(e => e.EmployeeID == tr.EmployeeID)?.Name;
                    tr.TaskName = _tasks.FirstOrDefault(t => t.TaskID == tr.TaskID)?.TaskName;
                    TimeRecords.Add(tr);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void OnRefreshClicked(object sender, EventArgs e)
            => await LoadAsync();

        private async void OnAddClicked(object sender, EventArgs e)
            => await Shell.Current.GoToAsync(nameof(TimeRecordAddPage));

        private async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count == 0) return;
            var tr = e.CurrentSelection[0] as TimeRecordModel;
            ((CollectionView)sender).SelectedItem = null;
            if (tr == null) return;

            string action = await DisplayActionSheet(
                $"Record {tr.TimeRecordID}",
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
                        "Confirm", $"Delete record {tr.TimeRecordID}?", "Yes", "No");
                    if (c && await _trSvc.DeleteTimeRecord(tr.TimeRecordID))
                    {
                        await DisplayAlert("Deleted", "OK", "OK");
                        await LoadAsync();
                    }
                    break;
            }
        }
    }
}
