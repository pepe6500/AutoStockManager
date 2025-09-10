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
    public partial class Form7 : Form
    {
        Form1 main;
        StockChecker stockChecker;
        public Form7(Form1 _main, StockChecker _stockChecker)
        {
            main = _main;
            stockChecker = _stockChecker;
            InitializeComponent();
            textBox1.Text = stockChecker.sell_start.ToString("##0.0");
            textBox2.Text = stockChecker.sell_end.ToString("##0.0");
            textBox3.Text = stockChecker.sell_per.ToString("##0.00");
            textBox6.Text = stockChecker.buy_start.ToString("##0.0");
            textBox5.Text = stockChecker.buy_end.ToString("##0.0");
            textBox4.Text = stockChecker.buy_per.ToString("##0.00");
            textBox7.Text = stockChecker.buy_per_count.ToString("##0.00");
            textBox8.Text = stockChecker.sell_per_count.ToString("##0.00");
            this.KeyDown += TextBoxKeyDown;
            this.textBox1.KeyDown += TextBoxKeyDown;
            this.textBox2.KeyDown += TextBoxKeyDown;
            this.textBox3.KeyDown += TextBoxKeyDown;
            this.textBox4.KeyDown += TextBoxKeyDown;
            this.textBox5.KeyDown += TextBoxKeyDown;
            this.textBox6.KeyDown += TextBoxKeyDown;
            this.textBox7.KeyDown += TextBoxKeyDown;
            this.textBox8.KeyDown += TextBoxKeyDown;
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

        private void Form7_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Action<float, bool> temp = (a, b) =>
            {
                if (b)
                {
                    ((TextBox)sender).Text = a.ToString("##0.0");
                }
            };
            main.CallValueWindowFloat("저상승", temp);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            Action<float, bool> temp = (a, b) =>
            {
                if (b)
                {
                    ((TextBox)sender).Text = a.ToString("##0.0");
                }
            };
            main.CallValueWindowFloat("고상승", temp);
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            Action<float, bool> temp = (a, b) =>
            {
                if (b)
                {
                    ((TextBox)sender).Text = a.ToString("##0.00");
                }
            };
            main.CallValueWindowFloat("변동", temp);
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            Action<float, bool> temp = (a, b) =>
            {
                if (b)
                {
                    ((TextBox)sender).Text = a.ToString("##0.0");
                }
            };
            main.CallValueWindowFloat("저하락", temp);
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            Action<float, bool> temp = (a, b) =>
            {
                if (b)
                {
                    ((TextBox)sender).Text = a.ToString("##0.0");
                }
            };
            main.CallValueWindowFloat("고하락", temp);
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            Action<float, bool> temp = (a, b) =>
            {
                if (b)
                {
                    ((TextBox)sender).Text = a.ToString("##0.00");
                }
            };
            main.CallValueWindowFloat("변동", temp);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            stockChecker.sell_start = float.Parse(textBox1.Text);
            stockChecker.sell_end = float.Parse(textBox2.Text);
            stockChecker.sell_per = float.Parse(textBox3.Text);
            stockChecker.sell_per_count = float.Parse(textBox8.Text);
            stockChecker.buy_start = float.Parse(textBox6.Text);
            stockChecker.buy_end = float.Parse(textBox5.Text);
            stockChecker.buy_per = float.Parse(textBox4.Text);
            stockChecker.buy_per_count = float.Parse(textBox7.Text);
            stockChecker.SetCounts();
            main.Send_Log(stockChecker.name + " 비중변경");
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            Action<float, bool> temp = (a, b) =>
            {
                if (b)
                {
                    ((TextBox)sender).Text = a.ToString("##0.00");
                }
            };
            main.CallValueWindowFloat("매수 호가당개수", temp);
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            Action<float, bool> temp = (a, b) =>
            {
                if (b)
                {
                    ((TextBox)sender).Text = a.ToString("##0.00");
                }
            };
            main.CallValueWindowFloat("매도 호가당개수", temp);
        }
    }
}
