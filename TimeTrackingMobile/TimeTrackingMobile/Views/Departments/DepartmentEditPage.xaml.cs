using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;
using TimeTrackingMobile.Models;
using TimeTrackingMobile.Services;

namespace TimeTrackingMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    // Deklarujemy, że strona przyjmie query param "departmentId"
    [QueryProperty(nameof(DepartmentId), "departmentId")]
    public partial class DepartmentEditPage : ContentPage
    {
        private readonly DepartmentService _departmentService;
        private DepartmentModel currentDepartment;

        // To jest ID, które przyjdzie w URL np. "?departmentId=5"
        private int _departmentId;
        public int DepartmentId
        {
            get => _departmentId;
            set
            {
                _departmentId = value;
                // Po ustawieniu parametru wczytajmy dane (async)
                _ = LoadDepartment();
            }
        }

        public DepartmentEditPage()
        {
            InitializeComponent();
            _departmentService = new DepartmentService();
        }

        private async System.Threading.Tasks.Task LoadDepartment()
        {
            try
            {
                currentDepartment = await _departmentService.GetDepartment(_departmentId);
                if (currentDepartment == null)
                {
                    await DisplayAlert("Error", "Department not found", "OK");
                    await Shell.Current.GoToAsync("..");
                    return;
                }
                // Wyświetlamy nazwę
                DepartmentNameEntry.Text = currentDepartment.DepartmentName;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
                await Shell.Current.GoToAsync("..");
            }
        }

        private async void SaveButtonClicked(object sender, EventArgs e)
        {
            if (currentDepartment == null) return;

            // Odczyt nowej nazwy
            var newName = DepartmentNameEntry.Text;
            if (string.IsNullOrWhiteSpace(newName))
            {
                await DisplayAlert("Validation", "Department Name cannot be empty.", "OK");
                return;
            }

            currentDepartment.DepartmentName = newName;
            bool success = await _departmentService.UpdateDepartment(_departmentId, currentDepartment);
            if (success)
            {
                await DisplayAlert("OK", "Department updated", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await DisplayAlert("Error", "Failed to update", "OK");
            }
        }

        private async void DeleteButtonClicked(object sender, EventArgs e)
        {
            if (currentDepartment == null) return;

            bool confirmed = await DisplayAlert(
                "Confirm",
                $"Are you sure to delete '{currentDepartment.DepartmentName}'?",
                "Yes", "No");

            if (!confirmed)
                return;

            bool success = await _departmentService.DeleteDepartment(_departmentId);
            if (success)
            {
                await DisplayAlert("OK", "Department deleted", "OK");
                // wracamy do listy
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await DisplayAlert("Error", "Delete failed", "OK");
            }
        }

        private async void CancelButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
