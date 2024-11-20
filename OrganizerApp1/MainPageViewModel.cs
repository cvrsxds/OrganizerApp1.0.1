using OrganizerApp1.Models;
using OrganizerApp1.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;

namespace OrganizerApp1
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private readonly DatabaseService _databaseService;

        private Event _selectedEvent;

        public ObservableCollection<Event> Events { get; set; }

        public Event SelectedEvent
        {
            get => _selectedEvent;
            set
            {
                if (_selectedEvent != value)
                {
                    _selectedEvent = value;
                    OnPropertyChanged(nameof(SelectedEvent));
                }
            }
        }

        public MainPageViewModel()
        {
            _databaseService = new DatabaseService();
            Events = new ObservableCollection<Event>();
            LoadEventsAsync();
        }

        // Метод для загрузки всех мероприятий
        public async Task LoadEventsAsync()
        {
            var eventsFromDb = await _databaseService.GetEventsAsync();
            var sortedEvents = eventsFromDb.OrderBy(evt => evt.Date).ToList();

            Events.Clear();
            foreach (var evt in sortedEvents)
            {
                Events.Add(evt);
            }
        }

        // Метод для добавления нового мероприятия
        public async Task AddEventAsync(Event newEvent)
        {
            await _databaseService.SaveEventAsync(newEvent);  
           
            var sortedEvents = Events.Concat(new[] { newEvent }).OrderBy(evt => evt.Date).ToList();

            Events.Clear();  
            foreach (var evt in sortedEvents)
            {
                Events.Add(evt);  
            }
        }

        // Метод для обновления мероприятия
        public async Task UpdateEventAsync(Event updatedEvent)
        {
            await _databaseService.SaveEventAsync(updatedEvent); 

            int index = Events.IndexOf(updatedEvent);
            if (index >= 0)
            {
                Events[index] = updatedEvent; 
            }

            var sortedEvents = Events.OrderBy(evt => evt.Date).ToList();
            foreach (var evt in sortedEvents)
            {
                Events.Add(evt);
            }
        }

        // Метод для удаления мероприятия
        public async Task DeleteEventAsync(Event eventToDelete)
        {
            await _databaseService.DeleteEventAsync(eventToDelete); 
            Events.Remove(eventToDelete); 
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // Метод для уведомления об изменениях
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}