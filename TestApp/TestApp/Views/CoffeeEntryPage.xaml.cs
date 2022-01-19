using System;
using System.Collections.Generic;
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
            var coffee = (Coffee)BindingContext;
            await App.Database.SaveCoffeeAsync(coffee);
            //if (CoffeeIsValid(CurrentCoffee))
            //{
            //    await App.Database.SaveCoffeeAsync(CurrentCoffee);
            //}
            await NavigateBack();
        }

        private bool CoffeeIsValid(Coffee currentCoffee)
        {
            if (!string.IsNullOrWhiteSpace(currentCoffee.Company) &&
                !string.IsNullOrWhiteSpace(currentCoffee.Name) &&
                !string.IsNullOrWhiteSpace(currentCoffee.RoastStyle))
            {
                return true;
            }
            return false;
        }

        public async void OnDeleteButtonClicked(object sender, EventArgs e)
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

