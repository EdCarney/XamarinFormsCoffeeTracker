using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BeanCounter.Models;
using Xamarin.Forms;

namespace BeanCounter.Views
{	
	public partial class NotesPage : ContentPage
	{
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

