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
    public partial class Form2 : Form
    {
        Action<float, bool> action;
        public Form2(string name, Action<float, bool> _action)
        {
            InitializeComponent();
            this.Text = name;
            label1.Text = name;
            action = _action;
            textBox1.KeyDown += TextBoxKeyDown;
        }

        void TextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                action(0, false);
                Close();
            }
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(sender, e);
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                float temp = float.Parse(textBox1.Text.Trim());
                if (temp >= 0)
                {
                    action(temp, true);
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
            action(0, false);
            Close();
        }
    }
}
