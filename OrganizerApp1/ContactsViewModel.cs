using OrganizerApp1.Models;
using OrganizerApp1.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;

namespace OrganizerApp1
{
    public class ContactsViewModel : INotifyPropertyChanged
    {
        private readonly ContactsDatabaseService _databaseService;

        private AppContact _selectedContact;
        public ObservableCollection<AppContact> Contacts { get; set; }
        public AppContact SelectedContact
        {
            get => _selectedContact;
            set
            {
                if (_selectedContact != value)
                {
                    _selectedContact = value;
                    OnPropertyChanged(nameof(SelectedContact));
                }
            }
        }

        public ContactsViewModel()
        {
            _databaseService = new ContactsDatabaseService();
            Contacts = new ObservableCollection<AppContact>();
        }

        // Загрузить контакты из базы данных
        public async Task LoadContactsAsync()
        {
            var contacts = await _databaseService.GetContactsAsync();
            var sortedContacts = contacts.OrderBy(contact => contact.Name).ToList();

            Contacts.Clear();
            foreach (var contact in sortedContacts)
            {
                Contacts.Add(contact); 
            }
        }

        // Добавить контакт
        public async Task AddContactAsync(AppContact contact)
        {
            await _databaseService.SaveContactAsync(contact);
            await LoadContactsAsync(); 
        }

        // Обновить контакт
        public async Task UpdateContactAsync(AppContact contact)
        {
            if (contact.Id == 0)
            {
                // Если Id == 0, значит это новый контакт, добавляем его
                await _databaseService.SaveContactAsync(contact);
            }
            else
            {
                // Иначе обновляем существующий контакт
                await _databaseService.SaveContactAsync(contact);
            }

            await LoadContactsAsync();
        }

        // Удалить контакт
        public async Task DeleteContactAsync(AppContact contact)
        {
            await _databaseService.DeleteContactAsync(contact);
            await LoadContactsAsync(); 
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // Оповещение об изменении свойства
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}