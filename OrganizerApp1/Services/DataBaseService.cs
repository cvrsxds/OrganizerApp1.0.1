using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using OrganizerApp1.Models;
using Microsoft.Maui.Controls.PlatformConfiguration;

namespace OrganizerApp1.Services
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseService()
        {
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "events.db3");
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<EventReport>().Wait();
            _database.CreateTableAsync<Event>().Wait();
        }

        public Task<List<Event>> GetEventsAsync()
        {
            return _database.Table<Event>().ToListAsync(); 
        }

        public Task<int> SaveEventAsync(Event evt)
        {
            if (evt.Id == 0)
            {
                return _database.InsertAsync(evt);
            }
            else
            {
                return _database.UpdateAsync(evt);
            }
        }

        public Task<int> DeleteEventAsync(Event evt)
        {
            return _database.DeleteAsync(evt);
        }

        // Метод для получения мероприятий с временем до текущего момента
        public Task<List<Event>> GetPastEventsAsync()
        {
            var currentDateTime = DateTime.Now;
            return _database.Table<Event>()
                            .Where(evt => evt.Date < currentDateTime) 
                            .ToListAsync();
        }

        // Сохранить отчёт
        public Task<int> SaveReportAsync(EventReport report)
        {
            if (report.Id == 0)
            {
                return _database.InsertAsync(report);
            }
            else
            {
                return _database.UpdateAsync(report);
            }
        }

        // Получить все отчёты
        public async Task<List<EventReport>> GetReportsAsync()
        {
            try
            {
                return await _database.Table<EventReport>().ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при получении отчетов: {ex.Message}");
                return new List<EventReport>();
            }
        }

        // Получить отчёт по ID
        public Task<EventReport> GetReportByIdAsync(int reportId)
        {
            return _database.FindAsync<EventReport>(reportId);
        }

        public Task<int> DeleteReportAsync(EventReport report)
        {
            return _database.DeleteAsync(report);
        }

    }
}