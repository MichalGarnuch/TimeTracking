using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TimeTrackingMobile.Models;
using TimeTrackingMobile.Services;

namespace TimeTrackingMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty(nameof(ProjectId), "projectId")]
    public partial class ProjectEditPage : ContentPage
    {
        private readonly ProjectService _service = new ProjectService();
        private ProjectModel _current;

        public int ProjectId
        {
            get => _id;
            set { _id = value; LoadProject(value); }
        }
        private int _id;

        public ProjectEditPage()
        {
            InitializeComponent();
        }

        private async void LoadProject(int id)
        {
            try
            {
                _current = await _service.GetProject(id);
                if (_current == null) throw new Exception("Nie odnaleziono");
                NameEntry.Text = _current.ProjectName;
                TypeIdEntry.Text = _current.ProjectTypeID.ToString();
                StartPicker.Date = _current.StartDate;
                EndPicker.Date = _current.EndDate;
                BudgetEntry.Text = _current.Budget.ToString();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
                await Shell.Current.GoToAsync("..");
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            // walidacja analogicznie do Add
            if (string.IsNullOrWhiteSpace(NameEntry.Text))
            {
                await DisplayAlert("Validation", "Name required.", "OK");
                return;
            }
            if (!int.TryParse(TypeIdEntry.Text, out int typeId))
            {
                await DisplayAlert("Validation", "Invalid Type ID.", "OK");
                return;
            }
            if (!decimal.TryParse(BudgetEntry.Text, out decimal budget))
            {
                await DisplayAlert("Validation", "Invalid budget.", "OK");
                return;
            }

            _current.ProjectName = NameEntry.Text.Trim();
            _current.ProjectTypeID = typeId;
            _current.StartDate = StartPicker.Date;
            _current.EndDate = EndPicker.Date;
            _current.Budget = budget;

            bool ok = await _service.UpdateProject(_current.ProjectID, _current);
            if (ok)
            {
                await DisplayAlert("Success", "Saved.", "OK");
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
                "Confirm", "Delete this project?", "Yes", "No");
            if (!confirm) return;

            bool ok = await _service.DeleteProject(_current.ProjectID);
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
            => await Shell.Current.GoToAsync("..");
    }
}
