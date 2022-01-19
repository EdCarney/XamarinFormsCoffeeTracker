using System;
using System.Collections.Generic;
using System.IO;
using TestApp.Models;
using Xamarin.Forms;

namespace TestApp.Views
{
	[QueryProperty(nameof(ItemId), nameof(ItemId))]
	public partial class NoteEntryPage : ContentPage
	{
        public string ItemId
        {
            set
            {
                LoadNote(value);
            }
        }

        public NoteEntryPage()
		{
			InitializeComponent();
            BindingContext = new Note();
		}

		private async void LoadNote(string itemId)
        {
            try
            {
                int id = Convert.ToInt32(itemId);
                var note = await App.Database.GetNoteAsync(id);
                BindingContext = note;
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to load note");
            }
        }

        private async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            var note = (Note)BindingContext;

            if (!string.IsNullOrWhiteSpace(note.Text))
            {
                note.Date = DateTime.UtcNow;
                await App.Database.SaveNoteAsync(note);
            }

            await Shell.Current.GoToAsync("..");
        }

        public async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            var note = (Note)BindingContext;
            await App.Database.DeleteNoteAsync(note);

            await Shell.Current.GoToAsync("..");
        }
	}
}

