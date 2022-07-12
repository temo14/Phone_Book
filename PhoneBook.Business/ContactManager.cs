using System;
using System.Collections.Generic;
using System.Linq;
using PhoneBook.Data;
using PhoneBook.Models;

namespace PhoneBook.Business
{
    public class ContactManager
    {
        private readonly FileRepository _fileRepository;
        private List<Contact> _contacts = new List<Contact>();

        public ContactManager()
        {
            _fileRepository = new();
        }

        //public IEnumerable<Contact> Contacts => _contacts.AsReadOnly();

        public void Add(Contact contact)
        {
            _contacts.Add(contact);
        }

        public void Edit(Contact contact)
        {
            int index = _contacts.IndexOf(contact);
            _contacts[index] = contact;
        }

        public void Delete(Contact contact)
        {
            int index = _contacts.IndexOf(contact);
            _contacts.RemoveAt(index);
        }

        public IEnumerable<Contact> Search(string text = null)
        {
            if (string.IsNullOrEmpty(text))
                return _contacts.AsReadOnly();

            List<Contact> contacts = new();
            foreach (var contact in _contacts)
            {
                if (contact.FirstName.ToLower().StartsWith(text) ||
                    contact.LastName.ToLower().StartsWith(text) ||
                    contact.Phone.ToLower().Contains(text) ||
                    contact.Email.ToLower().Contains(text))
                {
                    contacts.Add(contact);
                }
            }

            return contacts;
        }

        public void LoadContacts(string filePath)
        {
            _contacts = _fileRepository.LoadContacts(filePath).ToList();
        }

        public void SaveContacts(string filePath)
        {
            _fileRepository.SaveContacts(filePath, _contacts);
        }
    }
}
