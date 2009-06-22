using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TripleAGameCreator
{
    public partial class StringRetriever : Form
    {
        public StringRetriever()
        {
            InitializeComponent();
            oldSize = Size;
        }
        public string Value = "";
        public Form1 parent = null;
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Visible)
            {
                Value = textBox1.Text;
                textBox1.Focus();
                textBox1.SelectAll();
            }
            else if (comboBox1.Visible)
            {
                Value = comboBox1.Text;
                comboBox1.Focus();
                comboBox1.SelectAll();
            }
            else if (numericUpDown1.Visible)
            {
                Value = numericUpDown1.Text;
                numericUpDown1.Focus();
                this.numericUpDown1.Select(0, this.numericUpDown1.Text.Length);
            }
            Hide();
        }
        Size oldSize;
        private void StringRetriever_Load(object sender, EventArgs e)
        {

        }
        public String RetrieveString(string labelString)
        {
            this.Value = "";
            this.Text = labelString;
            this.textBox1.Show();
            this.comboBox1.Hide();
            this.numericUpDown1.Hide();
            this.textBox1.SelectAll();
            this.ShowDialog();
            return this.Value;
        }
        public String RetrieveString(string labelString, string textBoxString)
        {
            this.Value = "";
            this.Text = labelString;
            this.textBox1.Show();
            this.comboBox1.Hide();
            this.numericUpDown1.Hide();
            this.textBox1.Text = textBoxString;
            this.textBox1.SelectAll();
            this.ShowDialog();
            return this.Value;
        }
        public String RetrieveString(string labelString, string textBoxString,object[] comboBoxItems)
        {
                this.Value = "";
                this.Text = labelString;
                this.textBox1.Hide();
                this.numericUpDown1.Hide();
                this.comboBox1.Show();

                this.comboBox1.Text = textBoxString;
                this.comboBox1.SelectAll();
                this.comboBox1.Items.Clear();
                this.comboBox1.Items.AddRange(comboBoxItems);

                this.ShowDialog();
                return this.Value;
        }
        public String RetrieveString(string labelString, int numberToDisplay)
        {
                this.Value = "";
                this.Text = labelString;
                this.textBox1.Hide();
                this.comboBox1.Hide();
                this.numericUpDown1.Show();

                this.numericUpDown1.Value = numberToDisplay;
                this.numericUpDown1.Select(0, this.numericUpDown1.Text.Length);

                this.ShowDialog();
                return this.Value;
        }
        private void StringRetriever_Resize(object sender, EventArgs e)
        {
            textBox1.Width = 207 + (Size.Width - oldSize.Width);
            button1.Location = new Point(225 + (Size.Width - oldSize.Width), 10);
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Value = textBox1.Text;
                Hide();
            }
        }
    }
}
