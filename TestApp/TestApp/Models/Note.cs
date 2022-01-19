using System;
using SQLite;

namespace TestApp.Models
{
	public class Note
	{
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public DateTime Date { get; set; }

        public string DoseGrams { get; set; }
        public string ExtractGrams { get; set; }
        public string ExtractTimeSec { get; set; }
        public string Coffee { get; set; }
        public string Text { get; set; }
	}
}