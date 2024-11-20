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

            // Приведение первой буквы имени к заглавной
            if (!string.IsNullOrEmpty(NameEntry.Text))
            {
                Contact.Name = NameEntry.Text;
            }

            // Обновляем данные контакта
            Contact.PhoneNumber = PhoneEntry.Text;
            Contact.Email = EmailEntry.Text;

            // Сохраняем измененные данные в базе данных
            var databaseService = new ContactsDatabaseService();
            if (Contact.Id == 0)
            {
                await databaseService.SaveContactAsync(Contact);
            }
            else
            {
                await databaseService.SaveContactAsync(Contact);
            }

            // Генерируем событие обновления контакта
            ContactUpdated?.Invoke(this, Contact);

            // Возвращаемся на предыдущую страницу
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

            // Если поле пустое, добавляем "+"
            if (string.IsNullOrEmpty(newText) && !newText.Contains("+"))
            {
                newText = "+";
            }

            // Ограничиваем количество символов до 13 (включая +)
            if (newText.Length > 13)
            {
                newText = newText.Substring(0, 13);
            }

            // Оставляем только цифры и символ "+"
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

            // Если текст был изменен, обновляем поле
            if (newText != e.NewTextValue)
            {
                PhoneEntry.Text = newText;
            }
        }
    }
}
