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
    public partial class FormChangeMarkPrice : Form
    {
        Action<int, bool> action;

        public FormChangeMarkPrice(string name, bool isChecked, Action<int, bool> _action)
        {
            InitializeComponent();
            this.Text = name;
            label1.Text = name;
            checkBox1.Checked = isChecked;
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

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int result1 = int.Parse(textBox1.Text.Trim());
                bool result2 = checkBox1.Checked;
                if (result1 >= 0)
                {
                    action(result1, result2);
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

        private void FormChangeMarkPrice_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }
    }
}
