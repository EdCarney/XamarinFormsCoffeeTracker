using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using TestApp.Models;

namespace TestApp.Data
{
	public class NoteDatabase
	{
		private readonly SQLiteAsyncConnection database;

		public NoteDatabase(string dbPath)
		{
			database = new SQLiteAsyncConnection(dbPath);
			database.CreateTableAsync<Note>().Wait();
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
	}
}

