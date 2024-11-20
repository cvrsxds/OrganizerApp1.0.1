using SQLite;
using System;
using System.ComponentModel;

namespace OrganizerApp1.Models
{
    public class AppContact : INotifyPropertyChanged
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        // Метод для уведомления об изменениях
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}