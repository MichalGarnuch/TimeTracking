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
    public partial class ProjectTypesPage : ContentPage
    {
        private readonly ProjectTypeService _service = new ProjectTypeService();

        public ObservableCollection<ProjectTypeModel> ProjectTypes { get; }
            = new ObservableCollection<ProjectTypeModel>();

        public ProjectTypesPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (ProjectTypes.Count == 0)
                await LoadTypesAsync();
        }

        private async void OnRefreshClicked(object sender, EventArgs e)
            => await LoadTypesAsync();

        private async void OnAddClicked(object sender, EventArgs e)
            => await Shell.Current.GoToAsync(nameof(ProjectTypeAddPage));

        private async Task LoadTypesAsync()
        {
            ProjectTypes.Clear();
            try
            {
                var list = await _service.GetAllProjectTypes();
                foreach (var t in list)
                    ProjectTypes.Add(t);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count == 0) return;
            var t = e.CurrentSelection[0] as ProjectTypeModel;
            ((CollectionView)sender).SelectedItem = null;
            if (t == null) return;

            string action = await DisplayActionSheet(
                t.TypeName,
                "Cancel", null,
                "Edit", "Delete");

            switch (action)
            {
                case "Edit":
                    await Shell.Current.GoToAsync(
                        $"{nameof(ProjectTypeEditPage)}?typeId={t.ProjectTypeID}");
                    break;

                case "Delete":
                    bool confirm = await DisplayAlert(
                        "Confirm",
                        $"Delete '{t.TypeName}'?",
                        "Yes", "No");
                    if (confirm)
                    {
                        bool ok = await _service.DeleteProjectType(t.ProjectTypeID);
                        if (ok)
                        {
                            await DisplayAlert("OK", "Deleted", "OK");
                            await LoadTypesAsync();
                        }
                        else
                        {
                            await DisplayAlert("Error", "Delete failed", "OK");
                        }
                    }
                    break;
            }
        }
    }
}
