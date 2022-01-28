using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeanCounter.Models;
using Xamarin.Forms;

namespace BeanCounter.Views
{	
	public partial class CoffeesPage : ContentPage
	{	
		public CoffeesPage()
		{
			InitializeComponent();
		}

        protected override async void OnAppearing()
        {
            base.OnAppearing();
			collectionView.ItemsSource = await App.Database.GetCoffeesAsync();
        }

		public async void OnAddClicked(object sender, EventArgs e)
        {
			await Shell.Current.GoToAsync(nameof(CoffeeEntryPage));
        }

		public async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
			if (e.CurrentSelection == null)
            {
                return;
            }

            Coffee coffee = (Coffee)e.CurrentSelection.FirstOrDefault();
            await Shell.Current.GoToAsync($"{nameof(CoffeeEntryPage)}?{nameof(CoffeeEntryPage.ItemId)}={coffee.ID}");
        }
    }
}

