using Xamarin.Forms;
using TimeTrackingMobile.Models;
using TimeTrackingMobile.Services;
using System.Collections.Generic;
using TaskModel = TimeTrackingMobile.Models.TaskModel;
using System.Threading.Tasks;
using System;

namespace TimeTrackingMobile.Views
{
    public partial class DepartmentsPage : ContentPage
    {
        private DepartmentService _deptService;
        private List<DepartmentModel> _departments;

        public DepartmentsPage()
        {
            InitializeComponent();
            _deptService = new DepartmentService(); // Tworzymy serwis do API
        }

        private async void LoadDepartmentsClicked(object sender, System.EventArgs e)
        {
            try
            {
                _departments = await _deptService.GetAllDepartments();

                await DisplayAlert("Debug", $"Pobrano: {_departments.Count} rekordów", "OK");

                DepartmentsList.ItemsSource = _departments;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Błąd", ex.Message, "OK");
            }
        }


        private async void DepartmentsList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            // Czy coś wybrano?
            var dept = e.SelectedItem as DepartmentModel;
            if (dept == null) return;

            // Przechodzimy do EmployeesPage, przekazując DepartmentID
            await Shell.Current.GoToAsync($"{nameof(EmployeesPage)}?departmentId={dept.DepartmentID}");

        }

        private async void AddDepartmentClicked(object sender, EventArgs e)
        {
            DepartmentModel newDept = new DepartmentModel
            {
                DepartmentName = "NowyDział"
            };

            bool success = await _deptService.CreateDepartment(newDept);
            if (success)
            {
                await DisplayAlert("OK", "Dodano department", "OK");
                // ponownie pobierz listę:
                await LoadDepartmentAgain();
            }
            else
            {
                await DisplayAlert("Błąd", "Nie udało się dodać", "OK");
            }
        }

        private async Task LoadDepartmentAgain()
        {
            // to samo co w LoadDepartmentsClicked
            _departments = await _deptService.GetAllDepartments();
            DepartmentsList.ItemsSource = _departments;
        }

    }
}
