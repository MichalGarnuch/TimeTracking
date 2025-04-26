using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TimeTrackingMobile.Models;
using TimeTrackingMobile.Services;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace TimeTrackingMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty(nameof(ProjectId), "projectId")]
    public partial class ProjectEditPage : ContentPage
    {
        private readonly ProjectService _projectSvc = new ProjectService();
        private readonly ProjectTypeService _typeSvc = new ProjectTypeService();
        private ProjectModel _current;
        private List<ProjectTypeModel> _types;

        public int ProjectId
        {
            set { _id = value; }
            get => _id;
        }
        private int _id;

        public ProjectEditPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadTypesAsync();
            await LoadProjectAsync();
        }

        private async Task LoadTypesAsync()
        {
            _types = await _typeSvc.GetAllProjectTypes();
            TypePicker.ItemsSource = _types;
        }

        private async Task LoadProjectAsync()
        {
            _current = await _projectSvc.GetProject(_id);
            if (_current == null)
            {
                await DisplayAlert("Error", "Nie znaleziono projektu.", "OK");
                await Shell.Current.GoToAsync("..");
                return;
            }

            NameEntry.Text = _current.ProjectName;
            BudgetEntry.Text = _current.Budget.ToString();
            StartPicker.Date = _current.StartDate;
            EndPicker.Date = _current.EndDate;

            // wybieramy w Pickerze ten typ
            TypePicker.SelectedItem = _types
              .FirstOrDefault(t => t.ProjectTypeID == _current.ProjectTypeID);
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameEntry.Text)
             || !(TypePicker.SelectedItem is ProjectTypeModel selType)
             || !decimal.TryParse(BudgetEntry.Text, out decimal budget))
            {
                await DisplayAlert("Validation", "Wypełnij wszystkie pola poprawnie.", "OK");
                return;
            }

            _current.ProjectName = NameEntry.Text.Trim();
            _current.ProjectTypeID = selType.ProjectTypeID;
            _current.StartDate = StartPicker.Date;
            _current.EndDate = EndPicker.Date;
            _current.Budget = budget;

            bool ok = await _projectSvc.UpdateProject(_current.ProjectID, _current);
            if (ok)
            {
                await DisplayAlert("Success", "Zapisano zmiany.", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await DisplayAlert("Error", "Aktualizacja nie powiodła się.", "OK");
            }
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert("Confirm", $"Usuń projekt '{_current.ProjectName}'?", "Tak", "Nie");
            if (!confirm) return;

            if (await _projectSvc.DeleteProject(_current.ProjectID))
            {
                await DisplayAlert("Success", "Usunięto.", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await DisplayAlert("Error", "Usuwanie nie powiodło się.", "OK");
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
            => await Shell.Current.GoToAsync("..");
    }
}
