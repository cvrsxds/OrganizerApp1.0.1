using Microsoft.Maui.Controls;
using OrganizerApp1.Models;
using System;

namespace OrganizerApp1.Pages
{
    public partial class AddEventPage : ContentPage
    {
        public event EventHandler<Event> EventSaved;

        public AddEventPage()
        {
            InitializeComponent();
        }

        private async void OnSaveEventClicked(object sender, EventArgs e)
        {
            string eventName = EventNameEntry.Text;
            DateTime eventDate = EventDatePicker.Date;
            TimeSpan eventTime = EventTimePicker.Time;
            string eventDescription = EventDescriptionEditor.Text;

            if (!string.IsNullOrEmpty(eventName))
            {
                var newEvent = new Event
                {
                    Name = eventName,
                    Date = eventDate.Add(eventTime),
                    Description = eventDescription
                };

                EventSaved?.Invoke(this, newEvent);

                await DisplayAlert("Сохранено", $"Мероприятие '{eventName}' на {newEvent.Date:dd.MM.yyyy HH:mm} сохранено.", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Ошибка", "Пожалуйста, введите название мероприятия.", "OK");
            }
        }
    }
}