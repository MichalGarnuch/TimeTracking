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
        private readonly EmployeeService _service = new EmployeeService();

        public EmployeeAddPage()
        {
            InitializeComponent();
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            var name = NameEntry.Text?.Trim();
            var email = EmailEntry.Text?.Trim();
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email))
            {
                await DisplayAlert("Validation", "Name and Email are required.", "OK");
                return;
            }

            if (!int.TryParse(DeptIdEntry.Text, out int deptId))
            {
                await DisplayAlert("Validation", "Invalid Department ID.", "OK");
                return;
            }

            var emp = new EmployeeModel
            {
                Name = name,
                Email = email,
                DepartmentID = deptId
            };

            bool ok = await _service.CreateEmployee(emp);
            if (ok)
            {
                await DisplayAlert("Success", "Employee added", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await DisplayAlert("Error", "Add failed", "OK");
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
