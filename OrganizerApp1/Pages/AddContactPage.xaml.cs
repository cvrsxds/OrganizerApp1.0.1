using OrganizerApp1.Models;
using System;
using System.Linq;
using Microsoft.Maui.Controls;

namespace OrganizerApp1.Pages
{
    public partial class AddContactPage : ContentPage
    {
        public event EventHandler<AppContact> ContactSaved;

        public AddContactPage()
        {
            InitializeComponent();
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

            // �������� ����������� ����� (���� �������)
            if (!string.IsNullOrWhiteSpace(EmailEntry.Text) &&
                (!EmailEntry.Text.Contains('@') || !EmailEntry.Text.Contains('.')))
            {
                await DisplayAlert("������", "����������, ������� ���������� ����� ����������� �����.", "OK");
                return;
            }

            // �������� ������ ��������
            var newContact = new AppContact
            {
                Name = NameEntry.Text,
                PhoneNumber = PhoneEntry.Text,
                Email = EmailEntry.Text
            };

            // ��������� ������� ��� �������� ������ ��������
            ContactSaved?.Invoke(this, newContact);
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

            if (!newText.StartsWith("+"))
            {
                newText = "+" + newText;
            }

            // ��������� ������ ����� � ���� ������ "+"
            newText = "+" + new string(newText.Skip(1).Where(char.IsDigit).ToArray());

            // ������������ ���������� �������� �� 13 (������� +)
            if (newText.Length > 13)
            {
                newText = newText.Substring(0, 13);
            }

            // �������� ��� ���������� ������ � ���� � �������������� ������� �������
            if (newText != e.NewTextValue)
            {
                int cursorPosition = PhoneEntry.CursorPosition;

                PhoneEntry.Text = newText;
                
                PhoneEntry.CursorPosition = cursorPosition < 1 ? 1 : cursorPosition;
            }
            else if (PhoneEntry.CursorPosition == 0)
            {
                PhoneEntry.CursorPosition = 1;
            }else if(PhoneEntry.CursorPosition == 1)
            {
                PhoneEntry.CursorPosition = 2;
            }
        }

    }
}
