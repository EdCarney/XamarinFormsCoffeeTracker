using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApp.Models;
using Xamarin.Forms;

namespace TestApp.Views
{
	[QueryProperty(nameof(ItemId), nameof(ItemId))]
	public partial class CoffeeEntryPage : ContentPage
	{
		public string ItemId
        {
			set => LoadNote(value);
        }

        private Coffee CurrentCoffee => (Coffee)BindingContext;

        public CoffeeEntryPage()
        {
            InitializeComponent();
            BindingContext = new Coffee();
        }

        private async Task LoadNote(string itemId)
        {
            try
            {
                int id = Convert.ToInt32(itemId);
                var coffee = await App.Database.GetCoffeeAsync(id);
                BindingContext = coffee;
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to load coffee");
            }
        }

        public async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            var coffee = CurrentCoffee;
            await App.Database.SaveCoffeeAsync(coffee);
            await NavigateBack();

            //if (CoffeeIsValid(coffee))
            //{
            //    await App.Database.SaveCoffeeAsync(coffee);
            //    await NavigateBack();
            //}
            //else
            //{
            //    await RaiseInvalidCoffeeError(coffee);
            //}
        }

        //private async Task RaiseInvalidCoffeeError(Coffee coffee)
        //{
        //    string message = $"Entry is invalid. Please address the following:";
        //    string errors = GetCoffeeErrorText(coffee);
        //    return await Application.Current.MainPage.DisplayAlert("Warning", message, "OK", "Cancel");
        //}

        //private bool CoffeeIsValid(Coffee coffee)
        //{
        //    if (!string.IsNullOrWhiteSpace(coffee.Company) &&
        //        !string.IsNullOrWhiteSpace(coffee.Name) &&
        //        !string.IsNullOrWhiteSpace(coffee.RoastStyle))
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        //private string GetCoffeeErrorText(Coffee coffee)
        //{
        //    var errorText = new StringBuilder();

        //    if (CurrentCompanyIsInvalid(coffee))
        //    {
        //        errorText.Append("\n")
        //    }
        //}

        public async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            var notes = await App.Database.GetNotesAsync();
            var notesWithCurrentCoffee = notes.Where(n => n.CoffeeID == CurrentCoffee.ID).ToList();
            if (notesWithCurrentCoffee.Any())
            {
                await DeleteCoffeeWithExistingNotes(notesWithCurrentCoffee);
            }
            else
            {
                await DeleteCoffee();
            }
        }

        private async Task DeleteCoffeeWithExistingNotes(IEnumerable<Note> notesWithCurrentCoffee)
        {
            bool haveUserApproval = await RaiseDeletionAlert(notesWithCurrentCoffee);
            if (haveUserApproval)
            {
                await DeleteCoffeeAndRelatedNotes(notesWithCurrentCoffee);
            }
        }

        private async Task<bool> RaiseDeletionAlert(IEnumerable<Note> notes)
        {
            string message = $"Deleting this coffee will delete {notes.Count()} associated note(s). Select OK to continue with deletion.";
            return await Application.Current.MainPage.DisplayAlert("Warning", message, "OK", "Cancel");
        }

        private async Task DeleteCoffeeAndRelatedNotes(IEnumerable<Note> notes)
        {
            foreach (var note in notes)
            {
                await App.Database.DeleteNoteAsync(note);
            }
            await DeleteCoffee();
        }

        private async Task DeleteCoffee()
        {
            await App.Database.DeleteCoffeeAsync(CurrentCoffee);
            await NavigateBack();
        }

        private async Task NavigateBack()
        {
            await Shell.Current.GoToAsync("..");
        }
	}
}

