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
    public partial class Form4 : Form
    {
        Action<int, int, bool> action;

        public Form4(string name, Action<int, int, bool> _action, int orgtick, int orgkind)
        {
            InitializeComponent();
            this.Text = name;
            label1.Text = name;
            textBox1.Text = orgtick.ToString();
            if(orgkind == 1)
            {
                checkBox1.Checked = true;
                checkBox2.Checked = false;
            }
            else if(orgkind == 2)
            {
                checkBox1.Checked = false;
                checkBox2.Checked = true;
            }

            action = _action;
        }

        void TextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                action(0, 0, false);
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
                int temp = int.Parse(textBox1.Text.Trim());
                int kind;
                if (checkBox1.Checked == true)
                    kind = 1;
                else
                    kind = 2;
                if (temp > 0)
                {
                    action(temp,kind,true);
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
            action(0, 0, false);
            Close();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(((CheckBox)sender).Checked)
            {
                checkBox2.Checked = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                checkBox1.Checked = false;
            }
        }
    }
}
