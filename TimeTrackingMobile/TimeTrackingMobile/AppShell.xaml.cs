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

            Routing.RegisterRoute(nameof(ProjectsPage), typeof(ProjectsPage));
            Routing.RegisterRoute(nameof(ProjectAddPage), typeof(ProjectAddPage));
            Routing.RegisterRoute(nameof(ProjectEditPage), typeof(ProjectEditPage));

            Routing.RegisterRoute(nameof(ProjectTypesPage), typeof(ProjectTypesPage));
            Routing.RegisterRoute(nameof(ProjectTypeAddPage), typeof(ProjectTypeAddPage));
            Routing.RegisterRoute(nameof(ProjectTypeEditPage), typeof(ProjectTypeEditPage));

            Routing.RegisterRoute(nameof(TagsPage), typeof(TagsPage));
            Routing.RegisterRoute(nameof(TagAddPage), typeof(TagAddPage));
            Routing.RegisterRoute(nameof(TagEditPage), typeof(TagEditPage));

            Routing.RegisterRoute(nameof(TasksPage), typeof(TasksPage));
            Routing.RegisterRoute(nameof(TaskAddPage), typeof(TaskAddPage));
            Routing.RegisterRoute(nameof(TaskEditPage), typeof(TaskEditPage));

            Routing.RegisterRoute(nameof(TimeRecordsPage), typeof(TimeRecordsPage));
            Routing.RegisterRoute(nameof(TimeRecordAddPage), typeof(TimeRecordAddPage));
            Routing.RegisterRoute(nameof(TimeRecordEditPage), typeof(TimeRecordEditPage));

            Routing.RegisterRoute(nameof(TaskTagsPage), typeof(TaskTagsPage));
            Routing.RegisterRoute(nameof(TaskTagAddPage), typeof(TaskTagAddPage));

            // analogicznie rejestruj kolejne strony dla innych encji
            // Routing.RegisterRoute(nameof(DepartmentAddPage), typeof(DepartmentAddPage)); itp.
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
