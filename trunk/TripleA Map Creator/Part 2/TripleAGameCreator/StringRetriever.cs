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
                Value = textBox1.Text;
                Hide();
        }
        Size oldSize;
        private void StringRetriever_Load(object sender, EventArgs e)
        {

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

        private void StringRetriever_Activated(object sender, EventArgs e)
        {
            textBox1.Focus();
        }
    }
}
