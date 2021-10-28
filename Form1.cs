using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Kajabity.Tools.Java;

namespace JavaPropertiesDemo
{
    public partial class Form1 : Form
    {
        private readonly JavaProperties _properties = new JavaProperties();
        private bool _changed;
        private string _filename;

        public Form1()
        {
            InitializeComponent();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseDocument(sender, e);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                DefaultExt = @"properties",
                Title = @"Open Java Properties file",
                Filter = @"Java Properties files (*.properties)|*.properties|All files (*.*)|*.*",

                AddExtension = true,
                CheckFileExists = true,
                CheckPathExists = true,
                ReadOnlyChecked = false
            };

            if (dialog.ShowDialog(this) == DialogResult.OK && CloseDocument(sender, e))
            {
                Stream stream = new FileStream(dialog.FileName, FileMode.Open);
                _properties.Load(stream);
                stream.Close();

                UpdateDisplay();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_filename is null)
            {
                saveAsToolStripMenuItem_Click(sender, e);
            }
            else
            {
                SaveDocument();
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                DefaultExt = @"properties",
                Title = @"Save Java Properties file",
                Filter = @"Java Properties files (*.properties)|*.properties|All files (*.*)|*.*",
                FileName = _filename
            };

            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                _filename = dialog.FileName;
                SaveDocument();
            }
        }

        private void SaveDocument()
        {
            Stream stream = new FileStream(_filename, FileMode.Create);
            _properties.Store(stream, true);
            stream.Close();

            _changed = false;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private bool CloseDocument(object sender, EventArgs e)
        {
            if (_changed)
            {
                var result = MessageBox.Show(this,
                    @"Java Properties file " + _filename + @" has been modified!\n\nDo you want to save it?",
                    @"Java Properties Demo",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Exclamation);

                if (result == DialogResult.Yes)
                {
                    saveToolStripMenuItem_Click(sender, e);
                }
                else if (result == DialogResult.Cancel)
                {
                    return false;
                }
            }

            _properties.Clear();
            _changed = false;
            _filename = null;

            return true;
        }

        private void UpdateDisplay()
        {
            // Clear the list.
            listView1.Items.Clear();

            // Add the entries to the ListView.
            foreach (var key in _properties.Keys)
            {
                var item = new ListViewItem(key);
                var text = _properties.GetProperty(key);
                item.SubItems.Add(new ListViewItem.ListViewSubItem(item, text));

                listView1.Items.Add(item);
            }

            //	Force a display update.
            Refresh();
        }

        private void listView1_ItemActivate(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                var item = listView1.SelectedItems[0];
                var dialog = new Form2
                {
                    Text = @"Edit Entry",
                    EntryName = item.Text,
                    EntryValue = item.SubItems[1].Text
                };

                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    _properties.Remove(item.Text);
                    _properties.SetProperty(dialog.EntryName, dialog.EntryValue);
                    _changed = true;

                    UpdateDisplay();
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!CloseDocument(sender, e))
            {
                e.Cancel = true;
            }
        }

        private void insertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new Form2
            {
                Text = @"Add Entry"
            };

            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                _properties.SetProperty(dialog.EntryName, dialog.EntryValue);
                _changed = true;

                UpdateDisplay();
            }
        }

        private void goToArticleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.kajabity.com");
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            deleteToolStripMenuItem.Enabled = listView1.SelectedIndices.Count > 0;
        }
    }
}
