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
    public partial class SelectAutoBuy : Form
    {
        Form1 main;

        public SelectAutoBuy(Form1 _main)
        {
            main = _main;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < main.conditionProcess.conditions.Count; i++)
            {
                if(main.xmlData.Find("Condition" + main.conditionProcess.conditions[i].name + ".isPlay") != null)
                {
                    main.conditionProcess.conditions[i].isPlay = bool.Parse(main.xmlData.Find("Condition" + main.conditionProcess.conditions[i].name + ".isPlay").value);
                }
            }
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
