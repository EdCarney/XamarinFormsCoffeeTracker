using System;
using SQLite;

namespace TestApp.Models
{
	public class Note
	{
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public DateTime Date { get; set; }

        public double DoseGrams { get; set; }
        public double ExtractGrams { get; set; }
        public double ExtractTimeSec { get; set; }
        public string Coffee { get; set; }
        public string Text { get; set; }
	}
}