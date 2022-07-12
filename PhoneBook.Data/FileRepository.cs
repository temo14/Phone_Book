using System;
using System.IO;
using System.Collections.Generic;
using PhoneBook.Models;

namespace PhoneBook.Data
{
    public class FileRepository
    {
        public IEnumerable<Contact> LoadContacts(string filePath)
        {
            List<Contact> contacts = new();

            if (filePath == null) throw new ArgumentNullException(nameof(filePath));

            using BinaryReader reader = new(new FileStream(filePath, FileMode.Open));

            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                Contact contact = new(Guid.Parse(reader.ReadString()));

                contact.FirstName = reader.ReadString();
                contact.LastName = reader.ReadString();
                contact.Email = reader.ReadString();
                contact.Phone = reader.ReadString();

                if (contacts.Contains(contact))
                {
                    throw new ArgumentException($"ID {contact.ID} is not unique");
                }

                contacts.Add(contact);
            }

            return contacts;
        }

        public void SaveContacts(string filePath, IEnumerable<Contact> contacts)
        {
            if (filePath == null) throw new ArgumentNullException(nameof(filePath));
            if (contacts == null) throw new ArgumentNullException(nameof(filePath));

            using BinaryWriter writer = new(new FileStream(filePath, FileMode.Create));

            foreach (var contact in contacts)
            {
                writer.Write(contact.ID.ToString());
                writer.Write(contact.FirstName);
                writer.Write(contact.LastName);
                writer.Write(contact.Email);
                writer.Write(contact.Phone);
            }
        }
    }
}
