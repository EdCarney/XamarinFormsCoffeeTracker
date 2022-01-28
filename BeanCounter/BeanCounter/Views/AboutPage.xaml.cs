using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BeanCounter.Views
{	
	public partial class AboutPage : ContentPage
	{	
		public AboutPage ()
		{
			InitializeComponent ();
		}

		public void OnButtonClicked(object sender, EventArgs e)
        {
			Launcher.OpenAsync("https://aka.ms/xamarin-quickstart");
        }
	}
}

