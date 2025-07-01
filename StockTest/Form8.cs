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
    public partial class Form8 : Form
    {
        StockChecker stockChecker;
        Form1 main;
        int per_p;

        public Form8(StockChecker _stockChecker, Form1 _main)
        {
            InitializeComponent();
            stockChecker = _stockChecker;
            main = _main;
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

        private void Form8_Load(object sender, EventArgs e)
        {
            if (stockChecker.is_highmedo_per)
            {
                textBox1.Text = String.Format("{0:#,###}", stockChecker.highMedo_per);
                per_p = stockChecker.highMedo_per;
            }
            else
            {
                textBox1.Text = String.Format("{0:#,###}", stockChecker.highMedo_price);
                per_p = stockChecker.highMedo_price;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Action<float, bool> action = (a, b) =>
            {
                if (b)
                {
                    per_p = (int)(stockChecker.price.price * a / 100f);
                    textBox1.Text = String.Format("{0:#,###}", per_p);
                }
            };
            main.CallValueWindowFloat(stockChecker.name + " : 고매도점 설정", action);
        }
    }
}
