using System;
using System.Collections.Generic;
using TimeTrackingMobile.ViewModels;
using TimeTrackingMobile.Views;
using Xamarin.Forms;

namespace TimeTrackingMobile
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            // Inne Routing.RegisterRoute(...) które już masz:

            Routing.RegisterRoute(nameof(DepartmentsPage), typeof(DepartmentsPage));
            Routing.RegisterRoute(nameof(DepartmentAddPage), typeof(DepartmentAddPage));
            Routing.RegisterRoute(nameof(DepartmentEditPage), typeof(DepartmentEditPage));

            Routing.RegisterRoute(nameof(EmployeesPage), typeof(EmployeesPage));
            Routing.RegisterRoute(nameof(EmployeeAddPage), typeof(EmployeeAddPage));
            Routing.RegisterRoute(nameof(EmployeeEditPage), typeof(EmployeeEditPage));

            // analogicznie rejestruj kolejne strony dla innych encji
            // Routing.RegisterRoute(nameof(DepartmentAddPage), typeof(DepartmentAddPage)); itp.
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
