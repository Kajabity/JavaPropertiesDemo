using System;
using System.Windows.Forms;

namespace JavaPropertiesDemo
{
    public partial class Form2 : Form
    {
        public String EntryName { get; set; }
        public string EntryValue { get; set; }

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            textBox1.Text = EntryName;
            textBox2.Text = EntryValue;
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            {
                EntryName = textBox1.Text;
                EntryValue = textBox2.Text;
            }
        }
    }
}
