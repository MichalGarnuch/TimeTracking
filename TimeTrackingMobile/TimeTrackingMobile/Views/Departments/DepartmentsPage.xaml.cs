using Xamarin.Forms;
using TimeTrackingMobile.Models;
using TimeTrackingMobile.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace TimeTrackingMobile.Views
{
    public partial class DepartmentsPage : ContentPage
    {
        private readonly DepartmentService _deptService;

        // ✚ zastępujemy "new()" explicite: 
        public ObservableCollection<DepartmentModel> Departments { get; }
            = new ObservableCollection<DepartmentModel>();

        public Command LoadDepartmentsCommand { get; }

        public DepartmentsPage()
        {
            InitializeComponent();
            _deptService = new DepartmentService();

            LoadDepartmentsCommand = new Command(async () => await ExecuteLoadDepartments());

            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (Departments.Count == 0)
                await ExecuteLoadDepartments();
        }

        private async Task ExecuteLoadDepartments()
        {
            Departments.Clear();
            var list = await _deptService.GetAllDepartments();
            foreach (var d in list)
                Departments.Add(d);
        }
    }
}
