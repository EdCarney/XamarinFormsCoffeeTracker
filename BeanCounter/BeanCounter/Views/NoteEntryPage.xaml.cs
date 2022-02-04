using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeanCounter.Validation;
using BeanCounter.ViewModels;
using Xamarin.Forms;

namespace BeanCounter.Views
{
	[QueryProperty(nameof(ItemId), nameof(ItemId))]
	public partial class NoteEntryPage : ContentPage
    {
        public string ItemId { get; set; }

        private NoteEntryPageViewModel ViewModel { get; set; }

        private Dictionary<Label, IValidatable> LabelValidationObjectMap { get; set; }

        public NoteEntryPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            SetCancelDeleteBtn();
            await SetBindingContext();
            await PopulateCoffeeOptions(); 
            await SetSelectedCoffee();
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
                SetViewModelForNewNote();
            }
            else
            {
                await SetViewModelFromExistingNote(ItemId);
            }

            BindingContext = ViewModel;
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
                ViewModel?.CoffeeOptions?.Add(opt);
            }
        }

        private async Task SetSelectedCoffee()
        {
            if (ItemId is null)
            {
                return;
            }
            
            int itemId = Convert.ToInt32(ItemId);
            var note = await App.Database.GetNoteAsync(itemId);
            int coffeeId = note.CoffeeID;
            for (int i = 0; i < ViewModel.CoffeeOptions.Count; i++)
            {
                if (ViewModel.CoffeeOptions[i].ID == coffeeId)
                {
                    CoffeePicker.SelectedIndex = i;
                }
            }
        }
        
        private void CreateLabelValidationObjectMap()
        {
            LabelValidationObjectMap = new Dictionary<Label, IValidatable>
            {
                { GrindSizeErrMsg, ViewModel.GrindSize },
                { ExtractGramsErrMsg, ViewModel.ExtractGrams },
                { ExtractTimeErrMsg, ViewModel.ExtractTimeSec },
                { DoseErrMsg, ViewModel.DoseGrams },
                { SelectedCoffeeErrMsg, ViewModel.SelectedCoffee }
            };
        }

        private void ResetErrorLabels()
        {
            foreach (var label in LabelValidationObjectMap.Keys)
            {
                label.IsVisible = false;
            }
        }

        private async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            ResetErrorLabels();

            if (ViewModel.FieldsAreValid())
            {
                var note = ((NoteEntryPageViewModel)BindingContext).GenerateNote();
                note.Date = DateTime.Now;
                await App.Database.SaveNoteAsync(note);
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
            if (ItemId != null)
            {
                var note = ((NoteEntryPageViewModel)BindingContext).GenerateNote();
                await App.Database.DeleteNoteAsync(note);
            }
            await NavigateBack();
        }

        private async Task NavigateBack()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}