using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TimeTrackingMobile.Models;
using TimeTrackingMobile.Services;

namespace TimeTrackingMobile.Views.TaskTags
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TaskTagAddPage : ContentPage
    {
        private readonly TaskTagService _ttSvc = new TaskTagService();
        private readonly TaskService _tSvc = new TaskService();
        private readonly TagService _tagSvc = new TagService();

        private List<TaskModel> _tasks;
        private List<TagModel> _tags;

        public TaskTagAddPage()
        {
            InitializeComponent();
            LoadPickers();
        }

        private async void LoadPickers()
        {
            _tasks = await _tSvc.GetAllTasks();
            TaskPicker.ItemsSource = _tasks;

            _tags = await _tagSvc.GetAllTags();
            TagPicker.ItemsSource = _tags;
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            if (!(TaskPicker.SelectedItem is TaskModel task) ||
                !(TagPicker.SelectedItem is TagModel tag))
            {
                await DisplayAlert("Validation", "Select both Task and Tag.", "OK");
                return;
            }

            var model = new TaskTagModel
            {
                TaskID = task.TaskID,
                TagID = tag.TagID
            };

            if (await _ttSvc.CreateTaskTag(model))
            {
                await DisplayAlert("Success", "Link created.", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await DisplayAlert("Error", "Failed to create link.", "OK");
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
            => await Shell.Current.GoToAsync("..");
    }
}
