using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TimeTrackingMobile.Models;
using TimeTrackingMobile.Services;

namespace TimeTrackingMobile.Views
{
    [QueryProperty(nameof(EmployeeId), "employeeId")]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmployeeEditPage : ContentPage
    {
        private readonly EmployeeService _employeeService;

        // Properta, przez którą Shell przekazuje param "employeeId"
        public int EmployeeId { get; set; }

        private EmployeeModel currentEmployee;

        public EmployeeEditPage()
        {
            InitializeComponent();
            _employeeService = new EmployeeService();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Wczytujemy pracownika
            currentEmployee = await _employeeService.GetEmployee(EmployeeId);
            if (currentEmployee == null)
            {
                await DisplayAlert("Error", "Employee not found", "OK");
                await Shell.Current.GoToAsync("..");
                return;
            }

            // Wypełniamy pola
            NameEntry.Text = currentEmployee.Name;
            EmailEntry.Text = currentEmployee.Email;
            DepartmentIdEntry.Text = currentEmployee.DepartmentID.ToString();
        }

        private async void SaveButtonClicked(object sender, EventArgs e)
        {
            // Aktualizujemy dane w obiekcie
            currentEmployee.Name = NameEntry.Text;
            currentEmployee.Email = EmailEntry.Text;

            int deptId;
            int.TryParse(DepartmentIdEntry.Text, out deptId);
            currentEmployee.DepartmentID = deptId;

            // Wywołanie metody update w serwisie
            bool success = await _employeeService.UpdateEmployee(currentEmployee.EmployeeID, currentEmployee);
            if (success)
            {
                await DisplayAlert("Success", "Employee updated", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await DisplayAlert("Error", "Failed to update", "OK");
            }
        }

        private async void CancelButtonClicked(object sender, EventArgs e)
        {
            // cofamy się
            await Shell.Current.GoToAsync("..");
        }
    }
}
