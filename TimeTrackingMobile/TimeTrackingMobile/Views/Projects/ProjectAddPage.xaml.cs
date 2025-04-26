using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TimeTrackingMobile.Models;
using TimeTrackingMobile.Services;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace TimeTrackingMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProjectAddPage : ContentPage
    {
        private readonly ProjectService _projectSvc = new ProjectService();
        private readonly ProjectTypeService _typeSvc = new ProjectTypeService();
        private List<ProjectTypeModel> _types;

        public ProjectAddPage()
        {
            InitializeComponent();
            LoadTypes();
        }

        private async void LoadTypes()
        {
            _types = await _typeSvc.GetAllProjectTypes();
            TypePicker.ItemsSource = _types;
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

            var proj = new ProjectModel
            {
                ProjectName = NameEntry.Text.Trim(),
                ProjectTypeID = selType.ProjectTypeID,
                StartDate = StartPicker.Date,
                EndDate = EndPicker.Date,
                Budget = budget
            };

            bool ok = await _projectSvc.CreateProject(proj);
            if (ok)
            {
                await DisplayAlert("Success", "Projekt dodany.", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await DisplayAlert("Error", "Dodanie nie powiodło się.", "OK");
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
            => await Shell.Current.GoToAsync("..");
    }
}
