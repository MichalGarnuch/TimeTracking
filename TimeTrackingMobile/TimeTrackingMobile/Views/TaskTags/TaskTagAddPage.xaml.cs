using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;
using TimeTrackingMobile.Models;
using TimeTrackingMobile.Services;

namespace TimeTrackingMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TaskTagAddPage : ContentPage
    {
        private readonly TaskTagService _service;

        public TaskTagAddPage()
        {
            InitializeComponent();
            _service = new TaskTagService();
        }

        private async void SaveButtonClicked(object sender, EventArgs e)
        {
            int taskId = 0;
            int.TryParse(TaskIdEntry.Text, out taskId);

            int tagId = 0;
            int.TryParse(TagIdEntry.Text, out tagId);

            if (taskId <= 0 || tagId <= 0)
            {
                await DisplayAlert("Validation", "TaskID and TagID must be > 0", "OK");
                return;
            }

            var newLink = new TaskTagModel
            {
                TaskID = taskId,
                TagID = tagId
            };

            bool success = await _service.CreateTaskTag(newLink);
            if (success)
            {
                await DisplayAlert("OK", "Link created", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await DisplayAlert("Error", "Create failed", "OK");
            }
        }

        private async void CancelButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
