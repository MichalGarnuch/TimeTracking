using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TimeTrackingMobile.Models;
using TimeTrackingMobile.Services;

namespace TimeTrackingMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty(nameof(DepartmentId), "departmentId")]
    public partial class DepartmentEditPage : ContentPage
    {
        private readonly DepartmentService _service = new DepartmentService();
        private DepartmentModel _current;

        public int DepartmentId
        {
            get => _deptId;
            set
            {
                _deptId = value;
                LoadDepartment(value);
            }
        }
        private int _deptId;

        public DepartmentEditPage()
        {
            InitializeComponent();
        }

        private async void LoadDepartment(int id)
        {
            try
            {
                _current = await _service.GetDepartment(id);
                if (_current == null) throw new Exception("Not found");
                DepartmentNameEntry.Text = _current.DepartmentName;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
                await Shell.Current.GoToAsync("..");
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            var newName = DepartmentNameEntry.Text?.Trim();
            if (string.IsNullOrEmpty(newName))
            {
                await DisplayAlert("Validation", "Name cannot be empty.", "OK");
                return;
            }

            _current.DepartmentName = newName;
            bool ok = await _service.UpdateDepartment(_current.DepartmentID, _current);
            if (ok)
            {
                await DisplayAlert("Success", "Updated.", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await DisplayAlert("Error", "Update failed.", "OK");
            }
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert(
                "Confirm",
                $"Delete '{_current.DepartmentName}'?",
                "Yes",
                "No");
            if (!confirm) return;

            bool ok = await _service.DeleteDepartment(_current.DepartmentID);
            if (ok)
            {
                await DisplayAlert("Success", "Deleted.", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await DisplayAlert("Error", "Delete failed.", "OK");
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
