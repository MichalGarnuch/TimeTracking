using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;
using TimeTrackingMobile.Models;
using TimeTrackingMobile.Services;

namespace TimeTrackingMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty(nameof(TagId), "tagId")]
    public partial class TagEditPage : ContentPage
    {
        private readonly TagService _tagService;
        private TagModel currentTag;

        private int _tagId;
        public int TagId
        {
            get => _tagId;
            set
            {
                _tagId = value;
                _ = LoadTag();
            }
        }

        public TagEditPage()
        {
            InitializeComponent();
            _tagService = new TagService();
        }

        private async System.Threading.Tasks.Task LoadTag()
        {
            try
            {
                currentTag = await _tagService.GetTag(_tagId);
                if (currentTag == null)
                {
                    await DisplayAlert("Error", "Not found", "OK");
                    await Shell.Current.GoToAsync("..");
                    return;
                }
                TagNameEntry.Text = currentTag.TagName;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
                await Shell.Current.GoToAsync("..");
            }
        }

        private async void SaveButtonClicked(object sender, EventArgs e)
        {
            if (currentTag == null) return;
            var newName = TagNameEntry.Text;
            if (string.IsNullOrWhiteSpace(newName))
            {
                await DisplayAlert("Validation", "Tag name cannot be empty", "OK");
                return;
            }
            currentTag.TagName = newName;
            bool updated = await _tagService.UpdateTag(_tagId, currentTag);
            if (updated)
            {
                await DisplayAlert("Success", "Tag updated", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await DisplayAlert("Error", "Update failed", "OK");
            }
        }

        private async void CancelButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
