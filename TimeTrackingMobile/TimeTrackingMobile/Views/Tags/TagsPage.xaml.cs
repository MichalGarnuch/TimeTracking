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
    public partial class TagsPage : ContentPage
    {
        private readonly TagService _service = new TagService();

        public ObservableCollection<TagModel> Tags { get; }
            = new ObservableCollection<TagModel>();

        public TagsPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (Tags.Count == 0)
                await LoadTagsAsync();
        }

        private async void OnRefreshClicked(object sender, EventArgs e)
            => await LoadTagsAsync();

        private async void OnAddClicked(object sender, EventArgs e)
            => await Shell.Current.GoToAsync(nameof(TagAddPage));

        private async Task LoadTagsAsync()
        {
            Tags.Clear();
            try
            {
                var list = await _service.GetAllTags();
                foreach (var t in list)
                    Tags.Add(t);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count == 0) return;
            var tag = e.CurrentSelection[0] as TagModel;
            ((CollectionView)sender).SelectedItem = null;
            if (tag == null) return;

            string action = await DisplayActionSheet(
                tag.TagName,
                "Cancel", null,
                "Edit", "Delete");

            switch (action)
            {
                case "Edit":
                    await Shell.Current.GoToAsync(
                        $"{nameof(TagEditPage)}?tagId={tag.TagID}");
                    break;
                case "Delete":
                    bool confirm = await DisplayAlert(
                        "Confirm", $"Delete '{tag.TagName}'?", "Yes", "No");
                    if (confirm)
                    {
                        bool ok = await _service.DeleteTag(tag.TagID);
                        if (ok)
                        {
                            await DisplayAlert("OK", "Deleted", "OK");
                            await LoadTagsAsync();
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
