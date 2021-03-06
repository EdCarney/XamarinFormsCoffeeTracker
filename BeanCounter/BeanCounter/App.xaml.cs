using System;
using System.IO;
using BeanCounter.Data;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BeanCounter
{
    public partial class App : Application
    {
        private static NoteDatabase _database;

        public static NoteDatabase Database
        {
            get
            {
                if (_database == null)
                {
                    _database = new NoteDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Notes.db3"));
                }
                return _database;
            }
        }

        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }

        protected override void OnStart ()
        {
        }

        protected override void OnSleep ()
        {
        }

        protected override void OnResume ()
        {
        }
    }
}

