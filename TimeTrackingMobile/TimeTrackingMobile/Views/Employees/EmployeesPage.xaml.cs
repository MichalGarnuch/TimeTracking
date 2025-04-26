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
    public partial class EmployeesPage : ContentPage
    {
        private readonly EmployeeService _service = new EmployeeService();

        public ObservableCollection<EmployeeModel> Employees { get; }
            = new ObservableCollection<EmployeeModel>();

        public EmployeesPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (Employees.Count == 0)
                await LoadEmployeesAsync();
        }

        private async void OnRefreshClicked(object sender, EventArgs e)
        {
            await LoadEmployeesAsync();
        }

        private async void OnAddClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(EmployeeAddPage));
        }

        private async Task LoadEmployeesAsync()
        {
            Employees.Clear();
            try
            {
                var list = await _service.GetAllEmployees();
                foreach (var emp in list)
                    Employees.Add(emp);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count == 0) return;
            var emp = e.CurrentSelection[0] as EmployeeModel;
            ((CollectionView)sender).SelectedItem = null;

            if (emp == null) return;

            string action = await DisplayActionSheet(
                $"{emp.Name}",
                "Cancel",
                null,
                "Edit",
                "Delete");

            switch (action)
            {
                case "Edit":
                    await Shell.Current.GoToAsync($"{nameof(EmployeeEditPage)}?employeeId={emp.EmployeeID}");
                    break;
                case "Delete":
                    bool confirm = await DisplayAlert("Confirm",
                        $"Delete '{emp.Name}'?",
                        "Yes", "No");
                    if (confirm)
                    {
                        bool ok = await _service.DeleteEmployee(emp.EmployeeID);
                        if (ok)
                        {
                            await DisplayAlert("OK", "Deleted", "OK");
                            await LoadEmployeesAsync();
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
