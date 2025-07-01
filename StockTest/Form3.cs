using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StockTest
{
    public partial class Form3 : Form
    {
        Action<int> action;

        public Form3(string name, Action<int> _action)
        {
            InitializeComponent();
            this.Text = name;
            label1.Text = name;
            action = _action;
            textBox1.KeyDown += TextBoxKeyDown;
            this.KeyDown += TextBoxKeyDown;
        }

        void TextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(sender, e);
            }
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int temp = int.Parse(textBox1.Text.Trim());
                if (temp >= 0)
                {
                    action(temp);
                    Close();
                }
                else
                {
                    MessageBox.Show("올바른 수를 입력해 주십시오.");
                }
            }
            catch
            {
                MessageBox.Show("올바른 수를 입력해 주십시오.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
