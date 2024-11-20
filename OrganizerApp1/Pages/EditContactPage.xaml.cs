using OrganizerApp1.Services;
using Microsoft.Maui.Controls;
using OrganizerApp1.Models;
using System;
using System.Linq;

namespace OrganizerApp1.Pages
{
    public partial class EditContactPage : ContentPage
    {
        public AppContact Contact { get; set; }
        public event EventHandler<AppContact> ContactUpdated;

        public EditContactPage(AppContact contact)
        {
            InitializeComponent();
            Contact = contact;

            NameEntry.Text = Contact.Name;
            PhoneEntry.Text = Contact.PhoneNumber;
            EmailEntry.Text = Contact.Email;
        }

        private async void OnSaveContactClicked(object sender, EventArgs e)
        {
            // �������� ������������ �����
            if (string.IsNullOrWhiteSpace(NameEntry.Text) || string.IsNullOrWhiteSpace(PhoneEntry.Text))
            {
                await DisplayAlert("������", "����������, ��������� ��� � ����� ��������.", "OK");
                return;
            }

            // �������� ����� ������ ��������
            if (PhoneEntry.Text.Length != 13)
            {
                await DisplayAlert("������", "������� ���������� ����� ��������.", "OK");
                return;
            }

            // ���������� ������ ����� ����� � ���������
            if (!string.IsNullOrEmpty(NameEntry.Text))
            {
                Contact.Name = NameEntry.Text;
            }

            // ��������� ������ ��������
            Contact.PhoneNumber = PhoneEntry.Text;
            Contact.Email = EmailEntry.Text;

            // ��������� ���������� ������ � ���� ������
            var databaseService = new ContactsDatabaseService();
            if (Contact.Id == 0)
            {
                await databaseService.SaveContactAsync(Contact);
            }
            else
            {
                await databaseService.SaveContactAsync(Contact);
            }

            // ���������� ������� ���������� ��������
            ContactUpdated?.Invoke(this, Contact);

            // ������������ �� ���������� ��������
            await Navigation.PopAsync();
        }

        private void OnNameEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            var newText = e.NewTextValue;

            // ��������� ������� ������ ����� � �������
            newText = new string(newText.Where(c => char.IsLetter(c) || c == ' ').ToArray());

            // ����������� ������ ����� ���, ����� ������ ����� ���� ���������, � ��������� - ���������
            if (!string.IsNullOrWhiteSpace(newText))
            {
                newText = string.Join(" ", newText.Split(' ')
                                                  .Select(word => word.Length > 0 ? char.ToUpper(word[0]) + word.Substring(1).ToLower() : ""));
            }

            // ��������� ����� ������ ���� �� ��� �������
            if (newText != e.NewTextValue)
            {
                int cursorPosition = NameEntry.CursorPosition;

                NameEntry.Text = newText;

                if (cursorPosition > newText.Length)
                {
                    cursorPosition = newText.Length;
                }

                NameEntry.CursorPosition = cursorPosition;
            }
        }

        private void OnPhoneEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            var newText = e.NewTextValue;

            // ���� ���� ������, ��������� "+"
            if (string.IsNullOrEmpty(newText) && !newText.Contains("+"))
            {
                newText = "+";
            }

            // ������������ ���������� �������� �� 13 (������� +)
            if (newText.Length > 13)
            {
                newText = newText.Substring(0, 13);
            }

            // ��������� ������ ����� � ������ "+"
            newText = new string(newText.Where(c => char.IsDigit(c) || c == '+').ToArray());

            if (newText != e.NewTextValue)
            {
                int cursorPosition = PhoneEntry.CursorPosition;

                PhoneEntry.Text = newText;
                
                PhoneEntry.CursorPosition = cursorPosition < 1 ? 1 : cursorPosition;
            }
            else if (PhoneEntry.CursorPosition == 0)
            {
                PhoneEntry.CursorPosition = 1;
            }
            else if (PhoneEntry.CursorPosition == 1)
            {
                PhoneEntry.CursorPosition = 2;
            }

            // ���� ����� ��� �������, ��������� ����
            if (newText != e.NewTextValue)
            {
                PhoneEntry.Text = newText;
            }
        }
    }
}
