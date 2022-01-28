using System;
using SQLite;

namespace BeanCounter.Models
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

        public string DisplayName => $"{Company} - {Name}";
    }
}

