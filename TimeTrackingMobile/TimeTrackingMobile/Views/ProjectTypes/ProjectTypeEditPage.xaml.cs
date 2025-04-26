using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TimeTrackingMobile.Models;
using TimeTrackingMobile.Services;

namespace TimeTrackingMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty(nameof(TypeId), "typeId")]
    public partial class ProjectTypeEditPage : ContentPage
    {
        private readonly ProjectTypeService _service = new ProjectTypeService();
        private ProjectTypeModel _current;

        public int TypeId
        {
            get => _id;
            set { _id = value; LoadType(value); }
        }
        private int _id;

        public ProjectTypeEditPage()
        {
            InitializeComponent();
        }

        private async void LoadType(int id)
        {
            try
            {
                _current = await _service.GetProjectType(id);
                if (_current == null) throw new Exception("Not found");
                TypeNameEntry.Text = _current.TypeName;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
                await Shell.Current.GoToAsync("..");
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            var name = TypeNameEntry.Text?.Trim();
            if (string.IsNullOrEmpty(name))
            {
                await DisplayAlert("Validation", "Name required", "OK");
                return;
            }

            _current.TypeName = name;
            bool ok = await _service.UpdateProjectType(_current.ProjectTypeID, _current);
            if (ok)
            {
                await DisplayAlert("Success", "Saved", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await DisplayAlert("Error", "Update failed", "OK");
            }
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert("Confirm", "Delete this type?", "Yes", "No");
            if (!confirm) return;

            bool ok = await _service.DeleteProjectType(_current.ProjectTypeID);
            if (ok)
            {
                await DisplayAlert("Success", "Deleted", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await DisplayAlert("Error", "Delete failed", "OK");
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
            => await Shell.Current.GoToAsync("..");
    }
}
