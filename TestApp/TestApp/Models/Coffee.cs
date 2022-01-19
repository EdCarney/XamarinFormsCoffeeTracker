using System;
using SQLite;

namespace TestApp.Models
{
	public class Coffee
	{
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string Company { get; set; }
        public string Name { get; set; }
        public string RoastStyle { get; set; }
        public DateTime? RoastDate { get; set; }
        public string Notes { get; set; }
    }
}

