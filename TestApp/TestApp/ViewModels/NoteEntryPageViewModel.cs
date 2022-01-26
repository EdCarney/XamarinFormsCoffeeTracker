﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TestApp.Models;
using TestApp.Validation;

namespace TestApp.ViewModels
{
	public class NoteEntryPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<Coffee> CoffeeOptions { get; } = new ObservableCollection<Coffee>();
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public ValidatableObject<Coffee> SelectedCoffee { get; } = new ValidatableObject<Coffee>();
        public ValidatableObject<string> DoseGrams { get; } = new ValidatableObject<string>();
        public ValidatableObject<string> ExtractGrams { get; } = new ValidatableObject<string>();
        public ValidatableObject<string> ExtractTimeSec { get; } = new ValidatableObject<string>();
        public string Text { get; set; }

        public NoteEntryPageViewModel()
        {
            AddValidators(this);
        }
        
        /// <summary>
        /// Factory method to construct instance
        /// </summary>
        /// <param name="note"></param>
        /// <returns>New instance of <c>NoteEntryPageViewModel</c></returns>
        public static async Task<NoteEntryPageViewModel> CreateAsync(Note note)
        {
            var coffee = await App.Database.GetCoffeeAsync(note.CoffeeID);
            var instance = new NoteEntryPageViewModel
            {
                ID = note.ID,
                Date = note.Date,
                Text = note.Text,
            };

            instance.SelectedCoffee.Value = coffee;
            instance.ExtractGrams.Value = note.ExtractGrams;
            instance.ExtractTimeSec.Value = note.ExtractTimeSec;
            instance.DoseGrams.Value = note.DoseGrams;

            return instance;
        }

        private static void AddValidators(NoteEntryPageViewModel instance)
        {
            const double numMin = 0;
            const double numMax = 100;
            
            instance.DoseGrams.Validations.Add(new IsNotNullOrEmptyRule<string>() { ValidationMessage = "Dose is required"});
            instance.DoseGrams.Validations.Add(new IsInNumericRangeRule<string>(numMin, numMax) { ValidationMessage = $"Dose must be between {numMin} and {numMax}"});
           
            instance.ExtractGrams.Validations.Add(new IsNotNullOrEmptyRule<string>() { ValidationMessage = "Extraction Amount is required"});
            instance.ExtractGrams.Validations.Add(new IsInNumericRangeRule<string>(numMin, numMax) { ValidationMessage = $"Extraction Amount must be between {numMin} and {numMax}"});
            
            instance.ExtractTimeSec.Validations.Add(new IsNotNullOrEmptyRule<string>() { ValidationMessage = "Extraction Time is required"});
            instance.ExtractTimeSec.Validations.Add(new IsInNumericRangeRule<string>(numMin, numMax) { ValidationMessage = $"Extraction Time must be between {numMin} and {numMax}"});
            
            instance.SelectedCoffee.Validations.Add(new IsNotNullOrEmptyRule<Coffee>() { ValidationMessage = "Coffee selection is required"});
        }

        /// <summary>
        /// Generates instance of a <c>Note</c> from view model
        /// </summary>
        /// <returns></returns>
        public Note GenerateNote()
        {
            return new Note
            {
                ID = ID,
                Date = Date,
                CoffeeID = SelectedCoffee.Value.ID,
                CoffeeDisplayName = SelectedCoffee.Value.DisplayName,
                DoseGrams = DoseGrams.Value,
                ExtractGrams = ExtractGrams.Value,
                ExtractTimeSec = ExtractTimeSec.Value,
                Text = Text
            };
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool FieldsAreValid()
        {
            bool isDoseGramsValid = DoseGrams.Validate();
            bool isExtractGramsValid = ExtractGrams.Validate();
            bool isExtractTimeSecValid = ExtractTimeSec.Validate();
            bool isSelectedCoffeeValid = SelectedCoffee.Validate();
            
            return isDoseGramsValid && isExtractGramsValid &&
                   isExtractTimeSecValid && isSelectedCoffeeValid;
        }
    }
}

