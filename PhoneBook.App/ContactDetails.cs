using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PhoneBook.Models;

namespace PhoneBook.App
{
    public partial class ContactDetails : Form
    {
        public readonly Contact Contact;

        public ContactDetails(Contact? contact = null)
        {
            InitializeComponent();

            Contact = contact ?? new Contact();

            if (contact != null)
            {
                txtFirstName.Text = contact.FirstName;
                txtLastName.Text = contact.LastName;
                txtEmail.Text = contact.Email;
                txtPhoneNum.Text = contact.Phone;
            }
        }

        private void txtFirstName_TextChanged(object sender, EventArgs e)
        {
            Contact.FirstName = txtFirstName.Text;
        }

        private void txtLastName_TextChanged(object sender, EventArgs e)
        {
            Contact.LastName = txtLastName.Text;
        }

        private void txtPhoneNum_TextChanged(object sender, EventArgs e)
        {
            Contact.Phone = txtPhoneNum.Text;
        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {
            Contact.Email = txtEmail.Text;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;

            if ( Contact.Email == null ||
                Contact.Phone == null)
            {
                throw new ArgumentNullException("Email or Phone must be filled!", nameof(Contact));
            }
        }
    }
}
