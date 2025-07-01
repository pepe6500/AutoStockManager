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
    public partial class Form12 : Form
    {
        Form1 main;

        public Form12(Form1 _main)
        {
            main = _main;
            main.conditionWindow = this;
            InitializeComponent();
            this.KeyPreview = true;
            this.FormClosing += OnClosing;
            this.KeyDown += TextBoxKeyDown;
            for (int i = 0; i < this.Controls.Count; i++)
            {
                for (int j = 0; j < this.Controls[i].Controls.Count; j++)
                {
                    this.Controls[i].Controls[j].KeyDown += TextBoxKeyDown;
                }
                this.Controls[i].KeyDown += TextBoxKeyDown;
            }

            playing_cb.Checked = main.conditionProcess.is_playing;
            checkBox11.Checked = main.conditionProcess.is_sell;
            checkBox10.Checked = main.conditionProcess.is_buy;
            checkBox15.Checked = main.conditionProcess.is_highbuy;
            checkBox15.Checked = main.conditionProcess.is_highbuy;
            pricechangesound.Checked = main.conditionProcess.is_price_change_sound;
            targetprice_text.Text = "목표가: " + String.Format("{0:#,###}", main.conditionProcess.targetPrice / 10000) + "만원";
            label41.Text = "남김: " + main.conditionProcess.remainCountVal;
            checkBox6.Checked = main.conditionProcess.end_clear;
            checkBox7.Checked = main.conditionProcess.is_highmedo;
            checkBox7.Checked = main.conditionProcess.is_cut;
            checkBox9.Checked = main.conditionProcess.is_rebuy;
            checkBox13.Checked = main.conditionProcess.haveEffect;
            checkBox12.Checked = main.conditionProcess.is_endLimit;
            if (main.conditionProcess.is_highmedo_per)
                label37.Text = "고매:" + main.conditionProcess.highMedo_perval.ToString("##0.0") + "%";
            else
                label37.Text = "고매:" + String.Format("{0:#,###}", main.conditionProcess.highMedo_price) + "원";
            if (main.conditionProcess.cut_price < 100)
                label38.Text = "손절: " + main.conditionProcess.cut_price.ToString("##0.0") + "%";
            else
                label38.Text = "손절: " + String.Format("{0:#,###}", main.conditionProcess.cut_price) + "원";
        }

        private void OnClosing(object sender, EventArgs e)
        {
            main.conditionWindow = null;
        }

        private void Form12_Load(object sender, EventArgs e)
        {
            string[] conditions = new string[main.conditionProcess.conditions.Count + 1];
            conditions[0] = "선택안함";
            for (int i = 0; i < main.conditionProcess.conditions.Count; i++)
            {
                conditions[i + 1] = main.conditionProcess.conditions[i].name;
            }
            comboBox1.Items.AddRange(conditions);
            comboBox2.Items.AddRange(conditions);
            comboBox3.Items.AddRange(conditions);
            comboBox4.Items.AddRange(conditions);
            comboBox5.Items.AddRange(conditions);
            if(main.conditionProcess.onPlayConditions[0] != null)
                comboBox1.SelectedItem = main.conditionProcess.onPlayConditions[0].name;
            else
                comboBox1.SelectedIndex = 0;
            if (main.conditionProcess.onPlayConditions[1] != null)
                comboBox2.SelectedItem = main.conditionProcess.onPlayConditions[1].name;
            else
                comboBox2.SelectedIndex = 0;
            if (main.conditionProcess.onPlayConditions[2] != null)
                comboBox3.SelectedItem = main.conditionProcess.onPlayConditions[2].name;
            else
                comboBox3.SelectedIndex = 0;
            if (main.conditionProcess.onPlayConditions[3] != null)
                comboBox4.SelectedItem = main.conditionProcess.onPlayConditions[3].name;
            else
                comboBox4.SelectedIndex = 0;
            if (main.conditionProcess.onPlayConditions[4] != null)
                comboBox5.SelectedItem = main.conditionProcess.onPlayConditions[4].name;
            else
                comboBox5.SelectedIndex = 0;
            ReloadPanel();
            ReloadLinks();
        }

        public void ReloadPanel(int index1, int index2)
        {
            if (index1 >= 5)
                return;
            if (main.conditionProcess.onPlayConditions[index1] == null)
                return;
            List<StockInfo> stocks = main.conditionProcess.onPlayConditions[index1].stocks;
            if (index2 >= stocks.Count)
                return;
            DataGridView dataGrid = (DataGridView)this.Controls["flowLayoutPanel1"].Controls["panel" + (index1 + 1)].Controls["dataGridView" + (index1 + 1)];
            DataGridViewRow dataGridViewRow = null;
            for(int i = 0; i < dataGrid.Rows.Count; i++)
            {
                if (dataGrid.Rows[i].Cells[0].Value.ToString() == stocks[index2].name)
                {
                    dataGridViewRow = dataGrid.Rows[i];
                    break;
                }
            }

            if (dataGridViewRow == null)
                return;

            dataGridViewRow.Cells[0].Value = stocks[index2].name;
            dataGridViewRow.Cells[1].Value = stocks[index2].price;
            dataGridViewRow.Cells[2].Value = stocks[index2].fluctuation;
            dataGridViewRow.Cells[3].Value = stocks[index2].netChange;
            dataGridViewRow.Cells[4].Value = stocks[index2].netChangeTrading;
        }

        public void ReloadPanel(int index)
        {
            if (index >= 5)
                return;
            DataGridView dataGrid = (DataGridView)this.Controls["flowLayoutPanel1"].Controls["panel" + (index + 1)].Controls["dataGridView" + (index + 1)];
            if(main.conditionProcess.onPlayConditions[index] == null)
            {
                dataGrid.Rows.Clear();
                return;
            }
            List<StockInfo> stocks = main.conditionProcess.onPlayConditions[index].stocks;
            if (dataGrid.Rows.Count > stocks.Count)
            {
                dataGrid.Rows.Clear();
                if(stocks.Count != 0)
                    dataGrid.Rows.Add(stocks.Count);
            }
            else if(dataGrid.Rows.Count < stocks.Count)
            {
                dataGrid.Rows.Add(stocks.Count - dataGrid.Rows.Count);
            }

            for(int i = 0; i < stocks.Count; i++)
            {
                dataGrid.Rows[i].Cells[0].Value = stocks[i].name;
                dataGrid.Rows[i].Cells[1].Value = stocks[i].price;
                dataGrid.Rows[i].Cells[2].Value = stocks[i].fluctuation;
                dataGrid.Rows[i].Cells[3].Value = stocks[i].netChange;
                dataGrid.Rows[i].Cells[4].Value = stocks[i].netChangeTrading;
            }
            Time starttime = main.conditionProcess.onPlayConditions[index].startTime;
            Time endtime = main.conditionProcess.onPlayConditions[index].endTime;
            ((CheckBox)this.Controls["flowLayoutPanel1"].Controls["panel" + (index + 1)].Controls["checkBox" + (index + 1)]).Checked = main.conditionProcess.onPlayConditions[index].isPlay;
            ((Label)this.Controls["flowLayoutPanel1"].Controls["panel" + (index + 1)].Controls["starttime" + (index + 1)]).Text = "매수시작시각 : " + starttime.hour + "시 " + starttime.min + "분 " + starttime.sec + "초";
            ((Label)this.Controls["flowLayoutPanel1"].Controls["panel" + (index + 1)].Controls["endtime" + (index + 1)]).Text = "매수종료시각 : " + endtime.hour + "시 " + endtime.min + "분 " + endtime.sec + "초";
            ((Label)this.Controls["flowLayoutPanel1"].Controls["panel" + (index + 1)].Controls["buyMoney" + (index + 1)]).Text = "매수금액 : " + (main.conditionProcess.onPlayConditions[index].buyMoney / 10000).ToString() + "만원";
            ((Label)this.Controls["flowLayoutPanel1"].Controls["panel" + (index + 1)].Controls["maxbuycount" + (index + 1)]).Text = "최대매수종목수 : " + main.conditionProcess.onPlayConditions[index].maxBuyCount.ToString();
            ((Label)this.Controls["flowLayoutPanel1"].Controls["panel" + (index + 1)].Controls["minprice" + (index + 1)]).Text = "최소값 : " + main.conditionProcess.onPlayConditions[index].minValue.ToString();
            ((ComboBox)this.Controls["flowLayoutPanel1"].Controls["panel" + (index + 1)].Controls["comboBox" + (index + 6)]).SelectedIndex = main.conditionProcess.onPlayConditions[index].buyType;
            ((ComboBox)this.Controls["flowLayoutPanel1"].Controls["panel" + (index + 1)].Controls["comboBox" + (index + 11)]).SelectedIndex = main.conditionProcess.onPlayConditions[index].addConditionIndex;
            ((Label)this.Controls["flowLayoutPanel1"].Controls["panel" + (index + 1)].Controls["BuyedCount" + (index + 1)]).Text = "매수된종목 : " + main.conditionProcess.onPlayConditions[index].buyStocks.Count;
            if(main.conditionProcess.onPlayConditions[index].buyStocks.Count >= main.conditionProcess.onPlayConditions[index].maxBuyCount)
            {
                ((Label)this.Controls["flowLayoutPanel1"].Controls["panel" + (index + 1)].Controls["BuyedCount" + (index + 1)]).ForeColor = Color.Red;
            }
            else
            {
                ((Label)this.Controls["flowLayoutPanel1"].Controls["panel" + (index + 1)].Controls["BuyedCount" + (index + 1)]).ForeColor = Color.Blue;
            }
        }

        public void ReloadPanel()
        {
            for (int i = 0; i < 5; i++)
                ReloadPanel(i);
        }

        public void ReloadLinks()
        {
            comboBox16.Items.Clear();
            comboBox17.Items.Clear();
            comboBox18.Items.Clear();
            comboBox19.Items.Clear();
            comboBox20.Items.Clear();

            string[] conditions = new string[main.conditionProcess.conditions.Count + 1];
            conditions[0] = "선택안함";

            for (int i = 0; i < main.conditionProcess.conditions.Count; i++)
            {
                if (main.conditionProcess.conditions[i] != null)
                    conditions[i + 1] = main.conditionProcess.conditions[i].name;
                else
                    conditions[i + 1] = "-----";
            }

            comboBox16.Items.AddRange(conditions);
            comboBox17.Items.AddRange(conditions);
            comboBox18.Items.AddRange(conditions);
            comboBox19.Items.AddRange(conditions);
            comboBox20.Items.AddRange(conditions);

            if (main.conditionProcess.onPlayConditions[0] != null)
                comboBox16.SelectedIndex = main.conditionProcess.conditions.FindIndex(a=>a.name == main.conditionProcess.onPlayConditions[0].linked) + 1;
            else
                comboBox16.SelectedIndex = 0;

            if (main.conditionProcess.onPlayConditions[1] != null)
                comboBox17.SelectedIndex = main.conditionProcess.conditions.FindIndex(a => a.name == main.conditionProcess.onPlayConditions[1].linked) + 1;
            else
                comboBox17.SelectedIndex = 0;

            if (main.conditionProcess.onPlayConditions[2] != null)
                comboBox18.SelectedIndex = main.conditionProcess.conditions.FindIndex(a => a.name == main.conditionProcess.onPlayConditions[2].linked) + 1;
            else
                comboBox18.SelectedIndex = 0;

            if (main.conditionProcess.onPlayConditions[3] != null)
                comboBox19.SelectedIndex = main.conditionProcess.conditions.FindIndex(a => a.name == main.conditionProcess.onPlayConditions[3].linked) + 1;
            else
                comboBox19.SelectedIndex = 0;

            if (main.conditionProcess.onPlayConditions[4] != null)
                comboBox20.SelectedIndex = main.conditionProcess.conditions.FindIndex(a => a.name == main.conditionProcess.onPlayConditions[4].linked) + 1;
            else
                comboBox20.SelectedIndex = 0;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                if (main.conditionProcess.onPlayConditions[0] != null)
                {
                    main.conditionProcess.onPlayConditions[0].StopCondition();
                    int index = 0;
                    if (main.conditionProcess.onPlayConditions[index].linked != null)
                    {
                        main.conditionProcess.conditions.Find(a => a.name == main.conditionProcess.onPlayConditions[index].linked).linked = null;
                        main.conditionProcess.onPlayConditions[index].linked = null;
                        ReloadLinks();
                    }
                }
                main.conditionProcess.onPlayConditions[0] = null;
                ReloadPanel(0);
                return;
            }
            if (comboBox2.SelectedIndex == comboBox1.SelectedIndex || 
                comboBox3.SelectedIndex == comboBox1.SelectedIndex || 
                comboBox4.SelectedIndex == comboBox1.SelectedIndex || 
                comboBox5.SelectedIndex == comboBox1.SelectedIndex)
            {
                comboBox1.SelectedIndex = 0;
                return;
            }
            if (main.conditionProcess.onPlayConditions[0] != null)
                if (main.conditionProcess.onPlayConditions[0].name == (string)comboBox1.SelectedItem)
                    return;
                else
                if (main.conditionProcess.onPlayConditions[0] != null)
                {
                    main.conditionProcess.onPlayConditions[0].StopCondition();
                    int index = 0;
                    if (main.conditionProcess.onPlayConditions[index].linked != null)
                    {
                        main.conditionProcess.conditions.Find(a => a.name == main.conditionProcess.onPlayConditions[index].linked).linked = null;
                        main.conditionProcess.onPlayConditions[index].linked = null;
                        ReloadLinks();
                    }
                }

            main.conditionProcess.onPlayConditions[0] = main.conditionProcess.FindCondition((string)comboBox1.SelectedItem);
            main.conditionProcess.onPlayConditions[0].isPlay = false;
            main.conditionProcess.onPlayConditions[0].SendCondition();
            ReloadPanel(0); 
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex == 0)
            {
                if (main.conditionProcess.onPlayConditions[1] != null)
                {
                    main.conditionProcess.onPlayConditions[1].StopCondition();

                    int index = 1;
                    if (main.conditionProcess.onPlayConditions[index].linked != null)
                    {
                        main.conditionProcess.conditions.Find(a => a.name == main.conditionProcess.onPlayConditions[index].linked).linked = null;
                        main.conditionProcess.onPlayConditions[index].linked = null;
                        ReloadLinks();
                    }
                }
                main.conditionProcess.onPlayConditions[1] = null;
                ReloadPanel(1);
                return;
            }
            if (comboBox1.SelectedIndex == comboBox2.SelectedIndex ||
                comboBox3.SelectedIndex == comboBox2.SelectedIndex ||
                comboBox4.SelectedIndex == comboBox2.SelectedIndex ||
                comboBox5.SelectedIndex == comboBox2.SelectedIndex)
            {
                comboBox2.SelectedIndex = 0;
                return;
            }
            if (main.conditionProcess.onPlayConditions[1] != null)
                if (main.conditionProcess.onPlayConditions[1].name == (string)comboBox2.SelectedItem)
                    return;
                else
                if (main.conditionProcess.onPlayConditions[1] != null)
                {
                    main.conditionProcess.onPlayConditions[1].StopCondition();
                    int index = 1;
                    if (main.conditionProcess.onPlayConditions[index].linked != null)
                    {
                        main.conditionProcess.conditions.Find(a => a.name == main.conditionProcess.onPlayConditions[index].linked).linked = null;
                        main.conditionProcess.onPlayConditions[index].linked = null;
                        ReloadLinks();
                    }
                }

            main.conditionProcess.onPlayConditions[1] = main.conditionProcess.FindCondition((string)comboBox2.SelectedItem);
            main.conditionProcess.onPlayConditions[1].isPlay = false;
            main.conditionProcess.onPlayConditions[1].SendCondition();
            ReloadPanel(1);
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox3.SelectedIndex == 0)
            {
                if (main.conditionProcess.onPlayConditions[2] != null)
                {
                    main.conditionProcess.onPlayConditions[2].StopCondition();
                    int index = 2;
                    if (main.conditionProcess.onPlayConditions[index].linked != null)
                    {
                        main.conditionProcess.conditions.Find(a => a.name == main.conditionProcess.onPlayConditions[index].linked).linked = null;
                        main.conditionProcess.onPlayConditions[index].linked = null;
                        ReloadLinks();
                    }
                }
                main.conditionProcess.onPlayConditions[2] = null;
                ReloadPanel(2);
                return;
            }
            if (comboBox1.SelectedIndex == comboBox3.SelectedIndex ||
                comboBox2.SelectedIndex == comboBox3.SelectedIndex ||
                comboBox4.SelectedIndex == comboBox3.SelectedIndex ||
                comboBox5.SelectedIndex == comboBox3.SelectedIndex)
            {
                comboBox3.SelectedIndex = 0;
                return;
            }

            if (main.conditionProcess.onPlayConditions[2] != null)
                if (main.conditionProcess.onPlayConditions[2].name == (string)comboBox3.SelectedItem)
                    return;
                else
                if (main.conditionProcess.onPlayConditions[2] != null)
                {
                    main.conditionProcess.onPlayConditions[2].StopCondition();
                    int index = 2;
                    if (main.conditionProcess.onPlayConditions[index].linked != null)
                    {
                        main.conditionProcess.conditions.Find(a => a.name == main.conditionProcess.onPlayConditions[index].linked).linked = null;
                        main.conditionProcess.onPlayConditions[index].linked = null;
                        ReloadLinks();
                    }
                }

            main.conditionProcess.onPlayConditions[2] = main.conditionProcess.FindCondition((string)comboBox3.SelectedItem);
            main.conditionProcess.onPlayConditions[2].isPlay = false;
            main.conditionProcess.onPlayConditions[2].SendCondition();
            ReloadPanel(2);
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox4.SelectedIndex == 0)
            {
                if (main.conditionProcess.onPlayConditions[3] != null)
                {
                    main.conditionProcess.onPlayConditions[3].StopCondition();
                    int index = 3;
                    if (main.conditionProcess.onPlayConditions[index].linked != null)
                    {
                        main.conditionProcess.conditions.Find(a => a.name == main.conditionProcess.onPlayConditions[index].linked).linked = null;
                        main.conditionProcess.onPlayConditions[index].linked = null;
                        ReloadLinks();
                    }
                }
                main.conditionProcess.onPlayConditions[3] = null;
                ReloadPanel(3);
                return;
            }
            if (comboBox1.SelectedIndex == comboBox4.SelectedIndex ||
                comboBox2.SelectedIndex == comboBox4.SelectedIndex ||
                comboBox3.SelectedIndex == comboBox4.SelectedIndex ||
                comboBox5.SelectedIndex == comboBox4.SelectedIndex)
            {
                comboBox4.SelectedIndex = 0;
                return;
            }
            if (main.conditionProcess.onPlayConditions[3] != null)
                if (main.conditionProcess.onPlayConditions[3].name == (string)comboBox4.SelectedItem)
                    return;
                else
                if (main.conditionProcess.onPlayConditions[3] != null)
                {
                    main.conditionProcess.onPlayConditions[3].StopCondition();
                    int index = 3;
                    if (main.conditionProcess.onPlayConditions[index].linked != null)
                    {
                        main.conditionProcess.conditions.Find(a => a.name == main.conditionProcess.onPlayConditions[index].linked).linked = null;
                        main.conditionProcess.onPlayConditions[index].linked = null;
                        ReloadLinks();
                    }
                }

            main.conditionProcess.onPlayConditions[3] = main.conditionProcess.FindCondition((string)comboBox4.SelectedItem);
            main.conditionProcess.onPlayConditions[3].isPlay = false;
            main.conditionProcess.onPlayConditions[3].SendCondition();
            ReloadPanel(3);
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox5.SelectedIndex == 0)
            {
                if (main.conditionProcess.onPlayConditions[4] != null)
                {
                    main.conditionProcess.onPlayConditions[4].StopCondition();
                    int index = 4;
                    if (main.conditionProcess.onPlayConditions[index].linked != null)
                    {
                        main.conditionProcess.conditions.Find(a => a.name == main.conditionProcess.onPlayConditions[index].linked).linked = null;
                        main.conditionProcess.onPlayConditions[index].linked = null;
                        ReloadLinks();
                    }
                }
                main.conditionProcess.onPlayConditions[4] = null;
                ReloadPanel(4);
                return;
            }
            if (comboBox1.SelectedIndex == comboBox5.SelectedIndex ||
                comboBox2.SelectedIndex == comboBox5.SelectedIndex ||
                comboBox3.SelectedIndex == comboBox5.SelectedIndex ||
                comboBox4.SelectedIndex == comboBox5.SelectedIndex)
            {
                comboBox5.SelectedIndex = 0;
                return;
            }
            if (main.conditionProcess.onPlayConditions[4] != null)
            {
                if (main.conditionProcess.onPlayConditions[4].name == (string)comboBox5.SelectedItem)
                    return;
                else
                if (main.conditionProcess.onPlayConditions[4] != null)
                {
                    main.conditionProcess.onPlayConditions[4].StopCondition();
                    int index = 4;
                    if (main.conditionProcess.onPlayConditions[index].linked != null)
                    {
                        main.conditionProcess.conditions.Find(a => a.name == main.conditionProcess.onPlayConditions[index].linked).linked = null;
                        main.conditionProcess.onPlayConditions[index].linked = null;
                        ReloadLinks();
                    }
                }
            }

            main.conditionProcess.onPlayConditions[4] = main.conditionProcess.FindCondition((string)comboBox5.SelectedItem);
            main.conditionProcess.onPlayConditions[4].isPlay = false;
            main.conditionProcess.onPlayConditions[4].SendCondition();
            ReloadPanel(4);
        }

        private void label1_Click(object sender, EventArgs e)
        {
            int index = 0;
            if (main.conditionProcess.onPlayConditions[index] == null)
                return;
            Action<string> action = (a) =>
            {
                this.ReloadPanel(index);
            };
            TimeSettingWindow timeSetting = new TimeSettingWindow(main.conditionProcess.onPlayConditions[index].startTime, action);
            timeSetting.ShowDialog();
        }

        private void label10_Click(object sender, EventArgs e)
        {
            int index = 1;
            if (main.conditionProcess.onPlayConditions[index] == null)
                return;
            Action<string> action = (a) =>
            {
                this.ReloadPanel(index);
            };
            TimeSettingWindow timeSetting = new TimeSettingWindow(main.conditionProcess.onPlayConditions[index].startTime, action);
            timeSetting.ShowDialog();
        }

        private void label17_Click(object sender, EventArgs e)
        {
            int index = 2;
            if (main.conditionProcess.onPlayConditions[index] == null)
                return;
            Action<string> action = (a) =>
            {
                this.ReloadPanel(index);
            };
            TimeSettingWindow timeSetting = new TimeSettingWindow(main.conditionProcess.onPlayConditions[index].startTime, action);
            timeSetting.ShowDialog();
        }

        private void label24_Click(object sender, EventArgs e)
        {
            int index = 3;
            if (main.conditionProcess.onPlayConditions[index] == null)
                return;
            Action<string> action = (a) =>
            {
                this.ReloadPanel(index);
            };
            TimeSettingWindow timeSetting = new TimeSettingWindow(main.conditionProcess.onPlayConditions[index].startTime, action);
            timeSetting.ShowDialog();
        }

        private void label31_Click(object sender, EventArgs e)
        {
            int index = 4;
            if (main.conditionProcess.onPlayConditions[index] == null)
                return;
            Action<string> action = (a) =>
            {
                this.ReloadPanel(index);
            };
            TimeSettingWindow timeSetting = new TimeSettingWindow(main.conditionProcess.onPlayConditions[index].startTime, action);
            timeSetting.ShowDialog();
        }

        private void endtime1_Click(object sender, EventArgs e)
        {
            int index = 0;
            if (main.conditionProcess.onPlayConditions[index] == null)
                return;
            Action<string> action = (a) =>
            {
                this.ReloadPanel(index);
            };
            TimeSettingWindow timeSetting = new TimeSettingWindow(main.conditionProcess.onPlayConditions[index].endTime, action);
            timeSetting.ShowDialog();
        }

        private void endtime2_Click(object sender, EventArgs e)
        {
            int index = 1;
            if (main.conditionProcess.onPlayConditions[index] == null)
                return;
            Action<string> action = (a) =>
            {
                this.ReloadPanel(index);
            };
            TimeSettingWindow timeSetting = new TimeSettingWindow(main.conditionProcess.onPlayConditions[index].endTime, action);
            timeSetting.ShowDialog();
        }

        private void label19_Click(object sender, EventArgs e)
        {
            int index = 2;
            if (main.conditionProcess.onPlayConditions[index] == null)
                return;
            Action<string> action = (a) =>
            {
                this.ReloadPanel(index);
            };
            TimeSettingWindow timeSetting = new TimeSettingWindow(main.conditionProcess.onPlayConditions[index].endTime, action);
            timeSetting.ShowDialog();
        }

        private void endtime4_Click(object sender, EventArgs e)
        {
            int index = 3;
            if (main.conditionProcess.onPlayConditions[index] == null)
                return;
            Action<string> action = (a) =>
            {
                this.ReloadPanel(index);
            };
            TimeSettingWindow timeSetting = new TimeSettingWindow(main.conditionProcess.onPlayConditions[index].endTime, action);
            timeSetting.ShowDialog();
        }

        private void endtime5_Click(object sender, EventArgs e)
        {
            int index = 4;
            if (main.conditionProcess.onPlayConditions[index] == null)
                return;
            Action<string> action = (a) =>
            {
                this.ReloadPanel(index);
            };
            TimeSettingWindow timeSetting = new TimeSettingWindow(main.conditionProcess.onPlayConditions[index].endTime, action);
            timeSetting.ShowDialog();
        }

        private void buyMoney1_Click(object sender, EventArgs e)
        {
            int index = 0;
            if (main.conditionProcess.onPlayConditions[index] == null)
                return;
            Action<int> action = (a) =>
            {
                main.conditionProcess.onPlayConditions[index].buyMoney = a * 10000;
                this.ReloadPanel(index);
            };
            main.CallValueWindowInt(main.conditionProcess.onPlayConditions[index].name + " : 매수금액 설정", action);
        }

        private void buyMoney2_Click(object sender, EventArgs e)
        {
            int index = 1;
            if (main.conditionProcess.onPlayConditions[index] == null)
                return;
            Action<int> action = (a) =>
            {
                main.conditionProcess.onPlayConditions[index].buyMoney = a * 10000;
                this.ReloadPanel(index);
            };
            main.CallValueWindowInt(main.conditionProcess.onPlayConditions[index].name + " : 매수금액 설정", action);
        }

        private void buyMoney3_Click(object sender, EventArgs e)
        {
            int index = 2;
            if (main.conditionProcess.onPlayConditions[index] == null)
                return;
            Action<int> action = (a) =>
            {
                main.conditionProcess.onPlayConditions[index].buyMoney = a * 10000;
                this.ReloadPanel(index);
            };
            main.CallValueWindowInt(main.conditionProcess.onPlayConditions[index].name + " : 매수금액 설정", action);
        }

        private void buyMoney4_Click(object sender, EventArgs e)
        {
            int index = 3;
            if (main.conditionProcess.onPlayConditions[index] == null)
                return;
            Action<int> action = (a) =>
            {
                main.conditionProcess.onPlayConditions[index].buyMoney = a * 10000;
                this.ReloadPanel(index);
            };
            main.CallValueWindowInt(main.conditionProcess.onPlayConditions[index].name + " : 매수금액 설정", action);
        }

        private void buyMoney5_Click(object sender, EventArgs e)
        {
            int index = 4;
            if (main.conditionProcess.onPlayConditions[index] == null)
                return;
            Action<int> action = (a) =>
            {
                main.conditionProcess.onPlayConditions[index].buyMoney = a * 10000;
                this.ReloadPanel(index);
            };
            main.CallValueWindowInt(main.conditionProcess.onPlayConditions[index].name + " : 매수금액 설정", action);
        }

        private void maxbuycount1_Click(object sender, EventArgs e)
        {
            int index = 0;
            if (main.conditionProcess.onPlayConditions[index] == null)
                return;
            Action<int> action = (a) =>
            {
                main.conditionProcess.onPlayConditions[index].maxBuyCount = a;
                this.ReloadPanel(index);
            };
            main.CallValueWindowInt(main.conditionProcess.onPlayConditions[index].name + " : 매수종목수 설정", action);
        }

        private void maxbuycount2_Click(object sender, EventArgs e)
        {
            int index = 1;
            if (main.conditionProcess.onPlayConditions[index] == null)
                return;
            Action<int> action = (a) =>
            {
                main.conditionProcess.onPlayConditions[index].maxBuyCount = a;
                this.ReloadPanel(index);
            };
            main.CallValueWindowInt(main.conditionProcess.onPlayConditions[index].name + " : 매수종목수 설정", action);
        }

        private void maxbuycount3_Click(object sender, EventArgs e)
        {
            int index = 2;
            if (main.conditionProcess.onPlayConditions[index] == null)
                return;
            Action<int> action = (a) =>
            {
                main.conditionProcess.onPlayConditions[index].maxBuyCount = a;
                this.ReloadPanel(index);
            };
            main.CallValueWindowInt(main.conditionProcess.onPlayConditions[index].name + " : 매수종목수 설정", action);
        }

        private void maxbuycount4_Click(object sender, EventArgs e)
        {
            int index = 3;
            if (main.conditionProcess.onPlayConditions[index] == null)
                return;
            Action<int> action = (a) =>
            {
                main.conditionProcess.onPlayConditions[index].maxBuyCount = a;
                this.ReloadPanel(index);
            };
            main.CallValueWindowInt(main.conditionProcess.onPlayConditions[index].name + " : 매수종목수 설정", action);
        }

        private void maxbuycount5_Click(object sender, EventArgs e)
        {
            int index = 4;
            if (main.conditionProcess.onPlayConditions[index] == null)
                return;
            Action<int> action = (a) =>
            {
                main.conditionProcess.onPlayConditions[index].maxBuyCount = a;
                this.ReloadPanel(index);
            };
            main.CallValueWindowInt(main.conditionProcess.onPlayConditions[index].name + " : 매수종목수 설정", action);
        }

        private void minprice1_Click(object sender, EventArgs e)
        {
            int index = 0;
            if (main.conditionProcess.onPlayConditions[index] == null)
                return;
            Action<int> action = (a) =>
            {
                main.conditionProcess.onPlayConditions[index].minValue = a;
                this.ReloadPanel(index);
            };
            main.CallValueWindowInt(main.conditionProcess.onPlayConditions[index].name + " : 최소값 설정", action);
        }

        private void minprice2_Click(object sender, EventArgs e)
        {
            int index = 1;
            if (main.conditionProcess.onPlayConditions[index] == null)
                return;
            Action<int> action = (a) =>
            {
                main.conditionProcess.onPlayConditions[index].minValue = a;
                this.ReloadPanel(index);
            };
            main.CallValueWindowInt(main.conditionProcess.onPlayConditions[index].name + " : 최소값 설정", action);
        }

        private void minprice3_Click(object sender, EventArgs e)
        {
            int index = 2;
            if (main.conditionProcess.onPlayConditions[index] == null)
                return;
            Action<int> action = (a) =>
            {
                main.conditionProcess.onPlayConditions[index].minValue = a;
                this.ReloadPanel(index);
            };
            main.CallValueWindowInt(main.conditionProcess.onPlayConditions[index].name + " : 최소값 설정", action);
        }

        private void minprice4_Click(object sender, EventArgs e)
        {
            int index = 3;
            if (main.conditionProcess.onPlayConditions[index] == null)
                return;
            Action<int> action = (a) =>
            {
                main.conditionProcess.onPlayConditions[index].minValue = a;
                this.ReloadPanel(index);
            };
            main.CallValueWindowInt(main.conditionProcess.onPlayConditions[index].name + " : 최소값 설정", action);
        }

        private void minprice5_Click(object sender, EventArgs e)
        {
            int index = 4;
            if (main.conditionProcess.onPlayConditions[index] == null)
                return;
            Action<int> action = (a) =>
            {
                main.conditionProcess.onPlayConditions[index].minValue = a;
                this.ReloadPanel(index);
            };
            main.CallValueWindowInt(main.conditionProcess.onPlayConditions[index].name + " : 최소값 설정", action);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            int index = 0;
            if (main.conditionProcess.onPlayConditions[index] == null)
                return;
            main.conditionProcess.onPlayConditions[index].isPlay = ((CheckBox)sender).Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            int index = 1;
            if (main.conditionProcess.onPlayConditions[index] == null)
                return;
            main.conditionProcess.onPlayConditions[index].isPlay = ((CheckBox)sender).Checked;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            int index = 2;
            if (main.conditionProcess.onPlayConditions[index] == null)
                return;
            main.conditionProcess.onPlayConditions[index].isPlay = ((CheckBox)sender).Checked;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            int index = 3;
            if (main.conditionProcess.onPlayConditions[index] == null)
                return;
            main.conditionProcess.onPlayConditions[index].isPlay = ((CheckBox)sender).Checked;
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            int index = 4;
            if (main.conditionProcess.onPlayConditions[index] == null)
                return;
            main.conditionProcess.onPlayConditions[index].isPlay = ((CheckBox)sender).Checked;
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = 0;

            if (main.conditionProcess.onPlayConditions[index] == null)
                return;
            main.conditionProcess.onPlayConditions[index].buyType = ((ComboBox)sender).SelectedIndex;

        }

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = 1;

            if (main.conditionProcess.onPlayConditions[index] == null)
                return;
            main.conditionProcess.onPlayConditions[index].buyType = ((ComboBox)sender).SelectedIndex;

        }

        private void comboBox8_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = 2;

            if (main.conditionProcess.onPlayConditions[index] == null)
                return;
            main.conditionProcess.onPlayConditions[index].buyType = ((ComboBox)sender).SelectedIndex;

        }

        private void comboBox9_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = 3;

            if (main.conditionProcess.onPlayConditions[index] == null)
                return;
            main.conditionProcess.onPlayConditions[index].buyType = ((ComboBox)sender).SelectedIndex;

        }

        private void comboBox10_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = 4;

            if (main.conditionProcess.onPlayConditions[index] == null)
                return;
            main.conditionProcess.onPlayConditions[index].buyType = ((ComboBox)sender).SelectedIndex;

        }

        void TextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }

        private void playing_cb_CheckedChanged(object sender, EventArgs e)
        {
            main.conditionProcess.is_playing = ((CheckBox)sender).Checked;
        }

        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {
            main.conditionProcess.is_sell = ((CheckBox)sender).Checked;
        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            main.conditionProcess.is_buy = ((CheckBox)sender).Checked;
        }

        private void checkBox15_CheckedChanged(object sender, EventArgs e)
        {
            main.conditionProcess.is_highbuy = ((CheckBox)sender).Checked;
        }

        private void pricechangesound_CheckedChanged(object sender, EventArgs e)
        {
            main.conditionProcess.is_price_change_sound = ((CheckBox)sender).Checked;
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            main.conditionProcess.end_clear = ((CheckBox)sender).Checked;
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            main.conditionProcess.is_highmedo = ((CheckBox)sender).Checked;
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            main.conditionProcess.is_cut = ((CheckBox)sender).Checked;
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            main.conditionProcess.is_rebuy = ((CheckBox)sender).Checked;
        }

        private void checkBox13_CheckedChanged(object sender, EventArgs e)
        {
            main.conditionProcess.haveEffect = ((CheckBox)sender).Checked;
        }

        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            main.conditionProcess.is_endLimit = ((CheckBox)sender).Checked;
        }

        private void label38_Click(object sender, EventArgs e)
        {
            Action<float, bool> action = (a, b) =>
            {
                if (b)
                {
                    if (a <= 100)
                    {
                        main.conditionProcess.highMedo_perval = a;
                        main.conditionProcess.is_highmedo_per = true;
                        label38.Text = "고매: " + main.conditionProcess.highMedo_perval.ToString("##0.0") + "%";
                    }
                    else
                    {
                        main.conditionProcess.highMedo_price = (int)a;
                        main.conditionProcess.is_highmedo_per = false;
                        label38.Text = "고매: " + String.Format("{0:#,###}", main.conditionProcess.highMedo_price) + "원";
                    }
                }
                else
                {
                }
            };
            main.CallValueWindowFloat("고점매도 기본값 설정", action);
        }

        private void targetprice_text_Click(object sender, EventArgs e)
        {
            Action<int> action = (a) =>
            {
                main.conditionProcess.targetPrice = a * 10000;
                targetprice_text.Text = "목표가: " + String.Format("{0:#,###}", a) + "만원";
            };
            main.CallValueWindowInt("목표가 기본값 설정", action);
        }

        private void label41_Click(object sender, EventArgs e)
        {
            Action<int> action = (a) =>
            {
                main.conditionProcess.remainCountVal = a;
                label41.Text = "남길수: " + a.ToString("##0.0") + "개";
            };
            main.CallValueWindowInt("남길수 기본값 설정", action);
        }

        private void label37_Click(object sender, EventArgs e)
        {
            Action<float, bool> action = (a, b) =>
            {
                if (b)
                {
                    label37.Text = a.ToString("##0.0") + "%";
                    main.conditionProcess.cut_price = a;
                }
                else
                {
                }
            };
            main.CallValueWindowFloat("손절 기본값 설정", action);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form6 form6 = new Form6(main, main.conditionProcess);
            form6.Name = "기본 비중설정";
            form6.ShowDialog();
        }

        private void comboBox11_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = 0;

            if (main.conditionProcess.onPlayConditions[index] == null)
                return;
            main.conditionProcess.onPlayConditions[index].addConditionIndex = ((ComboBox)sender).SelectedIndex;
        }

        private void comboBox12_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = 1;

            if (main.conditionProcess.onPlayConditions[index] == null)
                return;
            main.conditionProcess.onPlayConditions[index].addConditionIndex = ((ComboBox)sender).SelectedIndex;
        }

        private void comboBox13_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = 2;

            if (main.conditionProcess.onPlayConditions[index] == null)
                return;
            main.conditionProcess.onPlayConditions[index].addConditionIndex = ((ComboBox)sender).SelectedIndex;
        }

        private void comboBox14_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = 3;

            if (main.conditionProcess.onPlayConditions[index] == null)
                return;
            main.conditionProcess.onPlayConditions[index].addConditionIndex = ((ComboBox)sender).SelectedIndex;
        }

        private void comboBox15_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = 4;

            if (main.conditionProcess.onPlayConditions[index] == null)
                return;
            main.conditionProcess.onPlayConditions[index].addConditionIndex = ((ComboBox)sender).SelectedIndex;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int index = 0;

            if (main.conditionProcess.onPlayConditions[index] == null)
                return;
            main.conditionProcess.onPlayConditions[index].buyStocks.Clear();
            ReloadPanel(index);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int index = 1;

            if (main.conditionProcess.onPlayConditions[index] == null)
                return;
            main.conditionProcess.onPlayConditions[index].buyStocks.Clear();
            ReloadPanel(index);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int index = 2;

            if (main.conditionProcess.onPlayConditions[index] == null)
                return;
            main.conditionProcess.onPlayConditions[index].buyStocks.Clear();
            ReloadPanel(index);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int index = 3;

            if (main.conditionProcess.onPlayConditions[index] == null)
                return;
            main.conditionProcess.onPlayConditions[index].buyStocks.Clear();
            ReloadPanel(index);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int index = 4;

            if (main.conditionProcess.onPlayConditions[index] == null)
                return;
            main.conditionProcess.onPlayConditions[index].buyStocks.Clear();
            ReloadPanel(index);
        }

        private void comboBox16_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = 0;



            if (main.conditionProcess.onPlayConditions[index] == null)
            {
                if (((ComboBox)sender).SelectedIndex != 0)
                {
                    ((ComboBox)sender).SelectedIndex = 0;
                    ReloadLinks();
                }
                return;
            }
            if (main.conditionProcess.onPlayConditions[index].linked == null && ((ComboBox)sender).SelectedIndex == 0)
            {
                return;
            }
            if (((ComboBox)sender).SelectedIndex != 0)
                if (main.conditionProcess.onPlayConditions[index].linked == main.conditionProcess.conditions[((ComboBox)sender).SelectedIndex - 1].name)
                {
                    return;
                }
            if (((ComboBox)sender).SelectedIndex == 0)
            {
                if (main.conditionProcess.onPlayConditions[index].linked != null)
                    main.conditionProcess.conditions.Find(a => a.name == main.conditionProcess.onPlayConditions[index].linked).linked = null;
                main.conditionProcess.onPlayConditions[index].linked = null;
                ReloadLinks();
                return;
            }
            if (main.conditionProcess.conditions[((ComboBox)sender).SelectedIndex - 1] == null)
            {
                if(main.conditionProcess.onPlayConditions[index].linked != null)
                    main.conditionProcess.conditions.Find(a => a.name == main.conditionProcess.onPlayConditions[index].linked).linked = null;
                main.conditionProcess.onPlayConditions[index].linked = null;
                return;
            }


            if (main.conditionProcess.conditions[((ComboBox)sender).SelectedIndex - 1].linked != null)
            {
                main.conditionProcess.conditions.Find(a => a.name == main.conditionProcess.conditions[((ComboBox)sender).SelectedIndex - 1].linked).linked = null;
            }
            if (main.conditionProcess.onPlayConditions[index].linked != null)
                main.conditionProcess.conditions.Find(a => a.name == main.conditionProcess.onPlayConditions[index].linked).linked = null;
            main.conditionProcess.onPlayConditions[index].linked = main.conditionProcess.conditions[((ComboBox)sender).SelectedIndex-1].name;
            main.conditionProcess.conditions[((ComboBox)sender).SelectedIndex-1].linked = main.conditionProcess.onPlayConditions[index].name;

            ReloadLinks();
        }

        private void comboBox17_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = 1;

            if (main.conditionProcess.onPlayConditions[index] == null)
            {
                if (((ComboBox)sender).SelectedIndex != 0)
                {
                    ((ComboBox)sender).SelectedIndex = 0;
                    ReloadLinks();
                }
                return;
            }
            if (main.conditionProcess.onPlayConditions[index].linked == null && ((ComboBox)sender).SelectedIndex == 0)
            {
                return;
            }
            if (((ComboBox)sender).SelectedIndex != 0)
                if (main.conditionProcess.onPlayConditions[index].linked == main.conditionProcess.conditions[((ComboBox)sender).SelectedIndex - 1].name)
            {
                return;
            }
            if (((ComboBox)sender).SelectedIndex == 0)
            {
                main.conditionProcess.conditions.Find(a => a.name == main.conditionProcess.onPlayConditions[index].linked).linked = null;
                main.conditionProcess.onPlayConditions[index].linked = null;
                ReloadLinks();
                return;
            }
            if (main.conditionProcess.conditions[((ComboBox)sender).SelectedIndex - 1] == null)
            {
                if (main.conditionProcess.onPlayConditions[index].linked != null)
                    main.conditionProcess.conditions.Find(a => a.name == main.conditionProcess.onPlayConditions[index].linked).linked = null;
                main.conditionProcess.onPlayConditions[index].linked = null;
                return;
            }


            if (main.conditionProcess.conditions[((ComboBox)sender).SelectedIndex - 1].linked != null)
            {
                main.conditionProcess.conditions.Find(a => a.name == main.conditionProcess.conditions[((ComboBox)sender).SelectedIndex - 1].linked).linked = null;
            }
            if (main.conditionProcess.onPlayConditions[index].linked != null)
                main.conditionProcess.conditions.Find(a => a.name == main.conditionProcess.onPlayConditions[index].linked).linked = null;
            main.conditionProcess.onPlayConditions[index].linked = main.conditionProcess.conditions[((ComboBox)sender).SelectedIndex-1].name;
            main.conditionProcess.conditions[((ComboBox)sender).SelectedIndex - 1].linked = main.conditionProcess.onPlayConditions[index].name;

            ReloadLinks();

        }

        private void comboBox18_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = 2;

            if (main.conditionProcess.onPlayConditions[index] == null)
            {
                if (((ComboBox)sender).SelectedIndex != 0)
                {
                    ((ComboBox)sender).SelectedIndex = 0;
                    ReloadLinks();
                }
                return;
            }
            if (main.conditionProcess.onPlayConditions[index].linked == null && ((ComboBox)sender).SelectedIndex == 0)
            {
                return;
            }
            if (((ComboBox)sender).SelectedIndex != 0)
                if (main.conditionProcess.onPlayConditions[index].linked == main.conditionProcess.conditions[((ComboBox)sender).SelectedIndex - 1].name)
            {
                return;
            }
            if (((ComboBox)sender).SelectedIndex == 0)
            {
                main.conditionProcess.conditions.Find(a => a.name == main.conditionProcess.onPlayConditions[index].linked).linked = null;
                main.conditionProcess.onPlayConditions[index].linked = null;
                ReloadLinks();
                return;
            }
            if (main.conditionProcess.conditions[((ComboBox)sender).SelectedIndex - 1] == null)
            {
                if (main.conditionProcess.onPlayConditions[index].linked != null)
                    main.conditionProcess.conditions.Find(a => a.name == main.conditionProcess.onPlayConditions[index].linked).linked = null;
                main.conditionProcess.onPlayConditions[index].linked = null;
                return;
            }


            if (main.conditionProcess.conditions[((ComboBox)sender).SelectedIndex - 1].linked != null)
            {
                main.conditionProcess.conditions.Find(a => a.name == main.conditionProcess.conditions[((ComboBox)sender).SelectedIndex - 1].linked).linked = null;
            }
            if (main.conditionProcess.onPlayConditions[index].linked != null)
                main.conditionProcess.conditions.Find(a => a.name == main.conditionProcess.onPlayConditions[index].linked).linked = null;
            main.conditionProcess.onPlayConditions[index].linked = main.conditionProcess.conditions[((ComboBox)sender).SelectedIndex-1].name;
            main.conditionProcess.conditions[((ComboBox)sender).SelectedIndex - 1].linked = main.conditionProcess.onPlayConditions[index].name;

            ReloadLinks();

        }

        private void comboBox19_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = 3;

            if (main.conditionProcess.onPlayConditions[index] == null)
            {
                if (((ComboBox)sender).SelectedIndex != 0)
                {
                    ((ComboBox)sender).SelectedIndex = 0;
                    ReloadLinks();
                }
                return;
            }
            if (main.conditionProcess.onPlayConditions[index].linked == null && ((ComboBox)sender).SelectedIndex == 0)
            {
                return;
            }
            if (((ComboBox)sender).SelectedIndex != 0)
                if (main.conditionProcess.onPlayConditions[index].linked == main.conditionProcess.conditions[((ComboBox)sender).SelectedIndex - 1].name)
            {
                return;
            }
            if (((ComboBox)sender).SelectedIndex == 0)
            {
                main.conditionProcess.conditions.Find(a => a.name == main.conditionProcess.onPlayConditions[index].linked).linked = null;
                main.conditionProcess.onPlayConditions[index].linked = null;
                ReloadLinks();
                return;
            }
            if (main.conditionProcess.conditions[((ComboBox)sender).SelectedIndex - 1] == null)
            {
                if (main.conditionProcess.onPlayConditions[index].linked != null)
                    main.conditionProcess.conditions.Find(a => a.name == main.conditionProcess.onPlayConditions[index].linked).linked = null;
                main.conditionProcess.onPlayConditions[index].linked = null;
                return;
            }


            if (main.conditionProcess.conditions[((ComboBox)sender).SelectedIndex - 1].linked != null)
            {
                main.conditionProcess.conditions.Find(a => a.name == main.conditionProcess.conditions[((ComboBox)sender).SelectedIndex - 1].linked).linked = null;
            }
            if (main.conditionProcess.onPlayConditions[index].linked != null)
                main.conditionProcess.conditions.Find(a => a.name == main.conditionProcess.onPlayConditions[index].linked).linked = null;
            main.conditionProcess.onPlayConditions[index].linked = main.conditionProcess.conditions[((ComboBox)sender).SelectedIndex-1].name;
            main.conditionProcess.conditions[((ComboBox)sender).SelectedIndex - 1].linked = main.conditionProcess.onPlayConditions[index].name;

            ReloadLinks();

        }

        private void comboBox20_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = 4;

            if (main.conditionProcess.onPlayConditions[index] == null)
            {
                if (((ComboBox)sender).SelectedIndex != 0)
                {
                    ((ComboBox)sender).SelectedIndex = 0;
                    ReloadLinks();
                }
                return;
            }
            if (main.conditionProcess.onPlayConditions[index].linked == null && ((ComboBox)sender).SelectedIndex == 0)
            {
                return;
            }
            if (((ComboBox)sender).SelectedIndex != 0)
                if (main.conditionProcess.onPlayConditions[index].linked == main.conditionProcess.conditions[((ComboBox)sender).SelectedIndex - 1].name)
            {
                return;
            }
            if (((ComboBox)sender).SelectedIndex == 0)
            {
                main.conditionProcess.conditions.Find(a => a.name == main.conditionProcess.onPlayConditions[index].linked).linked = null;
                main.conditionProcess.onPlayConditions[index].linked = null;
                ReloadLinks();
                return;
            }
            if (main.conditionProcess.conditions[((ComboBox)sender).SelectedIndex - 1] == null)
            {
                if (main.conditionProcess.onPlayConditions[index].linked != null)
                    main.conditionProcess.conditions.Find(a => a.name == main.conditionProcess.onPlayConditions[index].linked).linked = null;
                main.conditionProcess.onPlayConditions[index].linked = null;
                return;
            }


            if (main.conditionProcess.conditions[((ComboBox)sender).SelectedIndex - 1].linked != null)
            {
                main.conditionProcess.conditions.Find(a => a.name == main.conditionProcess.conditions[((ComboBox)sender).SelectedIndex - 1].linked).linked = null;
            }
            if (main.conditionProcess.onPlayConditions[index].linked != null)
                main.conditionProcess.conditions.Find(a => a.name == main.conditionProcess.onPlayConditions[index].linked).linked = null;
            main.conditionProcess.onPlayConditions[index].linked = main.conditionProcess.conditions[((ComboBox)sender).SelectedIndex-1].name;
            main.conditionProcess.conditions[((ComboBox)sender).SelectedIndex - 1].linked = main.conditionProcess.onPlayConditions[index].name;

            ReloadLinks();

        }
    }
}
