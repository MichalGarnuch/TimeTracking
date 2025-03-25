using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TimeTrackingMobile.Models;
using TimeTrackingMobile.Services;

namespace TimeTrackingMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmployeeAddPage : ContentPage
    {
        private readonly EmployeeService _employeeService;

        public EmployeeAddPage()
        {
            InitializeComponent();
            _employeeService = new EmployeeService();
        }

        private async void SaveButtonClicked(object sender, EventArgs e)
        {
            // Odczyt danych z pól
            var name = NameEntry.Text;
            var email = EmailEntry.Text;
            var departmentText = DepartmentIdEntry.Text;

            // Prosta konwersja deptID
            int deptId = 0;
            int.TryParse(departmentText, out deptId);

            var newEmployee = new EmployeeModel
            {
                Name = name,
                Email = email,
                DepartmentID = deptId
            };

            bool success = await _employeeService.CreateEmployee(newEmployee);
            if (success)
            {
                await DisplayAlert("Success", "Employee created.", "OK");
                // cofamy się do poprzedniej strony (lista)
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await DisplayAlert("Error", "Failed to create employee", "OK");
            }
        }

        private async void CancelButtonClicked(object sender, EventArgs e)
        {
            // Po prostu cofamy się
            await Shell.Current.GoToAsync("..");
        }
    }
}
