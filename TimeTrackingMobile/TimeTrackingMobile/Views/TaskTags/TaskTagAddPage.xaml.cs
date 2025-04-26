using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TimeTrackingMobile.Models;
using TimeTrackingMobile.Services;

namespace TimeTrackingMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TaskTagAddPage : ContentPage
    {
        private readonly TaskTagService _service = new TaskTagService();

        public TaskTagAddPage()
        {
            InitializeComponent();
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            if (!int.TryParse(TaskIdEntry.Text, out int taskId)
             || !int.TryParse(TagIdEntry.Text, out int tagId))
            {
                await DisplayAlert("Validation", "Invalid IDs.", "OK");
                return;
            }

            var model = new TaskTagModel { TaskID = taskId, TagID = tagId };
            bool ok = await _service.CreateTaskTag(model);
            if (ok)
            {
                await DisplayAlert("Success", "Link created", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await DisplayAlert("Error", "Create failed", "OK");
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
            => await Shell.Current.GoToAsync("..");
    }
}
