using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TimeTrackingMobile.Models;
using TimeTrackingMobile.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace TimeTrackingMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DepartmentsPage : ContentPage
    {
        private readonly DepartmentService _deptService;

        public ObservableCollection<DepartmentModel> Departments { get; }
            = new ObservableCollection<DepartmentModel>();

        public DepartmentsPage()
        {
            InitializeComponent();
            _deptService = new DepartmentService();
            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (Departments.Count == 0)
                await LoadDepartmentsAsync();
        }

        // Odświeżanie
        private async void OnRefreshClicked(object sender, EventArgs e)
            => await LoadDepartmentsAsync();

        // Dodawanie
        private async void OnAddClicked(object sender, EventArgs e)
            => await Shell.Current.GoToAsync(nameof(DepartmentAddPage));

        // Wczytaj listę
        private async Task LoadDepartmentsAsync()
        {
            Departments.Clear();
            try
            {
                var list = await _deptService.GetAllDepartments();
                foreach (var d in list)
                    Departments.Add(d);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Błąd", ex.Message, "OK");
            }
        }

        // Tapnięcie na kartę – ActionSheet
        private async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count == 0) return;
            var dept = e.CurrentSelection[0] as DepartmentModel;
            ((CollectionView)sender).SelectedItem = null;

            if (dept == null) return;

            string action = await DisplayActionSheet(
                $"Dział: {dept.DepartmentName}",
                "Anuluj",
                null,
                "Pokaż pracowników",
                "Edytuj dział",
                "Usuń dział");

            switch (action)
            {
                case "Pokaż pracowników":
                    // absolutna nawigacja do zakładki Employees
                    await Shell.Current.GoToAsync($"//EmployeesPage?departmentId={dept.DepartmentID}");
                    break;

                case "Edytuj dział":
                    await Shell.Current.GoToAsync(
                        $"{nameof(DepartmentEditPage)}?departmentId={dept.DepartmentID}");
                    break;

                case "Usuń dział":
                    bool confirmed = await DisplayAlert(
                        "Potwierdź",
                        $"Usunąć dział '{dept.DepartmentName}'?",
                        "Tak", "Nie");
                    if (confirmed)
                    {
                        bool ok = await _deptService.DeleteDepartment(dept.DepartmentID);
                        if (ok)
                        {
                            await DisplayAlert("OK", "Dział usunięty", "OK");
                            await LoadDepartmentsAsync();
                        }
                        else
                        {
                            await DisplayAlert("Błąd", "Nie udało się usunąć działu", "OK");
                        }
                    }
                    break;
            }
        }
    }
}
