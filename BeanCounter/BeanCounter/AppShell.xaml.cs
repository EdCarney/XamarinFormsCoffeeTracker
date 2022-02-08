using System;
using System.Collections.Generic;
using BeanCounter.Views;
using Xamarin.Forms;

namespace BeanCounter
{	
	public partial class AppShell : Shell
	{	
		public AppShell()
		{
			InitializeComponent();
			Routing.RegisterRoute(nameof(NoteEntryPage), typeof(NoteEntryPage));
			Routing.RegisterRoute(nameof(CoffeeEntryPage), typeof(CoffeeEntryPage));
			Routing.RegisterRoute(nameof(ToolsPage), typeof(ToolsPage));
		}
	}
}

