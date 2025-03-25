using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;
using TimeTrackingMobile.Models;
using TimeTrackingMobile.Services;

namespace TimeTrackingMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DepartmentAddPage : ContentPage
    {
        private readonly DepartmentService _departmentService;

        public DepartmentAddPage()
        {
            InitializeComponent();
            _departmentService = new DepartmentService();
        }

        private async void SaveButtonClicked(object sender, EventArgs e)
        {
            var name = DepartmentNameEntry.Text;
            if (string.IsNullOrWhiteSpace(name))
            {
                await DisplayAlert("Validation", "Department Name cannot be empty.", "OK");
                return;
            }

            var newDept = new DepartmentModel
            {
                DepartmentName = name
            };

            bool success = await _departmentService.CreateDepartment(newDept);
            if (success)
            {
                await DisplayAlert("Success", "Department created!", "OK");
                // wracamy do poprzedniej strony (DepartmentsPage)
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await DisplayAlert("Error", "Failed to create department", "OK");
            }
        }

        private async void CancelButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
