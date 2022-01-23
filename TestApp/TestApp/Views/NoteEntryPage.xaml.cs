using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestApp.Models;
using TestApp.ViewModels;
using Xamarin.Forms;

namespace TestApp.Views
{
	[QueryProperty(nameof(ItemId), nameof(ItemId))]
	public partial class NoteEntryPage : ContentPage
    {
        public string ItemId { get; set; }

        public NoteEntryPageViewModel ViewModel { get; set; }

        public NoteEntryPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if (ItemId is null)
            {
                SetViewModelForNewNote();
            }
            else
            {
                await SetViewModelFromExistingNote(ItemId);
            }

            BindingContext = ViewModel;
            await PopulateCoffeeOptions();

            if (ItemId != null)
            {
                await SetSelectedCoffee();
            }
        }

        private async Task SetSelectedCoffee()
        {
            int itemId = Convert.ToInt32(ItemId);
            var note = await App.Database.GetNoteAsync(itemId);
            int coffeeId = note.CoffeeID;
            for (int i = 0; i < ViewModel.CoffeeOptions.Count; i++)
            {
                if (ViewModel.CoffeeOptions[i].ID == coffeeId)
                {
                    coffeePicker.SelectedIndex = i;
                }
            }
        }

        private void SetViewModelForNewNote()
        {
            ViewModel = new NoteEntryPageViewModel();
        }

        private async Task SetViewModelFromExistingNote(string itemId)
        {
            try
            {
                int id = Convert.ToInt32(itemId);
                var note = await App.Database.GetNoteAsync(id);
                ViewModel = await NoteEntryPageViewModel.CreateAsync(note);
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to load note");
            }
        }

        private async Task PopulateCoffeeOptions()
        {
            if (ViewModel?.CoffeeOptions?.Any() ?? false)
            {
                return;
            }

            var coffeeOptions = await App.Database.GetCoffeesAsync();
            var coffeeOptionStrings = coffeeOptions
                .OrderBy(c => c.Company)
                .ToList();
            foreach (var opt in coffeeOptionStrings)
            {
                ViewModel.CoffeeOptions.Add(opt);
            }
        }


        private async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            var note = ((NoteEntryPageViewModel)BindingContext).GenerateNote();

            if (!string.IsNullOrWhiteSpace(note.Text))
            {
                note.Date = DateTime.Now;
                await App.Database.SaveNoteAsync(note);
            }

            await Shell.Current.GoToAsync("..");
        }

        public async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            var note = ((NoteEntryPageViewModel)BindingContext).GenerateNote();
            await App.Database.DeleteNoteAsync(note);

            await Shell.Current.GoToAsync("..");
        }
	}
}

