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
            // Проверка обязательных полей
            if (string.IsNullOrWhiteSpace(NameEntry.Text) || string.IsNullOrWhiteSpace(PhoneEntry.Text))
            {
                await DisplayAlert("Ошибка", "Пожалуйста, заполните имя и номер телефона.", "OK");
                return;
            }

            // Проверка длины номера телефона
            if (PhoneEntry.Text.Length != 13)
            {
                await DisplayAlert("Ошибка", "Введите корректный номер телефона.", "OK");
                return;
            }

            // Проверка электронной почты (если введена)
            if (!string.IsNullOrWhiteSpace(EmailEntry.Text) &&
                (!EmailEntry.Text.Contains('@') || !EmailEntry.Text.Contains('.')))
            {
                await DisplayAlert("Ошибка", "Пожалуйста, введите корректный адрес электронной почты.", "OK");
                return;
            }

            // Создание нового контакта
            var newContact = new AppContact
            {
                Name = NameEntry.Text,
                PhoneNumber = PhoneEntry.Text,
                Email = EmailEntry.Text
            };

            // Генерация события для возврата нового контакта
            ContactSaved?.Invoke(this, newContact);
            await Navigation.PopAsync();
        }

        private void OnNameEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            var newText = e.NewTextValue;

            // Разрешаем вводить только буквы и пробелы
            newText = new string(newText.Where(c => char.IsLetter(c) || c == ' ').ToArray());

            // Преобразуем каждое слово так, чтобы первая буква была заглавной, а остальные - строчными
            if (!string.IsNullOrWhiteSpace(newText))
            {
                newText = string.Join(" ", newText.Split(' ')
                                                  .Select(word => word.Length > 0 ? char.ToUpper(word[0]) + word.Substring(1).ToLower() : ""));
            }

            // Обновляем текст только если он был изменен
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

            // Оставляем только цифры и один символ "+"
            newText = "+" + new string(newText.Skip(1).Where(char.IsDigit).ToArray());

            // Ограничиваем количество символов до 13 (включая +)
            if (newText.Length > 13)
            {
                newText = newText.Substring(0, 13);
            }

            // Проверка для обновления текста в поле и восстановления позиции курсора
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
