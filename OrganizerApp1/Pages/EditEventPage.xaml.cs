using Microsoft.Maui.Controls;
using OrganizerApp1.Models;
using System;

namespace OrganizerApp1.Pages
{
    public partial class EditEventPage : ContentPage
    {
        public event EventHandler<Event> EventUpdated;
        private Event _eventToEdit;

        public EditEventPage(Event selectedEvent)
        {
            InitializeComponent();

            _eventToEdit = selectedEvent;
            EventNameEntry.Text = selectedEvent.Name;
            EventDatePicker.Date = selectedEvent.Date;
            EventTimePicker.Time = selectedEvent.Date.TimeOfDay;
            EventDescriptionEditor.Text = selectedEvent.Description; 
        }

        private async void OnSaveChangesClicked(object sender, EventArgs e)
        {
            string eventName = EventNameEntry.Text;
            DateTime eventDate = EventDatePicker.Date;
            TimeSpan eventTime = EventTimePicker.Time;
            string eventDescription = EventDescriptionEditor.Text;

            if (!string.IsNullOrEmpty(eventName))
            {
                _eventToEdit.Name = eventName;
                _eventToEdit.Date = eventDate.Add(eventTime);
                _eventToEdit.Description = eventDescription;

                EventUpdated?.Invoke(this, _eventToEdit);

                await DisplayAlert("Сохранено", $"Изменения для мероприятия '{eventName}' сохранены.", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Ошибка", "Пожалуйста, введите название мероприятия.", "OK");
            }
        }
    }
}