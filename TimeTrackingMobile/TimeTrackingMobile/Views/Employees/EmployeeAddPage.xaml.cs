using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TimeTrackingMobile.Models;
using TimeTrackingMobile.Services;
using System.Collections.Generic;

namespace TimeTrackingMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmployeeAddPage : ContentPage
    {
        private readonly EmployeeService _empSvc = new EmployeeService();
        private readonly DepartmentService _deptSvc = new DepartmentService();
        private List<DepartmentModel> _departments;

        public EmployeeAddPage()
        {
            InitializeComponent();
            LoadDepartments();
        }

        private async void LoadDepartments()
        {
            _departments = await _deptSvc.GetAllDepartments();
            DeptPicker.ItemsSource = _departments;
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            var name = NameEntry.Text?.Trim();
            var email = EmailEntry.Text?.Trim();
            var selectedDept = DeptPicker.SelectedItem as DepartmentModel;

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || selectedDept == null)
            {
                await DisplayAlert("Validation", "Wszystkie pola muszą być wypełnione.", "OK");
                return;
            }

            var emp = new EmployeeModel
            {
                Name = name,
                Email = email,
                DepartmentID = selectedDept.DepartmentID
            };

            if (await _empSvc.CreateEmployee(emp))
            {
                await DisplayAlert("OK", "Dodano pracownika", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await DisplayAlert("Error", "Nie udało się dodać", "OK");
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
            => await Shell.Current.GoToAsync("..");
    }
}
