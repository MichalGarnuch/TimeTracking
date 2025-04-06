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
            // Opcjonalnie automatyczne wczytanie
            await LoadDepartments();
        }

        // Ręczne wczytanie z API
        private async void LoadDepartmentsClicked(object sender, System.EventArgs e)
        {
            await LoadDepartments();
        }

        private async Task LoadDepartments()
        {
            try
            {
                _departments = await _deptService.GetAllDepartments();
                DepartmentsList.ItemsSource = _departments;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Błąd", ex.Message, "OK");
            }
        }

        // Po kliknięciu "Add Department" - przechodzimy do nowej strony z formularzem
        private async void AddDepartmentClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(DepartmentAddPage));
        }

        // Kliknięcie w komórkę listy
        private async void DepartmentsList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is DepartmentModel dept)
            {
                // Wyświetlamy ActionSheet z 3 opcjami
                var action = await DisplayActionSheet(
                    $"Wybrano dział: {dept.DepartmentName}",
                    "Cancel",
                    null,
                    "Pokaż pracowników",
                    "Edytuj dział",
                    "Usuń dział");

                // Reakcja na wybór
                if (action == "Pokaż pracowników")
                {
                    // Stary mechanizm: idziemy do EmployeesPage, przekazując departmentId
                    await Shell.Current.GoToAsync($"{nameof(EmployeesPage)}?departmentId={dept.DepartmentID}");
                }
                else if (action == "Edytuj dział")
                {
                    // Przechodzimy do strony edycji (DepartmentEditPage), przekazując ID
                    await Shell.Current.GoToAsync($"{nameof(DepartmentEditPage)}?departmentId={dept.DepartmentID}");
                }
                else if (action == "Usuń dział")
                {
                    // Możemy tu zrobić potwierdzenie i usunąć od razu z listy:
                    bool confirmed = await DisplayAlert(
                        "Confirm",
                        $"Czy na pewno chcesz usunąć '{dept.DepartmentName}'?",
                        "Yes", "No");
                    if (confirmed)
                    {
                        bool deleted = await _deptService.DeleteDepartment(dept.DepartmentID);
                        if (deleted)
                        {
                            await DisplayAlert("OK", "Dział usunięty", "OK");
                            await LoadDepartments(); // odśwież
                        }
                        else
                        {
                            await DisplayAlert("Error", "Nie udało się usunąć", "OK");
                        }
                    }
                }

                // Odznaczenie
                DepartmentsList.SelectedItem = null;
            }
        }
    }
}
