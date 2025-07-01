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
    public partial class Form9 : Form
    {
        Form1 main;

        string code;
        string accnt_no;
        int count;
        int price;

        public Form9(Form1 _main, string _accnt_no)
        {
            InitializeComponent();
            main = _main;
            accnt_no = _accnt_no;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text == "")
            {
                MessageBox.Show("값을 입력해 주십시요");
                Close();
                return;
            }
            if (textBox2.Text == "" || textBox2.Text == "0")
            {
                main.AddList(textBox1.Text);
                Close();
                return;
            }
            if (!checkBox1.Checked && textBox3.Text == "")
            {
                MessageBox.Show("값을 입력해 주십시요");
                Close();
                return;
            }

            if (checkBox1.Checked)
            {
                main.Mesu("", accnt_no, main.get_scr_no(), textBox1.Text, count);
            }
            else
            {
                main.Mesu("", accnt_no, main.get_scr_no(), textBox1.Text, count, price);
            }
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
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

        private void Form9_Load(object sender, EventArgs e)
        {
            this.KeyDown += TextBoxKeyDown;
            this.textBox1.KeyDown += TextBoxKeyDown;
            this.textBox2.KeyDown += TextBoxKeyDown;
            this.textBox3.KeyDown += TextBoxKeyDown;
            this.button1.KeyDown += TextBoxKeyDown;
            this.button2.KeyDown += TextBoxKeyDown;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Action<int> action = (a) =>
            {
                code = a.ToString();
                textBox1.Text = code;
            };
            main.CallValueWindowInt("종목코드", action);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            Action<int> action = (a) =>
            {
                count = a;
                textBox2.Text = count.ToString();
            };
            main.CallValueWindowInt("주문수량", action);
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            Action<int> action = (a) =>
            {
                price = a;
                textBox3.Text = price.ToString();
            };
            main.CallValueWindowInt("주문가격", action);
        }
        
    }
}
