using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TimeTrackingMobile.Models;
using TimeTrackingMobile.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TimeTrackingMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty(nameof(EmployeeId), "employeeId")]
    public partial class EmployeeEditPage : ContentPage
    {
        private readonly EmployeeService _empSvc = new EmployeeService();
        private readonly DepartmentService _deptSvc = new DepartmentService();
        private EmployeeModel _current;
        private List<DepartmentModel> _departments;

        public int EmployeeId { get; set; }

        public EmployeeEditPage()
        {
            InitializeComponent();
            LoadDepartments();
        }

        private async void LoadDepartments()
        {
            _departments = await _deptSvc.GetAllDepartments();
            DeptPicker.ItemsSource = _departments;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadEmployeeAsync();
        }

        private async Task LoadEmployeeAsync()
        {
            _current = await _empSvc.GetEmployee(EmployeeId);
            if (_current == null)
            {
                await DisplayAlert("Error", "Nie znaleziono pracownika", "OK");
                await Shell.Current.GoToAsync("..");
                return;
            }
            NameEntry.Text = _current.Name;
            EmailEntry.Text = _current.Email;
            DeptPicker.SelectedItem = _departments.FirstOrDefault(d => d.DepartmentID == _current.DepartmentID);
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            var name = NameEntry.Text?.Trim();
            var email = EmailEntry.Text?.Trim();
            var dept = DeptPicker.SelectedItem as DepartmentModel;
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || dept == null)
            {
                await DisplayAlert("Validation", "Wszystkie pola muszą być wypełnione.", "OK");
                return;
            }

            _current.Name = name;
            _current.Email = email;
            _current.DepartmentID = dept.DepartmentID;

            if (await _empSvc.UpdateEmployee(_current.EmployeeID, _current))
            {
                await DisplayAlert("OK", "Zaktualizowano", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await DisplayAlert("Error", "Nie udało się zaktualizować", "OK");
            }
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            bool c = await DisplayAlert("Confirm", $"Usuń {_current.Name}?", "Tak", "Nie");
            if (!c) return;

            if (await _empSvc.DeleteEmployee(_current.EmployeeID))
            {
                await DisplayAlert("OK", "Usunięto", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await DisplayAlert("Error", "Nie udało się usunąć", "OK");
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
            => await Shell.Current.GoToAsync("..");
    }
}
