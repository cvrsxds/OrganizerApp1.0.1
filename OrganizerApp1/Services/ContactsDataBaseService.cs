using SQLite;
using OrganizerApp1.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace OrganizerApp1.Services
{
    public class ContactsDatabaseService
    {
        private readonly SQLiteAsyncConnection _database;

        public ContactsDatabaseService()
        {
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "contacts.db3");
            _database = new SQLiteAsyncConnection(dbPath);

            _database.CreateTableAsync<AppContact>().Wait();
        }

        public Task<List<AppContact>> GetContactsAsync()
        {
            return _database.Table<AppContact>().ToListAsync();
        }

        public Task<int> SaveContactAsync(AppContact contact)
        {
            if (contact.Id == 0)
            {
                return _database.InsertAsync(contact);
            }
            else
            {
                return _database.UpdateAsync(contact);
            }
        }

        public Task<int> DeleteContactAsync(AppContact contact)
        {
            return _database.DeleteAsync(contact);
        }
    }
}