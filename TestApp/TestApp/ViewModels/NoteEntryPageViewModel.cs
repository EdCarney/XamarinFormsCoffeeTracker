using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TestApp.Models;
using Xamarin.Forms;

namespace TestApp.ViewModels
{
	public class NoteEntryPageViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Factory method to construct instance
        /// </summary>
        /// <param name="note"></param>
        /// <returns>New instance of <c>NoteEntryPageViewModel</c></returns>
        public static async Task<NoteEntryPageViewModel> CreateAsync(Note note)
        {
            var coffee = await App.Database.GetCoffeeAsync(note.CoffeeID);
            var instance = new NoteEntryPageViewModel()
            {
                ID = note.ID,
                Date = note.Date,
                SelectedCoffee = coffee,
                DoseGrams = note.DoseGrams,
                ExtractGrams = note.ExtractGrams,
                ExtractTimeSec = note.ExtractTimeSec,
                Text = note.Text
            };
            return instance;
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
                CoffeeID = SelectedCoffee.ID,
                CoffeeDisplayName = SelectedCoffee.DisplayName,
                DoseGrams = DoseGrams,
                ExtractGrams = ExtractGrams,
                ExtractTimeSec = ExtractTimeSec,
                Text = Text
            };
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Coffee> CoffeeOptions { get; } = new ObservableCollection<Coffee>();
        public Coffee SelectedCoffee { get; set; }

        public int ID { get; set; }
        public DateTime Date { get; set; }
        public string DoseGrams { get; set; }
        public string ExtractGrams { get; set; }
        public string ExtractTimeSec { get; set; }
        public string Text { get; set; }
    }
}

