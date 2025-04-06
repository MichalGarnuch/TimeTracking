using Xamarin.Forms;
using TimeTrackingMobile.Models;
using TimeTrackingMobile.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace TimeTrackingMobile.Views
{
    public partial class TagsPage : ContentPage
    {
        private readonly TagService _tagService;
        private List<TagModel> _tags;

        public TagsPage()
        {
            InitializeComponent();
            _tagService = new TagService();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadTags();
        }

        private async void LoadTagsClicked(object sender, EventArgs e)
        {
            await LoadTags();
        }

        private async Task LoadTags()
        {
            try
            {
                _tags = await _tagService.GetAllTags();
                TagsList.ItemsSource = _tags;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void AddTagClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(TagAddPage));
        }

        private async void TagsList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is TagModel tag)
            {
                var action = await DisplayActionSheet(
                    $"Selected: {tag.TagName}",
                    "Cancel",
                    null,
                    "Edit",
                    "Delete");

                if (action == "Edit")
                {
                    await Shell.Current.GoToAsync($"{nameof(TagEditPage)}?tagId={tag.TagID}");
                }
                else if (action == "Delete")
                {
                    bool confirm = await DisplayAlert("Confirm", $"Delete '{tag.TagName}'?", "Yes", "No");
                    if (confirm)
                    {
                        bool deleted = await _tagService.DeleteTag(tag.TagID);
                        if (deleted)
                        {
                            await DisplayAlert("OK", "Tag deleted", "OK");
                            await LoadTags();
                        }
                        else
                        {
                            await DisplayAlert("Error", "Delete failed", "OK");
                        }
                    }
                }
                TagsList.SelectedItem = null;
            }
        }
    }
}
