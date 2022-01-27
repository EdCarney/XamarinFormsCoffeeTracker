using System;
using System.Collections.Generic;
using SQLite;
using SQLiteNetExtensions;
using SQLiteNetExtensions.Attributes;

namespace TestApp.Models
{
	public class Note
	{
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int CoffeeID { get; set; }
        public string CoffeeDisplayName { get; set; }
        public DateTime Date { get; set; }
        public string GrindSize { get; set; }
        public string DoseGrams { get; set; }
        public string ExtractGrams { get; set; }
        public string ExtractTimeSec { get; set; }
        public string Text { get; set; }
	}
}