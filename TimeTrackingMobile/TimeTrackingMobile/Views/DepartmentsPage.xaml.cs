using Xamarin.Forms;
using TimeTrackingMobile.Models;
using TimeTrackingMobile.Services;
using System.Collections.Generic;
using System;

namespace TimeTrackingMobile.Views
{
    public partial class DepartmentsPage : ContentPage
    {
        private DepartmentService _deptService;
        private List<Department> _departments;

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
            var dept = e.SelectedItem as Department;
            if (dept == null) return;

            // Przechodzimy do EmployeesPage, przekazując DepartmentID
            await Navigation.PushAsync(new EmployeesPage(dept.DepartmentID));
        }
    }
}
