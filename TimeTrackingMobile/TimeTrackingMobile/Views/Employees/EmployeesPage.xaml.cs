using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TimeTrackingMobile.Services;
using TimeTrackingMobile.Models;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace TimeTrackingMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty(nameof(DepartmentID), "departmentId")]
    public partial class EmployeesPage : ContentPage
    {
        private readonly EmployeeService _employeeService = new EmployeeService();
        private int _departmentId;

        public int DepartmentID
        {
            get => _departmentId;
            set
            {
                _departmentId = value;
                _ = LoadEmployees(); // wywołanie async w setterze
            }
        }

        public EmployeesPage()
        {
            InitializeComponent();
        }

        private async Task LoadEmployees()
        {
            try
            {
                var employees = await _employeeService.GetEmployeesByDepartment(_departmentId);
                EmployeesList.ItemsSource = employees;
                HeaderLabel.Text = $"Dział {_departmentId} – pracownicy ({employees.Count})";
            }
            catch (Exception ex)
            {
                await DisplayAlert("Błąd", ex.Message, "OK");
            }
        }

        private async void LoadEmployeesClicked(object sender, EventArgs e)
        {
            await LoadEmployees();
        }

        private async void AddEmployeeClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(EmployeeAddPage));
        }

        private async void EmployeesList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is EmployeeModel selectedEmployee)
            {
                await Shell.Current.GoToAsync($"{nameof(EmployeeEditPage)}?employeeId={selectedEmployee.EmployeeID}");
                EmployeesList.SelectedItem = null;
            }
        }
    }
}
