using Xamarin.Forms;
using TimeTrackingMobile.Models;
using TimeTrackingMobile.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace TimeTrackingMobile.Views
{
    public partial class TaskTagsPage : ContentPage
    {
        private readonly TaskTagService _service;
        private List<TaskTagModel> _list;

        public TaskTagsPage()
        {
            InitializeComponent();
            _service = new TaskTagService();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadTaskTags();
        }

        private async void LoadTaskTagsClicked(object sender, EventArgs e)
        {
            await LoadTaskTags();
        }

        private async Task LoadTaskTags()
        {
            try
            {
                _list = await _service.GetAllTaskTags();
                TaskTagsList.ItemsSource = _list;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void AddTaskTagClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(TaskTagAddPage));
        }

        private async void TaskTagsList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is TaskTagModel tt)
            {
                // ActionSheet do ewentualnego Edit / Delete
                var action = await DisplayActionSheet(
                    $"Task={tt.TaskID}, Tag={tt.TagID}",
                    "Cancel",
                    null,
                    "Delete");
                if (action == "Delete")
                {
                    bool confirm = await DisplayAlert(
                        "Confirm",
                        $"Delete link (TaskID={tt.TaskID}, TagID={tt.TagID})?",
                        "Yes", "No");
                    if (confirm)
                    {
                        bool deleted = await _service.DeleteTaskTag(tt.TaskID, tt.TagID);
                        if (deleted)
                        {
                            await DisplayAlert("OK", "Deleted", "OK");
                            await LoadTaskTags();
                        }
                        else
                        {
                            await DisplayAlert("Error", "Delete failed", "OK");
                        }
                    }
                }
                // ewentualnie "Edit" => Shell.Current.GoToAsync($"{nameof(TaskTagEditPage)}?taskId={tt.TaskID}&tagId={tt.TagID}");
                TaskTagsList.SelectedItem = null;
            }
        }
    }
}
