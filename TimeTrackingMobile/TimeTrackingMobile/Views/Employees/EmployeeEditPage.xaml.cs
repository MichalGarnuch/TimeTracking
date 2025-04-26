using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TimeTrackingMobile.Models;
using TimeTrackingMobile.Services;

namespace TimeTrackingMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty(nameof(EmployeeId), "employeeId")]
    public partial class EmployeeEditPage : ContentPage
    {
        private readonly EmployeeService _service = new EmployeeService();
        private EmployeeModel _current;

        public int EmployeeId
        {
            get => _empId;
            set
            {
                _empId = value;
                LoadEmployee(value);
            }
        }
        private int _empId;

        public EmployeeEditPage()
        {
            InitializeComponent();
        }

        private async void LoadEmployee(int id)
        {
            try
            {
                _current = await _service.GetEmployee(id);
                if (_current == null) throw new Exception("Not found");
                NameEntry.Text = _current.Name;
                EmailEntry.Text = _current.Email;
                DeptIdEntry.Text = _current.DepartmentID.ToString();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
                await Shell.Current.GoToAsync("..");
            }
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

            _current.Name = name;
            _current.Email = email;
            _current.DepartmentID = deptId;

            bool ok = await _service.UpdateEmployee(_current.EmployeeID, _current);
            if (ok)
            {
                await DisplayAlert("Success", "Updated", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await DisplayAlert("Error", "Update failed", "OK");
            }
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert(
                "Confirm", "Delete this employee?", "Yes", "No");
            if (!confirm) return;

            bool ok = await _service.DeleteEmployee(_current.EmployeeID);
            if (ok)
            {
                await DisplayAlert("Success", "Deleted", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await DisplayAlert("Error", "Delete failed", "OK");
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
