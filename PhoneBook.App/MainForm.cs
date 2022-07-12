using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PhoneBook.Business;
using PhoneBook.Models;

namespace PhoneBook.App
{
    public partial class MainForm : Form
    {
        private readonly ContactManager _contactManager;
        private string _filePath;

        private string FilePath
        {
            get
            {
                return _filePath;
            }
            set
            {
                _filePath = value;
                this.Text = $"PhoneBook - {(_filePath != null ? Path.GetFileName(value) : "Untitled.txt")}";
            }
        }
        public MainForm()
        {
            InitializeComponent();
            _contactManager = new();

            grdContacts.AutoGenerateColumns = false;
        }
        private bool IsModified { get; set; } = false;

        #region Event Handlers
        private void EditBtn_Click(object sender, EventArgs e)
        {
            EditRow();
        }
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditRow();
        }
        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddRow();
        }     
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewFile();
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFile();
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile(true);
        }
        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            DeleteRow();
        }
        #endregion
        private void EditRow()
        {
            // Pass chosen Contact object.
            ContactDetails contactDetails = new(ReturnContact());

            if (contactDetails.ShowDialog() == DialogResult.OK)
            {
                IsModified = true;
                _contactManager.Edit(contactDetails.Contact);
                var contacts = _contactManager.Search().ToList();
                grdContacts.DataSource = contacts;
            }
        }
        private void AddRow()
        {
            ContactDetails contactDetails = new();

            if (contactDetails.ShowDialog() == DialogResult.OK)
            {
                IsModified = true;
                _contactManager.Add(contactDetails.Contact);
                var contacts = _contactManager.Search().ToList();
                grdContacts.DataSource = contacts;
            }
        }
        private void DeleteRow()
        {
            _contactManager.Delete(ReturnContact());

            var contacts = _contactManager.Search().ToList();
            grdContacts.DataSource = contacts;
        }
        private void NewFile()
        {
            if (!ConfirmSave())
            {
                return;
            }
            grdContacts.DataSource = null;
            FilePath = null;
            IsModified = false;
        }

        private void OpenFile()
        {
            if (!ConfirmSave())
            {
                return;
            }
            if (dlgOpen.ShowDialog() == DialogResult.OK)
            {
                FilePath = dlgOpen.FileName;
                IsModified = false;

                _contactManager.LoadContacts(FilePath);
                var contacts = _contactManager.Search().ToList();

                grdContacts.DataSource = contacts;
            }

        }

        private bool SaveFile(bool isSaveAs = false)
        {
            if (FilePath == null || isSaveAs)
            {
                dlgSave.FileName = Path.GetFileName(dlgSave.FileName);
                if (dlgSave.ShowDialog() == DialogResult.OK)
                    FilePath = dlgSave.FileName;
                else
                    return false;
            }
            _contactManager.SaveContacts(FilePath);
            IsModified = false;

            return true;
        }
        private Contact ReturnContact()
        {

            var row = grdContacts.SelectedRows[0];

            Contact contact = new();
            for (int col = 0; col < row.Cells.Count; col++)
            {
                // Read value from cell.
                var cell = grdContacts.Rows[row.Index].Cells[col].EditedFormattedValue.ToString();
                switch (col)
                {
                    case 1:
                        contact.FirstName = cell;
                        break;
                    case 2:
                        contact.LastName = cell;
                        break;
                    case 3:
                        contact.Email = cell;
                        break;
                    case 4:
                        contact.Phone = cell;
                        break;
                    default:
                        contact = new(Guid.Parse(cell));
                        break;
                }
            }
            return contact;
        }

        private bool ConfirmSave()
        {
            if (!IsModified)
            {
                return true;
            }
            DialogResult result = MessageBox.Show(
                "Do you want to save changes?",
                "Confirmation",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button3
                );
            switch (result)
            {
                case DialogResult.Yes:
                    return SaveFile();
                case DialogResult.No:
                    return true;
                default:
                    return false;
            }
        }

        private void search_Click(object sender, EventArgs e)
        {
            grdContacts.DataSource = _contactManager.Search(search.Text.ToLower());
        }
    }
}
