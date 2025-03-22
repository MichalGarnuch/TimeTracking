using Xamarin.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using TimeTrackingMobile.Models;
using TimeTrackingMobile.Services;

namespace TimeTrackingMobile.Views
{
    public partial class EmployeesPage : ContentPage
    {
        private EmployeeService _empService;
        private int _deptId;

        public EmployeesPage(int deptId)
        {
            InitializeComponent();
            _empService = new EmployeeService();
            _deptId = deptId;

            HeaderLabel.Text = $"Employees in Department {_deptId}";
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // POBIERZ WSZYSTKICH PRACOWNIKÓW
            List<Employee> allEmployees = await _empService.GetAllEmployees();

            // ODFILTRUJ wg DepartmentID
            var filtered = allEmployees.Where(e => e.DepartmentID == _deptId).ToList();

            // USTAW LISTVIEW
            EmployeesList.ItemsSource = filtered;
        }

        private async void AddEmployeeClicked(object sender, EventArgs e)
        {
            // Tworzysz przykładowego nowego pracownika
            Employee newEmp = new Employee
            {
                Name = "Nowy Pracownik",
                Email = "nowy@example.com",
                DepartmentID = _deptId
            };

            bool success = await _empService.CreateEmployee(newEmp);
            if (success)
            {
                await DisplayAlert("OK", "Employee added", "OK");
                OnAppearing(); // reload list
            }
            else
            {
                await DisplayAlert("Error", "Cannot create employee", "OK");
            }
        }
    }
}
