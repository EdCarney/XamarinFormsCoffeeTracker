using System;
using System.Collections.Generic;
using TestApp.Views;
using Xamarin.Forms;

namespace TestApp
{	
	public partial class AppShell : Shell
	{	
		public AppShell()
		{
			InitializeComponent();
			Routing.RegisterRoute(nameof(NoteEntryPage), typeof(NoteEntryPage));
		}
	}
}

