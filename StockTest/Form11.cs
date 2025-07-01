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
    public partial class Form11 : Form
    {
        Form1 main;
        StockChecker stockChecker;
        int kind;

        public Form11(Form1 _main, int _kind, StockChecker _stockChecker)
        {
            InitializeComponent();
            main = _main;
            kind = _kind;
            stockChecker = _stockChecker;
            if (kind == 0)
            {
                label1.Text = stockChecker.code + " " + stockChecker.name + Environment.NewLine + "전량매도 하시겠습니까?";
            }
            else
            {
                label1.Text = stockChecker.code + " " + stockChecker.name + Environment.NewLine + "반매도 하시겠습니까?";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(kind == 0)
            {
                main.Medo(stockChecker.name, main.get_scr_no(), stockChecker.accnt_no, stockChecker.code, stockChecker.count);
            }
            else
            {
                main.Medo(stockChecker.name, main.get_scr_no(), stockChecker.accnt_no, stockChecker.code, stockChecker.count / 2);
            }
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
