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
    public partial class AccountNumbersForm : Form
    {
        string[] accountNumbers;
        bool[] isEnabled;
        Callback callback;

        public delegate void Callback(bool[] resultEnabledArray);

        public AccountNumbersForm(string[] accountNumbers, bool[] isEnabled, Callback callback)
        {
            InitializeComponent();

            this.accountNumbers = accountNumbers;
            this.isEnabled = isEnabled;
            this.callback = callback;
        }

        private void AccountNumbersForm_Load(object sender, EventArgs e)
        {
            checkedListBox1.Items.Clear();
            checkedListBox1.Items.AddRange(accountNumbers);
            for (int i = 0; i < isEnabled.Length; i++)
            {
                checkedListBox1.SetItemChecked(i, isEnabled[i]);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < isEnabled.Length; i++)
            {
                isEnabled[i] = checkedListBox1.GetItemChecked(i);
            }
            callback(isEnabled);
            this.Close();
        }
    }
}
