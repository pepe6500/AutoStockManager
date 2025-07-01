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
    public partial class Form5 : Form
    {
        Form1 main;
        int index;
        public Form5(Form1 _main, int _index)
        {
            this.KeyDown += TextBoxKeyDown;
            InitializeComponent();
            main = _main;
            index = _index;
            if(main.stockCheckers[index].state == StockChecker.StockState.buywait)
            {
                checkBox2.Checked = false;
                checkBox3.Checked = true;
            }
            else
            {
                checkBox2.Checked = true;
                checkBox3.Checked = false;
            }
            KeyPreview = true;
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            if (checkBox1.Checked == false)
            {
                try
                {
                    int temp = int.Parse(textBox1.Text.Trim());
                    if (temp > 0)
                    {
                        if (checkBox2.Checked)
                        {
                            main.stockCheckers[index].auto_buy_price = false;
                            main.stockCheckers[index].buy_price = temp;
                        }
                        else
                        {
                            main.stockCheckers[index].auto_sell_price = false;
                            main.stockCheckers[index].sell_price = temp;
                        }
                    }
                    else
                    {
                        MessageBox.Show("올바른 수를 입력해 주십시오.");
                        return;
                    }
                }
                catch
                {
                    MessageBox.Show("올바른 수를 입력해 주십시오.");
                    return;
                }
            }

            if (checkBox2.Checked)
            {
                main.stockCheckers[index].state = StockChecker.StockState.sellwait;
            }
            else
            {
                main.stockCheckers[index].state = StockChecker.StockState.buywait;
            }
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox2.Checked)
            {
                checkBox3.Checked = false;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                checkBox2.Checked = false;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
            {
                textBox1.Text = "현재가";
                textBox1.ReadOnly = true;
                textBox1.Enabled = false;
            }
            else
            {
                textBox1.Text = "";
                textBox1.ReadOnly = false;
                textBox1.Enabled = true;
            }
        }
    }
}
