using Microsoft.Maui.Controls;
using OrganizerApp1.Models;
using OrganizerApp1.Pages;
using OrganizerApp1.Services;
using System;
using System.Collections.ObjectModel;

namespace OrganizerApp1
{
    public partial class MainPage : ContentPage
    {
        public MainPageViewModel ViewModel { get; set; }

        public MainPage()
        {
            InitializeComponent();
            ViewModel = new MainPageViewModel();
            BindingContext = ViewModel;
        }

        private async void OnAddEventClicked(object sender, EventArgs e)
        {
            var addEventPage = new AddEventPage();
            addEventPage.EventSaved += async (s, newEvent) =>
            {
                // Добавляем новое мероприятие в базу данных и обновляем список
                await ((MainPageViewModel)BindingContext).AddEventAsync(newEvent);
            };
            await Navigation.PushAsync(addEventPage);
        }

        private async void OnEditEventClicked(object sender, EventArgs e)
        {
            if (((MainPageViewModel)BindingContext).SelectedEvent != null)
            {
                var editEventPage = new EditEventPage(((MainPageViewModel)BindingContext).SelectedEvent);
                editEventPage.EventUpdated += async (s, updatedEvent) =>
                {
                    // Обновляем мероприятие в базе данных
                    await ((MainPageViewModel)BindingContext).UpdateEventAsync(updatedEvent);
                };
                await Navigation.PushAsync(editEventPage);
            }
            else
            {
                await DisplayAlert("Ошибка", "Выберите мероприятие для редактирования.", "OK");
            }
        }

        private async void OnDeleteEventClicked(object sender, EventArgs e)
        {
            if (ViewModel.SelectedEvent != null)
            {
                bool isConfirmed = await DisplayAlert("Подтверждение удаления", "Вы уверены, что хотите удалить это мероприятие?", "Да", "Нет");

                if (isConfirmed)
                {
                    await ViewModel.DeleteEventAsync(ViewModel.SelectedEvent);
                    EventsListView.SelectedItem = null;
                }
            }
            else
            {
                await DisplayAlert("Ошибка", "Выберите мероприятие для удаления.", "OK");
            }
        }

        private async void OnContactsPageClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ContactsPage());
        }

        private async void OnExportReportClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ExportReportPage());
        }
    }
}