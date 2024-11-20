using OrganizerApp1.Services;
using Microsoft.Maui.Controls;
using OrganizerApp1.Models;
using System;

namespace OrganizerApp1.Pages
{
    public partial class ContactsPage : ContentPage
    {
        // �������� ��� ������� � ViewModel
        public ContactsViewModel ViewModel { get; set; }

        public ContactsPage()
        {
            InitializeComponent();
            ViewModel = new ContactsViewModel();
            BindingContext = ViewModel;
        }

        // ��������� �������� ��� ������������� ��������
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await ViewModel.LoadContactsAsync(); 
        }

        private async void OnAddContactClicked(object sender, EventArgs e)
        {
            var addContactPage = new AddContactPage();
            addContactPage.ContactSaved += async (s, newContact) =>
            {
                await ViewModel.AddContactAsync(newContact);
            };

            await Navigation.PushAsync(addContactPage);
        }

        private async void OnEditContactClicked(object sender, EventArgs e)
        {
            var selectedContact = ContactsListView.SelectedItem as AppContact;

            if (selectedContact != null)
            {
                var editContactPage = new EditContactPage(selectedContact);
                editContactPage.ContactUpdated += async (s, updatedContact) =>
                {
                    await ViewModel.UpdateContactAsync(updatedContact); 
                };

                await Navigation.PushAsync(editContactPage);
            }
            else
            {
                await DisplayAlert("������", "����������, �������� ������� ��� ���������.", "OK");
            }
        }

        private async void OnDeleteContactClicked(object sender, EventArgs e)
        {
            var selectedContact = ContactsListView.SelectedItem as AppContact;

            if (selectedContact != null)
            {
                bool confirm = await DisplayAlert("������������� ��������", $"�� �������, ��� ������ ������� ������� '{selectedContact.Name}'?", "��", "���");
                if (confirm)
                {
                    var databaseService = new ContactsDatabaseService();
                    int result = await databaseService.DeleteContactAsync(selectedContact); 

                    if (result > 0)
                    {
                        await ViewModel.LoadContactsAsync();
                        ContactsListView.SelectedItem = null;
                    }
                    else
                    {
                        await DisplayAlert("������", "�� ������� ������� �������.", "OK");
                    }
                }
            }
            else
            {
                await DisplayAlert("������", "����������, �������� ������� ��� ��������.", "OK");
            }
        }
    }
}