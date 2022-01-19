using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TestApp.Models;
using Xamarin.Forms;

namespace TestApp.Views
{	
	public partial class NotesPage : ContentPage
	{
		private readonly string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "notes.txt");

		public NotesPage()
		{
			InitializeComponent();
		}

        protected override async void OnAppearing()
        {
			base.OnAppearing();
			collectionView.ItemsSource = await App.Database.GetNotesAsync();
        }

		public async void OnAddClicked(object sender, EventArgs e)
        {
			await Shell.Current.GoToAsync(nameof(NoteEntryPage));
        }

		public async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
			if (e.CurrentSelection is null)
            {
				return;
            }

			Note note = (Note)e.CurrentSelection.FirstOrDefault();
			await Shell.Current.GoToAsync($"{nameof(NoteEntryPage)}?{nameof(NoteEntryPage.ItemId)}={note.ID}");
        }
    }
}

