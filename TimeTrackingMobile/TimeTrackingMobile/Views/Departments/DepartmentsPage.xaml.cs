using Xamarin.Forms;
using TimeTrackingMobile.Models;
using TimeTrackingMobile.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace TimeTrackingMobile.Views
{
    public partial class DepartmentsPage : ContentPage
    {
        private readonly DepartmentService _deptService;
        private List<DepartmentModel> _departments;

        public DepartmentsPage()
        {
            InitializeComponent();
            _deptService = new DepartmentService();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadDepartments();
        }

        private async void LoadDepartmentsClicked(object sender, EventArgs e)
        {
            await LoadDepartments();
        }

        private async Task LoadDepartments()
        {
            try
            {
                _departments = await _deptService.GetAllDepartments();
                DepartmentsList.ItemsSource = _departments;
                HeaderLabel.Text = $"Działy ({_departments.Count})";
            }
            catch (Exception ex)
            {
                await DisplayAlert("Błąd", ex.Message, "OK");
            }
        }

        private async void AddDepartmentClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(DepartmentAddPage));
        }

        private async void DepartmentsList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is DepartmentModel dept)
            {
                var action = await DisplayActionSheet(
                    $"Wybrano: {dept.DepartmentName}",
                    "Cancel",
                    null,
                    "Pokaż pracowników",
                    "Edytuj dział",
                    "Usuń dział");

                if (action == "Pokaż pracowników")
                {
                    await Shell.Current.GoToAsync($"{nameof(EmployeesPage)}?departmentId={dept.DepartmentID}");
                }
                else if (action == "Edytuj dział")
                {
                    await Shell.Current.GoToAsync($"{nameof(DepartmentEditPage)}?departmentId={dept.DepartmentID}");
                }
                else if (action == "Usuń dział")
                {
                    bool confirmed = await DisplayAlert("Potwierdź", $"Usunąć '{dept.DepartmentName}'?", "Tak", "Nie");
                    if (confirmed)
                    {
                        bool deleted = await _deptService.DeleteDepartment(dept.DepartmentID);
                        if (deleted)
                        {
                            await DisplayAlert("OK", "Usunięto dział", "OK");
                            await LoadDepartments();
                        }
                        else
                        {
                            await DisplayAlert("Błąd", "Nie udało się usunąć", "OK");
                        }
                    }
                }

                DepartmentsList.SelectedItem = null;
            }
        }

        private async void OnSyncClicked(object sender, EventArgs e)
        {
            await LoadDepartments();
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
