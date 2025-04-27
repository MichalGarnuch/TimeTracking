using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TimeTrackingMobile.Models;
using TimeTrackingMobile.Services;

namespace TimeTrackingMobile.Views.TaskTags
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TaskTagsPage : ContentPage
    {
        private readonly TaskTagService _ttSvc = new TaskTagService();

        public ObservableCollection<TaskTagModel> TaskTags { get; }
            = new ObservableCollection<TaskTagModel>();

        public TaskTagsPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadAsync();
        }

        private async Task LoadAsync()
        {
            TaskTags.Clear();
            try
            {
                var list = await _ttSvc.GetAllTaskTags();
                foreach (var tt in list)
                    TaskTags.Add(tt);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void OnRefreshClicked(object sender, EventArgs e)
            => await LoadAsync();

        private async void OnAddClicked(object sender, EventArgs e)
            => await Shell.Current.GoToAsync(nameof(TaskTagAddPage));

        private async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count == 0) return;
            var model = e.CurrentSelection[0] as TaskTagModel;
            ((CollectionView)sender).SelectedItem = null;
            if (model == null) return;

            bool confirm = await DisplayAlert(
                "Confirm",
                $"Delete link Task {model.TaskID} ↔ Tag {model.TagID}?",
                "Yes", "No");
            if (!confirm) return;

            if (await _ttSvc.DeleteTaskTag(model.TaskID, model.TagID))
                await LoadAsync();
            else
                await DisplayAlert("Error", "Delete failed", "OK");
        }
    }
}
