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
    public partial class TimeSettingWindow : Form
    {
        Time time;
        Action<string> action;

        public TimeSettingWindow(Time _time, Action<string> _action)
        {
            InitializeComponent();
            time = _time;
            action = _action;
            this.KeyDown += TextBoxKeyDown;
            textBox1.KeyDown += TextBoxKeyDown;
        }

        private void TimeSettingWindow_Load(object sender, EventArgs e)
        {
            textBox1.Text = time.hour.ToString("00") + time.min.ToString("00") + time.sec.ToString("00");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length != 6)
                return;

            time.hour = int.Parse(textBox1.Text[0].ToString()) * 10 + int.Parse(textBox1.Text[1].ToString());
            time.min = int.Parse(textBox1.Text[2].ToString()) * 10 + int.Parse(textBox1.Text[3].ToString());
            time.sec = int.Parse(textBox1.Text[4].ToString()) * 10 + int.Parse(textBox1.Text[5].ToString());
            action(textBox1.Text);
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
    }
}
