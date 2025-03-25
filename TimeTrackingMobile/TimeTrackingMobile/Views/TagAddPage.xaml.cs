using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;
using TimeTrackingMobile.Models;
using TimeTrackingMobile.Services;

namespace TimeTrackingMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TagAddPage : ContentPage
    {
        private readonly TagService _tagService;

        public TagAddPage()
        {
            InitializeComponent();
            _tagService = new TagService();
        }

        private async void SaveButtonClicked(object sender, EventArgs e)
        {
            var name = TagNameEntry.Text;
            if (string.IsNullOrWhiteSpace(name))
            {
                await DisplayAlert("Validation", "Tag name is required", "OK");
                return;
            }

            var newTag = new TagModel
            {
                TagName = name
            };

            bool success = await _tagService.CreateTag(newTag);
            if (success)
            {
                await DisplayAlert("Success", "Tag created", "OK");
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
