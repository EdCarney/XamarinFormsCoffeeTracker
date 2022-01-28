using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using BeanCounter.Models;

namespace BeanCounter.Data
{
	public class NoteDatabase
	{
		private readonly SQLiteAsyncConnection database;

		public NoteDatabase(string dbPath)
		{
			database = new SQLiteAsyncConnection(dbPath);
			database.CreateTableAsync<Note>().Wait();
			database.CreateTableAsync<Coffee>().Wait();
		}

		public async Task<List<Note>> GetNotesAsync()
        {
			return await database.Table<Note>()
				.OrderByDescending(n => n.Date)
				.ToListAsync();
        }

		public async Task<Note> GetNoteAsync(int id)
        {
			return await database.Table<Note>()
				.Where(n => n.ID == id)
				.FirstOrDefaultAsync();
        }

		public async Task<int> SaveNoteAsync(Note note)
        {
			if (note.ID != 0)
            {
				return await database.UpdateAsync(note);
            }
            else
            {
				return await database.InsertAsync(note);
            }
        }

		public async Task<int> DeleteNoteAsync(Note note)
        {
			return await database.DeleteAsync(note);
        }

		public async Task<List<Coffee>> GetCoffeesAsync()
		{
			return await database.Table<Coffee>()
				.OrderByDescending(c => c.Company)
				.ToListAsync();
		}

		public async Task<Coffee> GetCoffeeAsync(int id)
		{
			return await database.Table<Coffee>()
				.Where(c => c.ID == id)
				.FirstOrDefaultAsync();
		}

		public async Task<int> SaveCoffeeAsync(Coffee coffee)
		{
			if (coffee.ID != 0)
			{
				return await database.UpdateAsync(coffee);
			}
			else
			{
				return await database.InsertAsync(coffee);
			}
		}

		public async Task<int> DeleteCoffeeAsync(Coffee coffee)
		{
			return await database.DeleteAsync(coffee);
		}
	}
}

