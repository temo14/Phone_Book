using System;
using System.Collections.Generic;

namespace PhoneBook.Models
{
    public class Contact
    {
        public Contact(Guid? id = null)
        {
            ID = id ?? Guid.NewGuid();
        }

        public Guid ID { get; private set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string FullName => $"{FirstName} {LastName}";

        public override bool Equals(object obj)
        {
            return obj is Contact contact &&
                   ID == contact.ID;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ID);
        }

        public override string ToString() => FullName;

        public static bool operator ==(Contact left, Contact right)
        {
            return EqualityComparer<Contact>.Default.Equals(left, right);
        }

        public static bool operator !=(Contact left, Contact right)
        {
            return !(left == right);
        }
    }
}
