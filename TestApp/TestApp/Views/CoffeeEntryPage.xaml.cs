using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApp.Models;
using TestApp.Validation;
using TestApp.ViewModels;
using Xamarin.Forms;

namespace TestApp.Views
{
	[QueryProperty(nameof(ItemId), nameof(ItemId))]
	public partial class CoffeeEntryPage : ContentPage
	{
		public string ItemId { get; set; }

        private CoffeeEntryPageViewModel ViewModel { get; set; }
        
        private Dictionary<Label, IValidatable> LabelValidationObjectMap { get; set; }

        public CoffeeEntryPage()
        {
            InitializeComponent();
            BindingContext = new Coffee();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            SetCancelDeleteBtn();
            await SetBindingContext();
            CreateLabelValidationObjectMap();
            ResetErrorLabels();
        }

        private void SetCancelDeleteBtn()
        {
            CancelDeleteBtn.Text = ItemId is null ? "Cancel" : "Delete";
        }

        private async Task SetBindingContext()
        {
            if (ItemId is null)
            {
                SetViewModelForNewCoffee();
            }
            else
            {
                await SetViewModelFromExistingCoffee(ItemId);
            }

            BindingContext = ViewModel;
        }

        private void ResetErrorLabels()
        {
            foreach (var label in LabelValidationObjectMap.Keys)
            {
                label.IsVisible = false;
            }
        }

        private void SetViewModelForNewCoffee()
        {
            ViewModel = new CoffeeEntryPageViewModel();
        }

        private async Task SetViewModelFromExistingCoffee(string itemId)
        {
            try
            {
                int id = Convert.ToInt32(itemId);
                var coffee = await App.Database.GetCoffeeAsync(id);
                ViewModel = CoffeeEntryPageViewModel.Create(coffee);
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to load coffee");
            }
        }

        private void CreateLabelValidationObjectMap()
        {
            LabelValidationObjectMap = new Dictionary<Label, IValidatable>
            {
                { CompanyErrMsg, ViewModel.Company },
                { NameErrMsg, ViewModel.Name },
                { RoastStyleErrMsg, ViewModel.RoastStyle },
                { RoastDateErrMsg, ViewModel.RoastDate }
            };
        }

        private async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            ResetErrorLabels();

            if (ViewModel.FieldsAreValid())
            {
                var coffee = ((CoffeeEntryPageViewModel)BindingContext).GenerateCoffee();
                await App.Database.SaveCoffeeAsync(coffee);
                await NavigateBack();
            }
            else
            {
                SetErrorLabels();
            }
        }

        private void SetErrorLabels()
        {
            var invalidLabelValidations = LabelValidationObjectMap
                .Where(kvp => !kvp.Value.IsValid);

            foreach (var kvp in invalidLabelValidations)
            {
                kvp.Key.IsVisible = true;
                kvp.Key.Text = kvp.Value.Errors?.FirstOrDefault();
            }
        }

        private async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            var notes = await App.Database.GetNotesAsync();
            int coffeeId = ((CoffeeEntryPageViewModel) BindingContext).ID;
            var notesWithCurrentCoffee = notes.Where(n => n.CoffeeID == coffeeId).ToList();
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
            var coffee = ((CoffeeEntryPageViewModel)BindingContext).GenerateCoffee();
            await App.Database.DeleteCoffeeAsync(coffee);
            await NavigateBack();
        }

        private async Task NavigateBack()
        {
            await Shell.Current.GoToAsync("..");
        }
	}
}

