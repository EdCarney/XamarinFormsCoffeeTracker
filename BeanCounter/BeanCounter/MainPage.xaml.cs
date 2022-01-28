using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BeanCounter
{
    public partial class MainPage : ContentPage
    {
        private int Count { get; set; }

        public MainPage()
        {
            InitializeComponent();
        }

        void Handle_Clicked(System.Object sender, System.EventArgs e)
        {
            Count++;
            ((Button)sender).Text = $"You clicked the button {Count} times!";
        }
    }
}

