using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using PhoneBook.Models;
using Xunit;
using PhoneBook.Data;

namespace PhoneBook.Test
{   
    internal static class SavedListClass
    {
        public static List<Contact> SavedContacts = new();

    }
    public class RepositoryTests
    {
        private bool AreListsSame(List<Contact> contacts1, List<Contact> contacts2)
        {
            if (contacts1 == null) throw new ArgumentNullException(nameof(contacts1));
            if (contacts2 == null) throw new ArgumentNullException(nameof(contacts2));

            if (contacts1.Count != contacts2.Count || contacts1.Count == 0)
                return false;

            for (int i = 0; i < contacts1.Count; i++)
            {
                if (contacts1[i].ID != contacts2[i].ID &&
                    contacts1[i].FirstName != contacts2[i].FirstName &&
                    contacts1[i].LastName != contacts2[i].LastName &&
                    contacts1[i].Email != contacts2[i].Email &&
                    contacts1[i].Phone != contacts2[i].Phone)
                {
                    return false;
                }
            }

            return true;
        }

        private void InitializeContacts(List<Contact> contacts)
        {
            for (int i = 1; i < 5; i++)
            {
                Contact contact = new();
                contacts.Add(contact);
            }
        }

        private void GenerateContacts(List<Contact> contacts)
        {
            InitializeContacts(contacts);
            Random r = new();
            foreach (var contact in contacts)
            {
                contact.FirstName = $"{r.Next(1, 1000)} ";
                contact.LastName = $"{r.Next(1, 1000)},";
                contact.Email = $"email{r.Next(1, 1000)}@email.com,";
                contact.Phone = $"Number : {r.Next(100000, 999999)}. ";
            }
        }


        [Theory]
        [InlineData(@"D:\G05.txt")]
        public void A_SaveTest(string filePath)
        {
            FileRepository fileRepository = new();
            List<Contact> contacts = new();
            GenerateContacts(contacts);
            SavedListClass.SavedContacts = contacts;
            try
            {
                fileRepository.SaveContacts(filePath, contacts);
                Assert.True(true);
            }
            catch
            {
                Assert.True(false);
            }
        }

        [Theory]
        [InlineData(@"D:\G05.txt")]
        public void B_LoadTest(string filePath)
        {
            if (filePath == null) throw new ArgumentNullException(nameof(filePath));
            FileRepository fileRepository = new();
            List<Contact> contacts = fileRepository.LoadContacts(filePath).ToList();

            Assert.True(AreListsSame(contacts, SavedListClass.SavedContacts));
        }
    }
}
