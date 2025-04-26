using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TimeTrackingMobile.Models;
using TimeTrackingMobile.Services;

namespace TimeTrackingMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty(nameof(TagId), "tagId")]
    public partial class TagEditPage : ContentPage
    {
        private readonly TagService _service = new TagService();
        private TagModel _current;

        public int TagId
        {
            get => _id;
            set { _id = value; LoadTag(value); }
        }
        private int _id;

        public TagEditPage()
        {
            InitializeComponent();
        }

        private async void LoadTag(int id)
        {
            try
            {
                _current = await _service.GetTag(id);
                if (_current == null) throw new Exception("Not found");
                NameEntry.Text = _current.TagName;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
                await Shell.Current.GoToAsync("..");
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            var name = NameEntry.Text?.Trim();
            if (string.IsNullOrEmpty(name))
            {
                await DisplayAlert("Validation", "Name required.", "OK");
                return;
            }

            _current.TagName = name;
            bool ok = await _service.UpdateTag(_current.TagID, _current);
            if (ok)
            {
                await DisplayAlert("Success", "Saved", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await DisplayAlert("Error", "Update failed", "OK");
            }
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert("Confirm", "Delete this tag?", "Yes", "No");
            if (!confirm) return;

            bool ok = await _service.DeleteTag(_current.TagID);
            if (ok)
            {
                await DisplayAlert("Success", "Deleted", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await DisplayAlert("Error", "Delete failed", "OK");
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
            => await Shell.Current.GoToAsync("..");
    }
}
